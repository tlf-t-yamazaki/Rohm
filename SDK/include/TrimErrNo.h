/***************************************************************************//**
 * @file        TrimErrNo.h
 * @brief       �G���[�ԍ���`�t�@�C��
 *
 * @par     (C) Copyright OMRONLASERFRONT INC. 2010-2012 All Rights Reserved
 ******************************************************************************/
#ifndef	__TRIM_ERR_NO_H__
#define	__TRIM_ERR_NO_H__

//========================================================================
// Error No.
//========================================================================
//-----( �G���[�X�e�[�^�X )-----
//	LastError�ɐݒ肵�Adwerrno��Windows���֕ԐM
#define	ERR_CLEAR					0			///< ���~�b�g��ԃN���A�i���~�b�g�G���[�Ȃǁj
#define ERR_ALREADY_SET				1
//#define ERR_TERMINATE				2			///< �z�X�g��蒆�f�I��
//#define ERR_RESET					3			///< ���Z�b�g�I��
//#define ERR_CONTACT				5			///< �R���^�N�g�G���[

// �\�t�g�E�F�A�G���[
//#define	ERR_EMGSWCH				102			///< ����~
//#define	ERR_SRV_ALM				103			///< �T�[�{�A���[��
//#define	ERR_AXS_LIM				104			///< XYZ�����~�b�g���o

// �X�e�[�W�G���[
// �^�C���A�E�g�G���[�ɂ��ẮA�x�[�X+�Ώ̎��r�b�g(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
// �̃G���[�R�[�h�ŕԂ��B
#if DOXYGEN_SHOULD_SKIP_THIS
/*#define ERR_TIMEOUT_BASE			100		*/	/**< �^�C���A�E�g�G���[(�x�[�X�ԍ�)
												 * - X���^�C���A�E�g=101
												 * - Y���^�C���A�E�g=102
												 * - Z���^�C���A�E�g=104
												 * - Theta���^�C���A�E�g=108
												 * - Z2���^�C���A�E�g=116
												 * - ��������or�̒l
												 * - X,Y���^�C���A�E�g=103 */
#endif /* DOXYGEN_SHOULD_SKIP_THIS */
//#define	ERR_TIMEOUT_X				101			///< X���^�C���A�E�g
//#define	ERR_TIMEOUT_Y				102			///< Y���^�C���A�E�g
//#define	ERR_TIMEOUT_Z				103			///< Z���^�C���A�E�g
//#define	ERR_TIMEOUT_T				104			///< �Ǝ��^�C���A�E�g
/*///#define	ERR_SOFTLIMIT_X				105			///< X���\�t�g���~�b�g */
/*///#define	ERR_SOFTLIMIT_Y				106			///< Y���\�t�g���~�b�g */
/*///#define	ERR_SOFTLIMIT_Z				107			///< Z���\�t�g���~�b�g */
#define	ERR_TIMEOUT_ATT				108			///< ATT�^�C���A�E�g 
//#define	ERR_TIMEOUT_Z2				109			///< Z2���^�C���A�E�g
//#define	ERR_BP_XLIMIT				110			///< BP X�����͈̓I�[�o�[�i���g�p�j
//#define	ERR_BP_YLIMIT				111			///< BP Y�����͈̓I�[�o�[(���g�p�j
//#define ERR_BP_MOVE_TIMEOUT			112			///< BP �^�C���A�E�g

