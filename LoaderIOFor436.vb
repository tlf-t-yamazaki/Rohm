'===============================================================================
'   Description  : オートローダＩＯ処理(SL436R/SL436S用)
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc              'V4.4.0.0-0
Imports LaserFront.Trimmer.DllSystem                'V6.0.0.0�Q
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources                'V4.4.0.0-0

Module LoaderIOFor436
#Region "【変数定義】"
    '===============================================================================
    '   定数定義
    '===============================================================================
    '----- API定義 -----
    'Private Declare Function GetPrivateProfileInt Lib "kernel32" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer
    'Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long

    '-------------------------------------------------------------------------------
    '   ローダＩ／Ｏビット定義(SL436R用)
    '-------------------------------------------------------------------------------
    '----- 出力データ(トリマ  → ローダ) -----
    Public Const LOUT_REDY As UShort = &H1                              ' B0 : トリマ部レディ(0=Not Ready, 1=Ready) ※BITを反転して出力する
    Public Const LOUT_AUTO As UShort = &H2                              ' B1 : ローダモード切替え(0=手動, 1=自動(マガジンチェック))
    Public Const LOUT_STOP As UShort = &H4                              ' B2 : トリマ部停止中(0=動作中,1=停止中)
    Public Const LOUT_SUPLY As UShort = &H8                             ' B3 : 基板要求(0=基板要求無,1=基板要求)※連続運転開始
    Public Const LOUT_TRM_NG As UShort = &H10                           ' B4 : トリミングＮＧ  (0:正常, 1:ＮＧ)
    'Public Const LOUT_PTN_NG As UShort = &H20                          ' B5 : パターン認識ＮＧ(0:正常, 1:ＮＧ) ###070
    Public Const LOUT_INTLOK_DISABLE As UShort = &H20                   ' B5 : インターロック解除(0:インターロック中, 1:インターロック解除中(全部/一部)) ###070
    Public Const LOUT_REQ_COLECT As UShort = &H40                       ' B6 : 基板回収要求(0=要求無,1=要求有)
    '###148    Public Const LOUT_NO_ALM As UShort = &H80                ' B7 : トリマアラーム(0:アラーム, 1:正常)
    Public Const LOUT_PROC_CONTINUE As UShort = &H80                    ' B7 : 動作継続信号(0:なし, 1:継続実行)
    Public Const LOUT_ORG_BACK As UShort = &H100                        ' B8 : ローダ原点復帰要求(0=原点復帰未要求, 1=原点復帰要求)
    Public Const LOUT_STB As UShort = &H200                             ' B9 : 品種データ送信(STB)
    Public Const LOUT_NG_DISCHRAGE As UShort = &H400                    ' B10: ＮＧ基板排出要求(0=ＮＧ排出要求無, 1=ＮＧ排出要求)
    Public Const LOUT_DISCHRAGE As UShort = &H800                       ' B11: 供給位置決完了(0=完了でない, 1=完了)
    Public Const LOUT_STS_RUN As UShort = &H1000                        ' B12: トリマ運転中(0:一時停止, 1:運転中)
    Public Const LOUT_EMPTY_OPE As UShort = &H2000                      ' B13: 空運転中(SL436R用)
    Public Const LOUT_CYCL_STOP As UShort = &H2000                      ' B13: サイクル停止要求(0=要求無,1=要求)(SL436S用) V4.0.0.0�R
    Public Const LOUT_VACCUME As UShort = &H4000                        ' B14: 吸着(手動モード時有効)
    Public Const LOUT_CLAMP As UShort = &H8000                          ' B15: 載物台クランプ開閉(0=開, 1=閉)(手動モード時有効)

    '----- 入力データ(ローダ  → トリマ) -----
    Public Const LINP_READY As UShort = &H1                             ' B0 : ローダ部レディ(0=Not Ready, 1=Ready) 
    Public Const LINP_AUTO As UShort = &H2                              ' B1 : ローダモード切替え(0=手動, 1=自動)
    Public Const LINP_STOP As UShort = &H4                              ' B2 : ローダ部停止中(0=基板交換中, 1=停止中)
    Public Const LINP_TRM_START As UShort = &H8                         ' B3 : トリミングスタート要求(0=スタート非要求, 1=スタート要求) 
    '###148    Public Const LINP_RSV04 As UShort = &H10                 ' B4 : 予備
    Public Const LINP_NGTRAY_OUT_COMP As UShort = &H10                  ' B4 : NGトレイへの排出完了信号
    Public Const LINP_RSV05 As UShort = &H20                            ' B5 : 予備(SL436R用)
    Public Const LIN_CYCL_STOP As UShort = &H20                         ' B5 : サイクル停止応答(0=応答無,1=応答)(SL436S用) V4.0.0.0�R
    'V5.0.0.6�A    Public Const LINP_RSV06 As UShort = &H40                            ' B6 : 予備
    Public Const LINP_HST_MOVESUPPLY As Short = &H40S                    ' B6 : 供給位置移動指示 'V5.0.0.6�A
    Public Const LINP_NO_ALM_RESTART As UShort = &H80                   ' B7 : ローダ部正常(0=アラーム発生, 1=正常)
    Public Const LINP_ORG_BACK As UShort = &H100                        ' B8 : ローダ原点復帰完了(0=原点復帰未完了, 1=原点復帰完了)
    Public Const LINP_LOT_CHG As UShort = &H200                         ' B9 : ロット切換要求(0=ロット切換非要求, 1=ロット切換要求(満杯))
    Public Const LINP_END_MAGAZINE As UShort = &H400                    ' B10: マガジン終了(0=マガジン非終了, 1=マガジン終了)
    Public Const LINP_END_ALL_MAGAZINE As UShort = &H800                ' B11: 全マガジン終了(0=全マガジン非終了, 1=全マガジン終了)
    Public Const LINP_NG_FULL As UShort = &H1000                        ' B12: NG排出満杯(0=NG排出未満杯, 1=NG排出満杯(完了))
    Public Const LINP_DISCHRAGE As UShort = &H2000                      ' B13: 排出ピック完了(0=完了でない, 1=完了)
    Public Const LINP_2PIECES As UShort = &H4000                        ' B14: ２枚取り検出(0=２枚取り未検出, 1=２枚取り検出)　
    Public Const LINP_WBREAK As UShort = &H8000                         ' B15: 基板割れ検出(0=基板割れ未検出, 1=基板割れ検出)　

    '-------------------------------------------------------------------------------
    '   ローダアラーム状態／アラーム詳細定義(SL436R用)
    '-------------------------------------------------------------------------------
    '----- ローダアラーム状態 SL436R用(W110) -----
    Public Const LARM_ARM1 As UShort = &H100                            ' B8 : 軽故障(続行可能)※アラーム状態解除後スタート(W109.01)で続行
    Public Const LARM_ARM2 As UShort = &H200                            ' B9 : サイクル停止    ※リセット(W109.03)後原点復帰してアイドル状態
    Public Const LARM_ARM3 As UShort = &H400                            ' B10: 全停止異常      ※同上

    '----- ローダアラーム詳細 SL436R用(W115, W116) -----
    '                                                                   ' W115.00-W115.15(続行不可)
    Public Const LARM_MSG000 As UShort = &H1                            ' B00: 非常停止
    Public Const LARM_MSG001 As UShort = &H2                            ' B01: マガジン整合性アラーム
    Public Const LARM_MSG002 As UShort = &H4                            ' B02: 割れ欠け品発生
    Public Const LARM_MSG003 As UShort = &H8                            ' B03: ハンド１吸着アラーム
    Public Const LARM_MSG004 As UShort = &H10                           ' B04: ハンド２吸着アラーム
    Public Const LARM_MSG005 As UShort = &H20                           ' B05: 載物台吸着センサ異常
    Public Const LARM_MSG006 As UShort = &H40                           ' B06: 載物台吸着ミス
    Public Const LARM_MSG007 As UShort = &H80                           ' B07: ロボットアラーム
    Public Const LARM_MSG008 As UShort = &H100                          ' B08: 工程間監視アラーム
    Public Const LARM_MSG009 As UShort = &H200                          ' B09: エレベータ異常
    Public Const LARM_MSG010 As UShort = &H400                          ' B10: マガジン無し
    Public Const LARM_MSG011 As UShort = &H800                          ' B11: 原点復帰タイムアウト
    Public Const LARM_MSG012 As UShort = &H1000                         ' B12: 未使用
    Public Const LARM_MSG013 As UShort = &H2000                         ' B13: 未使用
    Public Const LARM_MSG014 As UShort = &H4000                         ' B14: 未使用
    Public Const LARM_MSG015 As UShort = &H8000                         ' B15: 未使用

    '                                                                   ' W116.00-W116.15(続行可)
    Public Const LARM_MSG016 As UShort = &H1                            ' B00: 旋回シリンダタイムアウト
    Public Const LARM_MSG017 As UShort = &H2                            ' B01: ハンド１シリンダタイムアウト
    Public Const LARM_MSG018 As UShort = &H4                            ' B02: ハンド２シリンダタイムアウト
    Public Const LARM_MSG019 As UShort = &H8                            ' B03: 供給ハンド吸着ミス
    Public Const LARM_MSG020 As UShort = &H10                           ' B04: 回収ハンド吸着ミス
    Public Const LARM_MSG021 As UShort = &H20                           ' B05: ＮＧ排出満杯
    Public Const LARM_MSG022 As UShort = &H40                           ' B06: 一時停止
    Public Const LARM_MSG023 As UShort = &H80                           ' B07: ドアオープン
    Public Const LARM_MSG024 As UShort = &H100                          ' B08: 未使用
    Public Const LARM_MSG025 As UShort = &H200                          ' B09: 未使用
    Public Const LARM_MSG026 As UShort = &H400                          ' B10: 未使用
    Public Const LARM_MSG027 As UShort = &H800                          ' B11: 未使用
    Public Const LARM_MSG028 As UShort = &H1000                         ' B12: 未使用
    Public Const LARM_MSG029 As UShort = &H2000                         ' B13: 未使用
    Public Const LARM_MSG030 As UShort = &H4000                         ' B14: 未使用
    Public Const LARM_MSG031 As UShort = &H8000                         ' B15: 未使用

    '----- V2.0.0.0�E↓ -----
    '-------------------------------------------------------------------------------
    '   ■ローダシリアル通信用オフセット定義(SL436S用)■
    '-------------------------------------------------------------------------------
    '----- ローダアラーム状態 -----
    Public Const LOFS_W110 As Long = 110                                ' ローダアラーム状態(W110.08-10)
    Public Const LOFS_W115 As Long = 115                                ' ローダアラーム詳細内容(W115.00-W115.15(続行不可))
    Public Const LOFS_W116 As Long = 116                                ' ローダアラーム詳細内容(W116.00-W116.15(続行可))

    '----- ローダアラーム詳細(W115) -----                                       
    Public Const LDFS_ARM_AIR As Long = &H2000                          ' B13: 正圧異常検出
    'V5.0.0.1-25↓
    Public Const LDFS_SUPPLY_MG_FULL As Long = &H4000                   ' B14: 供給マガジンフル
    Public Const LDFS_STORE_MG_FULL As Long = &H8000                    ' B15: 収納マガジンフル
    'V5.0.0.1-25↑

    '----- ローダ特別操作デバイス -----
    Public Const LOFS_W109 As Long = 109                                ' ローダ特別操作デバイス(W109.00-W109.15)
    Public Const LDDV_ARM_START As Long = &H2                           ' B01: 一時停止状態(続行可能エラー等)からスタートする
    Public Const LDDV_ARM_RESET As Long = &H8                           ' B03: アラームリセット
    Public Const LDDV_BUZER_OFF As Long = &H10                          ' B04: ブザーOFF
    Public Const LDDV_CLR_PRDUCT As Long = &H80                         ' B07: 生産数クリア
    Public Const LDDV_ALM_DSP As Long = &H400                           ' B10: アラームメッセージ表示中(ON:表示中,OFF:表示中でない)

    '----- 初期設定状態 -----                                 
    Public Const LOFS_H00 As Long = 0                                   ' (H0.00-H0.15)
    Public Const LHST_MAGAZINE As Long = &H1                            ' B00: ON=4マガジン, OFF=2マガジン

    '----- 物理入力状態 -----
    Public Const LOFS_W40S As Long = 40                                 ' (W40.00-W40.15)
    Public Const LDABS_HAND_ZORG As Long = &H4                          ' B02: ハンド上下HOME(原点)

    Public Const LOFS_W42S As Long = 42                                 ' (W42.00-W42.15)
    Public Const LDSTS_CLMP_X_ON As Long = &H1                          ' B00:クランプX閉
    Public Const LDSTS_CLMP_Y_ON As Long = &H2                          ' B01:クランプY閉
    Public Const LDSTS_BREAK_X_ON As Long = &H4                         ' B02:割欠検出(X) V4.0.0.0�R
    Public Const LDSTS_BREAK_Y_ON As Long = &H8                         ' B03:割欠検出(Y) V4.0.0.0�R
    Public Const LDSTS_VACUME As Long = &H10                            ' B04:吸着確認(ワーク有)

    Public Const LOFS_W43S As Long = 43                                 ' (W43.00-W43.15)
    Public Const LDSTS_MG1_SUBTSENS As Long = &H10                      ' B04:供給基板検出(マガジン1)
    Public Const LDSTS_MG2_SUBTSENS As Long = &H20                      ' B05:収納基板検出(マガジン2)
    Public Const LDSTS_MG_EXIST As Long = &H40                          ' B06:マガジン有
    Public Const LDSTS_NGFULL As Long = &H80                            ' B07:NG排出BOX満杯


    Public Const LOFS_W44S As Long = 44                                 ' W44.00-W44.15

    '----- W52 -----
    Public Const LOFS_W52S As Long = 52                                 ' W52.00-W52.15
    Public Const OutSMG1Move_Sw As Long = &H1                           ' B00: MG1-動作 -SW 
    Public Const OutSMG1on_Sw As Long = &H2                             ' B01: MG1-上下切替え-Sw
    Public Const OutSMG2Move_Sw As Long = &H4                           ' B02: MG2-動作 -SW 
    Public Const OutSMG2on_Sw As Long = &H8                             ' B03: MG2-上下切替え-Sw

    Public Const LDABS_HAND1_VACUME As Long = &H100                     ' B08: 供給ハンド吸着 
    Public Const LDABS_HAND1_ABSORB As Long = &H200                     ' B09: 供給ハンド吸着破壊 
    Public Const LDABS_HAND2_VACUME As Long = &H400                     ' B10: 収納ハンド吸着
    Public Const LDABS_HAND2_ABSORB As Long = &H800                     ' B11: 収納ハンド吸着破壊

    '----- エアーバルブ関係 -----                                 
    Public Const LOFS_W53S As Long = 53                                 ' エアーバルブ関係(W53.00-W53.15)
    Public Const LDABS_CLMP_X As Long = &H1                             ' B00: クランプX (ONでバルブON 再度ONでバルブOFF)
    Public Const LDABS_CLMP_Y As Long = &H2                             ' B01: クランプY (同上)
    Public Const LDABS_VACUME As Long = &H4                             ' B02: 吸着 (同上)
    Public Const LDABS_ABSORB As Long = &H8                             ' B03: 破壊 (同上)

    '----- ローダ特別操作デバイス ----- 
    Public Const LOFS_W54S As Long = 54                                 ' エアーバルブ関係(W54.00-W54.15)

    Public Const LOFS_W60S As Long = 60                                 ' W60.00-W60.15
    Public Const LDABS_HAND_CW_MOVE As Long = &H1                       ' B00: 搬送ユニットCW(←)出力
    Public Const LDABS_HAND_CCW_MOVE As Long = &H2                      ' B01: 搬送ユニットCCW(→)出力
    Public Const LDABS_HAND_UP As Long = &H4                            ' B02: ハンド上下CW(↑)出力
    Public Const LDABS_HAND_DOWN As Long = &H8                          ' B03: ハンド上下CCW(↓)出力

    '----- 絶対位置移動 -----
    Public Const LOFS_W128S As Long = 128                                ' ロボット制御関係(W128.00-W128.15)
    Public Const POSMOVES_START_ADR As Long = &H200                      ' 動作開始アドレス

    '----- ロボット制御関係 -----                                 
    Public Const LOFS_W359S As Long = 359                                ' ロボット制御関係(W359.00-W359.15)
    Public Const LROBS_HAND_HOME As Long = &H1                           ' B00: ハンド上下ホーム位置
    Public Const LROBS_HAND_SPLY1 As Long = &H2                          ' B01: 供給ハンド下降距離
    Public Const LROBS_HAND_SPLY2 As Long = &H4                          ' B02: 供給ハンド上昇距離
    Public Const LROBS_HAND_STR2 As Long = &H10                          ' B04: 収納ハンド下降距離
    Public Const LROBS_HAND_STR4 As Long = &H40                          ' B06: 収納ハンド上昇距離
    Public Const LROBS_HAND_STAGE As Long = &H80                         ' B07: ハンド上下排出下降
    Public Const LROBS_HAND_NG As Long = &H100                           ' B08: ハンド上下上昇距離

    '----- V2.0.0.0�E↑ -----

    '----- V4.11.0.0�E↓ (WALSIN殿SL436S対応) -----
    '----- PC ←→ PLC通信用(SL436S用) -----
    Public Const LOFS_W113S As Long = 113                               ' PC → PLC通信用(W113.00-W113.15)
    Public Const LPCS_SET_SUBSTRATE As Long = &H10                      ' B04:基板追加要求(0=要求無,1=要求有)

    Public Const LOFS_W114S As Long = 114                               ' PLC → PC通信用(W114.00-W114.15)
    Public Const LPLCS_SET_LOTSTOPREADY As Long = &H2                   ' B01: LotStop準備完了
    Public Const LPLCS_SET_SUBSTRATE As Long = &H4                      ' B02: 基板追加準備完了

    '----- V4.11.0.0�E↑ -----

    '----- V4.0.0.0-26↓ -----
    '----- 品種選択 ----- 
    Public Const LOFS_D241S As Long = 241                                ' 品種選択(D241 1(品種1)〜10(品種10))(符号なし1WORD)
    Public Const LOFS_D242S As Long = 242                                ' ローバッテリーアラーム(W242 0正常、1:ローバッテリーアラーム

    Public Const LOFS_W360S As Long = 360                                ' 品種選択BIT(W360.00-W360.15)※LangSelectToolで使用(TKY側では未使用)
    '                                                                    ' B00:品種1
    '                                                                    ' B01:品種2
    '                                                                    '    :
    '                                                                    ' B08:品種9
    '                                                                    ' B09:品種10
    '                                                                    ' B10-15:予備
    '----- 座標書込み -----                                 
    Public Const LOFS_W361S As Long = 361                                ' 座標書込みBIT(W361.00-W361.15)
    '                                                                    ' B00:品種1
    '                                                                    ' B01:品種2
    '                                                                    '    :
    '                                                                    ' B08:品種9
    '                                                                    ' B09:品種10
    '                                                                    ' B10-15:予備
    '----- 二枚取りセンサ確認位置 -----                                 
    Public Const LOFS_D700S As Long = 700                                ' 座標データ(符号付2WORD)
    '                                                                    ' D700:品種1
    '                                                                    ' D702:品種2
    '                                                                    '    :
    '                                                                    ' D716:品種9
    '                                                                    ' D718:品種10
    '                                                                    ' B10-15:予備

    '----- 薄基板対応 -----                                 
    Public Const LOFS_D230S As Long = 230                                ' 現在の基板種別(D230 0=通常, 1=薄基板)(符号なし1WORD)

    '----- V4.0.0.0-26↑ -----
    '-------------------------------------------------------------------------------
    '   ■ローダシリアル通信用オフセット定義(SL436R用)■
    '-------------------------------------------------------------------------------
    ''----- ローダアラーム状態 -----
    'Public Const LOFS_W110 As Long = 110                                ' ローダアラーム状態(W110.08-10)
    Public Const LDPL_STP_AUTODRIVE As Long = &H800                     ' B11: 自動運転中断(PLC → PC) V1.23.0.0�I
    'Public Const LOFS_W115 As Long = 115                                ' ローダアラーム詳細内容(W115.00-W115.15(続行不可))
    'Public Const LOFS_W116 As Long = 116                                ' ローダアラーム詳細内容(W116.00-W116.15(続行可))

    ''----- ローダ特別操作デバイス -----
    'Public Const LOFS_W109 As Long = 109                                ' ローダ特別操作デバイス(W109.00-W109.15)
    'Public Const LDDV_ARM_START As Long = &H2                           ' B01: 一時停止状態(続行可能エラー等)からスタートする
    'Public Const LDDV_ARM_RESET As Long = &H8                           ' B03: アラームリセット
    'Public Const LDDV_BUZER_OFF As Long = &H10                          ' B04: ブザーOFF
    'Public Const LDDV_CLR_PRDUCT As Long = &H80                         ' B07: 生産数クリア
    'Public Const LDDV_ALM_DSP As Long = &H400                           ' B10: アラームメッセージ表示中(ON:表示中,OFF:表示中でない) 'V1.18.0.0�M
    Public Const LDDV_STP_AUTODRIVE As Long = &H2000                    ' B13: 自動運転中断(PC → PLC) V1.23.0.0�I

    '----- ローダ特別操作デバイス -----                                 ' ###001
    Public Const LOFS_W54 As Long = 54                                  ' エアーバルブ関係(W54.00-W54.15)
    Public Const LDAB_CLMP_X As Long = &H1                              ' B00: クランプX (ONでバルブON 再度ONでバルブOFF)
    Public Const LDAB_CLMP_Y As Long = &H2                              ' B01: クランプY (同上)
    Public Const LDAB_VACUME As Long = &H4                              ' B02: 吸着 (同上)
    Public Const LDAB_ABSORB As Long = &H8                              ' B03: 破壊 (同上)

    ''----- ###184↓ -----
    ''----- 初期設定状態 -----                                 
    'Public Const LOFS_H00 As Long = 0                                   ' (H0.00-H0.15)
    'Public Const LHST_MAGAZINE As Long = &H1                            ' B00: ON=4マガジン, OFF=2マガジン

    '----- 物理入力状態 -----
    Public Const LOFS_W42 As Long = 42                                  ' (W42.00-W42.15)
    Public Const LDST_MG1_DOWN As Long = &H1                            ' B00:マガジン1下限 V1.23.0.0�D
    Public Const LDST_MG1_UP As Long = &H2                              ' B01:マガジン1上限 V1.23.0.0�D
    Public Const LDST_MG1_SUBTSENS As Long = &H100                      ' B08:基板検出(マガジン1)
    Public Const LDST_MG2_SUBTSENS As Long = &H200                      ' B09:基板検出(マガジン2)
    Public Const LDST_MG3_SUBTSENS As Long = &H400                      ' B10:基板検出(マガジン3)
    Public Const LDST_MG4_SUBTSENS As Long = &H800                      ' B11:基板検出(マガジン4)
    '----- V1.18.0.0�H↓ -----
    Public Const LDST_MG1_EXIST As Long = &H1000                        ' B12:マガジン1有
    Public Const LDST_MG2_EXIST As Long = &H2000                        ' B13:マガジン2有
    Public Const LDST_MG3_EXIST As Long = &H4000                        ' B14:マガジン3有
    Public Const LDST_MG4_EXIST As Long = &H8000                        ' B15:マガジン4有
    Public Const LDST_MG2_REVERS As Long = &H8000                       ' B14:マガジン2裏/表(1/0) ※ローム殿特注
    '----- V1.18.0.0�H↑ -----

    Public Const LOFS_W44 As Long = 44                                  ' (W44.00-W44.15)
    Public Const LDST_CLMP_X_ON As Long = &H1                           ' B00:クランプX閉 V1.16.0.0�D
    Public Const LDST_CLMP_Y_ON As Long = &H2                           ' B01:クランプY閉 V1.16.0.0�D
    Public Const LDST_BREAK_X_ON As Long = &H4                          ' B02:割欠検出(X) V4.0.0.0�R
    Public Const LDST_BREAK_Y_ON As Long = &H8                          ' B03:割欠検出(Y) V4.0.0.0�R
    Public Const LDST_VACUME As Long = &H10                             ' B04:吸着確認(ワーク有)
    '                                                                   ' B05:２枚取検出
    Public Const LDST_NGFULL As Long = &H40                             ' B06:NG排出BOX満杯 ###197
    '----- ###184↑ -----

    '----- ###144↓ -----
    '----- エアーバルブ関係 -----                                 
    Public Const LOFS_W53 As Long = 53                                  ' エアーバルブ関係(W53.00-W53.15)
    Public Const LDAB_HAND_ORGMOVE As Long = &H1                        ' B00: ハンド旋回原点移動(ONでバルブON 再度ONでバルブOFF)
    Public Const LDAB_HAND_MOVE As Long = &H2                           ' B01: ハンド旋回動作移動(同上)
    Public Const LDAB_HAND1_DOWN As Long = &H4                          ' B02: ハンド1下降 (同上)
    Public Const LDAB_HAND2_DOWN As Long = &H8                          ' B03: ハンド2下降 (同上)
    Public Const LDAB_HAND1_VACUME As Long = &H10                       ' B04: ハンド1吸着 (同上)
    Public Const LDAB_HAND1_ABSORB As Long = &H20                       ' B05: ハンド1破壊 (同上)
    Public Const LDAB_HAND2_VACUME As Long = &H40                       ' B06: ハンド2吸着 (同上)
    Public Const LDAB_HAND2_ABSORB As Long = &H80                       ' B07: ハンド2破壊 (同上)
    '----- ###188↓ -----
    Public Const LDAB_HAND1_ZORG As Long = &H400                        ' B10: ハンド1Z原点(同上)
    Public Const LDAB_HAND1_ZMOVE As Long = &H800                       ' B11: ハンド1Z動作(同上)
    Public Const LDAB_HAND2_ZORG As Long = &H1000                       ' B12: ハンド2Z原点(同上)
    Public Const LDAB_HAND2_ZMOVE As Long = &H2000                      ' B13: ハンド2Z動作(同上)
    '----- ###188↑ -----
    '----- ###144↑ -----

    '-----#### W52 ####----- 2013.01.28 '###182  
    Public Const LOFS_W52 As Long = 52                                  ' W52アドレス 
    Public Const OutMG1Move_Sw As Long = &H1                            ' MG1-動作 -SW 
    Public Const OutMG1on_Sw As Long = &H2                              ' MG1-上下切替え-Sw
    Public Const OutMG2Move_Sw As Long = &H4                            ' MG2-動作 -SW 
    Public Const OutMG2on_Sw As Long = &H8                              ' MG2-上下切替え-Sw
    Public Const OutMG3Move_Sw As Long = &H10                           ' MG3-動作 -SW 
    Public Const OutMG3on_Sw As Long = &H20                             ' MG3-上下切替え-Sw
    Public Const OutMG4Move_Sw As Long = &H40                           ' MG4-動作 -SW 
    Public Const OutMG4on_Sw As Long = &H80                             ' MG4-上下切替え-Sw
    '-----#### W52 ####-----  

    '----- ###197↓ -----
    '----- ロボット制御関係 -----                                 
    Public Const LOFS_W339 As Long = 339                                ' ロボット制御関係関係(W339.00-W339.15)
    Public Const LROB_HAND_HOME As Long = &H1                           ' B00: ハンドホーム位置
    Public Const LROB_HAND_SPLY1 As Long = &H2                          ' B01: ハンド供給位置1
    Public Const LROB_HAND_SPLY2 As Long = &H4                          ' B02: ハンド供給位置2
    Public Const LROB_HAND_SPLY3 As Long = &H8                          ' B03: ハンド供給位置3
    Public Const LROB_HAND_STR2 As Long = &H10                          ' B04: 収納位置2
    Public Const LROB_HAND_STR3 As Long = &H20                          ' B05: 収納位置3
    Public Const LROB_HAND_STR4 As Long = &H40                          ' B06: 収納位置4
    Public Const LROB_HAND_STAGE As Long = &H80                         ' B07: ハンド搭載位置
    Public Const LROB_HAND_NG As Long = &H100                           ' B08: NG位置
    '----- 絶対位置移動 -----
    Public Const LOFS_W128 As Long = 128                                ' ロボット制御関係関係(W128.00-W128.15)
    Public Const POSMOVE_START_ADR As Long = &H200                      ' 動作開始アドレス

    '----- ###197↑ -----

    '----- V4.0.0.0�R↓ -----
    '----- PC ←→ PLC通信用(SL436R用(SL436SはI/O)) -----                                 
    Public Const LOFS_W113 As Long = 113                                ' PC → PLC通信用(W113.00-W113.15)
    Public Const LPC_CYCL_STOP As Long = &H1                            ' B00: サイクル停止(SL436SはI/O)
    Public Const LPC_LOT_STOP As Long = &H4                             ' B02: LOT中断要求(SL436S)
    Public Const LPC_LOT_STOP_PREPARE As Long = &H8                     ' B03: LOT中断処理準備要求(SL436S)
    Public Const LPC_LOT_STOP_SUB_EXIST As Long = &H20                  ' B05: LOT中断時基板有無(SL436S)
    '                                                                   ' B01: 予備
    '                                                                   '  : :
    '                                                                   ' B15: 予備

    Public Const LOFS_W114 As Long = 114                                ' PLC → PC通信用(W114.00-W114.15)
    Public Const LPLC_CYCL_STOP As Long = &H1                           ' B00: サイクル停止応答(SL436SはI/O)
    '                                                                   ' B01: 予備
    '                                                                   '  : :
    '                                                                   ' B15: 予備
    '----- V4.0.0.0�R↑ -----

    'V4.12.2.0�E     ↓'V6.0.4.1�@
    Public Const LOFS_W192 As Long = 192                                ' PC → PLC通信用(W192)
    Public Const LPC_CLAMP_EXEC As Long = &H80                          ' B07: クランプ実行有無
    'V4.12.2.0�E     ↑'V6.0.4.1�@

    '----- ###188↓ -----
    '-------------------------------------------------------------------------------
    '   軸関係定義
    '-------------------------------------------------------------------------------
    '----- 軸種別 -----
    Public Const MAX_AXIS As Integer = 4                                ' 軸最大数
    'Public Const AXIS_X As Integer = 0                                  ' X軸 ※TrimErrNo.vbに定義有
    'Public Const AXIS_Y As Integer = 1                                  ' Y軸
    'Public Const AXIS_Z As Integer = 2                                  ' Z軸
    'Public Const AXIS_T As Integer = 3                                  ' Theta軸

    '----- サブステータスアドレス -----
    Public Const ADRSUB_STS_X As Integer = &H2118                       ' X軸
    Public Const ADRSUB_STS_Y As Integer = &H2158                       ' Y軸
    Public Const ADRSUB_STS_Z As Integer = &H2198                       ' Z軸
    Public Const ADRSUB_STS_T As Integer = &H21D8                       ' Theta軸

    '----- 軸サブステータスアドレス -----
    Public AxisSubStsAry(MAX_AXIS) As Integer

    '----- サブステータス -----                                 
    Public Const SUBSTS_ORG As Integer = &H4000                         ' B14: ORG(ORG入力時ON)

    '----- ###240↓ -----
    '----- 吸着検出アドレス(SL432R) -----
    Public Const VACUME_STS As Integer = &H216A                         ' 吸着検出アドレス
    Public Const VACUME_STS_BIT As Long = &H4                           ' B02: 吸着検出(0=OFF/1=ON)
    '----- ###240↑ -----
    '----- ###188↑ -----

    '----- V2.0.0.0�L↓ -----
    '----- サーボアラーム(SL432R/SL436S) -----
    '-----<ステータスアドレス>-----
    Public Const SRVALM_XY_STS As Integer = &H210A                      ' ステータスアドレス(X,Y軸)
    Public Const SRVALM_ZT_STS As Integer = &H213A                      ' ステータスアドレス(Z,θ軸)
    Public Const SRVALM_Z2_STS As Integer = &H21BA                      ' ステータスアドレス(Z2軸)

    '-----<ステータビット>-----
    Public Const SRVALM_X_BIT As Long = &H10                            ' B04: X軸サーボアラーム (0=正常/1=ALM)
    Public Const SRVALM_Y_BIT As Long = &H20                            ' B05: Y軸サーボアラーム (0=正常/1=ALM)

    Public Const SRVALM_Z_BIT As Long = &H40                            ' B06: Z軸サーボアラーム (0=正常/1=ALM)
    Public Const SRVALM_T_BIT As Long = &H80                            ' B07: θ軸サーボアラーム (0=正常/1=ALM)

    Public Const SRVALM_Z2_BIT As Long = &H40                           ' B06: Z2軸サーボアラーム (0=正常/1=ALM)
    '----- V2.0.0.0�L↑ -----

    '===============================================================================
    '   変数定義
    '===============================================================================
    Public Const LTIMER_COUNT As Integer = 22                           ' ローダ部タイマ数
    Public LTimerDT(LTIMER_COUNT) As UShort                             ' ローダ部タイマデータ
    Public LoaderVer As String                                          ' ローダバージョン情報
    Public gLdWDate As UShort = 0                                       ' ローダ部送信データ(モニタ用)
    Public gLdRDate As UShort = 0                                       ' ローダ部受信データ(モニタ用)

    '----- ローダアラーム情報 -----
    Private Const LALARM_COUNT As Integer = 128                         ' 最大アラーム数
    Private strLoaderAlarm(LALARM_COUNT) As String                      ' アラーム文字列
    Private strLoaderAlarmInfo(LALARM_COUNT) As String                  ' アラーム情報1
    Private strLoaderAlarmExec(LALARM_COUNT) As String                  ' アラーム情報(対策)
    Private AlarmCount As Integer                                       ' アラーム数
    Private iBefData(7) As Integer                                      ' アラーム情報退避域
    Public iBefData_ClampVac(7) As Integer

    '----- フラグ等 -----
    Private bFgTimeOut As Boolean                                       ' ローダ通信タイムアウトフラグ
    Private bFgActLink As Boolean                                       ' ローダｰRS232C有効フラグ
    Private bFgLoaderAlarmFRM As Boolean                                ' ローダアラーム画面表示中フラグ

    '----- ファイルパス名 -----
    Private Const SysParamPath As String = "C:\TRIM\tky.ini"            ' システムパラメータパス名

    '----- PLC通信用オブジェクト -----
    Private m_PlcIf As DllPlcIf.DllPlcInterface

    ' 'V4.12.2.0�I↓'V6.0.5.0�E
    ' CIO=6 
    Public Const LIN_MONITOR_CLAMPOUT As UShort = &H1                           ' B00: クランプ出力状態 
    Public Const LIN_MONITOR_VACOUT As UShort = &H4                             ' B02: 吸着出力状態 
    ' 'V4.12.2.0�I↑'V6.0.5.0�E

