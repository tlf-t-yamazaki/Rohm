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

// �ȉ��� ifdef �u���b�N�� DLL ����̃G�N�X�|�[�g��e�Ղɂ���}�N�����쐬���邽�߂� 
// ��ʓI�ȕ��@�ł��B���� DLL ���̂��ׂẴt�@�C���́A�R�}���h ���C���Œ�`���ꂽ DLLTRIMFUNC_EXPORTS
// �V���{���ŃR���p�C������܂��B���̃V���{���́A���� DLL ���g���v���W�F�N�g�Œ�`���邱�Ƃ͂ł��܂���B
// �\�[�X�t�@�C�������̃t�@�C�����܂�ł��鑼�̃v���W�F�N�g�́A 
// DLLTRIMFUNC_API �֐��� DLL ����C���|�[�g���ꂽ�ƌ��Ȃ��̂ɑ΂��A���� DLL �́A���̃}�N���Œ�`���ꂽ
// �V���{�����G�N�X�|�[�g���ꂽ�ƌ��Ȃ��܂��B
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
//		�\���̒�`
//		���[�U�v���O����orTKY�n�Ƃ�I/F�Ɏg�p����\����
//		.NET�����A���C�����g��8�o�C�g�ɕύX
//		TKY�n�̃p�����[�^�Ɋւ��ẮA���J�̌���������
//------------------------------------------------------------------------------
#define MAX_PARAM_CNT				(32)	///< �p�����[�^��
#define	MAX_CUT_CND					(8)		///< �ő���H�����ݒ萔(���H����1-4)
#define MAX_CND_CNT				    (32)	///< ���H�������@// V809�A

// TrimBlock DataType
#define	TRIMDATA_TYP_PLATE			(1)
#define	TRIMDATA_TYP_RES			(2)
#define	TRIMDATA_TYP_CUT			(3)
#define	TRIMDATA_TYP_CUTPRM			(4)
#define	TRIMDATA_TYP_GPIB			(8)
#define	TRIMDATA_TYP_GPIB2			(9)		// @@@013�A 

// Console Lamp No
#define CSL_LAMPNO_START			(0)		///< �X�^�[�g�����v�w��ԍ�
#define CSL_LAMPNO_RESET			(1)		///< ���Z�b�g�����v�w��ԍ�
#define CSL_LAMPNO_Z				(2)		///< Z�����v�w��ԍ�
#define CSL_LAMPNO_HALT				(5)		///< HALT�����v�w��ԍ�
// Console Switch status No
#define CSL_SWON_START				(1)		///< �X�^�[�g�{�^��ON
#define CSL_SWON_RESET				(3)		///< ���Z�b�g�{�^��ON
#define CSL_SWON_HALT				(4)		///< HALT�{�^��ON
#define CSL_SWON_Z					(5)		///< Z�{�^��ON
// Switch status
#define SWITCH_ON					(1)		///< �X�C�b�`ON���
#define SWITCH_OFF					(0)		///< �X�C�b�`OFF���

#define MAX_RESISTORS				(256)	///< ��R���̍ő�l
#define MAX_NUMCALLENGTH			(101)	///< ���V�I�v�Z���̕����� 100�����܂�
#define MAX_RATIORES				(999)	///< ���V�I�v�Z���̒�R���̍ő�l
#define	DEF_MAX_REG_ITEM_PARAMCNT	(50)	///< �ő僌�W�X�^�����ڐ�
#define	DEF_MAX_REG_PARAMCNT		(100)	///< �ő僌�W�X�^��
#define	DEF_MAX_PROB		        (128)	///< �ő�v���[�u�� V8.0.0.14�A

// Slidecover get target
#define SLD_OPEN_CHECK		0				///< �X���C�h�J�o�[�F�I�[�v���`�F�b�N
#define SLD_CLOSE_CHECK		1				///< �X���C�h�J�o�[�F�N���[�Y�`�F�b�N


#pragma pack(8)
//------------------------------------------------------------
// ���[�U�v���O���������J�b�g�p�����[�^���
//------------------------------------------------------------
/// ------ �e�J�b�g�̃p�����[�^(�����A�X�s�[�h�AQ���[�g)�ۑ��̈�
typedef struct _tag_DBL_CUTCOND_ARRAY{
    double dblL1;					///< Line1�p�̃p�����[�^
    double dblL2;					///< Line2�p�̃p�����[�^
    double dblL3;					///< Line3�p�̃p�����[�^
    double dblL4;					///< Line4�p�̃p�����[�^
}DBL_CUTCOND_ARRAY;

