/***************************************************************************//**
 * @file        DllTrimFunc.h
 * @brief       SUMMARY
 *
 * @par     (C) Copyright OMRONLASERFRONT INC. 2010-2012 All Rights Reserved
 ******************************************************************************/
#include <WINDOWS.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sys/types.h>
#include <limits.h>
//#include "Common.h"

// 以下の ifdef ブロックは DLL からのエクスポートを容易にするマクロを作成するための 
// 一般的な方法です。この DLL 内のすべてのファイルは、コマンド ラインで定義された DLLTRIMFUNC_EXPORTS
// シンボルでコンパイルされます。このシンボルは、この DLL を使うプロジェクトで定義することはできません。
// ソースファイルがこのファイルを含んでいる他のプロジェクトは、 
// DLLTRIMFUNC_API 関数を DLL からインポートされたと見なすのに対し、この DLL は、このマクロで定義された
// シンボルをエクスポートされたと見なします。
#ifdef DLLTRIMFUNC_EXPORTS
#define DLLTRIMFUNC_API __declspec(dllexport)
#else
#define DLLTRIMFUNC_API __declspec(dllimport)
#endif

//#define		cVERSIONcINFORMATION	"V7.01, Jan 22 2010"		// 
#define		cVERSIONcINFORMATION	(8.00)						// 

extern DLLTRIMFUNC_API int nDllTrimFunc;

DLLTRIMFUNC_API int fnDllTrimFunc(void);

//------------------------------------------------------------------------------
//		構造体定義
//		ユーザプログラムorTKY系とのI/Fに使用する構造体
//		.NET環境よりアライメントを8バイトに変更
//		TKY系のパラメータに関しては、公開の検討をする
//------------------------------------------------------------------------------
#define MAX_PARAM_CNT				(32)	///< パラメータ数
#define	MAX_CUT_CND					(8)		///< 最大加工条件設定数(加工条件1-4)
#define MAX_CND_CNT				    (32)	///< 加工条件数　// V809②

// TrimBlock DataType
#define	TRIMDATA_TYP_PLATE			(1)
#define	TRIMDATA_TYP_RES			(2)
#define	TRIMDATA_TYP_CUT			(3)
#define	TRIMDATA_TYP_CUTPRM			(4)
#define	TRIMDATA_TYP_GPIB			(8)
#define	TRIMDATA_TYP_GPIB2			(9)		// @@@013② 

// Console Lamp No
#define CSL_LAMPNO_START			(0)		///< スタートランプ指定番号
#define CSL_LAMPNO_RESET			(1)		///< リセットランプ指定番号
#define CSL_LAMPNO_Z				(2)		///< Zランプ指定番号
#define CSL_LAMPNO_HALT				(5)		///< HALTランプ指定番号
// Console Switch status No
#define CSL_SWON_START				(1)		///< スタートボタンON
#define CSL_SWON_RESET				(3)		///< リセットボタンON
#define CSL_SWON_HALT				(4)		///< HALTボタンON
#define CSL_SWON_Z					(5)		///< ZボタンON
// Switch status
#define SWITCH_ON					(1)		///< スイッチON状態
#define SWITCH_OFF					(0)		///< スイッチOFF状態

#define MAX_RESISTORS				(256)	///< 抵抗数の最大値
#define MAX_NUMCALLENGTH			(101)	///< レシオ計算式の文字列長 100文字まで
#define MAX_RATIORES				(999)	///< レシオ計算式の抵抗数の最大値
#define	DEF_MAX_REG_ITEM_PARAMCNT	(50)	///< 最大レジスタ内項目数
#define	DEF_MAX_REG_PARAMCNT		(100)	///< 最大レジスタ数
#define	DEF_MAX_PROB		        (128)	///< 最大プローブ数 V8.0.0.14②

// Slidecover get target
#define SLD_OPEN_CHECK		0				///< スライドカバー：オープンチェック
#define SLD_CLOSE_CHECK		1				///< スライドカバー：クローズチェック


#pragma pack(8)
//------------------------------------------------------------
// ユーザプログラム向けカットパラメータ情報
//------------------------------------------------------------
/// ------ 各カットのパラメータ(長さ、スピード、Qレート)保存領域
typedef struct _tag_DBL_CUTCOND_ARRAY{
    double dblL1;					///< Line1用のパラメータ
    double dblL2;					///< Line2用のパラメータ
    double dblL3;					///< Line3用のパラメータ
    double dblL4;					///< Line4用のパラメータ
}DBL_CUTCOND_ARRAY;

typedef struct _tag_SRT_CUTCOND_ARRAY{
    short srtL1;					///< Line1用のパラメータ
    short srtL2;					///< Line2用のパラメータ
    short srtL3;					///< Line3用のパラメータ
    short srtL4;					///< Line4用のパラメータ
}SRT_CUTCOND_ARRAY;
/////###888 
/// ------ 加工設定構造体
typedef struct _tag_CUT_COND_STRUCT{
    DBL_CUTCOND_ARRAY CutLen;		///< カット長情報
    DBL_CUTCOND_ARRAY SpdOwd;		///< カットスピード（往路）
    DBL_CUTCOND_ARRAY SpdRet;		///< カットスピード（復路）
    DBL_CUTCOND_ARRAY QRateOwd;		///< カットQレート（往路）
    DBL_CUTCOND_ARRAY QRateRet;		///< カットQレート（復路）
    SRT_CUTCOND_ARRAY CondOwd;		///< カット条件番号（往路）
    SRT_CUTCOND_ARRAY CondRet;		///< カット条件番号（復路）
}CUT_COND_STRUCT;
/// ------ カット情報構造体
typedef struct _tag_CUT_INFO_STRUCT{
    short srtMoveMode;				///< 動作モード（0:トリミング、1:ティーチング、2:強制カット）
    short srtCutMode;				///< カットモード(0:ノーマル、1:リターン、2:リトレース、3:斜め）
    double dblTarget;				///< 目標値
    short srtSlope;					///< スロープ設定(1:電圧測定＋スロープ、2:電圧測定－スロープ、4:抵抗測定＋スロープ、5:抵抗測定－スロープ）
    short srtMeasType;				///< 測定タイプ(0:高速(3回)、1:高精度(2000回)、2:（IDXのみ）外部機器、3:測定無し、5～:指定回数測定）
    double dblAngle;				///< カット角度
    double dblLTP;					///< Lターンポイント
    short srtLTDIR;					///< Lターン後の方向
    double dblRADI1;					///< R部回転半径（Uカットで使用）
	// for Hook Or UCut
    double dblRADI2;				///< R2部回転半径（Uカットで使用）
	short srtHkOrUType;				///< HookCut(3)かUカット（3以外）の指定。
	// for Index
	short srtIdxScnCnt;				///< インデックス/スキャンカット数(1～32767)
	short srtIdxMeasMode;			///< インデックス測定モード（0:抵抗、1:電圧、2:外部）
	// for EdgeSense
	double dblEsPoint;				///< エッジセンスポイント
	double dblRdrJdgVal;			///< ラダー内部判定変化量
	double dblMinJdgVal;			///< ラダーカット後最低許容変化量
	short srtEsAftCutCnt;			///< ラダー切抜け後のカット回数（測定回数）
	short srtMinOvrNgCnt;			///< ラダー抜出し後、最低変化量の連続Over許容数
	short srtMinOvrNgMode;			///< 連続Over時のNG処理（0:NG判定未実施, 1:NG判定実施。ラダー中切り, 2:NG判定未実施。ラダー切上げ）
	// for Scan
	double dblStepPitch;			///< ステップ移動ピッチ
	short  srtStepDir;				///< ステップ方向
}CUT_INFO_STRUCT;
//---------------------------------------
/// ユーザプログラム向けカットパラメータ
//---------------------------------------
typedef struct _tag_CUT_COMMON_PRM{
    CUT_INFO_STRUCT CutInfo;
    CUT_COND_STRUCT CutCond;
}CUT_COMMON_PRM, *PCUT_COMMON_PRM;

