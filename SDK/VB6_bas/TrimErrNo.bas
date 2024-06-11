Attribute VB_Name = "TrimErrNo"
'===============================================================================
'   Description : エラーコードの定義
'
'   Copyright(C) OMRON Laser Front 2011
'
'===============================================================================

'===============================================================================
'   軸定義
'===============================================================================
'----- 座標軸番号 -----
Public Const AXIS_X = 0                                         ' X軸
Public Const AXIS_Y = 1                                         ' Y軸
Public Const AXIS_Z = 2                                         ' Z軸
Public Const AXIS_T = 3                                         ' Theta軸
Public Const AXIS_Z2 = 4                                        ' Z2軸
Public Const AXIS_XY = 5                                        ' XY軸同時指定

'----- 座標軸ビット指定 -----
Public Const STATUS_BIT_AXIS_X = &H1                            ' X軸
Public Const STATUS_BIT_AXIS_Y = &H2                            ' Y軸
Public Const STATUS_BIT_AXIS_Z = &H4                            ' Z軸
Public Const STATUS_BIT_AXIS_T = &H8                            ' Theta軸
Public Const STATUS_BIT_AXIS_Z2 = &H10                          ' Z2軸

'----- 全軸のステータスビット(GET_ALLAXIS_STATUS関数のステータスビット) -----
Public Const BIT_AXIS_X_SERVO_ALM = &H1000                      ' X軸サーボアラーム
Public Const BIT_AXIS_Y_SERVO_ALM = &H2000                      ' Y軸サーボアラーム
Public Const BIT_AXIS_Z_SERVO_ALM = &H4000                      ' Z軸サーボアラーム
Public Const BIT_AXIS_T_SERVO_ALM = &H8000                      ' Theta軸サーボアラーム
Public Const BIT_AXIS_X_PLUS_LIMIT = &H1                        ' X軸＋リミット
Public Const BIT_AXIS_Y_PLUS_LIMIT = &H2                        ' Y軸＋リミット
Public Const BIT_AXIS_Z_PLUS_LIMIT = &H4                        ' Z軸＋リミット
Public Const BIT_AXIS_T_PLUS_LIMIT = &H8                        ' Theta軸＋リミット
Public Const BIT_AXIS_X_MINUS_LIMIT = &H10                      ' X軸－リミット
Public Const BIT_AXIS_Y_MINUS_LIMIT = &H20                      ' Y軸－リミット
Public Const BIT_AXIS_Z_MINUS_LIMIT = &H40                      ' Z軸－リミット
Public Const BIT_AXIS_T_MINUS_LIMIT = &H80                      ' Theta軸－リミット



'===============================================================================
'   i-TKY系/ユーザプロのエラーコード（戻値）定義(0番～100番)
'===============================================================================
Public Const cFRS_NORMAL = 0                                    ' 正常
Public Const cFRS_ERR_ADV = 1                                   ' OK(ADV(STARTｷｰ))← START/RESET待ち時
Public Const cFRS_ERR_START = 1                                 ' OK(STARTｷｰ)     ← START/RESET待ち時
Public Const cFRS_ERR_RST = 3                                   ' Cancel(RESETｷｰ) ← START/RESET待ち時
'Public Const cFRS_ERR_HALT = 4                                 ' HALTｷｰ
Public Const cFRS_ERR_HALT = 2                                  ' HALTｷｰ
Public Const cFRS_ERR_Z = 4                                     ' Zｷｰ
Public Const cFRS_TxTy = 5                                      ' TX2/TY2押下
Public Const cFRS_ERR_NOP = 9                                   ' その他(OcxJogから呼出元への上記以外の戻り)

Public Const cFRS_ERR_CVR = -1                                  ' 筐体カバー開検出
Public Const cFRS_ERR_SCVR = -2                                 ' スライドカバー開検出
Public Const cFRS_ERR_LATCH = -3                                ' カバー開ラッチ検出

Public Const cFRS_ERR_EMG = -11                                 ' 非常停止
Public Const cFRS_ERR_DUST = -12                                ' 集塵機異常検出
Public Const cFRS_ERR_AIR = -13                                 ' エアー圧エラー検出
Public Const cFRS_ERR_MVC = -14                                 ' ﾏｽﾀｰﾊﾞﾙﾌﾞ回路状態エラー検出
Public Const cFRS_ERR_HW = -15                                  ' ハードウェアエラー検出
Public Const cFRS_ERR_LDR = -16                                 ' ローダアラーム検出
Public Const cFRS_ERR_LDR1 = -17                                ' ローダアラーム検出(全停止異常)
Public Const cFRS_ERR_LDR2 = -18                                ' ローダアラーム検出(サイクル停止)
Public Const cFRS_ERR_LDR3 = -19                                ' ローダアラーム検出(軽故障)
Public Const cFRS_ERR_LDRTO = -20                               ' ローダ通信タイムアウト

