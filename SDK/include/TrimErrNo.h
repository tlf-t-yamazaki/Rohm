/***************************************************************************//**
 * @file        TrimErrNo.h
 * @brief       エラー番号定義ファイル
 *
 * @par     (C) Copyright OMRONLASERFRONT INC. 2010-2012 All Rights Reserved
 ******************************************************************************/
#ifndef	__TRIM_ERR_NO_H__
#define	__TRIM_ERR_NO_H__

//========================================================================
// Error No.
//========================================================================
//-----( エラーステータス )-----
//	LastErrorに設定し、dwerrnoでWindows側へ返信
#define	ERR_CLEAR					0			///< リミット状態クリア（リミットエラーなど）
#define ERR_ALREADY_SET				1
//#define ERR_TERMINATE				2			///< ホストより中断終了
//#define ERR_RESET					3			///< リセット終了
//#define ERR_CONTACT				5			///< コンタクトエラー

// ソフトウェアエラー
//#define	ERR_EMGSWCH				102			///< 非常停止
//#define	ERR_SRV_ALM				103			///< サーボアラーム
//#define	ERR_AXS_LIM				104			///< XYZ軸リミット検出

// ステージエラー
// タイムアウトエラーについては、ベース+対称軸ビット(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
// のエラーコードで返す。
#if DOXYGEN_SHOULD_SKIP_THIS
/*#define ERR_TIMEOUT_BASE			100		*/	/**< タイムアウトエラー(ベース番号)
												 * - X軸タイムアウト=101
												 * - Y軸タイムアウト=102
												 * - Z軸タイムアウト=104
												 * - Theta軸タイムアウト=108
												 * - Z2軸タイムアウト=116
												 * - 複数軸はorの値
												 * - X,Y軸タイムアウト=103 */
#endif /* DOXYGEN_SHOULD_SKIP_THIS */
//#define	ERR_TIMEOUT_X				101			///< X軸タイムアウト
//#define	ERR_TIMEOUT_Y				102			///< Y軸タイムアウト
//#define	ERR_TIMEOUT_Z				103			///< Z軸タイムアウト
//#define	ERR_TIMEOUT_T				104			///< θ軸タイムアウト
/*///#define	ERR_SOFTLIMIT_X				105			///< X軸ソフトリミット */
/*///#define	ERR_SOFTLIMIT_Y				106			///< Y軸ソフトリミット */
/*///#define	ERR_SOFTLIMIT_Z				107			///< Z軸ソフトリミット */
#define	ERR_TIMEOUT_ATT				108			///< ATTタイムアウト 
//#define	ERR_TIMEOUT_Z2				109			///< Z2軸タイムアウト
//#define	ERR_BP_XLIMIT				110			///< BP X軸可動範囲オーバー（未使用）
//#define	ERR_BP_YLIMIT				111			///< BP Y軸可動範囲オーバー(未使用）
//#define ERR_BP_MOVE_TIMEOUT			112			///< BP タイムアウト

// ハードウェアエラー
#define	ERR_EMGSWCH					201			///< 非常停止
#define	ERR_SRV_ALM					202			///< サーボアラーム
//#define	ERR_AXS_LIM				203			///< XYZ軸リミット検出 （実際には未使用）
#define	ERR_AXS_LIM_X				203			///< X軸リミット検出 
#define	ERR_AXS_LIM_Y				204			///< Y軸リミット検出 
#define	ERR_AXS_LIM_Z				205			///< Z軸リミット検出 
#define	ERR_AXS_LIM_T				206			///< θ軸リミット検出
#define	ERR_AXS_LIM_ATT				207			///< ATT軸リミット検出
#define	ERR_AXS_LIM_Z2				208			///< Z2軸リミット検出
#define	ERR_OPN_CVR					209			///< 筐体カバー開検出
#define	ERR_OPN_SCVR				210			///< スライドカバー開検出
#define	ERR_OPN_CVRLTC				211			///< カバー開ラッチ検出
#define ERR_INTERLOCK_STSCHG		212			///< インターロック状態の変更検出
#define ERR_CLAMPPRM_UNMATCH		213			///< (SL436系のみ)クランプON-OFFパラメータ不一致。(X=0,Y=1やX=1,Y=0と設定されている)
#define	ERR_AXS_LIM					220			///< 各軸のリミット検出
#define ERR_HARD_FAILURE			250			///< ハードウェアの致命的故障