//------------------------------------------------------------
// TKY系向けカットパラメータ情報
//------------------------------------------------------------
/// カットパラメータ
typedef	union tag_PRM_CUTPARAM {
	struct tag_CUT_ST {
		WORD	mode;				///< 動作モード  0:NOM, 1:リターン, 2:リトレース, 4:斜め
		WORD	angle;				///< 斜めカット角度(0～359deg)
		double	length;				///< 最大カッティング長  0.0000～20.0000 (mm)
		double	RtOfs;				///< RTオフセット(mm) //V8.0.0.11①
	} cut_st;

	struct tag_CUT_L {
		WORD	mode;				///< 動作モード  0:NOM, 1:リターン, 2:リトレース, 4:斜め
		WORD	angle;				///< 斜め：カット角度(0～359deg)、ノーマル：方向指定(1:→, 2:↑, 3:←, 4:↓)
		WORD	tdir;				///< Lターン方向  1:時計回り(CW)、2:反時計回り(CCW)
		double	turn;				///< Lターンポイント 0.0～100.0 (%)
		double	L1;					///< L1 最大カッティング長  0.0000～20.0000 (mm)
		double	L2;					///< L2 最大カッティング長  0.0000～20.0000 (mm)
		double	r;					///< ターンの円弧半径 (mm)
		double	RtOfs;				///< RTオフセット(mm) //V8.0.0.11①
	} cut_l;

	struct tag_CUT_HOOK {
		WORD	mode;				///< 動作モード  0:NOM, 1:リターン, 2:リトレース, 4:斜め
		WORD	angle;				///< 斜め：カット角度(0～359deg)、ノーマル：方向指定(1:→, 2:↑, 3:←, 4:↓)
		WORD	tdir;				///< Lターン方向  1:時計回り(CW)、2:反時計回り(CCW)
		double	turn;				///< Lターンポイント 0.0～100.0 (%)
		double	L1;					///< L1 最大カッティング長  0.0000～20.0000 (mm)
		double	r1;					///< ターン1の円弧半径 (mm)
		double	L2;					///< L2 最大カッティング長  0.0000～20.0000 (mm)
		double	r2;					///< ターン2の円弧半径 (mm)
		double	L3;					///< L3 最大カッティング長  0.0000～20.0000 (mm)
        double  MinL2;              ///< L2 最小カッティング長(0.00000～20.0000(mm)) V8.0.0.14②
	} cut_hook;

	struct tag_CUT_INDEX {
		WORD	angle;				///< 斜め：カット角度(0～359deg)、ノーマル：方向指定(1:→, 2:↑, 3:←, 4:↓)
		WORD	maxindex;			///< インデックス数  1～32767
		WORD	measMode;			///< 測定モード 0:抵抗, 1:電圧, 3:外部
		WORD	measType;			///< 測定タイプ 0:高速, 1:高精度, 3:
		double	length;				///< インデックス長  0.0000～20.0000 (mm)
		double	LimitLen;			///< IXカットのリミット長 (mm) // V8.0.0.13①
		double	CutLen;				///< カット済みのIXカット長(mm) V8.2.0.5①(INtrimで使用)
	} cut_index;

	struct tag_CUT_SCAN {
		WORD	angle;				///< 斜め：カット角度(0～359deg)、ノーマル：方向指定(1:→, 2:↑, 3:←, 4:↓)
		WORD	sdir;				///< ステップ方向  1:+X, 2:-X, 3:+Y, 4:-Y
		WORD	lines;				///< 本数(1～n)
		double	length;				///< カッティング長  0.0000～20.0000 (mm)
		double	pitch;				///< ピッチ  　　　  0.0000～20.0000 (mm)
	} cut_scan;

	struct tag_CUT_MARKING {
		//BYTE	str[16];			///< 描画文字列
		BYTE	str[18];			///< 描画文字列
		double	mag;				///< 倍率 0.01～99.99倍
		WORD	dir;				///< 文字の向き  0:0, 1:90, 2:180, 3:270
	} cut_marking;

	//struct tag_CUT_C {
	//	WORD	dir; 				///< カット方向 1:CW 2:CCW
	//	WORD	angle;				///< 開始角度
	//	WORD	count;				///< 回数
	//	double	st_r;				///< 開始半径
	//	double	pitch;				///< ピッチ
	//} cut_c;

	/// ESカット
	struct tag_CUT_EG {
		WORD	angle;				///< 斜め：カット角度(0～359deg)、ノーマル：方向指定(1:→, 2:↑, 3:←, 4:↓)
		double	L1;					///< カット長  0.0000～20.0000 (mm)
		double	ESpoint;			///< ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ 0.0～-999.0 (%)
		double	ESwide;				///< ｴｯｼﾞｾﾝｽ変化率 0.0～100.0 (%)
		double	ESwide2;			///< ｴｯｼﾞｾﾝｽ後変化率 0.0～100.0 (%)
		WORD	EScount;			///< ｴｯｼﾞｾﾝｽ後確認回数  0～20
		WORD	CTcount;			///< ｴｯｼﾞｾﾝｽ後連続NG確認回数　
		WORD	wJudgeNg;			///< NG判定する/しない（0:TRUE/1:FALSE）
	} cut_eg;
	//// ESカット
	//struct tag_CUT_EG {
	//	WORD	dir;				///< カット方向  1:+X, 2:-Y, 3:-X, 4:+Y
	//	double	L1;					///< カット長  0.0000～20.0000 (mm)
	//	double	ESpoint;			///< ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ 0.0～-999.0 (%)
	//	double	ESwide;				///< ｴｯｼﾞｾﾝｽ変化率 0.0～100.0 (%)
	//	double	ESlen2;				///< ｴｯｼﾞｾﾝｽ後カット長  0.0000～20.0000 (mm)
	//} cut_eg;

	/// 初期ESカット
	struct tag_CUT_EG_ORG {
		WORD	angle;				///< 斜め：カット角度(0～359deg)、ノーマル：方向指定(1:→, 2:↑, 3:←, 4:↓)
		double	L1;					///< カット長  0.0000～20.0000 (mm)
		double	ESpoint;			///< ｴｯｼﾞｾﾝｽﾎﾟｲﾝﾄ 0.0～-999.0 (%)
		double	ESwide;				///< ｴｯｼﾞｾﾝｽ変化率 0.0～100.0 (%)
		double	ESlen2;				///< ｴｯｼﾞｾﾝｽ後ｶｯﾄ長 0.0000～20.0000 (mm)
	} cut_eg_org;

} PRM_CUT_PARAM, *PPRM_CUT_PARAM;

/// 加工条件設定テーブル：L1～L8の加工条件を設定したテーブル（1～4：通常カット時、5～8：リターンリトレース時）
typedef struct tag_PRM_CUT_CONDITION {
	WORD	cutSetNo;					///< （FL向け）加工条件番号
	double	cutSpd;						///< カットスピード
	double	cutQRate;					///< カットQレート
} PRM_CUT_COND, *PPRM_CUT_COND;

/// カットデータ
typedef	struct tag_PRM_CUT_DATA {
	WORD	wCutNo;						///< カット番号 1-9
	WORD	wDelayTime;					///< Delay time 0-32767 (msec) 定電流印加後測定遅延時間
	WORD	wCutType;					///< カット形状 1:st, 2:L, 3:HK, 4:IX
	WORD	wMoveMode;					///< 0:トリミングカット, 2:強制カット(x5)
	WORD	wDoPosition;				///< ポジショニングあり(0)/なし(1)
	WORD	wCutAftPause;				///< カット後ポーズタイム（ｍｓ）(ローム殿対応) V8.0.0.16②
	double	fCutStartX;					///< カットスタート座標 X  -80.0000～+80.0000
	double	fCutStartY;					///< カットスタート座標 Y  -80.0000～+80.0000
//	double	fCutSpeed;					///< カットスピード 0.1～409.0 (mm/s)
//	double	fQrate;						///< レーザーQスイッチレート 0.1～50.0 (KHz)
	double	fCutOff;					///< カットオフ %  -99.999 ～ +999.999
	double	fAveDataRate;				///< 切替ポイント (旧平均化率 % 0.0～100.0%) @@@089
	BYTE	bUcutNo;					///< Uカット パラメータ指定時、選択されたテーブル番号を保存
	double	fInitialVal;				///< Uカット カットごとの初期値実測を保存
	PRM_CUT_COND stCutCnd[MAX_CUT_CND];	///< 加工条件（条件番号、カットスピード、Qrate）1～8
	PRM_CUT_PARAM CutParam;				///< GUI側からはCUT_PARAM設定で設定を行う。
} PRM_CUT_DATA, *PPRM_CUT_DATA;

