/***************************************************************************//**
 * @file        TrimFuncCmndNo.h
 * @brief       コマンド番号定義ファイル
 *
 * @par     (C) Copyright OMRONLASERFRONT INC. 2010-2012 All Rights Reserved
 ******************************************************************************/
#ifndef	__TRIM_FUNC_CMND_H__
#define	__TRIM_FUNC_CMND_H__

//#include "Defines.h"

//========================================================================
//-----( コマンド )-----
//========================================================================
//-------------------------------------------------------
// 2010/08/19 新規でコマンド番号振りなおし
//-------------------------------------------------------
#define CMD_SEGMENT_BASE			1000	/**< トレースログ出力のためのベース番号
											 * - ログ出力時、CMD番号/ベース番号を実施し、トレース対象のログを出力する。 */
// System 
//#define	CMD_AXIS_Z_ORG				1001	///< Z原点確認
#define	CMD_BPOFF					1002	///< *ビームポジショナのワーク座標の原点の補正
#define	CMD_BSIZE					1003	///< *ブロックサイズの設定
//#define	CMD_DIMENS					1004	///< *第n象現設定
#define	CMD_DREAD					1005	///< ディジタルスィッチの読み込み（10進で返す）
#define	CMD_DREAD2					1006	///< ディジタルスィッチの読み込み（1位、10位分けて返す）
#define	CMD_EMGRESET				1007	///< *EMG解除
//#define	CMD_FPRESET					1008	///< ファーストパルスサプレッサーを無効にする(オプション)
//#define	CMD_FPSET					1009	///< ファーストパルスサプレッサを有効にする(オプション)
//#define	CMD_FPSET2					1010	///< ファーストパルスサプレッサを有効にする(オプション)
#define	CMD_GETERRSTS				1011	///< XYテーブルまたはBPのリミット検出
#define	CMD_ILUM					1012	///< トリマのレーザ出射口下（基板面）のランフのオン・オフ
//#define	CMD_INTIME_VERSION			1013	///< INtime_Version
#define	CMD_ITIMESET				1014	///< IRQ0割込禁止/解除(Trimfunc.ocx用)
#define	CMD_NO_OPERATION			1015	///< 何もしないコマンド(cmd_GetStatusを読み出す。）
#define	CMD_READRTCCMOS				1016	///< RTC CMOS読み込み
#define	CMD_SETAXISLIMITS			1018	///< 各軸ﾘﾐｯﾄ設定
#define	CMD_SETDLY					1019	///< ﾃﾞｨﾚｲﾀｲﾑ設定
#define	CMD_SETSIGNALTOWER			1020	///< ｼｸﾞﾅﾙﾀﾜｰ制御
#define	CMD_SETTIMMER_MODE			1021	///< ﾄﾘﾏｰ動作ﾓｰﾄﾞ
#define	CMD_SLIDE_COVER_CHK			1022	///< スライドカバーチェックの切替え
#define	CMD_RESET					1017	///< XYテーブルまたはBPのリミットフラグを初期化する
#define	CMD_SYSINIT					1023	///< *全イニシャライズ
#define	CMD_TERMINATE				1027	///< 終了(戻らない)
#define	CMD_X_INIT					1028	///< *Ｘ軸イニシャライズ
#define	CMD_Y_INIT					1029	///< *Ｙ軸イニシャライズ
#define	CMD_Z_INIT					1030	///< *Ｚ軸イニシャライズ
#define	CMD_Z_INPSTS				1031	///< トリマのコンソール入力からステータスを読む
#define	CMD_ZABSVACCUME				1032	///< バキュームの制御
#define	CMD_ZCONRST					1033	///< コンソールスイッチのラッチ解除
#define	CMD_ZLATCHOFF				1034	///< Zスイッチのラッチ解除
#define	CMD_ZSETPOS2				1035	///< 第２原点用オフセット
#define	CMD_ZSLCOVERCLOSE			1036	///< スライドカバークローズバルブのON/OFF
#define	CMD_ZSLCOVEROPEN			1037	///< スライドカバーオープンバルブのON/OFF
#define	CMD_ZSYSPARAM1				1038	///< システムパラメータ１設定
#define	CMD_ZSYSPARAM2				1039	///< システムパラメータ２設定
#define	CMD_ZSYSPARAM3				1040	///< システムパラメータ３設定
//#define	CMD_SYSPARAM3				1024	///< システムパラメータ３設定 ﾊﾟﾙｽ幅設定QRate
//#define	CMD_SYSPARAM4				1025	///< システムパラメータ４設定 ﾊﾟﾙｽ幅設定ﾊﾟﾙｽ幅
//#define	CMD_SYSPARAM5				1026	///< システムパラメータ５設定 ﾊﾟﾙｽ幅設定有効ﾃﾞｰﾀ数
#define	CMD_ZWAIT					1041	///< 指定時間ウエイトする
#define	CMD_ZZGETRTMODULEINFO		1042	///< RTモジュールのバージョン番号を取得する
#define	CMD_ZSYSPARAM4				1043	///< システムパラメータ４設定 // V808①(DllTrimFunc)
#define CMD_REMEASURE_PAUSE			1044	// 再測定前のポーズタイム設定	////V4.9.0.0②
#define	CMD_MIDIUM_CUT_PARAM		1045	/// 途中切り検出用のパラメータを設定 
#define	CMD_CUT_POINT_PARAM			1046	/// ターンポイント、切替ポイントのパラメータを設定 V4.1.0.2①
#define CMD_BSIZE2					1047	// ブロックサイズの設定コマンド：現在のサイズによらずに設定を行う。//V4.6.0.11①
#define CMD_VARIABLE_POINT_PARAM	1048	// 可変ターンポイント対応		//V5.2.0.0①
#define CMD_VARIABLE_POINT_PARAM2	1049	// 可変ターンポイント対応		//V5.2.0.0①