typedef struct _tag_SRT_CUTCOND_ARRAY{
    short srtL1;					///< Line1�p�̃p�����[�^
    short srtL2;					///< Line2�p�̃p�����[�^
    short srtL3;					///< Line3�p�̃p�����[�^
    short srtL4;					///< Line4�p�̃p�����[�^
}SRT_CUTCOND_ARRAY;
/////###888 
/// ------ ���H�ݒ�\����
typedef struct _tag_CUT_COND_STRUCT{
    DBL_CUTCOND_ARRAY CutLen;		///< �J�b�g�����
    DBL_CUTCOND_ARRAY SpdOwd;		///< �J�b�g�X�s�[�h�i���H�j
    DBL_CUTCOND_ARRAY SpdRet;		///< �J�b�g�X�s�[�h�i���H�j
    DBL_CUTCOND_ARRAY QRateOwd;		///< �J�b�gQ���[�g�i���H�j
    DBL_CUTCOND_ARRAY QRateRet;		///< �J�b�gQ���[�g�i���H�j
    SRT_CUTCOND_ARRAY CondOwd;		///< �J�b�g�����ԍ��i���H�j
    SRT_CUTCOND_ARRAY CondRet;		///< �J�b�g�����ԍ��i���H�j
}CUT_COND_STRUCT;
/// ------ �J�b�g���\����
typedef struct _tag_CUT_INFO_STRUCT{
    short srtMoveMode;				///< ���샂�[�h�i0:�g���~���O�A1:�e�B�[�`���O�A2:�����J�b�g�j
    short srtCutMode;				///< �J�b�g���[�h(0:�m�[�}���A1:���^�[���A2:���g���[�X�A3:�΂߁j
    double dblTarget;				///< �ڕW�l
    short srtSlope;					///< �X���[�v�ݒ�(1:�d������{�X���[�v�A2:�d������|�X���[�v�A4:��R����{�X���[�v�A5:��R����|�X���[�v�j
    short srtMeasType;				///< ����^�C�v(0:����(3��)�A1:�����x(2000��)�A2:�iIDX�̂݁j�O���@��A3:���薳���A5�`:�w��񐔑���j
    double dblAngle;				///< �J�b�g�p�x
    double dblLTP;					///< L�^�[���|�C���g
    short srtLTDIR;					///< L�^�[����̕���
    double dblRADI1;					///< R����]���a�iU�J�b�g�Ŏg�p�j
	// for Hook Or UCut
    double dblRADI2;				///< R2����]���a�iU�J�b�g�Ŏg�p�j
	short srtHkOrUType;				///< HookCut(3)��U�J�b�g�i3�ȊO�j�̎w��B
	// for Index
	short srtIdxScnCnt;				///< �C���f�b�N�X/�X�L�����J�b�g��(1�`32767)
	short srtIdxMeasMode;			///< �C���f�b�N�X���胂�[�h�i0:��R�A1:�d���A2:�O���j
	// for EdgeSense
	double dblEsPoint;				///< �G�b�W�Z���X�|�C���g
	double dblRdrJdgVal;			///< ���_�[��������ω���
	double dblMinJdgVal;			///< ���_�[�J�b�g��Œዖ�e�ω���
	short srtEsAftCutCnt;			///< ���_�[�ؔ�����̃J�b�g�񐔁i����񐔁j
	short srtMinOvrNgCnt;			///< ���_�[���o����A�Œ�ω��ʂ̘A��Over���e��
	short srtMinOvrNgMode;			///< �A��Over����NG�����i0:NG���薢���{, 1:NG������{�B���_�[���؂�, 2:NG���薢���{�B���_�[�؏グ�j
	// for Scan
	double dblStepPitch;			///< �X�e�b�v�ړ��s�b�`
	short  srtStepDir;				///< �X�e�b�v����
}CUT_INFO_STRUCT;
//---------------------------------------
/// ���[�U�v���O���������J�b�g�p�����[�^
//---------------------------------------
typedef struct _tag_CUT_COMMON_PRM{
    CUT_INFO_STRUCT CutInfo;
    CUT_COND_STRUCT CutCond;
}CUT_COMMON_PRM, *PCUT_COMMON_PRM;