/// 抵抗データ
typedef	struct	tag_PRM_RESISTOR {
	WORD	wResNo;						///< PR1  抵抗番号 1-999(トリミング), 1000-9999(マーキング)
	WORD	wMeasMode;					///< 測定モード　0：抵抗、1：電圧、２：外部 → 2010/12/06 抵抗データに移動する。
	WORD	wMeasType;					///< PR2  判定測定 0:高速、1:高精度
	WORD	wCircuit;					///< PR3  サーキット 抵抗が属するサーキット番号
	WORD	wHiProbNo;					///< PR4  H  High probe scanner channel
	WORD	wLoProbNo;					///<      L  Low
	WORD	w1stAG;						///<      G1 アクティブガード番号
	WORD	w2ndAG;						///<      G2 
	WORD	w3rdAG;						///<      G3
	WORD	w4thAG;						///<      G4
	WORD	w5thAG;						///<      G5
	DWORD	dwExBits;					///< External bits
	WORD	wPauseTime;					///< ポーズタイム(dwExBits出力後のウェイト) (msec)
	WORD	wRatioMode;					///< トリムモード  0:絶対値, 1:レシオ
	WORD	wBaseReg;					///< ベース抵抗No. レシオ時の基準抵抗番号
	double	fTargetVal;					///< トリミング目標値 (ohm)
	WORD	wSlope;						///< 電圧変化スロープ(PP2=1の場合) 0:+スロープ, 1:-スロープ
	double	fITLimitH;					///< IT Limit H  (%) -99.99～9999.99
	double	fITLimitL;					///< IT Limit L
	double	fFTLimitH;					///< FT Limit H  (%) -99.99～9999.99
	double	fFTLimitL;					///< FT Limit L
	WORD	wCutCnt;					///< カット数 1～30 // @@@460 @@@350
	WORD	wCorrectFlg;				///< カット位置補正実行フラグ(0:補正しない, 1:する)
	double	fITOKLimitH;				///< (SL436K IT OK HIGT LIMMIT)
	double	fITOKLimitL;				///< (SL436K IT OK LOW LIMMIT)
	double	fCutOutRatio;				///< 切上倍率
	//----- V808②↓-----
	WORD intCvMeasNum;					///< CV 最大測定回数
	WORD intCvMeasTime;					///< CV 最大測定時間(ms) 
	double dblCvValue;					///< CV CV値         
	WORD intOverloadNum;				///< ｵｰﾊﾞｰﾛｰﾄﾞ 回数
	double dblOverloadMin;				///< ｵｰﾊﾞｰﾛｰﾄﾞ 下限値
	double dblOverloadMax;				///< ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
	//----- V808②↑-----
	//----- V8.0.0.16②↓ -----
	WORD wPauseTimeFT;					///< FT前のポーズタイム(0-32767msec) ローム殿対応
	WORD wInsideEndChkCount;			///< 中切り判定回数(0-255)
	double dblInsideEndChgRate;			///< 中切り判定変化率(0.00-100.00%)
	//----- V8.0.0.16②↑ -----
    //----- V8.0.0.14②↓(HILO分離) -----
	WORD wProbHiSenseNo;				///< ハイ側プローブ番号(センス)
	WORD wProbLoSenseNo;				///< ロー側プローブ番号(センス)
#ifdef COMBINATION_SCANNER
	WORD CombHiAry[DEF_MAX_PROB];		///< プローブ番号組合せ(ハイ側) ※終了は0 V8.2.0.6① ADD
	WORD CombLoAry[DEF_MAX_PROB];		///< プローブ番号組合せ(ロー側)    　　   V8.2.0.6① ADD
#endif
    //----- V8.0.0.14②↑-----

	BOOL	bTrimEnd;					///< トリミング完了フラグ
	int		lLastCutNo;					///< 最終カット番号
	PPRM_CUT_DATA	pCutData;			///< カットデータポインタ 
} PRM_RESISTOR, *PPRM_RESISTOR;

/// プレートデータ
typedef	struct tag_TRIM_PLATE_DATA {
	WORD	wCircuitCnt;
	WORD	wResistCnt;
	WORD	wResCntInCrt;			///< サーキット内抵抗数
	WORD	wDelayTrim;				///< delay trim(0:なし,1:ﾃﾞｨﾚｲﾄﾘﾑ実施（通常),2:ﾃﾞｨﾚｲﾄﾘﾑ2(プレート単位のディレイ)
	double	fBPOffsetX;
	double	fBPOffsetY;
	double	fAdjustOffsetX;
	double	fAdjustOffsetY;
	double	fNgCriterion;
	double	fZStepPos;				///< Z軸ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置
	double	fZTrimPos;				///< Z軸ｺﾝﾀｸﾄ位置
	double	fReProbingX;			///< 再ﾌﾟﾛｰﾋﾞﾝｸﾞX移動量
	double	fReProbingY;			///< 再ﾌﾟﾛｰﾋﾞﾝｸﾞY移動量
	WORD	wReProbingCnt;			///< 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数
	WORD	wInitialOK;				///< IT時FT範囲時の処理(0:カット実行 1:カット未実行))
	WORD	wNGMark;				///< NGﾏｰｷﾝｸﾞする/しない
	WORD	w4Terminal;				///< 未使用(4端子ｵｰﾌﾟﾝﾁｪｯｸする/しない)
	WORD	wLogMode;				///< ﾛｷﾞﾝｸﾞﾓｰﾄﾞ
									///< 0:しない, 1:INITIAL TEST, 2:FINAL TEST, 3:INITIAL + FINAL)	
	//----- V808②↓ -----
	WORD intNgJudgeStop;			/// NG判定時停止(OVRERLOAD/CVエラー用)
	double dblReprobeVar;			/// 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量  
	double dblReprobePitch;			/// 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ
    double dblUdrStpUP;             /// 下方プローブステップ上昇距離      
    double dblUdrStpDown;           /// 下方プローブステップ下降距離      
	//----- V808②↑ -----
	BOOL	bTrimCutEnd;			///< カットオフ目標最大値に到達したらカットを終了する（TRUE）/しない（FALSE）
} TRIM_PLATE_DATA, *PTRIM_PLATE_DATA;

/// GPIB設定用データ(プレートデータ)
typedef	struct tag_TRIM_PLATE_GPIB {
	WORD	wGPIBflg;				///< GPIB制御の有無(0:無し,1:有り)
	WORD	wGPIBdlm;				///< ﾃﾞﾘﾐﾀ(0:CRLF, 1:CR, 2:LF,3:無し)
	WORD	wGPIBtout;				///< タイムアウト値(100ms単位)
	WORD	wGPIBdev;				///< 機器アドレス(0～30)
	char	cGPIBinit[40];			///< 初期化コマンド(後方空白)
	char	cGPIBtriger[10];		///< トリガコマンド(後方空白)
	char	cGPIBreserv[6];			///< 予備
	WORD	wGPIBmode;				///< 測定モード(0:絶対値測定モード, 1:偏差値測定モード)
} TRIM_PLATE_GPIB, *PTRIM_PLATE_GPIB;