//-----------------------
//新規追加
#define	CMD_GET_STATUS				1050	///< 装置状態（各軸、BPの状態）を取得する
#define CMD_SYSTEMRESET				1051	///<　システムリセットを実行する
#define CMD_SERVO_POWER				1052	///< サーボのON/OFF制御
#define CMD_CLEAR_SERVO_ALARM		1053	///< サーボアラームのクリア
#define CMD_INTERLOCK_CHECK			1054	///< インターロック状態の取得
#define CMD_CONSOLE_SWWAIT			1055	///<　コンソール(START/RESET/HALT)ボタンの押下状態待ち(無限ループ監視）
#define CMD_CONSOLE_LAMP_CTRL		1056	///< コンソール上のランプON/OFF制御 
#define CMD_COVER_LATCHCLR			1057	///< カバー開ラッチクリア
#define CMD_COVER_LATCHCHK			1058	///< カバー開ラッチチェック
#define CMD_COVER_CHK				1059	///< 固定カバー状態チェック
#define CMD_SLIDECOVER_MOVECHK		1060	///< スライドカバーのオープン/クローズの動作完了監視
#define CMD_SLIDECOVER_OPENCLOSECHK	1061	///< スライドカバーのオープン/クローズチェック
#define CMD_SLIDECOVER_GETSTS		1062	///< スライドカバーの状態取得
#define CMD_EMGRESET_SENSE			1063	///< 非常停止異常状態のリセット監視
#define CMD_EMGSTS_CHK				1064	///< 非常停止状態(ボタン)のチェック
#define CMD_HALT_SWCHK				1065	///< HALTボタンのチェック
#define CMD_SIGTWR_CTRL				1066	///< SignalTowerの制御
#define CMD_START_SWCHK				1067	///< スタートボタンのチェック
#define CMD_STARTRESET_SWCHK		1068	///< スタート/リセットボタンのチェック
#define CMD_CONSOLE_SWCHK			1069	///<　コンソール(START/RESET/HALT)ボタンの状態の取得
#define CMD_Z_SWCHK					1070	///< Zボタンのチェック
#define CMD_CLAMP_CTRL				1071	///< クランプのON/OFF制御
#define CMD_CLAMP_GETSTS			1072	///< クランプ状態の取得
#define	CMD_COVER_CHK_ONOFF			1073	///< 固定カバーチェックの切替え @@@064
#define	CMD_GET_COVER_CHK_STS		1074	///< 固定カバー/スライドカバーのチェック有無の取得 @@@065
#define CMD_SERVO_POWERZ2			1075	///< サーボのON/OFF制御(Z2用)		@@@131
#define CMD_CLEAR_SERVO_ALARMZ2		1076	///< サーボアラームのクリア(Z2用)	@@@131
//-----------------------
#define CMD_VARIABLE_INDEXPITCH_PARAM	1077	// 可変インデックスピッチ：初期値設定
#define CMD_VARIABLE_INDEXPITCH_PARAM2	1078	// 可変インデックスピッチ：インデックスピッチ設定
#define CMD_VARIABLE_TURNPOINT_PARAM 1079		// 可変ターンポイント対応		///V5.6.0.0①