//------------------------------------------------------------
// TKY�n�����J�b�g�p�����[�^���
//------------------------------------------------------------
/// �J�b�g�p�����[�^
typedef	union tag_PRM_CUTPARAM {
	struct tag_CUT_ST {
		WORD	mode;				///< ���샂�[�h  0:NOM, 1:���^�[��, 2:���g���[�X, 4:�΂�
		WORD	angle;				///< �΂߃J�b�g�p�x(0�`359deg)
		double	length;				///< �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
		double	RtOfs;				///< RT�I�t�Z�b�g(mm) //V8.0.0.11�@
	} cut_st;

	struct tag_CUT_L {
		WORD	mode;				///< ���샂�[�h  0:NOM, 1:���^�[��, 2:���g���[�X, 4:�΂�
		WORD	angle;				///< �΂߁F�J�b�g�p�x(0�`359deg)�A�m�[�}���F�����w��(1:��, 2:��, 3:��, 4:��)
		WORD	tdir;				///< L�^�[������  1:���v���(CW)�A2:�����v���(CCW)
		double	turn;				///< L�^�[���|�C���g 0.0�`100.0 (%)
		double	L1;					///< L1 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
		double	L2;					///< L2 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
		double	r;					///< �^�[���̉~�ʔ��a (mm)
		double	RtOfs;				///< RT�I�t�Z�b�g(mm) //V8.0.0.11�@
	} cut_l;

	struct tag_CUT_HOOK {
		WORD	mode;				///< ���샂�[�h  0:NOM, 1:���^�[��, 2:���g���[�X, 4:�΂�
		WORD	angle;				///< �΂߁F�J�b�g�p�x(0�`359deg)�A�m�[�}���F�����w��(1:��, 2:��, 3:��, 4:��)
		WORD	tdir;				///< L�^�[������  1:���v���(CW)�A2:�����v���(CCW)
		double	turn;				///< L�^�[���|�C���g 0.0�`100.0 (%)
		double	L1;					///< L1 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
		double	r1;					///< �^�[��1�̉~�ʔ��a (mm)
		double	L2;					///< L2 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
		double	r2;					///< �^�[��2�̉~�ʔ��a (mm)
		double	L3;					///< L3 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
        double  MinL2;              ///< L2 �ŏ��J�b�e�B���O��(0.00000�`20.0000(mm)) V8.0.0.14�A
	} cut_hook;

	struct tag_CUT_INDEX {
		WORD	angle;				///< �΂߁F�J�b�g�p�x(0�`359deg)�A�m�[�}���F�����w��(1:��, 2:��, 3:��, 4:��)
		WORD	maxindex;			///< �C���f�b�N�X��  1�`32767
		WORD	measMode;			///< ���胂�[�h 0:��R, 1:�d��, 3:�O��
		WORD	measType;			///< ����^�C�v 0:����, 1:�����x, 3:
		double	length;				///< �C���f�b�N�X��  0.0000�`20.0000 (mm)
		double	LimitLen;			///< IX�J�b�g�̃��~�b�g�� (mm) // V8.0.0.13�@
		double	CutLen;				///< �J�b�g�ς݂�IX�J�b�g��(mm) V8.2.0.5�@(INtrim�Ŏg�p)
	} cut_index;

	struct tag_CUT_SCAN {
		WORD	angle;				///< �΂߁F�J�b�g�p�x(0�`359deg)�A�m�[�}���F�����w��(1:��, 2:��, 3:��, 4:��)
		WORD	sdir;				///< �X�e�b�v����  1:+X, 2:-X, 3:+Y, 4:-Y
		WORD	lines;				///< �{��(1�`n)
		double	length;				///< �J�b�e�B���O��  0.0000�`20.0000 (mm)
		double	pitch;				///< �s�b�`  �@�@�@  0.0000�`20.0000 (mm)
	} cut_scan;

	struct tag_CUT_MARKING {
		//BYTE	str[16];			///< �`�敶����
		BYTE	str[18];			///< �`�敶����
		double	mag;				///< �{�� 0.01�`99.99�{
		WORD	dir;				///< �����̌���  0:0, 1:90, 2:180, 3:270
	} cut_marking;

	//struct tag_CUT_C {
	//	WORD	dir; 				///< �J�b�g���� 1:CW 2:CCW
	//	WORD	angle;				///< �J�n�p�x
	//	WORD	count;				///< ��
	//	double	st_r;				///< �J�n���a
	//	double	pitch;				///< �s�b�`
	//} cut_c;

	/// ES�J�b�g
	struct tag_CUT_EG {
		WORD	angle;				///< �΂߁F�J�b�g�p�x(0�`359deg)�A�m�[�}���F�����w��(1:��, 2:��, 3:��, 4:��)
		double	L1;					///< �J�b�g��  0.0000�`20.0000 (mm)
		double	ESpoint;			///< ���޾ݽ�߲�� 0.0�`-999.0 (%)
		double	ESwide;				///< ���޾ݽ�ω��� 0.0�`100.0 (%)
		double	ESwide2;			///< ���޾ݽ��ω��� 0.0�`100.0 (%)
		WORD	EScount;			///< ���޾ݽ��m�F��  0�`20
		WORD	CTcount;			///< ���޾ݽ��A��NG�m�F�񐔁@
		WORD	wJudgeNg;			///< NG���肷��/���Ȃ��i0:TRUE/1:FALSE�j
	} cut_eg;
	//// ES�J�b�g
	//struct tag_CUT_EG {
	//	WORD	dir;				///< �J�b�g����  1:+X, 2:-Y, 3:-X, 4:+Y
	//	double	L1;					///< �J�b�g��  0.0000�`20.0000 (mm)
	//	double	ESpoint;			///< ���޾ݽ�߲�� 0.0�`-999.0 (%)
	//	double	ESwide;				///< ���޾ݽ�ω��� 0.0�`100.0 (%)
	//	double	ESlen2;				///< ���޾ݽ��J�b�g��  0.0000�`20.0000 (mm)
	//} cut_eg;

	/// ����ES�J�b�g
	struct tag_CUT_EG_ORG {
		WORD	angle;				///< �΂߁F�J�b�g�p�x(0�`359deg)�A�m�[�}���F�����w��(1:��, 2:��, 3:��, 4:��)
		double	L1;					///< �J�b�g��  0.0000�`20.0000 (mm)
		double	ESpoint;			///< ���޾ݽ�߲�� 0.0�`-999.0 (%)
		double	ESwide;				///< ���޾ݽ�ω��� 0.0�`100.0 (%)
		double	ESlen2;				///< ���޾ݽ�㶯Ē� 0.0000�`20.0000 (mm)
	} cut_eg_org;

} PRM_CUT_PARAM, *PPRM_CUT_PARAM;

/// ���H�����ݒ�e�[�u���FL1�`L8�̉��H������ݒ肵���e�[�u���i1�`4�F�ʏ�J�b�g���A5�`8�F���^�[�����g���[�X���j
typedef struct tag_PRM_CUT_CONDITION {
	WORD	cutSetNo;					///< �iFL�����j���H�����ԍ�
	double	cutSpd;						///< �J�b�g�X�s�[�h
	double	cutQRate;					///< �J�b�gQ���[�g
} PRM_CUT_COND, *PPRM_CUT_COND;