//----- @@@013②↓ -----
/// GPIB設定用データ(プレートデータ)
typedef	struct tag_TRIM_PLATE_GPIB2 {
	WORD	wGPIBflg;				///< GPIB制御の有無(0:無し,1:有り)
	WORD	wGPIBdlm;				///< ﾃﾞﾘﾐﾀ(0:CRLF, 1:CR, 2:LF,3:無し)
	WORD	wGPIBtout;				///< タイムアウト値(100ms単位)
	WORD	wGPIBdev;				///< 機器アドレス(0～30)
	WORD	wEOI;					///< EOI(0:使用しない, 1:使用する)
	WORD	wPause1;				///< 設定コマンド1送信後ポーズ時間(1～32767msec) @@@014
	WORD	wPause2;				///< 設定コマンド2送信後ポーズ時間(1～32767msec) @@@014
	WORD	wPause3;				///< 設定コマンド3送信後ポーズ時間(1～32767msec) @@@014
	WORD	wPauseT;				///< トリガコマンド送信後ポーズ時間(1～32767msec)@@@014
	WORD	wRsv;					///< 予備
	char	cGPIBinit[40];			///< 初期化コマンド1(後方空白)
	char	cGPIBini2[40];			///< 初期化コマンド2(後方空白)
	char	cGPIBini3[40];			///< 初期化コマンド3(後方空白)
	char	cGPIBtriger[50];		///< トリガコマンド(後方空白)
	char	cGPIBName[10];			///< 機器名(後方空白)
	char	cRsv[8];				///< 予備
} TRIM_PLATE_GPIB2, *PTRIM_PLATE_GPIB2;
//----- @@@013②↑ -----

/// カット位置補正データ
typedef struct _tag_CUTPOS_CORRECT_DATA{
	double fCorrPosX[MAX_RESISTORS];	///< X座標補正ポジション
	double fCorrPosY[MAX_RESISTORS];	///< Y座標補正ポジション
	DWORD wCorrResult[MAX_RESISTORS];	///< パターン認識結果(0:補正なし, 1:OK, 2:NGｽｷｯﾌﾟ)
} CUTPOS_CORRECT_DATA, *PCUTPOS_CORRECT_DATA;

/// 要求データ
typedef struct _S_CMD_DAT {
	DWORD cmdNo;					///< コマンドNo
	double dbPara[MAX_PARAM_CNT];	///< double 型パラメータ
	DWORD dwPara[MAX_PARAM_CNT];	///< long	型パラメータ
//	double	dbPara[16];				///< double 型パラメータ
//	DWORD	dwPara[16];				///< long	型パラメータ
	DWORD flgTrim;				///< ﾄﾘﾐﾝｸﾞﾌﾗｸﾞ
} S_CMD_DAT;

typedef struct tag_TRIM_DAT {
	DWORD	cmdNo;
	WORD	type;					///< データタイプ
	WORD	index_reg;				///< 抵抗データ・インデックス
	WORD	index_cut;				///< カットデータ・インデックス
//	BYTE	dummy[2];				///< VB dummy data ...
	WORD	TkyKnd;					///< TKY/CHIP/NET種別(0:TKY, 1:CHIP, 2:NET)
	union tag_u {
		TRIM_PLATE_DATA		prmPlate;
		PRM_RESISTOR		prmRegistor;
		PRM_CUT_DATA		prmCut;
		PRM_CUT_PARAM		prmCutPattern;
		TRIM_PLATE_GPIB		prmGPIB;
		TRIM_PLATE_GPIB2	prmGPIB2; // @@@014
	} u;
} TRIM_DATA, *PTRIM_DATA;
/// @attention
/// 受信側はINTRIMとのI/Fにデータ型メールボックスを使用しているため、
/// データサイズは128Byteまでの制限。
typedef	struct _S_RES_DAT {
	DWORD	wTxSize;				///< 転送サイズ
	DWORD	status;					///< 0:成功 / -1:不成功
//	DWORD	errno;					///< errnoは予約語の為trimfuncで認識されない。??? 
	DWORD	dwerrno;				///< trimfuncに合わせ errno → dwerrno に変更する。
//	DWORD	signal[3];				///< 軸Signal状態 XYZ
	DWORD	signal[5];				///< 軸Signal状態 XYZ
	//DWORD	in_dat[INP_MAX];		///< I/O入力状態
	//								///<	[0]:コンソールSWセンス
	//								///<	[1]:インターロック関係SWセンス
	//								///<	[2]:オートローダLO
	//								///<	[3]:オートローダHI
	//								///<	[4]:固定アッテネータ
	//DWORD	outdat[OUT_MAX];		///< I/O出力状態
	//								///<	[0]:コンソール制御
	//								///<	[1]:サーボパワー
	//								///<	[2]:オートローダLO
	//								///<	[3]:オートローダHI
	//								///<	[4]:シグナルタワー
	DWORD	wData[4];				///< 戻値(2byte整数型)
	double	stgPos[5];				///< ステージ系現在位置 X,Y,Z,T,Z2
	double	bpPos[2];				///< BP現在位置 
	double	fData;
} S_RES_DAT;

typedef struct _S_RATIO2EXP {
	DWORD	cmdNo;						///< コマンドNo
	DWORD	rno;						///< 抵抗番号
	char	strExp[MAX_NUMCALLENGTH];	///< 計算式文字列
} S_RATIO2EXP, *PS_RATIO2EXP;


/// UCUT パラメータ（1要素）
typedef struct tag_UCUT_PARAM_EL {	
	double	ratio;						///< 目標値に対する初期値の差(%)
	double	ltp;						///< Lターンポイント 0.0～100.0 (%)
	double	ltp2;						///< Lターンポイント2 0.0～100.0 (%)
	double	L1;							///< L1 最大カッティング長  0.0000～20.0000 (mm)
	double	L2;							///< L2 最大カッティング長  0.0000～20.0000 (mm)
	double	r;							///< 円弧半径 (mm)
	double	v;							///< 速度
	double	v2;							///< 速度2(リターン/リトレース用) V8.0.0.14③
	double	nom;						///< 目標値
	BOOL	flg;						///< データ有効
} UCUT_PARAM_EL, *PUCUT_PARAM_EL;

/// UCUTパラメータテーブル（1抵抗分）
typedef struct tag_UCUT_PARAM {		
	short			rno;				///< 抵抗番号
	UCUT_PARAM_EL	el[20];				///< 20データまで保持
} UCUT_PARAM, *PUCUT_PARAM;

/// リトレースUCUT パラメータ（1要素）
typedef struct tag_UCUT4_PARAM_EL {
	double	dblRatio;					///< 目標値に対する初期値の差(%)
	double	dblLtp;						///< Lターンポイント 0.0～100.0 (%)
	double	dblLtp2;					///< LターンBポイント 0.0～100.0 (%)
	BOOL	blnFlg;						///< true:データ有効,false:テーブル終了
} UCUT4_PARAM_EL, *PUCUT4_PARAM_EL;

/// リトレースUCUTパラメータテーブル（1抵抗分）
typedef struct tag_UCUT4_PARAM {
	short			sRno;											///< 抵抗番号
	UCUT4_PARAM_EL	UCUT4_PARAM_EL_st[DEF_MAX_REG_ITEM_PARAMCNT];	///< 50データまで保持
} UCUT4_PARAM, *PUCUT4_PARAM;

/// 特注レシオ情報データ
typedef	struct	tag_RATIO3_DATA
{
	int			iGn;					///< ｸﾞﾙｰﾌﾟ番号(3～9) ※0はｽﾄｯﾊﾟｰ
	int			iBaseRn[2];				///< ﾍﾞｰｽ抵抗番号1,2(測定のみの抵抗番号)
	int			iRn[2];					///< 抵抗番号1,2
	double		fNom[2];				///< 比率1,2 
	double		fTargetVal[2];			///< 目標値1,2(Ω) ※ﾍﾞｰｽ抵抗の実測値 * 比率
} RATIO3_DATA;

typedef struct tag_LargeBuf {
//	WORD	wTxSize;
	double wTxSize;
	union {
		WORD	wd[256];
		double	dd[256];
	} u;
} LARGEBUF, *PLARGEBUF;

/// GPIB設定用データ
typedef	struct	tag_GPIB_DATA {
	WORD		wGPIBflg;				///< GPIB制御の有無(0:無し,1:有り)
	WORD		wInitFlg;				///< 初期設定フラグ(0:初期設定未, 1:初期設定済み)
	WORD		wGPIBdlm;				///< デリミタ(0:CRLF, 1:CR, 2:LF,3:無し)
    short		wAdex;					///< デバイス番号 = 21(0-30)
    short		wTimeout;				///< GPIB Timeout値(100ms)  
	double		dbDivVal;				///< 測定値をΩに変換する為の値 
    char		cInit[64];				///< 初期化コマンド文字列
    char		cTriger[16];			///< トリガー文字列
	WORD		wGPIBmode;				///< 測定モード(0:絶対値測定モード, 1:偏差値測定モード)
	double		dbNOM;					///< 標準抵抗値(単位Ω) ※偏差値測定モード時設定
	int			iRange;					///< 測定レンジ(0:R1～6:R6)
} GPIB_DATA, *PGPIB_DATA;
//========================================================================
#pragma pack()