'----- IO制御タイムアウト -----
Public Const cFRS_TO_SCVR_CL = -21                              ' タイムアウト(スライドカバー閉待ち)
Public Const cFRS_TO_SCVR_OP = -22                              ' タイムアウト(スライドカバー開待ち)
Public Const cFRS_TO_SCVR_ON = -23                              ' タイムアウト(ｽﾗｲﾄﾞｶﾊﾞｰｽﾄｯﾊﾟｰ行待ち)
Public Const cFRS_TO_SCVR_OFF = -24                             ' タイムアウト(ｽﾗｲﾄﾞｶﾊﾞｰｽﾄｯﾊﾟｰ戻待ち)
Public Const cFRS_TO_CLAMP_ON = -25                             ' タイムアウト(クランプＯＮ)
Public Const cFRS_TO_CLAMP_OFF = -26                            ' タイムアウト(クランプＯＦＦ)
Public Const cFRS_TO_PM_DW = -27                                ' タイムアウト(パワーメータ下降移動)
Public Const cFRS_TO_PM_UP = -28                                ' タイムアウト(パワーメータ上昇移動)
Public Const cFRS_TO_PM_FW = -29                                ' タイムアウト(パワーメータ測定端移動)
Public Const cFRS_TO_PM_BK = -30                                ' タイムアウト(パワーメータ待機端移動)

'----- ファイル入出力エラー -----
Public Const cFRS_FIOERR_INP = -31                              ' ファイル入力エラー
Public Const cFRS_FIOERR_OUT = -32                              ' ファイル出力エラー
Public Const cFRS_COM_ERR = -33                                 ' 通信エラー(PLC)
Public Const cFRS_COM_FL_ERR = -34                              ' 通信エラー(FL)
Public Const cFRS_FL_ERR_INP = -35                              ' ＦＬ側に加工条件の設定がない
Public Const cFRS_FL_ERR_SET = -36                              ' 加工条件番号設定エラー

'----- Video.OCXのエラー -----
Public Const cFRS_VIDEO_PTN = -40                               ' パターン認識エラー
Public Const cFRS_VIDEO_PT1 = -41                               ' パターン認識エラー(補正位置1)
Public Const cFRS_VIDEO_PT2 = -42                               ' パターン認識エラー(補正位置2)
Public Const cFRS_VIDEO_COM = -43                               ' 通信エラー(CV3000)
Public Const cFRS_VIDEO_INI = -44                               ' 初期化が行われていません
Public Const cFRS_VIDEO_IN2 = -45                               ' 初期化済み
Public Const cFRS_VIDEO_FRM = -46                               ' フォーム表示中
Public Const cFRS_VIDEO_PRP = -47                               ' プロパティ値不正
Public Const cFRS_VIDEO_GRP = -48                               ' ﾃﾝﾌﾟﾚｰﾄｸﾞﾙｰﾌﾟ番号ｴﾗｰ
Public Const cFRS_VIDEO_MXT = -49                               ' テンプレート数 > MAX
Public Const cFRS_VIDEO_UXP = -50                               ' 予期せぬエラー
Public Const cFRS_VIDEO_UX2 = -51                               ' 予期せぬエラー2
Public Const cFRS_MVC_UTL = -52                                 ' MvcUtil エラー
Public Const cFRS_MVC_PT2 = -53                                 ' MvcPt2 エラー
Public Const cFRS_MVC_10 = -54                                  ' Mvc10 エラー

'----- その他のエラー -----
Public Const cFRS_ERR_SHUTTER = -60                             ' 外部シャッターエラー検出 ###801⑤
Public Const cFRS_ERR_PLC = -61                                 ' PLCステータス異常

'----- V8.0.0.15①(OcxSystem)↓ -----
'----- パラメータエラー等(メッセージ表示はしない) -----
Public Const cFRS_ERR_CMD_NOTSPT = -70                          ' 未サポートコマンド
Public Const cFRS_ERR_CMD_PRM = -71                             ' パラメータエラー
Public Const cFRS_ERR_CMD_LIM_L = -72                           ' パラメータ下限値エラー
Public Const cFRS_ERR_CMD_LIM_U = -73                           ' パラメータ上限値エラー
Public Const cFRS_ERR_CMD_OBJ = -74                             ' オブジェクト未設定(Utilityｵﾌﾞｼﾞｪｸﾄ他)
Public Const cFRS_ERR_CMD_EXE = -75                             ' コマンド実行エラー(DllTrimFncから99で返ってくるもの)
'                                                               ' (注)cFRS_ERR_CMD_EXE～cFRS_ERR_CMD_NOTSPTで判定している箇所があるため
'                                                               ' 　　追加する場合は注意(cFRS_ERR_CMD_EXEをずらして番号を振り直す)