#define ERR_PROBE_UP_ERROR			251			///< プローブ上昇エラー		//V4.21.0.0③
#define ERR_PROBE_DOWN_ERROR		252			///< プローブ下降エラー		//V4.21.0.0③
#define	ERR_AXS_PLUS_LIM_ATT		253			///< ATT軸＋リミット検出	//V4.21.0.0④
#define	ERR_AXS_MINUS_LIM_ATT		254			///< ATT軸－リミット検出	//V4.21.0.0④
#define ERR_CMD_NOTSPT				301			///< 未サポートコマンド
#define ERR_CMD_PRM					302			///< パラメータエラー
#define ERR_CMD_LIM_L				303			///< パラメータ下限値エラー
#define ERR_CMD_LIM_U				304			///< パラメータ上限値エラー
#define ERR_RT2WIN_SEND				305			///< INTime→Windows送信エラー
#define ERR_RT2WIN_RECV				306			///< INTime→Windows受信エラー
#define ERR_WIN2RT_SEND				307			///< Windows→INTime送信エラー
#define ERR_WIN2RT_RECV				308			///< Windows→INTime受信エラー
#define ERR_SYS_BADPOINTER			309			///< 不正ポインタ
#define ERR_SYS_FREE_MEMORY			310			///< メモリ領域の開放エラー
#define ERR_SYS_ALLOC_MEMORY		311			///< メモリ領域の確保エラー
#define ERR_CALC_OVERFLOW			320			///< オーバーフロー
#define ERR_INTIME_NOTMOVE			350			///< INTRIMが起動していない

// ハードウェアIO系エラーコード：400番台
#define ERR_CSLLAMP_SETNO			401			///< コンソール系ランプ番号指定エラー
#define ERR_SIGTWRLAMP_SETNO		402			///< シグナルタワーランプ番号指定エラー
#define ERR_SIGTWRLAMP_SETMODENO	403			///< シグナルタワーランプ点灯/点滅モード指定エラー
#define ERR_BIT_ONOFF				404			///< ビットオン/オフ指定エラー
#define ERR_SETPRM_WAITSWNON		405			///< 入力待ち対象スイッチの指定が全てなしになっている。
#define ERR_START_SOFT_KEY			406			///< ハードSTARTキーでない時に呼んではいけない処理を呼んだ。//V4.5.0.0①