/// �J�b�g�f�[�^
typedef	struct tag_PRM_CUT_DATA {
	WORD	wCutNo;						///< �J�b�g�ԍ� 1-9
	WORD	wDelayTime;					///< Delay time 0-32767 (msec) ��d������㑪��x������
	WORD	wCutType;					///< �J�b�g�`�� 1:st, 2:L, 3:HK, 4:IX
	WORD	wMoveMode;					///< 0:�g���~���O�J�b�g, 2:�����J�b�g(x5)
	WORD	wDoPosition;				///< �|�W�V���j���O����(0)/�Ȃ�(1)
	WORD	wCutAftPause;				///< �J�b�g��|�[�Y�^�C���i�����j(���[���a�Ή�) V8.0.0.16�A
	double	fCutStartX;					///< �J�b�g�X�^�[�g���W X  -80.0000�`+80.0000
	double	fCutStartY;					///< �J�b�g�X�^�[�g���W Y  -80.0000�`+80.0000
//	double	fCutSpeed;					///< �J�b�g�X�s�[�h 0.1�`409.0 (mm/s)
//	double	fQrate;						///< ���[�U�[Q�X�C�b�`���[�g 0.1�`50.0 (KHz)
	double	fCutOff;					///< �J�b�g�I�t %  -99.999 �` +999.999
	double	fAveDataRate;				///< �ؑփ|�C���g (�����ω��� % 0.0�`100.0%) @@@089
	BYTE	bUcutNo;					///< U�J�b�g �p�����[�^�w�莞�A�I�����ꂽ�e�[�u���ԍ���ۑ�
	double	fInitialVal;				///< U�J�b�g �J�b�g���Ƃ̏����l������ۑ�
	PRM_CUT_COND stCutCnd[MAX_CUT_CND];	///< ���H�����i�����ԍ��A�J�b�g�X�s�[�h�AQrate�j1�`8
	PRM_CUT_PARAM CutParam;				///< GUI�������CUT_PARAM�ݒ�Őݒ���s���B
} PRM_CUT_DATA, *PPRM_CUT_DATA;

/// ��R�f�[�^
typedef	struct	tag_PRM_RESISTOR {
	WORD	wResNo;						///< PR1  ��R�ԍ� 1-999(�g���~���O), 1000-9999(�}�[�L���O)
	WORD	wMeasMode;					///< ���胂�[�h�@0�F��R�A1�F�d���A�Q�F�O�� �� 2010/12/06 ��R�f�[�^�Ɉړ�����B
	WORD	wMeasType;					///< PR2  ���葪�� 0:�����A1:�����x
	WORD	wCircuit;					///< PR3  �T�[�L�b�g ��R��������T�[�L�b�g�ԍ�
	WORD	wHiProbNo;					///< PR4  H  High probe scanner channel
	WORD	wLoProbNo;					///<      L  Low
	WORD	w1stAG;						///<      G1 �A�N�e�B�u�K�[�h�ԍ�
	WORD	w2ndAG;						///<      G2 
	WORD	w3rdAG;						///<      G3
	WORD	w4thAG;						///<      G4
	WORD	w5thAG;						///<      G5
	DWORD	dwExBits;					///< External bits
	WORD	wPauseTime;					///< �|�[�Y�^�C��(dwExBits�o�͌�̃E�F�C�g) (msec)
	WORD	wRatioMode;					///< �g�������[�h  0:��Βl, 1:���V�I
	WORD	wBaseReg;					///< �x�[�X��RNo. ���V�I���̊��R�ԍ�
	double	fTargetVal;					///< �g���~���O�ڕW�l (ohm)
	WORD	wSlope;						///< �d���ω��X���[�v(PP2=1�̏ꍇ) 0:+�X���[�v, 1:-�X���[�v
	double	fITLimitH;					///< IT Limit H  (%) -99.99�`9999.99
	double	fITLimitL;					///< IT Limit L
	double	fFTLimitH;					///< FT Limit H  (%) -99.99�`9999.99
	double	fFTLimitL;					///< FT Limit L
	WORD	wCutCnt;					///< �J�b�g�� 1�`30 // @@@460 @@@350
	WORD	wCorrectFlg;				///< �J�b�g�ʒu�␳���s�t���O(0:�␳���Ȃ�, 1:����)
	double	fITOKLimitH;				///< (SL436K IT OK HIGT LIMMIT)
	double	fITOKLimitL;				///< (SL436K IT OK LOW LIMMIT)
	double	fCutOutRatio;				///< �؏�{��
	//----- V808�A��-----
	WORD intCvMeasNum;					///< CV �ő呪���
	WORD intCvMeasTime;					///< CV �ő呪�莞��(ms) 
	double dblCvValue;					///< CV CV�l         
	WORD intOverloadNum;				///< ���ް۰�� ��
	double dblOverloadMin;				///< ���ް۰�� �����l
	double dblOverloadMax;				///< ���ް۰�� ����l
	//----- V808�A��-----
	//----- V8.0.0.16�A�� -----
	WORD wPauseTimeFT;					///< FT�O�̃|�[�Y�^�C��(0-32767msec) ���[���a�Ή�
	WORD wInsideEndChkCount;			///< ���؂蔻���(0-255)
	double dblInsideEndChgRate;			///< ���؂蔻��ω���(0.00-100.00%)
	//----- V8.0.0.16�A�� -----
    //----- V8.0.0.14�A��(HILO����) -----
	WORD wProbHiSenseNo;				///< �n�C���v���[�u�ԍ�(�Z���X)
	WORD wProbLoSenseNo;				///< ���[���v���[�u�ԍ�(�Z���X)
#ifdef COMBINATION_SCANNER
	WORD CombHiAry[DEF_MAX_PROB];		///< �v���[�u�ԍ��g����(�n�C��) ���I����0 V8.2.0.6�@ ADD
	WORD CombLoAry[DEF_MAX_PROB];		///< �v���[�u�ԍ��g����(���[��)    �@�@   V8.2.0.6�@ ADD
#endif
    //----- V8.0.0.14�A��-----

	BOOL	bTrimEnd;					///< �g���~���O�����t���O
	int		lLastCutNo;					///< �ŏI�J�b�g�ԍ�
	PPRM_CUT_DATA	pCutData;			///< �J�b�g�f�[�^�|�C���^ 
} PRM_RESISTOR, *PPRM_RESISTOR;