#End Region

#Region "【オートローダＩＯ用メソッド】"
    '===============================================================================
    '   オートローダＩＯ処理
    '===============================================================================
#Region "オートローダへデータを出力する"
    '''=========================================================================
    ''' <summary>オートローダへデータを出力する</summary>
    ''' <param name="lOn"> (INP)On Bit</param>
    ''' <param name="lOff">(INP)Off Bit</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function SetLoaderIO(ByVal lOn As UShort, ByVal lOff As UShort) As Integer

        Dim r As Integer
        Dim strMSG As String
        'V4.12.2.0�I↓'V6.0.5.0�E
        Dim ReadAddress As Integer
        Dim SerialReadData As Integer
        Dim tmplOn As UShort
        Dim tmplOff As UShort

        Try

            If ((lOff And LOUT_AUTO) = LOUT_AUTO) And (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then  '手動に切り替え時
                ' CIO=6
                ReadAddress = 6
                SerialReadData = 0
                tmplOn = 0
                tmplOff = 0

                m_PlcIf.ReadPlcCIO(ReadAddress, SerialReadData)
                'クランプON/OFF
                If SerialReadData And LIN_MONITOR_CLAMPOUT Then
                    tmplOff = LOUT_CLAMP
                Else
                    tmplOn = LOUT_CLAMP
                End If
                '吸着ON/OFF
                If SerialReadData And LIN_MONITOR_VACOUT Then
                    tmplOn = tmplOn Or LOUT_VACCUME
                Else
                    tmplOff = tmplOff Or LOUT_VACCUME
                End If

                ' オートローダ出力(OnBit, OffBit)
                r = ZATLDSET(tmplOn, tmplOff)
            End If
            'V4.12.2.0�I↑'V6.0.5.0�E


            ' ローダ部送信データ設定(モニタ用)
            gLdWDate = gLdWDate And (lOff Xor &HFFFF)                   ' Bit Off 
            gLdWDate = gLdWDate Or lOn                                  ' Bit On 

            ' オートローダ出力(OnBit, OffBit)
            r = ZATLDSET(lOn, lOff)

            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SetLoaderIO() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "オートローダからデータを入力する"
    '''=========================================================================
    ''' <summary>オートローダからデータを入力する</summary>
    ''' <param name="LdIn">(OUT)入力データ</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function GetLoaderIO(ByRef LdIn As UShort) As Integer

        Dim r As Integer
        Dim iData As Integer
        Dim strMSG As String

        Try
            ' オートローダ入力
            r = ZATLDGET(iData)
            'Call INP16(&H219A, stt)
            LdIn = iData                                                ' ローダ部受信データ設定
            gLdRDate = iData                                            ' ローダ部受信データ設定(モニタ用)
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.GetLoaderIO() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "オートローダ初期処理"
    '''=========================================================================
    ''' <summary>オートローダ初期処理</summary>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_Init() As Integer

        Dim r As Integer
        Dim LdIn As UShort
        Dim strMSG As String

        Try
            ' ＩＯモニタを表示する
#If (cIOMONITOR = 1) Then
            Form1.ObjIOMon = New FrmIOMon()                             ' オブジェクト生成
            Call Form1.ObjIOMon.show()                                  ' ＩＯモニタ表示(モードレス)
#End If
            ' 初期処理
            bFgActLink = False                                          ' ローダ有効フラグ初期化
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then                   ' SL436系でなければNOP 
                Return (cFRS_NORMAL)
            End If

            ' オートローダ初期処理
            r = GetLoaderIO(LdIn)                                       ' ローダ入力
            If ((LdIn And LINP_READY) <> LINP_READY) Then               ' ローダ無 ?
                Return (cFRS_NORMAL)
            End If
            bFgActLink = True                                           ' ローダｰRS232C有効フラグON

            '' 釜屋殿特殊対応をローダに通知
            'ReDim iData(0)
            'iData(0) = giCustomFlg                                  ' (1)釜屋殿向け　(1以外)通常動作
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4020", 1, iData)

            '' 薄物基板特殊対応をローダに通知
            'iData(0) = gdOPX2DFlg                                   ' (1)薄物対応　(1以外)通常動作
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4021", 1, iData)

            '' 載物台吸着アラーム検出をローダに通知
            'iData(0) = giOPVacFlg                                   '(0)無し　(1)有り
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4022", 1, iData)

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "オートローダ終了処理"
    '''=========================================================================
    ''' <summary>オートローダ終了処理</summary>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_Term() As Integer

        Dim strMSG As String

        Try
            ' ローダー出力(ON=なし,OFF=トリマ部レディ)
            'V4.5.0.0�B↓　'V4.4.0.0�@            Call SetLoaderIO(LOUT_REDY, &H0)                            '###098
            Call SetLoaderIO(LOUT_REDY, LOUT_AUTO)                            'V4.4.0.0�@
            'V4.5.0.0�B↑

            ' オブジェクト開放
            If (Form1.ObjIOMon Is Nothing = False) Then                 ' ＩＯモニタ表示 ?
                Call Form1.ObjIOMon.Close()                             ' オブジェクト開放
                Call Form1.ObjIOMon.Dispose()                           ' リソース開放
            End If

            ''#4.12.3.0�F↓
            If m_PlcIf Is Nothing = False Then
                m_PlcIf.ClosePlcFinsPort()
            End If
            ''#4.12.3.0�F↑

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_Term() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "原点復帰処理(オートローダ/トリマ)"
    '''=========================================================================
    ''' <summary>原点復帰処理(オートローダ/トリマ)</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>ローダ原点復帰後、トリマ原点復帰を行う</remarks>
    '''=========================================================================
    Public Function Loader_OrgBack(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Loader_OrgBack(ByVal ObjSys As Object) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 原点復帰処理(オートローダ/トリマ)
            r = Sub_CallFrmRset(ObjSys, cGMODE_ORG)                 ' 原点復帰処理 
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_OrgBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "オートローダ原点復帰実行サブルーチン"
    '''=========================================================================
    ''' <summary>オートローダ原点復帰実行サブルーチン</summary>
    ''' <param name="igMode">(INP)処理モード</param>
    ''' '                         ※下記を想定
    '''                             cGMODE_ORG     = 原点復帰
    '''                             cGMODE_LDR_ORG = ローダ原点復帰
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_LDR  = ローダアラーム検出
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    ''' </returns> 
    '''=========================================================================
    Public Function Sub_Loader_OrgBack(ByVal igMode As Integer) As Integer

        Dim TimerRS As System.Threading.Timer = Nothing
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer                                                ' ###163
        Dim LdIn As UShort
        'V6.0.0.0�Q        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            '(2012/02/21)オートローダリセット処理を追加

            ' オートローダ原点復帰処理
            If (bFgActLink = False) Then                                ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            ' シグナルタワー制御(On=原点復帰中,Off=全ﾋﾞｯﾄ)
            If (igMode = cGMODE_LDR_ORG) Then                           ' ローダ原点復帰モード ?
                ' シグナルタワー制御(On=原点復帰中,Off=全ﾋﾞｯﾄ) ###007
                Select Case (gSysPrm.stIOC.giSignalTower)
                    Case SIGTOWR_NORMAL                                 ' 標準(原点復帰中(緑点滅))
                        'V5.0.0.9�M ↓ V6.0.3.0�G(ローム殿仕様は無点灯)
                        ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_BLK, &HFFFF)
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ZRN)
                        'V5.0.0.9�M ↑ V6.0.3.0�G

                    Case SIGTOWR_SPCIAL                                 ' 特注(原点復帰中(黄色点滅))
                        'Call Form1.System1.SetSignalTower(EXTOUT_YLW_BLK, &HFFFF)
                End Select
            End If

            ' ダミーでシリアル通信実施しPLCとの通信状態をチェックする


            ' ローダ出力(On=トリマ部レディ+トリマ正常, Off=左記以外)
            'Call SetLoaderIO(LOUT_REDY + LOUT_NO_ALM, Not (LOUT_REDY + LOUT_NO_ALM))'###098
            '###148            Call SetLoaderIO(LOUT_NO_ALM, LOUT_REDY) '###098
            Call SetLoaderIO(0, LOUT_REDY) '###098

            ' ローダ原点復帰処理
            Call SetLoaderIO(LOUT_ORG_BACK, &H0)                        ' ローダ出力(On=ローダ原点復帰要求, Off=0)

            ' 前回のローダ原点復帰完了BitのOffを待つ
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(Form1.System1, LINP_ORG_BACK, False, True, True, 0, 0, 0)
            If (rtnCode <> cFRS_NORMAL) Then                            ' エラー ? (※エラー発生時のメッセージは表示済)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' 軽故障(続行可能) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then  ' ###196
                    Call W_RESET()                                      ' アラームリセット信号送出 
                    Call W_START()                                      ' スタート信号送出 
                    GoTo STP_RETRY
                End If
                '----- V2.0.0.0_22↓ -----
                If (giMachineKd = MACHINE_KD_RS) Then                   ' Sl436S時はステージを原点に戻さない
                    ''V4.1.0.0�H
                    ' トリマ運転中(原点復帰後は原則ONのまま、一時停止はローダの基板交換動作中に停止したい場合に使用(使用しない?))
                    Call SetLoaderIO(LOUT_STS_RUN, LOUT_ORG_BACK)               ' ローダ出力(On=トリマ運転中, Off=ローダ原点復帰要求)
                    ''V4.1.0.0�H

                    Return (rtnCode)
                End If
                '----- V2.0.0.0_22↑ -----
                'Return (rtnCode)                                       ' ###163
                GoTo STP_END                                            ' ###163
            End If

            ' ローダ通信タイムアウトチェック用タイマーオブジェクトの作成(TimerRS_TickをX msec間隔で実行する)
            Sub_SetTimeoutTimer(TimerRS)

            ' ローダの原点復帰完了を待つ
            Do
                ' ローダアラーム/非常停止チェック
                Call GetLoaderIO(LdIn)                                  ' ローダ入力
                If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                    'Return (cFRS_ERR_LDR)                              ' ###163 Return値 = ローダアラーム検出
                    rtnCode = cFRS_ERR_LDR                              ' ###163
                    GoTo STP_END                                        ' ###163
                End If

                ' ローダ通信タイムアウトチェック 
                If (bFgTimeOut = True) Then                             ' タイムアウト ?
                    ' コールバックメソッドの呼出しを停止する
                    TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                    rtnCode = cFRS_ERR_LDRTO                            ' Return値 = ローダ通信タイムアウト
                    GoTo STP_END
                End If

                ''V4.1.0.0�I
                ' インターロック状態の表示およびローダへ通知(SL436R) ###162
                r = Form1.DispInterLockSts()
                'V4.1.0.0�I
                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While ((LdIn And LINP_ORG_BACK) <> LINP_ORG_BACK)      ' 原点復帰完了待ち


            ' 終了処理