// 測定系エラーコード：500番台
#define ERR_MEAS_RANGESET_TYPE		501			///< 測定レンジ設定エラー：指定レンジ設定タイプなし
#define ERR_MEAS_SETRNG_NO			502			///< 測定レンジ設定エラー：対象レンジなし
#define ERR_MEAS_SETRNG_LO			503			///< 測定レンジ設定エラー：最小レンジ以下
#define ERR_MEAS_SETRNG_HI			504			///< 測定レンジ設定エラー：最大レンジ以上
#define ERR_MEAS_RNG_NOTSET			505			///< 測定レンジ設定エラー：レンジ未設定
#define ERR_MEAS_FAIL				506			///< 測定エラー
#define ERR_MEAS_FAST_R				507			///< 高速度抵抗測定エラー
#define ERR_MEAS_HIGHPRECI_R		508			///< 高精度抵抗測定エラー
#define ERR_MEAS_FAST_V				509			///< 高速度電圧測定エラー
#define ERR_MEAS_HIGHPRECI_V		510			///< 高精度電圧測定エラー
#define ERR_MEAS_TARGET				511			///< 測定目標値設定エラー
#define ERR_MEAS_TARGET_LO			512			///< 測定目標値設定エラー：最小目標値以下
#define ERR_MEAS_TARGET_HI			513			///< 測定目標値設定エラー：最大目標値以下
#define ERR_MEAS_SCANNER			514			///< 測定スキャナ設定エラー：不正スキャナ番号
#define ERR_MEAS_SCANNER_LO			515			///< 測定スキャナ設定エラー：最小スキャナ番号以下
#define ERR_MEAS_SCANNER_HI			516			///< 測定スキャナ設定エラー：最大スキャナ番号以上
#define ERR_MEAS_RNG_SHORT			517			///< 測定値：レンジショート（0.01以下）
#define ERR_MEAS_RNG_OVER			518			///< 測定値：レンジオーバー（67000000.0以上）
#define ERR_MEAS_SPAN				519			///< 測定範囲外
#define ERR_MEAS_SPAN_SHORT			520			///< 測定範囲外：ショート-抵抗(0x6666)/電圧(0x3333)以下
#define ERR_MEAS_SPAN_OVER			521			///< 測定範囲外：オーバー-抵抗(0xCCCC)/電圧(0x6666)以上
#define ERR_MEAS_INVALID_SLOPE		522			///< スロープ設定エラー
#define ERR_MEAS_COUNT				523			///< 測定回数指定エラー
#define ERR_MEAS_SETMODE			524			///< 測定時の設定モード（Mfsetモード）エラー
#define ERR_MEAS_AUTORNG_OVER		525			///< オートレンジ測定:定電流測定範囲オーバー（差電流領域）
#define ERR_MEAS_K2VAL_SHORT		526			///< 差電流測定:K2設定値0.4未満
#define ERR_MEAS_K2VAL_OVER			527			///< 差電流測定:K2設定値0.8以上
#define ERR_MEAS_SCANSET_TIMEOUT	528			///< スキャナ設定完了タイムアウト
#define ERR_MEAS_RANGESET_TIMEOUT	529			///< レンジ設定回数をオーバした場合のタイムアウト @@@009
#define ERR_MEAS_CV 				530			///< 測定ばらつき検出 @@@80010
#define ERR_MEAS_OVERLOAD			531			///< オーバロード検出 @@@80010
#define ERR_MEAS_REPROBING			532			///< 再プロービングエラー @@@80010

// BP系エラーコード：600番台
#define ERR_BP_MAXLINEANUM_LO		601			///< リニアリティ補正：最小番号以下
#define ERR_BP_MAXLINEANUM_HI		602			///< リニアリティ補正：最大番号以上
#define ERR_BP_LOGICALNUM_LO		603			///< 象限設定：最小象限値番号以下
#define ERR_BP_LOGICALNUM_HI		604			///< 象限設定：最大象限値番号以下
#define ERR_BP_LIMITOVER			605			///< BP移動距離設定：リミットオーバー
#define ERR_BP_HARD_LIMITOVER_LO	606			///< BP移動距離設定：リミットオーバー（最小可動範囲以下）
#define ERR_BP_HARD_LIMITOVER_HI	607			///< BP移動距離設定：リミットオーバー（最大可動範囲以上）
#define ERR_BP_SOFT_LIMITOVER		608			///< ソフト可動範囲オーバー
#define ERR_BP_BSIZE_OVER			609			///< ブロックサイズ設定オーバー（ソフト可動範囲オーバー）
#define	ERR_BP_SIZESET				610			///< BPサイズ設定エラー
#define ERR_BP_MOVE_TIMEOUT			611			///< BP タイムアウト
#define ERR_BP_GRV_ALARM_X			620			///< ガルバノアラームX
#define ERR_BP_GRV_ALARM_Y			621			///< ガルバノアラームY
#define ERR_BP_GRVMOVE_HARDERR		622			///< ポジショニング動作時のガルバノ動作異常（指令値と現在座標が不一致）
#define ERR_BP_GRVSET_AXIS			630			///< ポジショニング動作時の座標設定or座標取得エラー
#define ERR_BP_GRVSET_MOVEMODE		631			///< ガルバノモード設定時の動作モード設定値不正（ポジショニング=0、トリミング=1以外の値が設定されている）
#define ERR_BP_GRVSET_SPEEDSHORT	632			///< ガルバノ速度指定：最小スピード未満
#define ERR_BP_GRVSET_SPEEDOVER		633			///< ガルバノ速度指定：最大スピードオーバー
#define ERR_BP_GRVSET_MISMATCH		634			///< ポジショニング動作時のガルバノ動作異常（設定値に対し設定された値(ハードから読出した値）が不一致）