// �n�[�h�E�F�A�G���[
#define	ERR_EMGSWCH					201			///< ����~
#define	ERR_SRV_ALM					202			///< �T�[�{�A���[��
//#define	ERR_AXS_LIM				203			///< XYZ�����~�b�g���o �i���ۂɂ͖��g�p�j
#define	ERR_AXS_LIM_X				203			///< X�����~�b�g���o 
#define	ERR_AXS_LIM_Y				204			///< Y�����~�b�g���o 
#define	ERR_AXS_LIM_Z				205			///< Z�����~�b�g���o 
#define	ERR_AXS_LIM_T				206			///< �Ǝ����~�b�g���o
#define	ERR_AXS_LIM_ATT				207			///< ATT�����~�b�g���o
#define	ERR_AXS_LIM_Z2				208			///< Z2�����~�b�g���o
#define	ERR_OPN_CVR					209			///< ➑̃J�o�[�J���o
#define	ERR_OPN_SCVR				210			///< �X���C�h�J�o�[�J���o
#define	ERR_OPN_CVRLTC				211			///< �J�o�[�J���b�`���o
#define ERR_INTERLOCK_STSCHG		212			///< �C���^�[���b�N��Ԃ̕ύX���o
#define ERR_CLAMPPRM_UNMATCH		213			///< (SL436�n�̂�)�N�����vON-OFF�p�����[�^�s��v�B(X=0,Y=1��X=1,Y=0�Ɛݒ肳��Ă���)
#define	ERR_AXS_LIM					220			///< �e���̃��~�b�g���o
#define ERR_HARD_FAILURE			250			///< �n�[�h�E�F�A�̒v���I�̏�


#define ERR_PROBE_UP_ERROR			251			///< �v���[�u�㏸�G���[		//V4.21.0.0�B
#define ERR_PROBE_DOWN_ERROR		252			///< �v���[�u���~�G���[		//V4.21.0.0�B
#define	ERR_AXS_PLUS_LIM_ATT		253			///< ATT���{���~�b�g���o	//V4.21.0.0�C
#define	ERR_AXS_MINUS_LIM_ATT		254			///< ATT���|���~�b�g���o	//V4.21.0.0�C
#define ERR_CMD_NOTSPT				301			///< ���T�|�[�g�R�}���h
#define ERR_CMD_PRM					302			///< �p�����[�^�G���[
#define ERR_CMD_LIM_L				303			///< �p�����[�^�����l�G���[
#define ERR_CMD_LIM_U				304			///< �p�����[�^����l�G���[
#define ERR_RT2WIN_SEND				305			///< INTime��Windows���M�G���[
#define ERR_RT2WIN_RECV				306			///< INTime��Windows��M�G���[
#define ERR_WIN2RT_SEND				307			///< Windows��INTime���M�G���[
#define ERR_WIN2RT_RECV				308			///< Windows��INTime��M�G���[
#define ERR_SYS_BADPOINTER			309			///< �s���|�C���^
#define ERR_SYS_FREE_MEMORY			310			///< �������̈�̊J���G���[
#define ERR_SYS_ALLOC_MEMORY		311			///< �������̈�̊m�ۃG���[
#define ERR_CALC_OVERFLOW			320			///< �I�[�o�[�t���[
#define ERR_INTIME_NOTMOVE			350			///< INTRIM���N�����Ă��Ȃ�

// �n�[�h�E�F�AIO�n�G���[�R�[�h�F400�ԑ�
#define ERR_CSLLAMP_SETNO			401			///< �R���\�[���n�����v�ԍ��w��G���[
#define ERR_SIGTWRLAMP_SETNO		402			///< �V�O�i���^���[�����v�ԍ��w��G���[
#define ERR_SIGTWRLAMP_SETMODENO	403			///< �V�O�i���^���[�����v�_��/�_�Ń��[�h�w��G���[
#define ERR_BIT_ONOFF				404			///< �r�b�g�I��/�I�t�w��G���[
#define ERR_SETPRM_WAITSWNON		405			///< ���͑҂��ΏۃX�C�b�`�̎w�肪�S�ĂȂ��ɂȂ��Ă���B
#define ERR_START_SOFT_KEY			406			///< �n�[�hSTART�L�[�łȂ����ɌĂ�ł͂����Ȃ��������Ă񂾁B//V4.5.0.0�@