//------------------------------------------------------------------------------
//		内部関数定義
//      (注)VBからは下記のように定義をする。(Alias名は.mapを参照)
//          Public Declare Function PROBON Lib
//           "C:\TRIM\DllTrimFnc.dll" Alias "_PROBON@0" () As Long
//------------------------------------------------------------------------------
DLLTRIMFUNC_API long __stdcall BIFC(short TIM, short BRDIDX);                  
DLLTRIMFUNC_API long __stdcall BSIZE(double BSX, double BSY);                  
DLLTRIMFUNC_API long __stdcall CIRCUT(double V, double RADI, double ANG2, double ANG);
DLLTRIMFUNC_API long __stdcall CMARK(LPCTSTR MKSTR, double STX, double STY, double HIGH, double V, short ANG);         
DLLTRIMFUNC_API long __stdcall CUT2(double L, double V, short ANG);
#ifdef __INTIME2_MOVE__
DLLTRIMFUNC_API long __stdcall DREAD(short* DGSW);                    
DLLTRIMFUNC_API long __stdcall DREAD2(short* DGL, short* DGH, short* DGSW);                
#endif
DLLTRIMFUNC_API long __stdcall DSCAN(short HP, short LP, short GP);                
DLLTRIMFUNC_API long __stdcall EXTIN(long* EIN);                    
DLLTRIMFUNC_API long __stdcall EXTOUT(long ODAT);                    
DLLTRIMFUNC_API long __stdcall EXTOUT1(long EON, long EOFF);                  
DLLTRIMFUNC_API long __stdcall EXTOUT2(long EON, long EOFF);                  
DLLTRIMFUNC_API long __stdcall EXTOUT3(long EON, long EOFF);                  
DLLTRIMFUNC_API long __stdcall FPRESET();                     
DLLTRIMFUNC_API long __stdcall FPSET();                     
DLLTRIMFUNC_API long __stdcall FPSET2(long TIM);                    
DLLTRIMFUNC_API long __stdcall GETERRSTS(long* ERRSTS);                    
DLLTRIMFUNC_API long __stdcall GETSETTIME();                     
DLLTRIMFUNC_API long __stdcall HCUT2(short LTDIR, double L1, double L2, double L3, double V, short ANG);          
DLLTRIMFUNC_API long __stdcall IACLEAR();                     
DLLTRIMFUNC_API long __stdcall ICLEAR(short GADR);                    
DLLTRIMFUNC_API long __stdcall ICUT2(short N, double L, double V, short ANG);              
DLLTRIMFUNC_API long __stdcall IDELIM(short DLM);                    
DLLTRIMFUNC_API long __stdcall IREAD(short GADR, LPCTSTR DAT);                  
DLLTRIMFUNC_API long __stdcall IRHVAL(short GADR, short HED, double* DAT);                
DLLTRIMFUNC_API long __stdcall IRVAL(short GADR, double* DAT);                  
DLLTRIMFUNC_API long __stdcall ITIMEOUT(short TIM);                    
DLLTRIMFUNC_API long __stdcall ITRIGGER(short GADR);                    
DLLTRIMFUNC_API long __stdcall IWRITE(short GADR, LPCTSTR DAT);

DLLTRIMFUNC_API long __stdcall LATTSET(long FAT,long RAT);
DLLTRIMFUNC_API long __stdcall ATTRESET();
DLLTRIMFUNC_API long __stdcall ATTSTATUS1(long *status);
DLLTRIMFUNC_API long __stdcall ATTSTATUS2(long *FAT,long *RAT);

DLLTRIMFUNC_API long __stdcall LCUT2(short LTDIR, double L1, double L2, double V, short ANG);            
DLLTRIMFUNC_API long __stdcall MFSET(LPCTSTR MSDEV);
//(新規IF)差電流対応版
DLLTRIMFUNC_API long __stdcall MFSET_EX(LPCTSTR MSDEV, double targetVal); 

DLLTRIMFUNC_API long __stdcall MOVE(double dpStx, double dpSty, short Flg);                
DLLTRIMFUNC_API long __stdcall MSCAN(short HP, short LP, short GP1, short GP2, short GP3, short GP4, short GP5);        
DLLTRIMFUNC_API long __stdcall QRATE(double QR);                    
DLLTRIMFUNC_API long __stdcall RMEAS(short MODE, short DVM, double *R);                
DLLTRIMFUNC_API long __stdcall ROUND(long PLS);                    
DLLTRIMFUNC_API long __stdcall RTEST(double NOM, short MODE, double LOW, double HIGH, short JM, short DVM);          
DLLTRIMFUNC_API long __stdcall RTRACK(double NOM, short JM);                  
DLLTRIMFUNC_API long __stdcall START(double XOFF, double YOFF); 
DLLTRIMFUNC_API long __stdcall STCUT(double L , double V , double NOM, double CUTOFF,double V2, double Q2, short DIR, short CUTMODE, short MODE);   
DLLTRIMFUNC_API long __stdcall TEST(double X, double NOM, short MODE, double LOW, double HIGH);            
DLLTRIMFUNC_API long __stdcall UCUT2(short LTDIR, double L1, double L2, double RADI, double V, short ANG);          
DLLTRIMFUNC_API long __stdcall VCIRTRIM(short SLP, double NOM, double V, double RADI, double ANG2, double ANG);          
DLLTRIMFUNC_API long __stdcall VHTRIM2(short SLP, double NOM, short MD, double LTP, short LTDIR, double L1, double L2, double L3, double V, short ANG);  
DLLTRIMFUNC_API long __stdcall VITRIM2(short SLP, double NOM, short MD, short N, double L, double V, short ANG);        
DLLTRIMFUNC_API long __stdcall VLTRIM2(short SLP, double NOM, short MD, double LTP, short LTDIR, double L1, double L2, double V, short ANG);    
DLLTRIMFUNC_API long __stdcall VMEAS(short MODE, short DVM, double* V);                
DLLTRIMFUNC_API long __stdcall VTEST(double NOM, short MODE, double LOW, double HIGH, short JM, short DVM);          
DLLTRIMFUNC_API long __stdcall VTRACK(short SLP, double NOM, short JM);                
DLLTRIMFUNC_API long __stdcall VTRIM2(short SLP, double NOM, double L, double V, short ANG);            
DLLTRIMFUNC_API long __stdcall VUTRIM2(short SLP, double NOM, short MD, double LTP, short LTDIR, double L1, double L2, double RADI, double V, short ANG);  
DLLTRIMFUNC_API long __stdcall ZCONRST();                     
DLLTRIMFUNC_API long __stdcall ZGETBPPOS(double* XP, double* YP);                  
DLLTRIMFUNC_API long __stdcall ZGETSRVSIGNAL(long* X, long* Y, long* Z, long* T);              
DLLTRIMFUNC_API long __stdcall ZSETPOS2(double POS2X, double POS2Y, double POS2Z);                
DLLTRIMFUNC_API long __stdcall ZTIMERINIT();                     
DLLTRIMFUNC_API long __stdcall ZVMEAS(short MODE, short RANGE, double* V);                
DLLTRIMFUNC_API long __stdcall ZWAIT(long lngWaitMilliSec);                    
DLLTRIMFUNC_API long __stdcall MEASURE(short MEASMODE, short RANGSETTYPE, short MEASTYPE, double TARGET, short RANGE, double *RESULT);