#define CMD_SETLOG_ALLTARGET		1101	///< 全ログ出力対象セグメントの設定コマンド
#define CMD_GETLOG_ALLTARGET		1102	///< 全ログ出力対象セグメントの取得コマンド
#define CMD_SETLOG_TARGET			1103	///< 特定ログ出力対象セグメントの設定コマンド
//#define CMD_GETLOG_TARGET			1103	///< ログ出力対象セグメントの取得コマンド

// IO ctrl
#define	CMD_ALDFLGRST				2001	///< オートローダーフラグリセット
#define	CMD_EXTIN					2002	///< ユーザ用、標準EXTBIT I/O 
#define	CMD_EXTOUT					2003	///< ユーザー用、標準EXTBIT I/O ポートへ出力
#define	CMD_EXTOUT1					2004	///< ユーザ用、標準EXTBIT I/O ポートから入力
#define	CMD_EXTOUT2					2005	///< 拡張EXT BITのビット I/O LOWER ポートへ出力(ビット保存)
#define	CMD_EXTOUT3					2006	///< 拡張EXT BITのビット I/O HIGHER ポートへ出力(ビット保存)
#define	CMD_EXTOUT4					2007	///< 拡張 EXTOUT4
#define	CMD_EXTOUT5					2008	///< 拡張 EXTOUT5
#define	CMD_EXTRSTSET				2009	///< ユーザ用、標準EXTBIT I/O ポートへ出力既定値の指定
#define	CMD_HOST_CMD_IN				2010	///< ホスト通信用CMD IN
#define	CMD_HOST_STS_OUT			2011	///< ホスト通信用OUT
#define	CMD_INP16					2012	///< *指定のアドレスから１６ビットのデータを無符号数として入力
#define	CMD_OUT_WORD				2013	///< *I/O出力(Word)
#define	CMD_OUT16					2014	///< *指定のアドレスへ１６ビットのデータを出力
#define	CMD_OUTBIT					2015	///< *I/O出力(bit set)
#define	CMD_PIN16					2016	///< *指定のアドレスから１６ビツトのデータを入力
#define	CMD_ZATLDGET				2017	///< *ローダからデータを入力する
#define	CMD_ZATLDSET				2018	///< ローダへデータを出力する
#define	CMD_ZATLDRED				2019	///< 現在のローダ出力データ返す @@@059
#define	CMD_SETEXTPOWER				2020	///< 外部電源出力用に関数追加	>//V8.2.0.3①