/// �v���[�g�f�[�^
typedef	struct tag_TRIM_PLATE_DATA {
	WORD	wCircuitCnt;
	WORD	wResistCnt;
	WORD	wResCntInCrt;			///< �T�[�L�b�g����R��
	WORD	wDelayTrim;				///< delay trim(0:�Ȃ�,1:�ިڲ��ю��{�i�ʏ�),2:�ިڲ���2(�v���[�g�P�ʂ̃f�B���C)
	double	fBPOffsetX;
	double	fBPOffsetY;
	double	fAdjustOffsetX;
	double	fAdjustOffsetY;
	double	fNgCriterion;
	double	fZStepPos;				///< Z���ï��&��߰Ĉʒu
	double	fZTrimPos;				///< Z������Ĉʒu
	double	fReProbingX;			///< ����۰��ݸ�X�ړ���
	double	fReProbingY;			///< ����۰��ݸ�Y�ړ���
	WORD	wReProbingCnt;			///< ����۰��ݸމ�
	WORD	wInitialOK;				///< IT��FT�͈͎��̏���(0:�J�b�g���s 1:�J�b�g�����s))
	WORD	wNGMark;				///< NGϰ�ݸނ���/���Ȃ�
	WORD	w4Terminal;				///< ���g�p(4�[�q�������������/���Ȃ�)
	WORD	wLogMode;				///< ۷�ݸ�Ӱ��
									///< 0:���Ȃ�, 1:INITIAL TEST, 2:FINAL TEST, 3:INITIAL + FINAL)	
	//----- V808�A�� -----
	WORD intNgJudgeStop;			/// NG���莞��~(OVRERLOAD/CV�G���[�p)
	double dblReprobeVar;			/// ����۰��ݸނ΂����  
	double dblReprobePitch;			/// ����۰��ݸ��߯�
    double dblUdrStpUP;             /// �����v���[�u�X�e�b�v�㏸����      
    double dblUdrStpDown;           /// �����v���[�u�X�e�b�v���~����      
	//----- V808�A�� -----
	BOOL	bTrimCutEnd;			///< �J�b�g�I�t�ڕW�ő�l�ɓ��B������J�b�g���I������iTRUE�j/���Ȃ��iFALSE�j
} TRIM_PLATE_DATA, *PTRIM_PLATE_DATA;

/// GPIB�ݒ�p�f�[�^(�v���[�g�f�[�^)
typedef	struct tag_TRIM_PLATE_GPIB {
	WORD	wGPIBflg;				///< GPIB����̗L��(0:����,1:�L��)
	WORD	wGPIBdlm;				///< �����(0:CRLF, 1:CR, 2:LF,3:����)
	WORD	wGPIBtout;				///< �^�C���A�E�g�l(100ms�P��)
	WORD	wGPIBdev;				///< �@��A�h���X(0�`30)
	char	cGPIBinit[40];			///< �������R�}���h(�����)
	char	cGPIBtriger[10];		///< �g���K�R�}���h(�����)
	char	cGPIBreserv[6];			///< �\��
	WORD	wGPIBmode;				///< ���胂�[�h(0:��Βl���胂�[�h, 1:�΍��l���胂�[�h)
} TRIM_PLATE_GPIB, *PTRIM_PLATE_GPIB;

//----- @@@013�A�� -----
/// GPIB�ݒ�p�f�[�^(�v���[�g�f�[�^)
typedef	struct tag_TRIM_PLATE_GPIB2 {
	WORD	wGPIBflg;				///< GPIB����̗L��(0:����,1:�L��)
	WORD	wGPIBdlm;				///< �����(0:CRLF, 1:CR, 2:LF,3:����)
	WORD	wGPIBtout;				///< �^�C���A�E�g�l(100ms�P��)
	WORD	wGPIBdev;				///< �@��A�h���X(0�`30)
	WORD	wEOI;					///< EOI(0:�g�p���Ȃ�, 1:�g�p����)
	WORD	wPause1;				///< �ݒ�R�}���h1���M��|�[�Y����(1�`32767msec) @@@014
	WORD	wPause2;				///< �ݒ�R�}���h2���M��|�[�Y����(1�`32767msec) @@@014
	WORD	wPause3;				///< �ݒ�R�}���h3���M��|�[�Y����(1�`32767msec) @@@014
	WORD	wPauseT;				///< �g���K�R�}���h���M��|�[�Y����(1�`32767msec)@@@014
	WORD	wRsv;					///< �\��
	char	cGPIBinit[40];			///< �������R�}���h1(�����)
	char	cGPIBini2[40];			///< �������R�}���h2(�����)
	char	cGPIBini3[40];			///< �������R�}���h3(�����)
	char	cGPIBtriger[50];		///< �g���K�R�}���h(�����)
	char	cGPIBName[10];			///< �@�햼(�����)
	char	cRsv[8];				///< �\��
} TRIM_PLATE_GPIB2, *PTRIM_PLATE_GPIB2;
//----- @@@013�A�� -----

/// �J�b�g�ʒu�␳�f�[�^
typedef struct _tag_CUTPOS_CORRECT_DATA{
	double fCorrPosX[MAX_RESISTORS];	///< X���W�␳�|�W�V����
	double fCorrPosY[MAX_RESISTORS];	///< Y���W�␳�|�W�V����
	DWORD wCorrResult[MAX_RESISTORS];	///< �p�^�[���F������(0:�␳�Ȃ�, 1:OK, 2:NG�����)
} CUTPOS_CORRECT_DATA, *PCUTPOS_CORRECT_DATA;