'----- V8.0.0.15①(OcxSystem)↑ -----

'----- 画面処理用の戻り値 -----                                 ' -80番代と-90番代はUsrpro,TKY等の画面処理で使用
Public Const cFRS_ERR_HING = -80                                ' 連続NG-HIGHｴエラー発生
Public Const cFRS_ERR_REPROB = -81                              ' 再プロービング失敗

Public Const cFRS_FNG_DATA = -85                                ' データ未ロード
Public Const cFRS_FNG_CMD = -86                                 ' 他コマンド実行中
Public Const cFRS_FNG_PASS = -87                                ' ﾊﾟｽﾜｰﾄﾞ入力ｴﾗｰ
Public Const cFRS_FNG_CPOS = -88                                ' カット位置補正対象の抵抗がない

Public Const cFRS_TRIM_NG = -90                                 ' トリミングNG
Public Const cFRS_ERR_TRIM = -91                                ' トリマエラー
Public Const cFRS_ERR_PTN = -92                                 ' パターン認識エラー(パターンが見つからなかった)
Public Const cFRS_ERR_PT2 = -93                                 ' パターン認識エラー(閾値エラー)

'----- その他のエラー -----
Public Const cERR_TRAP = -99                                    ' ﾄﾗｯﾌﾟｴﾗｰ発生

'----- INtime側エラー(100番以降)は下記のエラーコード× -1 で返す


'===============================================================================
'   INtime側エラーコード定義(100番以降)
'===============================================================================
'-----( エラーステータス )-----
'  トリムブロック処理時のエラー (LastErrorに設定し、dwerrnoでWindows側へ返信)
Public Const ERR_CLEAR = 0                                      ' リミット状態クリア（リミットエラーなど）
Public Const ERR_ALREADY_SET = 1
Public Const ERR_TERMINATE = 2                                  ' ホストより中断終了
Public Const ERR_RESET = 3                                      ' リセット終了
Public Const ERR_CONTACT = 5                                    ' コンタクトエラー

' ソフトウェアエラー
'Public Const ERR_EMGSWCH = 102                                  ' 非常停止
'Public Const ERR_SRV_ALM = 103                                  ' サーボアラーム
'Public Const ERR_AXS_LIM = 104                                  ' XYZ軸リミット検出

'ステージエラー
'  タイムアウトエラーについては、ベース+対称軸ビット(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
'  のエラーコードで返す。
'Public Const ERR_TIMEOUT_BASE = 100                             ' タイムアウトエラー(ベース番号)
                                                                ' X軸タイムアウト=101
                                                                ' Y軸タイムアウト=102
                                                                ' Z軸タイムアウト=104
                                                                ' Theta軸タイムアウト=108
                                                                ' Z2軸タイムアウト=116
                                                                ' 複数軸はorの値
                                                                ' X,Y軸タイムアウト=103