// Laser
#define	CMD_FLSET					3001	///< 加工条件番号設定(FL用)
#define	CMD_GETLASERCURRENT			3002	///< ﾚｰｻﾞｰ電流制御電流値取得
#define	CMD_GETPOWERCTRL			3003	///< ﾊﾟﾜｰｺﾝﾄﾛｰﾙ読み込み
#define	CMD_LASER_OFF				3004	///< Laser　OFF
#define	CMD_LASER_ON				3005	///< Laser　ON
#define	CMD_LATTSET					3006	///< ロータリーアッテネータ（レーザー減衰率設定）
#define	CMD_POWERMEASURE			3007	///< ﾊﾟﾜｰ測定
#define	CMD_PROCPOWER				3008	///< 加工条件電力の設定
#define	CMD_QRATE					3009	///< *レーザ発振器のＱスィッチレートを設定
#define	CMD_ROT_ATT2				3010	///< ロータリーアッテネータ（レーザー減衰率設定）2
#define	CMD_SETLASERCURRENT			3011	///< ﾚｰｻﾞｰ電流制御電流値設定
#define	CMD_SETLASERCURRENTCONTROL	3012	///< ﾚｰｻﾞｰ電流制御ｺﾝﾄﾛｰﾙ設定
#define	CMD_SETQRATE2				3013	///< Qレート設定(ﾊﾟﾙｽ幅用)
#define	CMD_ZSETBPTIME				3014	///< レーザーON/OFFのポーズタイム設定
// NewIF
#define CMD_GET_QRATE				3015	///< Qレートの取得
#define CMD_SET_FL_ERRLOG			3016	///< FLで発生したエラー情報をログへ出力
#define CMD_ATTRESET				3017	///< アッテネータリセット
#define CMD_ATTSTATUS1				3018	///< アッテネータ制御データ1取得：エラーならびに各種リミット状態
#define CMD_ATTSTATUS2				3019	///< アッテネータ制御データ2取得：アッテネータ座標位置、固定ATT状態
#define CMD_PMON_SHUTCTRL			3020	///< パワーメータシャッタ制御(0/1=OFF/ON)
#define CMD_PMON_MEASCTRL			3021	///< パワーメータ測定モード制御(0/1=通常測定/パワーモニタ測定)
#define CMD_ONESHOTLASER			3022	///< レーザ　１ショット機能
#define CMD_SETFIXATTINFO			3023	///< 固定ATT情報の設定(配列)　// V809② 
#define CMD_SETFIXATTONEINFO		3024	///< 固定ATT情報の設定(個別)　// V809② 
#define CMD_FPS_SET					3025	///< //V8.2.3.10①　//V4.19.0.0②
#define	CMD_EXECFLG_SEND			3026	///< 基板ディレイ時のトリミング実行有無の送信	// V8.0.1.17①　//V4.19.0.0③

// Bp Ctrl
#define	CMD_BP_CALIBRATION			4001	///< BPキャリブレーション設定
#define	CMD_BP_LINEARITY			4002	///< BPリニアリティ補正値
#define	CMD_BPMOVE					4003	///< *BP 移動
#define	CMD_CIRTST					4005	///< CIRCLE Test
#define	CMD_CUTPOSCOR				4006	///< カット位置補正値受け取り
#define	CMD_CUTPOSCOR_ALL			4015	///< カット位置補正値受け取り（一括受信）
#define	CMD_MOVE					4007	///< ビームポジショナの最高速移動
#define	CMD_TEACH1					4008	///< ビームポジショナのオフセット値をティーチングによって、セットする
#define	CMD_TEACH2					4009	///< 各トリミングスタート点をティーチングにより、入力
#define	CMD_TRIM_NGMARK				4010	///< NGサーキットのマーキング
#define	CMD_ZBPLOGICALCOORD			4011	///< BP論理座標の象現設定
#define	CMD_ZGETBPPOS				4012	///< BP現在座標読み込み
#define CMD_TCORPOSSET				4013	///< 座標軸回転（θ）補正用XYポジション設定（θ載物台無し）
#define CMD_TCORANGSET				4014	///< 座標軸回転（θ）補正用角度設定（θ載物台有り）
#define CMD_BP_GET_CALIBDATA		4016	///< BPキャリブレーションデータの取得
#define	CMD_ZGETBPPOS2				4017	///< BP現在座標読み込み2(cmd_moveで保存した指令値を返す)		// V809④
#define	CMD_GETCUTLENGTH			4018	///< カットした長さを取得			//V4.6.0.9③
// Meas
#define	CMD_DSCAN					5001	///< ＤＣスキャナのプローブ番号を指定する
#define	CMD_DIFFERENCE_CORRECT		5002	///< 差電流補正値設定
//#define	CMD_DMEAS					5003	///< 内部測定器で抵抗測定(差電流)
#define	CMD_FAST_WMEAS				5004	///< パワーモニタ測定
//#define	CMD_LJRATE					5005	///< *平均化トリミング有無と判定率の指定
#define	CMD_RANGE_SET				5006	///< 測定レンジの設定（新規追加）
//#define	CMD_MEASMODE				5006	///< 抵抗、電圧、スロープモード設定
#define	CMD_MFSET					5007	///< ＤＣスキャナに接続する測定器とモード切り替え
#define	CMD_MSCAN					5008	///< DSCANをガードプローブを拡張した関数
#define	CMD_RRANGE_CORRECT			5009	///< レンジ補正値設定