// ����n�G���[�R�[�h�F500�ԑ�
#define ERR_MEAS_RANGESET_TYPE		501			///< ���背���W�ݒ�G���[�F�w�背���W�ݒ�^�C�v�Ȃ�
#define ERR_MEAS_SETRNG_NO			502			///< ���背���W�ݒ�G���[�F�Ώۃ����W�Ȃ�
#define ERR_MEAS_SETRNG_LO			503			///< ���背���W�ݒ�G���[�F�ŏ������W�ȉ�
#define ERR_MEAS_SETRNG_HI			504			///< ���背���W�ݒ�G���[�F�ő僌���W�ȏ�
#define ERR_MEAS_RNG_NOTSET			505			///< ���背���W�ݒ�G���[�F�����W���ݒ�
#define ERR_MEAS_FAIL				506			///< ����G���[
#define ERR_MEAS_FAST_R				507			///< �����x��R����G���[
#define ERR_MEAS_HIGHPRECI_R		508			///< �����x��R����G���[
#define ERR_MEAS_FAST_V				509			///< �����x�d������G���[
#define ERR_MEAS_HIGHPRECI_V		510			///< �����x�d������G���[
#define ERR_MEAS_TARGET				511			///< ����ڕW�l�ݒ�G���[
#define ERR_MEAS_TARGET_LO			512			///< ����ڕW�l�ݒ�G���[�F�ŏ��ڕW�l�ȉ�
#define ERR_MEAS_TARGET_HI			513			///< ����ڕW�l�ݒ�G���[�F�ő�ڕW�l�ȉ�
#define ERR_MEAS_SCANNER			514			///< ����X�L���i�ݒ�G���[�F�s���X�L���i�ԍ�
#define ERR_MEAS_SCANNER_LO			515			///< ����X�L���i�ݒ�G���[�F�ŏ��X�L���i�ԍ��ȉ�
#define ERR_MEAS_SCANNER_HI			516			///< ����X�L���i�ݒ�G���[�F�ő�X�L���i�ԍ��ȏ�
#define ERR_MEAS_RNG_SHORT			517			///< ����l�F�����W�V���[�g�i0.01�ȉ��j
#define ERR_MEAS_RNG_OVER			518			///< ����l�F�����W�I�[�o�[�i67000000.0�ȏ�j
#define ERR_MEAS_SPAN				519			///< ����͈͊O
#define ERR_MEAS_SPAN_SHORT			520			///< ����͈͊O�F�V���[�g-��R(0x6666)/�d��(0x3333)�ȉ�
#define ERR_MEAS_SPAN_OVER			521			///< ����͈͊O�F�I�[�o�[-��R(0xCCCC)/�d��(0x6666)�ȏ�
#define ERR_MEAS_INVALID_SLOPE		522			///< �X���[�v�ݒ�G���[
#define ERR_MEAS_COUNT				523			///< ����񐔎w��G���[
#define ERR_MEAS_SETMODE			524			///< ���莞�̐ݒ胂�[�h�iMfset���[�h�j�G���[
#define ERR_MEAS_AUTORNG_OVER		525			///< �I�[�g�����W����:��d������͈̓I�[�o�[�i���d���̈�j
#define ERR_MEAS_K2VAL_SHORT		526			///< ���d������:K2�ݒ�l0.4����
#define ERR_MEAS_K2VAL_OVER			527			///< ���d������:K2�ݒ�l0.8�ȏ�
#define ERR_MEAS_SCANSET_TIMEOUT	528			///< �X�L���i�ݒ芮���^�C���A�E�g
#define ERR_MEAS_RANGESET_TIMEOUT	529			///< �����W�ݒ�񐔂��I�[�o�����ꍇ�̃^�C���A�E�g @@@009
#define ERR_MEAS_CV 				530			///< ����΂�����o @@@80010
#define ERR_MEAS_OVERLOAD			531			///< �I�[�o���[�h���o @@@80010
#define ERR_MEAS_REPROBING			532			///< �ăv���[�r���O�G���[ @@@80010