/// �v���f�[�^
typedef struct _S_CMD_DAT {
	DWORD cmdNo;					///< �R�}���hNo
	double dbPara[MAX_PARAM_CNT];	///< double �^�p�����[�^
	DWORD dwPara[MAX_PARAM_CNT];	///< long	�^�p�����[�^
//	double	dbPara[16];				///< double �^�p�����[�^
//	DWORD	dwPara[16];				///< long	�^�p�����[�^
	DWORD flgTrim;				///< ���ݸ��׸�
} S_CMD_DAT;

typedef struct tag_TRIM_DAT {
	DWORD	cmdNo;
	WORD	type;					///< �f�[�^�^�C�v
	WORD	index_reg;				///< ��R�f�[�^�E�C���f�b�N�X
	WORD	index_cut;				///< �J�b�g�f�[�^�E�C���f�b�N�X
//	BYTE	dummy[2];				///< VB dummy data ...
	WORD	TkyKnd;					///< TKY/CHIP/NET���(0:TKY, 1:CHIP, 2:NET)
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
/// ��M����INTRIM�Ƃ�I/F�Ƀf�[�^�^���[���{�b�N�X���g�p���Ă��邽�߁A
/// �f�[�^�T�C�Y��128Byte�܂ł̐����B
typedef	struct _S_RES_DAT {
	DWORD	wTxSize;				///< �]���T�C�Y
	DWORD	status;					///< 0:���� / -1:�s����
//	DWORD	errno;					///< errno�͗\���̈�trimfunc�ŔF������Ȃ��B??? 
	DWORD	dwerrno;				///< trimfunc�ɍ��킹 errno �� dwerrno �ɕύX����B
//	DWORD	signal[3];				///< ��Signal��� XYZ
	DWORD	signal[5];				///< ��Signal��� XYZ
	//DWORD	in_dat[INP_MAX];		///< I/O���͏��
	//								///<	[0]:�R���\�[��SW�Z���X
	//								///<	[1]:�C���^�[���b�N�֌WSW�Z���X
	//								///<	[2]:�I�[�g���[�_LO
	//								///<	[3]:�I�[�g���[�_HI
	//								///<	[4]:�Œ�A�b�e�l�[�^
	//DWORD	outdat[OUT_MAX];		///< I/O�o�͏��
	//								///<	[0]:�R���\�[������
	//								///<	[1]:�T�[�{�p���[
	//								///<	[2]:�I�[�g���[�_LO
	//								///<	[3]:�I�[�g���[�_HI
	//								///<	[4]:�V�O�i���^���[
	DWORD	wData[4];				///< �ߒl(2byte�����^)
	double	stgPos[5];				///< �X�e�[�W�n���݈ʒu X,Y,Z,T,Z2
	double	bpPos[2];				///< BP���݈ʒu 
	double	fData;
} S_RES_DAT;

typedef struct _S_RATIO2EXP {
	DWORD	cmdNo;						///< �R�}���hNo
	DWORD	rno;						///< ��R�ԍ�
	char	strExp[MAX_NUMCALLENGTH];	///< �v�Z��������
} S_RATIO2EXP, *PS_RATIO2EXP;


/// UCUT �p�����[�^�i1�v�f�j
typedef struct tag_UCUT_PARAM_EL {	
	double	ratio;						///< �ڕW�l�ɑ΂��鏉���l�̍�(%)
	double	ltp;						///< L�^�[���|�C���g 0.0�`100.0 (%)
	double	ltp2;						///< L�^�[���|�C���g2 0.0�`100.0 (%)
	double	L1;							///< L1 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
	double	L2;							///< L2 �ő�J�b�e�B���O��  0.0000�`20.0000 (mm)
	double	r;							///< �~�ʔ��a (mm)
	double	v;							///< ���x
	double	v2;							///< ���x2(���^�[��/���g���[�X�p) V8.0.0.14�B
	double	nom;						///< �ڕW�l
	BOOL	flg;						///< �f�[�^�L��
} UCUT_PARAM_EL, *PUCUT_PARAM_EL;

/// UCUT�p�����[�^�e�[�u���i1��R���j
typedef struct tag_UCUT_PARAM {		
	short			rno;				///< ��R�ԍ�
	UCUT_PARAM_EL	el[20];				///< 20�f�[�^�܂ŕێ�
} UCUT_PARAM, *PUCUT_PARAM;

/// ���g���[�XUCUT �p�����[�^�i1�v�f�j
typedef struct tag_UCUT4_PARAM_EL {
	double	dblRatio;					///< �ڕW�l�ɑ΂��鏉���l�̍�(%)
	double	dblLtp;						///< L�^�[���|�C���g 0.0�`100.0 (%)
	double	dblLtp2;					///< L�^�[��B�|�C���g 0.0�`100.0 (%)
	BOOL	blnFlg;						///< true:�f�[�^�L��,false:�e�[�u���I��
} UCUT4_PARAM_EL, *PUCUT4_PARAM_EL;

/// ���g���[�XUCUT�p�����[�^�e�[�u���i1��R���j
typedef struct tag_UCUT4_PARAM {
	short			sRno;											///< ��R�ԍ�
	UCUT4_PARAM_EL	UCUT4_PARAM_EL_st[DEF_MAX_REG_ITEM_PARAMCNT];	///< 50�f�[�^�܂ŕێ�
} UCUT4_PARAM, *PUCUT4_PARAM;

/// �������V�I���f�[�^
typedef	struct	tag_RATIO3_DATA
{
	int			iGn;					///< ��ٰ�ߔԍ�(3�`9) ��0�ͽį�߰
	int			iBaseRn[2];				///< �ް���R�ԍ�1,2(����݂̂̒�R�ԍ�)
	int			iRn[2];					///< ��R�ԍ�1,2
	double		fNom[2];				///< �䗦1,2 
	double		fTargetVal[2];			///< �ڕW�l1,2(��) ���ް���R�̎����l * �䗦
} RATIO3_DATA;

typedef struct tag_LargeBuf {
//	WORD	wTxSize;
	double wTxSize;
	union {
		WORD	wd[256];
		double	dd[256];
	} u;
} LARGEBUF, *PLARGEBUF;

/// GPIB�ݒ�p�f�[�^
typedef	struct	tag_GPIB_DATA {
	WORD		wGPIBflg;				///< GPIB����̗L��(0:����,1:�L��)
	WORD		wInitFlg;				///< �����ݒ�t���O(0:�����ݒ薢, 1:�����ݒ�ς�)
	WORD		wGPIBdlm;				///< �f���~�^(0:CRLF, 1:CR, 2:LF,3:����)
    short		wAdex;					///< �f�o�C�X�ԍ� = 21(0-30)
    short		wTimeout;				///< GPIB Timeout�l(100ms)  
	double		dbDivVal;				///< ����l�����ɕϊ�����ׂ̒l 
    char		cInit[64];				///< �������R�}���h������
    char		cTriger[16];			///< �g���K�[������
	WORD		wGPIBmode;				///< ���胂�[�h(0:��Βl���胂�[�h, 1:�΍��l���胂�[�h)
	double		dbNOM;					///< �W����R�l(�P�ʃ�) ���΍��l���胂�[�h���ݒ�
	int			iRange;					///< ���背���W(0:R1�`6:R6)
} GPIB_DATA, *PGPIB_DATA;
//========================================================================
#pragma pack()

//------------------------------------------------------------------------------
//		�����֐���`
//      (��)VB����͉��L�̂悤�ɒ�`������B(Alias����.map���Q��)
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
//(�V�KIF)���d���Ή���
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
// �J�b�g�nI/F
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

//--- ���[�U���J������(2010/12/14)
//DLLTRIMFUNC_API long __stdcall TRIM_SC(PCUT_COMMON_PRM pCutCmn);
DLLTRIMFUNC_API long __stdcall TRIM_IX(PCUT_COMMON_PRM pCutCmn);	
//---�@�K�v�Ɣ��f�������_��DllTrimFunc��IF��ǉ�����B
//------------------------------------------

//*********************************************************************
//	2010/01/26 ���[�U�����I�v�V����IF
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
//	2010/01/26 �������J����IF�i�G���[�`�F�b�N�������j
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
DLLTRIMFUNC_API long __stdcall TRIMDATA_GPIB2(PTRIM_PLATE_GPIB2 pGpibDat, int tkyKnd); // @@@013�A

DLLTRIMFUNC_API long __stdcall TRIMDATA_RESISTOR(PPRM_RESISTOR pResistorDat, int resNo); 
DLLTRIMFUNC_API long __stdcall TRIMDATA_CUTDATA(PPRM_CUT_DATA pCutDat, int resNo, int cutNo);
DLLTRIMFUNC_API long __stdcall TRIMDATA_CUTPRM(PPRM_CUT_PARAM pCutPrm, int resNo, int cutNo); 

DLLTRIMFUNC_API long __stdcall TRIMEND();                             
DLLTRIMFUNC_API long __stdcall TSTEP(short BNX, short BNY, double stepOffX, double stepOffY);                          
DLLTRIMFUNC_API long __stdcall UCUT_RESULT(short RNO, short CNO, short *UcutNo, double *InitVal);                      
DLLTRIMFUNC_API long __stdcall UCUT_RESULT2(short RNO, short CNO, short *UcutNo, double *InitVal, short *Result); // V8.0.0.14�D
DLLTRIMFUNC_API long __stdcall UCUT4RESULT(short* sRegNo_p, short* sCutNo_p);                          
DLLTRIMFUNC_API long __stdcall VCTRIM(short SLP, double NOM, short MD, double X, double Y, double VX, double VY, double LIMX, double LIMY);            
DLLTRIMFUNC_API long __stdcall VRangeCorrect(short Idx, double Val, short Flg, short RMin, short RMax);                    
DLLTRIMFUNC_API long __stdcall VUTRIM4(short SLP, double NOM, short MD, double LTP, short LTDIR, double L1, double L2, double RADI, double V, short ANG, short TRMD, double TRL, short CN, short DT, short MODE);
DLLTRIMFUNC_API long __stdcall Z_INIT();       ///< �����ł�AXIS_Z_INIT���R�[���B���ʌ݊��ׂ̈�IF�Ƃ��Ă͎c���B                      
DLLTRIMFUNC_API long __stdcall ZABSVACCUME(long ZON);                            
DLLTRIMFUNC_API long __stdcall ZATLDGET(long* LDIN);                            
DLLTRIMFUNC_API long __stdcall ZATLDSET(long LDON, long LDOFF);
DLLTRIMFUNC_API long __stdcall ZATLDRED(long FAR* LDOUT); // @@@009
DLLTRIMFUNC_API long __stdcall ZBPLOGICALCOORD(long dwCOORD);                            
DLLTRIMFUNC_API long __stdcall ZGETDCVRANG(double* VMAX);                            
DLLTRIMFUNC_API long __stdcall ZGETPHPOS(double* NOWXP, double* NOWYP);
DLLTRIMFUNC_API long __stdcall ZGETPHPOS2(double* NOWXP, double* NOWYP); // V808�B               
DLLTRIMFUNC_API long __stdcall ZINPSTS(long SW, long* STS);                          
DLLTRIMFUNC_API long __stdcall ZLATCHOFF();                             
DLLTRIMFUNC_API long __stdcall ZMOVE(double POS, short MD);                          
DLLTRIMFUNC_API long __stdcall ZMOVE2(double POS, short MD);   
DLLTRIMFUNC_API long __stdcall ZZMOVE(double POS, short MD);  // V3.0.0.0�A                          
DLLTRIMFUNC_API long __stdcall ZZMOVE2(double POS, short MD); // V3.0.0.0�A               
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
DLLTRIMFUNC_API long __stdcall ZSYSPARAM4(double *pdbPRM, long *pdwPRM); // V808�@
DLLTRIMFUNC_API long __stdcall MIDIUMCUTPARAM(double *pdbPRM, long *pdwPRM);	// V8.0.0.16�@
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

// SL43xR�����V�K�ǉ�
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
DLLTRIMFUNC_API long __stdcall GET_Z_POS2(double *Z); // V809�B
DLLTRIMFUNC_API long __stdcall CONSOLE_SWCHECK(BOOL bReleaseCheck, long* SwitchChk);
DLLTRIMFUNC_API long __stdcall Z_SWCHECK(long* SwitchChk);
DLLTRIMFUNC_API long __stdcall CLAMP_CTRL(long XOnOff, long YOnOff);
DLLTRIMFUNC_API long __stdcall CLAMP_GETSTS(long *XOnOff, long *YOnOff); 
DLLTRIMFUNC_API long __stdcall BP_GET_CALIBDATA(double *GainX, double *GainY, double *OfsX, double *OfsY);
DLLTRIMFUNC_API long __stdcall PMON_SHUTCTRL(long onOff); 
DLLTRIMFUNC_API long __stdcall PMON_MEASCTRL(long measMode); 


// �f�o�b�O/�]���p�֐�
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
DLLTRIMFUNC_API long __stdcall SETAXISPRM2(DWORD AXIS, DWORD FL, DWORD FH, DWORD DRVRAT, DWORD MAGNIF); // V8.0.0.16�B

// 2013.02.25 add Yamazaki 
// �����R�p�ɒǉ�		
DLLTRIMFUNC_API long __stdcall MES_LOW_RESIST(long measMode,long curmode,long cnt,long time,long CurSel,double *res,long *WDres);
DLLTRIMFUNC_API long __stdcall PARAM_LOW_RESIST(long BoardSel,long modeSel,long Gain,long RevTime,long ResTime,long mode,long MesCnt,long MesTime,long CurSel);

DLLTRIMFUNC_API long __stdcall MES_LOW_RESULT(long measMode,void far* pRes);
DLLTRIMFUNC_API long __stdcall TRIMBLOCKMEASURE(short MD, short HZ, short RI, short CI, short NG);    // @@@015
//----- V809�@�� -----
DLLTRIMFUNC_API long __stdcall LASERONESHOT(int dotcnt);
DLLTRIMFUNC_API long __stdcall TRIM_ST1(short mMode, short cMode, double Nom, short Slp, double Ang, double L1, double spdo, double spdr, double Qrateo ,double Qrater, short iCondo, short iCondr);
DLLTRIMFUNC_API long __stdcall TRIM_L1(short mMode, short cMode, double Nom, short Slp, double Ang, double Ltp, short LtDir, short MType, double L1,double L2, double spdo1, double spdo2, double Qrateo1,double Qrateo2, short iCondo1,short iCondo2);
DLLTRIMFUNC_API long __stdcall TRIM_IX1(short mMode, double Nom, short Slp, double Ang, short IScCnt , short IMeasMode, short MType, double L1, double spdo1, double Qrateo1, short iCondo1);
//DLLTRIMFUNC_API long __stdcall TRIM_H1(short mMode, short cMode, double Nom, short Slp, double Ang, double Ltp, short LtDir, short MType, double Radi1, double Radi2, short HKOrUType);
DLLTRIMFUNC_API long __stdcall TRIM_H1(short mMode, short cMode, double Nom, short Slp, double Ang, double Ltp, short LtDir, short MType, double Radi1, double Radi2, short HKOrUType ,double L1,double L2,double L3, double spdo1, double spdo2,double spdo3, double Qrateo1,double Qrateo2,double Qrateo3, short iCondo1,short iCondo2,short iCondo3);
//----- V809�@�� -----
DLLTRIMFUNC_API long __stdcall TRIM_HkU(PCUT_COMMON_PRM pCutCmn);


DLLTRIMFUNC_API long __stdcall SetFixAttInfo(long *pFixAttAry); // V809�A
DLLTRIMFUNC_API long __stdcall SetFixAttOneInfo(int CondNum, int FixAtt); // V809�A
DLLTRIMFUNC_API long __stdcall SET_FIX_ATT_INFO(long *pFixAttAry);				// V8.0.0.10�B
DLLTRIMFUNC_API long __stdcall SET_FIX_ATTONE_INFO(int CondNum, int FixAtt);	// V8.0.0.10�B
DLLTRIMFUNC_API long __stdcall SETZOFFPOS(double Pos); // V8.0.0.10�A

//********************************************