#define CMD_MEASURE					5010	///< 測定（抵抗/電圧-オート/固定-高速/高精度をパラメータで指定）
#define	CMD_RMEAS_AUTORANGE			5011	///< 内部測定器で抵抗測定
#define	CMD_RMEAS_FIXRANGE			5012	///< 内部測定器で抵抗測定（固定レンジを指定）
#define	CMD_VMEAS_AUTORANGE			5013	///< 内部測定器でＤＣ電圧測定
#define	CMD_VMEAS_FIXRANGE			5014	///< 内部測定器で電圧測定（固定レンジを指定）

#define	CMD_RMEASHL					5015	///< 内部測定器で抵抗測定
#define	CMD_TEST					5016	///< 指定した値の範囲を比較する
#define	CMD_VRANGE_CORRECT			5017	///< 電圧レンジ補正値設定
#define	CMD_ZGETDCVRANG				5018	///< 内部ＤＣ測定器の最大レンジの最大電圧値を返す。
#define	CMD_HILOSEPARATEDSET		5020	///< ＤＣスキャナのプローブ番号ＨＩ，ＬＯ（Force,Sense)分離設定 V8.0.0.14③
#define	CMD_DSCAN_SEPARATED			5021	///< ＤＣスキャナのプローブ番号を指定するＨＩ，ＬＯ（Force,Sense)分離 V8.0.0.14③
#define	CMD_MSCAN_SEPARATED			5022	///< DSCAN_SEPARATEDをガードプローブを拡張した関数 V8.0.0.14③
#define	CMD_SET_POWER_RESOLUTION	5023	///< パワーモニタ測定分解能設定//V5.0.0.1① 2016.12.27

//#define	CMD_BPTRIM					4004	///< *BP TRIM
//#define	CMD_RMEAS2					5011	///< 抵抗測定(function)
//#define	CMD_ZRMEAS2					5018	///< 抵抗測定オートレンジ
//#define	CMD_ZVMEAS2					5020	///< 内部測定器で電圧測定（スキャナ指定なし、オートレンジ）
//#define	CMD_ZZVMEAS2				5021	///< 内部測定器で電圧測定（固定レンジ）

// Cut&Trimming
//VE装置向け-新規追加コマンド
#define CMD_TrimST					6000	///< 斜め直線カット/トリミング（FL対応版）
#define CMD_TrimL					6010	///< 斜めLカット/トリミング（FL対応版）
#define CMD_TrimL6					6011	///< V8.0.0.17①６点ターンポイントＬカットトリミング（FL未対応版）//V4.6.0.9②
#define CMD_TrimHkU					6020	///< 斜めフックorUカット/トリミング（FL対応版）
//--- ユーザ公開検討中(2010/12/14)
#define CMD_TrimSC					6030	///< スキャンカット/トリミング（FL対応版）
#define CMD_TrimIX					6040	///< 斜めインデックスカット/トリミング（FL対応版）
//---　必要と判断した時点でDllTrimFuncにIFを追加する。
#define CMD_TrimES					6050	///< エッジセンスカット/トリミング（FL対応版）
#define CMD_TrimES0					6051	///< エッジセンスカット/トリミング（FL対応版）　//V4.12.0.0①