// BP�n�G���[�R�[�h�F600�ԑ�
#define ERR_BP_MAXLINEANUM_LO		601			///< ���j�A���e�B�␳�F�ŏ��ԍ��ȉ�
#define ERR_BP_MAXLINEANUM_HI		602			///< ���j�A���e�B�␳�F�ő�ԍ��ȏ�
#define ERR_BP_LOGICALNUM_LO		603			///< �ی��ݒ�F�ŏ��ی��l�ԍ��ȉ�
#define ERR_BP_LOGICALNUM_HI		604			///< �ی��ݒ�F�ő�ی��l�ԍ��ȉ�
#define ERR_BP_LIMITOVER			605			///< BP�ړ������ݒ�F���~�b�g�I�[�o�[
#define ERR_BP_HARD_LIMITOVER_LO	606			///< BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ŏ����͈͈ȉ��j
#define ERR_BP_HARD_LIMITOVER_HI	607			///< BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ő���͈͈ȏ�j
#define ERR_BP_SOFT_LIMITOVER		608			///< �\�t�g���͈̓I�[�o�[
#define ERR_BP_BSIZE_OVER			609			///< �u���b�N�T�C�Y�ݒ�I�[�o�[�i�\�t�g���͈̓I�[�o�[�j
#define	ERR_BP_SIZESET				610			///< BP�T�C�Y�ݒ�G���[
#define ERR_BP_MOVE_TIMEOUT			611			///< BP �^�C���A�E�g
#define ERR_BP_GRV_ALARM_X			620			///< �K���o�m�A���[��X
#define ERR_BP_GRV_ALARM_Y			621			///< �K���o�m�A���[��Y
#define ERR_BP_GRVMOVE_HARDERR		622			///< �|�W�V���j���O���쎞�̃K���o�m����ُ�i�w�ߒl�ƌ��ݍ��W���s��v�j
#define ERR_BP_GRVSET_AXIS			630			///< �|�W�V���j���O���쎞�̍��W�ݒ�or���W�擾�G���[
#define ERR_BP_GRVSET_MOVEMODE		631			///< �K���o�m���[�h�ݒ莞�̓��샂�[�h�ݒ�l�s���i�|�W�V���j���O=0�A�g���~���O=1�ȊO�̒l���ݒ肳��Ă���j
#define ERR_BP_GRVSET_SPEEDSHORT	632			///< �K���o�m���x�w��F�ŏ��X�s�[�h����
#define ERR_BP_GRVSET_SPEEDOVER		633			///< �K���o�m���x�w��F�ő�X�s�[�h�I�[�o�[
#define ERR_BP_GRVSET_MISMATCH		634			///< �|�W�V���j���O���쎞�̃K���o�m����ُ�i�ݒ�l�ɑ΂��ݒ肳�ꂽ�l(�n�[�h����Ǐo�����l�j���s��v�j