//------------------------------------------
// カット系I/F
//------------------------------------------
DLLTRIMFUNC_API long __stdcall TRIM_ST(PCUT_COMMON_PRM pCutCmn);
DLLTRIMFUNC_API long __stdcall TRIM_L(PCUT_COMMON_PRM pCutCmn);
DLLTRIMFUNC_API long __stdcall TRIM_HkU(PCUT_COMMON_PRM pCutCmn);
DLLTRIMFUNC_API long __stdcall TRIM_ES(PCUT_COMMON_PRM pCutCmn);
// 
//V3.0.0.0
DLLTRIMFUNC_API long __stdcall TRIM_MK(LPCTSTR MKSTR, double STX, double STY, double HIGH, double V1, short ANG, double QRate1, short condNoCut1, short moveMode);
DLLTRIMFUNC_API long __stdcall TRIM_SC(PCUT_COMMON_PRM pCutCmn);
//V3.0.0.0//

//--- ユーザ公開検討中(2010/12/14)
//DLLTRIMFUNC_API long __stdcall TRIM_SC(PCUT_COMMON_PRM pCutCmn);
DLLTRIMFUNC_API long __stdcall TRIM_IX(PCUT_COMMON_PRM pCutCmn);	
//---　必要と判断した時点でDllTrimFuncにIFを追加する。
//------------------------------------------

//*********************************************************************
//	2010/01/26 ユーザ向けオプションIF
//*********************************************************************
DLLTRIMFUNC_API long __stdcall CTRIM(double X, double Y, double VX, double VY, double LIMX, double LIMY);
DLLTRIMFUNC_API long __stdcall EXTRSTSET(long ODATA);          
DLLTRIMFUNC_API long __stdcall FAST_WMEAS(double *MR, short OSC);        
DLLTRIMFUNC_API long __stdcall GET_Z2_POS(double *Z2);          
DLLTRIMFUNC_API long __stdcall INP16(long ADR, long* DAT);        
DLLTRIMFUNC_API long __stdcall LASEROFF();           
DLLTRIMFUNC_API long __stdcall LASERON();           
DLLTRIMFUNC_API long __stdcall OUT16(long ADR, long DAT);        
DLLTRIMFUNC_API long __stdcall RangeCorrect(short Idx, double Val, short Flg, short RMin, short RMax);  
DLLTRIMFUNC_API long __stdcall RATIO2EXP(long RNO, LPCTSTR DAT);        
DLLTRIMFUNC_API long __stdcall RBACK();           
DLLTRIMFUNC_API long __stdcall RESET();      

DLLTRIMFUNC_API long __stdcall SYSTEM_RESET();
DLLTRIMFUNC_API long __stdcall CLEAR_SERVO_ALARM(short xy, short zt);		// @@@007
DLLTRIMFUNC_API long __stdcall CLEAR_SERVO_ALARM_Z2(short z2);				// @@@017

DLLTRIMFUNC_API long __stdcall SERVO_POWER(int XOnOff, int YOnOff, int ZOnOff, int TOnOff);
DLLTRIMFUNC_API long __stdcall SERVO_POWER_Z2(int Z2OnOff, int YobiOnOff); // @@@017
DLLTRIMFUNC_API long __stdcall RINIT();           
DLLTRIMFUNC_API long __stdcall ROUND4(double ANG);          
DLLTRIMFUNC_API long __stdcall SETDLY(long DTIME);          
DLLTRIMFUNC_API long __stdcall ZRANGTRIM(double NOM, short RNG, double L, double V, short ANG);  
DLLTRIMFUNC_API long __stdcall ZRCIRTRIM(double NOM, short RNG, double V, double RADI, double ANG2, double ANG);
DLLTRIMFUNC_API long __stdcall ZRTRIM2(double NOM, short RNG, double L, double V, short ANG);  
DLLTRIMFUNC_API long __stdcall ZSETBPTIME(long BPTIME,long EPTIME);         
DLLTRIMFUNC_API long __stdcall RANGE_SET(LPCTSTR MSDEV, int rangeNo);