#define CMD_TrimMK					6060	///< 方向指定文字マーキング（FL対応版）
#define CMD_TrimU4					6061	///< 特注Uカットリミング（FL対応版）V4.2.0.0①

#define	CMD_UCUT_PARAMSET			6021	///< UCUT PARAMETER
#define	CMD_UCUT_RESULT				6022	///< UCUT retrieve result
#define	CMD_RATIO2EXP				6023	///< レシオモード２の計算式指定
#define	CMD_ZSETUCUT				6024	///< リトレースUカットパラメータ設定
#define	CMD_UCUT4RESULT				6027	///< リトレースUカットパラメータテーブルインデックス取得
#define	CMD_VUTRIM4					6028	///< Uカットリトレース電圧トリミング(斜めではない)

#define	CMD_TRIMBLOCK				6070	///< トリミング実行（ブロック単位)
#define	CMD_TRIMDATA				6071	///< トリミングデータ
#define	CMD_TRIM_RESULT				6072	///< トリミング結果取得
#define	CMD_TRIMEND					6073	///< トリミング終了(Memory clean-up)
#define	CMD_TRIMBLOCKMEASURE		6074	///< オートレンジ測定実行（ブロック単位)	// @@@015
#define	CMD_TRIMMEASURERESIST		6075	///< 抵抗番号してしての抵抗測定	

#define CMD_PARAM_LTURNPARAM		6076	///< 可変Lターンポイントのデータ設定	//V4.6.0.5①  
#define	CMD_LTURN_RESULT			6077	///< 可変Lターンポイント実行結果の取得	//V4.6.0.5①
#define	CMD_ZCOVEROPEN_MOVE			6078	///< カバー開でのステージ移動許可用フラグ設定コマンド	V4.6.0.5③
#define	CMD_HIVOLT_SET				6079	///< 高電圧用設定						// //V4.15.0.0①
#define	CMD_HIVOLT_INIT				6080	///< 高電圧用設定の初期化				// //V4.15.0.0①
#define	CMD_HIVOLT_EXTPOWER			6081	///< 高電圧用の外部電源をON/OFFする		// //V4.15.0.0①
#define	CMD_HIVOLT_CORRECTSET		6082	///< 高電圧用の補正係数設定用			// //V4.15.0.0①
#define	CMD_HIVOLT_SPECSET			6083	///< 高電圧仕様の設定					// //V4.15.0.0①
#define	CMD_HIVOLT_VXSELSET			6084	///< 高電圧用VxSELの設定				// //V4.15.0.0①
#define	CMD_HIVOLT_ADDRESIST		6085	///< 合成抵抗有無指定設定				// //V4.15.0.0①
#define	CMD_FPS_SETTING				6086	///< FPS設定コマンド					// //V4.15.0.0
#define	CMD_ATT_RESET_FLGSET		6087	///< アッテネータリセットフラグ設定		// //V4.15.0.0