// �g���~���O/�J�b�g�n�G���[�R�[�h:700�ԑ�
#define ERR_CUT_NOT_SUPPORT			701			///< ���T�|�[�g�J�b�g�`��
#define ERR_CUT_PARAM_LEN			702			///< �J�b�g�p�����[�^�G���[�F�J�b�g���ݒ�G���[
#define ERR_CUT_PARAM_LEN_LO		703			///< �J�b�g�p�����[�^�G���[�F�J�b�g���ŏ��l�ȉ�
#define ERR_CUT_PARAM_LEN_HI		704			///< �J�b�g�p�����[�^�G���[�F�J�b�g���ő�l�ȏ�
#define ERR_CUT_PARAM_CORR			706			///< �J�b�g�p�����[�^�G���[�F�p�����[�^���փG���[
#define ERR_CUT_PARAM_SPD			707			///< �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�G���[
#define ERR_CUT_PARAM_SPD_LO		708			///< �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�I�[�o�[
#define ERR_CUT_PARAM_SPD_HI		709			///< �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�V���[�g
#define ERR_CUT_PARAM_DIR			710			///< �J�b�g�p�����[�^�G���[�F�J�b�g����
#define ERR_CUT_PARAM_CUTCNT		711			///< �J�b�g�p�����[�^�G���[�F�J�b�g�{��
#define ERR_CUT_PARAM_CHRSIZE		712			///< �J�b�g�p�����[�^�G���[�F�����T�C�Y�w��G���[
#define ERR_CUT_PARAM_CUTANGLE		713			///< �J�b�g�p�����[�^�G���[�F�p�x�w��G���[
#define ERR_CUT_PARAM_CHARSET		714			///< �J�b�g�p�����[�^�G���[�F������w��G���[
#define ERR_CUT_PARAM_STRLEN		715			///< �J�b�g�p�����[�^�G���[�F�}�[�L���O�����񒷎w��G���[
#define ERR_CUT_PARAM_NOHAR			716			///< �J�b�g�p�����[�^�G���[�F�w�蕶���Ȃ�
#define ERR_CUT_PARAM_NORES			717			///< �J�b�g�p�����[�^�G���[�F��R�ԍ��s��
#define ERR_CUT_PARAM_CUTMODE		718			///< �J�b�g�p�����[�^�G���[�F�J�b�g���[�h�s���iCUT_MODE_NORMAL(1)�`CUT_MODE_NANAME(4)�ȊO�w��j
#define ERR_CUT_PARAM_PARCENT		719			///< �J�b�g�p�����[�^�G���[�F�|�C���g�ݒ�
#define ERR_CUT_PARAM_BP			720
#define ERR_L1_LENGTH_LOW			721			///< �k�P�J�b�g�������l���B�G���[ //V8.2.0.6�B
#define ERR_L2_LENGTH_LOW			722			///< �k�Q�J�b�g�������l���B�G���[ //V8.2.0.6�B
#define ERR_L3_LENGTH_LOW			723			///< �k�R�J�b�g�������l���B�G���[ //V8.2.0.6�B

#define ERR_CUT_RATIOPRM_BASERNO	730			///< ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R�ԍ��w��G���[
#define ERR_CUT_RATIOPRM_BASER_NG	731			///< ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R����NG
#define ERR_CUT_RATIOPRM_MODENOT2	732			///< ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R��Ratio���[�h2�łȂ�
#define ERR_CUT_RATIOPRM_CALCFORM	733			///< ���V�I�p�����[�^�G���[�F�v�Z�p�����s��
#define ERR_CUT_RATIOPRM_CANTSET	734			///< ���V�I�p�����[�^�G���[�F�f�[�^�̓o�^���o���Ȃ�
#define ERR_CUT_RATIOPRM_GRPNO		735			///< ���V�I�p�����[�^�G���[�F�O���[�v�ԍ��w��G���[

#define ERR_CUT_UCUTPRM_NOPRMRES	740			///< U�J�b�g�p�����[�^�G���[�F��R���p�����[�^�G���A�̐ݒ�Ȃ�
#define ERR_CUT_UCUTPRM_NOPRMCUT	741			///< U�J�b�g�p�����[�^�G���[�F�J�b�g���p�����[�^�G���A�̐ݒ�Ȃ�
#define ERR_CUT_UCUTPRM_NORES		742			///< U�J�b�g�p�����[�^�G���[�F�Ώے�R�Ȃ�
#define ERR_CUT_UCUTPRM_NODATA		743			///< U�J�b�g�p�����[�^�G���[�F�p�����[�^�f�[�^���ݒ肳��Ă��Ȃ�
#define ERR_CUT_UCUTPRM_NOMODE		744			///< U�J�b�g�p�����[�^�G���[�F�w�胂�[�h�Ȃ�
#define ERR_CUT_UCUTPRM_TBLIDX		745			///< U�J�b�g�p�����[�^�G���[�F�e�[�u���̃C���f�b�N�X���s��
#define ERR_CUT_UCUTPRM_L1L2LEN		746			///< U�J�b�g�p�����[�^�G���[�FL1�y��L2�̋�����0.1mm�ȉ��ɂȂ���(�������g���[�X�L��̏ꍇ)  //V4.3.0.0�@
#define ERR_CUT_UCUTPRM_L2		    747			///< U�J�b�g�p�����[�^�G���[�FL2�̋�����(R1 *2)�ȉ��ƂȂ��� //V4.3.0.0�@