'Public Const ERR_TIMEOUT_X = 101                               ' X軸タイムアウト
'Public Const ERR_TIMEOUT_Y = 102                               ' Y軸タイムアウト
'Public Const ERR_TIMEOUT_Z = 103                               ' Z軸タイムアウト
'Public Const ERR_TIMEOUT_T = 104                               ' θ軸タイムアウト
'Public Const ERR_SOFTLIMIT_X = 105                            ' X軸ソフトリミット
'Public Const ERR_SOFTLIMIT_Y = 106                            ' Y軸ソフトリミット
'Public Const ERR_SOFTLIMIT_Z = 107                            ' Z軸ソフトリミット
'Public Const ERR_TIMEOUT_ATT = 108                              ' ATT ← タイムアウト下に移動
'Public Const ERR_TIMEOUT_Z2 = 109                              ' Z2軸タイムアウト
'Public Const ERR_BP_XLIMIT = 110                               ' BP X軸可動範囲オーバー（未使用）
'Public Const ERR_BP_YLIMIT = 111                               ' BP Y軸可動範囲オーバー(未使用）
'Public Const ERR_BP_MOVE_TIMEOUT = 112                         ' BP タイムアウト

'----- ソフトリミットエラー(OcxSystemで使用) -----
Public Const ERR_SOFTLIMIT_X = 105                              ' X軸ソフトリミット
Public Const ERR_SOFTLIMIT_Y = 106                              ' Y軸ソフトリミット
Public Const ERR_SOFTLIMIT_Z = 107                              ' Z軸ソフトリミット
Public Const ERR_SOFTLIMIT_Z2 = 113                             ' Z2軸ソフトリミット
Public Const ERR_BP_XLIMIT = 110                                ' BP X軸ソフトリミットエラー
Public Const ERR_BP_YLIMIT = 111                                ' BP Y軸ソフトリミットエラー
Public Const ERR_SOFTLIMIT_T = 112                              ' THETA軸ソフトリミット '###801①

Public Const ERR_INTIME_BASE = 100                              ' INtime側エラーベース番号(INtime側エラーコードの最小値)
                                                                ' 200にしたいがERR_TIMEOUT_ATT（108）がいきている
'----- ハードウェアエラー -----
Public Const ERR_EMGSWCH = 201                                  ' 非常停止
Public Const ERR_SRV_ALM = 202                                  ' サーボアラーム
'Public Const ERR_AXS_LIM = 203                                 ' XYZ軸リミット検出 （実際には未使用）
Public Const ERR_AXS_LIM_X = 203                                ' X軸リミット検出
Public Const ERR_AXS_LIM_Y = 204                                ' Y軸リミット検出
Public Const ERR_AXS_LIM_Z = 205                                ' Z軸リミット検出
Public Const ERR_AXS_LIM_T = 206                                ' θ軸リミット検出
Public Const ERR_AXS_LIM_ATT = 207                              ' ATT軸リミット検出
Public Const ERR_AXS_LIM_Z2 = 208                               ' Z2軸リミット検出
Public Const ERR_OPN_CVR = 209                                  ' 筐体カバー開検出
Public Const ERR_OPN_SCVR = 210                                 ' スライドカバー開検出
Public Const ERR_OPN_CVRLTC = 211                               ' カバー開ラッチ検出
Public Const ERR_INTERLOCK_STSCHG = 212                         ' インターロック状態の変更

Public Const ERR_AXS_LIM = 220                                  ' 各軸のリミット検出(リミット到達中)
    
Public Const ERR_HARD_FAILURE = 250                             ' ハードウェアの致命的な故障

Public Const ERR_CLAMP_MOVE_TIMEOUT = 260                       ' クランプ タイムアウト

'----- ロジカルエラー -----
Public Const ERR_CMD_NOTSPT = 301                               ' 未サポートコマンド
Public Const ERR_CMD_PRM = 302                                  ' パラメータエラー
Public Const ERR_CMD_LIM_L = 303                                ' パラメータ下限値エラー
Public Const ERR_CMD_LIM_U = 304                                ' パラメータ上限値エラー
Public Const ERR_RT2WIN_SEND = 305                              ' INTime→Windows送信エラー
Public Const ERR_RT2WIN_RECV = 306                              ' INTime→Windows受信エラー
Public Const ERR_WIN2RT_SEND = 307                              ' Windows→INTime送信エラー
Public Const ERR_WIN2RT_RECV = 308                              ' Windows→INTime受信エラー
Public Const ERR_SYS_BADPOINTER = 309                           ' 不正ポインタ
Public Const ERR_SYS_FREE_MEMORY = 310                          ' メモリ領域の開放エラー
Public Const ERR_SYS_ALLOC_MEMORY = 311                         ' メモリ領域の確保エラー
Public Const ERR_CALC_OVERFLOW = 320                            ' オーバーフロー

Public Const ERR_INTIME_NOTMOVE = 350                           ' INTRIMが起動していない

'----- ハードウェアIO系エラーコード：400番台 -----
Public Const ERR_CSLLAMP_SETNO = 401                            ' コンソール系ランプ番号指定エラー
Public Const ERR_SIGTWRLAMP_SETNO = 402                         ' シグナルタワーランプ番号指定エラー
Public Const ERR_SIGTWRLAMP_SETMODENO = 402                     ' シグナルタワーランプ点灯/点滅モード指定エラー
Public Const ERR_BIT_ONOFF = 403                                ' ビットオン/オフ指定エラー
Public Const ERR_SETPRM_WAITSWNON = 404                         ' 入力待ち対象スイッチの指定が全てなしになっている。

'----- 測定系エラーコード：500番台 -----
Public Const ERR_MEAS_RANGESET_TYPE = 501                       ' 測定レンジ設定エラー：指定レンジ設定タイプなし
Public Const ERR_MEAS_SETRNG_NO = 502                           ' 測定レンジ設定エラー：対象レンジなし
Public Const ERR_MEAS_SETRNG_LO = 503                           ' 測定レンジ設定エラー：最小レンジ以下
Public Const ERR_MEAS_SETRNG_HI = 504                           ' 測定レンジ設定エラー：最大レンジ以上
Public Const ERR_MEAS_RNG_NOTSET = 505                          ' 測定レンジ設定エラー：レンジ未設定
Public Const ERR_MEAS_FAIL = 506                                ' 測定エラー
Public Const ERR_MEAS_FAST_R = 507                              ' 高速度抵抗測定エラー
Public Const ERR_MEAS_HIGHPRECI_R = 508                         ' 高精度抵抗測定エラー
Public Const ERR_MEAS_FAST_V = 509                              ' 高速度電圧測定エラー
Public Const ERR_MEAS_HIGHPRECI_V = 510                         ' 高精度電圧測定エラー
Public Const ERR_MEAS_TARGET = 511                              ' 測定目標値設定エラー
Public Const ERR_MEAS_TARGET_LO = 512                           ' 測定目標値設定エラー：最小目標値以下
Public Const ERR_MEAS_TARGET_HI = 513                           ' 測定目標値設定エラー：最大目標値以下
Public Const ERR_MEAS_SCANNER = 514                             ' 測定スキャナ設定エラー：不正スキャナ番号
Public Const ERR_MEAS_SCANNER_LO = 515                          ' 測定スキャナ設定エラー：最小スキャナ番号以下
Public Const ERR_MEAS_SCANNER_HI = 516                          ' 測定スキャナ設定エラー：最大スキャナ番号以上
Public Const ERR_MEAS_RNG_SHORT = 517                           ' 測定値：レンジショート（0.01以下）
Public Const ERR_MEAS_RNG_OVER = 518                            ' 測定値：レンジオーバー（67000000.0以上）
Public Const ERR_MEAS_SPAN = 519                                ' 測定範囲外
Public Const ERR_MEAS_SPAN_SHORT = 520                          ' 測定範囲外：ショート-抵抗(0x6666)/電圧(0x3333)以下
Public Const ERR_MEAS_SPAN_OVER = 521                           ' 測定範囲外：オーバー-抵抗(0xCCCC)/電圧(0x6666)以上
Public Const ERR_MEAS_INVALID_SLOPE = 522                       ' スロープ設定エラー
Public Const ERR_MEAS_COUNT = 523                               ' 測定回数指定エラー
Public Const ERR_MEAS_SETMODE = 524                             ' 測定時の設定モード（Mfsetモード）エラー
Public Const ERR_MEAS_AUTORNG_OVER = 525                        ' オートレンジ測定:定電流測定範囲オーバー（差電流領域）
Public Const ERR_MEAS_K2VAL_SHORT = 526                         ' 差電流測定:K2設定値0.4未満
Public Const ERR_MEAS_K2VAL_OVER = 527                          ' 差電流測定:K2設定値0.8以上
Public Const ERR_MEAS_SCANSET_TIMEOUT = 528                     ' スキャナ設定完了タイムアウト

'----- BP系エラーコード：600番台 -----
Public Const ERR_BP_MAXLINEANUM_LO = 601                        ' リニアリティ補正：最小番号以下
Public Const ERR_BP_MAXLINEANUM_HI = 602                        ' リニアリティ補正：最大番号以上
Public Const ERR_BP_LOGICALNUM_LO = 603                         ' 象限設定：最小象限値番号以下
Public Const ERR_BP_LOGICALNUM_HI = 604                         ' 象限設定：最大象限値番号以下
Public Const ERR_BP_LIMITOVER = 605                             ' BP移動距離設定：リミットオーバー
Public Const ERR_BP_HARD_LIMITOVER_LO = 606                     ' BP移動距離設定：リミットオーバー（最小可動範囲以下）
Public Const ERR_BP_HARD_LIMITOVER_HI = 607                     ' BP移動距離設定：リミットオーバー（最大可動範囲以上）
Public Const ERR_BP_SOFT_LIMITOVER = 608                        ' ソフト可動範囲オーバー
Public Const ERR_BP_BSIZE_OVER = 609                            ' ブロックサイズ設定オーバー（ソフト可動範囲オーバー）
Public Const ERR_BP_SIZESET = 610                               ' BPサイズ設定エラー
Public Const ERR_BP_MOVE_TIMEOUT = 611                          ' BP タイムアウト
Public Const ERR_BP_GRV_ALARM_X = 620                           ' ガルバノアラームX
Public Const ERR_BP_GRV_ALARM_Y = 621                           ' ガルバノアラームY
Public Const ERR_BP_GRVMOVE_HARDERR = 622                       ' ポジショニング動作時のガルバノ動作異常（指令値と現在値不一致）
Public Const ERR_BP_GRVSET_AXIS = 630                           ' ポジショニング動作時の座標設定or座標取得エラー
Public Const ERR_BP_GRVSET_MOVEMODE = 631                       ' ガルバノモード設定時の動作モード設定値不正（ポジショニング=0、トリミング=1以外の値が設定されている）
Public Const ERR_BP_GRVSET_SPEEDSHORT = 632                     ' ガルバノ速度指定：最小スピード未満
Public Const ERR_BP_GRVSET_SPEEDOVER = 633                      ' ガルバノ速度指定：最大スピードオーバー
Public Const ERR_BP_GRVSET_MISMATCH = 634                       ' ポジショニング動作時のガルバノ動作異常（設定値に対し設定された値(ハードから読出した値）が不一致）

'----- トリミング/カット系エラーコード:700番台 -----
Public Const ERR_CUT_NOT_SUPPORT = 701                          ' 未サポートカット形状
Public Const ERR_CUT_PARAM_LEN = 702                            ' カットパラメータエラー：カット長設定エラー
Public Const ERR_CUT_PARAM_LEN_LO = 703                         ' カットパラメータエラー：カット長最小値以下
Public Const ERR_CUT_PARAM_LEN_HI = 704                         ' カットパラメータエラー：カット長最大値以上
Public Const ERR_CUT_PARAM_CORR = 706                           ' カットパラメータエラー：パラメータ相関エラー
Public Const ERR_CUT_PARAM_SPD = 707                            ' カットパラメータエラー：スピード設定エラー
Public Const ERR_CUT_PARAM_SPD_LO = 708                         ' カットパラメータエラー：スピード設定オーバー
Public Const ERR_CUT_PARAM_SPD_HI = 709                         ' カットパラメータエラー：スピード設定ショート
Public Const ERR_CUT_PARAM_DIR = 710                            ' カットパラメータエラー：カット方向
Public Const ERR_CUT_PARAM_CUTCNT = 711                         ' カットパラメータエラー：カット本数
Public Const ERR_CUT_PARAM_CHRSIZE = 712                        ' カットパラメータエラー：文字サイズ指定エラー
Public Const ERR_CUT_PARAM_CUTANGLE = 713                       ' カットパラメータエラー：角度指定エラー
Public Const ERR_CUT_PARAM_CHARSET = 714                        ' カットパラメータエラー：文字列指定エラー
Public Const ERR_CUT_PARAM_STRLEN = 715                         ' カットパラメータエラー：マーキング文字列長指定エラー
Public Const ERR_CUT_PARAM_NOHAR = 716                          ' カットパラメータエラー：指定文字なし
Public Const ERR_CUT_PARAM_NORES = 717                          ' カットパラメータエラー：抵抗番号不正
Public Const ERR_CUT_PARAM_CUTMODE = 718                        ' カットパラメータエラー：カットモード不正（CUT_MODE_NORMAL(1)～CUT_MODE_NANAME(4)以外指定）
Public Const ERR_CUT_PARAM_PARCENT = 719                        ' カットパラメータエラー：ポイント設定
Public Const ERR_CUT_PARAM_BP = 720

Public Const ERR_CUT_RATIOPRM_BASERNO = 730                     ' レシオパラメータエラー：計算用ベース抵抗番号指定エラー
Public Const ERR_CUT_RATIOPRM_BASER_NG = 731                    ' レシオパラメータエラー：計算用ベース抵抗判定NG
Public Const ERR_CUT_RATIOPRM_MODENOT2 = 732                    ' レシオパラメータエラー：計算用ベース抵抗がRatioモード2でない
Public Const ERR_CUT_RATIOPRM_CALCFORM = 733                    ' レシオパラメータエラー：計算用式が不正
Public Const ERR_CUT_RATIOPRM_CANTSET = 734                     ' レシオパラメータエラー：データの登録が出来ない
Public Const ERR_CUT_RATIOPRM_GRPNO = 735                       ' レシオパラメータエラー：グループ番号指定エラー

Public Const ERR_CUT_UCUTPRM_NOPRMRES = 740                     ' Uカットパラメータエラー：抵抗情報パラメータエリアの設定なし
Public Const ERR_CUT_UCUTPRM_NOPRMCUT = 741                     ' Uカットパラメータエラー：カット情報パラメータエリアの設定なし
Public Const ERR_CUT_UCUTPRM_NORES = 742                        ' Uカットパラメータエラー：対象抵抗なし
Public Const ERR_CUT_UCUTPRM_NODATA = 743                       ' Uカットパラメータエラー：パラメータデータが設定されていない
Public Const ERR_CUT_UCUTPRM_NOMODE = 744                       ' Uカットパラメータエラー：指定モードなし
Public Const ERR_CUT_UCUTPRM_TBLIDX = 745                       ' Uカットパラメータエラー：テーブルのインデックスが不正

Public Const ERR_CUT_CIRCUIT_NO = 750                           ' サーキットパラメータエラー：サーキット番号指定エラー

Public Const ERR_TRIMRESULT_TESTNO = 760                        ' トリミング結果：取得対象結果番号指定エラー（テスト実施数より大きい）
Public Const ERR_TRIMRESULT_RESNO = 761                         ' トリミング結果：取得対象抵抗開始番号指定エラー（登録抵抗数より大きい開始番号）
Public Const ERR_TRIMRESULT_TOTALRESNO = 762                    ' トリミング結果：取得対象抵抗番号指定エラー（登録抵抗数より大きい）
Public Const ERR_TRIMRESULT_CUTNO = 763                         ' トリミング結果：取得対象カット開始番号指定エラー（最大カット数より大きい開始番号）
Public Const ERR_TRIMRESULT_TOTALCUTNO = 764                    ' トリミング結果：取得対象カット番号指定エラー（最大カット数より大きい開始番号）
Public Const ERR_TRIMRESULT_ESRESCUTNO = 765                    ' トリミング結果：取得対象抵抗番号orカット番号指定エラー（最大カット数より大きい開始番号）

Public Const ERR_TRIM_COTACTCHK = 770                           ' コンタクトチェックエラー
Public Const ERR_TRIM_OPNSRTCHK = 771                           ' オープンショートチェックエラー
Public Const ERR_TRIM_CIR_NORES = 772                           ' サーキットトリミング-次対象抵抗なし

Public Const ERR_MARKVFONT_ANGLE = 780                          ' フォント設定：角度指定エラー
Public Const ERR_MARKVFONT_MAXPOINT = 781                       ' フォント設定：ポイント数オーバー

Public Const ERR_TRIMMODE_UNMATCH = 790                         ' トリミングモード不一致：レシオトリミング対象があるのにディレイモードが設定されている

'----- レーザ系エラーコード:800番台 -----
Public Const ERR_LSR_PARAM_ONOFF = 801                          ' レーザON/OFFパラメータエラー
Public Const ERR_LSR_PARAM_QSW = 802                            ' Qレートパラメータエラー
Public Const ERR_LSR_PARAM_QSW_LO = 803                         ' Qレートパラメータエラー：最小Qレート以下
Public Const ERR_LSR_PARAM_QSW_HI = 804                         ' Qレートパラメータエラー：最小Qレート以上
Public Const ERR_LSR_PARAM_FLCUTCND = 830                       ' FL用加工条件番号設定エラー
Public Const ERR_LSR_PARAM_FLPRM = 831                          ' FL用パラメータエラー（制御種別指定エラー）
Public Const ERR_LSR_PARAM_FLTIMEOUT = 832                      ' FL用設定タイムアウト
Public Const ERR_LSR_STATUS_STANBY = 833                        ' FL:Stanby error,ES:POWER ON READY error
Public Const ERR_LSR_STATUS_OSCERR = 850                        ' FL:Error occured,:ES:LD Alarm

'----- ステージ系エラーコード: -----
Public Const ERR_STG_STATUS = 901                               ' ステージ系でステータスエラー発生
Public Const ERR_STG_NOT_EXIST = 902                            ' 対象ステージ(X/Y/Z/Z2/Theta)存在せず。
Public Const ERR_STG_ORG_NONCOMPLETION = 903                    ' 原点復帰未完了
'Public Const ERR_SOFTLIMIT_X = 105                             ' X軸ソフトリミット
'Public Const ERR_SOFTLIMIT_Y = 106                             ' Y軸ソフトリミット
'Public Const ERR_SOFTLIMIT_Z = 107                             ' Z軸ソフトリミット
Public Const ERR_STG_MOVESEQUENCE = 905                         ' ステージ動作シーケンス設定エラー
Public Const ERR_STG_THETANOTEXIST = 906                        ' θ載物台なし
Public Const ERR_STG_INIT_X = 910                               ' X軸原点復帰エラー
Public Const ERR_STG_INIT_Y = 911                               ' Y軸原点復帰エラー
Public Const ERR_STG_INIT_Z = 912                               ' Z軸原点復帰エラー
Public Const ERR_STG_INIT_T = 913                               ' Theta軸原点復帰エラー
Public Const ERR_STG_INIT_Z2 = 914                              ' Z2軸原点復帰エラー
Public Const ERR_STG_MOVE_X = 915                               ' X軸ステージ動作異常：指令値 != 移動位置
Public Const ERR_STG_MOVE_Y = 916                               ' Y軸ステージ動作異常：指令値 != 移動位置
Public Const ERR_STG_MOVE_Z = 917                               ' Z軸ステージ動作異常：指令値 != 移動位置
Public Const ERR_STG_MOVE_T = 918                               ' Theta軸ステージ動作異常：指令値 != 移動位置
Public Const ERR_STG_MOVE_Z2 = 919                              ' Z2軸ステージ動作異常：指令値 != 移動位置

Public Const ERR_STG_SOFTLMT_PLUS = 930                         ' プラスリミットオーバー
Public Const ERR_STG_SOFTLMT_MINUS = 931                        ' マイナスリミットオーバー

'----- タイムアウトエラー -----
'  タイムアウトエラーについては、ベース+対称軸ビット(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
'  のエラーコードで返す。
Public Const ERR_TIMEOUT_BASE = 950
                                                                ' タイムアウトエラー(ベース番号)
                                                                ' X軸タイムアウト=951
                                                                ' Y軸タイムアウト=952
                                                                ' Z軸タイムアウト=954
                                                                ' Theta軸タイムアウト=958
                                                                ' Z2軸タイムアウト=966
                                                                ' 複数軸はorの値
                                                                ' X,Y軸タイムアウト=953
                                                                                                                           
Public Const ERR_AXIS_X_BASE = 1000                             ' X軸系エラーベース番号
Public Const ERR_AXIS_Y_BASE = 2000                             ' Y軸系エラーベース番号
Public Const ERR_AXIS_Z_BASE = 3000                             ' Z軸系エラーベース番号
Public Const ERR_AXIS_T_BASE = 4000                             ' Theta軸系エラーベース番号
Public Const ERR_AXIS_Z2_BASE = 5000                            ' Z2軸系エラーベース番号

                                                                                                                      
Public Const ERR_TIMEOUT_AXIS_X = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_X                      ' X軸タイムアウト
Public Const ERR_TIMEOUT_AXIS_Y = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Y                      ' Y軸タイムアウト
Public Const ERR_TIMEOUT_AXIS_Z = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Z                      ' Z軸タイムアウト
Public Const ERR_TIMEOUT_AXIS_T = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_T                      ' θ軸タイムアウト
Public Const ERR_TIMEOUT_AXIS_Z2 = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Z2                    ' Z2軸タイムアウト
Public Const ERR_TIMEOUT_AXIS_XY = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_X + STATUS_BIT_AXIS_Y ' XY軸タイムアウト
Public Const ERR_TIMEOUT_ATT = 108                                                          ' ロータリアッテネータタイムアウト

Public Const ERR_AXIS_X_SERVO_ALM = ERR_AXIS_X_BASE + 0                                     ' X軸サーボアラーム
Public Const ERR_AXIS_Y_SERVO_ALM = ERR_AXIS_Y_BASE + 0                                     ' Y軸サーボアラーム
Public Const ERR_AXIS_Z_SERVO_ALM = ERR_AXIS_Z_BASE + 0                                     ' Z軸サーボアラーム
Public Const ERR_AXIS_T_SERVO_ALM = ERR_AXIS_T_BASE + 0                                     ' θ軸サーボアラーム

Public Const ERR_STG_SOFTLMT_PLUS_AXIS_X = ERR_AXIS_X_BASE + ERR_STG_SOFTLMT_PLUS           ' X軸プラスリミットオーバー
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Y = ERR_AXIS_Y_BASE + ERR_STG_SOFTLMT_PLUS           ' Y軸プラスリミットオーバー
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Z = ERR_AXIS_Z_BASE + ERR_STG_SOFTLMT_PLUS           ' Z軸プラスリミットオーバー
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_T = ERR_AXIS_T_BASE + ERR_STG_SOFTLMT_PLUS           ' θ軸プラスリミットオーバー
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Z2 = ERR_AXIS_Z2_BASE + ERR_STG_SOFTLMT_PLUS         ' Z2軸プラスリミットオーバー

Public Const ERR_STG_SOFTLMT_MINUS_AXIS_X = ERR_AXIS_X_BASE + ERR_STG_SOFTLMT_MINUS         ' X軸マイナスリミットオーバー
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Y = ERR_AXIS_Y_BASE + ERR_STG_SOFTLMT_MINUS         ' Y軸マイナスリミットオーバー
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Z = ERR_AXIS_Z_BASE + ERR_STG_SOFTLMT_MINUS         ' Z軸マイナスリミットオーバー
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_T = ERR_AXIS_T_BASE + ERR_STG_SOFTLMT_MINUS         ' θ軸マイナスリミットオーバー
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Z2 = ERR_AXIS_Z2_BASE + ERR_STG_SOFTLMT_MINUS       ' Z2軸マイナスリミットオーバー

'----- IO系エラーコード: -----
Public Const ERR_IO_PCINOTFOUND = 10000                         ' PCIボードが検出できず
Public Const ERR_IO_NOTGET_RTCCMOSDATA = 10001                  ' RTCﾃﾞｰﾀ読み込み失敗

'----- GPIB系エラーコード -----
Public Const ERR_GPIB_PARAM = 11001                             ' GPIBパラメータエラー
Public Const ERR_GPIB_TCPSOCKET = 11002                         ' GPIB-TCP/IP送信エラー
Public Const ERR_GPIB_EXEC = 11003                              ' GPIBコマンド実行エラー