// トリミング/カット系エラーコード:700番台
#define ERR_CUT_NOT_SUPPORT			701			///< 未サポートカット形状
#define ERR_CUT_PARAM_LEN			702			///< カットパラメータエラー：カット長設定エラー
#define ERR_CUT_PARAM_LEN_LO		703			///< カットパラメータエラー：カット長最小値以下
#define ERR_CUT_PARAM_LEN_HI		704			///< カットパラメータエラー：カット長最大値以上
#define ERR_CUT_PARAM_CORR			706			///< カットパラメータエラー：パラメータ相関エラー
#define ERR_CUT_PARAM_SPD			707			///< カットパラメータエラー：スピード設定エラー
#define ERR_CUT_PARAM_SPD_LO		708			///< カットパラメータエラー：スピード設定オーバー
#define ERR_CUT_PARAM_SPD_HI		709			///< カットパラメータエラー：スピード設定ショート
#define ERR_CUT_PARAM_DIR			710			///< カットパラメータエラー：カット方向
#define ERR_CUT_PARAM_CUTCNT		711			///< カットパラメータエラー：カット本数
#define ERR_CUT_PARAM_CHRSIZE		712			///< カットパラメータエラー：文字サイズ指定エラー
#define ERR_CUT_PARAM_CUTANGLE		713			///< カットパラメータエラー：角度指定エラー
#define ERR_CUT_PARAM_CHARSET		714			///< カットパラメータエラー：文字列指定エラー
#define ERR_CUT_PARAM_STRLEN		715			///< カットパラメータエラー：マーキング文字列長指定エラー
#define ERR_CUT_PARAM_NOHAR			716			///< カットパラメータエラー：指定文字なし
#define ERR_CUT_PARAM_NORES			717			///< カットパラメータエラー：抵抗番号不正
#define ERR_CUT_PARAM_CUTMODE		718			///< カットパラメータエラー：カットモード不正（CUT_MODE_NORMAL(1)～CUT_MODE_NANAME(4)以外指定）
#define ERR_CUT_PARAM_PARCENT		719			///< カットパラメータエラー：ポイント設定
#define ERR_CUT_PARAM_BP			720
#define ERR_L1_LENGTH_LOW			721			///< Ｌ１カット長下限値未達エラー //V8.2.0.6③
#define ERR_L2_LENGTH_LOW			722			///< Ｌ２カット長下限値未達エラー //V8.2.0.6③
#define ERR_L3_LENGTH_LOW			723			///< Ｌ３カット長下限値未達エラー //V8.2.0.6③

#define ERR_CUT_RATIOPRM_BASERNO	730			///< レシオパラメータエラー：計算用ベース抵抗番号指定エラー
#define ERR_CUT_RATIOPRM_BASER_NG	731			///< レシオパラメータエラー：計算用ベース抵抗判定NG
#define ERR_CUT_RATIOPRM_MODENOT2	732			///< レシオパラメータエラー：計算用ベース抵抗がRatioモード2でない
#define ERR_CUT_RATIOPRM_CALCFORM	733			///< レシオパラメータエラー：計算用式が不正
#define ERR_CUT_RATIOPRM_CANTSET	734			///< レシオパラメータエラー：データの登録が出来ない
#define ERR_CUT_RATIOPRM_GRPNO		735			///< レシオパラメータエラー：グループ番号指定エラー