#define ERR_CUT_CIRCUIT_NO			750			///< �T�[�L�b�g�p�����[�^�G���[�F�T�[�L�b�g�ԍ��w��G���[

#define ERR_TRIMRESULT_TESTNO		760			///< �g���~���O���ʁF�擾�Ώی��ʔԍ��w��G���[�i�e�X�g���{�����傫���j
#define ERR_TRIMRESULT_RESNO		761			///< �g���~���O���ʁF�擾�Ώے�R�J�n�ԍ��w��G���[�i�o�^��R�����傫���J�n�ԍ��j
#define ERR_TRIMRESULT_TOTALRESNO	762			///< �g���~���O���ʁF�擾�Ώے�R�ԍ��w��G���[�i�o�^��R�����傫���j
#define ERR_TRIMRESULT_CUTNO		763			///< �g���~���O���ʁF�擾�ΏۃJ�b�g�J�n�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j
#define ERR_TRIMRESULT_TOTALCUTNO	764			///< �g���~���O���ʁF�擾�ΏۃJ�b�g�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j
#define ERR_TRIMRESULT_ESRESCUTNO	765			///< �g���~���O���ʁF�擾�Ώے�R�ԍ�or�J�b�g�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j

#define ERR_TRIM_COTACTCHK			770			///< �R���^�N�g�`�F�b�N�G���[
#define ERR_TRIM_OPNSRTCHK			771			///< �I�[�v���V���[�g�`�F�b�N�G���[
#define ERR_TRIM_CIR_NORES			772			///< �T�[�L�b�g�g���~���O-���Ώے�R�Ȃ�

#define ERR_MARKVFONT_ANGLE			780			///< �t�H���g�ݒ�F�p�x�w��G���[
#define ERR_MARKVFONT_MAXPOINT		781			///< �t�H���g�ݒ�F�|�C���g���I�[�o�[

#define ERR_TRIMMODE_UNMATCH		790			///< �g���~���O���[�h�s��v�F���V�I�g���~���O�Ώۂ�����̂Ƀf�B���C���[�h���ݒ肳��Ă���

// ���[�U�n�G���[�R�[�h:800�ԑ�
#define ERR_LSR_PARAM_ONOFF			801			///< ���[�UON/OFF�p�����[�^�G���[
#define ERR_LSR_PARAM_QSW			802			///< Q���[�g�p�����[�^�G���[
#define ERR_LSR_PARAM_QSW_LO		803			///< Q���[�g�p�����[�^�G���[�F�ŏ�Q���[�g�ȉ�
#define ERR_LSR_PARAM_QSW_HI		804			///< Q���[�g�p�����[�^�G���[�F�ŏ�Q���[�g�ȏ�
#define ERR_LSR_PARAM_FLCUTCND		830			///< FL�p���H�����ԍ��ݒ�G���[
#define ERR_LSR_PARAM_FLPRM			831			///< FL�p�p�����[�^�G���[�i�����ʎw��G���[�j
#define ERR_LSR_PARAM_FLTIMEOUT		832			///< FL�p�ݒ�^�C���A�E�g
#define ERR_LSR_STATUS_STANBY		833			///< FL:Stanby error,ES:POWER ON READY error
#define ERR_LSR_FL_BIASON_WAITOUT	834			///< FL�pBIAS ON��̃E�F�C�g�^�C���A�E�g
#define ERR_LSR_FL_BIASON_ERROR		835			///< FL�pBIAS ON�G���[
#define ERR_LSR_STATUS_OSCERR		850			///< FL:Error occured,:ES:LD Alarm
#define ERR_LSR_OFF_SW_ON			851			///< SL436S�p:�uLASER OFF SW�v���� V.2.0.0.0_24