//*********************************************************************
//	2010/01/26 内部公開向けIF（エラーチェック未実装）
//*********************************************************************
DLLTRIMFUNC_API long __stdcall ALDFLGRST();                             
DLLTRIMFUNC_API long __stdcall BP_CALIBRATION(double GainX, double GainY, double OfsX, double OfsY);                      
DLLTRIMFUNC_API long __stdcall BPLINEARITY(short XY, short Idx, short Flg, double Val);                      
DLLTRIMFUNC_API long __stdcall BPOFF(double BPOX, double BPOY);                          
DLLTRIMFUNC_API long __stdcall CUTPOSCOR(short RN, void far* POSX, void far* POSY, void far* FLG);                   
DLLTRIMFUNC_API long __stdcall CUTPOSCOR_ALL(int resCnt, PCUTPOS_CORRECT_DATA pCutPosCorrData); 
DLLTRIMFUNC_API long __stdcall DebugMode(short mode, short level);                          
DLLTRIMFUNC_API long __stdcall EMGRESET();                             
DLLTRIMFUNC_API long __stdcall EXTOUT4(long EON, long EOFF);                          
DLLTRIMFUNC_API long __stdcall EXTOUT5(long EON, long EOFF);                          
DLLTRIMFUNC_API long __stdcall GET_VERSION(double* VER);
DLLTRIMFUNC_API long __stdcall GPBActRen(short brdIdx);                            
DLLTRIMFUNC_API long __stdcall GPBAdrStRead(short* btadrst, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBClrRen(short brdIdx);                            
DLLTRIMFUNC_API long __stdcall GPBExeSpoll(short* bttlks, short wtlknum, short* bttlk, short* btstb, short brdIdx);                    
DLLTRIMFUNC_API long __stdcall GPBGetAdrs(short* btadrs, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBGetDlm(short* btdlm, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBGetTimeout(short* wtim, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBIfc(short wtim, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBInit(short brdIdx);                            
DLLTRIMFUNC_API long __stdcall GPBRecvData(short* btdata, short wsize, short* wrecv, short brdIdx);                      
DLLTRIMFUNC_API long __stdcall GPBSendCmd(LPCTSTR btcmd, short wsize, short brdIdx);                        
DLLTRIMFUNC_API long __stdcall GPBSendData(LPCTSTR btdata, short wsize, short weoi, short brdIdx);                      
DLLTRIMFUNC_API long __stdcall GPBSetDlm(short btdlm, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBSetTimeout(short wtim, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPBWaitForSRQ(short timeout, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPERecv(short bttlk, short* btlsns, short wlsnnum, short *btmsge, short wsize, short* wrecv, short brdIdx);                
DLLTRIMFUNC_API long __stdcall GPESend(short* btlsns, short wlsnnum, LPCTSTR btmsge, short wsize, short brdIdx);                    
DLLTRIMFUNC_API long __stdcall GPSGetSrqTkn(long* hSrqSem, short brdIdx);                          
DLLTRIMFUNC_API long __stdcall GPSInit();                             
DLLTRIMFUNC_API long __stdcall GPSLExeSRQ(short weoi, short wdevst, short brdIdx);                        
DLLTRIMFUNC_API long __stdcall GPSLock(short timeout);                            
DLLTRIMFUNC_API long __stdcall GPSUnlock();                             
DLLTRIMFUNC_API long __stdcall ILUM(short SW);                            
DLLTRIMFUNC_API long __stdcall InitFunction();                             
DLLTRIMFUNC_API long __stdcall IREAD2(short GADR, short GADR2, LPCTSTR DAT);                        
DLLTRIMFUNC_API long __stdcall IREADM(short GADR, short FAR* MAX, LPCTSTR DAT[], LPCTSTR DLM);                     
DLLTRIMFUNC_API long __stdcall IRHVAL2(short GADR, short GADR2, short HED, double* DAT);                      
DLLTRIMFUNC_API long __stdcall IRMVAL(short GADR, short* MAX, double* DAT, LPCTSTR DLM);                      
DLLTRIMFUNC_API long __stdcall IRVAL2(short GADR, short GADR2, double* DAT);                        
DLLTRIMFUNC_API long __stdcall ITIMESET(short MODE);                            
DLLTRIMFUNC_API long __stdcall IWRITE2(short GADR, short GADR2, LPCTSTR DAT);                        
DLLTRIMFUNC_API long __stdcall NO_OPERATION(double *X, double *Y, double *Z, double *BPx, double *BPy);                    
DLLTRIMFUNC_API long __stdcall GET_STATUS(int bpMode, double *X, double *Y, double *Z, double *BPx, double *BPy); 

DLLTRIMFUNC_API long __stdcall OUTBIT(short CATEGORY, short BITNUM, short BON);                        
DLLTRIMFUNC_API long __stdcall PIN16(short ADR, short* DAT);                          
DLLTRIMFUNC_API long __stdcall PROBOFF();  
DLLTRIMFUNC_API long __stdcall PROBOFF_EX(double Pos); //@@@008                       
DLLTRIMFUNC_API long __stdcall PROBOFF2();                             
DLLTRIMFUNC_API long __stdcall PROBON();                             
DLLTRIMFUNC_API long __stdcall PROBON2(double Z2ON);                            
DLLTRIMFUNC_API long __stdcall PROBUP(double UP);                            
DLLTRIMFUNC_API long __stdcall PROCPOWER(short POWER);                            
DLLTRIMFUNC_API void __stdcall PROP_SET(double Zon, double Zoff, double PosX, double PosY, double SmaxX, double SmaxY);                  
DLLTRIMFUNC_API long __stdcall RMeasHL(short HP, short LP, short MODE, double NOM, double *R, short *AD);                  
DLLTRIMFUNC_API long __stdcall SBACK();                             
DLLTRIMFUNC_API long __stdcall SLIDECOVERCHK(short CHK);
DLLTRIMFUNC_API long __stdcall COVERCHK_ONOFF(short CHK);	//@@@010
DLLTRIMFUNC_API long __stdcall GETCOVERCHK_STS(short *CVRCHK, short *SLDCVR); //@@@011
DLLTRIMFUNC_API long __stdcall SMOVE(double XD, double YD);                          
DLLTRIMFUNC_API long __stdcall SMOVE2(double XP, double YP);                          
DLLTRIMFUNC_API long __stdcall SMOVE3(double XP, double YP);                          
DLLTRIMFUNC_API long __stdcall SYSINIT(double ZOFF, double ZON);                          
DLLTRIMFUNC_API long __stdcall TRIM_NGMARK(double POSX, double POSY, short TM, short SN, short SW, short FLG);                  
DLLTRIMFUNC_API long __stdcall TRIM_RESULT(short KD, short SN, short NM, short CI, short DI, void far* pRes);                 

DLLTRIMFUNC_API long __stdcall TRIM80(double X, double Y, double V);                        
DLLTRIMFUNC_API long __stdcall TRIMBLOCK(short MD, short HZ, short RI, short CI, short NG);                 

DLLTRIMFUNC_API long __stdcall TRIMDATA(void far* pDat, void far* pRes);                        
DLLTRIMFUNC_API long __stdcall TRIMDATA_PLATE(PTRIM_PLATE_DATA pPlateDat, int tkyKnd); 
DLLTRIMFUNC_API long __stdcall TRIMDATA_GPIB(PTRIM_PLATE_GPIB pGpibDat, int tkyKnd); 
DLLTRIMFUNC_API long __stdcall TRIMDATA_GPIB2(PTRIM_PLATE_GPIB2 pGpibDat, int tkyKnd); // @@@013②

DLLTRIMFUNC_API long __stdcall TRIMDATA_RESISTOR(PPRM_RESISTOR pResistorDat, int resNo); 
DLLTRIMFUNC_API long __stdcall TRIMDATA_CUTDATA(PPRM_CUT_DATA pCutDat, int resNo, int cutNo);
DLLTRIMFUNC_API long __stdcall TRIMDATA_CUTPRM(PPRM_CUT_PARAM pCutPrm, int resNo, int cutNo); 

DLLTRIMFUNC_API long __stdcall TRIMEND();                             
DLLTRIMFUNC_API long __stdcall TSTEP(short BNX, short BNY, double stepOffX, double stepOffY);                          
DLLTRIMFUNC_API long __stdcall UCUT_RESULT(short RNO, short CNO, short *UcutNo, double *InitVal);                      
DLLTRIMFUNC_API long __stdcall UCUT_RESULT2(short RNO, short CNO, short *UcutNo, double *InitVal, short *Result); // V8.0.0.14⑤
DLLTRIMFUNC_API long __stdcall UCUT4RESULT(short* sRegNo_p, short* sCutNo_p);                          
DLLTRIMFUNC_API long __stdcall VCTRIM(short SLP, double NOM, short MD, double X, double Y, double VX, double VY, double LIMX, double LIMY);            
DLLTRIMFUNC_API long __stdcall VRangeCorrect(short Idx, double Val, short Flg, short RMin, short RMax);                    
DLLTRIMFUNC_API long __stdcall VUTRIM4(short SLP, double NOM, short MD, double LTP, short LTDIR, double L1, double L2, double RADI, double V, short ANG, short TRMD, double TRL, short CN, short DT, short MODE);
DLLTRIMFUNC_API long __stdcall Z_INIT();       ///< 内部ではAXIS_Z_INITをコール。下位互換の為にIFとしては残す。                      
DLLTRIMFUNC_API long __stdcall ZABSVACCUME(long ZON);                            
DLLTRIMFUNC_API long __stdcall ZATLDGET(long* LDIN);                            
DLLTRIMFUNC_API long __stdcall ZATLDSET(long LDON, long LDOFF);
DLLTRIMFUNC_API long __stdcall ZATLDRED(long FAR* LDOUT); // @@@009
DLLTRIMFUNC_API long __stdcall ZBPLOGICALCOORD(long dwCOORD);                            
DLLTRIMFUNC_API long __stdcall ZGETDCVRANG(double* VMAX);                            
DLLTRIMFUNC_API long __stdcall ZGETPHPOS(double* NOWXP, double* NOWYP);
DLLTRIMFUNC_API long __stdcall ZGETPHPOS2(double* NOWXP, double* NOWYP); // V808③               
DLLTRIMFUNC_API long __stdcall ZINPSTS(long SW, long* STS);                          
DLLTRIMFUNC_API long __stdcall ZLATCHOFF();                             
DLLTRIMFUNC_API long __stdcall ZMOVE(double POS, short MD);                          
DLLTRIMFUNC_API long __stdcall ZMOVE2(double POS, short MD);   
DLLTRIMFUNC_API long __stdcall ZZMOVE(double POS, short MD);  // V3.0.0.0②                          
DLLTRIMFUNC_API long __stdcall ZZMOVE2(double POS, short MD); // V3.0.0.0②               
DLLTRIMFUNC_API long __stdcall ZSELXYZSPD(short SPD);         
DLLTRIMFUNC_API long __stdcall ZSETUCUT(short MD, short RNO, short INDEX, short EL, double RATIO, double LTP, double LTP2);                
DLLTRIMFUNC_API long __stdcall ZSLCOVERCLOSE(short ZONOFF);                            
DLLTRIMFUNC_API long __stdcall ZSLCOVEROPEN(short ZONOFF);                            
DLLTRIMFUNC_API long __stdcall ZSTGXYMODE(long mode);                            
DLLTRIMFUNC_API long __stdcall ZSTOPSTS();                             
DLLTRIMFUNC_API long __stdcall ZSTOPSTS2();                             
DLLTRIMFUNC_API long __stdcall ZSYSPARAM1(short POWERCYCLE, short THETA, short BPDIRXY, short BPSIZE, short DCSCANNER, short DCVRANGE, short LRANGE, double LDPOSX, double LDPOSY, short FPSUP, short DELAYSKIP, short OSCILLATOR, short MACHINETYPE);   
DLLTRIMFUNC_API long __stdcall ZSYSPARAM2(short PRBTYP, double SMINMAXZ2, short ZPTIMEON, short ZPTIMEOFF, short XYTBL, double SMAXX, double SMAXY, long ABSTIME, double TRIMX, double TRIMY, short BPSOFTMOVELIMX, short BPSOFTMOVELIMY); 
DLLTRIMFUNC_API long __stdcall ZSYSPARAM3(short ProcPower2, long GrvTime, short UcutType, long ExtBit, short PosSpd, short BiasOn_AddTime); // @@@004
DLLTRIMFUNC_API long __stdcall ZSYSPARAM4(double *pdbPRM, long *pdwPRM); // V808①
DLLTRIMFUNC_API long __stdcall MIDIUMCUTPARAM(double *pdbPRM, long *pdwPRM);	// V8.0.0.16①
DLLTRIMFUNC_API long __stdcall SETLOADPOS(double LDPOSX, double LDPOSY);	// V8.0.0.10

DLLTRIMFUNC_API long __stdcall ZZGETRTMODULEINFO();                             

DLLTRIMFUNC_API long __stdcall SMOVE_EX(double XD, double YD, short bNotLsrStop); 
DLLTRIMFUNC_API long __stdcall SMOVE2_EX(double XP, double YP, short bNotLsrStop, short JogMode);

DLLTRIMFUNC_API long __stdcall AXIS_X_INIT(); 
DLLTRIMFUNC_API long __stdcall AXIS_Y_INIT(); 
DLLTRIMFUNC_API long __stdcall AXIS_Z_INIT(); 

DLLTRIMFUNC_API long __stdcall SET_THETA_CORRECT(double XPos, double YPos);
DLLTRIMFUNC_API long __stdcall SET_THETA_CORRECT_ANGLE(double Angle); // @@@006

DLLTRIMFUNC_API long __stdcall ISALIVE_INTIME();
DLLTRIMFUNC_API long __stdcall TERMINATE_INTIME(); 

// SL43xR向け新規追加
DLLTRIMFUNC_API long __stdcall GET_ALLAXIS_STATUS(long* err, long* AllStatus);
DLLTRIMFUNC_API long __stdcall INTERLOCK_CHECK(int *InterlockSts, long *status); 
DLLTRIMFUNC_API long __stdcall LAMP_CTRL(int LampNo, BOOL OnOff); 
DLLTRIMFUNC_API long __stdcall SIGNALTOWER_CTRLEX(int OnBit, int OffBit); 
DLLTRIMFUNC_API long __stdcall COVERLATCH_CLEAR();
DLLTRIMFUNC_API long __stdcall COVERLATCH_CHECK(long* cvrLatSts);
DLLTRIMFUNC_API long __stdcall START_SWWAIT(long* SwitchSts);
DLLTRIMFUNC_API long __stdcall START_SWCHECK(BOOL bReleaseCheck, long* SwitchChk);
DLLTRIMFUNC_API long __stdcall HALT_SWCHECK(long* SwitchChk);
DLLTRIMFUNC_API long __stdcall STARTRESET_SWCHECK(BOOL bReleaseCheck, long* SwitchChk);
DLLTRIMFUNC_API long __stdcall STARTRESET_SWWAIT(long* SwitchSts);
DLLTRIMFUNC_API long __stdcall GET_Z_POS(double *Z); 
DLLTRIMFUNC_API long __stdcall GET_Z_POS2(double *Z); // V809③
DLLTRIMFUNC_API long __stdcall CONSOLE_SWCHECK(BOOL bReleaseCheck, long* SwitchChk);
DLLTRIMFUNC_API long __stdcall Z_SWCHECK(long* SwitchChk);
DLLTRIMFUNC_API long __stdcall CLAMP_CTRL(long XOnOff, long YOnOff);
DLLTRIMFUNC_API long __stdcall CLAMP_GETSTS(long *XOnOff, long *YOnOff); 
DLLTRIMFUNC_API long __stdcall BP_GET_CALIBDATA(double *GainX, double *GainY, double *OfsX, double *OfsY);
DLLTRIMFUNC_API long __stdcall PMON_SHUTCTRL(long onOff); 
DLLTRIMFUNC_API long __stdcall PMON_MEASCTRL(long measMode); 


// デバッグ/評価用関数
DLLTRIMFUNC_API long __stdcall SETLOG_ALLTARGET(short base, short io, 
												short laser, short bp, short meas, 
												short trim, short correct, short stage, 
												short laserBase, short bpBase, short measBase, 
												short trimBase, short correctBase, short stageBase, 
												short loader);
DLLTRIMFUNC_API long __stdcall SETLOG_TARGET(short segNo, short mode); 
DLLTRIMFUNC_API long __stdcall GETLOG_TARGET(BOOL status[]); 
DLLTRIMFUNC_API long __stdcall PERFORMCHK(DWORD ADDR, DWORD COUNT, DWORD WAIT); 
DLLTRIMFUNC_API long __stdcall SETAXISSPD(DWORD XL, DWORD XH, DWORD YL, DWORD YH, DWORD ZL, DWORD ZH, DWORD Z2L, DWORD Z2H, DWORD waitTime);
DLLTRIMFUNC_API long __stdcall SETAXISSPDY(DWORD YH);			// @@@018
DLLTRIMFUNC_API long __stdcall LSI_RESET();
DLLTRIMFUNC_API long __stdcall SETAXISPRM(DWORD RMGx, DWORD RMGy, DWORD RMGz, DWORD RDPx, DWORD RDPy, DWORD RDPz); //@@@012
DLLTRIMFUNC_API long __stdcall SETAXISPRM2(DWORD AXIS, DWORD FL, DWORD FH, DWORD DRVRAT, DWORD MAGNIF); // V8.0.0.16③

// 2013.02.25 add Yamazaki 
// 超低抵抗用に追加		
DLLTRIMFUNC_API long __stdcall MES_LOW_RESIST(long measMode,long curmode,long cnt,long time,long CurSel,double *res,long *WDres);
DLLTRIMFUNC_API long __stdcall PARAM_LOW_RESIST(long BoardSel,long modeSel,long Gain,long RevTime,long ResTime,long mode,long MesCnt,long MesTime,long CurSel);

DLLTRIMFUNC_API long __stdcall MES_LOW_RESULT(long measMode,void far* pRes);
DLLTRIMFUNC_API long __stdcall TRIMBLOCKMEASURE(short MD, short HZ, short RI, short CI, short NG);    // @@@015
//----- V809①↓ -----
DLLTRIMFUNC_API long __stdcall LASERONESHOT(int dotcnt);
DLLTRIMFUNC_API long __stdcall TRIM_ST1(short mMode, short cMode, double Nom, short Slp, double Ang, double L1, double spdo, double spdr, double Qrateo ,double Qrater, short iCondo, short iCondr);
DLLTRIMFUNC_API long __stdcall TRIM_L1(short mMode, short cMode, double Nom, short Slp, double Ang, double Ltp, short LtDir, short MType, double L1,double L2, double spdo1, double spdo2, double Qrateo1,double Qrateo2, short iCondo1,short iCondo2);
DLLTRIMFUNC_API long __stdcall TRIM_IX1(short mMode, double Nom, short Slp, double Ang, short IScCnt , short IMeasMode, short MType, double L1, double spdo1, double Qrateo1, short iCondo1);
//DLLTRIMFUNC_API long __stdcall TRIM_H1(short mMode, short cMode, double Nom, short Slp, double Ang, double Ltp, short LtDir, short MType, double Radi1, double Radi2, short HKOrUType);
DLLTRIMFUNC_API long __stdcall TRIM_H1(short mMode, short cMode, double Nom, short Slp, double Ang, double Ltp, short LtDir, short MType, double Radi1, double Radi2, short HKOrUType ,double L1,double L2,double L3, double spdo1, double spdo2,double spdo3, double Qrateo1,double Qrateo2,double Qrateo3, short iCondo1,short iCondo2,short iCondo3);
//----- V809①↑ -----
DLLTRIMFUNC_API long __stdcall TRIM_HkU(PCUT_COMMON_PRM pCutCmn);


DLLTRIMFUNC_API long __stdcall SetFixAttInfo(long *pFixAttAry); // V809②
DLLTRIMFUNC_API long __stdcall SetFixAttOneInfo(int CondNum, int FixAtt); // V809②
DLLTRIMFUNC_API long __stdcall SET_FIX_ATT_INFO(long *pFixAttAry);				// V8.0.0.10③
DLLTRIMFUNC_API long __stdcall SET_FIX_ATTONE_INFO(int CondNum, int FixAtt);	// V8.0.0.10③
DLLTRIMFUNC_API long __stdcall SETZOFFPOS(double Pos); // V8.0.0.10②

//********************************************