#define ERR_CUT_UCUTPRM_NOPRMRES	740			///< Uカットパラメータエラー：抵抗情報パラメータエリアの設定なし
#define ERR_CUT_UCUTPRM_NOPRMCUT	741			///< Uカットパラメータエラー：カット情報パラメータエリアの設定なし
#define ERR_CUT_UCUTPRM_NORES		742			///< Uカットパラメータエラー：対象抵抗なし
#define ERR_CUT_UCUTPRM_NODATA		743			///< Uカットパラメータエラー：パラメータデータが設定されていない
#define ERR_CUT_UCUTPRM_NOMODE		744			///< Uカットパラメータエラー：指定モードなし
#define ERR_CUT_UCUTPRM_TBLIDX		745			///< Uカットパラメータエラー：テーブルのインデックスが不正
#define ERR_CUT_UCUTPRM_L1L2LEN		746			///< Uカットパラメータエラー：L1及びL2の距離が0.1mm以下になった(内側リトレース有りの場合)  //V4.3.0.0①
#define ERR_CUT_UCUTPRM_L2		    747			///< Uカットパラメータエラー：L2の距離が(R1 *2)以下となった //V4.3.0.0①

#define ERR_CUT_CIRCUIT_NO			750			///< サーキットパラメータエラー：サーキット番号指定エラー

#define ERR_TRIMRESULT_TESTNO		760			///< トリミング結果：取得対象結果番号指定エラー（テスト実施数より大きい）
#define ERR_TRIMRESULT_RESNO		761			///< トリミング結果：取得対象抵抗開始番号指定エラー（登録抵抗数より大きい開始番号）
#define ERR_TRIMRESULT_TOTALRESNO	762			///< トリミング結果：取得対象抵抗番号指定エラー（登録抵抗数より大きい）
#define ERR_TRIMRESULT_CUTNO		763			///< トリミング結果：取得対象カット開始番号指定エラー（最大カット数より大きい開始番号）
#define ERR_TRIMRESULT_TOTALCUTNO	764			///< トリミング結果：取得対象カット番号指定エラー（最大カット数より大きい開始番号）
#define ERR_TRIMRESULT_ESRESCUTNO	765			///< トリミング結果：取得対象抵抗番号orカット番号指定エラー（最大カット数より大きい開始番号）

#define ERR_TRIM_COTACTCHK			770			///< コンタクトチェックエラー
#define ERR_TRIM_OPNSRTCHK			771			///< オープンショートチェックエラー
#define ERR_TRIM_CIR_NORES			772			///< サーキットトリミング-次対象抵抗なし

#define ERR_MARKVFONT_ANGLE			780			///< フォント設定：角度指定エラー
#define ERR_MARKVFONT_MAXPOINT		781			///< フォント設定：ポイント数オーバー

#define ERR_TRIMMODE_UNMATCH		790			///< トリミングモード不一致：レシオトリミング対象があるのにディレイモードが設定されている

// レーザ系エラーコード:800番台
#define ERR_LSR_PARAM_ONOFF			801			///< レーザON/OFFパラメータエラー
#define ERR_LSR_PARAM_QSW			802			///< Qレートパラメータエラー
#define ERR_LSR_PARAM_QSW_LO		803			///< Qレートパラメータエラー：最小Qレート以下
#define ERR_LSR_PARAM_QSW_HI		804			///< Qレートパラメータエラー：最小Qレート以上
#define ERR_LSR_PARAM_FLCUTCND		830			///< FL用加工条件番号設定エラー
#define ERR_LSR_PARAM_FLPRM			831			///< FL用パラメータエラー（制御種別指定エラー）
#define ERR_LSR_PARAM_FLTIMEOUT		832			///< FL用設定タイムアウト
#define ERR_LSR_STATUS_STANBY		833			///< FL:Stanby error,ES:POWER ON READY error
#define ERR_LSR_FL_BIASON_WAITOUT	834			///< FL用BIAS ON後のウェイトタイムアウト
#define ERR_LSR_FL_BIASON_ERROR		835			///< FL用BIAS ONエラー
#define ERR_LSR_STATUS_OSCERR		850			///< FL:Error occured,:ES:LD Alarm
#define ERR_LSR_OFF_SW_ON			851			///< SL436S用:「LASER OFF SW」押下 V.2.0.0.0_24