// Mechanical(stage)
#define	CMD_PROBOFF					8001	///< Ｚプローブをオフ位置へ移動
#define	CMD_PROBOFF2				8002	///< *Z2プローブをオフ位置へ移動
#define	CMD_PROBON					8003	///< Ｚプローブをオン位置へ移動
#define	CMD_PROBON2					8004	///< *Z2プローブをオン位置へ移動
#define	CMD_PROBUP					8005	///< *Ｚプローブをオン位置から指定距離上げる
#define	CMD_PROBUP2					8006	///< *Z2プローブをオン位置から指定距離上げる
#define	CMD_RBACK					8007	///< -θ載物台を原点位置に戻す(オプション)
#define	CMD_RINIT					8008	///< -θ載物台の初期化(オプション)
#define	CMD_ROUND					8009	///< θ載物台の角度位置指定の回転（オプション）
#define	CMD_ROUND4					8010	///< θ載物台の絶対角度指定の回転（オプション）
#define	CMD_SBACK					8011	///< パーツハンドラをロード位置に戻す
#define	CMD_SMOVE					8012	///< パーツハンドラを相対距離で移動
#define	CMD_SMOVE2					8013	///< パーツハンドラを絶対座標で移動
#define	CMD_START					8014	///< パーツハンドラをロード位置からトリム位置に移動
#define	CMD_TSTEP					8015	///< *パーツハンドラを指定のブロック位置へ移動
/*///#define	CMD_XYOFF					8017	///< XY テーブルのステップアンドリピート座標の補正 */
#define	CMD_ZMOVE					8016	///< *Z動作
#define	CMD_ZMOVE2					8017	///< *Z2動作
#define	CMD_ZSTOPSTS				8018	///< *Z停止チェック
#define	CMD_ZSTOPSTS2				8019	///< *Z2停止チェック
//#define	CMD_ZOUTPUT					8021	///< *Z動作のみ
//#define	CMD_ZOUTPUT2				8022	///< *Z2動作のみ
#define	CMD_ZSELXYZSPD				8020	///< XYZのスピードの選択
#define	CMD_ZSTGXYMODE				8021	///< XYステージ動作指定(通常、下方プローブ対応のシーケンス)
#define	CMD_SMOVE3					8022	///< パーツハンドラを相対距離で移動指令値からの差分で移動			　//V8.2.0.1⑥

#define	CMD_GETZPOS					8030	///< Z位置取得
#define	CMD_GETZ2POS				8031	///< Z2位置取得
#define	CMD_ZGETPHPOS				8032	///< *パーツハンドラ(XY)の現在座標を得る
#define	CMD_ZGETPHPOS2				8033	///< *パーツハンドラ(XY)の現在座標を得る//@@@777
#define	CMD_GETZPOS2				8034	///< Z位置取得（第２原点基準座標系）//V8.2.0.1⑦
#define	CMD_SETLOADPOS				8035	///< ロードポジション（cmd_Sback()）の座標を設定 //V8.2.0.2②
#define	CMD_SETZOFFPOS				8036	///< Zの待機位置を設定する //V8.0.0.10②

#define CMD_GET_ALLAXIS_STATUS		8040	///<　全軸のステータス状態を取得する

//デバッグ用コマンド
#define CMD_MEAS_PERFORM			30001	///< パフォーマンス測定用コマンド
#define CMD_SET_AXISSPD				30002	///< XYZ軸速度設定コマンド
#define CMD_RESET_LSI				30003	///< LSIのリセットコマンド
#define CMD_SET_AXISPRM				30004	///<XYZ軸設定コマンド @@@075
#define CMD_SET_AXISSPDX			30005	///< X軸速度設定コマンド @@@129
#define CMD_SET_AXISSPDY			30006	///< Y軸速度設定コマンド @@@129
#define CMD_SET_AXISSPDZ			30007	///< X軸速度設定コマンド V8.0.0.14②
#define CMD_SET_AXISPRM2			30008	///< XYZ軸設定コマンド2 V.2.0.0.0_21

//V4.6.0.6①↓
#define	CMD_MES_LOW_RESIST			30010	// 超低抵抗測定：単独測定コマンド 
#define	CMD_PARAM_LOW_RESIST		30011	// 超低抵抗測定：メインパラメータの転送
#define	CMD_MES_LOW_RESULT			30012	// 超低抵抗測定
#define	CMD_CORR_LOW_SEND			30013	// 超低抵抗測定:校正値の転送 
//V4.6.0.6①↑

#endif //#ifndef  __TRIM_FUNC_CMND_H__