// �X�e�[�W�n�G���[�R�[�h:
// 
#define ERR_STG_STATUS				901			///< �X�e�[�W�n�ŃX�e�[�^�X�G���[����
#define ERR_STG_NOT_EXIST			902			///< �ΏۃX�e�[�W(X/Y/Z/Z2/Theta)���݂����B
#define ERR_STG_ORG_NONCOMPLETION	903			///< ���_���A������
//#define	ERR_SOFTLIMIT_X				105			///< X���\�t�g���~�b�g @@@440
//#define	ERR_SOFTLIMIT_Y				106			///< Y���\�t�g���~�b�g @@@440
//#define	ERR_SOFTLIMIT_Z				107			///< Z���\�t�g���~�b�g @@@440
#define ERR_STG_MOVESEQUENCE		905			///< �X�e�[�W����V�[�P���X�ݒ�G���[
#define ERR_STG_THETANOTEXIST		906			///< �ƍڕ���Ȃ�
#define ERR_STG_INIT_X				910			///< X�����_���A�G���[
#define ERR_STG_INIT_Y				911			///< Y�����_���A�G���[
#define ERR_STG_INIT_Z				912			///< Z�����_���A�G���[
#define ERR_STG_INIT_T				913			///< Theta�����_���A�G���[
#define ERR_STG_INIT_Z2				914			///< Z2�����_���A�G���[
#define ERR_STG_MOVE_X				915			///< X���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
#define ERR_STG_MOVE_Y				916			///< Y���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
#define ERR_STG_MOVE_Z				917			///< Z���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
#define ERR_STG_MOVE_T				918			///< Theta���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
#define ERR_STG_MOVE_Z2				919			///< Z2���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
#define ERR_STG_CLAMPX				920			///< �N�����vX�`�F�b�N�G���[ V4.17.0.0�@
#define ERR_STG_CLAMPY				921			///< �N�����vY�`�F�b�N�G���[ V4.17.0.0�@
#define ERR_STG_SOFTLMT_PLUS		930			///< �v���X���~�b�g�I�[�o�[
#define ERR_STG_SOFTLMT_MINUS		931			/**< �}�C�i�X���~�b�g�I�[�o�[ */
#define ERR_Z2_ORG_INTERLOCK		932			/// �X�e�[�W���쎞��Z2���_�Z���T�[��ON���Ă��Ȃ����ߓ���s��
#define ERR_STG_SERR		        933			/// ���C���X�e�[�^�X���[�h���u�G���[�����ݔ����v//V.2.0.0.0�P 

///	�^�C���A�E�g�G���[�ɂ��ẮA�x�[�X+�Ώ̎��r�b�g(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
///	�̃G���[�R�[�h�ŕԂ��B
#define ERR_TIMEOUT_BASE			950			/**< �^�C���A�E�g�G���[(�x�[�X�ԍ�)
												 * - X���^�C���A�E�g=951
												 * - Y���^�C���A�E�g=952
												 * - Z���^�C���A�E�g=954
												 * - Theta���^�C���A�E�g=958
												 * - Z2���^�C���A�E�g=956
												 * - ��������or�̒l
												 * - X,Y���^�C���A�E�g=953 */

#define ERR_AXIS_X_BASE				1000		///< X���n�G���[�x�[�X�ԍ�
#define ERR_AXIS_Y_BASE				2000		///< Y���n�G���[�x�[�X�ԍ�
#define ERR_AXIS_Z_BASE				3000		///< Z���n�G���[�x�[�X�ԍ�
#define ERR_AXIS_T_BASE				4000		///< Theta���n�G���[�x�[�X�ԍ�
#define ERR_AXIS_Z2_BASE			5000		///< Z2���n�G���[�x�[�X�ԍ�

// IO�n�G���[�R�[�h:
#define ERR_IO_PCINOTFOUND			10000		///< PCI�{�[�h�����o�ł���
#define ERR_IO_NOTGET_RTCCMOSDATA	10001		///< RTC�ް��ǂݍ��ݎ��s

//GPIB�n�G���[�R�[�h
#define ERR_GPIB_PARAM				11001		///< GPIB�p�����[�^�G���[
#define ERR_GPIB_TCPSOCKET			11002		///< GPIB-TCP/IP���M�G���[
#define ERR_GPIB_EXEC				11003		///< GPIB�R�}���h���s�G���[


#endif //#ifndef	__TRIM_ERR_NO_H__