STP_END:
            ''V4.1.0.0�H
            ' トリマ運転中(原点復帰後は原則ONのまま、一時停止はローダの基板交換動作中に停止したい場合に使用(使用しない?))
            Call SetLoaderIO(LOUT_STS_RUN, LOUT_ORG_BACK)               ' ローダ出力(On=トリマ運転中, Off=ローダ原点復帰要求)
            ''V4.1.0.0�H

            ' コールバックメソッドの呼出しを停止する
            If (IsNothing(TimerRS) = False) Then                        ' ###173
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' タイマーを破棄する
            End If

            'V5.0.0.1-21↓
            'ローダ原点復帰タイムアウトの場合には、
            If rtnCode = cFRS_ERR_LDRTO Then
                Return (rtnCode)
            ElseIf rtnCode = cFRS_ERR_LDR Then
                ' "ローダ原点復帰未完了","",""
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH37, "", "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
                Return (rtnCode)
            End If
            'V5.0.0.1-21↑

            '----- ###163↓ -----
            ' ローダエラーならステージを原点に戻す(残基板取り除くため)
            If (rtnCode <> cFRS_NORMAL) Then
                ' XYZθ軸初期化
                r = Form1.System1.EX_SYSINIT(gSysPrm, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (※メッセージは表示済)
                    Return (r)
                End If

                ' 第二原点位置にステージを移動する
                If (giMachineKd = MACHINE_KD_RS) Then
                    If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                        r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                        If (r < cFRS_NORMAL) Then                           ' エラー ?(※エラーメッセージは表示済み) 
                            Return (r)
                        End If
                    End If
                End If

            End If
            '----- ###163↑ -----

            ' シグナルタワー制御(On=レディ(手動),Off=全ﾋﾞｯﾄ) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(無点灯)
                    'V5.0.0.9�M ↓ V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(0, &HEFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M ↑ V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' 特注(黄色点灯)
                    'Call Form1.System1.SetSignalTower(EXTOUT_YLW_ON, &HEFFF)
            End Select

            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_Loader_OrgBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "トリマ部エラー発生時処理"
    '''=========================================================================
    ''' <summary>トリマ部エラー発生時処理</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <param name="gMode"> (INP)処理モード(下記を想定)
    '''                           ・cGMODE_ERR_HING   (連続HI-NGエラー発生)
    '''                           ・cGMODE_ERR_REPROBE(再プロービング失敗)
    '''                                                          </param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_TrimError(ByVal ObjSys As SystemNET, ByVal gMode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Loader_TrimError(ByVal ObjSys As Object, ByVal gMode As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ローダ無効NならOP
            If (bFgActLink = False) Then                            ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            ' トリマ部エラー発生時処理
            r = Sub_CallFrmRset(ObjSys, gMode)
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_TrimError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "オートローダ用トリマエラー発生時処理サブルーチン"
    '''=========================================================================
    ''' <summary>オートローダ用トリマエラー発生時処理サブルーチン</summary>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function Sub_Loader_TrimError() As Integer

        Dim r As Integer
        Dim LdIn As UShort
        Dim iData() As UShort
        'V6.0.0.0�Q        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            ' ローダ無効ならNOP
            If (bFgActLink = False) Then                                ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            ' オートローダ用トリマエラー発生時処理
            ReDim iData(0)
            iData(0) = &H2                                          ' ﾄﾘﾏ初期化完了ﾌﾗｸﾞ 1にしないとﾛｰﾀﾞｰが動かないため
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)    ' 上位イニシャライズ完了
            'Call wait(0.1)
            'iData(0) = &H0                                         ' ﾄﾘﾏ初期化完了ﾌﾗｸﾞ 1にしないとﾛｰﾀﾞｰが動かないため
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)    ' 上位イニシャライズ完了

            'Call SetSignalTower(4, 0)                              'ﾛｰﾀﾞｱﾗｰﾑ リセット
            'Call SetSignalTower(3, 1)                              'ｼｸﾞﾅﾙﾀﾜｰ原点復帰ON

            ' ローダ初期化(ON=トリマ部正常, OFF=左記以外) 
            '###148            Call SetLoaderIO(LOUT_NO_ALM, Not (LOUT_NO_ALM))
            Call SetLoaderIO(0, 0)

            'ReDim iData(0)
            'iData(0) = &H2                      'ﾛｰﾀﾞｰｱﾗｰﾑﾘｾｯﾄ
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)
            'Call wait(0.1)
            'iData(0) = &H0                      '自動ﾓｰﾄﾞ(手動ｾｯﾄ)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)
            'iData(0) = &H1                      'ﾄﾘﾏ初期化完了ﾌﾗｸﾞ
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4000", 1, iData)

            'iData(0) = &H4                      '自動運転ｷｬﾝｾﾙ
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)
            'Call wait(0.2)
            'iData(0) = &H0                      '自動ﾓｰﾄﾞ(手動ｾｯﾄ)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)

            ' ローダ原点復帰可/不可取得
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "M4992", 1, iData)
            If (iData(0) And &H800) Then
                ' ローダ原点復帰可の場合
                Call SetLoaderIO(LOUT_ORG_BACK, &H0)                        ' ローダ出力(On=ローダ原点復帰要求, Off=0)
                r = GetLoaderIO(LdIn)                                       ' ローダ入力
                ' ローダの原点復帰完了を待つ
                Do
                    r = GetLoaderIO(LdIn)                                   ' ローダ入力
                    If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                        Return (cFRS_ERR_LDR)                               ' Return値 = ローダアラーム検出
                    End If
                    System.Windows.Forms.Application.DoEvents()
                Loop While ((LdIn And LINP_ORG_BACK) <> LINP_ORG_BACK)      ' ローダ原点復帰完了待ち
                Call SetLoaderIO(&H0, LOUT_ORG_BACK)                        ' ローダ出力(On=0, Off=ローダ原点復帰要求)

            Else
                ' ローダ原点復帰不可の場合
                Return (cFRS_ERR_LDR)                                       ' Return値 = ローダアラーム検出
            End If

            'Call AbsVaccume(0)
            'Call ClampCtrl(0)
            'Call Adsorption(0)
            'Call SetSignalTower(3, 0)                                      ' ｼｸﾞﾅﾙﾀﾜｰ原点復帰OFF

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_Loader_TrimError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダ自動/手動モード切替え処理"
    '''=========================================================================
    ''' <summary>ローダ自動/手動モード切替え処理</summary>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <param name="bFgAutoMode">  (OUT)ローダ自動モードフラグ</param>
    ''' <param name="Md">           (INP)切替えモード(1=自動モード, 0=手動モード)</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    '''          cFRS_ERR_RST  = RESETキー押下
    '''          cFRS_ERR_CVR  = 筐体カバー開検出
    '''          cFRS_ERR_SCVR = スライドカバー開検出
    '''          cFRS_ERR_EMG  = 非常停止 他
    ''' </returns> 
    '''=========================================================================
    Public Function Loader_ChangeMode(ByVal ObjSys As SystemNET, ByRef bFgAutoMode As Boolean, ByVal Md As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Loader_ChangeMode(ByVal ObjSys As Object, ByRef bFgAutoMode As Boolean, ByVal Md As Integer) As Integer

        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim LdIn As UShort
        'V6.0.0.0�Q        Dim objForm As Object = Nothing
        Dim bOnOff As Boolean
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   SL432R系時の処理
            '-------------------------------------------------------------------
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ? 
                If (Md = MODE_AUTO) Then                                ' ローダ自動/手動モード切替え
                    ' ローダ自動モード切替え時
                    '    ' シグナルタワー制御(On=自動運転中 , Off=異常+ﾌﾞｻﾞｰ1)  ※シグナルタワー制御はOcxSystemで行う
                    '    Call Form1.System1.SetSignalTower(EXTOUT_GRN_ON, &HFFFF)

                    ' ローダー出力(ON=トリマ動作中, OFF=ﾄﾘﾐﾝｸﾞNG+ﾊﾟﾀｰﾝ認識NG+ﾄﾘﾏｰｴﾗｰ) ###035
                    If gbLoaderSecondPosition Then  'V5.0.0.6�A
                        Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_NG Or COM_STS_PTN_NG Or COM_STS_TRM_ERR Or COM_STS_TRM_COMP_SUPPLY)
                    Else
                        Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_NG Or COM_STS_PTN_NG Or COM_STS_TRM_ERR)
                    End If
                Else
                    ' ローダ手動モード切替え時

                    '    ' シグナルタワー制御(On=レディ(手動),Off=異常+ﾌﾞｻﾞｰ1)  ※シグナルタワー制御はOcxSystemで行う
                    '    Call Form1.System1.SetSignalTower(EXTOUT_YLW_ON, &HFFFF)
                End If

                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   SL436R系時の処理
            '-------------------------------------------------------------------
            ' ローダ無効またはローダ手動モードならNOP
            If ((bFgActLink = False) Or (gbFgAutoOperation = False)) Then
                Return (cFRS_NORMAL)
            End If

            ' ローダ自動/手動モード切替え
            If (Md = MODE_AUTO) Then
                ' ローダ自動モード切替え時
                ' ローダの有無(Ready/Not Ready)をチェックする
                bFgAutoMode = False                                     ' ローダ自動モードフラグOFF
                r = GetLoaderIO(LdIn)                                   ' ローダ入力
                If ((LdIn And LINP_READY) = &H0) Then                   ' ローダ無し？
                    Return (cFRS_NORMAL)
                End If
                ' ローダ自動モード切替え(マガジンチェック)
                Call SetLoaderIO(LOUT_AUTO, &H0)                        ' ローダ出力(ON=自動, OFF=なし)
            Else
                ' ローダ手動モード切替え時
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ローダ出力(ON=なし, OFF=自動)
            End If

            ' ローダからのモード切替完了を待つ
            If (Md = MODE_AUTO) Then                                    ' ローダ自動モード切替え ?
                bOnOff = True                                           ' On待ち
            Else                                                        ' ローダ手動モード切替え時
                bOnOff = False                                          ' Off待ち
            End If
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_AUTO, bOnOff, True, False, 0, 0, 0)
            'If (rtnCode = cFRS_ERR_LDR3) Then                           ' 軽故障(続行可能) ? ###196 ###073
            If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                Call W_RESET()                                          ' アラームリセット信号送出 
                Call W_START()                                          ' スタート信号送出 
                GoTo STP_RETRY
            End If

            ' ローダ自動モードフラグ設定
            If (rtnCode = cFRS_NORMAL) Then
                If (Md = MODE_AUTO) Then
                    ' シグナルタワー制御(On=自動運転中,Off=全ﾋﾞｯﾄ) ###007
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                                     ' 標準(緑点灯)
                            'V5.0.0.9�M ↓ V6.0.3.0�G(ローム殿仕様は緑点灯)
                            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                            'V5.0.0.9�M ↑ V6.0.3.0�G

                        Case SIGTOWR_SPCIAL                                     ' 特注(緑点灯)
                            'Call Form1.System1.SetSignalTower(EXTOUT_GRN_ON, &HFFFF)
                    End Select
                    bFgAutoMode = True                                  ' ローダ自動モードフラグON
                Else
                    ' シグナルタワー制御(On=レディ, Off=全て) ###007
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                             ' 標準(無点灯)
                            'V5.0.0.9�M ↓ V6.0.3.0�G
                            ' Call Form1.System1.SetSignalTower(0, &HFFFF)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                            'V5.0.0.9�M ↑ V6.0.3.0�G

                        Case SIGTOWR_SPCIAL                             ' 特注(黄色点灯)
                            'Call Form1.System1.SetSignalTower(EXTOUT_YLW_ON, &HFFFF)
                    End Select
                    bFgAutoMode = False                                 ' ローダ自動モードフラグOFF
                End If
            End If
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_ChangeMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "トリミング前処理"
    '''=========================================================================
    ''' <summary>トリミング前処理</summary>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <param name="bFgAutoMode">  (INP)ローダ自動モードフラグ</param>
    ''' <param name="dblCSx">       (INP)チップサイズX</param>
    ''' <param name="dblCSy">       (INP)チップサイズY</param>
    ''' <param name="iTrimNgMAX">   (INP)連続トリミングＮＧ枚数上限</param>
    ''' <param name="iTrimNgCount"> (INP)トリミングＮＧカウンタ(上限)</param>
    ''' <param name="iTrimNgBoxCnt">(INP)割欠け排出カウンタ(上限)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_TrimPreStart(ByVal ObjSys As SystemNET, ByVal bFgAutoMode As Boolean, ByVal dblCSx As Double, ByVal dblCSy As Double, _
                                        ByVal iTrimNgMAX As Integer, ByVal iTrimNgCount As Integer, ByVal iTrimNgBoxCnt As Integer) As Integer 'V6.0.0.0�Q
        'Public Function Loader_TrimPreStart(ByVal ObjSys As Object, ByVal bFgAutoMode As Boolean, ByVal dblCSx As Double, ByVal dblCSy As Double, _
        '                                    ByVal iTrimNgMAX As Integer, ByVal iTrimNgCount As Integer, ByVal iTrimNgBoxCnt As Integer) As Integer

        'Dim iData() As UShort
        Dim strMSG As String

        Try
            ' ローダ無効またはローダ手動モードならNOP
            If (bFgActLink = False) Or (bFgAutoMode = False) Then
                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   チップサイズが0606以下はローダに品種データ送信(STB)する
            '-------------------------------------------------------------------
            ' 千鳥指定時はチップサイズを設定しなおす(チップサイズ=チップサイズ÷ブロック数) ###136
            If (typPlateInfo.intDirStepRepeat = STEP_RPT_CHIPXSTPY) Then        ' ステップ＆リピート = チップ幅+X方向
                dblCSx = typPlateInfo.dblChipSizeXDir / typPlateInfo.intBlockCntXDir
            ElseIf (typPlateInfo.intDirStepRepeat = STEP_RPT_CHIPYSTPX) Then    ' ステップ＆リピート = チップ幅+X方向
                dblCSy = typPlateInfo.dblChipSizeYDir / typPlateInfo.intBlockCntYDir
            End If

            ' ###128
            If (((dblCSx <= (0.6 + 0.2)) And (dblCSy <= 0.3 + 0.1)) Or _
                    ((dblCSx <= (0.3 + 0.1)) And (dblCSy <= 0.6 + 0.2))) Then
                Call SetLoaderIO(LOUT_STB, &H0)                         ' ローダ出力(ON=品種データ送信(STB), OFF=なし)
            Else
                Call SetLoaderIO(&H0, LOUT_STB)                         ' ローダ出力(ON=なし, OFF=品種データ送信(STB))
            End If

            '' 連続トリミングＮＧ枚数上限セット
            'ReDim iData(0)
            'iData(0) = iTrimNgMAX
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4004", 1, iData) ' D1004 連続ﾄﾘﾐﾝｸﾞNG枚数上限

            '' トリミングＮＧカウンタ(上限)セット
            'iData(0) = iTrimNgCount
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4016", 1, iData)

            '' 割欠け排出カウンタ(上限)セット
            'iData(0) = iTrimNgBoxCnt
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4015", 1, iData)

            '' 割れ欠け排出ｶｳﾝﾀ(M4003)/NG排出ｶｳﾝﾀ(M4004)ｸﾘｱ
            'iData(0) = &H8
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "M4000", 1, iData)
            'iData(0) = &H10
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "M4000", 1, iData)

            'Call System.Threading.Thread.Sleep(500)                     ' Wait(msec)
            'iData(0) = &H0
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "M4000", 1, iData)
            'iData(0) = &H0
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "M4000", 1, iData)

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_TrimPreStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダからのトリミングスタート待ち処理(トリミング実行時用)"
    '''=========================================================================
    ''' <summary>ローダからのトリミングスタート待ち処理(トリミング実行時用)</summary>
    ''' <remarks>ローダへ基板要求を送信し、ローダからのトリミングスタート信号を待つ</remarks>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <param name="bFgAutoMode">  (INP)ローダ自動モードフラグ</param>
    ''' <param name="iTrimResult">  (INP)トリミング結果(前回)
    '''                                   cFRS_NORMAL   = 正常
    '''                                   cFRS_TRIM_NG  = トリミングNG
    '''                                   cFRS_ERR_PTN  = パターン認識エラー</param>
    ''' <param name="bFgMagagin">   (OUT)マガジン終了フラグ</param>
    ''' <param name="bFgAllMagagin">(OUT)全マガジン終了フラグ</param>
    ''' <param name="bFgLot">       (OUT)ロット切替え要求フラグ</param>
    ''' <param name="bIniFlg">      (INP)初期フラグ(0=初回, 1=トリミング中,
    '''                                             2=全マガジン終了(未使用), 3=最終基板の取出)
    '''                                                                                        </param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    '''          cFRS_ERR_RST  = RESETキー押下
    '''          cFRS_ERR_CVR  = 筐体カバー開検出
    '''          cFRS_ERR_SCVR = スライドカバー開検出
    '''          cFRS_ERR_EMG  = 非常停止 他</returns> 
    '''=========================================================================
    Public Function Loader_WaitTrimStart(ByVal ObjSys As SystemNET, ByVal bFgAutoMode As Boolean, ByVal iTrimResult As Integer, _
                                         ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean, ByVal bIniFlg As Integer) As Integer 'V6.0.0.0�Q
        'Public Function Loader_WaitTrimStart(ByVal ObjSys As Object, ByVal bFgAutoMode As Boolean, ByVal iTrimResult As Integer, _
        '                                     ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean, ByVal bIniFlg As Integer) As Integer

        Dim Idx As Integer
        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim OnBit As UShort
        Dim OffBit As UShort
        Dim WaitBit As UShort
        Dim strMSG As String

        Try
            ' ローダ無効またはローダ手動モードならNOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   初回以外なら基板排出処理を行う
            '-------------------------------------------------------------------
            If (bIniFlg <> 0) Then                                      ' 初回以外 ?
                rtnCode = Loader_WaitDischarge(ObjSys, bFgAutoMode, iTrimResult, bFgMagagin, bFgAllMagagin, bFgLot)
                '----- V4.0.0.0�R↓ローム殿特注(SL436R/SL436S) -----
                If (rtnCode = cFRS_ERR_START) Then                      ' サイクル停止で基板なし続行指定 ?
                    GoTo STP_010                                        '  
                End If
                '----- V4.0.0.0�R↑ -----
                If (rtnCode <> cFRS_NORMAL) Then                        ' エラー ? (※エラー発生時のメッセージは表示済)
                    Return (rtnCode)
                End If
                If (bIniFlg = 2) Then                                   ' 全マガジン終了なら終了
                    Return (rtnCode)
                End If
            End If
STP_010:    ' V4.0.0.0�R
            ' 前回のトリミングスタート要求Bit/排出ピック完了BitのOffを待つ
            If (bIniFlg = 0) Then                                       ' 初回 ?
                '----- V4.0.0.0-26↓ -----
                ' 品種番号を送出する(初回のみ)
                r = W_SubstrateType(1)
                '----- V4.0.0.0-26↑ -----
                WaitBit = LINP_TRM_START
                OnBit = LOUT_SUPLY + LOUT_STOP                          ' ローダ出力BIT = 基板要求+トリマ部停止中
                '###148 OffBit = LOUT_DISCHRAGE                         ' OffBit = 供給位置決完了
                OffBit = LOUT_DISCHRAGE + LOUT_PROC_CONTINUE            ' OffBit = 供給位置決完了
            Else
                WaitBit = LINP_TRM_START
                OnBit = LOUT_DISCHRAGE + LOUT_STOP                      ' ローダ出力BIT = 供給位置決完了+トリマ部停止中
                '###148 OffBit = LOUT_SUPLY                             ' OffBit = 基板要求
                OffBit = LOUT_SUPLY + LOUT_PROC_CONTINUE                ' OffBit = 基板要求
            End If
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(ObjSys, WaitBit, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            If (rtnCode <> cFRS_NORMAL) Then                            ' エラー ? (※エラー発生時のメッセージは表示済)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' 軽故障(続行可能) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' アラームリセット信号送出 
                    Call W_START()                                      ' スタート信号送出 
                    GoTo STP_RETRY
                End If
                Return (rtnCode)
            End If

            '-------------------------------------------------------------------
            '   テーブルを基板供給位置に移動する
            '-------------------------------------------------------------------
            If (bIniFlg <> 3) Then                                      ' 最終基板の取出ならNOP V2.0.0.0�R
                Idx = typPlateInfo.intWorkSetByLoader - 1               ' Idx = 基板品種番号 - 1
                If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then 'V5.0.0.9�G
                    ' TODO: 条件検討
                    ROUND4(gdClampLessTheta)    'V5.0.0.9�G  0.0からgdClampLessThetaに変更
                    'V6.0.2.0�E↓
                ElseIf (0 <> gSysPrm.stDEV.giTheta) AndAlso (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    'ステージへの基板供給時には、角度指定を使用できるようにする
                    ROUND4(gdClampLessTheta)    'V5.0.0.9�G  gdClampLessThetaに変更
                    'V6.0.2.0�E↑
                End If
                r = SMOVE2(gfBordTableInPosX(Idx), gfBordTableInPosY(Idx))
                If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                    rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' メッセージ表示
                    Return (rtnCode)
                End If
            End If                                                      ' V2.0.0.0�R

            '-------------------------------------------------------------------
            '   NG排出BOX満杯ならメッセージを表示する ###089
            '-------------------------------------------------------------------
            'If (iTrimResult <> cFRS_NORMAL) Then                        ' トリミング結果(前回)が正常でなければ
            '    giNgBoxCounter = giNgBoxCounter + 1                     ' NG排出BOXの収納枚数カウンターを+ 1する
            '    DspNGTrayCount()    '###130
            'End If###148
            ' ###130
            r = JudgeNGtrayCount(Idx)                                   ' ###181 2013.01.24
            If (r < cFRS_NORMAL) Then Return (r) '                      ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 

            '' NG排出BOX満杯 ?
            'If (giNgBoxCounter > giNgBoxCount(Idx)) Then                ' NG排出BOXの収納枚数カウンター > NG排出BOXの収納枚数 ?
            '    giAppMode = APP_MODE_LDR_ALRM                           ' アプリモード = ローダアラーム画面(カバー開のエラーとしない為)
            '    Call COVERCHK_ONOFF(COVER_CHECK_OFF)                    ' 「固定カバー開チェックなし」にする ###088

            '    ' "ＮＧ排出ボックス満杯","ＮＧ排出ボックスからＮＧ基板を取り除いてから","STARTキーを押すか、OKボタンを押して下さい。"
            '    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
            '            MSG_LOADER_21, MSG_LOADER_20, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            '    If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 

            '    giAppMode = APP_MODE_TRIM                               ' アプリモード = トリミング中
            '    Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア ###088
            '    Call COVERCHK_ONOFF(COVER_CHECK_ON)                     ' 「固定カバー開チェックあり」にする ###088
            '    giNgBoxCounter = 0                                      ' 「NG排出BOXの収納枚数カウンター」を初期化する
            'End If

            '-------------------------------------------------------------------
            '   初回    →基板要求信号を送信し、トリミングスタート要求を待つ
            '   初回以外→供給位置決完了信号を送信し、トリミングスタート要求を待つ
            '-------------------------------------------------------------------
            ' ローダへ基板要求信号(初回)または供給位置決完了(初回以外)を送信する (トリミングNG, パターン認識エラーは基板要求信号と同時に出力する)
            OffBit = OffBit + LOUT_NG_DISCHRAGE + LOUT_TRM_NG           ' 「ＮＧ基板排出要求」/ [トリミングNG](BITをOFFする) ###089
            If (iTrimResult <> cFRS_NORMAL) Then                        ' 基板単位のトリミング結果(前回)が正常でなければ
                OnBit = OnBit + LOUT_NG_DISCHRAGE                       ' 「ＮＧ基板排出要求」BITをONする
            End If
            If (iTrimResult = cFRS_TRIM_NG) Then                        ' 基板単位のトリミング結果(前回) = トリミングNG ?
                OnBit = OnBit + LOUT_TRM_NG
            ElseIf (iTrimResult = cFRS_ERR_PTN) Then                    ' 基板単位のトリミング結果(前回) = パターン認識エラー ?
                'OnBit = OnBit + LOUT_PTN_NG                            ' ###070
                OnBit = OnBit + LOUT_TRM_NG                             ' ###070
            End If
            OffBit = OffBit And Not LOUT_SUPLY                          ' 基板要求(連続運転開始)はOFFしない  
            Call SetLoaderIO(OnBit, OffBit)                             ' ローダ出力(ON=基板要求または供給位置決完了+ﾄﾘﾏ停止中+他, OFF=供給位置決完了または基板要求)

            ' ローダからのトリミングスタート要求を待つ
STP_RETRY2:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_TRM_START, True, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            'If (rtnCode = cFRS_ERR_LDR3) Then                           ' 軽故障(続行可能) ? ###196 ###073
            If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                Call W_RESET()                                          ' アラームリセット信号送出 
                Call W_START()                                          ' スタート信号送出 
                GoTo STP_RETRY2
            End If
            If (rtnCode = cFRS_NORMAL) Then                             ' 正常 ? (※エラー発生時のメッセージは表示済)
                ' ローダへトリマ動作中(ﾄﾘﾏ停止OFF)を送信する  
                OnBit = OnBit And Not LOUT_SUPLY                        ' 基板要求(連続運転開始)はOFFしない  
                Call SetLoaderIO(&H0, OnBit)                            ' ローダ出力(ON=なし, OFF=基板要求+ﾄﾘﾏ停止中+他)
            End If
            ' ###130
            r = JudgeNGtrayCount(Idx)
            If (r < cFRS_NORMAL) Then Return (r) '                      ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み)  ' ###181 2013.01.24

            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_WaitTrimStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダからの基板排出完了待ち処理(トリミング実行時用)"
    '''=========================================================================
    ''' <summary>ローダからの基板排出完了待ち処理(トリミング実行時用)</summary>
    ''' <remarks>ローダへ基板排出要求を送信し、ローダからの基板排出完了信号を待つ</remarks>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <param name="bFgAutoMode">  (INP)ローダ自動モードフラグ</param>
    ''' <param name="iTrimResult">  (INP)トリミング結果(前回)
    '''                                   cFRS_NORMAL   = 正常
    '''                                   cFRS_TRIM_NG  = トリミングNG
    '''                                   cFRS_ERR_PTN  = パターン認識エラー</param>
    ''' <param name="bFgMagagin">   (OUT)マガジン終了フラグ</param>
    ''' <param name="bFgAllMagagin">(OUT)全マガジン終了フラグ</param>
    ''' <param name="bFgLot">       (OUT)ロット切替え要求フラグ</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_START= 正常(サイクル停止で基板なし続行指定) V4.0.0.0�R
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    '''          cFRS_ERR_RST  = RESETキー押下
    '''          cFRS_ERR_CVR  = 筐体カバー開検出
    '''          cFRS_ERR_SCVR = スライドカバー開検出
    '''          cFRS_ERR_EMG  = 非常停止 他</returns> 
    '''=========================================================================
    Public Function Loader_WaitDischarge(ByVal ObjSys As SystemNET, ByVal bFgAutoMode As Boolean, ByVal iTrimResult As Integer, _
                                         ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean) As Integer 'V6.0.0.0�Q
        'Public Function Loader_WaitDischarge(ByVal ObjSys As Object, ByVal bFgAutoMode As Boolean, ByVal iTrimResult As Integer, _
        '                                     ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean) As Integer

        Dim Idx As Integer
        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim OnBit As UShort
        Dim OffBit As UShort        ' '###127
        Dim strMSG As String

        Try
            ' ローダ無効またはローダ手動モードならNOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If
            ' ###148
            ' NGトレイに置いていたら判定を行う
            r = DspNGTrayChk()

            ' 前回の排出ピック完了BitのOffを待つ
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_DISCHRAGE, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            If (rtnCode <> cFRS_NORMAL) Then                            ' エラー ? (※エラー発生時のメッセージは表示済)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' 軽故障(続行可能) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' アラームリセット信号送出 
                    Call W_START()                                      ' スタート信号送出 
                    GoTo STP_RETRY
                End If
                Return (rtnCode)
            End If
            'V4.11.0.0�G
            'V4.9.0.0�@↓ 'High,Lowの率が悪くなった場合の停止画面用
            'V4.10.0.0�I            If gKeiTyp = KEY_TYPE_RS Then                                   ' シンプルトリマの場合
            If (gMachineType = MACHINE_TYPE_436S) Then                                              ' シンプルトリマの場合
                'V4.9.0.0�@
                If giNgStop = 1 Then
                    If JudgeNgRate.CheckTimmingPlate = True Then
                        r = sub_JudgeLotStop()
                        If r = cFRS_ERR_RST Then
                            ' ここは画面で中断がおされたときのみ
                            iTrimResult = cFRS_TRIM_NG
                        ElseIf r = cFRS_ERR_LDRTO Then
                            Return r
                            'V5.0.0.1-31↓
                        ElseIf r = cFRS_ERR_LDR Then
                            Return r
                            'V5.0.0.1-31↑
                        End If
                    End If
                End If
            End If
            'V4.9.0.0�@↑'V4.11.0.0�G

            '-------------------------------------------------------------------
            '   テーブルを基板排出位置に移動する
            '-------------------------------------------------------------------
            Idx = typPlateInfo.intWorkSetByLoader - 1                   ' Idx = 基板品種番号 - 1
            If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then 'V5.0.0.9�G
                'V6.0.5.0�D↓
                'V4.12.2.0�D                ROUND4(gdCorrectTheta + gdClampLessTheta)               '5.0.1.0�H
                'V4.12.2.0�D                r = SMOVE2(gfBordTableOutPosX(Idx) + gfCorrectPosX - gdClampLessOffsetX,
                'V4.12.2.0�D                           gfBordTableOutPosY(Idx) + gfCorrectPosY - gdClampLessOffsetY) '#5.0.1.0�G
                'V6.1.2.0�C↓
                If giClampLessOutPos = 0 Then
                    ROUND4(gdClampLessTheta)                                    'V4.12.2.0�D 立山科学殿仕様によりクランプレス時の
                    r = SMOVE2(gfBordTableInPosX(Idx), gfBordTableInPosY(Idx))  'V4.12.2.0�D 排出位置を供給位置と同じにする
                Else
                    ROUND4(gdClampLessTheta)
                    r = SMOVE2(gfBordTableOutPosX(Idx) + gdClampLessOffsetX, gfBordTableOutPosY(Idx) + gdClampLessOffsetY)
                End If
                'V6.0.5.0�D↑
                'V6.1.2.0�C↑
            Else
                'V6.0.2.0�E↓
                If (0 <> gSysPrm.stDEV.giTheta) AndAlso (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    'SL436Rでθ有の場合には、指定角度に回転する
                    'V6.0.5.0�D'                    ROUND4(gdCorrectTheta + gdClampLessTheta)
                    ROUND4(gdClampLessTheta)       'V6.0.5.0�D
                End If
                'V6.0.2.0�E↑

                r = SMOVE2(gfBordTableOutPosX(Idx), gfBordTableOutPosY(Idx))
                End If
                If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' メッセージ表示
                Return (rtnCode)
            End If


            ''V4.9.0.0�@↓ 'High,Lowの率が悪くなった場合の停止画面用'V4.11.0.0�G
            ''V4.10.0.0�I            If gKeiTyp = KEY_TYPE_RS Then                                   ' シンプルトリマの場合
            'If (gMachineType = MACHINE_TYPE_436S) Then                                              ' シンプルトリマの場合
            '    'V4.9.0.0�@
            '    If giNgStop = 1 Then
            '        If JudgeNgRate.CheckTimmingPlate = True Then
            '            r = sub_JudgeLotStop()
            '            If r = cFRS_ERR_RST Then
            '                ' ここは画面で中断がおされたときのみ
            '                iTrimResult = cFRS_TRIM_NG
            '            End If
            '        End If
            '    End If
            'End If
            ''V4.9.0.0�@↑'V4.11.0.0�G

            '----- V4.0.0.0�R↓ -----
            '-------------------------------------------------------------------
            '   サイクル停止処理(ローム殿特注(SL436R/SL436S))
            '-------------------------------------------------------------------
            If (bFgCyclStp = True) Then                                 ' サイクル停止フラグ ON ?
                r = CycleStop_Proc(ObjSys)                              ' サイクル停止処理
                Call LAMP_CTRL(LAMP_HALT, False)                        ' サイクル停止処理が終わったらHALTランプは消灯する
                bFgCyclStp = False              ' サイクル停止フラグOFF
                Form1.btnCycleStop.Text = "CYCLE STOP OFF"                          'V5.0.0.4�@
                Form1.btnCycleStop.BackColor = System.Drawing.SystemColors.Control  'V5.0.0.4�@
                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                If ((r = cFRS_ERR_START) Or (r = cFRS_ERR_RST)) Then    ' 基板なし続行またはCancel(RESETキー押下) ?
                    Return (r)                                          ' Return値 = cFRS_ERR_START(基板なし続行), cFRS_ERR_RST(Cancel(RESETキー押下))
                End If
            End If                                                      ' r = cFRS_NORMAL(基板あり続行)なら処理続行
            '----- V4.0.0.0�R↑ -----

            '-------------------------------------------------------------------
            '   基板要求信号(初回以外)を送信し、排出ピック完了を待つ
            '-------------------------------------------------------------------
            ' 基板要求信号を送信する前にローダ部停止中BitのONを待つ
STP_RETRY2:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_STOP, True, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            If (rtnCode <> cFRS_NORMAL) Then                            ' エラー ? (※エラー発生時のメッセージは表示済)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' 軽故障(続行可能) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' アラームリセット信号送出 
                    Call W_START()                                      ' スタート信号送出 
                    GoTo STP_RETRY2
                End If
                Return (rtnCode)
            End If

            ' ローダへ基板要求信号(初回以外)を送信する (トリミングNG, パターン認識エラーは基板要求信号と同時に出力する)
            'OnBit = LOUT_SUPLY + LOUT_STOP                             ' ローダ出力BIT = 基板要求+トリマ部停止中
            '###127　↓
            If (bFgAllMagagin = True) Then
                OnBit = LOUT_REQ_COLECT + LOUT_STOP                     ' ローダ出力BIT = 基板回収要求+トリマ部停止中
                OffBit = LOUT_SUPLY
            Else
                OnBit = LOUT_SUPLY + LOUT_REQ_COLECT + LOUT_STOP        ' ローダ出力BIT = 基板要求+基板回収要求+トリマ部停止中
                OffBit = 0
            End If
            If (iTrimResult <> cFRS_NORMAL) Then                        ' 基板単位のトリミング結果(前回)が正常でなければ
                OnBit = OnBit + LOUT_NG_DISCHRAGE                       ' 「ＮＧ基板排出要求」BITをONする
            End If
            If (iTrimResult = cFRS_TRIM_NG) Then                        ' 基板単位のトリミング結果(前回) = トリミングNG ?
                OnBit = OnBit + LOUT_TRM_NG
            ElseIf (iTrimResult = cFRS_ERR_PTN) Then                    ' 基板単位のトリミング結果(前回) = パターン認識エラー ?
                'OnBit = OnBit + LOUT_PTN_NG                            ' ###070
                OnBit = OnBit + LOUT_TRM_NG                             ' ###070
            End If
            Call SetLoaderIO(OnBit, OffBit)                             ' ローダ出力(ON=基板要求+ﾄﾘﾏ停止中+他, OFF=なし)
            '###127  ↑          Call SetLoaderIO(OnBit, &H0)                                ' ローダ出力(ON=基板要求+ﾄﾘﾏ停止中+他, OFF=なし)

            ' ローダからの排出ピック完了を待つ
STP_RETRY3:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_DISCHRAGE, True, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            'If (rtnCode = cFRS_ERR_LDR3) Then                           ' 軽故障(続行可能) ? ###196 ###073
            If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                Call W_RESET()                                          ' アラームリセット信号送出 
                Call W_START()                                          ' スタート信号送出 
                GoTo STP_RETRY3
            End If
            If (rtnCode = cFRS_NORMAL) Then                             ' 正常 ? (※エラー発生時のメッセージは表示済)
                ' ローダへトリマ動作中(ﾄﾘﾏ停止OFF)を送信する  
                OnBit = OnBit And Not LOUT_SUPLY                        ' 基板要求(連続運転開始)はOFFしない  
                Call SetLoaderIO(&H0, OnBit)                            ' ローダ出力(ON=なし, OFF=基板要求+ﾄﾘﾏ停止中+他)
            End If
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_WaitDischarge() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダからのマガジン終了,全マガジン終了,ロット切替え要求チェック"
    '''=========================================================================
    ''' <summary>ローダからのマガジン終了,全マガジン終了,ロット切替え要求チェック</summary>
    ''' <param name="bFgMagagin">   (OUT)マガジン終了フラグ</param>
    ''' <param name="bFgAllMagagin">(OUT)全マガジン終了フラグ</param>
    ''' <param name="bFgLot">       (OUT)ロット切替え要求フラグ</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    '''          cFRS_ERR_RST  = RESETキー押下
    '''          cFRS_ERR_CVR  = 筐体カバー開検出
    '''          cFRS_ERR_SCVR = スライドカバー開検出
    '''          cFRS_ERR_EMG  = 非常停止 他</returns> 
    '''=========================================================================
    Private Function LoaderBitCheck(ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean) As Integer

        Dim r As Integer
        Dim LdIn As UShort
        Dim strMSG As String

        Try
            ' 全マガジン終了チェック
            r = GetLoaderIO(LdIn)                                       ' ローダ入力
            If (LdIn And LINP_END_ALL_MAGAZINE) Then                    ' 全マガジン終了？
                bFgAllMagagin = True
            Else
                bFgAllMagagin = False
            End If

            ' マガジン終了チェック
            If (bFgMagagin = True) Then                                 ' マガジン終了フラグON ?
                ' 前回のマガジン終了BitのOffを待つ
STP_RETRY:
                r = Sub_WaitLoaderData(Form1.System1, LINP_END_MAGAZINE, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (※エラー発生時のメッセージは表示済)
                    'If (r = cFRS_ERR_LDR3) Then                         ' 軽故障(続行可能) ? ###196 ###073
                    If (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDR2) Then  ' ###196
                        Call W_RESET()                                  ' アラームリセット信号送出 
                        Call W_START()                                  ' スタート信号送出 
                        GoTo STP_RETRY
                    End If
                    Return (r)
                End If
            End If
            r = GetLoaderIO(LdIn)                                       ' ローダ入力
            If (LdIn And LINP_END_MAGAZINE) Then                        ' マガジン終了？
                bFgMagagin = True
            Else
                bFgMagagin = False
            End If

            ' ロット切替え要求チェック  
            If (bFgLot = True) Then                                     ' ロット切替え要求フラグON ?
                ' 前回のロット切替えBitのOffを待つ
STP_RETRY2:
                r = Sub_WaitLoaderData(Form1.System1, LINP_LOT_CHG, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (※エラー発生時のメッセージは表示済)
                    'If (r = cFRS_ERR_LDR3) Then                         ' 軽故障(続行可能) ? ###196 ###073
                    If (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDR2) Then  ' ###196
                        Call W_RESET()                                  ' アラームリセット信号送出 
                        Call W_START()                                  ' スタート信号送出 
                        GoTo STP_RETRY2
                    End If
                    Return (r)
                End If
            End If
            r = GetLoaderIO(LdIn)                                       ' ローダ入力
            If (LdIn And LINP_LOT_CHG) Then                             ' ロット切替え要求 ？
                bFgLot = True
            Else
                bFgLot = False
            End If

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.LoaderBitCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "自動運転終了メッセージ表示およびシグナルタワー制御(全マガジン終了,自動運転OFF)"
    '''=========================================================================
    ''' <summary>自動運転終了メッセージ表示(自動運転時)および
    '''          シグナルタワー制御(全マガジン終了,自動運転OFF)</summary>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <param name="bFgAutoMode">  (OUT)ローダ自動モードフラグ</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_EMG  = 非常停止</returns> 
    '''=========================================================================
    Public Function Loader_EndAutoDrive(ByVal ObjSys As SystemNET, ByRef bFgAutoMode As Boolean) As Integer 'V6.0.0.0�Q 
        'V6.0.0.0�Q    Public Function Loader_EndAutoDrive(ByVal ObjSys As Object, ByRef bFgAutoMode As Boolean) As Integer

        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' シグナルタワー制御
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R系 ? 
                ' SL436R系時
                ' シグナルタワー制御(On=全マガジン終了, Off=全ﾋﾞｯﾄ)
                If (bFgAutoMode = True) And (giErrLoader = 0) Then      ' ###073 ###064

                    'V5.0.0.9�M ↓　V6.0.3.0�G
                    ''----- V4.0.0.0-25↓ -----
                    '' 全マガジン終了のシグナルタワー制御
                    'If (giMachineKd = MACHINE_KD_RS) Then
                    '    ' シグナルタワー制御(On=緑点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436S時
                    '    Call Form1.System1.SetSignalTower(SIGOUT_GRN_BLK Or SIGOUT_BZ1_ON, &HFFFF)

                    'Else
                    '    ' シグナルタワー制御(On=赤点滅+ブザー１, Off=全ﾋﾞｯﾄ) SL436R時
                    '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    'End If

                    ''Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)  ' 標準((赤点滅+ブザー１)) ###007
                    ''----- V4.0.0.0-25↑ -----
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)
                    'V5.0.0.9�M ↑　V6.0.3.0�G

                End If

            Else
                ' SL432R系時

            End If

            ' 自動運転終了メッセージ表示(自動運転時)
            If (bFgAutoMode = True) And (giErrLoader = 0) Then          ' ###073
                r = Sub_CallFrmRset(ObjSys, cGMODE_LDR_END)             ' 自動運転終了メッセージ表示 & STARTキー待ち
                'V5.0.0.9�M ↓　V6.0.3.0�G
                'Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)  ' 赤点滅+ブザー１OFF  V4.0.0.0-25 ###159
                '                Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON Or SIGOUT_GRN_BLK)     ' V4.0.0.0-25
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                'V5.0.0.9�M ↑　V6.0.3.0�G

                If (r < cFRS_NORMAL) Then                               ' STARTキー押下以外 ?
                    rtnCode = r                                         ' Return値設定 
                End If
            End If

            ' 終了処理
STP_END:
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_EndAutoDrive() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダ部タイマー値読込み処理"
    '''=========================================================================
    ''' <summary>ローダ部タイマー値読込み処理</summary>
    ''' <param name="LTimerDT">(OUT)タイマー値読込み域</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_GetTimerValue(ByRef LTimerDT() As UShort) As Integer

        'Dim iData() As UShort
        Dim strMSG As String

        Try
            If (bFgActLink = False) Then                            ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            '' D4032 薄物押付供給ハンド1回目下降　→　吸着OFF
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4032", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4033 薄物押付供給ハンド吸着OFF　→　載物台クランプ
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4033", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4034 薄物押付載物台クランプ　→　供給ハンド上昇
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4034", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' 未 'D4035 薄物押付供給ハンド上昇　→　供給ハンド2回目下降
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4035", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4036 薄物押付供給ハンド2回目下降　→　載物台クランプ解除
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4036", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4037 薄物押付載物台クランプ開　→　通常下降
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4036", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4038　供給ハンド通常下降　→　フラグ処理及び吸着OFF
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4038", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4039　供給ハンド通常吸着OFF　→　供給ハンド上昇
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4039", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4040　下限ON直後、薄物押付け時の強制破壊
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4040", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4041　供給ハンド真空破壊時間
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4041", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4042　供給ハンド真空破壊時間
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4042", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4045　載物台クランプON　→　吸着ON
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4045", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4046　載物台吸着ON　→　割欠／2枚取りの以上検出待ち
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4046", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4049　収納ハンド下降　→　載物台クランプ解除
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4049", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4050　載物台クランプ解除　→　吸着OFF
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4050", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4051　載物台吸着OFF　→　収納ハンド上昇
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4051", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4052　載物台クランプ解除　→　収納ハンド上昇
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4052", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4053　供給マガジンエアブロー時間
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4053", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' 未 'D4055　供給ハンド上限　→　吸着ミスの異常検出待ち
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4055", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' 未 'D4056　載物台吸着ON　→　吸着ミスの異常検出待ち
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4056", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' 未 'D4057　収納ハンド上限　→　吸着ミスの異常検出待ち
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4057", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4058　収納エレベータ下降時間
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4058", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_GetTimerValue() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダ部タイマー値書込み処理"
    '''=========================================================================
    ''' <summary>ローダ部タイマー値書込み処理</summary>
    ''' <param name="LTimerDT">(INP)タイマー値</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_SetTimerValue(ByRef LTimerDT() As UShort) As Integer

        'Dim iData() As UShort
        'Dim i As Integer
        Dim strMSG As String

        Try
            If (bFgActLink = False) Then                            ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            '' D4032 薄物押付供給ハンド1回目下降　→　吸着OFF
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4032", 1, iData)
            'i = i + 1
            '' D4033 薄物押付供給ハンド吸着OFF　→　載物台クランプ
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4033", 1, iData)
            'i = i + 1
            '' D4034 薄物押付載物台クランプ　→　供給ハンド上昇
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4034", 1, iData)
            'i = i + 1
            '' 未 'D4035 薄物押付供給ハンド上昇　→　供給ハンド2回目下降
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4035", 1, iData)
            'i = i + 1
            '' D4036 薄物押付供給ハンド2回目下降　→　載物台クランプ解除
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4036", 1, iData)
            'i = i + 1
            '' D4037 薄物押付載物台クランプ開　→　通常下降
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4037", 1, iData)
            'i = i + 1
            '' D4038　供給ハンド通常下降　→　フラグ処理及び吸着OFF
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4038", 1, iData)
            'i = i + 1
            '' D4039　供給ハンド通常吸着OFF　→　供給ハンド上昇
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4039", 1, iData)
            'i = i + 1
            '' D4040　下限ON直後、薄物押付け時の強制破壊
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4040", 1, iData)
            'i = i + 1
            '' D4041　供給ハンド真空破壊時間        ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4041", 1, iData)
            'i = i + 1
            '' D4042　供給ハンド真空破壊時間
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4042", 1, iData)
            'i = i + 1
            '' D4045　載物台クランプON　→　吸着ON
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4045", 1, iData)
            'i = i + 1
            '' D4046　載物台吸着ON　→　割欠／2枚取りの以上検出待ち
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4046", 1, iData)
            'i = i + 1
            '' D4049　収納ハンド下降　→　載物台クランプ解除
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4049", 1, iData)
            'i = i + 1
            '' D4050　載物台クランプ解除　→　吸着OFF
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4050", 1, iData)
            'i = i + 1
            '' D4051　載物台吸着OFF　→　収納ハンド上昇
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4051", 1, iData)
            'i = i + 1
            '' D4052　載物台クランプ解除　→　収納ハンド上昇
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4052", 1, iData)
            'i = i + 1
            '' D4053　供給マガジンエアブロー時間
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4053", 1, iData)
            'i = i + 1
            '' 未 'D4055　供給ハンド上限　→　吸着ミスの異常検出待ち
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4055", 1, iData)
            'i = i + 1
            '' 未 'D4056　載物台吸着ON　→　吸着ミスの異常検出待ち
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4056", 1, iData)
            'i = i + 1
            '' 未 'D4057　収納ハンド上限　→　吸着ミスの異常検出待ち
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4057", 1, iData)
            'i = i + 1
            '' D4058　収納エレベータ下降時間
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4058", 1, iData)
            'i = i + 1

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_SetTimerValue() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダバージョン取得"
    '''=========================================================================
    ''' <summary>ローダバージョン取得</summary>
    ''' <param name="LoaderVer">(OUT)ローダバージョン</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_GetVersion(ByRef LoaderVer As String) As Integer

        'Dim iData() As UShort
        Dim strMSG As String

        Try
            ' ローダバージョン取得
            If (bFgActLink = False) Then                            ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D5030", 1, iData)
            'LoaderVer = Format(iData(0) / 100, "0.00")
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_GetVersion() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '===============================================================================
    '   共通関数
    '===============================================================================
#Region "ローダからの応答データ待ち"
    '''=========================================================================
    ''' <summary>ローダからの応答データ待ち</summary>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <param name="WaitData">     (INP)応答待ちするデータ</param>
    ''' <param name="OnOff">        (INP)True=On待ち, False=Off待ち</param>
    ''' <param name="bReset">       (INP)RESTキーの有効/無効</param>
    ''' <param name="bOrgFlg">      (INP)原点復帰フラグ</param>
    ''' <param name="bFgMagagin">   (OUT)マガジン終了フラグ</param>
    ''' <param name="bFgAllMagagin">(OUT)全マガジン終了フラグ</param>
    ''' <param name="bFgLot">       (OUT)ロット切替え要求フラグ</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    '''          cFRS_ERR_RST  = RESETキー押下
    '''          cFRS_ERR_CVR  = 筐体カバー開検出
    '''          cFRS_ERR_SCVR = スライドカバー開検出
    '''          cFRS_ERR_EMG  = 非常停止 他
    ''' </returns> 
    '''=========================================================================
    Private Function Sub_WaitLoaderData(ByVal ObjSys As SystemNET, ByVal WaitData As UShort, ByVal OnOff As Boolean, ByVal bReset As Boolean, ByVal bOrgFlg As Boolean, _
                                        ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean) As Integer 'V6.0.0.0�Q
        'Private Function Sub_WaitLoaderData(ByVal ObjSys As Object, ByVal WaitData As UShort, ByVal OnOff As Boolean, ByVal bReset As Boolean, ByVal bOrgFlg As Boolean, _
        '                                    ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean) As Integer

        Dim TimerRS As System.Threading.Timer = Nothing
        Dim LdIn As UShort
        Dim WaitBit As UShort
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer
        Dim strMSG As String
        Dim BreakFirst As Integer '###130
        Dim TwoTakeFirst As Integer '###130
        Dim bFlgWbrk As Boolean = False                                 ' タイマーリセットフラグ 'V4.5.0.0�@
        Dim bFlg2Pce As Boolean = False                                 ' タイマーリセットフラグ 'V4.5.0.0�@
        Dim iCnt As Integer 'V1.25.0.2�E


        Try
            ' 応答待ちするデータはOn/Off待ち ?
            If (OnOff = True) Then
                WaitBit = WaitData                                      ' On待ち
            Else
                WaitBit = 0                                             ' Off待ち
            End If
            BreakFirst = 0 '###130
            TwoTakeFirst = 0    '###130

            ' ローダ通信タイムアウトチェック用タイマーオブジェクトの作成(TimerRS_TickをX msec間隔で実行する)
            Sub_SetTimeoutTimer(TimerRS)

            ' ローダからの応答データを待つ
            Do
                ' ローダアラーム/非常停止チェック
                r = GetLoaderIO(LdIn)                                   ' ローダ
                If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                    rtnCode = cFRS_ERR_LDR                              ' Return値 = ローダアラーム検出
                    GoTo STP_ERR_LDR                                    ' ローダアラーム表示へ
                End If

                '----- 'V4.5.0.0�@↓ -----
                '---------------------------------------------------------------
                '   基板割れ検出チェック
                '---------------------------------------------------------------
                If ((LdIn And LINP_WBREAK) = LINP_WBREAK) Then          ' 基板割れ検出 ?
                    ' 基板割れ検出あり
                    If (bFlgWbrk = False) Then                          ' 検出なしから検出ありとなったらタイマーをリセットする 
                        bFlgWbrk = True                                 ' タイマーリセットFLG ON
                    Else                                                ' 検出ありで検出ありのままならタイムアウト監視を続ける
                        bFlgWbrk = False                                ' タイマーリセットFLG OFF
                    End If
                Else
                    ' 基板割れ検出なし
                    bFlgWbrk = False                                    ' タイマーリセットFLG OFF
                End If

                '---------------------------------------------------------------
                '   ２枚取り検出チェックチェック
                '---------------------------------------------------------------
                If ((LdIn And LINP_2PIECES) = LINP_2PIECES) Then        ' ２枚取り検出 ?
                    ' ２枚取り検出あり
                    If (bFlg2Pce = False) Then                          ' 検出なしから検出ありとなったらタイマーをリセットする 
                        bFlg2Pce = True                                 ' タイマーリセットFLG ON
                    Else                                                ' 検出ありで検出ありのままならタイムアウト監視を続ける
                        bFlg2Pce = False                                ' タイマーリセットFLG OFF
                    End If
                Else
                    ' ２枚取り検出なし
                    bFlg2Pce = False                                    ' タイマーリセットFLG OFF
                End If
                '----- 'V4.5.0.0�@↑ -----

                ' ローダ通信タイムアウトチェック 
                If (bFgTimeOut = True) Then                             ' タイムアウト ?
                    ' コールバックメソッドの呼出しを停止する
                    TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                    rtnCode = cFRS_ERR_LDRTO                            ' Return値 = ローダ通信タイムアウト
                    GoTo STP_ERR_LDR                                    ' エラーメッセージ表示へ
                End If

                ' RESETキー押下チェック
                If (bReset = True) Then                                 ' RESETキー有効 ? 
                    r = Sub_ResetSWCheck(ObjSys)
                    If (r <> cFRS_NORMAL) Then
                        rtnCode = r                                     ' Return値設定(cFRS_ERR_RST/cFRS_ERR_EMG)
                        GoTo STP_END
                    End If
                End If

                ' 筐体カバー閉/スライドカバー閉/非常停止チェック
                r = Check_CoverOpen(ObjSys, bOrgFlg)
                If (r <> cFRS_NORMAL) Then                              ' 筐体カバー閉/非常停止 ?(エラーメッセージは表示済) 
                    rtnCode = r                                         ' Return値設定
                    GoTo STP_END
                End If
                '###126
                ' トリミング要求ON待ちのときにオールマガジン終了で交換中信号がOFFした場合には、最後の基板が異常終了したと認識する。
                If (WaitData = LINP_TRM_START) And (OnOff = True) Then      ' トリミングスタート要求待ち ?
                    If (((LdIn And LINP_STOP) = LINP_STOP) And ((LdIn And LINP_END_ALL_MAGAZINE) = LINP_END_ALL_MAGAZINE)) Then
                        rtnCode = cFRS_ERR_LOTEND                       ' Return値設定(cFRS_ERR_RST/cFRS_ERR_EMG)
                        bFgAllMagagin = True                            ' V4.0.0.0�R
                        For iCnt = 1 To 3   'V1.25.0.2�E
                            '----- V1.16.0.0�J↓ -----
                            ' ローダアラーム/非常停止チェック
                            r = GetLoaderIO(LdIn)                           ' ローダ入力
                            If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                                Console.WriteLine("Sub_WaitLoaderData() ローダアラーム(LdIn=%f)", LdIn)
                                rtnCode = cFRS_ERR_LDR                      ' Return値 = ローダアラーム検出
                                GoTo STP_ERR_LDR                            ' ローダアラーム表示へ
                            End If
                            '----- V1.16.0.0�J↑ -----
                            Call System.Threading.Thread.Sleep(100)     'V1.25.0.2�E
                        Next                'V1.25.0.2�E
                        GoTo STP_END
                    End If

                    '-----'V4.5.0.0�@↓ -----
                    '-----------------------------------------------------------
                    '「トリミングスタート要求待ち」のときに「基板割れ検出」又は
                    '「２枚取り検出」なら ローダ通信タイムアウトタイマーを生成し直す
                    '-----------------------------------------------------------
                    If ((bFlgWbrk = True) Or (bFlg2Pce = True)) Then    ' タイマーリセットFLG ON ?
                        ' コールバックメソッドの呼出しを停止する
                        TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                        TimerRS.Dispose()                               ' タイマーを破棄する
                        Sub_SetTimeoutTimer(TimerRS)                    ' タイマー生成 
                    End If
                    '----- 'V4.5.0.0�@↑ -----

                    ''###130 
                    'If (((LdIn And LINP_2PIECES) = LINP_2PIECES)) Then ' B14: ２枚取り検出(0=２枚取り未検出, 1=２枚取り検出)　
                    '    If TwoTakeFirst = 0 Then   '###130
                    '        giTwoTakeCounter = giTwoTakeCounter + 1                                    ' ２枚取り基板のカウント
                    '        TwoTakeFirst = 1
                    '        DspNGTrayCount()    '###130
                    '    End If
                    'Else
                    '    TwoTakeFirst = 0
                    'End If
                    '###130 
                    'If (((LdIn And LINP_WBREAK) = LINP_WBREAK)) Then  ' B15: 基板割れ検出(0=基板割れ未検出, 1=基板割れ検出)　
                    '    If BreakFirst = 0 Then
                    '        BreakFirst = 1
                    '        giBreakCounter = giBreakCounter + 1                                      ' 割れ欠け基板のカウント
                    '        DspNGTrayCount()    '###130
                    '    End If
                    'Else
                    '    BreakFirst = 0
                    'End If
                End If

                '----- ###173↓ -----
                ' NGトレイに置いていたら判定を行う(原点復帰モード/ローダモード切替以外の時にチェックする
                If (WaitData <> LINP_ORG_BACK) And (WaitData <> LINP_AUTO) Then
                    r = DspNGTrayChk()                                  ' NGトレイに置いていたら判定を行う ###148
                    If (r = cFRS_ERR_LDRTO) Then
                        ' コールバックメソッドの呼出しを停止する
                        TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                        rtnCode = cFRS_ERR_LDRTO                        ' Return値 = ローダ通信タイムアウト
                        GoTo STP_ERR_LDR                                ' エラーメッセージ表示へ
                    End If
                End If
                '----- ###173↑ -----

                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While ((LdIn And WaitData) <> WaitBit)                 ' 応答データ待ち

            ' マガジン終了, 全マガジン終了, ロット切替えチェック
            If (WaitData = LINP_TRM_START) And (OnOff = True) Then      ' トリミングスタート要求待ち ?
                rtnCode = LoaderBitCheck(bFgMagagin, bFgAllMagagin, bFgLot)
            End If

            ' 終了処理
STP_END:
            ' コールバックメソッドの呼出しを停止する
            TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            TimerRS.Dispose()                                           ' タイマーを破棄する
            Return (rtnCode)

            ' ローダエラー発生時
STP_ERR_LDR:
            ' コールバックメソッドの呼出しを停止する
            TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            TimerRS.Dispose()                                           ' タイマーを破棄する
            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ローダ通信タイムアウト ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' エラーメッセージ表示
            Else
                ' ローダアラームメッセージ作成 & ローダアラーム画面表示
                rtnCode = Loader_AlarmCheck(ObjSys, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            End If
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_WaitLoaderData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダ通信タイムアウトチェック用タイマー作成"
    '''=========================================================================
    ''' <summary>ローダ通信タイムアウトチェック用タイマー作成</summary>
    ''' <param name="TimerRS">(OUT)タイマー</param>
    '''=========================================================================
    Private Sub Sub_SetTimeoutTimer(ByRef TimerRS As System.Threading.Timer)

        Dim TimeVal As Integer
        Dim strMSG As String

        Try
            ' タイマー値を設定する
            If (giOPLDTimeOutFlg = 0) Then                              ' ローダ通信タイムアウト検出無し ?
                TimeVal = System.Threading.Timeout.Infinite             ' タイマー値 = なし
            Else
                TimeVal = giOPLDTimeOut                                 ' タイマー値 = ローダ通信タイムアウト時間(msec)
            End If

            ' ローダ通信タイムアウトチェック用タイマーオブジェクトの作成(TimerRS_TickをTimeVal msec間隔で実行する)
            bFgTimeOut = False                                          ' ローダ通信タイムアウトフラグOFF
            TimerRS = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerRS_Tick), Nothing, TimeVal, TimeVal)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SetTimeoutTimer() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "タイマーイベント(指定タイマ間隔が経過した時に発生)"
    '''=========================================================================
    ''' <summary>タイマーイベント(指定タイマ間隔が経過した時に発生)</summary>
    ''' <param name="Sts">(INP)</param>
    '''=========================================================================
    Private Sub TimerRS_Tick(ByVal Sts As Object)

        Dim strMSG As String
        Dim r As Integer
        Dim lData As Long

        Try
            ' シリアルでの通信状態を取得する
            '   ダミーでデータを取得し戻り値を判定する。
            r = W_Read(LOFS_W110, lData)                               ' ローダアラーム状態取得(W110.08-10)
            If (r <> 0) Then
                'Error
            End If

            bFgTimeOut = True                                           ' ローダ通信タイムアウトフラグON
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.TimerRS_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "RESETキー押下チェック"
    '''=========================================================================
    ''' <summary>RESETキー押下チェック</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Private Function Sub_ResetSWCheck(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Private Function Sub_ResetSWCheck(ByVal ObjSys As Object) As Integer

        Dim sts As Long = 0                                         ' ###160
        Dim r As Long
        Dim strMSG As String

        Try
            ' RESETキー押下チェック
            '----- ###160↓ -----
            'Call ZCONRST()                                          ' コンソールキーラッチ解除
            'r = STARTRESET_SWCHECK(False, sts)                      ' RESET SW押下チェック
            'If (sts = cFRS_ERR_RST) Then                            ' RESETキー押下 ?
            '    Call LAMP_CTRL(LAMP_HALT, False)                    ' HALTランプOFF 
            '    Call LAMP_CTRL(LAMP_RESET, True)                    ' RESETランプON
            '    Call ObjSys.OperationLogging(gSysPrm, MSG_OPLOG_TRIMRES, "") ' 操作ログ出力("トリミング中RESET SW押下により停止")
            '    Return (cFRS_ERR_RST)                               ' ExitFlag = Cancel(RESETｷｰ)
            'End If
            '----- ###160↑ -----

            If (ObjSys.EmergencySwCheck()) Then                     ' 非常停止 ?
                r = cGMODE_EMG                                      ' Return値 = 非常停止検出
                GoTo STP_ERR
            End If
            Return (cFRS_NORMAL)

            ' エラーメッセージ表示
STP_ERR:
            r = Sub_CallFrmRset(ObjSys, r)                          ' エラーメッセージ表示
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_ResetSWCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "筐体カバー閉/スライドカバー閉チェック"
    '''=========================================================================
    ''' <summary>筐体カバー閉/スライドカバー閉チェック</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_CVR  = 筐体カバー開検出
    '''          cFRS_ERR_SCVR = スライドカバー開検出
    '''          cFRS_ERR_EMG  = 非常停止</returns> 
    '''=========================================================================
    Private Function Check_CoverOpen(ByVal ObjSys As SystemNET, ByVal bOrgFlg As Boolean) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Private Function Check_CoverOpen(ByVal ObjSys As Object, ByVal bOrgFlg As Boolean) As Integer

        Dim sw As Long = 0
        Dim InterlockSts As Integer = 0
        Dim sldcvrSts As Long = 0
        'Dim coverSts As Long
        Dim r As Long
        Dim Md As Integer
        Dim strMSG As String

        Try
            ' インターロック解除ならNOP
            If (bOrgFlg = True) Then
                r = ORG_INTERLOCK_CHECK(InterlockSts, sw)           ' インターロック状態取得
            Else
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' インターロック状態取得
            End If
            If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Then     ' インターロック全解除 ?
                Return (cFRS_NORMAL)                                ' Return値 = 正常
            End If

            ' カバー状態の判定
            If (r <> cFRS_NORMAL And r <> ERR_OPN_CVRLTC) Then
                ' エラーメッセージ処理
                'GoTo STP_ERR
            Else
                Return (cFRS_NORMAL)
            End If

            ' '' '' '' 筐体カバー閉チェック
            '' '' ''r = COVER_CHECK(coverSts)                             ' 非常停止 or 筐体カバー開チェック
            '' '' ''If (coverSts = 0) Then                              ' 非常停止/筐体カバー開検出 ?
            '' '' ''    GoTo STP_ERR                                        ' Return値 = 非常停止/筐体カバー開検出
            '' '' ''End If
            ' '' '' '' '' '' '' 筐体カバー閉チェック
            '' '' '' '' '' ''r = ObjSys.InterLockCheck()                             ' 非常停止 or 筐体カバー開チェック
            '' '' '' '' '' ''If (r <> cFRS_NORMAL) Then                              ' 非常停止/筐体カバー開検出 ?
            '' '' '' '' '' ''    GoTo STP_ERR                                        ' Return値 = 非常停止/筐体カバー開検出
            '' '' '' '' '' ''End If

            ' '' '' '' スライドカバー閉チェック
            '' '' ''Call SLIDECOVER_CLOSECHK(sldcvrSts)                     ' スライドカバー閉チェック
            '' '' ''If (sldcvrSts = 0) Then                                 ' スライドカバーオープン ?
            '' '' ''    r = cGMODE_SCVRMSG                                  ' Return値 = 筐体カバー開検出
            '' '' ''    GoTo STP_ERR
            '' '' ''End If

            'Return (cFRS_NORMAL)

            ' エラーメッセージ表示
            'STP_ERR:
            If (r = ERR_EMGSWCH) Then                              ' 非常停止 ?
                Md = cGMODE_EMG
            ElseIf (r = ERR_OPN_CVR) Then                          ' 筐体カバー開検出 ?
                Md = cGMODE_CVR_OPN
            ElseIf (r = ERR_OPN_SCVR) Then                          ' スライドカバー開検出
                Md = cGMODE_SCVR_OPN
            Else
                Md = r
            End If
            '' '' ''If (r = cFRS_ERR_EMG) Then                              ' 非常停止 ?
            '' '' ''    Md = cGMODE_EMG
            '' '' ''ElseIf (r = cFRS_ERR_CVR) Then                          ' 筐体カバー開検出 ?
            '' '' ''    Md = cGMODE_CVR_OPN
            '' '' ''Else                                                    ' スライドカバー開検出
            '' '' ''    Md = cGMODE_SCVR_OPN
            '' '' ''End If

            r = Sub_CallFrmRset(ObjSys, Md)                         ' エラーメッセージ表示
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Check_CoverOpen() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###184↓ -----
#Region "原点復帰時の載物台の基板なしをチェックする"
    '''=========================================================================
    ''' <summary>原点復帰時の載物台の基板なしをチェックする</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <param name="Mode">   (INP)APP_MODE_LOADERINIT = ローダ原点復帰時
    '''                            APP_MODE_AUTO　　　 = 自動運転時
    '''                            APP_MODE_FINEADJ 　 = 一時停止(サイクル停止でCancel指定時) V4.0.0.0�R
    ''' </param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function SubstrateCheck(ByVal ObjSys As SystemNET, ByVal Mode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function SubstrateCheck(ByVal ObjSys As Object, ByVal Mode As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""

        Try
            ' 載物台に基板がある場合、取り除かれるまで待つ(SL436R時)
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then Return (cFRS_NORMAL)
            '----- V4.0.0.0-27↓ -----
            ' SL436Sで原点復帰時はチェックなし
            If (giMachineKd = MACHINE_KD_RS) And (Mode = APP_MODE_LOADERINIT) Then
                Return (cFRS_NORMAL)
            End If
            '----- V4.0.0.0-27↑ -----
            Do
                System.Threading.Thread.Sleep(300)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' 載物台に基板がない事をチェックする
                Call ZCONRST()                                          ' コンソールキーラッチ解除
                r = Sub_SubstrateCheck(ObjSys, Mode)                    ' 基板なしチェック
                If (r = cFRS_NORMAL) Then Exit Do '                     ' 基板なしならループを抜ける 
                If (r < cFRS_NORMAL) Then                               ' 異常終了レベルのエラー ? 
                    Return (r)                                          ' 呼び出し元でアプリケーション強制終了へ
                End If
                '                                                       ' 基板有(r = cFRS_ERR_RST)なら取り除かれるまで待つ
            Loop While (1)

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SubstrateCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###197↓ -----
#Region "原点復帰時のNG排出BOX満杯をチェックする"
    '''=========================================================================
    ''' <summary>原点復帰時のNG排出BOX満杯をチェックする</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function NgBoxCheck(ByVal ObjSys As SystemNET, ByVal Mode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function NgBoxCheck(ByVal ObjSys As Object, ByVal Mode As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""

        Try
            ' NG排出BOXが満杯の場合、取り除かれるまで待つ(SL436R時)
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then Return (cFRS_NORMAL)
            Do
                System.Threading.Thread.Sleep(300)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' NG排出BOXが満杯でない事をチェックする
                Call ZCONRST()                                          ' コンソールキーラッチ解除
                r = Sub_NgBoxCheck(ObjSys, Mode)                        ' NG排出BOXが満杯でないかチェック
                If (r = cFRS_NORMAL) Then Exit Do '                     ' 満杯でないならループを抜ける 
                If (r < cFRS_NORMAL) Then                               ' 異常終了レベルのエラー ? 
                    Return (r)                                          ' 呼び出し元でアプリケーション強制終了へ
                End If
                '                                                       ' 基板有(r = cFRS_ERR_RST)なら取り除かれるまで待つ
            Loop While (1)

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.NgBoxCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "NG排出BOXが満杯でないかチェックする"
    '''=========================================================================
    ''' <summary>NG排出BOXが満杯でないかチェックする</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <param name="Mode">   (INP)APP_MODE_LOADERINIT = ローダ原点復帰時
    '''                            APP_MODE_AUTO　　　 = 自動運転時</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_NgBoxCheck(ByVal ObjSys As SystemNET, ByVal Mode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_NgBoxCheck(ByVal ObjSys As Object, ByVal Mode As Integer) As Integer

        Dim lData As Long = 0
        Dim lBit As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""
        Dim strMS2 As String = ""
        Dim strMS3 As String = ""
        Dim bFlg As Boolean = True

        Try
            ' NG排出BOXが満杯でないかチェックする
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                r = W_Read(LOFS_W43S, lData)                            ' 物理入力状態取得(W43.00-W43.15)
                lBit = LDSTS_NGFULL
            Else
                ' SL436R時
                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                lBit = LDST_NGFULL
            End If

            ' G排出BOXが満杯ならメッセージ表示
            If (lData And lBit) Then                                    ' NG排出BOX満杯 ? V2.0.0.0�E
                '' ハンドを搭載位置に移動する(残基板取り除くため) ※原点復帰してないとハンドは動かない
                'Call W_HAND_STAGE()

                ' 「固定カバー開チェックなし」にする
                Call COVERCHK_ONOFF(COVER_CHECK_OFF)

                '----- V1.18.0.5�@↓(解除はここで行う) -----
                '----- V1.18.0.1�G↓ -----
                ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
                r = EL_Lock_OnOff(EX_LOK_MD_OFF)
                If (r = cFRS_TO_EXLOCK) Then                            ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                    rtn = cFRS_ERR_RST
                    GoTo STP_END                                        ' 終了処理へ
                End If
                If (r < cFRS_NORMAL) Then                               ' 異常終了レベルのエラー ? 
                    Return (r)
                End If
                '----- V1.18.0.1�G↑ -----
                '----- V1.18.0.5�@↑ -----

                ' モードによって表示メッセージを設定する
                If (Mode = APP_MODE_AUTO) Then                          ' 自動運転時 ? 
                    strMSG = MSG_LOADER_21                              ' "ＮＧ排出ボックス満杯"
                    strMS2 = MSG_LOADER_41                              ' "ＮＧ基板を取り除いてから"
                    strMS3 = MSG_LOADER_33                              ' "再度自動運転を実行して下さい"

                Else                                                    ' ローダ原点復帰時
                    strMSG = MSG_LOADER_21                              ' "ＮＧ排出ボックス満杯"
                    strMS2 = MSG_LOADER_40                              ' "ＮＧ基板を取り除いて下さい"
                    strMS3 = ""                                         ' ""
                End If

                ' メッセージ表示(STARTキー押下待ち)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                rtn = cFRS_ERR_RST                                      ' Return値 = Cancel(RESETｷｰ)
            End If

STP_END:
            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()

            ' 「固定カバー開チェックあり」にする
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする

            '----- V1.18.0.5�@↓(電磁ロックは自動運転時(トリミング時)のみ行う) -----
            If (Mode = APP_MODE_TRIM) Then                              ' 自動運転時(トリミング時) ? 
                '----- V1.18.0.1�G↓ -----
                ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
                r = EL_Lock_OnOff(EX_LOK_MD_ON)
                If (r = cFRS_TO_EXLOCK) Then                            ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                    rtn = cFRS_ERR_RST
                End If
                If (r < cFRS_NORMAL) Then                               ' 異常終了レベルのエラー ? 
                    rtn = r
                End If
                '----- V1.18.0.1�G↑ -----
            End If
            '----- V1.18.0.5�@↑ -----

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_NgBoxCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###197↑ -----
#Region "載物台に基板がない事をチェックする"
    '''=========================================================================
    ''' <summary>載物台に基板がない事をチェックする</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <param name="Mode">   (INP)APP_MODE_LOADERINIT = ローダ原点復帰時
    '''                            APP_MODE_AUTO　　　 = 自動運転時
    '''                            APP_MODE_FINEADJ 　 = 一時停止(サイクル停止でCancel指定時) V4.0.0.0�R
    ''' </param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_SubstrateCheck(ByVal ObjSys As SystemNET, ByVal Mode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_SubstrateCheck(ByVal ObjSys As Object, ByVal Mode As Integer) As Integer

        Dim lData As Long = 0
        Dim lBit As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""
        Dim strMS2 As String = ""
        Dim strMS3 As String = ""
        Dim bFlg As Boolean = True

        Try

            'V4.12.2.0�G    'V6.0.5.0�B             ↓
            ' クランプレス時は念のためクランプOFFする
            If (gSysPrm.stIOC.giClamp = 2) Then
                W_CLMP_ONOFF(0)                                         ' クランプOFF
            End If
            'V4.12.2.0�G    'V6.0.5.0�B             ↑

            ' 載物台に基板がない事をチェックする
            '----- V1.16.0.0�K↓ -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            End If
            '----- V1.16.0.0�K↑ -----
            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ※200msだとワーク有が検出されない場合がある
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
                lBit = LDSTS_VACUME
            Else
                ' SL436R時
                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                lBit = LDST_VACUME
            End If

            '----- V1.16.0.0�K↓ -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            End If
            '----- V1.16.0.0�K↑ -----
            Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF)

            ' ワーク有ならメッセージ表示
            If (lData And lBit) Then                                    ' 載物台に基板有 ? V2.0.0.0�E
                ' 「固定カバー開チェックなし」にする
                Call COVERCHK_ONOFF(COVER_CHECK_OFF)
                '----- V4.0.0.0�R↓ -----
                ' 電磁ロック(観音扉右側ロック)を解除する(一時停止(サイクル停止でCancel指定時)) ローム殿特注(SL436R/SL436S)
                r = EL_Lock_OnOff(EX_LOK_MD_OFF)                        ' 電磁ロック(観音扉右側ロック)を解除する
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済)
                    rtn = r                                         ' Return値設定 
                    Return (rtn)
                End If
                '----- V4.0.0.0�R↑ -----

                ' モードによって表示メッセージを設定する
                If (Mode = APP_MODE_AUTO) Then                          ' 自動運転時 ? 
                    strMSG = MSG_LOADER_32                              ' "載物台の基板を取り除いてから"
                    strMS2 = MSG_LOADER_33                              ' "再度自動運転を実行して下さい"
                    strMS3 = ""                                         ' ""
                    '----- V4.0.0.0�R↓ -----
                ElseIf (Mode = APP_MODE_FINEADJ) Then                   ' 一時停止モード ?(サイクル停止でCancel指定時) 
                    strMSG = MSG_LOADER_36                              ' "載物台の基板を取り除いて下さい"
                    strMS2 = ""                                         ' ""
                    strMS3 = ""                                         ' ""
                    '----- V4.0.0.0�R↑ -----
                Else                                                    ' ローダ原点復帰時
                    '----- ###188↓ -----
                    ' X軸,Y軸が原点にあるかチェックする
                    bFlg = CheckAxisXYOrg()
                    If (bFlg = False) Then                              ' X軸,Y軸が原点にない ?
                        strMSG = MSG_LOADER_37                          ' "載物台上に基板が残っています"
                        strMS2 = MSG_LOADER_38                          ' "STARTキー又はOKボタン押下で"
                        strMS3 = MSG_LOADER_39                          ' "ステージを原点に戻します"
                    Else
                        strMSG = MSG_LOADER_36                          ' "載物台の基板を取り除いて下さい"
                        strMS2 = ""                                     ' ""
                        strMS3 = ""                                     ' ""
                    End If
                    '----- ###188↑ -----
                End If

                ' メッセージ表示(STARTキー押下待ち)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                rtn = cFRS_ERR_RST                                      ' Return値 = Cancel(RESETｷｰ)
            End If

            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()

            ' 「固定カバー開チェックあり」にする
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする

            '----- V4.0.0.0�R↓ -----
            ' 一時停止モード(サイクル停止でCancel指定時)なら電磁ロックする ローム殿特注(SL436R/SL436S)
            If (Mode = APP_MODE_FINEADJ) Then
                ' 電磁ロック(観音扉右側ロック)をロックする
                r = EL_Lock_OnOff(EX_LOK_MD_ON)                         ' 電磁ロック
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済)
                    rtn = r                                             ' Return値設定
                End If
                Return (rtn)
            End If
            '----- V4.0.0.0�R↑ -----
            '----- ###188↓ -----
            ' X軸,Y軸が原点にない時はステージを原点に戻す(残基板取り除くため)
            If (bFlg = False) Then                                          ' X軸,Y軸が原点にない ?
                r = SUbXYOrg(ObjSys)
                If (r < cFRS_NORMAL) Then Return (r) '                      ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                Call W_CLMP_ONOFF(0)                                        ' クランプＯＦＦ V1.16.0.0�D
            End If
            '----- ###188↑ -----

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R↓ -----
#Region "載物台に基板がある事をチェックする(サイクル停止中の続行指定時 SL436R用)"
    '''=========================================================================
    ''' <summary>載物台に基板がある事をチェックする(サイクル停止中の続行指定時用)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <returns>cFRS_NORMAL    = 正常(基板あり続行)
    '''          cFRS_ERR_START = 正常(基板なし続行)
    '''          cFRS_ERR_RST   = 基板なしでCancel(RESETキー押下)
    '''          cFRS_ERR_HALT  = 割欠検出
    '''          上記以外=エラー</returns>
    ''' <remarks>ローム殿特注(SL436R/SL436S)</remarks>
    '''=========================================================================
    Public Function Sub_SubstrateExistCheck2(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_SubstrateExistCheck2(ByVal ObjSys As Object) As Integer

        Dim lData As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""
        Dim strMS2 As String = ""
        Dim strMS3 As String = ""
        Dim bFlg As Boolean = True

        Try
            ' 載物台に基板がある事をチェックする
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call ZABSVACCUME(1)                                     ' (クランプOFFで基板がづれるのをふせぐため) 
                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            End If
            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ※200msだとワーク有が検出されない場合がある
            If (giMachineKd = MACHINE_KD_RS) Then
                r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
            Else
                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
            End If

            ' 基板がある場合はクランプOFFしない
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                If (lData And LDST_VACUME) Then                         ' 載物台に基板有 ?
                    '                                                   ' クランプOFFしない
                Else
                    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                    Call W_CLMP_ONOFF(0)                                ' クランプOFF
                End If
            End If

            ' 基板がある場合は吸着OFFしない
            If (lData And LDST_VACUME) Then                             ' 載物台に基板有 ?

            Else
                Call ZABSVACCUME(0)                                     ' バキュームの制御(吸着OFF)
            End If

            ' 「固定カバー開チェックなし」にする
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)                            ' 電磁ロック(観音扉右側ロック)を解除する
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                Return (r)
            End If

            ' メッセージ表示
            If (lData And LDST_VACUME) Then                             ' 載物台に基板がある場合
                rtn = cFRS_NORMAL                                       ' Return値 = 正常(基板あり続行)

                ' 割欠チェック
                r = R_BREAK_XY()
                If (r <> cFRS_NORMAL) Then
                    Call W_CLMP_ONOFF(0)                                ' クランプOFF
                    Call ZABSVACCUME(0)                                 ' バキュームの制御(吸着OFF)
                    ' メッセージ表示(STARTキー押下待ち)
                    ' "割欠検出", "載物台上に基板が残っている場合は", "取り除いてください"
                    r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                            MSG_LOADER_49, MSG_LOADER_17, MSG_LOADER_18, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (r < cFRS_NORMAL) Then Return (r) '              ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み)
                    rtn = cFRS_ERR_HALT                                 ' Return値 = 割欠検出(HALTキー押下で戻る)
                End If

            Else                                                        ' 載物台に基板がない場合

                ' メッセージ表示(STARTキー, RESETキー押下待ち)
                ' "載物台に基板がありません", "OKボタン押下で自動運転を続行します", "Cancelボタン押下で自動運転を終了します"
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                        MSG_LOADER_42, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み)
                If (r = cFRS_ERR_RST) Then
                    rtn = cFRS_ERR_RST                                  ' Return値 = 基板なしでCancel(RESETキー押下)
                Else
                    rtn = cFRS_ERR_START                                ' Return値 = 正常(基板なし続行)
                End If
            End If

            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()

            ' 「固定カバー開チェックあり」にする
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         '「固定カバー開チェックあり」にする
            Call ZCONRST()                                              ' コンソールキーラッチ解除

            ' 電磁ロック(観音扉右側ロック)をロックする
            r = EL_Lock_OnOff(EX_LOK_MD_ON)                             ' 電磁ロック
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                rtn = r                                                 ' Return値設定 
            End If

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateExistCheck2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R↑ -----
    '----- ###240↓ -----
#Region "載物台に基板がある事をチェックする(手動モード時 SL436R用)"
    '''=========================================================================
    ''' <summary>載物台に基板がある事をチェックする(手動モード時 SL436R用)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常
    '''          3=基板無し(Cancel(RESETｷｰ)　
    '''          上記以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_SubstrateExistCheck(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_SubstrateExistCheck(ByVal ObjSys As Object) As Integer
#If cPLCcOFFLINEcDEBUG Then
        Return (cFRS_NORMAL)
#End If
        Dim lData As Long = 0
        Dim lBit As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""
        Dim strMS2 As String = ""
        Dim strMS3 As String = ""
        Dim bFlg As Boolean = True

        Try
            ' 載物台に基板なしでトリミング実行するならNOP
            If (giTrimExe_NoWork = 0) Then
                Return (cFRS_NORMAL)
            End If

            'V4.12.2.0�G                 ↓ 'V6.0.5.0�B
            ' クランプレス時は念のためクランプOFFする
            If (gSysPrm.stIOC.giClamp = 2) Then
                W_CLMP_ONOFF(0)                                         ' クランプOFF
            End If
            'V4.12.2.0�G                 ↑ 'V6.0.5.0�B

            ' 載物台に基板がある事をチェックする
            '----- V1.16.0.0�K↓ -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            End If
            '----- V1.16.0.0�K↑ -----
            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ※200msだとワーク有が検出されない場合がある
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
                lBit = LDSTS_VACUME
            Else
                ' SL436R時
                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                lBit = LDST_VACUME
            End If

            ''V5.0.0.9�L
            ' ''----- V1.16.0.0�K↓ -----
            ''If (gSysPrm.stIOC.giClamp = 1) Then
            ''    Call W_CLMP_ONOFF(1)                                    ' クランプON
            ''    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
            ''    Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            ''End If
            ' ''----- V1.16.0.0�K↑ -----
            ''V5.0.0.9�L
            Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF)

            ' ワーク無しならメッセージ表示
            If (lData And lBit) Then                                    ' 載物台に基板有 ? V2.0.0.0�E

            Else                                                        ' 載物台に基板なし
                ' 「固定カバー開チェックなし」にする
                Call COVERCHK_ONOFF(COVER_CHECK_OFF)

                If (giSubExistMsgFlag = True) Then

                    ' 表示メッセージを設定する
                    strMSG = MSG_LOADER_42                                  ' "載物台に基板がありません"
                    strMS2 = MSG_LOADER_43                                  ' "基板を置いて再度実行して下さい"
                    strMS3 = ""                                             ' ""

                    ' メッセージ表示(STARTキー押下待ち)
                    r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                            strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                End If
                rtn = cFRS_ERR_RST                                      ' Return値 = 基板無しはCancel(RESETｷｰ)で返す
            End If

            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()

            ' 「固定カバー開チェックあり」にする
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            Call ZCONRST()                                              ' コンソールキーラッチ解除

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateExistCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "載物台に基板がある事をチェックする(手動モード時 SL432R用)"
    '''=========================================================================
    ''' <summary>載物台に基板がある事をチェックする(手動モード時 SL432R用)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常
    '''          3=基板無し(Cancel(RESETｷｰ)　
    '''          上記以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_SubstrateExistCheck_432R(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_SubstrateExistCheck_432R(ByVal ObjSys As Object) As Integer

        Dim lData As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""
        Dim strMS2 As String = ""
        Dim strMS3 As String = ""
        Dim SubSts As Integer = 0

        Try
            '----- V1.13.0.0�F↓ -----
            ' SL432RW時で TRIM_EXEC_NOWORK=1ならクランプONへ
            If (giMachineKd = KEY_TYPE_RW) And (giTrimExe_NoWork = 1) Then
                GoTo STP_010
            End If
            '----- V1.13.0.0�F↑ -----

            ' 載物台に基板なしでトリミング実行する又は吸着制御なし又は吸着センサチェックなしならNOP
            If (giTrimExe_NoWork = 0) Or (gSysPrm.stIOC.giVacume = 0) Or (gSysPrm.stIOC.giRetryVaccume = 0) Then
                Return (cFRS_NORMAL)
            End If

STP_010:
            ' クランプON
            r = Form1.System1.ClampCtrl(gSysPrm, 1, giTrimErr)          ' V1.13.0.0�F
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                Return (r)
            End If
            'If (gSysPrm.stIOC.giClamp = 1) Then                         ' クランプ制御あり ?
            '    r = CLAMP_CTRL(1, 1)                                    ' クランプON(X,Y) 
            '    r = ObjSys.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            '    If (r <> cFRS_NORMAL) Then                              ' エラー ? 
            '        Return (r)
            '    End If
            'End If

            ' 吸着ON
            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) 

            Call INP16(VACUME_STS, SubSts)                              ' 吸着ステータス取得 
            'Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF) ###253

            ' ワーク無しならメッセージ表示
            If (SubSts And VACUME_STS_BIT) Then                         ' 吸着ON(載物台に基板有) ?

            Else                                                        ' 載物台に基板なし
                Call ZABSVACCUME(0)                                     ' バキュームの制御(吸着OFF) ###253
                ' 表示メッセージを設定する
                strMSG = MSG_LOADER_42                                  ' "載物台に基板がありません"
                strMS2 = MSG_LOADER_43                                  ' "基板を置いて再度実行して下さい"
                strMS3 = ""                                             ' ""

                ' メッセージ表示(STARTキー押下待ち)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                rtn = cFRS_ERR_RST                                      ' Return値 = 基板無しはCancel(RESETｷｰ)で返す
                '----- V1.13.0.0�F↓ -----
                ' クランプOFF
                r = Form1.System1.ClampCtrl(gSysPrm, 0, giTrimErr)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? 
                    Return (r)
                End If
                '----- V1.13.0.0�F↑ -----
            End If
            Call ZCONRST()                                              ' コンソールキーラッチ解除

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateExistCheck_432R() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###240↑ -----
#Region "マガジン基板検出センサがOFFである事をチェックする"
    '''=========================================================================
    ''' <summary>マガジン基板検出センサがOFFである事をチェックする</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_SubstrateSensorOffCheck(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_SubstrateSensorOffCheck(ByVal ObjSys As Object) As Integer

        Dim iFlg As Integer = 0
        Dim lData As Long = 0
        Dim lSts As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' マガジン基板検出センサ状態を取得する
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                r = W_Read(LOFS_W43S, lData)                            ' 物理入力状態取得(W43.00-W43.15)

                ' マガジンの基板検出センサがOFFである事をチェックする
                If (lData And LDSTS_MG1_SUBTSENS) Then                  ' 基板検出(マガジン1) ? 
                    iFlg = 1
                End If
                If (lData And LDSTS_MG2_SUBTSENS) Then                  ' 基板検出(マガジン2) ? 
                    iFlg = iFlg + 2
                End If

            Else
                ' SL436R時
                r = H_Read(LOFS_H00, lSts)                              ' 初期設定状態取得(H0.00-H0.15)
                r = W_Read(LOFS_W42, lData)                             ' 物理入力状態取得(W42.00-W42.15)

                ' マガジン1-4の基板検出センサがOFFである事をチェックする
                If (lData And LDST_MG1_SUBTSENS) Then                       ' 基板検出(マガジン1) ? 
                    iFlg = 1
                End If
                If (lData And LDST_MG2_SUBTSENS) Then                       ' 基板検出(マガジン2) ? 
                    iFlg = iFlg + 2
                End If
                ' マガジン3,4は4マガジンの時のみチェックする
                If (lSts And LHST_MAGAZINE) Then                            ' 4マガジン ? 
                    If (lData And LDST_MG3_SUBTSENS) Then                   ' 基板検出(マガジン3) ? 
                        iFlg = iFlg + 4
                    End If
                    If (lData And LDST_MG4_SUBTSENS) Then                   ' 基板検出(マガジン4) ? 
                        iFlg = iFlg + 8
                    End If
                End If
            End If

            ' 基板検出センサONならメッセージ表示
            If (iFlg <> 0) Then
                ' "マガジンを基板検出センサＯＦＦ位置まで","下げてから", "再度自動運転を実行して下さい"
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_LOADER_34, MSG_LOADER_35, MSG_LOADER_33, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                rtn = cFRS_ERR_RST                                      ' Return値 = Cancel(RESETｷｰ)
            End If

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateSensorOffCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###184↑ -----
    '----- V1.18.0.0�H↓ -----
#Region "マガジンの有無をチェックする(ローム殿特注)"
    '''=========================================================================
    ''' <summary>マガジンの有無をチェックする(ローム殿特注)</summary>
    ''' <param name="Exist">(INP)0=無しチェック, 1=有りチェック</param>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          cFRS_ERR_RST = エラー
    ''' </returns> 
    '''=========================================================================
    Public Function Sub_MagazineExitCheck(ByVal Exist As Integer) As Integer

        Dim iFlg As Integer = 0
        Dim lData As Long = 0
        Dim lSts As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            '---------------------------------------------------------------
            '   マガジン有りの場合
            '---------------------------------------------------------------
            ' 物理入力状態を取得する
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Return (cFRS_NORMAL)
            Else
                ' SL436R時
                r = W_Read(LOFS_W42, lData)                             ' 物理入力状態取得(W42.00-W42.15)
            End If

            ' マガジン2の有無をチェックする
            If (lData And LDST_MG2_EXIST) Then                          ' マガジン2有 ?
                If (Exist = 0) Then                                     ' マガジン2無しチェックなら
                    Return (cFRS_ERR_RST)                               ' Return値 = エラー             
                End If

                '---------------------------------------------------------------
                '   マガジン無しの場合
                '---------------------------------------------------------------
            Else                                                        ' マガジン2無しの場合 ?
                If (Exist = 0) Then                                     ' マガジン2無しチェックなら
                    Return (cFRS_NORMAL)                                ' Return値 = 正常  
                Else                                                    ' マガジン2有チェックなら
                    Return (cFRS_ERR_RST)                               ' Return値 = エラー  
                End If
            End If

            '---------------------------------------------------------------
            '   マガジン2有りで、マガジン有りチェックなら
            '   マガジン2が表かチェックする
            '---------------------------------------------------------------
            If (lData And LDST_MG2_REVERS) Then                         ' マガジン2裏 ?
                Return (cFRS_ERR_RST)                                   ' Return値 = エラー  
            End If

            ' マガジン1の有無をチェックする
            If (lData And LDST_MG1_EXIST) Then                          ' マガジン1有 ?
                Return (cFRS_NORMAL)
            End If

            Return (cFRS_ERR_RST)                                       ' Return値 = エラー  

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_MagazineExitCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.18.0.0�H↑ -----
    '----- V1.23.0.0�D↓ -----
#Region "マガジンが下がっている事をチェックする"
    '''=========================================================================
    ''' <summary>マガジンが下がっている事をチェックする</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_MagazineDownCheck(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_MagazineDownCheck(ByVal ObjSys As Object) As Integer

        Dim TimerRS As System.Threading.Timer = Nothing
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer
        Dim lData As Long = 0
        Dim strMSG As String

        Try
            ' ローダ通信タイムアウトチェック用タイマーオブジェクトの作成(TimerRS_TickをX msec間隔で実行する)
            Sub_SetTimeoutTimer(TimerRS)

            ' マガジン1の下がりを待つ
            Do
                r = W_Read(LOFS_W42, lData)                             ' 物理入力状態取得(W42.00-W42.15)
                If ((lData And LDST_MG1_DOWN) = LDST_MG1_DOWN) Then
                    rtnCode = cFRS_NORMAL                               ' Return値 = 正常
                    GoTo STP_END
                End If

                ' ローダ通信タイムアウトチェック 
                If (bFgTimeOut = True) Then                             ' タイムアウト ?
                    rtnCode = cFRS_ERR_LDRTO                            ' Return値 = ローダ通信タイムアウト
                    GoTo STP_END
                End If

                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(300)                 ' Wait(msec)
            Loop While (1)

            ' 終了処理
STP_END:
            ' コールバックメソッドの呼出しを停止する
            If (IsNothing(TimerRS) = False) Then
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' タイマーを破棄する
            End If

            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' タイムアウト ?
                r = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)           ' エラーメッセージ表示
            End If

            Return (rtnCode)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(Sub_MagazineDownCheck)"
            MsgBox(strMSG)
            rtnCode = cFRS_NORMAL
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V1.23.0.0�D↑ -----
    '----- ###188↓ -----
#Region "ステージを原点に戻す(残基板取り除くため)"
    '''=========================================================================
    ''' <summary>ステージを原点に戻す(残基板取り除くため)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function SUbXYOrg(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function SUbXYOrg(ByVal ObjSys As Object) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim sts As Integer = 0
        Dim strMSG As String

        Try
            ' ステージを原点に戻す(残基板取り除くため)
            r = Sub_CallFrmRset(ObjSys, cGMODE_LDR_STAGE_ORG)
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SUbXYOrg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "X軸,Y軸が原点にあるかチェックする"
    '''=========================================================================
    ''' <summary>X軸,Y軸が原点にあるかチェックする</summary>
    ''' <returns>True= X,Y軸共原点, False=原点でない</returns>
    '''=========================================================================
    Public Function CheckAxisXYOrg() As Boolean

        Dim sts As Integer = 0
        Dim strMSG As String

        Try
            ' X,YテーブルなしならNOP
            If (gSysPrm.stDEV.giXYtbl <> 3) Then Return (True)

            ' SL436SならNOP V2.0.0.0�M
            If (giMachineKd = MACHINE_KD_RS) Then Return (True)

            ' X軸,Y軸が原点にあるかチェックする
            GetAxisSubStatus(AXIS_X, sts)                               ' X軸サブステータス取得 
            If (sts And SUBSTS_ORG) Then                                ' X軸は原点 ? 
                GetAxisSubStatus(AXIS_Y, sts)                           ' Y軸サブステータス取得 
                If (sts And SUBSTS_ORG) Then                            ' Y軸は原点 ? 
                    Return (True)                                       ' Return値 = X,Y軸共原点 
                End If
            End If

            Return (False)                                              ' Return値 = 原点でない 

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.CheckAxisXYOrg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "軸サブステータスを返す"
    '''=========================================================================
    ''' <summary>軸サブステータスを返す</summary>
    ''' <param name="Axis">  (INP)軸種別</param>
    ''' <param name="SubSts">(OUT)軸サブステータス</param>
    '''=========================================================================
    Public Sub GetAxisSubStatus(ByVal Axis As Integer, ByRef SubSts As Integer)

        Dim strMSG As String

        Try
            SubSts = 0
            Call INP16(AxisSubStsAry(Axis), SubSts)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.GetAxisSubStatus() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###188↑ -----
    '----- V1.16.0.0�O↓ -----
#Region "吸着確認ステータスを返す"
    '''=========================================================================
    ''' <summary>吸着確認ステータスを返す</summary>
    ''' <param name="Sts">(OUT)ステータス(1:吸着確認, 0:吸着未確認)</param>
    '''=========================================================================
    Public Sub GetVacumeStatus(ByRef Sts As Integer)

        Dim lData As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                ' 物理入力状態取得(W42.00-W42.15)
                r = W_Read(LOFS_W42S, lData)
                If (lData And LDSTS_VACUME) Then                        ' 吸着確認(載物台に基板有) ?
                    Sts = 1
                Else
                    Sts = 0
                End If

            Else
                ' SL436R時
                ' 物理入力状態取得(W44.00-W44.15)
                r = W_Read(LOFS_W44, lData)
                If (lData And LDST_VACUME) Then                         ' 吸着確認(載物台に基板有) ?
                    Sts = 1
                Else
                    Sts = 0
                End If
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.GetVacumeStatus() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.16.0.0�O↑ -----
#Region "FrmReset実行サブルーチン"
    '''=========================================================================
    ''' <summary>FrmReset実行サブルーチン</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <param name="gMode"> (INP)処理モード</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Sub_CallFrmRset(ByVal ObjSys As SystemNET, ByVal gMode As Integer) As Integer   'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_CallFrmRset(ByVal ObjSys As Object, ByVal gMode As Integer) As Integer

        Dim r As Integer
        'V6.0.0.0�Q        Dim objForm As Object = Nothing
        Dim objForm As FrmReset = Nothing           'V6.0.0.0�Q
        Dim strMSG As String

        Try


            '----- V4.11.0.0�C↓ (WALSIN殿SL436S対応) ----
            'If gMode = cGMODE_LDR_TMOUT Then
            '    ' 一時停止開始時間を設定する(オプション)
            '    m_blnElapsedTime = True                             ' 経過時間を表示する
            '    Call Set_PauseStartTime(StPauseTime)
            'End If
            '----- V4.11.0.0�C↑ -----

            ' FrmReset画面表示(処理モードに対応する処理を行う)
            objForm = New FrmReset()
            Call objForm.ShowDialog(Nothing, gMode, ObjSys)
            r = objForm.sGetReturn()                                    ' Return値取得

            '----- V4.11.0.0�C↓ (WALSIN殿SL436S対応) ----
            'If gMode = cGMODE_LDR_TMOUT Then
            '    ' 一時停止終了時間を設定し一時停止時間を集計する(オプション)
            '    Call Set_PauseTotalTime(StPauseTime)
            '    ' DllTrimClassLibraryに一時停止時間を渡す(オプション)
            '    TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            'End If
            '----- V4.11.0.0�C↑ -----

            ' オブジェクト開放
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                    ' オブジェクト開放
                Call objForm.Dispose()                                  ' リソース開放
            End If

             'V6.0.5.0�I↓
            ' 原点復帰時はクランプを開状態にする ###001
            'V4.12.2.4�A            If (gMode = cGMODE_ORG) Or (gMode = cGMODE_LDR_ORG) Then
            Select Case (gMode)
                Case cGMODE_ORG, cGMODE_LDR_ORG
                    If (r = cFRS_NORMAL) Then                               ' ###163
                        '----- V1.16.0.0�D↓ -----
                        Call W_CLMP_ONOFF(0)                                ' クランプＯＦＦ(開)  
                        'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                        'Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_CLMP_X + LDAB_CLMP_Y)
                        '----- V1.16.0.0�D↑ -----
                    End If
                Case cGMODE_LDR_END                                         'V4.12.2.4�A 自動運転終了時追加
                    If (r = cFRS_ERR_START) Then
                        Call W_CLMP_ONOFF(0)                                ' クランプＯＦＦ(開)  
                    End If
            End Select
            'V4.12.2.4�A
             'V6.0.5.0�I↑

            Return (r)                                                  ' Return(エラー時のメッセージは表示済) 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_CallFrmRset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "FrmResetを使用して指定のメッセージを表示する"
    '''=========================================================================
    ''' <summary>FrmResetを使用して指定のメッセージを表示する ###089</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <param name="gMode"> (INP)処理モード</param>
    ''' <param name="Md">    (INP)cFRS_ERR_START                = STARTキー押下待ち
    '''                           cFRS_ERR_RST                  = RESETキー押下待ち
    '''                           cFRS_ERR_START + cFRS_ERR_RST = START/RESETキー押下待ち</param>
    ''' <param name="BtnDsp">(INP)ボタン表示する/しない</param>
    ''' <param name="Msg1">  (INP)表示メッセージ１</param>
    ''' <param name="Msg2">  (INP)表示メッセージ２</param>
    ''' <param name="MSG3">  (INP)表示メッセージ３</param>
    ''' <param name="Col1">  (INP)メッセージ色１</param>
    ''' <param name="Col2">  (INP)メッセージ色２</param>
    ''' <param name="Col3">  (INP)メッセージ色３</param>
    ''' <returns>cFRS_ERR_START = OKボタン(STARTキー)押下
    '''          cFRS_ERR_RST   = Cancelボタン(RESETキー)押下
    '''          上記以外       = エラー</returns> 
    '''=========================================================================
    Public Function Sub_CallFrmMsgDisp(ByVal ObjSys As SystemNET, ByVal gMode As Integer, ByVal Md As Integer, ByVal BtnDsp As Boolean, _
                                       ByVal Msg1 As String, ByVal Msg2 As String, ByVal Msg3 As String, _
                                       ByVal Col1 As Color, ByVal Col2 As Color, ByVal Col3 As Color) As Integer     'V6.0.0.0�Q
        'Public Function Sub_CallFrmMsgDisp(ByVal ObjSys As Object, ByVal gMode As Integer, ByVal Md As Integer, ByVal BtnDsp As Boolean, _
        '                                   ByVal Msg1 As String, ByVal Msg2 As String, ByVal Msg3 As String, _
        '                                   ByVal Col1 As Object, ByVal Col2 As Object, ByVal Col3 As Object) As Integer

        Dim r As Integer
        'V6.0.0.0�J        Dim objForm As Object = Nothing
        'V6.0.0.0�Q        Dim ColAry(3) As Object
        Dim ColAry(3) As Color          'V6.0.0.0�Q
        Dim MsgAry(3) As String
        Dim strMSG As String

        Try
            ' パラメータ設定
            MsgAry(0) = Msg1
            MsgAry(1) = Msg2
            MsgAry(2) = Msg3
            ColAry(0) = Col1
            ColAry(1) = Col2
            ColAry(2) = Col3

            ' FrmReset画面表示(指定のメッセージを表示する)
            'V6.0.0.0�J            objForm = New FrmReset()
            Dim objForm As New FrmReset()   'V6.0.0.0�J
            Call objForm.ShowDialog(Nothing, gMode, ObjSys, MsgAry, ColAry, Md, BtnDsp)
            r = objForm.sGetReturn()                                    ' Return値取得

            ' オブジェクト開放
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                    ' オブジェクト開放
                Call objForm.Dispose()                                  ' リソース開放
            End If

            Return (r)                                                  ' Return(エラー時のメッセージは表示済) 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_CallFrmMsgDisp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "メッセージ表示処理"
    '''=========================================================================
    ''' <summary>メッセージ表示処理</summary>
    ''' <param name="ErrCode">(INP)エラーコード</param>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Private Function Sub_MsgDisp(ByVal ErrCode As Short, ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Private Function Sub_MsgDisp(ByVal ErrCode As Short, ByVal ObjSys As Object) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 正常またはタイムアウト等の表示済のエラーコードならNOP
            If ((ErrCode >= cFRS_NORMAL) Or _
               ((ErrCode <= cFRS_TO_SCVR_CL) And (ErrCode >= cFRS_TO_PM_BK))) Then
                Return (ErrCode)
            End If

            ' エラーメッセージ表示
            r = ObjSys.Form_AxisErrMsgDisp(System.Math.Abs(ErrCode))
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_MsgDisp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "NG排出トレイに乗せた数をカウントし、設定値を超えていれば画面表示する"
    '''=========================================================================    ###130 
    ''' <summary>NG排出トレイに乗せた数をカウントし、設定値を超えていれば画面表示する</summary>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function JudgeNGtrayCount(ByVal Idx As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL ' ###199
        Dim strMSG As String = ""       ' V6.1.1.0�L

        '----- ###199↓ -----
        '「NG排出BOX満杯」はセンサーで行うため、NG排出BOXへの排出数のチェックは止める
        '----- V6.0.3.0_48↓ -----
        ' 止めるのを止める  
        'Return (cFRS_NORMAL)
        '----- V6.0.3.0_48↑ -----
        '----- ###199↑ -----
        ' V6.0.4.0�B↓
        ' NGボックス排出枚数の設定が０の場合にはNGボックスへの排出枚数のチェックは行わない
        If giNgBoxCount(Idx) = 0 Then
            giNgBoxCounter = 0
            giBreakCounter = 0
            giTwoTakeCounter = 0
            Return (cFRS_NORMAL)
        End If
        ' V6.0.4.0�B↑

        '-------------------------------------------------------------------
        '   NG排出BOX満杯ならメッセージを表示する ###089
        '-------------------------------------------------------------------
        ' NG排出BOX満杯 ?
        'If (giNgBoxCounter > giNgBoxCount(Idx)) Then                ' NG排出BOXの収納枚数カウンター > NG排出BOXの収納枚数 ?
        If ((giNgBoxCounter + giBreakCounter + giTwoTakeCounter) >= giNgBoxCount(Idx)) Then ' NG排出BOXの収納枚数カウンター > NG排出BOXの収納枚数 ? ###181 2013.01.24
            giAppMode = APP_MODE_LDR_ALRM                           ' アプリモード = ローダアラーム画面(カバー開のエラーとしない為)
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                    ' 「固定カバー開チェックなし」にする ###088
            '----- V1.18.0.1�G↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                Return (r)
            End If
            '----- V1.18.0.1�G↑ -----
            '----- V6.1.1.0�L↓ -----
            If (giAlmTimeDsp = 0) Then                                  ' ローダアラーム時の時間表示の有無(0=表示なし, 1=表示あり)　
                strMSG = MSG_LOADER_21                                  '"ＮＧ排出ボックス満杯"
            Else
                Call Get_NowYYMMDDHHMMSS(strMSG)
                strMSG = MSG_LOADER_21 + " " + strMSG
            End If
            '----- V6.1.1.0�L↑ -----
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            ''"ＮＧ排出ボックス満杯","ＮＧ排出ボックスからＮＧ基板を取り除いてから","STARTキーを押すか、OKボタンを押して下さい。"V6.1.1.0�I
            ' "ＮＧ排出ボックス満杯","ＮＧ排出ボックスからＮＧ基板を取り除いて","STARTキーを押すか、OKボタンを押して下さい。"    V6.1.1.0�I
            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                    strMSG, MSG_LOADER_20, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
            If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 

            '----- ###189 -----
            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()
            If (r < cFRS_NORMAL) Then Return (r) '                  ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
            '----- '###189 -----
            giAppMode = APP_MODE_TRIM                               ' アプリモード = トリミング中
            Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア ###088
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                     ' 「固定カバー開チェックあり」にする ###088

            '----- V1.18.0.1�G↓ -----
            ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
            r = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (r = cFRS_TO_EXLOCK) Then                            ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                               ' 異常終了レベルのエラー ? 
                Return (r)
            End If
            '----- V1.18.0.1�G↑ -----

            Call ClearNGTrayCount()                                 ' NG排出BOXの収納枚数カウンターを初期化する
        End If
    End Function

#End Region

#Region "NG排出トレイに乗せた数をクリアする"
    '''=========================================================================    ###130 
    ''' <summary>NG排出トレイに乗せた数をクリアする</summary>
    '''=========================================================================
    Public Sub ClearNGTrayCount()
        giNgBoxCounter = 0                                      ' 「NG排出BOXの収納枚数カウンター」を初期化する
        giBreakCounter = 0                                      ' 割れ欠け基板のカウント
        giTwoTakeCounter = 0                                    ' ２枚取り基板のカウント
        DspNGTrayCount()
    End Sub
#End Region

#Region "NG排出トレイに載せた数の画面上の表示を更新する"
    '''=========================================================================    ###149 
    ''' <summary>NG排出トレイに載せた数の画面上の表示を更新する</summary>
    '''=========================================================================
    Public Sub DspNGTrayCount()
        'Form1.lblNgCount.Text = "NG Count = " + Str(giNgBoxCounter)
        'Form1.lblBreakCount.Text = "Break = " + Str(giBreakCounter)
        'Form1.lblTwoCount.Text = "Two = " + Str(giTwoTakeCounter)
        Form1.lblNgCount.Text = "NG Count = " + Str(giNgBoxCounter + giBreakCounter + giTwoTakeCounter)
    End Sub
#End Region

#Region "NGトレイに置いていたら判定を行う"
    '''=========================================================================    ' ###148 
    ''' <summary>NGトレイに置いていたら判定を行う</summary>
    '''=========================================================================
    Public Function DspNGTrayChk() As Integer

        Dim LdIn As UShort
        Dim r As Integer
        Dim Idx As Integer
        '----- ###173↓ -----
        Dim strMSG As String
        Dim rtnCode As Integer = cFRS_NORMAL
        '----- ###173↑ -----
        Dim TimerRS As System.Threading.Timer = Nothing     'V5.0.0.1�D

        Try
            '----- ###173↓ -----
            If (bFgAutoMode = False) Then                                   ' 自動運転でなければNOP ?
                Return (cFRS_NORMAL)
            End If
            '----- ###173↑ -----

            ' NGトレイへの排出完了信号を待つ
            r = GetLoaderIO(LdIn)
            If ((LdIn And LINP_NGTRAY_OUT_COMP) = LINP_NGTRAY_OUT_COMP) Then
                If (((LdIn And LINP_WBREAK) = LINP_WBREAK)) Then            ' B15: 基板割れ検出(0=基板割れ未検出, 1=基板割れ検出)　
                    giBreakCounter = giBreakCounter + 1                     ' 割れ欠け基板のカウント
                ElseIf (((LdIn And LINP_2PIECES) = LINP_2PIECES)) Then      ' B14: ２枚取り検出(0=２枚取り未検出, 1=２枚取り検出)　
                    giTwoTakeCounter = giTwoTakeCounter + 1                 ' ２枚取り基板のカウント
                Else
                    giNgBoxCounter = giNgBoxCounter + 1                     ' NG排出BOXの収納枚数カウンターを+ 1する
                    '----- V1.18.0.0�B↓ -----
                    ' 不良基板数更新(ローム殿特注)
                    If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' 不良基板数 = NGボックスに排出した枚数をカウントする 
                        stPRT_ROHM.Tol_NG_Sheet = stPRT_ROHM.Tol_NG_Sheet + 1
                    End If
                    '----- V1.18.0.0�B↑ -----
                End If
                DspNGTrayCount()
                Idx = typPlateInfo.intWorkSetByLoader - 1                   ' Idx = 基板品種番号 - 1
                If (Idx < 0) Then Return (rtnCode) ' ###173
                r = JudgeNGtrayCount(Idx)
                '----- ###200 -----
                ' NG排出未満杯時はアラームリセット信号送出しない
                If ((LdIn And LINP_NG_FULL) = 0) Then                       ' B12: NG排出満杯(0=NG排出未満杯, 1=NG排出満杯(完了))
                    Call W_RESET()                                          ' アラームリセット信号送出 
                    Call W_START()                                          ' スタート信号送出 
                End If
                'Call W_RESET()                                              ' アラームリセット信号送出 
                'Call W_START()                                              ' スタート信号送出 
                '----- ###200 -----
                SetLoaderIO(LOUT_PROC_CONTINUE, 0)                          ' 動作継続信号ON(0:なし, 1:継続実行)

                'V5.0.0.1�D↓
                ' ローダ通信タイムアウトチェック用タイマーオブジェクトの作成(TimerRS_TickをX msec間隔で実行する)
                Sub_SetTimeoutTimer(TimerRS)
                'V5.0.0.1�D↑

                ' NGトレイへの排出完了信号のOFFを待つ
                Do
                    r = GetLoaderIO(LdIn)
                    If (((LdIn And LINP_NGTRAY_OUT_COMP) = 0)) Then         ' NGトレイへの排出完了信号OFF ?　
                        SetLoaderIO(0, LOUT_PROC_CONTINUE)                  ' 動作継続信号OFF(0:なし, 1:継続実行)
                        Exit Do
                    End If
                    '----- ###173↓ -----
                    If (bFgTimeOut = True) Then                             ' タイムアウト ?
                        Return (cFRS_ERR_LDRTO)                             ' Return値 = ローダ通信タイムアウト
                    End If
                    System.Windows.Forms.Application.DoEvents()
                    Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
                    '----- ###173↑ -----
                Loop
            End If

            '----- ###173↓ -----
STP_EXIT:

            ' コールバックメソッドの呼出しを停止する
            If (IsNothing(TimerRS) = False) Then
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' タイマーを破棄する
            End If

            Return (rtnCode)
            '----- ###173↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.DspNGTrayChk() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.18.0.1�G↓ -----
#Region "電磁ロック(観音扉右側ロック)をロックまたは解除する(ローム殿特注(SL436R/SL436S))"
    '''=========================================================================
    ''' <summary>電磁ロック(観音扉右側ロック)をロックまたは解除する</summary>
    ''' <param name="Md">(INP)モード(0(EX_LOK_MD_OFF)=ロック解除,
    '''                               1(EX_LOK_MD_ON) =ロック)</param>
    ''' <returns>cFRS_NORMAL     = 正常
    '''          cFRS_TO_EXLOCK  = 前面扉ロックタイムアウト
    '''          上記以外      = その他のエラー
    ''' </returns> 
    '''=========================================================================
    Public Function EL_Lock_OnOff(ByVal Md As Integer) As Integer

        Dim TimerLock As System.Threading.Timer = Nothing
        Dim sw As Long = 0
        Dim InterlockSts As Integer = 0
        Dim Sts As Integer = 0
        Dim r As Integer = cFRS_NORMAL
        Dim bTOut As Boolean
        Dim strTOUT As String
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 電磁ロック機能なしならNOP
            If (giDoorLock = 0) Then Return (cFRS_NORMAL)

            ' インターロック解除ならNOP
            r = INTERLOCK_CHECK(InterlockSts, sw)                       ' インターロック状態取得
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' インターロック中でない ?
                Return (cFRS_NORMAL)                                    ' Return値 = 正常
            End If

            ' タイムアウトチェック用タイマーオブジェクトの作成(TimerLock_TickをX msec間隔で実行する)
            TimerTM_Create(TimerLock, EX_LOK_TOUT)

STP_RETRY:
            '-------------------------------------------------------------------
            '   電磁ロック(観音扉右側ロック)をロックまたは解除する
            '-------------------------------------------------------------------
            If (Md = EX_LOK_MD_ON) Then                                 ' ロックモード ? 
                Call EXTOUT1(EXTOUT_EX_LOK_ON, 0)                       ' 電磁ロック(観音扉右側ロック)をロックする
                strTOUT = MSG_SPRASH43                                  '  "前面扉ロックタイムアウト"
            Else
                Call EXTOUT1(0, EXTOUT_EX_LOK_ON)                       ' 電磁ロック(観音扉右側ロック)を解除する
                strTOUT = MSG_SPRASH44                                  '  "前面扉ロック解除タイムアウト"
            End If

            '-------------------------------------------------------------------
            '   電磁ロックがロックまたは解除されたかチェックする
            '-------------------------------------------------------------------
            Do
                System.Threading.Thread.Sleep(100)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' 電磁ロックステータス取得 
                Call INP16(EX_LOK_STS, Sts)

                ' 電磁ロック(観音扉右側ロック)をロックまたは解除されたかチェックする
                If (Md = EX_LOK_MD_ON) Then                             ' ロックモード ? 
                    If ((Sts And EXTINP_EX_LOK_ON) = EXTINP_EX_LOK_ON) Then
                        Exit Do                                         ' 電磁ロックならExit
                    End If
                Else
                    If ((Sts And EXTINP_EX_LOK_ON) = 0) Then
                        Exit Do                                         ' 電磁ロック解除ならExit
                    End If
                End If

                '-------------------------------------------------------------------
                '   タイムアウトチェック 
                '-------------------------------------------------------------------
                bTOut = TimerTM_Sts()
                If (bTOut = True) Then                                  ' タイムアウト ? 
                    ' コールバックメソッドの呼出しを停止する
                    TimerTM_Stop(TimerLock)

                    ' ランプ制御
                    Call LAMP_CTRL(LAMP_START, True)                    ' STARTランプON
                    Call LAMP_CTRL(LAMP_RESET, True)                    ' RESETランプON
                    Call ZCONRST()                                      ' コンソールキーラッチ解除

                    '  "前面扉ロック(or 解除)タイムアウト" "STARTキー：処理続行，RESETキー：処理終了" 
                    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                            strTOUT, MSG_SPRASH35, "", System.Drawing.Color.Blue, System.Drawing.Color.Black, System.Drawing.Color.Black)
                    ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み)  
                    If (r < cFRS_NORMAL) Then Exit Do
                    If (r = cFRS_ERR_RST) Then
                        Call ZCONRST()                                  ' コンソールキーラッチ解除
                        r = cFRS_TO_EXLOCK                              ' Return値 =  電磁ロックタイムアウト
                        Exit Do
                    End If

                    ' ランプ制御
                    Call LAMP_CTRL(LAMP_START, False)                   ' STARTランプOFF
                    Call LAMP_CTRL(LAMP_RESET, False)                   ' RESETランプOFF
                    Call ZCONRST()                                      ' コンソールキーラッチ解除

                    ' タイマー開始
                    Call TimerTM_Start(TimerLock, EX_LOK_TOUT)
                    GoTo STP_RETRY                                      ' リトライへ 

                End If

            Loop While (1)

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
            ' タイマーを破棄する
            TimerTM_Dispose(TimerLock)

            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.EL_Lock_OnOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.18.0.1�G↑ -----
    '----- V4.0.0.0�R↓ -----
#Region "サイクル停止処理(ローム殿特注(SL436R/SL436S))"
    '''=========================================================================
    ''' <summary>サイクル停止処理</summary>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <returns>cFRS_NORMAL     = 正常
    '''          cFRS_ERR_RST    = Cancel(RESETキー押下)
    '''          cFRS_TO_EXLOCK  = 前面扉ロックタイムアウト
    '''          上記以外        = その他のエラー
    ''' </returns> 
    '''=========================================================================
    Public Function CycleStop_Proc(ByVal ObjSys As SystemNET) As Integer        'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function CycleStop_Proc(ByVal ObjSys As Object) As Integer

        Dim RtnCode As Integer = cFRS_NORMAL
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' サイクル停止要求信号を送出してローダからのサイクル停止応答を待つ
            ''V5.0.0.4�A 
            Dim gObjMSG As FrmWait = Nothing 'V6.0.0.0�Q
            'V6.0.0.0�Q            Dim gObjMSG As Object = Nothing
            If IsNothing(gObjMSG) = True Then
                gObjMSG = New FrmWait()
                Call gObjMSG.Show(Form1)
            End If
            ''V5.0.0.4�A 
            r = W_CycleStop(ObjSys, 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                ''V5.0.0.4�A 
                If IsNothing(gObjMSG) <> True Then
                    gObjMSG.MsgClose()
                    gObjMSG = Nothing
                End If
                ''V5.0.0.4�A 
                RtnCode = r                                             ' Return値設定 
                GoTo STP_END                                            ' 処理終了へ 
            End If

            ' シグナルタワー制御(On=なし, Off=自動運転中(緑点灯))
            'V5.0.0.9�M ↓　V6.0.3.0�G
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_GRN_ON)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_IDLE)
            'V5.0.0.9�M ↑　V6.0.3.0�G

            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALTランプOFF(SL436S用)
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            Call W_CLMP_ONOFF(0)                                        ' クランプOFF 'V5.0.0.4�@ １回目ＴＫＹがＩ／ＯでＯＮすると２回目ＰＬＣでＯＦＦ出来ないのでＯＦＦしておく。
            '-------------------------------------------------------------------
            '   電磁ロック(観音扉右側ロック)を解除する
            '-------------------------------------------------------------------

STP_RETRY:
            RtnCode = cFRS_NORMAL
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        '「固定カバー開チェックなし」にする
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)                            ' 電磁ロック(観音扉右側ロック)を解除する
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                RtnCode = r                                             ' Return値設定 
                GoTo STP_END                                            ' 処理終了へ 
            End If
            Call ZCONRST()

            ''V5.0.0.4�A 
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            ''V5.0.0.4�A 

            '-------------------------------------------------------------------
            '   「サイクル停止中」メッセージを表示して「STARTキー」「RESETキー」
            '    の押下待ち(この間に基板を取り出して確認が可能)
            '-------------------------------------------------------------------
            Dim md As Short = Globals_Renamed.giAppMode                 'V5.0.0.9�P
            Globals_Renamed.giAppMode = APP_MODE_IDLE                   'V5.0.0.9�P
            ' メッセージ表示(STARTキー, RESETキー押下待ち)
            ' "サイクル停止中", "OKボタン押下で自動運転を続行します", "Cancelボタン押下で自動運転を終了します"
            r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                    MSG_LOADER_48, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Globals_Renamed.giAppMode = md                              'V5.0.0.9�P
            If (r < cFRS_NORMAL) Then                                   ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                RtnCode = r                                             ' Return値設定 
                GoTo STP_END                                            ' 処理終了へ
            ElseIf (r = cFRS_ERR_RST) Then                              ' Cancel(RESETキー押下) ? 
                RtnCode = cFRS_ERR_RST                                  ' Return値 = Cancel(RESETキー押下)
            End If

            '-------------------------------------------------------------------
            '   電磁ロック(観音扉右側ロック)をロックする
            '-------------------------------------------------------------------
            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                RtnCode = r                                             ' Return値設定 
                GoTo STP_END                                            ' 処理終了へ 
            End If

            ' 「固定カバー開チェックあり」にする
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         '「固定カバー開チェックあり」にする

            ' 電磁ロック(観音扉右側ロック)をロックする
            r = EL_Lock_OnOff(EX_LOK_MD_ON)                             ' 電磁ロック
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                RtnCode = r                                             ' Return値設定 
                GoTo STP_END                                            ' 処理終了へ 
            End If

            '-------------------------------------------------------------------
            '   「STARTキー押下時」は基板ありを確認
            '   「RESETキー押下時」は基板なしを確認
            '-------------------------------------------------------------------
            If (RtnCode = cFRS_ERR_RST) Then                            ' RESETキー押下 ?
STP_010:
                ' 載物台に基板がない事を確認(載物台に基板がある場合は、取り除かれるまで待つ)
                r = SubstrateCheck(Form1.System1, APP_MODE_FINEADJ)     ' ※ 一時停止画面モードでCall
                If (r < cFRS_NORMAL) Then                               ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                    RtnCode = r                                         ' Return値設定 
                    GoTo STP_END                                        ' 処理終了へ
                End If
            Else                                                        ' STARTキー押下時
                'V5.0.0.4�B  STP_010:        'V5.0.0.4�B移動
                ' 載物台の基板あり/なしを確認
                RtnCode = Sub_SubstrateExistCheck2(ObjSys)              ' RtnCode = cFRS_NORMAL(基板あり続行), cFRS_ERR_START(基板なし続行), cFRS_ERR_RST(基板なしでCancel(RESETキー押下)
                If (RtnCode < cFRS_NORMAL) Then                         ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                    GoTo STP_END                                        ' 処理終了へ
                End If
                '「基板なし続行」指定なら載物台に基板がない事を確認へ
                If (RtnCode = cFRS_ERR_START) Then
                    GoTo STP_010
                End If
                ' 割欠検出なら再度頭からチェック
                If (RtnCode = cFRS_ERR_HALT) Then
                    GoTo STP_RETRY
                End If
            End If

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
STP_END:
            ' サイクル停止要求信号をOFFする
            r = W_CycleStop(ObjSys, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済)
                RtnCode = r                                             ' Return値設定 
            End If

            ' シグナルタワー制御(On=自動運転中(緑点灯),Off=全ﾋﾞｯﾄ)
            'V5.0.0.9�M ↓　V6.0.3.0�G
            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
            'V5.0.0.9�M ↑　V6.0.3.0�G

            ' PLC側と同期を取るため吸着OFF(I/O)を行う
            Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF)

            Return (RtnCode)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.CycleStop_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            RtnCode = cERR_TRAP
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R↑ -----
    '----- V4.0.0.0-26↓ -----
#Region "10進→BCD変換"
    '''=========================================================================
    ''' <summary>10進→BCD変換</summary>
    ''' <param name="strDec">(INP)OcxSystemオブジェクト</param>
    ''' <returns>cFRS_NORMAL     = 正常
    '''          cFRS_ERR_RST    = Cancel(RESETキー押下)
    '''          cFRS_TO_EXLOCK  = 前面扉ロックタイムアウト
    '''          上記以外        = その他のエラー
    ''' </returns> 
    '''=========================================================================
    Public Function DecToBcd(ByVal strDec As String) As UShort

        Dim kekka As UShort = 0
        Dim length As Integer
        Dim i As Integer
        Dim wkC As Byte
        Dim strMSG As String

        Try

            length = strDec.Length
            For i = 0 To (length - 1)
                wkC = strDec.Substring(i + 1, 1)
                kekka = kekka + ((wkC And &HF) << (4 * ((length - 1) - i)))
            Next i

            Return (kekka)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "LoaderIOFor436.DecToBcd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (0)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26↑ -----
    '----- V4.11.0.0�E↓ (WALSIN殿SL436S対応) -----
#Region "基板投入ボタン押下時の処理"
    '''=========================================================================
    ''' <summary>基板投入ボタン押下時の処理</summary>
    ''' <param name="ObjSys">       (INP)OcxSystemオブジェクト</param>
    ''' <returns>cFRS_NORMAL     = 正常
    '''          cFRS_ERR_RST    = Cancel(RESETキー押下)
    '''          上記以外        = その他のエラー
    ''' </returns> 
    '''=========================================================================
    Public Function SubstrateSet_Proc(ByVal ObjSys As SystemNET) As Integer     'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function SubstrateSet_Proc(ByVal ObjSys As Object) As Integer

        Dim TimerLock As System.Threading.Timer = Nothing
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer = cFRS_NORMAL
        Dim TimeVal As Integer
        Dim bTOut As Boolean
        Dim lData As Long = 0
        Dim LdIn As UShort = 0
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   基板追加要求信号送信(SL436S)
            '-------------------------------------------------------------------
            ' ローダ無効またはローダ手動モードならNOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If
            ' 基板投入フラグOFFならNOP
            If (gbChkSubstrateSet = False) Then
                Return (cFRS_NORMAL)
            End If
            ' 

            ''V5.0.0.1�C↓
            If giNgStop = 1 Then
                btnJudgeEnable(False)
            End If
            ''V5.0.0.1�C↑
            ' 基板投入ボタンの背景色を灰色にする
            ' gbChkSubstrateSet = False                                   ' 基板投入フラグOFF
            Form1.BtnSubstrateSet.Enabled = False                       ' 基板投入ボタン非活性化
            '            Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)

STP_RETRY:
            'V5.0.0.1�G
            If IsNothing(gObjMSG) = True Then
                gObjMSG = New FrmWait()
                Call gObjMSG.Show(Form1)
            End If
            'V5.0.0.1�G

            ' タイマー値を設定する
            If (giOPLDTimeOutFlg = 0) Then                              ' ローダ通信タイムアウト検出無し ?
                TimeVal = System.Threading.Timeout.Infinite             ' タイマー値 = なし
            Else
                TimeVal = giOPLDTimeOut                                 ' タイマー値 = ローダ通信タイムアウト時間(msec)
            End If

            ' 基板追加要求信号送信
            lData = LPCS_SET_SUBSTRATE
            Call m_PlcIf.WritePlcWR(LOFS_W113S, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W113S, lData)

            '-------------------------------------------------------------------
            '   タイムアウトチェック用タイマーオブジェクトの作成
            '   (TimerLock_TickをX msec間隔で実行する)
            '-------------------------------------------------------------------
            TimerTM_Create(TimerLock, TimeVal)

            'V5.0.0.1�Iメインシーケンスでエラーチェックをしないように設定する
            giErrLoader = cFRS_ERR_RST

            '-------------------------------------------------------------------
            '   基板追加要求応答待ち
            '-------------------------------------------------------------------
            Do
                System.Threading.Thread.Sleep(10)                       ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' ローダアラーム/非常停止チェック
                r = GetLoaderIO(LdIn)                                   ' ローダ
                If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                    rtnCode = cFRS_ERR_LDR                              ' Return値 = ローダアラーム検出
                    GoTo STP_ERR_LDR                                    ' ローダアラーム表示へ
                End If

                ' 基板追加要求応答応答待ち
                Call m_PlcIf.ReadPlcWR(LOFS_W114S, lData)               ' 基板追加要求応答取得
                Console.WriteLine("SubstrateSet_Proc() ReadPlcWR =" + lData.ToString("0"))
                If (lData And LPLCS_SET_SUBSTRATE) Then
                    Exit Do                                             ' 基板追加要求応答ならExit
                End If

                '-------------------------------------------------------------------
                '   タイムアウトチェック 
                '-------------------------------------------------------------------
                bTOut = TimerTM_Sts()
                If (bTOut = True) Then                                  ' タイムアウト ? 
                    rtnCode = cFRS_ERR_LDRTO                            ' Retuen値 = ローダ通信タイムアウト 
                    GoTo STP_END
                End If

            Loop While (1)
            'V4.11.0.0�J
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            'V4.11.0.0�J

            '----- V4.11.0.0�C↓ (WALSIN殿SL436S対応) ----
            ' 一時停止開始時間を設定する(オプション)
            m_blnElapsedTime = True                             ' 経過時間を表示する
            Call Set_PauseStartTime(StPauseTime)
            '----- V4.11.0.0�C↑ -----

            '-------------------------------------------------------------------
            '   メッセージを表示してSTARTボタンの押下待ち
            '-------------------------------------------------------------------
            ' シグナルタワー制御(On=なし, Off=自動運転中(緑点灯))
            'V5.0.0.9�M ↓
            'Call Form1.System1.SetSignalTower(0, SIGOUT_GRN_ON)
            Call Form1.System1.SetSignalTowerCtrl(ObjSys.SIGNAL_IDLE)
            'V5.0.0.9�M ↑

            '「固定カバー開チェックなし」にする 
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)

            Form1.TimerAdjust.Enabled = False ''V5.0.0.1�I

            ' "カバーを開けて基板をセットしてください","再開する場合はSTARTボタンを押して下さい。","
            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_BTN_START, True, _
                    MSG_LOADER_50, MSG_LOADER_51, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            'r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
            '        MSG_LOADER_50, MSG_LOADER_51, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            If (r < cFRS_NORMAL) Then                                   ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み) 
                rtnCode = r                                             ' Return値設定 
            End If

            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()

            ' 「固定カバー開チェックあり」にする
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         '「固定カバー開チェックあり」にする
            Call ZCONRST()                                              ' コンソールキーラッチ解除

            ' シグナルタワー制御(On=自動運転中(緑点灯),Off=全ﾋﾞｯﾄ)
            'V5.0.0.9�M ↓
            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
            'V5.0.0.9�M ↑


            '----- V4.11.0.0�C↓ (WALSIN殿SL436S対応) ----
            ' 一時停止終了時間を設定し一時停止時間を集計する(オプション)
            Call Set_PauseTotalTime(StPauseTime)
            ' DllTrimClassLibraryに一時停止時間を渡す(オプション)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C↑ -----

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
STP_END:
            'V4.11.0.0�J
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            'V4.11.0.0�J

            ''V5.0.0.1�C↓
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1�C↑

            ' 基板投入ボタンの背景色を灰色にする
            gbChkSubstrateSet = False                                   ' 基板投入フラグOFF
            Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
            'V5.0.0.1�A↓
            If Form1.BtnADJ.BackColor = System.Drawing.Color.Yellow Then
                Form1.BtnSubstrateSet.Enabled = True                        ' 基板投入ボタン活性化
            End If
            'V5.0.0.1�A↑
            TimerTM_Stop(TimerLock)                                     ' コールバックメソッドの呼出しを停止する
            TimerTM_Dispose(TimerLock)                                  ' タイマーを破棄する

            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ローダ通信タイムアウト ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' エラーメッセージ表示
            End If

            ' 基板追加要求信号OFF送信
            Call m_PlcIf.WritePlcWR(LOFS_W113S, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W113S, 0)

            'V5.0.0.1�Iメインシーケンスでエラーチェックする
            giErrLoader = cFRS_NORMAL

            Return (rtnCode)

            ' ローダエラー発生時
STP_ERR_LDR:

            'V4.11.0.0�J
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            'V4.11.0.0�J

            TimerTM_Stop(TimerLock)                                     ' コールバックメソッドの呼出しを停止する
            TimerTM_Dispose(TimerLock)                                  ' タイマーを破棄する
            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ローダ通信タイムアウト ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' エラーメッセージ表示
            Else
                ' ローダアラームメッセージ作成 & ローダアラーム画面表示
                rtnCode = Loader_AlarmCheck(ObjSys, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                Call ZCONRST()     ''V5.0.0.1�I
                'V5.0.0.1�E
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then
                    Call W_START()                                  ' スタート信号送出 
                    GoTo STP_RETRY
                End If
                'V5.0.0.1�E
            End If
            Form1.BtnSubstrateSet.Enabled = True                        ' 基板投入ボタン活性化 'V4.11.0.0�O
            ''V5.0.0.1�C↓
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1�C↑
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SubstrateSet_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            rtnCode = cERR_TRAP
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V4.11.0.0�E↑ -----

    '===============================================================================
    '   オートローダアラーム処理
    '===============================================================================
#Region "ローダアラームチェック"
    '''=========================================================================
    ''' <summary>ローダアラームチェック</summary>
    ''' <param name="ObjSys">            (INP)OcxSystemオブジェクト</param>
    ''' <param name="bDspFlg">           (INP)アラームリストを表示する(True)/表示しない(False)</param>
    ''' <param name="AlarmCount">        (OUT)発生アラーム数</param>
    ''' <param name="strLoaderAlarm">    (OUT)アラーム文字列</param>
    ''' <param name="strLoaderAlarmInfo">(OUT)アラーム情報1</param>
    ''' <param name="strLoaderAlarmExec">(OUT)アラーム情報(対策)</param>
    ''' <returns>アラーム種類
    '''          cFRS_NORMAL   = 正常(アラーム無し)
    '''          cFRS_ERR_LDR1 = 全停止異常
    '''          cFRS_ERR_LDR2 = サイクル停止
    '''          cFRS_ERR_LDR3 = 軽故障(続行可能)
    '''          cFRS_ERR_PLC  = PLCステータス異常
    '''          cFRS_ERR_RST  = 軽故障(続行可能)でCancel指定
    ''' </returns> 
    '''=========================================================================
    Public Function Loader_AlarmCheck(ByVal ObjSys As SystemNET, ByVal bDspFlg As Boolean, ByRef AlarmCount As Integer, _
                                      ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String) As Integer 'V6.0.0.0�Q
        'Public Function Loader_AlarmCheck(ByVal ObjSys As Object, ByVal bDspFlg As Boolean, ByRef AlarmCount As Integer, _
        '                                  ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String) As Integer

        Dim bFg As Boolean
        Dim r As Integer
        Dim i As Integer
        Dim iRetc As Integer = cFRS_NORMAL
        Dim iData(2) As UShort
        Dim lData As Long
        Dim strMSG As String

        Try
            ' ローダ無効ならNOP
            If (bFgActLink = False) Then
                Return (cFRS_NORMAL)
            End If

            ' ローダ部アラーム状態をチェックする
            r = W_Read(LOFS_W110, lData)                                ' ローダアラーム状態取得(W110.08-10)
            If (r <> cFRS_NORMAL) Then
                ' PLCステータス異常の場合はエラーリストにはPLCステータス異常を表示する
                iRetc = cFRS_ERR_PLC                                    ' Return値 = PLCステータス異常

                ' ローダアラーム画面を表示する
                If (bDspFlg = True) Then                                ' 画面表示する ?
                    r = Sub_CallFormLoaderAlarm(ObjSys, iRetc, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                    If (r = cFRS_ERR_RST) Then                          ' ローダアラーム検出(軽故障)でCancel指定なら ###073
                        Call W_RESET()                                  ' アラームリセット送出 
                        Call W_START()                                  ' スタート信号送出 
                        iRetc = cFRS_ERR_RST                            ' Return値 = Cancel(RESETｷｰ)とする 
                    End If
                End If
            Else
                ' アラーム状態をチェックする
                iData(0) = lData
                If (iData(0) And LARM_ARM1) Then                        ' 軽故障発生中
                    iRetc = cFRS_ERR_LDR3                               ' Return値 = ローダアラーム検出(軽故障) 
                End If
                If (iData(0) And LARM_ARM2) Then                        ' サイクル停止異常発生中
                    iRetc = cFRS_ERR_LDR2                               ' Return値 = ローダアラーム検出(サイクル停止)
                End If
                If (iData(0) And LARM_ARM3) Then                        ' 全停止異常発生中
                    iRetc = cFRS_ERR_LDR1                               ' Return値 = ローダアラーム検出(全停止異常)
                End If

                ' ローダアラーム詳細を取得する(アラーム発生時)
                If (iRetc <> cFRS_NORMAL) Then                          ' アラーム発生 ?
                    r = W_Read(LOFS_W115, lData)                        ' ローダアラーム詳細取得(W115.00-W115.15(続行不可))
                    iData(0) = lData
                    r = W_Read(LOFS_W116, lData)                        ' ローダアラーム詳細取得(W116.00-W116.15(続行可))
                    iData(1) = lData

                    ' アラーム内容が変わったかチェックする
                    bFg = False
                    For i = 0 To 1
                        If (iBefData(i) <> iData(i)) Then               ' アラーム内容が変わった ? 
                            bFg = True
                        End If
                    Next

                    ' アラーム内容が変わったらアラーム情報をiBefData()に退避する
                    If (bFg) Then                                       ' アラーム内容が変わったら
                        ' アラーム情報を退避する
                        For i = 0 To 1
                            iBefData(i) = iData(i)                      ' アラーム情報を退避
                        Next

                        ' ローダアラームメッセージを作成する(AlmCount = 発生アラーム数)
                        AlarmCount = Loader_MakeAlarmStrings(iData, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        '
                        If iRetc = cFRS_ERR_LDR1 Then
                            If (iData(0) And &H8) Or (iData(0) And &H10) Or (iData(0) And &H20) Or (iData(0) And &H2000) Then
                                strLoaderAlarmExec(0) = strLoaderAlarmExec(0) + MSG_LOADER_52
                            End If
                            'V5.0.0.1�N↓
                        ElseIf iRetc = cFRS_ERR_LDR3 Then
                            strLoaderAlarmExec(0) = strLoaderAlarmExec(0) + MSG_LOADER_53
                        End If
                        'V5.0.0.1�N↑

                    End If

                    ' ローダアラーム画面を表示する
                    If (bDspFlg = True) Then                            ' 画面表示する ?
                        'V5.0.0.1�B↓
                        Form1.TimerAdjust.Enabled = False
                        'V5.0.0.1�B↑
                        r = Sub_CallFormLoaderAlarm(ObjSys, iRetc, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        If (r = cFRS_ERR_RST) Then                      ' ローダアラーム検出(軽故障)でCancel指定なら ###073
                            Call W_RESET()                              ' アラームリセット送出 
                            'Call W_START()                             ' スタート信号送出 ###174(続行するのでコメント化)
                            iRetc = cFRS_ERR_RST                        ' Return値 = Cancel(RESETｷｰ)とする 
                        End If
                        'V5.0.0.1�B↓
                        Call ZCONRST()
                        Form1.TimerAdjust.Enabled = True
                        'V5.0.0.1�B↑
                    End If

                    ' 正常時
                Else
                    Call ClearBefAlarm()                                ' アラーム情報退避域クリア
                End If

            End If

            Return (iRetc)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_AlarmCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V2.0.0.0�A↓ -----
#Region "ローダアラームチェック(自動運転時以外 SL436RS用)"
    '''=========================================================================
    ''' <summary>ローダアラームチェック(自動運転時以外 SL436RS用)</summary>
    ''' <param name="ObjSys">            (INP)OcxSystemオブジェクト</param>
    ''' <returns>アラーム種類
    '''          cFRS_NORMAL   = 正常(アラーム無し)
    '''          cFRS_ERR_LDR1 = 全停止異常
    '''          cFRS_ERR_LDR2 = サイクル停止
    '''          cFRS_ERR_LDR3 = 軽故障(続行可能)
    '''          cFRS_ERR_PLC  = PLCステータス異常
    '''          cFRS_ERR_RST  = 軽故障(続行可能)でCancel指定
    ''' </returns> 
    '''=========================================================================
    Public Function Loader_AlarmCheck_ManualMode(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Loader_AlarmCheck_ManualMode(ByVal ObjSys As Object) As Integer

        Dim r As Integer
        Dim iRetc As Integer = cFRS_NORMAL
        Dim LdIn As UShort
        Dim iData(2) As UShort
        Dim lData As Long
        Dim strMSG As String = ""
        Dim strMS2 As String = ""
        Dim strMS3 As String = ""

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' SL436RSでなければNOP
            If (giMachineKd <> MACHINE_KD_RS) Or (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                Return (cFRS_NORMAL)
            End If

            ' ローダ無効ならNOP
            If (bFgActLink = False) Then
                Return (cFRS_NORMAL)
            End If

            ' 自動運転中ならNOP
            If (bFgAutoMode = True) Then
                Return (cFRS_NORMAL)
            End If

            ' ローダアラーム発生でなければNOP
            Call GetLoaderIO(LdIn)                                      ' ローダ入力
            If ((LdIn And LINP_NO_ALM_RESTART) = LINP_NO_ALM_RESTART) Then
                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   ローダ部アラーム状態をチェックする(アラーム発生時)
            '-------------------------------------------------------------------
            r = W_Read(LOFS_W110, lData)                                ' ローダアラーム状態取得(W110.08-10)
            If (r <> cFRS_NORMAL) Then
                iRetc = cFRS_ERR_PLC                                    ' Return値 = PLCステータス異常
                strMSG = MSG_SPRASH30                                   ' "PLCステータス異常"
                strMS2 = MSG_SPRASH16                                   ' "RESETキーを押すとプログラムを終了します"

            Else
                ' アラーム状態をチェックする
                iData(0) = lData
                If (iData(0) And LARM_ARM1) Then                        ' 軽故障発生中
                    iRetc = cFRS_ERR_LDR3                               ' Return値 = ローダアラーム検出(軽故障) 
                End If
                If (iData(0) And LARM_ARM2) Then                        ' サイクル停止異常発生中
                    iRetc = cFRS_ERR_LDR2                               ' Return値 = ローダアラーム検出(サイクル停止)
                End If
                If (iData(0) And LARM_ARM3) Then                        ' 全停止異常発生中
                    iRetc = cFRS_ERR_LDR1                               ' Return値 = ローダアラーム検出(全停止異常)
                End If

                ' ローダアラーム詳細を取得する(アラーム発生時)
                If (iRetc <> cFRS_NORMAL) Then                          ' アラーム発生 ?
                    r = W_Read(LOFS_W115, lData)                        ' ローダアラーム詳細取得(W115.00-W115.15(続行不可))
                    iData(0) = lData
                    r = W_Read(LOFS_W116, lData)                        ' ローダアラーム詳細取得(W116.00-W116.15(続行可))
                    iData(1) = lData
                End If

                ' 正圧異常検出以外のアラームは無視する
                If ((iData(0) And LDFS_ARM_AIR) = LDFS_ARM_AIR) Then
                    strMSG = MSG_SPRASH12                               ' "エアー圧低下検出"
                    strMS2 = MSG_SPRASH16                               ' "RESETキーを押すとプログラムを終了します"
                Else
                    iRetc = cFRS_NORMAL
                End If

            End If

            '-------------------------------------------------------------------
            '   メッセージ表示
            '-------------------------------------------------------------------
            If (iRetc <> cFRS_NORMAL) Then                              ' アラーム発生 ? 
                ' メッセージ表示(RESETキー押下待ち)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_RST, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                If (r < cFRS_NORMAL) Then                               ' 非常停止等のエラーなら
                    iRetc = r                                           ' Return値を再設定する
                End If
            End If

            Return (iRetc)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_AlarmCheck_ManualMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V2.0.0.0�A↑ -----
#Region "アラーム情報退避域クリア"
    '''=========================================================================
    ''' <summary>アラーム情報退避域クリア</summary>
    '''=========================================================================
    Public Sub ClearBefAlarm()

        Dim Len As Integer
        Dim i As Integer
        Dim strMSG As String

        Try
            ' アラーム情報退避域を初期化する
            Len = iBefData.Length
            For i = 0 To (Len - 1)
                iBefData(i) = 0
            Next

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.ClearBefAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ローダアラームメッセージを作成する"
    '''=========================================================================
    ''' <summary>ローダアラームメッセージを作成する</summary>
    ''' <param name="iData">             (INP)ローダアラーム詳細</param>
    ''' <param name="strLoaderAlarm">    (OUT)アラーム文字列</param>
    ''' <param name="strLoaderAlarmInfo">(OUT)アラーム情報1</param>
    ''' <param name="strLoaderAlarmExec">(OUT)アラーム情報(対策)</param>
    ''' <returns>発生アラーム数</returns>
    '''=========================================================================
    Public Function Loader_MakeAlarmStrings(ByRef iData() As UShort, _
                                            ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String) As Integer

        Dim i As Integer
        Dim j As Integer
        Dim num As Integer
        Dim strMSG As String

        Try
            ' ローダアラームメッセージを作成する
            num = 0                                                                             ' 発生アラーム数
            For i = 0 To 1
                For j = 0 To 15
                    If (iData(i) And (2 ^ j)) Then                                              ' アラームビットON ?
                        Select Case ((i * 16) + j)
                            Case 0 ' W115.00
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_00   ' 非常停止
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_00                         ' 非常停止ＳＷが押されました。

                            Case 1 ' W115.01
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_01   ' マガジン整合性アラーム
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_01                         ' マガジンが正規の位置か確認してください。

                            Case 2 ' W115.02
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_02   ' 割れ欠け品発生
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_02                         ' 割れ欠け品が指定された枚数発生しました。

                            Case 3 ' W115.03
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_03   ' ハンド１吸着アラーム
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_03                         ' 吸着センサを確認してください。

                            Case 4 ' W115.04
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_04   ' ハンド２吸着アラーム
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_04                         ' 吸着センサを確認してください。

                            Case 5 ' W115.05
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_05   ' 載物台吸着センサ異常
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_05                         ' 吸着センサを確認してください。

                            Case 6 ' W115.06
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_06   ' 載物台吸着ミス
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_06                         ' 吸着センサを確認してください。 トッププレートにキズや基板のかけらが無いか確認してください。

                            Case 7 ' W115.07
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_07   ' ロボットアラーム
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_07                         ' ロボットアラームが発生しました。

                            Case 8 ' W115.08
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_08   ' 工程間監視アラーム
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_08                         ' 工程間監視でタイムアウトが発生しました。

                            Case 9 ' W115.09
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_09   ' エレベータ異常
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_09                         ' エレベータのセンサを確認してください。

                            Case 10 ' W115.10
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_10   ' マガジン無し
                                strLoaderAlarmInfo(num) = ""
                                'strLoaderAlarmExec(num) = MSG_LDGUID_11                         ' マガジン検出ドグが倒れているか、マガジンをセットしてください。V6.0.3.0_41
                                strLoaderAlarmExec(num) = MSG_LDGUID_10                        ' マガジン検出ドグが倒れているか、マガジンをセットしてください。V6.0.3.0_41
                                giReqLotSelect = 1                                              ' 終了時にロットエンド、継続の画面を表示   V6.0.3.0_38

                            Case 11 ' W115.11
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_11   ' 原点復帰タイムアウト
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_11                         ' 原点復帰時にタイムアウトが発生しました。

                            Case 12 ' W115.12
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_12   ' クランプ異常 ###125
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_12                         ' クランプ異常が発生しました ###125

                            Case 13 ' W115.13
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_13   ' エアー圧低下検出 'V2.0.0.0�B 
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_13                         ' アーが正しく供給されているか確認してください。 'V2.0.0.0�B 

                            Case 14 ' W115.14
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_14   ' 供給マガジンアラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_14                         ' 想定外アラーム

                            Case 15 ' W115.15
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_15   ' 収納マガジン満杯アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_15                         ' 想定外アラーム

                            Case 16 ' W116.00
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_16   ' 旋回シリンダタイムアウト
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_16                         ' シリンダセンサを確認してください。

                            Case 17 ' W116.01
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_17   ' ハンド１シリンダタイムアウト
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_17                         ' シリンダセンサを確認してください。

                            Case 18 ' W116.02
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_18   ' ハンド２シリンダタイムアウト
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_18                         ' シリンダセンサを確認してください。

                            Case 19 ' W116.03
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_19   ' 供給ハンド吸着ミス
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_19                         ' 基板吸着時にタイムアウトが発生しました。

                            Case 20 ' W116.04
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_20   ' 回収ハンド吸着ミス
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_20                         ' 基板吸着時にタイムアウトが発生しました。

                            Case 21 ' W116.05
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_21   ' ＮＧ排出満杯
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_21                         ' ＮＧ排出ＢＯＸが満杯です。基板を取り除いてください。

                            Case 22 ' W116.06
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_22   ' 一時停止
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_22                         ' 一時停止中です。

                            Case 23 ' W116.07
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_23   ' ドアオープン
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_23                         ' ドアオープンが検出されました。ドアを閉じてください。

                            Case 24 ' W116.08
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_24   ' 二枚取り検出　' V1.18.0.0�J
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_24                         ' 供給ハンドの基板を取り除いて再実行してください。 ' V1.18.0.0�J

                            Case 25 ' W116.09
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_25   ' 搬送上下機構アラーム 'V4.0.0.0-59
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_25                         '  搬送上下機構を確認してください。 'V4.0.0.0-59

                            Case 26 ' W116.10
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_26   ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_26                         ' 想定外アラーム

                            Case 27 ' W116.11
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_27   ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_27                         ' 想定外アラーム

                            Case 28 ' W116.12
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_28   ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_28                         ' 想定外アラーム

                            Case 29 ' W116.13
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_29   ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_29                         ' 想定外アラーム

                            Case 30 ' W116.14
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_30   ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_30                         ' 想定外アラーム

                            Case 31 ' W116.15
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_31   ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_31                         ' 想定外アラーム

                            Case Else
                                strLoaderAlarm(num) = MSG_LDALARM_UD & ((i * 16) + j)           ' 未定義アラーム No.
                                strLoaderAlarmInfo(num) = MSG_LDINFO_UD                         ' 想定外アラーム
                                strLoaderAlarmExec(num) = MSG_LDGUID_UD                         ' メーカにアラーム番号を問い合わせてください。
                        End Select
                        num = num + 1                                                           ' 発生アラーム数 + 1

                    End If

                Next j
            Next i

            Return (num)                                                                        ' Return値 = 発生アラーム数

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_MakeAlarmStrings() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "FormLoaderAlarm実行サブルーチン"
    '''=========================================================================
    ''' <summary>FormLoaderAlarm実行サブルーチン</summary>
    ''' <param name="ObjSys">            (INP)OcxSystemオブジェクト</param>
    ''' <param name="AlarmKind">         (INP)アラーム種類(全停止異常, サイクル停止, 軽故障, アラーム無し)</param>
    ''' <param name="AlarmCount">        (INP)発生アラーム数</param>
    ''' <param name="strLoaderAlarm">    (INP)アラーム文字列</param>
    ''' <param name="strLoaderAlarmInfo">(INP)アラーム情報1(※未使用)</param>
    ''' <param name="strLoaderAlarmExec">(INP)アラーム情報(対策)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    ''' 
    Private Function Sub_CallFormLoaderAlarm(ByVal ObjSys As SystemNET, ByRef AlarmKind As Integer, ByVal AlarmCount As Integer, _
                                    ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String) As Integer 'V6.0.0.0�Q
        ' 'V5.0.0.7�@ Private Function Sub_CallFormLoaderAlarm(ByVal ObjSys As Object, ByVal AlarmKind As Integer, ByVal AlarmCount As Integer, _
        ' 'V5.0.0.7�@                               ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String) As Integer

        Dim rtn As Integer = cFRS_NORMAL                                ' ###200
        Dim r As Integer
        Dim svAppMode As Integer = 0                                    ' ###088
        'V6.0.0.0�J        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            ' ローダ無効NならOP
            If (bFgActLink = False) Then                                ' ローダ無効 ?
                Return (cFRS_NORMAL)
            End If

            ' アプリモードを「ローダアラーム表示」にする ###088
            svAppMode = giAppMode
            giAppMode = APP_MODE_LDR_ALRM
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする

            '----- V1.18.0.1�G↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ?
                Return (r)
            End If
            '----- V1.18.0.1�G↑ -----

            ' シグナルタワー制御(On=異常, Off=全ﾋﾞｯﾄ) ###191
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(赤点滅)
                    'V5.0.0.9�M ↓　V6.0.3.0�G(ローム殿仕様は赤点滅＋ブザーＯＮ)
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                    'V5.0.0.9�M ↑　V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' 特注(赤点滅+ブザー１)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            ' アラームメッセージ表示中ON信号送出
            Call W_ALM_DSP()                                            ' V1.18.0.0�M

            '----- V4.11.0.0�C↓ (WALSIN殿SL436S対応) ----
            ' 停止開始時間を設定する(オプション)
            m_blnElapsedTime = True                             ' 経過時間を表示する
            Call Set_PauseStartTime(StPauseTime)
            '----- V4.11.0.0�C↑ -----

            ' ローダアラーム画面を表示する
            bFgLoaderAlarmFRM = True                                    ' ローダアラーム画面表示中ON
            'V6.0.0.0�J            objForm = New FormLoaderAlarm()
            Dim objForm As New FormLoaderAlarm()    'V6.0.0.0�J
            Call objForm.ShowDialog(Nothing, ObjSys, AlarmKind, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            rtn = objForm.sGetReturn()                                  ' Return値取得 ###200

            ' オブジェクト開放
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                    ' オブジェクト開放
                Call objForm.Dispose()                                  ' リソース開放
            End If
            bFgLoaderAlarmFRM = False                                   ' ローダアラーム画面表示中OFF

            '----- V4.11.0.0�C↓ (WALSIN殿SL436S対応) ----
            ' 停止終了時間を設定し一時停止時間を集計する(オプション)
            Call Set_PauseTotalTime(StPauseTime)
            ' DllTrimClassLibraryに一時停止時間を渡す(オプション)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C↑ -----

            '----- ###200↓ ----- 
            '' アプリモードを元に戻す ###088
            'giAppMode = svAppMode
            'Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア ###088
            'Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            '----- ###200↑ ----- 

            ' アラームメッセージ表示中OFF信号送出
            Call W_ALM_DSP()                                            ' V1.18.0.0�M

            ' シグナルタワー制御(On=0, Off=異常) ###191
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準
                    'V5.0.0.9�M ↓ V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M ↑ V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' 特注(赤点滅+ブザー１)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            '----- V1.18.0.7�@↓ -----
            ' 自動運転中(一時停止中以外)はシグナルタワー制御(自動運転中(緑点灯))を行う
            If ((bFgAutoMode = True) And (gObjADJ Is Nothing = True)) Then
                ' シグナルタワー制御(On=自動運転中(緑点灯),Off=全ﾋﾞｯﾄ)
                'V5.0.0.9�M ↑ V6.0.3.0�G
                ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                'V5.0.0.9�M ↑ V6.0.3.0�G

            End If
            '----- V1.18.0.7�@↑ -----

            ' ----- V1.18.0.0�B↓ -----
            ' アラーム停止情報を設定する(ローム殿特注)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B↑ -----

            '----- ###200↓ ----- 
            ' NG排出BOXが満杯の場合は、取り除かれるまで待つ
            r = NgBoxCheck(Form1.System1, APP_MODE_LOADERINIT)
            If (r < cFRS_NORMAL) Then
                Return (r)
            End If

            ' 筐体カバー閉を確認する
            r = FrmReset.Sub_CoverCheck()
            If (r < cFRS_NORMAL) Then
                Return (r)
            End If

            giAppMode = svAppMode
            Call COVERLATCH_CLEAR()                                     ' カバー開ラッチのクリア ###088
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 「固定カバー開チェックあり」にする
            '----- ###200↑ ----- 

            '----- V1.18.0.1�G↓ -----
            ' 電磁ロック(観音扉右側ロック)する
            If (giAppMode = APP_MODE_FINEADJ) Then
                ' 一時停止画面なら電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
                r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            Else
                r = EL_Lock_OnOff(EX_LOK_MD_ON)
            End If
            If (r = cFRS_TO_EXLOCK) Then                                ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                Return (r)
            End If
            '----- V1.18.0.1�G↑ -----

            Return (rtn)                                                ' Return(エラー時のメッセージは表示済) ###200

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_CallFormLoaderAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '===============================================================================
    '   オートローダシリアル通信用関数(SysmacCompolet.dll使用)
    '===============================================================================
#Region "PLC通信設定の初期化"
    '''=========================================================================
    ''' <summary>PLC通信設定の初期化</summary>
    '''=========================================================================
    Public Function Init_PlcIF() As Integer
        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            MessageBox.Show("PLC OFFLINE DEBUG", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return (cFRS_NORMAL)
#End If
            m_PlcIf = New DllPlcIf.DllPlcInterface()

            'PLC通信アドレス設定
            m_PlcIf.SetNetAddress(0)
            m_PlcIf.SetNodeAddress(240)
            m_PlcIf.SetUnitAddress(0)
            m_PlcIf.SetReceiveTimeLimit(1000)


            Return ret
        Catch ex As Exception
            Dim strMsg As String
            strMsg = ex.Message + "(Init_PlcIF)"
            MsgBox(strMsg)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try

    End Function
#End Region

#Region "データ入力(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>データ入力(オートローダシリアル通信)</summary>
    ''' <param name="Offset">(INP)オフセット(10進)</param>
    ''' <param name="lData"> (OUT)入力データ(16進)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function DM_Read(ByVal Offset As Long, ByRef lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' データ入力
            'txtCJDMValue.Text = Hex(SysmacCJ1.DM(CLng(txtCJDMOffset.Text)))
            'lData = Form1.SysmacCJ1.DM(Offset)
            'lData = Form1.SysmacCJ1.DM(Offset)

            ret = m_PlcIf.ReadPlcDM(Offset, lData)
            Return (ret)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(DM_Read)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try
    End Function
#End Region

#Region "データ出力(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>データ出力(オートローダシリアル通信)</summary>
    ''' <param name="Offset">(INP)オフセット(10進)</param>
    ''' <param name="lData"> (INP)出力データ(16進)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function DM_Write(ByVal Offset As Long, ByVal lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' データ出力
            'Form1.SysmacCJ1.DM(CLng(txtCJDMOffset.Text)) = CLng("&H" & txtCJDMValue.Text)
            'Form1.SysmacCJ1.DM(Offset) = lData
            ret = m_PlcIf.WritePlcDM(Offset, lData)
            Return (ret)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される)  
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(DM_Write)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try
    End Function
#End Region

#Region "データ入力(WR=内部保持リレーエリア)(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>データ入力(WR=内部保持リレーエリア)(オートローダシリアル通信)</summary>
    ''' <param name="Offset">(INP)オフセット(10進)</param>
    ''' <param name="lData"> (OUT)入力データ(16進)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function W_Read(ByVal Offset As Long, ByRef lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' データ入力
            'txtCJDMValue.Text = Hex(SysmacCJ1.DM(CLng(txtCJDMOffset.Text)))
            'lData = Form1.SysmacCJ1.DM(Offset)
            'lData = Form1.SysmacCJ1.DM(Offset)

            ret = m_PlcIf.ReadPlcWR(Offset, lData)
            Return (ret)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(W_Read)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try
    End Function
#End Region

#Region "データ出力(WR=内部保持リレーエリア)(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>データ出力(WR=内部保持リレーエリア)(オートローダシリアル通信)</summary>
    ''' <param name="Offset">(INP)オフセット(10進)</param>
    ''' <param name="lData"> (INP)出力データ(16進)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function W_Write(ByVal Offset As Long, ByVal lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' データ出力
            'Form1.SysmacCJ1.DM(CLng(txtCJDMOffset.Text)) = CLng("&H" & txtCJDMValue.Text)
            'Form1.SysmacCJ1.DM(Offset) = lData
            ret = m_PlcIf.WritePlcWR(Offset, lData)
            Return (ret)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される)  
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(W_Write)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try
    End Function
#End Region

#Region "データ入力(HR=電源保持リレーエリア)(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>データ入力(HR=電源保持リレーエリア)(オートローダシリアル通信)</summary>
    ''' <param name="Offset">(INP)オフセット(10進)</param>
    ''' <param name="lData"> (OUT)入力データ(16進)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    ''' <remarks>###184</remarks>
    '''=========================================================================
    Public Function H_Read(ByVal Offset As Long, ByRef lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' データ入力
            ret = m_PlcIf.ReadPlcHR(Offset, lData)
            Return (ret)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(H_Read)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26↓ -----
#Region "Long型データ出力(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>Long型データ出力(オートローダシリアル通信)</summary>
    ''' <param name="Offset">(INP)オフセット(10進)</param>
    ''' <param name="lData"> (INP)出力データ(16進)</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function DM_Write2W(ByVal Offset As Long, ByVal lData As Long) As Integer

        Dim ret As Integer
        Dim wkLong As Long = 0

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' データ出力
            wkLong = (lData >> 16) And &HFFFF
            ret = m_PlcIf.WritePlcDM(Offset + 1, wkLong)
            wkLong = lData And &HFFFF
            ret = m_PlcIf.WritePlcDM(Offset, wkLong)
            Return (ret)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される)  
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(DM_Write2W)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' 通信エラー(PLC)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26↑ -----
#Region "ブザーOFF信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ブザーOFF信号送出(オートローダシリアル通信) ###073</summary>
    '''=========================================================================
    Public Sub W_BzOff()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                ' ブザーOFF
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_BUZER_OFF)

            Else
                ' SL436R時
                ' ブザーOFF
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_BUZER_OFF)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_BzOff)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "スタート信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>スタート信号送出(オートローダシリアル通信) ###073</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_START()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' スタート信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_START)

            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_START)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_START)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "アラームリセット信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>アラームリセット信号送出(オートローダシリアル通信) ###073</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_RESET()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' アラームリセット信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_RESET)

            Else
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_RESET)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_RESET)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�M↓ -----
#Region "アラームメッセージ表示中信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>アラームメッセージ表示中信号送出(オートローダシリアル通信)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_ALM_DSP()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' アラームメッセージ表示中信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ALM_DSP)
            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ALM_DSP)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_ALM_DSP)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�M↑ -----
#Region "ハンド上下ホーム位置信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド上下ホーム位置信号送出(オートローダシリアル通信)V2.0.0.0�E</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND_HOME()

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' アラーム発生時はローダ側でハンド上下ホーム位置に移動するため下記き削除
            '' ハンド上下ホーム位置信号送出
            'If (giMachineKd = MACHINE_KD_RS) Then
            '    ' SL436S時
            '    ' 該当BITのみを変更するため、現状の値を読込み 
            '    Call m_PlcIf.ReadPlcWR(LOFS_W359S, lData)

            '    If ((lData And LROBS_HAND_HOME) = LROBS_HAND_HOME) Then
            '        ' 現在ONしているならOFFする 
            '        lData = lData And (Not LROBS_HAND_HOME)
            '    Else
            '        ' 現在OFFしているならONする
            '        lData = lData Or LROBS_HAND_HOME
            '    End If
            '    Call m_PlcIf.WritePlcWR(LOFS_W359S, lData)

            'Else
            '    ' SL436R時
            '    Return
            'End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND_HOME)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###144↓ -----
#Region "ハンド１吸着信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド１吸着信号送出(オートローダシリアル通信)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND1_VACUME()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' ハンド１吸着信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND1_VACUME)
            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND1_VACUME)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND1_VACUME)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ハンド１吸着破壊信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド１吸着破壊信号送出(オートローダシリアル通信)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND1_ABSORB()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' ハンド１吸着破壊信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND1_ABSORB)
            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND1_ABSORB)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND1_ABSORB)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ハンド２吸着送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド２吸着送出(オートローダシリアル通信)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND2_VACUME()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' ハンド２吸着信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND2_VACUME)
            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND2_VACUME)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND2_VACUME)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ハンド２吸着破壊信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド２吸着破壊信号送出(オートローダシリアル通信)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND2_ABSORB()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' ハンド２吸着破壊信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND2_ABSORB)
            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND2_ABSORB)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND2_ABSORB)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "クランプＯＮ/ＯＦＦ信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>クランプＯＮ/ＯＦＦ信号送出(オートローダシリアル通信)</summary>
    ''' <param name="OnOff">(INP)1=クランプＯＮ(閉), 0=クランプＯＦＦ(開) V1.16.0.0�D</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_CLMP_ONOFF(ByVal OnOff As Integer)

        Dim lData As Long = 0
        Dim lwData As Long = 0
        Dim strMSG As String
        Dim r As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                'V4.11.0.0�K↓
                r = Form1.System1.ClampCtrl(gSysPrm, OnOff, giTrimErr)         '###060
                If (r <> cFRS_NORMAL) Then

                End If

                '' SL436S時
                ''----- V1.16.0.0�D↓ -----
                '' クランプ状態取得
                'Call m_PlcIf.ReadPlcWR(LOFS_W42S, lData)

                '' クランプＯＮ信号送出
                'If (OnOff = 1) Then                                     ' クランプＯＮ時
                '    ' クランプはOFF ?
                '    If ((lData And LDSTS_CLMP_X_ON) = 0) Then lwData = LDABS_CLMP_X
                '    If ((lData And LDSTS_CLMP_Y_ON) = 0) Then lwData = lwData Or LDABS_CLMP_Y
                '    If (lwData = 0) Then Return '                       ' クランプX,YともONならNOP

                'Else                                                    ' クランプＯＦＦ時
                '    ' クランプはON ?
                '    If ((lData And LDSTS_CLMP_X_ON) = LDSTS_CLMP_X_ON) Then lwData = LDABS_CLMP_X
                '    If ((lData And LDSTS_CLMP_Y_ON) = LDSTS_CLMP_Y_ON) Then lwData = lwData Or LDABS_CLMP_Y
                '    If (lwData = 0) Then Return '                       '  クランプX,YともOFFならNOP
                'End If

                '' クランプＯＮ/ＯＦＦ信号送出
                'Call m_PlcIf.WritePlcWR(LOFS_W53S, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W53S, lwData)
                'V4.11.0.0�K↑

            Else
                ' SL436R時
                ''V5.0.0.8�E↓
                r = Form1.System1.ClampCtrl(gSysPrm, OnOff, giTrimErr)         '###060
                If (r <> cFRS_NORMAL) Then

                End If
                ''V5.0.0.8�E↑        ↓コメント

                ''----- V1.16.0.0�D↓ -----
                '' クランプ状態取得
                'Call m_PlcIf.ReadPlcWR(LOFS_W44, lData)

                '' クランプＯＮ信号送出
                'If (OnOff = 1) Then                                     ' クランプＯＮ時
                '    ' クランプはOFF ?
                '    If ((lData And LDST_CLMP_X_ON) = 0) Then lwData = LDAB_CLMP_X
                '    If ((lData And LDST_CLMP_Y_ON) = 0) Then lwData = lwData Or LDAB_CLMP_Y
                '    If (lwData = 0) Then Return '                       ' クランプX,YともONならNOP

                'Else                                                    ' クランプＯＦＦ時
                '    ' クランプはON ?
                '    If ((lData And LDST_CLMP_X_ON) = LDST_CLMP_X_ON) Then lwData = LDAB_CLMP_X
                '    If ((lData And LDST_CLMP_Y_ON) = LDST_CLMP_Y_ON) Then lwData = lwData Or LDAB_CLMP_Y
                '    If (lwData = 0) Then Return '                       '  クランプX,YともOFFならNOP
                'End If

                '' クランプＯＮ/ＯＦＦ信号送出
                'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W54, lwData)

                ''V5.0.0.8�E↑

                ' クランプＯＮ/ＯＦＦ信号送出
                'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_CLMP_X + LDAB_CLMP_Y)
                '----- V1.16.0.0�D↑ -----
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_CLMP_ONOFF)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "吸着ＯＮ/ＯＦＦ信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>吸着ＯＮ/ＯＦＦ信号送出(オートローダシリアル通信)</summary>
    ''' <param name="OnOff">(INP)1=ＯＮ(閉), 0=ＯＦＦ(開) V1.18.0.0�N</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_VACUME_ONOFF(ByVal OnOff As Integer)

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                '----- V6.0.3.0_37↓ -----
                If gVacuumIO = 1 Then
                    'V4.11.0.0�L↓
                    ZABSVACCUME(OnOff)

                Else
                    '' 吸着状態取得
                    Call m_PlcIf.ReadPlcWR(LOFS_W42S, lData)

                    ' 吸着ＯＮ信号送出
                    If (OnOff = 1) Then                                     ' 吸着ＯＮ時
                        ' 吸着はON ?
                        If ((lData And LDSTS_VACUME) = LDSTS_VACUME) Then
                            Return
                        End If

                        ' 吸着ＯＦＦ信号送出
                    Else                                                    ' 吸着ＯＦＦ時
                        ' 吸着はOFF ?
                        ''V4.1.0.0�P　↓
                        'If ((lData And LDSTS_VACUME) = 0) Then
                        '    Return
                        'End If
                        'V4.1.0.0�P　↑
                    End If

                    ' 吸着ＯＮ/ＯＦＦ信号送出
                    Call m_PlcIf.WritePlcWR(LOFS_W53S, 0)
                    Call m_PlcIf.WritePlcWR(LOFS_W53S, LDABS_VACUME)

                End If
                '----- V6.0.3.0_37↑ -----

            Else
                ' SL436R時
                '----- V1.18.0.0�N↓ -----
                ' 吸着状態取得
          '      Call m_PlcIf.ReadPlcWR(LOFS_W44, lData)

                'V6.0.5.0�E↓
                ' 吸着ＯＮ信号送出
                If (OnOff = 1) Then                                     ' 吸着ＯＮ時
                    'V4.12.2.0�I↓
                    Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着OFF)
                    '' 吸着はON ?
                    'If ((lData And LDST_VACUME) = LDST_VACUME) Then
                    '    Return
                    'End If

                    ' 吸着ＯＦＦ信号送出
                Else                                                    ' 吸着ＯＦＦ時
                    Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF)

                    '' 吸着はOFF ?
                    'If ((lData And LDST_VACUME) = 0) Then
                    '    Return
                    'End If
                    'V4.12.2.0�I↑

                End If
                '----- V1.18.0.0�N↑ -----

                'V4.12.2.0�I↓
                '' 吸着ＯＮ/ＯＦＦ信号送出
                'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_VACUME)
                'V4.12.2.0�I↑ 
                'V6.0.5.0�E↑

            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_VACUME_ONOFF)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "吸着破壊信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>吸着破壊信号送出(オートローダシリアル通信)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_ABSORB()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' 吸着破壊信号送出
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                Call m_PlcIf.WritePlcWR(LOFS_W53S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53S, LDABS_ABSORB)
            Else
                ' SL436R時
                Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_ABSORB)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_ABSORB)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###144↑ -----
#Region "マガジン上下動作" '###182
    '''=========================================================================
    ''' <summary>マガジン上下動作</summary>
    ''' <param name="MgNo">(INP)1=</param>
    ''' <param name="Mode">(INP)MG_UP/MG_DOWN</param>
    '''=========================================================================
    Public Sub MGMoveJog(ByVal MgNo As Integer, ByVal Mode As Integer)

        Dim SerialReadData As Long
        Dim CmpData As Long
        Dim strmsg As String

        SerialReadData = 0

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                '---------------------------------------------------------------
                '   SL436S時
                '---------------------------------------------------------------
                '該当BITのみを変更するため、現状の値を読込み 
                Call m_PlcIf.ReadPlcWR(LOFS_W52S, SerialReadData)

                If (Mode = MG_UP) Then                                  'マガジン上昇
                    CmpData = 0

                    If ((MgNo And &H1) = &H1) Then                      ' MG1
                        CmpData = OutSMG1on_Sw
                        SerialReadData = SerialReadData Or CmpData
                    End If

                    If ((MgNo And &H2) = &H2) Then                      ' MG2
                        CmpData = OutSMG2on_Sw
                        SerialReadData = SerialReadData Or CmpData
                    End If

                    Call m_PlcIf.WritePlcWR(LOFS_W52S, SerialReadData)
                Else                                                    'マガジン下降
                    CmpData = 0

                    If ((MgNo And &H1) = &H1) Then                      ' MG1
                        CmpData = Not OutSMG1on_Sw
                        SerialReadData = SerialReadData And CmpData
                    End If

                    If ((MgNo And &H2) = &H2) Then                      ' MG2
                        CmpData = Not OutSMG2on_Sw
                        SerialReadData = SerialReadData And CmpData
                    End If

                    Call m_PlcIf.WritePlcWR(LOFS_W52S, SerialReadData)
                End If

                CmpData = 0

                If ((MgNo And &H1) = &H1) Then
                    CmpData = OutSMG1Move_Sw
                    SerialReadData = SerialReadData Or CmpData
                End If

                If ((MgNo And &H2) = &H2) Then
                    CmpData = OutSMG2Move_Sw
                    SerialReadData = SerialReadData Or CmpData
                End If

                Call m_PlcIf.WritePlcWR(LOFS_W52S, SerialReadData)

            Else
                '---------------------------------------------------------------
                '   SL436R時
                '---------------------------------------------------------------
                '該当BITのみを変更するため、現状の値を読込み 
                Call m_PlcIf.ReadPlcWR(LOFS_W52, SerialReadData)

                If (Mode = MG_UP) Then                                  'マガジン上昇
                    CmpData = 0

                    If ((MgNo And &H1) = &H1) Then                      ' MG1
                        CmpData = OutMG1on_Sw
                        SerialReadData = SerialReadData Or CmpData
                    End If

                    If ((MgNo And &H2) = &H2) Then                      ' MG2
                        CmpData = OutMG2on_Sw
                        SerialReadData = SerialReadData Or CmpData
                    End If

                    If ((MgNo And &H4) = &H4) Then                      ' MG3
                        CmpData = OutMG3on_Sw
                        SerialReadData = SerialReadData Or CmpData
                    End If

                    If ((MgNo And &H8) = &H8) Then                      ' MG4
                        CmpData = OutMG4on_Sw
                        SerialReadData = SerialReadData Or CmpData
                    End If

                    Call m_PlcIf.WritePlcWR(LOFS_W52, SerialReadData)
                Else                                                    'マガジン下降
                    CmpData = 0

                    If ((MgNo And &H1) = &H1) Then                      ' MG1
                        CmpData = Not OutMG1on_Sw
                        SerialReadData = SerialReadData And CmpData
                    End If

                    If ((MgNo And &H2) = &H2) Then                      ' MG2
                        CmpData = Not OutMG2on_Sw
                        SerialReadData = SerialReadData And CmpData
                    End If
                    If ((MgNo And &H4) = &H4) Then                      ' MG3
                        CmpData = Not OutMG3on_Sw
                        SerialReadData = SerialReadData And CmpData
                    End If

                    If ((MgNo And &H8) = &H8) Then                      ' MG4
                        CmpData = Not OutMG4on_Sw
                        SerialReadData = SerialReadData And CmpData
                    End If

                    Call m_PlcIf.WritePlcWR(LOFS_W52, SerialReadData)
                End If

                CmpData = 0

                If ((MgNo And &H1) = &H1) Then
                    CmpData = OutMG1Move_Sw
                    SerialReadData = SerialReadData Or CmpData
                End If

                If ((MgNo And &H2) = &H2) Then
                    CmpData = OutMG2Move_Sw
                    SerialReadData = SerialReadData Or CmpData
                End If

                If ((MgNo And &H4) = &H4) Then
                    CmpData = OutMG3Move_Sw
                    SerialReadData = SerialReadData Or CmpData
                End If

                If ((MgNo And &H8) = &H8) Then
                    CmpData = OutMG4Move_Sw
                    SerialReadData = SerialReadData Or CmpData
                End If

                Call m_PlcIf.WritePlcWR(LOFS_W52, SerialReadData)

            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strmsg = "LoaderIOFor436.MGMoveJog() TRAP ERROR = " + ex.Message
            MsgBox(strmsg)
        End Try
    End Sub
#End Region

#Region "マガジン上下動作停止" '###182
    '''=========================================================================
    ''' <summary>マガジン上下動作停止</summary>
    '''=========================================================================
    Public Sub MGStopJog()

        Dim SerialReadData As Long
        Dim OutData As Long

        SerialReadData = 0
        OutData = 0
#If cPLCcOFFLINEcDEBUG Then
        Exit Sub
#End If

        If (giMachineKd = MACHINE_KD_RS) Then                           ' V2.0.0.0�E
            ' SL436S時
            ' 該当BITのみを変更するため、現状の値を読込み 
            Call m_PlcIf.ReadPlcWR(LOFS_W52S, SerialReadData)

            OutData = SerialReadData And Not OutSMG1Move_Sw
            OutData = OutData And Not OutMG2Move_Sw

            Call m_PlcIf.WritePlcWR(LOFS_W52S, OutData)

        Else
            ' SL436R時
            '該当BITのみを変更するため、現状の値を読込み 
            Call m_PlcIf.ReadPlcWR(LOFS_W52, SerialReadData)

            OutData = SerialReadData And Not OutMG1Move_Sw
            OutData = OutData And Not OutMG2Move_Sw
            OutData = OutData And Not OutMG3Move_Sw
            OutData = OutData And Not OutMG4Move_Sw

            Call m_PlcIf.WritePlcWR(LOFS_W52, OutData)
        End If

    End Sub
#End Region
    '----- ###188↓ -----
#Region "ハンド１上昇(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド１上昇(オートローダシリアル通信)</summary>
    ''' <remarks>供給ハンドが下がっている場合に上昇させる</remarks>
    '''=========================================================================
    Public Sub W_HAND1_UP()

        Dim lData As Long = 0
        Dim lOutData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                ' ハンド１が上がっているならNOP
                Call m_PlcIf.ReadPlcWR(LOFS_W359S, lData)
                If ((lData And LROBS_HAND_HOME) = LROBS_HAND_HOME) Then Exit Sub

                ' ローダ手動モード切替え
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ローダ出力(ON=なし, OFF=自動)

                ' ハンド１上昇信号送出
                lData = lData Or LROBS_HAND_HOME
                Call m_PlcIf.WritePlcWR(LOFS_W359S, lData)

            Else
                ' SL436R時
                ' ハンド１が上がっているならNOP
                Call m_PlcIf.ReadPlcWR(43, lData)
                If ((lData And LDAB_HAND1_ZMOVE) = 0) Then Exit Sub

                ' ローダ手動モード切替え '###222
                Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ローダ出力(ON=なし, OFF=自動)

                ' ハンド１上昇信号送出
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND1_DOWN)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND1_UP)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ハンド２上昇(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド２上昇(オートローダシリアル通信)</summary>
    ''' <remarks>収納ハンドが下がっている場合に上昇させる</remarks>
    '''=========================================================================
    Public Sub W_HAND2_UP()

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                ' ハンド２が上がっているならNOP
                Call m_PlcIf.ReadPlcWR(LOFS_W359S, lData)
                If ((lData And LROBS_HAND_HOME) = LROBS_HAND_HOME) Then Exit Sub

                ' ローダ手動モード切替え
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ローダ出力(ON=なし, OFF=自動)

                ' ハンド２上昇信号送出
                lData = lData Or LROBS_HAND_HOME
                Call m_PlcIf.WritePlcWR(LOFS_W359S, lData)

            Else
                ' SL436R時
                ' ハンド２が上がっているならNOP
                Call m_PlcIf.ReadPlcWR(43, lData)
                If ((lData And LDAB_HAND2_ZMOVE) = 0) Then Exit Sub

                ' ローダ手動モード切替え '###222
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ローダ出力(ON=なし, OFF=自動)

                ' ハンド２上昇信号送出
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND2_DOWN)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND2_UP)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###188↑ -----
    '----- ###197↓ -----
#Region "ハンド移動(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>ハンド移動(オートローダシリアル通信)</summary>
    ''' <remarks>供給ハンドが下がっている場合に上昇させる</remarks>
    '''=========================================================================
    Public Sub W_HAND_STAGE()

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S時
                ' ハンド搭載位置への移動選択 
                Call m_PlcIf.WritePlcWR(LOFS_W339, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W339, LROB_HAND_STAGE)

                ' ハンド搭載位置へ移動
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W128, POSMOVE_START_ADR)   ' 移動開始BitのON
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)                   ' 移動開始BitのOFF 

            Else
                ' SL436R時
                ' ハンド搭載位置への移動選択 
                Call m_PlcIf.WritePlcWR(LOFS_W339, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W339, LROB_HAND_STAGE)

                ' ハンド搭載位置へ移動
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W128, POSMOVE_START_ADR)   ' 移動開始BitのON
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)                   ' 移動開始BitのOFF 
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(HAND_STAGE)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###197↑ -----
    '----- V1.23.0.0�I↓ -----
#Region "自動運転中断ＯＮ/ＯＦＦ信号送出(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>自動運転中断ＯＮ/ＯＦＦ信号送出(オートローダシリアル通信)</summary>
    ''' <returns>cFRS_NORMAL   = 正常
    '''          cFRS_ERR_LDR  = ローダアラーム検出
    '''          cFRS_ERR_LDR1 = ローダアラーム検出(全停止異常)
    '''          cFRS_ERR_LDR2 = ローダアラーム検出(サイクル停止)
    '''          cFRS_ERR_LDR3 = ローダアラーム検出(軽故障(続行可能))
    ''' </returns> 
    '''=========================================================================
    Public Function Send_AutoStopToLoader() As Integer

        Dim TimerRS As System.Threading.Timer = Nothing
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer
        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            '----- V4.3.0.0�D↓ -----
            ' SL436R時で太陽社版以前(ローム殿, KAMAYA殿)はPLC側が未対応なのでタイムアウトとなる。
            ' PLC側をVersion UPする必要あり。
            '----- V4.3.0.0�D↑ -----

            ' 自動運転中断ON(PC → PLC)信号送出
            Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_STP_AUTODRIVE)

            ' ローダ通信タイムアウトチェック用タイマーオブジェクトの作成(TimerRS_TickをX msec間隔で実行する)
            Sub_SetTimeoutTimer(TimerRS)

            ' ローダの自動運転中断ON(PC → PLC)信号を待つ
            lData = 0
            Do
                ' ローダからの自動運転中断ON(PLC → PC)信号を待つ
                r = W_Read(LOFS_W110, lData)
                If (r <> 0) Then
                    r = cFRS_ERR_LDRTO
                    GoTo STP_END
                End If

                ' ローダ通信タイムアウトチェック 
                If (bFgTimeOut = True) Then                             ' タイムアウト ?
                    ' コールバックメソッドの呼出しを停止する
                    TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                    rtnCode = cFRS_ERR_LDRTO                            ' Return値 = ローダ通信タイムアウト
                    GoTo STP_END
                End If

                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While ((lData And LDPL_STP_AUTODRIVE) <> LDPL_STP_AUTODRIVE)

            ' 自動運転中断OFF(PC → PLC)信号送出
            Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W109, 0)

            ' 終了処理
STP_END:
            ' コールバックメソッドの呼出しを停止する
            If (IsNothing(TimerRS) = False) Then
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' タイマーを破棄する
            End If

            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Send_AutoStopToLoader() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.23.0.0�I↑ -----
    '----- V4.0.0.0�R↓ -----
#Region "サイクル停止信号を送出してローダからのサイクル停止応答を待つ(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>サイクル停止要求信号を送出してローダからのサイクル停止応答を待つ</summary>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェクト</param>
    ''' <param name="OnOff"> (INP)1=サイクル停止信号ON送出, 0=サイクル停止信号OFF送出</param>
    ''' <returns>cFRS_NORMAL    = 正常(基板あり続行)
    '''          cFRS_ERR_START = 正常(基板なし続行)
    '''          cFRS_ERR_RST   = Cancel(RESETキー押下)
    '''          cFRS_ERR_LDR0  = ローダ通信タイムアウト他</returns>
    ''' <remarks>ローム殿特注(SL436R/SL436S)</remarks>
    '''=========================================================================
    Public Function W_CycleStop(ByVal ObjSys As SystemNET, ByVal OnOff As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function W_CycleStop(ByVal ObjSys As Object, ByVal OnOff As Integer) As Integer

        Dim TimerLock As System.Threading.Timer = Nothing
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer = cFRS_NORMAL
        Dim TimeVal As Integer
        Dim bTOut As Boolean
        Dim lData As Long = 0
        Dim LdIn As UShort = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' ローダ無効またはローダ手動モードならNOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If

RETRY_CYCLE:  ''V4.0.0.0-85
            rtnCode = cFRS_NORMAL
            '-------------------------------------------------------------------
            '   サイクル停止ON/OFF信号送信(SL436S/SL436R)
            '-------------------------------------------------------------------
            ' タイマー値を設定する
            If (giOPLDTimeOutFlg = 0) Then                              ' ローダ通信タイムアウト検出無し ?
                TimeVal = System.Threading.Timeout.Infinite             ' タイマー値 = なし
            Else
                TimeVal = giOPLDTimeOut                                 ' タイマー値 = ローダ通信タイムアウト時間(msec)
            End If

            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S時
                If (OnOff = 1) Then                                     ' サイクル停止要求BIT ON ?
                    ' サイクル停止要求BIT ON
                    Call SetLoaderIO(LOUT_CYCL_STOP, &H0)               ' ローダ出力(ON=サイクル停止要求, OFF=なし)
                Else
                    ' サイクル停止要求BIT OFF
                    Call SetLoaderIO(&H0, LOUT_CYCL_STOP)               ' ローダ出力(ON=なし, OFF=サイクル停止要求)
                End If

                ' サイクル停止OFFなら応答待ちしない
                If (OnOff = 0) Then
                    Return (cFRS_NORMAL)
                End If

            Else                                                        ' SL436R時
                ' 送出BITを設定する
                If (OnOff = 1) Then                                     ' サイクル停止ON信号送信 ?
                    ' サイクル停止BIT ON
                    lData = LPC_CYCL_STOP
                Else                                                    ' サイクル停止OFF信号送信
                    ' サイクル停止OFF
                    lData = 0
                End If

                ' サイクル停止ON/OFF信号送信
                Call m_PlcIf.WritePlcWR(LOFS_W113, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W113, lData)
                Console.WriteLine("W_CycleStop() WritePlcWR =" + lData.ToString("0")) ' For Debug
                lData = 0

                ' サイクル停止OFF信号送信なら応答待ちしない
                If (OnOff = 0) Then
                    Return (cFRS_NORMAL)
                End If
            End If

            '-------------------------------------------------------------------
            '   タイムアウトチェック用タイマーオブジェクトの作成
            '   (TimerLock_TickをX msec間隔で実行する)
            '-------------------------------------------------------------------
            TimerTM_Create(TimerLock, TimeVal)

            '-------------------------------------------------------------------
            '   サイクル停止応答待ち(SL436S/SL436R)
            '-------------------------------------------------------------------
            Do
                System.Threading.Thread.Sleep(10)                       ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' サイクル停止応答待ち(SL436S/SL436R)
                If (giMachineKd = MACHINE_KD_RS) Then                   ' SL436S ? 
                    ' ローダアラーム/非常停止チェック
                    r = GetLoaderIO(LdIn)                               ' ローダI/O入力
                    If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                        rtnCode = cFRS_ERR_LDR                          ' Return値 = ローダアラーム検出
                        GoTo STP_ERR_LDR                                ' ローダアラーム表示へ
                    End If
                    ' サイクル停止応答かチェックする(SL436S時)
                    If ((LdIn And LIN_CYCL_STOP) = LIN_CYCL_STOP) Then
                        Exit Do                                         ' サイクル停止応答ならExit
                    End If

                Else
                    ' サイクル停止ON/OFF応答かチェックする(SL436R時)
                    Call m_PlcIf.ReadPlcWR(LOFS_W114, lData)            ' サイクル停止応答取得
                    Console.WriteLine("W_CycleStop() ReadPlcWR =" + lData.ToString("0")) ' For Debug
                    If (lData And LPLC_CYCL_STOP) Then
                        Exit Do                                         ' サイクル停止応答ならExit
                    End If
                End If

                '-------------------------------------------------------------------
                '   タイムアウトチェック 
                '-------------------------------------------------------------------
                bTOut = TimerTM_Sts()
                If (bTOut = True) Then                                  ' タイムアウト ? 
                    rtnCode = cFRS_ERR_LDRTO                            ' Retuen値 = ローダ通信タイムアウト 
                    Exit Do
                End If

            Loop While (1)

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
STP_END:
            TimerTM_Stop(TimerLock)                                     ' コールバックメソッドの呼出しを停止する
            TimerTM_Dispose(TimerLock)                                  ' タイマーを破棄する

            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ローダ通信タイムアウト ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' エラーメッセージ表示
            End If

            Return (rtnCode)

            ' ローダエラー発生時
STP_ERR_LDR:
            TimerTM_Stop(TimerLock)                                     ' コールバックメソッドの呼出しを停止する
            TimerTM_Dispose(TimerLock)                                  ' タイマーを破棄する
            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ローダ通信タイムアウト ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' エラーメッセージ表示
            Else
                ' ローダアラームメッセージ作成 & ローダアラーム画面表示
                rtnCode = Loader_AlarmCheck(ObjSys, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                'V4.0.0.0-85　↓
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' アラームリセット信号送出 
                    Call W_START()                                      ' スタート信号送出 
                    GoTo RETRY_CYCLE
                End If
                'V4.0.0.0-85　↑
            End If
            Return (rtnCode)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.W_CycleStop() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            rtnCode = cERR_TRAP
            GoTo STP_END
        End Try
    End Function
#End Region

#Region "割欠チェック(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>割欠チェック(オートローダシリアル通信)</summary>
    ''' <returns>cFRS_NORMAL    = 正常(割欠なし)
    '''          cFRS_ERR_RST   = 割欠検出
    '''          </returns>
    '''=========================================================================
    Public Function R_BREAK_XY() As Integer

        Dim r As Integer
        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' 割欠チェック
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ? 
                r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
                System.Threading.Thread.Sleep(500)                      ' 500msec待って再度リードする
                r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
                If ((lData And LDSTS_BREAK_X_ON) = 0) And ((lData And LDSTS_BREAK_Y_ON) = 0) Then
                    Return (cFRS_NORMAL)                                ' 割欠検出でなければ正常リターン
                End If
                Return (cFRS_ERR_RST)

            Else                                                        ' SL436R時
                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                System.Threading.Thread.Sleep(500)                      ' 500msec待って再度リードする
                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                If ((lData And LDST_BREAK_X_ON) = 0) And ((lData And LDST_BREAK_Y_ON) = 0) Then
                    Return (cFRS_NORMAL)                                ' 割欠検出でなければ正常リターン
                End If
                Return (cFRS_ERR_RST)
            End If

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(R_BREAK_XY)"
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R↑ -----
    '----- V4.0.0.0-26↓ -----
#Region "品種番号を送出する(オートローダシリアル通信)"
    '''=========================================================================
    ''' <summary>品種番号を送出する(オートローダシリアル通信)</summary>
    ''' <param name="OnOff">0=品種番号初期化, 1=品種番号を送出する</param>
    ''' <returns>cFRS_NORMAL    = 正常
    '''          上記以外       = エラー
    '''          </returns>
    '''=========================================================================
    Public Function W_SubstrateType(ByVal OnOff As Integer) As Integer

        Dim r As Integer
        Dim lData As Long = 0
        Dim lTpData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' 基板品種番号を送出する(SL436S/SL436R)
            If (giMachineKd <> MACHINE_KD_RS) Then                                  ' SL436R ?
                ' ■SL436R時

            Else
                ' ■SL436S時
                ' 基板品種番号と基板タイプを設定する
                If (OnOff = 1) Then                                                 ' 品種番号を送出 ?
                    lData = typPlateInfo.intWorkSetByLoader                         ' 基板品種番号(1-10)
                    lTpData = glSubstrateType(typPlateInfo.intWorkSetByLoader - 1)  ' 薄基板対応(0=通常, 1=薄基板(スルーホール))
                Else
                    lData = 0                                                       ' 基板品種番号初期化
                    lTpData = 0                                                     ' 薄基板対応(0=通常)
                End If

                ' 基板品種番号を送出する
                r = DM_Write(LOFS_D241S, lData)                                     ' 品種選択(D241 1(品種1)〜10(品種10))
                ' 基板タイプを送出する
                r = DM_Write(LOFS_D230S, lTpData)                                   ' 現在の基板種別(D230 0=通常, 1=薄基板)
            End If

            Return (cFRS_NORMAL)

            ' トラップエラー発生時(エラーメッセージはex.Messageに設定される) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_SubstrateType)"
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26↑ -----
    '===============================================================================
    '   システムパラメータＩＯ処理
    '===============================================================================
#Region "シスパラよりローダ用各種設定値を設定する"
    '''=========================================================================
    ''' <summary>シスパラよりローダ用各種設定値を設定する V4.0.0.0-26</summary>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_PutParameterFromSysparam() As Integer

        Dim r As Integer
        Dim Idx As Integer
        Dim lData As Long = 0
        Dim lAdr As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            '-------------------------------------------------------------------
            '   初期データをローダ側に送信する(SL436S)
            '-------------------------------------------------------------------
            If (giMachineKd <> MACHINE_KD_RS) Then                      ' SL436SでなければNOP
                Return (cFRS_NORMAL)
            End If

            ' 品種選択を初期化する
            lData = 0
            r = DM_Write(LOFS_D241S, lData)                             ' 品種選択初期化(D241 1(品種1)〜10(品種10))
            r = DM_Write(LOFS_W360S, lData)                             ' 品種選択BIT初期化(W360.00-W360.15)

            ' 基板種別を初期化する
            lData = 0
            r = DM_Write(LOFS_D230S, lData)                             ' 現在の基板種別(D230 0=通常, 1=薄基板)

            ' 二枚取りセンサ確認位置を設定する
            For Idx = 0 To (MAXWORK_KND - 1)
                lData = gfTwoSubPickChkPos(Idx)                         ' 二枚取りセンサ確認位置座標(mm)
                r = DM_Write2W(LOFS_D700S + Idx * 2, lData)             ' 座標データ1-10(D700-D718 符号付2WORD)
            Next Idx

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_PutParameterFromSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "システムパラメータからローダ部タイマー値を書き込む"
    '''=========================================================================
    ''' <summary>システムパラメータからローダ部タイマー値を書き込む</summary>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_PutTimerValFromSysparam() As Integer

        'Dim r As Integer
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' タイマー値をシステムパラメータから読込む
            'r = Loader_GetTimerValFromSysparam(LTimerDT())

            ' ローダ部タイマー値を書き込む　
            ' ※処理を追加する


            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_PutTimerValFromSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "システムパラメータからローダ部タイマー値を読込む"
    '''=========================================================================
    ''' <summary>システムパラメータからローダ部タイマー値を読込む</summary>
    ''' <param name="LTimerDT">(OUT)タイマー値読込み域</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_GetTimerValFromSysparam(ByRef LTimerDT() As UShort) As Integer

        Dim sPath As String                                                     ' ファイル名
        Dim sSect As String                                                     ' セクション名
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' タイマー値をシステムパラメータから読込む　　※↓以下修正する
            sPath = SysParamPath                                                ' ファイル名
            sSect = "LOADER_TIMER"                                              ' セクション名
            LTimerDT(0) = GetPrivateProfileInt(sSect, "D4032", 2, sPath)        ' 薄物押付供給ハンド1回目下降　→　吸着OFF
            LTimerDT(1) = GetPrivateProfileInt(sSect, "D4033", 5, sPath)        ' 薄物押付供給ハンド吸着OFF　→　載物台クランプ
            LTimerDT(2) = GetPrivateProfileInt(sSect, "D4034", 2, sPath)        ' 薄物押付載物台クランプ　→　供給ハンド上昇
            LTimerDT(3) = GetPrivateProfileInt(sSect, "D4035", 2, sPath)        ' 薄物押付供給ハンド上昇　→　供給ハンド2回目下降
            LTimerDT(4) = GetPrivateProfileInt(sSect, "D4036", 2, sPath)        ' 薄物押付供給ハンド2回目下降　→　載物台クランプ解除
            LTimerDT(5) = GetPrivateProfileInt(sSect, "D4037", 2, sPath)        ' 薄物押付載物台クランプ開　→　通常下降
            LTimerDT(6) = GetPrivateProfileInt(sSect, "D4038", 0, sPath)        ' 供給ハンド通常下降　→　フラグ処理及び吸着OFF
            LTimerDT(7) = GetPrivateProfileInt(sSect, "D4039", 2, sPath)        ' 供給ハンド通常吸着OFF　→　供給ハンド上昇
            LTimerDT(8) = GetPrivateProfileInt(sSect, "D4040", 10, sPath)       ' 下限ON直後、薄物押付け時の強制破壊
            LTimerDT(9) = GetPrivateProfileInt(sSect, "D4041", 1, sPath)        ' 供給ハンド真空破壊時間
            LTimerDT(10) = GetPrivateProfileInt(sSect, "D4042", 10, sPath)      ' 供給ハンド真空破壊時間（X2D）
            LTimerDT(11) = GetPrivateProfileInt(sSect, "D4045", 3, sPath)       ' 載物台クランプON　→　吸着ON
            LTimerDT(12) = GetPrivateProfileInt(sSect, "D4046", 1, sPath)       ' 載物台吸着ON　→　割欠／2枚取りの以上検出待ち
            LTimerDT(13) = GetPrivateProfileInt(sSect, "D4049", 2, sPath)       ' 収納ハンド下降　→　載物台クランプ解除
            LTimerDT(14) = GetPrivateProfileInt(sSect, "D4050", 1, sPath)       ' 載物台クランプ解除　→　吸着OFF
            LTimerDT(15) = GetPrivateProfileInt(sSect, "D4051", 2, sPath)       ' 載物台吸着OFF　→　収納ハンド上昇
            LTimerDT(16) = GetPrivateProfileInt(sSect, "D4052", 1, sPath)       ' 載物台クランプ解除　→　収納ハンド上昇
            LTimerDT(17) = GetPrivateProfileInt(sSect, "D4053", 5, sPath)       ' 供給マガジンエアブロー時間
            LTimerDT(18) = GetPrivateProfileInt(sSect, "D4055", 10, sPath)      ' 供給ハンド上限　→　吸着ミスの異常検出待ち
            LTimerDT(19) = GetPrivateProfileInt(sSect, "D4056", 10, sPath)      ' 載物台吸着ON　→　吸着ミスの異常検出待ち
            LTimerDT(20) = GetPrivateProfileInt(sSect, "D4057", 10, sPath)      ' 収納ハンド上限　→　吸着ミスの異常検出待ち
            LTimerDT(21) = GetPrivateProfileInt(sSect, "D4058", 5, sPath)       ' 収納エレベータ下降時間

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_GetTimerValFromSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ローダ部タイマー値をシステムパラメータに書込む"
    '''=========================================================================
    ''' <summary>ローダ部タイマー値をシステムパラメータに書込む</summary>
    ''' <param name="LTimerDT">(INP)タイマー値の配列</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Loader_PutTimerValToSysparam(ByRef LTimerDT() As UShort) As Integer

        Dim sPath As String                                                     ' ファイル名
        Dim sSect As String                                                     ' セクション名
        Dim s As String
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' ローダ部タイマー値をシステムパラメータに書込む
            sPath = SysParamPath                                                ' ファイル名
            sSect = "LOADER_TIMER"                                              ' セクション名

            s = CStr(LTimerDT(0))
            Call WritePrivateProfileString(sSect, "D4032", s, sPath)            ' 薄物押付供給ハンド1回目下降　→　吸着OFF
            s = CStr(LTimerDT(1))
            Call WritePrivateProfileString(sSect, "D4033", s, sPath)            ' 薄物押付供給ハンド吸着OFF　→　載物台クランプ
            s = CStr(LTimerDT(2))
            Call WritePrivateProfileString(sSect, "D4034", s, sPath)            ' 薄物押付載物台クランプ　→　供給ハンド上昇
            s = CStr(LTimerDT(3))
            Call WritePrivateProfileString(sSect, "D4035", s, sPath)            ' 薄物押付供給ハンド上昇　→　供給ハンド2回目下降
            s = CStr(LTimerDT(4))
            Call WritePrivateProfileString(sSect, "D4036", s, sPath)            ' 薄物押付供給ハンド2回目下降　→　載物台クランプ解除
            s = CStr(LTimerDT(5))
            Call WritePrivateProfileString(sSect, "D4037", s, sPath)            ' 薄物押付載物台クランプ開　→　通常下降
            s = CStr(LTimerDT(6))
            Call WritePrivateProfileString(sSect, "D4038", s, sPath)            ' 供給ハンド通常下降　→　フラグ処理及び吸着OFF
            s = CStr(LTimerDT(7))
            Call WritePrivateProfileString(sSect, "D4039", s, sPath)            ' 供給ハンド通常吸着OFF　→　供給ハンド上昇
            s = CStr(LTimerDT(8))
            Call WritePrivateProfileString(sSect, "D4040", s, sPath)            ' 下限ON直後、薄物押付け時の強制破壊
            s = CStr(LTimerDT(9))
            Call WritePrivateProfileString(sSect, "D4041", s, sPath)            ' 供給ハンド真空破壊時間
            s = CStr(LTimerDT(10))
            Call WritePrivateProfileString(sSect, "D4042", s, sPath)            ' 供給ハンド真空破壊時間（X2D）
            s = CStr(LTimerDT(11))
            Call WritePrivateProfileString(sSect, "D4045", s, sPath)            ' 載物台クランプON　→　吸着ON
            s = CStr(LTimerDT(12))
            Call WritePrivateProfileString(sSect, "D4046", s, sPath)            ' 載物台吸着ON　→　割欠／2枚取りの以上検出待ち
            s = CStr(LTimerDT(13))
            Call WritePrivateProfileString(sSect, "D4049", s, sPath)            ' 収納ハンド下降　→　載物台クランプ解除
            s = CStr(LTimerDT(14))
            Call WritePrivateProfileString(sSect, "D4050", s, sPath)            ' 載物台クランプ解除　→　吸着OFF
            s = CStr(LTimerDT(15))
            Call WritePrivateProfileString(sSect, "D4051", s, sPath)            ' 載物台吸着OFF　→　収納ハンド上昇
            s = CStr(LTimerDT(16))
            Call WritePrivateProfileString(sSect, "D4052", s, sPath)            ' 載物台クランプ解除　→　収納ハンド上昇
            s = CStr(LTimerDT(17))
            Call WritePrivateProfileString(sSect, "D4053", s, sPath)            ' 供給マガジンエアブロー時間
            s = CStr(LTimerDT(18))
            Call WritePrivateProfileString(sSect, "D4055", s, sPath)            ' 供給ハンド上限　→　吸着ミスの異常検出待ち
            s = CStr(LTimerDT(19))
            Call WritePrivateProfileString(sSect, "D4056", s, sPath)            ' 載物台吸着ON　→　吸着ミスの異常検出待ち
            s = CStr(LTimerDT(20))
            Call WritePrivateProfileString(sSect, "D4057", s, sPath)            ' 収納ハンド上限　→　吸着ミスの異常検出待ち
            s = CStr(LTimerDT(21))
            Call WritePrivateProfileString(sSect, "D4058", s, sPath)            ' 収納エレベータ下降時間

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_PutTimerValToSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "その他のシスパラを読込む"
    '''=========================================================================
    ''' <summary>その他のシスパラを読込む</summary>
    ''' <param name="sPath">(INP)ファイル名</param>
    ''' <param name="sSect">(INP)セクション名</param>
    ''' <param name="sKey"> (INP)キー名</param>
    ''' <param name="Data"> (OUT)データ</param>
    ''' <returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function Get_SystemParameterShort(ByRef sPath As String, ByRef sSect As String, ByRef sKey As String, ByRef Data As Short) As Integer

        Dim strMSG As String

        Try
            Data = GetPrivateProfileInt(sSect, sKey, 0, sPath)
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Get_SystemParameterShort() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    ''' <summary>
    ''' PLCのローバッテリーアラーム監視
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CheckPLCLowBatteryAlarm()
        Dim r As Integer
        Dim lData As Long

        If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
            ' SL436S時
            r = DM_Read(LOFS_D242S, lData)
            If (lData <> 0) Then
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH50, MSG_SPRASH51, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            End If
        End If

    End Sub

    'V4.9.0.0�@↓
    ''' <summary>
    ''' Lot中断処理BITをON/OFFする    ' W113.02をON/OFFする
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub SetLotStopBit(ByVal mode As Integer)
#If cPLCcOFFLINEcDEBUG Then
        Exit Sub
#End If
        Dim lData As Long = 0

        Call m_PlcIf.ReadPlcWR(LOFS_W113, lData)

        If (mode = 1) Then
            lData = lData + LPC_LOT_STOP
        Else
            lData = lData And (Not LPC_LOT_STOP)
        End If

        Call m_PlcIf.WritePlcWR(LOFS_W113, lData)

    End Sub
    'V4.9.0.0�@↑

    ''V4.11.0.0�G↓
    ''' <summary>
    ''' Lot中断処理準備要求BITをON/OFFする    ' W113.03をON/OFFする
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub SetLotStopRequestBit(ByVal mode As Integer)

        Dim lData As Long = 0

        Call m_PlcIf.ReadPlcWR(LOFS_W113, lData)

        If (mode = 1) Then
            lData = lData + LPC_LOT_STOP_PREPARE
        Else
            lData = lData And (Not LPC_LOT_STOP_PREPARE)
        End If

        Call m_PlcIf.WritePlcWR(LOFS_W113, lData)

    End Sub

    ''' <summary>
    ''' LotStopReady信号を待つ
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function WaitLotStopReady() As Integer
        Dim lData As Integer
        Dim rtnCode As Integer
        Dim TimerRS As System.Threading.Timer = Nothing
        Dim LdIn As UShort
        Dim r As Integer

STP_RETRY:  'V5.0.0.1�F

        Sub_SetTimeoutTimer(TimerRS)

        Do
            ' ローダアラーム/非常停止チェック
            GetLoaderIO(LdIn)                                   ' ローダ
            If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                rtnCode = cFRS_ERR_LDR                              ' Return値 = ローダアラーム検出
                GoTo STP_ERR_LDR 'V5.0.0.1�F
            End If

            Call m_PlcIf.ReadPlcWR(LOFS_W114S, lData)               ' Lot停止要求応答取得
            If (lData And LPLCS_SET_LOTSTOPREADY) Then
                rtnCode = cFRS_NORMAL
                Exit Do
            End If

            ' ローダ通信タイムアウトチェック 
            If (bFgTimeOut = True) Then                             ' タイムアウト ?
                ' コールバックメソッドの呼出しを停止する
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                rtnCode = cFRS_ERR_LDRTO                            ' Return値 = ローダ通信タイムアウト
                Exit Do
            End If

            System.Windows.Forms.Application.DoEvents()
            Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
        Loop While (True)

STP_ERR_LDR:
        ' コールバックメソッドの呼出しを停止する
        TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        TimerRS.Dispose()                                           ' タイマーを破棄する
        If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ローダ通信タイムアウト ?
            r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_TMOUT)     ' エラーメッセージ表示
        ElseIf (rtnCode = cFRS_ERR_LDR) Then
            ' ローダアラームメッセージ作成 & ローダアラーム画面表示
            r = Loader_AlarmCheck(Form1.System1, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            'V5.0.0.1�F
            If (r = cFRS_ERR_LDR3) Then
                Call W_START()                                  ' スタート信号送出 
                GoTo STP_RETRY
                'V5.0.0.1�F
                '    'V5.0.0.1-31↓
                'ElseIf (r = cFRS_ERR_LDR1) Then
                '    rtnCode = r
            End If
            ''V5.0.0.1-31↑
        End If

        Return (rtnCode)

    End Function


    ''' <summary>
    ''' Lot中断処理時基板有無BITをON/OFFする    ' W113.05をON/OFFする
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Function SetSubExistBit(ByVal mode As Integer) As Integer

        Dim lData As Long = 0
        Dim r As Integer
        Dim strMSG As String

        SetSubExistBit = cFRS_ERR_RST

        Try
            Call m_PlcIf.ReadPlcWR(LOFS_W113, lData)
            If (mode = 1) Then
                giSubExistMsgFlag = False
                '基板有無チェック
                r = Form1.SubstrateExistCheck(Form1.System1)
                giSubExistMsgFlag = True
                If (r = cFRS_NORMAL) Then                              ' エラー ? 
                    lData = lData Or LPC_LOT_STOP_SUB_EXIST
                    ClampAndVacuum(1)
                    SetSubExistBit = cFRS_NORMAL
                Else
                    lData = lData And (Not LPC_LOT_STOP_SUB_EXIST)
                End If
            Else
                lData = lData And (Not LPC_LOT_STOP_SUB_EXIST)
            End If

            Call m_PlcIf.WritePlcWR(LOFS_W113, lData)
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SetSubExistBit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try


    End Function


    ''' <summary>
    ''' Lot中断処理時基板有無BITをON/OFFする    ' W113.05をON/OFFする
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub ClampAndVacuum(ByVal mode As Integer)

        Dim lData As Long = 0
        'V5.0.0.4�@        Dim r As Integer

        If (mode = 1) Then
            '----- V1.16.0.0�K↓ -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                '                    Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            End If
            '----- V1.16.0.0�K↑ -----
            '            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
            W_VACUME_ONOFF(1)
        Else
            '----- V1.16.0.0�K↓ -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' クランプON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' クランプON
                '                    Call W_CLMP_ONOFF(0)                                    ' クランプOFF
            End If
            W_VACUME_ONOFF(0)
            '----- V1.16.0.0�K↑ -----
            '            Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着ON)
        End If

    End Sub
    ''V4.11.0.0�G↑


#End Region
    'V4.11.0.0�G
    ''' <summary>
    ''' 吸着状態をチェックしてIO状態を合わせる
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VacuumeStateCheck()
#If cPLCcOFFLINEcDEBUG Then
        Exit Sub
#End If
        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then Return 'V6.0.0.1�D

        Dim readData As Long

        m_PlcIf.ReadPlcCIO(5, readData)
        If readData And &H4 Then
            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
        Else
            Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着ON)

        End If

    End Sub



    ''V4.11.0.0�G↓
    ''' <summary>
    ''' W113をOFFする    
    ''' </summary>
    Public Sub Clear113Bit()
#If cPLCcOFFLINEcDEBUG Then
        Exit Sub
#End If
        Dim lData As Long = 0

        lData = 0

        Call m_PlcIf.WritePlcWR(LOFS_W113, lData)

    End Sub

End Module