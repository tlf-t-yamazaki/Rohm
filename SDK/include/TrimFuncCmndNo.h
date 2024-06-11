/***************************************************************************//**
 * @file        TrimFuncCmndNo.h
 * @brief       �R�}���h�ԍ���`�t�@�C��
 *
 * @par     (C) Copyright OMRONLASERFRONT INC. 2010-2012 All Rights Reserved
 ******************************************************************************/
#ifndef	__TRIM_FUNC_CMND_H__
#define	__TRIM_FUNC_CMND_H__

//#include "Defines.h"

//========================================================================
//-----( �R�}���h )-----
//========================================================================
//-------------------------------------------------------
// 2010/08/19 �V�K�ŃR�}���h�ԍ��U��Ȃ���
//-------------------------------------------------------
#define CMD_SEGMENT_BASE			1000	/**< �g���[�X���O�o�͂̂��߂̃x�[�X�ԍ�
											 * - ���O�o�͎��ACMD�ԍ�/�x�[�X�ԍ������{���A�g���[�X�Ώۂ̃��O���o�͂���B */
// System 
//#define	CMD_AXIS_Z_ORG				1001	///< Z���_�m�F
#define	CMD_BPOFF					1002	///< *�r�[���|�W�V���i�̃��[�N���W�̌��_�̕␳
#define	CMD_BSIZE					1003	///< *�u���b�N�T�C�Y�̐ݒ�
//#define	CMD_DIMENS					1004	///< *��n�ی��ݒ�
#define	CMD_DREAD					1005	///< �f�B�W�^���X�B�b�`�̓ǂݍ��݁i10�i�ŕԂ��j
#define	CMD_DREAD2					1006	///< �f�B�W�^���X�B�b�`�̓ǂݍ��݁i1�ʁA10�ʕ����ĕԂ��j
#define	CMD_EMGRESET				1007	///< *EMG����
//#define	CMD_FPRESET					1008	///< �t�@�[�X�g�p���X�T�v���b�T�[�𖳌��ɂ���(�I�v�V����)
//#define	CMD_FPSET					1009	///< �t�@�[�X�g�p���X�T�v���b�T��L���ɂ���(�I�v�V����)
//#define	CMD_FPSET2					1010	///< �t�@�[�X�g�p���X�T�v���b�T��L���ɂ���(�I�v�V����)
#define	CMD_GETERRSTS				1011	///< XY�e�[�u���܂���BP�̃��~�b�g���o
#define	CMD_ILUM					1012	///< �g���}�̃��[�U�o�ˌ����i��ʁj�̃����t�̃I���E�I�t
//#define	CMD_INTIME_VERSION			1013	///< INtime_Version
#define	CMD_ITIMESET				1014	///< IRQ0�����֎~/����(Trimfunc.ocx�p)
#define	CMD_NO_OPERATION			1015	///< �������Ȃ��R�}���h(cmd_GetStatus��ǂݏo���B�j
#define	CMD_READRTCCMOS				1016	///< RTC CMOS�ǂݍ���
#define	CMD_SETAXISLIMITS			1018	///< �e���ЯĐݒ�
#define	CMD_SETDLY					1019	///< �ިڲ��ѐݒ�
#define	CMD_SETSIGNALTOWER			1020	///< ������ܰ����
#define	CMD_SETTIMMER_MODE			1021	///< ��ϰ����Ӱ��
#define	CMD_SLIDE_COVER_CHK			1022	///< �X���C�h�J�o�[�`�F�b�N�̐ؑւ�
#define	CMD_RESET					1017	///< XY�e�[�u���܂���BP�̃��~�b�g�t���O������������
#define	CMD_SYSINIT					1023	///< *�S�C�j�V�����C�Y
#define	CMD_TERMINATE				1027	///< �I��(�߂�Ȃ�)
#define	CMD_X_INIT					1028	///< *�w���C�j�V�����C�Y
#define	CMD_Y_INIT					1029	///< *�x���C�j�V�����C�Y
#define	CMD_Z_INIT					1030	///< *�y���C�j�V�����C�Y
#define	CMD_Z_INPSTS				1031	///< �g���}�̃R���\�[�����͂���X�e�[�^�X��ǂ�
#define	CMD_ZABSVACCUME				1032	///< �o�L���[���̐���
#define	CMD_ZCONRST					1033	///< �R���\�[���X�C�b�`�̃��b�`����
#define	CMD_ZLATCHOFF				1034	///< Z�X�C�b�`�̃��b�`����
#define	CMD_ZSETPOS2				1035	///< ��Q���_�p�I�t�Z�b�g
#define	CMD_ZSLCOVERCLOSE			1036	///< �X���C�h�J�o�[�N���[�Y�o���u��ON/OFF
#define	CMD_ZSLCOVEROPEN			1037	///< �X���C�h�J�o�[�I�[�v���o���u��ON/OFF
#define	CMD_ZSYSPARAM1				1038	///< �V�X�e���p�����[�^�P�ݒ�
#define	CMD_ZSYSPARAM2				1039	///< �V�X�e���p�����[�^�Q�ݒ�
#define	CMD_ZSYSPARAM3				1040	///< �V�X�e���p�����[�^�R�ݒ�
//#define	CMD_SYSPARAM3				1024	///< �V�X�e���p�����[�^�R�ݒ� ��ٽ���ݒ�QRate
//#define	CMD_SYSPARAM4				1025	///< �V�X�e���p�����[�^�S�ݒ� ��ٽ���ݒ���ٽ��
//#define	CMD_SYSPARAM5				1026	///< �V�X�e���p�����[�^�T�ݒ� ��ٽ���ݒ�L���ް���
#define	CMD_ZWAIT					1041	///< �w�莞�ԃE�G�C�g����
#define	CMD_ZZGETRTMODULEINFO		1042	///< RT���W���[���̃o�[�W�����ԍ����擾����
#define	CMD_ZSYSPARAM4				1043	///< �V�X�e���p�����[�^�S�ݒ� // V808�@(DllTrimFunc)
#define CMD_REMEASURE_PAUSE			1044	// �đ���O�̃|�[�Y�^�C���ݒ�	////V4.9.0.0�A
#define	CMD_MIDIUM_CUT_PARAM		1045	/// �r���؂茟�o�p�̃p�����[�^��ݒ� 
#define	CMD_CUT_POINT_PARAM			1046	/// �^�[���|�C���g�A�ؑփ|�C���g�̃p�����[�^��ݒ� V4.1.0.2�@
#define CMD_BSIZE2					1047	// �u���b�N�T�C�Y�̐ݒ�R�}���h�F���݂̃T�C�Y�ɂ�炸�ɐݒ���s���B//V4.6.0.11�@
#define CMD_VARIABLE_POINT_PARAM	1048	// �σ^�[���|�C���g�Ή�		//V5.2.0.0�@
#define CMD_VARIABLE_POINT_PARAM2	1049	// �σ^�[���|�C���g�Ή�		//V5.2.0.0�@

//-----------------------
//�V�K�ǉ�
#define	CMD_GET_STATUS				1050	///< ���u��ԁi�e���ABP�̏�ԁj���擾����
#define CMD_SYSTEMRESET				1051	///<�@�V�X�e�����Z�b�g�����s����
#define CMD_SERVO_POWER				1052	///< �T�[�{��ON/OFF����
#define CMD_CLEAR_SERVO_ALARM		1053	///< �T�[�{�A���[���̃N���A
#define CMD_INTERLOCK_CHECK			1054	///< �C���^�[���b�N��Ԃ̎擾
#define CMD_CONSOLE_SWWAIT			1055	///<�@�R���\�[��(START/RESET/HALT)�{�^���̉�����ԑ҂�(�������[�v�Ď��j
#define CMD_CONSOLE_LAMP_CTRL		1056	///< �R���\�[����̃����vON/OFF���� 
#define CMD_COVER_LATCHCLR			1057	///< �J�o�[�J���b�`�N���A
#define CMD_COVER_LATCHCHK			1058	///< �J�o�[�J���b�`�`�F�b�N
#define CMD_COVER_CHK				1059	///< �Œ�J�o�[��ԃ`�F�b�N
#define CMD_SLIDECOVER_MOVECHK		1060	///< �X���C�h�J�o�[�̃I�[�v��/�N���[�Y�̓��슮���Ď�
#define CMD_SLIDECOVER_OPENCLOSECHK	1061	///< �X���C�h�J�o�[�̃I�[�v��/�N���[�Y�`�F�b�N
#define CMD_SLIDECOVER_GETSTS		1062	///< �X���C�h�J�o�[�̏�Ԏ擾
#define CMD_EMGRESET_SENSE			1063	///< ����~�ُ��Ԃ̃��Z�b�g�Ď�
#define CMD_EMGSTS_CHK				1064	///< ����~���(�{�^��)�̃`�F�b�N
#define CMD_HALT_SWCHK				1065	///< HALT�{�^���̃`�F�b�N
#define CMD_SIGTWR_CTRL				1066	///< SignalTower�̐���
#define CMD_START_SWCHK				1067	///< �X�^�[�g�{�^���̃`�F�b�N
#define CMD_STARTRESET_SWCHK		1068	///< �X�^�[�g/���Z�b�g�{�^���̃`�F�b�N
#define CMD_CONSOLE_SWCHK			1069	///<�@�R���\�[��(START/RESET/HALT)�{�^���̏�Ԃ̎擾
#define CMD_Z_SWCHK					1070	///< Z�{�^���̃`�F�b�N
#define CMD_CLAMP_CTRL				1071	///< �N�����v��ON/OFF����
#define CMD_CLAMP_GETSTS			1072	///< �N�����v��Ԃ̎擾
#define	CMD_COVER_CHK_ONOFF			1073	///< �Œ�J�o�[�`�F�b�N�̐ؑւ� @@@064
#define	CMD_GET_COVER_CHK_STS		1074	///< �Œ�J�o�[/�X���C�h�J�o�[�̃`�F�b�N�L���̎擾 @@@065
#define CMD_SERVO_POWERZ2			1075	///< �T�[�{��ON/OFF����(Z2�p)		@@@131
#define CMD_CLEAR_SERVO_ALARMZ2		1076	///< �T�[�{�A���[���̃N���A(Z2�p)	@@@131
//-----------------------
#define CMD_VARIABLE_INDEXPITCH_PARAM	1077	// �σC���f�b�N�X�s�b�`�F�����l�ݒ�
#define CMD_VARIABLE_INDEXPITCH_PARAM2	1078	// �σC���f�b�N�X�s�b�`�F�C���f�b�N�X�s�b�`�ݒ�
#define CMD_VARIABLE_TURNPOINT_PARAM 1079		// �σ^�[���|�C���g�Ή�		///V5.6.0.0�@

#define CMD_SETLOG_ALLTARGET		1101	///< �S���O�o�͑ΏۃZ�O�����g�̐ݒ�R�}���h
#define CMD_GETLOG_ALLTARGET		1102	///< �S���O�o�͑ΏۃZ�O�����g�̎擾�R�}���h
#define CMD_SETLOG_TARGET			1103	///< ���胍�O�o�͑ΏۃZ�O�����g�̐ݒ�R�}���h
//#define CMD_GETLOG_TARGET			1103	///< ���O�o�͑ΏۃZ�O�����g�̎擾�R�}���h

// IO ctrl
#define	CMD_ALDFLGRST				2001	///< �I�[�g���[�_�[�t���O���Z�b�g
#define	CMD_EXTIN					2002	///< ���[�U�p�A�W��EXTBIT I/O 
#define	CMD_EXTOUT					2003	///< ���[�U�[�p�A�W��EXTBIT I/O �|�[�g�֏o��
#define	CMD_EXTOUT1					2004	///< ���[�U�p�A�W��EXTBIT I/O �|�[�g�������
#define	CMD_EXTOUT2					2005	///< �g��EXT BIT�̃r�b�g I/O LOWER �|�[�g�֏o��(�r�b�g�ۑ�)
#define	CMD_EXTOUT3					2006	///< �g��EXT BIT�̃r�b�g I/O HIGHER �|�[�g�֏o��(�r�b�g�ۑ�)
#define	CMD_EXTOUT4					2007	///< �g�� EXTOUT4
#define	CMD_EXTOUT5					2008	///< �g�� EXTOUT5
#define	CMD_EXTRSTSET				2009	///< ���[�U�p�A�W��EXTBIT I/O �|�[�g�֏o�͊���l�̎w��
#define	CMD_HOST_CMD_IN				2010	///< �z�X�g�ʐM�pCMD IN
#define	CMD_HOST_STS_OUT			2011	///< �z�X�g�ʐM�pOUT
#define	CMD_INP16					2012	///< *�w��̃A�h���X����P�U�r�b�g�̃f�[�^�𖳕������Ƃ��ē���
#define	CMD_OUT_WORD				2013	///< *I/O�o��(Word)
#define	CMD_OUT16					2014	///< *�w��̃A�h���X�ւP�U�r�b�g�̃f�[�^���o��
#define	CMD_OUTBIT					2015	///< *I/O�o��(bit set)
#define	CMD_PIN16					2016	///< *�w��̃A�h���X����P�U�r�c�g�̃f�[�^�����
#define	CMD_ZATLDGET				2017	///< *���[�_����f�[�^����͂���
#define	CMD_ZATLDSET				2018	///< ���[�_�փf�[�^���o�͂���
#define	CMD_ZATLDRED				2019	///< ���݂̃��[�_�o�̓f�[�^�Ԃ� @@@059
#define	CMD_SETEXTPOWER				2020	///< �O���d���o�͗p�Ɋ֐��ǉ�	>//V8.2.0.3�@

// Laser
#define	CMD_FLSET					3001	///< ���H�����ԍ��ݒ�(FL�p)
#define	CMD_GETLASERCURRENT			3002	///< ڰ�ް�d������d���l�擾
#define	CMD_GETPOWERCTRL			3003	///< ��ܰ���۰ٓǂݍ���
#define	CMD_LASER_OFF				3004	///< Laser�@OFF
#define	CMD_LASER_ON				3005	///< Laser�@ON
#define	CMD_LATTSET					3006	///< ���[�^���[�A�b�e�l�[�^�i���[�U�[�������ݒ�j
#define	CMD_POWERMEASURE			3007	///< ��ܰ����
#define	CMD_PROCPOWER				3008	///< ���H�����d�͂̐ݒ�
#define	CMD_QRATE					3009	///< *���[�U���U��̂p�X�B�b�`���[�g��ݒ�
#define	CMD_ROT_ATT2				3010	///< ���[�^���[�A�b�e�l�[�^�i���[�U�[�������ݒ�j2
#define	CMD_SETLASERCURRENT			3011	///< ڰ�ް�d������d���l�ݒ�
#define	CMD_SETLASERCURRENTCONTROL	3012	///< ڰ�ް�d��������۰ِݒ�
#define	CMD_SETQRATE2				3013	///< Q���[�g�ݒ�(��ٽ���p)
#define	CMD_ZSETBPTIME				3014	///< ���[�U�[ON/OFF�̃|�[�Y�^�C���ݒ�
// NewIF
#define CMD_GET_QRATE				3015	///< Q���[�g�̎擾
#define CMD_SET_FL_ERRLOG			3016	///< FL�Ŕ��������G���[�������O�֏o��
#define CMD_ATTRESET				3017	///< �A�b�e�l�[�^���Z�b�g
#define CMD_ATTSTATUS1				3018	///< �A�b�e�l�[�^����f�[�^1�擾�F�G���[�Ȃ�тɊe�탊�~�b�g���
#define CMD_ATTSTATUS2				3019	///< �A�b�e�l�[�^����f�[�^2�擾�F�A�b�e�l�[�^���W�ʒu�A�Œ�ATT���
#define CMD_PMON_SHUTCTRL			3020	///< �p���[���[�^�V���b�^����(0/1=OFF/ON)
#define CMD_PMON_MEASCTRL			3021	///< �p���[���[�^���胂�[�h����(0/1=�ʏ푪��/�p���[���j�^����)
#define CMD_ONESHOTLASER			3022	///< ���[�U�@�P�V���b�g�@�\
#define CMD_SETFIXATTINFO			3023	///< �Œ�ATT���̐ݒ�(�z��)�@// V809�A 
#define CMD_SETFIXATTONEINFO		3024	///< �Œ�ATT���̐ݒ�(��)�@// V809�A 
#define CMD_FPS_SET					3025	///< //V8.2.3.10�@�@//V4.19.0.0�A
#define	CMD_EXECFLG_SEND			3026	///< ��f�B���C���̃g���~���O���s�L���̑��M	// V8.0.1.17�@�@//V4.19.0.0�B

// Bp Ctrl
#define	CMD_BP_CALIBRATION			4001	///< BP�L�����u���[�V�����ݒ�
#define	CMD_BP_LINEARITY			4002	///< BP���j�A���e�B�␳�l
#define	CMD_BPMOVE					4003	///< *BP �ړ�
#define	CMD_CIRTST					4005	///< CIRCLE Test
#define	CMD_CUTPOSCOR				4006	///< �J�b�g�ʒu�␳�l�󂯎��
#define	CMD_CUTPOSCOR_ALL			4015	///< �J�b�g�ʒu�␳�l�󂯎��i�ꊇ��M�j
#define	CMD_MOVE					4007	///< �r�[���|�W�V���i�̍ō����ړ�
#define	CMD_TEACH1					4008	///< �r�[���|�W�V���i�̃I�t�Z�b�g�l���e�B�[�`���O�ɂ���āA�Z�b�g����
#define	CMD_TEACH2					4009	///< �e�g���~���O�X�^�[�g�_���e�B�[�`���O�ɂ��A����
#define	CMD_TRIM_NGMARK				4010	///< NG�T�[�L�b�g�̃}�[�L���O
#define	CMD_ZBPLOGICALCOORD			4011	///< BP�_�����W�̏ی��ݒ�
#define	CMD_ZGETBPPOS				4012	///< BP���ݍ��W�ǂݍ���
#define CMD_TCORPOSSET				4013	///< ���W����]�i�Ɓj�␳�pXY�|�W�V�����ݒ�i�ƍڕ��䖳���j
#define CMD_TCORANGSET				4014	///< ���W����]�i�Ɓj�␳�p�p�x�ݒ�i�ƍڕ���L��j
#define CMD_BP_GET_CALIBDATA		4016	///< BP�L�����u���[�V�����f�[�^�̎擾
#define	CMD_ZGETBPPOS2				4017	///< BP���ݍ��W�ǂݍ���2(cmd_move�ŕۑ������w�ߒl��Ԃ�)		// V809�C
#define	CMD_GETCUTLENGTH			4018	///< �J�b�g�����������擾			//V4.6.0.9�B
// Meas
#define	CMD_DSCAN					5001	///< �c�b�X�L���i�̃v���[�u�ԍ����w�肷��
#define	CMD_DIFFERENCE_CORRECT		5002	///< ���d���␳�l�ݒ�
//#define	CMD_DMEAS					5003	///< ���������Œ�R����(���d��)
#define	CMD_FAST_WMEAS				5004	///< �p���[���j�^����
//#define	CMD_LJRATE					5005	///< *���ω��g���~���O�L���Ɣ��藦�̎w��
#define	CMD_RANGE_SET				5006	///< ���背���W�̐ݒ�i�V�K�ǉ��j
//#define	CMD_MEASMODE				5006	///< ��R�A�d���A�X���[�v���[�h�ݒ�
#define	CMD_MFSET					5007	///< �c�b�X�L���i�ɐڑ����鑪���ƃ��[�h�؂�ւ�
#define	CMD_MSCAN					5008	///< DSCAN���K�[�h�v���[�u���g�������֐�
#define	CMD_RRANGE_CORRECT			5009	///< �����W�␳�l�ݒ�

#define CMD_MEASURE					5010	///< ����i��R/�d��-�I�[�g/�Œ�-����/�����x���p�����[�^�Ŏw��j
#define	CMD_RMEAS_AUTORANGE			5011	///< ���������Œ�R����
#define	CMD_RMEAS_FIXRANGE			5012	///< ���������Œ�R����i�Œ背���W���w��j
#define	CMD_VMEAS_AUTORANGE			5013	///< ���������łc�b�d������
#define	CMD_VMEAS_FIXRANGE			5014	///< ���������œd������i�Œ背���W���w��j

#define	CMD_RMEASHL					5015	///< ���������Œ�R����
#define	CMD_TEST					5016	///< �w�肵���l�͈̔͂��r����
#define	CMD_VRANGE_CORRECT			5017	///< �d�������W�␳�l�ݒ�
#define	CMD_ZGETDCVRANG				5018	///< �����c�b�����̍ő僌���W�̍ő�d���l��Ԃ��B
#define	CMD_HILOSEPARATEDSET		5020	///< �c�b�X�L���i�̃v���[�u�ԍ��g�h�C�k�n�iForce,Sense)�����ݒ� V8.0.0.14�B
#define	CMD_DSCAN_SEPARATED			5021	///< �c�b�X�L���i�̃v���[�u�ԍ����w�肷��g�h�C�k�n�iForce,Sense)���� V8.0.0.14�B
#define	CMD_MSCAN_SEPARATED			5022	///< DSCAN_SEPARATED���K�[�h�v���[�u���g�������֐� V8.0.0.14�B
#define	CMD_SET_POWER_RESOLUTION	5023	///< �p���[���j�^���蕪��\�ݒ�//V5.0.0.1�@ 2016.12.27

//#define	CMD_BPTRIM					4004	///< *BP TRIM
//#define	CMD_RMEAS2					5011	///< ��R����(function)
//#define	CMD_ZRMEAS2					5018	///< ��R����I�[�g�����W
//#define	CMD_ZVMEAS2					5020	///< ���������œd������i�X�L���i�w��Ȃ��A�I�[�g�����W�j
//#define	CMD_ZZVMEAS2				5021	///< ���������œd������i�Œ背���W�j

// Cut&Trimming
//VE���u����-�V�K�ǉ��R�}���h
#define CMD_TrimST					6000	///< �΂ߒ����J�b�g/�g���~���O�iFL�Ή��Łj
#define CMD_TrimL					6010	///< �΂�L�J�b�g/�g���~���O�iFL�Ή��Łj
#define CMD_TrimL6					6011	///< V8.0.0.17�@�U�_�^�[���|�C���g�k�J�b�g�g���~���O�iFL���Ή��Łj//V4.6.0.9�A
#define CMD_TrimHkU					6020	///< �΂߃t�b�NorU�J�b�g/�g���~���O�iFL�Ή��Łj
//--- ���[�U���J������(2010/12/14)
#define CMD_TrimSC					6030	///< �X�L�����J�b�g/�g���~���O�iFL�Ή��Łj
#define CMD_TrimIX					6040	///< �΂߃C���f�b�N�X�J�b�g/�g���~���O�iFL�Ή��Łj
//---�@�K�v�Ɣ��f�������_��DllTrimFunc��IF��ǉ�����B
#define CMD_TrimES					6050	///< �G�b�W�Z���X�J�b�g/�g���~���O�iFL�Ή��Łj
#define CMD_TrimES0					6051	///< �G�b�W�Z���X�J�b�g/�g���~���O�iFL�Ή��Łj�@//V4.12.0.0�@

#define CMD_TrimMK					6060	///< �����w�蕶���}�[�L���O�iFL�Ή��Łj
#define CMD_TrimU4					6061	///< ����U�J�b�g���~���O�iFL�Ή��ŁjV4.2.0.0�@

#define	CMD_UCUT_PARAMSET			6021	///< UCUT PARAMETER
#define	CMD_UCUT_RESULT				6022	///< UCUT retrieve result
#define	CMD_RATIO2EXP				6023	///< ���V�I���[�h�Q�̌v�Z���w��
#define	CMD_ZSETUCUT				6024	///< ���g���[�XU�J�b�g�p�����[�^�ݒ�
#define	CMD_UCUT4RESULT				6027	///< ���g���[�XU�J�b�g�p�����[�^�e�[�u���C���f�b�N�X�擾
#define	CMD_VUTRIM4					6028	///< U�J�b�g���g���[�X�d���g���~���O(�΂߂ł͂Ȃ�)

#define	CMD_TRIMBLOCK				6070	///< �g���~���O���s�i�u���b�N�P��)
#define	CMD_TRIMDATA				6071	///< �g���~���O�f�[�^
#define	CMD_TRIM_RESULT				6072	///< �g���~���O���ʎ擾
#define	CMD_TRIMEND					6073	///< �g���~���O�I��(Memory clean-up)
#define	CMD_TRIMBLOCKMEASURE		6074	///< �I�[�g�����W������s�i�u���b�N�P��)	// @@@015
#define	CMD_TRIMMEASURERESIST		6075	///< ��R�ԍ����Ă��Ă̒�R����	

#define CMD_PARAM_LTURNPARAM		6076	///< ��L�^�[���|�C���g�̃f�[�^�ݒ�	//V4.6.0.5�@  
#define	CMD_LTURN_RESULT			6077	///< ��L�^�[���|�C���g���s���ʂ̎擾	//V4.6.0.5�@
#define	CMD_ZCOVEROPEN_MOVE			6078	///< �J�o�[�J�ł̃X�e�[�W�ړ����p�t���O�ݒ�R�}���h	V4.6.0.5�B
#define	CMD_HIVOLT_SET				6079	///< ���d���p�ݒ�						// //V4.15.0.0�@
#define	CMD_HIVOLT_INIT				6080	///< ���d���p�ݒ�̏�����				// //V4.15.0.0�@
#define	CMD_HIVOLT_EXTPOWER			6081	///< ���d���p�̊O���d����ON/OFF����		// //V4.15.0.0�@
#define	CMD_HIVOLT_CORRECTSET		6082	///< ���d���p�̕␳�W���ݒ�p			// //V4.15.0.0�@
#define	CMD_HIVOLT_SPECSET			6083	///< ���d���d�l�̐ݒ�					// //V4.15.0.0�@
#define	CMD_HIVOLT_VXSELSET			6084	///< ���d���pVxSEL�̐ݒ�				// //V4.15.0.0�@
#define	CMD_HIVOLT_ADDRESIST		6085	///< ������R�L���w��ݒ�				// //V4.15.0.0�@
#define	CMD_FPS_SETTING				6086	///< FPS�ݒ�R�}���h					// //V4.15.0.0
#define	CMD_ATT_RESET_FLGSET		6087	///< �A�b�e�l�[�^���Z�b�g�t���O�ݒ�		// //V4.15.0.0

// Mechanical(stage)
#define	CMD_PROBOFF					8001	///< �y�v���[�u���I�t�ʒu�ֈړ�
#define	CMD_PROBOFF2				8002	///< *Z2�v���[�u���I�t�ʒu�ֈړ�
#define	CMD_PROBON					8003	///< �y�v���[�u���I���ʒu�ֈړ�
#define	CMD_PROBON2					8004	///< *Z2�v���[�u���I���ʒu�ֈړ�
#define	CMD_PROBUP					8005	///< *�y�v���[�u���I���ʒu����w�苗���グ��
#define	CMD_PROBUP2					8006	///< *Z2�v���[�u���I���ʒu����w�苗���グ��
#define	CMD_RBACK					8007	///< -�ƍڕ�������_�ʒu�ɖ߂�(�I�v�V����)
#define	CMD_RINIT					8008	///< -�ƍڕ���̏�����(�I�v�V����)
#define	CMD_ROUND					8009	///< �ƍڕ���̊p�x�ʒu�w��̉�]�i�I�v�V�����j
#define	CMD_ROUND4					8010	///< �ƍڕ���̐�Ίp�x�w��̉�]�i�I�v�V�����j
#define	CMD_SBACK					8011	///< �p�[�c�n���h�������[�h�ʒu�ɖ߂�
#define	CMD_SMOVE					8012	///< �p�[�c�n���h���𑊑΋����ňړ�
#define	CMD_SMOVE2					8013	///< �p�[�c�n���h�����΍��W�ňړ�
#define	CMD_START					8014	///< �p�[�c�n���h�������[�h�ʒu����g�����ʒu�Ɉړ�
#define	CMD_TSTEP					8015	///< *�p�[�c�n���h�����w��̃u���b�N�ʒu�ֈړ�
/*///#define	CMD_XYOFF					8017	///< XY �e�[�u���̃X�e�b�v�A���h���s�[�g���W�̕␳ */
#define	CMD_ZMOVE					8016	///< *Z����
#define	CMD_ZMOVE2					8017	///< *Z2����
#define	CMD_ZSTOPSTS				8018	///< *Z��~�`�F�b�N
#define	CMD_ZSTOPSTS2				8019	///< *Z2��~�`�F�b�N
//#define	CMD_ZOUTPUT					8021	///< *Z����̂�
//#define	CMD_ZOUTPUT2				8022	///< *Z2����̂�
#define	CMD_ZSELXYZSPD				8020	///< XYZ�̃X�s�[�h�̑I��
#define	CMD_ZSTGXYMODE				8021	///< XY�X�e�[�W����w��(�ʏ�A�����v���[�u�Ή��̃V�[�P���X)
#define	CMD_SMOVE3					8022	///< �p�[�c�n���h���𑊑΋����ňړ��w�ߒl����̍����ňړ�			�@//V8.2.0.1�E

#define	CMD_GETZPOS					8030	///< Z�ʒu�擾
#define	CMD_GETZ2POS				8031	///< Z2�ʒu�擾
#define	CMD_ZGETPHPOS				8032	///< *�p�[�c�n���h��(XY)�̌��ݍ��W�𓾂�
#define	CMD_ZGETPHPOS2				8033	///< *�p�[�c�n���h��(XY)�̌��ݍ��W�𓾂�//@@@777
#define	CMD_GETZPOS2				8034	///< Z�ʒu�擾�i��Q���_����W�n�j//V8.2.0.1�F
#define	CMD_SETLOADPOS				8035	///< ���[�h�|�W�V�����icmd_Sback()�j�̍��W��ݒ� //V8.2.0.2�A
#define	CMD_SETZOFFPOS				8036	///< Z�̑ҋ@�ʒu��ݒ肷�� //V8.0.0.10�A

#define CMD_GET_ALLAXIS_STATUS		8040	///<�@�S���̃X�e�[�^�X��Ԃ��擾����

//�f�o�b�O�p�R�}���h
#define CMD_MEAS_PERFORM			30001	///< �p�t�H�[�}���X����p�R�}���h
#define CMD_SET_AXISSPD				30002	///< XYZ�����x�ݒ�R�}���h
#define CMD_RESET_LSI				30003	///< LSI�̃��Z�b�g�R�}���h
#define CMD_SET_AXISPRM				30004	///<XYZ���ݒ�R�}���h @@@075
#define CMD_SET_AXISSPDX			30005	///< X�����x�ݒ�R�}���h @@@129
#define CMD_SET_AXISSPDY			30006	///< Y�����x�ݒ�R�}���h @@@129
#define CMD_SET_AXISSPDZ			30007	///< X�����x�ݒ�R�}���h V8.0.0.14�A
#define CMD_SET_AXISPRM2			30008	///< XYZ���ݒ�R�}���h2 V.2.0.0.0_21

//V4.6.0.6�@��
#define	CMD_MES_LOW_RESIST			30010	// �����R����F�P�Ƒ���R�}���h 
#define	CMD_PARAM_LOW_RESIST		30011	// �����R����F���C���p�����[�^�̓]��
#define	CMD_MES_LOW_RESULT			30012	// �����R����
#define	CMD_CORR_LOW_SEND			30013	// �����R����:�Z���l�̓]�� 
//V4.6.0.6�@��

#endif //#ifndef  __TRIM_FUNC_CMND_H__