// ステージ系エラーコード:
// 
#define ERR_STG_STATUS				901			///< ステージ系でステータスエラー発生
#define ERR_STG_NOT_EXIST			902			///< 対象ステージ(X/Y/Z/Z2/Theta)存在せず。
#define ERR_STG_ORG_NONCOMPLETION	903			///< 原点復帰未完了
//#define	ERR_SOFTLIMIT_X				105			///< X軸ソフトリミット @@@440
//#define	ERR_SOFTLIMIT_Y				106			///< Y軸ソフトリミット @@@440
//#define	ERR_SOFTLIMIT_Z				107			///< Z軸ソフトリミット @@@440
#define ERR_STG_MOVESEQUENCE		905			///< ステージ動作シーケンス設定エラー
#define ERR_STG_THETANOTEXIST		906			///< θ載物台なし
#define ERR_STG_INIT_X				910			///< X軸原点復帰エラー
#define ERR_STG_INIT_Y				911			///< Y軸原点復帰エラー
#define ERR_STG_INIT_Z				912			///< Z軸原点復帰エラー
#define ERR_STG_INIT_T				913			///< Theta軸原点復帰エラー
#define ERR_STG_INIT_Z2				914			///< Z2軸原点復帰エラー
#define ERR_STG_MOVE_X				915			///< X軸ステージ動作異常：指令値 != 移動位置
#define ERR_STG_MOVE_Y				916			///< Y軸ステージ動作異常：指令値 != 移動位置
#define ERR_STG_MOVE_Z				917			///< Z軸ステージ動作異常：指令値 != 移動位置
#define ERR_STG_MOVE_T				918			///< Theta軸ステージ動作異常：指令値 != 移動位置
#define ERR_STG_MOVE_Z2				919			///< Z2軸ステージ動作異常：指令値 != 移動位置
#define ERR_STG_CLAMPX				920			///< クランプX閉チェックエラー V4.17.0.0①
#define ERR_STG_CLAMPY				921			///< クランプY閉チェックエラー V4.17.0.0①
#define ERR_STG_SOFTLMT_PLUS		930			///< プラスリミットオーバー
#define ERR_STG_SOFTLMT_MINUS		931			/**< マイナスリミットオーバー */
#define ERR_Z2_ORG_INTERLOCK		932			/// ステージ動作時にZ2原点センサーがONしていないため動作不可
#define ERR_STG_SERR		        933			/// メインステータスリード時「エラー割込み発生」//V.2.0.0.0⑰ 

///	タイムアウトエラーについては、ベース+対称軸ビット(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
///	のエラーコードで返す。
#define ERR_TIMEOUT_BASE			950			/**< タイムアウトエラー(ベース番号)
												 * - X軸タイムアウト=951
												 * - Y軸タイムアウト=952
												 * - Z軸タイムアウト=954
												 * - Theta軸タイムアウト=958
												 * - Z2軸タイムアウト=956
												 * - 複数軸はorの値
												 * - X,Y軸タイムアウト=953 */

#define ERR_AXIS_X_BASE				1000		///< X軸系エラーベース番号
#define ERR_AXIS_Y_BASE				2000		///< Y軸系エラーベース番号
#define ERR_AXIS_Z_BASE				3000		///< Z軸系エラーベース番号
#define ERR_AXIS_T_BASE				4000		///< Theta軸系エラーベース番号
#define ERR_AXIS_Z2_BASE			5000		///< Z2軸系エラーベース番号

// IO系エラーコード:
#define ERR_IO_PCINOTFOUND			10000		///< PCIボードが検出できず
#define ERR_IO_NOTGET_RTCCMOSDATA	10001		///< RTCﾃﾞｰﾀ読み込み失敗

//GPIB系エラーコード
#define ERR_GPIB_PARAM				11001		///< GPIBパラメータエラー
#define ERR_GPIB_TCPSOCKET			11002		///< GPIB-TCP/IP送信エラー
#define ERR_GPIB_EXEC				11003		///< GPIBコマンド実行エラー


#endif //#ifndef	__TRIM_ERR_NO_H__
