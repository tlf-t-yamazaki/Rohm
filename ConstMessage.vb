''===============================================================================
''   Description  : ���b�Z�[�W��`(���{��/�p��)
''
''   Copyright(C) : OMRON LASERFRONT INC. 2010
''
''===============================================================================
'Option Strict Off
'Option Explicit On
'Module ConstMessage
'#Region "���b�Z�[�W��`"
'    '===========================================================================
'    '   ���b�Z�[�W��`
'    '===========================================================================
'    ' �G���[���b�Z�[�W
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
'    Public MSG_127 As String    'V1.13.0.0�B

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
'    '----- V1.18.0.0�B�� -----
'    Public MSG_152 As String
'    Public MSG_153 As String
'    Public MSG_154 As String
'    '----- V1.18.0.0�B�� -----
'    Public MSG_155 As String 'V1.18.0.0�K
'    Public MSG_156 As String 'V1.23.0.0�F
'    Public MSG_157 As String 'V1.23.0.0�@
'    Public MSG_158 As String 'V2.0.0.0�D
'    Public MSG_159 As String 'V4.0.0.0�H
'    '----- V4.0.0.0-30�� -----
'    Public MSG_160 As String
'    Public MSG_161 As String
'    Public MSG_162 As String
'    '----- V4.0.0.0-30�� -----

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

'    ' ���x��
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

'    ' �ꎞ��~��ʗp ###014
'    Public LBL_FINEADJ_001 As String
'    '----- ###204�� ----- 
'    Public LBL_FINEADJ_002 As String

'    Public LBL_FINEADJ_003 As String
'    Public LBL_FINEADJ_004 As String
'    Public LBL_FINEADJ_005 As String
'    Public LBL_FINEADJ_006 As String ' V4.11.0.0�E
'    '----- ###204�� ----- 

'    'GP-IB����
'    Public LBL_PP_130 As String
'    Public LBL_PP_131 As String
'    Public LBL_PP_132 As String
'    Public LBL_PP_133 As String
'    Public LBL_PP_134 As String
'    Public LBL_PP_135 As String

'    'GP -IB����(�p�^�[��2)
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

'    '(TXTY�d�l�ύX)
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

'    ' �X�v���b�V�����b�Z�[�W
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
'    Public MSG_SPRASH39 As String                                   ' V1.13.0.0�B
'    Public MSG_SPRASH40 As String                                   ' V1.13.0.0�I
'    Public MSG_SPRASH41 As String                                   ' V1.13.0.0�I
'    Public MSG_SPRASH42 As String                                   ' V1.13.0.0�I
'    Public MSG_SPRASH43 As String                                   ' V1.18.0.1�G
'    Public MSG_SPRASH44 As String                                   ' V1.18.0.1�G
'    Public MSG_SPRASH45 As String                                   ' V4.0.0.0-71
'    Public MSG_SPRASH46 As String                                   ' V4.0.0.0-83
'    Public MSG_SPRASH47 As String                                   ' V4.0.0.0-83
'    Public MSG_SPRASH48 As String                                   ' V4.1.0.0�@
'    Public MSG_SPRASH49 As String                                   ' V4.1.0.0�@
'    Public MSG_SPRASH50 As String                                   ' V4.1.0.0�F
'    Public MSG_SPRASH51 As String                                   ' V4.1.0.0�F

'    '----- limit.frm�p -----
'    Public MSG_frmLimit_01 As String
'    Public MSG_frmLimit_02 As String
'    Public MSG_frmLimit_03 As String
'    Public MSG_frmLimit_04 As String
'    Public MSG_frmLimit_05 As String
'    Public MSG_frmLimit_06 As String
'    Public MSG_frmLimit_07 As String

'    '----- INtime���G���[���b�Z�[�W -----
'    Public MSG_SRV_ALM As String                                    ' �T�[�{�A���[��
'    Public MSG_AXIS_X_SERVO_ALM As String                           ' X���T�[�{�A���[��
'    Public MSG_AXIS_Y_SERVO_ALM As String                           ' Y���T�[�{�A���[��
'    Public MSG_AXIS_Z_SERVO_ALM As String                           ' Z���T�[�{�A���[��
'    Public MSG_AXIS_T_SERVO_ALM As String                           ' �Ǝ��T�[�{�A���[��

'    ' ���n�G���[(�^�C���A�E�g)
'    Public MSG_TIMEOUT_AXIS_X As String                             ' X���^�C���A�E�g�G���[
'    Public MSG_TIMEOUT_AXIS_Y As String                             ' Y���^�C���A�E�g�G���[
'    Public MSG_TIMEOUT_AXIS_Z As String                             ' Z���^�C���A�E�g�G���[
'    Public MSG_TIMEOUT_AXIS_T As String                             ' �Ǝ��^�C���A�E�g�G���[
'    Public MSG_TIMEOUT_AXIS_Z2 As String                            ' Z2���^�C���A�E�g�G���[
'    Public MSG_TIMEOUT_ATT As String                                ' ���[�^���A�b�e�l�[�^�^�C���A�E�g�G���[
'    Public MSG_TIMEOUT_AXIS_XY As String                            ' XY���^�C���A�E�g�G���[

'    ' ���n�G���[(�v���X���~�b�g�I�[�o�[)
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_X As String                    ' X���v���X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_Y As String                    ' Y���v���X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_Z As String                    ' Z���v���X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_T As String                    ' �Ǝ��v���X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_Z2 As String                   ' Z2���v���X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_PLUS As String                           ' ���v���X���~�b�g�I�[�o�[

'    ' ���n�G���[(�}�C�i�X���~�b�g�I�[�o�[)
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_X As String                   ' X���}�C�i�X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_Y As String                   ' Y���}�C�i�X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_Z As String                   ' Z���}�C�i�X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_T As String                   ' �Ǝ��}�C�i�X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_Z2 As String                  ' Z2���}�C�i�X���~�b�g�I�[�o�[
'    Public MSG_STG_SOFTLMT_MINUS As String                          ' ���}�C�i�X���~�b�g�I�[�o�[

'    ' ���n�G���[(���~�b�g���o)
'    Public MSG_AXS_LIM_X As String                                  ' X�����~�b�g���o
'    Public MSG_AXS_LIM_Y As String                                  ' Y�����~�b�g���o
'    Public MSG_AXS_LIM_Z As String                                  ' Z�����~�b�g���o
'    Public MSG_AXS_LIM_T As String                                  ' �Ǝ����~�b�g���o
'    Public MSG_AXS_LIM_Z2 As String                                 ' Z2�����~�b�g���o
'    Public MSG_AXS_LIM_ATT As String                                ' ATT�����~�b�g���o

'    ' BP�n�G���[
'    Public MSG_BP_MOVE_TIMEOUT As String                            ' BP �^�C���A�E�g
'    Public MSG_BP_GRV_ALARM_X As String                             ' �K���o�m�A���[��X
'    Public MSG_BP_GRV_ALARM_Y As String                             ' �K���o�m�A���[��Y
'    Public MSG_BP_HARD_LIMITOVER_LO As String                       ' BP���~�b�g�I�[�o�[�i�ŏ����͈͈ȉ��j
'    Public MSG_BP_HARD_LIMITOVER_HI As String                       ' BP���~�b�g�I�[�o�[�i�ő���͈͈ȏ�j
'    Public MSG_BP_LIMITOVER As String                               ' BP�ړ������ݒ胊�~�b�g�I�[�o�[
'    Public MSG_BP_SOFT_LIMITOVER As String                          ' BP�\�t�g���͈̓I�[�o�[
'    Public MSG_BP_BSIZE_OVER As String                              ' �u���b�N�T�C�Y�ݒ�I�[�o�[�i�\�t�g���͈̓I�[�o�[�j

'    ' �J�o�[�J���o
'    Public MSG_OPN_CVR As String                                    ' "➑̃J�o�[�J���o"
'    Public MSG_OPN_SCVR As String                                   ' "�X���C�h�J�o�[�J���o"
'    Public MSG_OPN_CVRLTC As String                                 ' "�J�o�[�J���b�`���o"

'    Public MSG_INTIME_ERROR As String                               ' INtime���G���[

'    '(��L��CONST_PPxx�͍폜�\��)
'    Public CONST_PP(256) As String '�f�[�^�ҏW��ʂ̃X�e�[�^�X�o�[�\���p�o�b�t�@

'    '(��L��CONST_SPx�͍폜�\��)
'    Public CONST_SP(5) As String '�f�[�^�ҏW��ʂ̃X�e�[�^�X�o�[�\���p�o�b�t�@
'    Public CONST_CA(5) As String '�f�[�^�ҏW��ʂ̃X�e�[�^�X�o�[�\���p�o�b�t�@
'    Public CONST_CI(5) As String '�f�[�^�ҏW��ʂ̃X�e�[�^�X�o�[�\���p�o�b�t�@
'    Public CONST_GP(5) As String '�f�[�^�ҏW��ʂ̃X�e�[�^�X�o�[�\���p�o�b�t�@
'    Public CONST_TY2(5) As String '�f�[�^�ҏW��ʂ̃X�e�[�^�X�o�[�\���p�o�b�t�@

'    ' ���b�Z�[�W������ł�
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

'    ' �f�[�^�ǂݍ��݂���ѕҏW�I�����̃G���[
'    Public CONST_LOAD01 As String '�폜�\��
'    Public CONST_LOAD06 As String '�폜�\��
'    Public CONST_LOAD08 As String '�폜�\��
'    Public CONST_LOAD99 As String '�폜�\��
'    Public LOAD_MSG01 As String '"�w�肵���t�@�C����������܂���B"

'    ' ����
'    ' �{�^�����������b�Z�[�W
'    Public MSG_BTN_CANCEL As String
'    ' �v���[�u�ʒu���킹���b�Z�[�W
'    Public MSG_PRB_XYMODE As String
'    Public MSG_PRB_BPMODE As String
'    Public MSG_PRB_ZTMODE As String
'    Public MSG_PRB_THETA As String
'    ' �V�X�e���G���[���b�Z�[�W
'    Public MSG_ERR_ZNOTORG As String
'    ' ���[�U�[������ʐ�����
'    Public MSG_LASER_LASERON As String
'    Public MSG_LASER_LASEROFF As String
'    Public MSG_LASEROFF As String
'    Public MSG_LASERON As String
'    Public MSG_ATTRATE As String
'    Public MSG_ERRQRATE As String
'    Public MSG_LOGERROR As String
'    ' �摜�o�^��ʐ�����
'    Public MSG_PATERN_MSG01 As String

'    ' ���샍�O�@���b�Z�[�W
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
'    Public MSG_OPLOG_FUNC30 As String '�T�[�L�b�g�e�B�[�`���O
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
'    '----- V1.13.0.0�B�� -----
'    ' TKY�n�I�v�V����
'    Public MSG_OPLOG_APROBEREC As String        ' �I�[�g�v���[�u�o�^
'    Public MSG_OPLOG_APROBEEXE As String        ' �I�[�g�v���[�u���s
'    Public MSG_OPLOG_IDTEACH As String          ' �h�c�e�B�[�`���O
'    Public MSG_OPLOG_SINSYUKU As String         ' �L�k�␳(�摜�o�^)
'    Public MSG_OPLOG_MAP As String              ' MAP
'    '----- V1.13.0.0�B�� -----
'    Public MSG_OPLOG_MAINT As String
'    Public MSG_OPLOG_PRBCLEAN As String

'    '----- V1.18.0.1�@�� -----
'    Public MSG_F_EXR1 As String
'    Public MSG_F_EXTEACH As String
'    Public MSG_F_CARREC As String
'    Public MSG_F_CAR As String
'    Public MSG_F_CUTREC As String
'    Public MSG_F_CUTREV As String
'    Public MSG_OPLOG_CMD As String
'    '----- V1.18.0.1�@�� -----

'    'BLICK MOVE�����p���b�Z�[�W
'    Public MSG_txtLog_BLOCKMOVE As String
'    Public MSG_ERRTRIMVAL As String
'    Public MSG_ERRCHKMEASM As String

'    ' ���C����ʃ��x��
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

'    ' �ҏW�@[cut]
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

'    ' ���e�B�[�`���O
'    Public MSG_TEACH_LABEL01 As String
'    Public MSG_TEACH_LABEL02 As String
'    Public MSG_TEACH_LABEL03 As String
'    Public MSG_TEACH_LABEL04 As String
'    Public MSG_TEACH_LABEL05 As String

'    ' ���v���[�u�ʒu���킹
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

'    ' ��frmMsgBox ��ʏI���m�F
'    Public MSG_CLOSE_LABEL01 As String
'    Public MSG_CLOSE_LABEL02 As String
'    Public MSG_CLOSE_LABEL03 As String
'    ' ��frmReset ���_���A��ʂȂ�
'    Public MSG_FRMRESET_LABEL01 As String
'    ' ��LASER_teaching
'    Public MSG_LASER_LABEL01 As String
'    Public MSG_LASER_LABEL02 As String
'    Public MSG_LASER_LABEL03 As String                  ' ���H�����ԍ�
'    Public MSG_LASER_LABEL04 As String                  ' ���H����
'    Public MSG_LASER_LABEL05 As String                  ' Q SWITCH RATE (KHz)
'    Public MSG_LASER_LABEL06 As String                  ' STEG�{��
'    Public MSG_LASER_LABEL07 As String                  ' �d���l(mA)
'    Public MSG_LASER_LABEL08 As String                  ' ���H�����ԍ����w�肵�ĉ������B

'    Public MSG_CUT_1 As String
'    Public LBL_CUT_COPYLINE As String
'    Public LBL_CUT_COPYCOLUMN As String

'    ' ���p���[����(FL�p) ###066
'    Public MSG_AUTOPOWER_01 As String
'    Public MSG_AUTOPOWER_02 As String
'    Public MSG_AUTOPOWER_03 As String
'    Public MSG_AUTOPOWER_04 As String
'    Public MSG_AUTOPOWER_05 As String
'    Public MSG_AUTOPOWER_06 As String 'V1.16.0.0�G

'    ' �ҏW��� �J�b�g
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

'    '(�g���~���O�p�����[�^����)
'    Public TRIMPARA(150) As String

'    '(���͔͈͗p���b�Z�[�W)
'    Public INPUTAREA(8) As String
'    Public InputErr(5) As String

'    '(�m�F���b�Z�[�W)
'    Public CHKMSG(20) As String

'    '(�ҏW��ʃ{�^��)
'    Public LBL_CMD_OK As String
'    Public LBL_CMD_CANCEL As String
'    Public LBL_CMD_CLEAR As String
'    Public LBL_CMD_LCOPY As String

'    '(�ҏW�̑���������x��)
'    Public DE_EXPLANATION(20) As String
'    Public MSG_ONOFF(1) As String

'    '====================================================================
'    '* Memo *
'    '   2005/02/22      basMsgLanguage.bas�t�@�C�����f�[�^��ǉ�
'    '====================================================================
'    'TX,TY,ExCam
'    'Title
'    Public TITLE_TX As String '�`�b�v�T�C�Y(TX)�e�B�[�`���O
'    Public TITLE_TY As String '�X�e�b�v�T�C�Y(TY)�e�B�[�`���O
'    Public TITLE_ExCam As String '�O���J�����ɂ���1��R�e�B�[�`���O
'    Public TITLE_ExCam1 As String '�O���J�����ɂ��e�B�[�`���O
'    Public TITLE_T_Theta As String 'T�ƃe�B�[�`���O
'    'Frame
'    Public FRAME_TX_01 As String '��P��R��_
'    Public FRAME_TX_02 As String '��P�O���[�v�̍ŏI��R��_
'    Public FRAME_TY_01 As String '��P�u���b�N��_
'    Public FRAME_TY_02 As String '��P�O���[�v�̍ŏI�u���b�N��_
'    Public FRAME_TY_03 As String '��Q�O���[�v�̑�P�u���b�N��_
'    Public FRAME_C_VAL As String '�␳�l
'    Public FRAME_ExCam_01 As String '�u���b�N�m���D�i�͈͂͂P�`�X�X�j
'    Public FRAME_ExCam_02 As String '�e�B�[�`���O�|�C���g
'    'Command Button
'    Public CMD_CANCEL As String '�L�����Z��
'    'FlexGrid
'    Public FXGRID_TITLE01 As String 'R No.(�Œ�\��)
'    Public FXGRID_TITLE02 As String 'C No.(�Œ�\��)
'    Public FXGRID_TITLE03 As String '�X�^�[�g�|�C���g�w
'    Public FXGRID_TITLE04 As String '�X�^�[�g�|�C���g�x
'    'Label
'    Public LBL_TXTY_TEACH_01 As String '�w����W
'    Public LBL_TXTY_TEACH_02 As String '�����(��m)
'    Public LBL_TXTY_TEACH_03 As String '�␳��
'    Public LBL_TXTY_TEACH_04 As String '�␳�䗦
'    Public LBL_TXTY_TEACH_05 As String '���߻��� (mm)
'    Public LBL_TXTY_TEACH_06 As String '��ۯ�����(mm)
'    Public LBL_TXTY_TEACH_07 As String '�␳�O
'    Public LBL_TXTY_TEACH_08 As String '�␳��
'    Public LBL_TXTY_TEACH_09 As String '��ٰ�߲������(mm)
'    Public LBL_TXTY_TEACH_10 As String '��ۯ����ޕ␳(mm)
'    Public LBL_TXTY_TEACH_11 As String '�X�e�b�v�C���^�[�o��(mm)(�ǉ�)
'    Public LBL_TXTY_TEACH_12 As String '��P��_
'    Public LBL_TXTY_TEACH_13 As String '��Q��_
'    Public LBL_TXTY_TEACH_14 As String '�O���[�v
'    Public LBL_TXTY_TEACH_15 As String '�p�x�␳(deg)
'    Public LBL_Ex_Cam_01 As String      '�w��
'    Public LBL_Ex_Cam_02 As String      '�x��
'    'Info
'    Public INFO_MSG01 As String '��P��R��_�����킹�Ă���START�L�[�������ĉ�����
'    Public INFO_MSG02 As String '�ŏI��R��_�����킹�Ă���START�L�[�������ĉ�����
'    Public INFO_MSG03 As String '�␳�l���m�F����START�L�[���������Ă�������
'    Public INFO_MSG04 As String '��P�u���b�N��_�����킹�Ă���START�L�[�������ĉ�����
'    Public INFO_MSG05 As String '�ŏI�u���b�N��_�����킹�Ă���START�L�[�������ĉ�����
'    Public INFO_MSG06 As String '��Q�O���[�v�̑�P�u���b�N��_�����킹�Ă���START�L�[�������ĉ�����
'    Public INFO_REC As String '�o�^:[START]  �L�����Z��:[RESET]
'    Public INFO_ST As String '�J�n:[START]  ���~:[STOP]
'    Public INFO_MSG07 As String '�R���\�[���̖��L�[�ňʒu�����킹�܂�
'    Public INFO_MSG08 As String '�ʒu����F[START]�@���~�F[RESET]
'    Public INFO_MSG09 As String '�e�B�[�`���O���I�����܂�
'    Public INFO_MSG10 As String '[START]�L�[�������ĉ������B[HALT]�L�[�łP�O�̃f�[�^�ɖ߂�܂�
'    Public INFO_MSG11 As String '�u���b�N������͂��ĉ������B[START]�L�[�������Ǝ��̏����֐i�݂܂�
'    Public INFO_MSG12 As String '�u���b�N������͂��ĉ������B�t�����g�J�o�[�����Ǝ��̏����֐i�݂܂�

'    '(TXTY�d�l�ύX�Ή�)
'    Public INFO_MSG13 As String '"�`�b�v�T�C�Y�@�e�B�[�`���O"
'    Public INFO_MSG14 As String '"�X�e�b�v�ԃC���^�[�o���@�e�B�[�`���O"��"�X�e�[�W�O���[�v�Ԋu�e�B�[�`���O"
'    Public INFO_MSG15 As String '"�X�e�b�v�I�t�Z�b�g�ʁ@�e�B�[�`���O"
'    Public INFO_MSG16 As String '"��ʒu�����킹�ĉ������B"
'    Public INFO_MSG17 As String '"�ړ�:[���]  ����:[START]  ���f:[RESET]" & vbCrLf & "[HALT]�łP�O�̏����ɖ߂�܂��B"
'    Public INFO_MSG18 As String '"��1�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"
'    Public INFO_MSG19 As String '"��"
'    Public INFO_MSG20 As String '"�O���[�v�A�ŏI��R��ʒu�̃e�B�[�`���O"
'    Public INFO_MSG21 As String '"�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"
'    Public INFO_MSG22 As String '"�ŏI�O���[�v�A�ŏI��R�ʒu�̃e�B�[�`���O"
'    Public INFO_MSG23 As String '"�O���[�v�ԃC���^�[�o���@�e�B�[�`���O"��"�a�o�O���[�v�Ԋu�e�B�[�`���O"
'    Public INFO_MSG24 As String '"��"
'    Public INFO_MSG25 As String '"�u���b�N�̃e�B�[�`���O
'    Public INFO_MSG26 As String '"�ŏI�u���b�N
'    Public INFO_MSG27 As String '"T�ƃe�B�[�`���O
'    Public INFO_MSG28 As String '"�O���[�v�A�ŏI�[�ʒu�̃e�B�[�`���O"
'    Public INFO_MSG29 As String '"�O���[�v�A�Ő�[�ʒu�̃e�B�[�`���O"
'    Public INFO_MSG30 As String '"�T�[�L�b�g�Ԋu�e�B�[�`���O"
'    Public INFO_MSG31 As String '"�X�e�b�v�I�t�Z�b�g�ʂ̃e�B�[�`���O"
'    Public INFO_MSG32 As String '"(�s�w)"   '###084
'    Public INFO_MSG33 As String '"(�s�x)"   '###084
'    '----- V1.19.0.0-33�� -----
'    Public ERR_PROB_EXE As String   '�L���Ȓ�R(1-999)�f�[�^���Ȃ����߂��̃R�}���h�͎��s�ł��܂���I
'    Public ERR_TEACH_EXE As String  '�L���Ȓ�R(1-999)���̓}�[�L���O��R�f�[�^���Ȃ����߂��̃R�}���h�͎��s�ł��܂���I
'    '----- V1.19.0.0-33�� -----

'    'Err
'    Public ERR_TXNUM_E As String '��R�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I
'    Public ERR_TXNUM_B As String '�u���b�N�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I
'    Public ERR_TXNUM_C As String '�T�[�L�b�g�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I
'    Public ERR_TXNUM_S As String '�X�e�b�v����񐔂�0�̂��߂��̃R�}���h�͎��s�ł��܂���I'V1.13.0.0�B

'    'OperationLogging
'    Public MSG_OPLOG_TX_START As String                         'TXè��ݸފJ�n
'    Public MSG_OPLOG_TY_START As String                         'TYè��ݸފJ�n
'    Public MSG_OPLOG_TY2_START As String                        'TY2è��ݸފJ�n
'    Public MSG_OPLOG_ExCam_START As String                      '�O���J�����e�B�[�`���O�J�n
'    Public MSG_OPLOG_ExCam1_START As String                     '�O���J�����e�B�[�`���O�J�n
'    Public MSG_OPLOG_CitTx_START As String                      '�T�[�L�b�gTX�e�B�[�`���O�J�n
'    Public MSG_OPLOG_CitTe_START As String                      '�T�[�L�b�g�e�B�[�`���O�J�n
'    Public MSG_OPLOG_T_THETA_START As String                    'T��è��ݸފJ�n
'    'flg
'    'Public ExCamTeach_Type As Short                             ' (0)��P��R�̂݁@(1)�S��R
'    'Private FmMsgbox As frmMsgbox
'    Public FRAME_PITCH As String                                ' XYZ/BP �s�b�`���ݒ�
'    Public LBL_PICTH(2) As String                               ' (0)LOW (1)HIGH (2)PAUSE

'    ' OperationLogging
'    Public MSG_OPLOG_CUT_POS_CORRECT_START As String ' ��Ĉʒu�␳�J�n
'    Public MSG_OPLOG_CALIBRATION_START As String ' �����ڰ��݊J�n
'    Public MSG_OPLOG_CUT_POS_CORRECT_RECOG_START As String ' ��Ĉʒu�␳�摜�o�^�J�n
'    Public MSG_OPLOG_CALIBRATION_RECOG_START As String ' �����ڰ��݉摜�o�^�J�n

'    ' ��Ĉʒu�␳��ʕ�����
'    Public FRM_CUT_POS_CORRECT_TITLE As String ' ��Ĉʒu�␳�ڰ�����
'    Public LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER As String ' ��ٰ�ߓ���R��
'    Public LBL_CUT_POS_CORRECT_BLOCK_NO_X As String ' ��ۯ��ԍ�X��
'    Public LBL_CUT_POS_CORRECT_BLOCK_NO_Y As String ' ��ۯ��ԍ�Y��
'    Public LBL_CUT_POS_CORRECT_OFFSET_X As String ' ��Ĉʒu�␳�̾��X
'    Public LBL_CUT_POS_CORRECT_OFFSET_Y As String ' ��Ĉʒu�␳�̾��Y

'    ' �����ڰ��݉�ʕ�����
'    Public FRM_CALIBRATION_TITLE As String ' �����ڰ����ڰ�����
'    Public LBL_CALIBRATION_STANDERD_COORDINATES1X As String ' �����ڰ��݊���W1X
'    Public LBL_CALIBRATION_STANDERD_COORDINATES1Y As String ' �����ڰ��݊���W1Y
'    Public LBL_CALIBRATION_STANDERD_COORDINATES2X As String ' �����ڰ��݊���W2X
'    Public LBL_CALIBRATION_STANDERD_COORDINATES2Y As String ' �����ڰ��݊���W2Y
'    Public LBL_CALIBRATION_TABLE_OFFSETX As String ' �����ڰ���ð��ٵ̾��X
'    Public LBL_CALIBRATION_TABLE_OFFSETY As String ' �����ڰ���ð��ٵ̾��Y
'    Public LBL_CALIBRATION_GAP1X As String ' �����ڰ��݂����1X
'    Public LBL_CALIBRATION_GAP1Y As String ' �����ڰ��݂����1Y
'    Public LBL_CALIBRATION_GAP2X As String ' �����ڰ��݂����2X
'    Public LBL_CALIBRATION_GAP2Y As String ' �����ڰ��݂����2Y
'    Public LBL_CALIBRATION_GAINX As String ' �����ڰ��ݹ޲ݕ␳�W��X
'    Public LBL_CALIBRATION_GAINY As String ' �����ڰ��ݹ޲ݕ␳�W��Y
'    Public LBL_CALIBRATION_OFFSETX As String ' �����ڰ��ݵ̾�ĕ␳��X
'    Public LBL_CALIBRATION_OFFSETY As String ' �����ڰ��ݵ̾�ĕ␳��Y
'    Public LBL_CALIBRATION_001 As String '�y�\���J�b�g���[�h(����W�P)�z
'    Public LBL_CALIBRATION_002 As String '�y�\���J�b�g���[�h(����W�Q)�z
'    Public LBL_CALIBRATION_003 As String '�y�摜�F�����[�h(����W�P)�z
'    Public LBL_CALIBRATION_004 As String '�y�摜�F�����[�h(����W�Q)�z

'    ' Recog
'    Public LBL_CALIBRATION_STANDERD_COORDINATES1 As String ' �����ڰ��݊���W1RECOG
'    Public LBL_CALIBRATION_STANDERD_COORDINATES2 As String ' �����ڰ��݊���W2RECOG
'    Public LBL_CALIBRATION_TABLE_OFFSET As String ' �����ڰ���ð��ٵ̾��RECOG
'    Public LBL_CALIBRATION_CUT As String ' �����ڰ��ݶ��RECOG
'    Public LBL_CALIBRATION_MOVE1 As String ' ����W�P�ֈړ�
'    Public LBL_CALIBRATION_MOVE2 As String ' ����W�Q�ֈړ�
'    Public LBL_CUT_POS_CORRECT_CUT As String ' �\�����
'    Public LBL_CUT_POS_CORRECT_MOVE As String ' �O����ׂֈړ�

'    ' ү���ޕ�����
'    Public MSG_CUT_POS_CORRECT_001 As String ' ��ۯ��ԍ��͂P�`�X�X�œ��͂��ĉ������B
'    Public MSG_CUT_POS_CORRECT_002 As String ' ��ۯ�No���͌�A[��ۯ��ړ�]�������Ă��������B
'    Public MSG_CUT_POS_CORRECT_003 As String ' �������J�n���܂��B" & vbCrLf & "[START]���s" & vbCrLf & "[RESET]���~
'    Public MSG_CUT_POS_CORRECT_004 As String ' �x��" & vbCrLf & "[START]�\���J�b�g���s" & vbCrLf & "[RESET]���~"
'    Public MSG_CUT_POS_CORRECT_005 As String ' �\���J�b�g���s��" & vbCrLf & "[ADJ]�ꎞ��~"
'    Public MSG_CUT_POS_CORRECT_006 As String ' [START]�\���J�b�g���s" & vbCrLf & "[RESET]���~" & vbCrLf & "[ADJ]�ꎞ��~����"
'    Public MSG_CUT_POS_CORRECT_007 As String ' [START]�摜�F�����s" & vbCrLf & "[RESET]���~"
'    Public MSG_CUT_POS_CORRECT_008 As String ' �摜�F�����s��" & vbCrLf & "[ADJ]�ꎞ��~"
'    Public MSG_CUT_POS_CORRECT_009 As String ' [START]�摜�F�����s" & vbCrLf & "[RESET]���~" & vbCrLf & "[ADJ]�ꎞ��~����"
'    Public MSG_CUT_POS_CORRECT_010 As String ' �␳�l�X�V�G���[" & vbCrLf & "�X�V���ʂ��͈͂𒴂��Ă��܂��B" & vbCrLf & "�ő�l�A�ŏ��l�ɕ␳����܂��B"
'    Public MSG_CUT_POS_CORRECT_011 As String ' �\���J�b�g�I��
'    Public MSG_CUT_POS_CORRECT_012 As String ' �摜�F���I��"
'    Public MSG_CUT_POS_CORRECT_013 As String ' �摜�}�b�`���O�G���["
'    Public MSG_CUT_POS_CORRECT_014 As String ' "�u���b�N������͂��ĉ�����" & vbCrLf & "[START]�L�[�������Ǝ��̏����֐i�݂܂�"
'    Public MSG_CUT_POS_CORRECT_015 As String ' "�u���b�N�ԍ�����̓G���["
'    Public MSG_CUT_POS_CORRECT_016 As String ' "�摜�F�����s��"
'    Public MSG_CUT_POS_CORRECT_017 As String ' "���X�C�b�`:�ʒu����, [START]:����, [HALT]:�O��"
'    Public MSG_CUT_POS_CORRECT_018 As String ' "���֌W��="
'    Public MSG_CUT_POS_CORRECT_019 As String ' �摜��������܂���"

'    Public MSG_CALIBRATION_001 As String ' �L�����u���[�V���������s���܂��B" & vbCrLf & "[START]�������Ă��������B" & vbcrlf & "[RESET]CANCEL"
'    Public MSG_CALIBRATION_002 As String ' �O���J�����ł���ʂ����o���܂��B" & vbCrLf & "[START]�������Ă��������B" & vbCrLf & "[RESET]CANCEL"
'    Public MSG_CALIBRATION_003 As String ' "[START�L�[]�F�摜�F�����s�C[RESET�L�[]�F���~"
'    Public MSG_CALIBRATION_004 As String ' "�O���J�����ł���ʂ����o���܂��B(����W�P)" & vbCrLf & "[START�L�[]�F����C[RESET�L�[]�F���~"
'    Public MSG_CALIBRATION_005 As String ' "�O���J�����ł���ʂ����o���܂��B(����W�Q)" & vbCrLf & "[START�L�[]�F����C[RESET�L�[]�F���~"
'    Public MSG_CALIBRATION_006 As String ' "�L�����u���[�V�������I�����܂��B" & "�f�[�^��ێ�����ꍇ��[START]���A�f�[�^��ێ����Ȃ��ꍇ��[RESET]�������ĉ������B"
'    Public MSG_CALIBRATION_007 As String ' "�摜�}�b�`���O�G���[" & vbCrLf & "�����𑱂���ꍇ��[OK]���A���~����ꍇ��[Cancel]�������ĉ������B"
'    Public MSG_CALIBRATION_008 As String ' "Reset���܂��B [Start]:Reset���s , [Reset]:Cancel���s "      ' ###078  

'    ' �������^�]..
'    Public MSG_AUTO_01 As String '���샂�[�h
'    Public MSG_AUTO_02 As String '�}�K�W�����[�h
'    Public MSG_AUTO_03 As String '���b�g���[�h
'    Public MSG_AUTO_04 As String '�G���h���X���[�h
'    Public MSG_AUTO_05 As String '�f�[�^�t�@�C��
'    Public MSG_AUTO_06 As String '�o�^�ς݃f�[�^�t�@�C��
'    Public MSG_AUTO_07 As String '���X�g��1���
'    Public MSG_AUTO_08 As String '���X�g��1����
'    Public MSG_AUTO_09 As String '���X�g����폜
'    Public MSG_AUTO_10 As String '���X�g���N���A
'    Public MSG_AUTO_11 As String '�o�^
'    Public MSG_AUTO_12 As String 'OK
'    Public MSG_AUTO_13 As String '�L�����Z��
'    Public MSG_AUTO_14 As String '�f�[�^�I��'
'    Public MSG_AUTO_15 As String '�o�^���X�g��S�č폜���܂��B
'    Public MSG_AUTO_16 As String '��낵���ł����H
'    Public MSG_AUTO_17 As String '�G���h���X���[�h���͕����̃f�[�^�t�@�C���͑I���ł��܂���B
'    Public MSG_AUTO_18 As String '�f�[�^�t�@�C����I�����Ă��������B
'    Public MSG_AUTO_19 As String '�ҏW���̃f�[�^��ۑ����܂����H
'    Public MSG_AUTO_20 As String '���H�����t�@�C�������݂��܂���B

'    ' ���ƕ␳..
'    Public MSG_THETA_01 As String '�␳�ʒu�P
'    Public MSG_THETA_02 As String '�␳�ʒu�Q
'    Public MSG_THETA_03 As String '�␳�l
'    Public MSG_THETA_04 As String 'START
'    Public MSG_THETA_05 As String 'CANCEL
'    Public MSG_THETA_06 As String '�w����W
'    Public MSG_THETA_07 As String '�����
'    Public MSG_THETA_08 As String '��]�␳�p�x
'    Public MSG_THETA_09 As String '�␳�ʒu1
'    Public MSG_THETA_10 As String '�␳�ʒu2
'    Public MSG_THETA_11 As String '�g�����ʒu
'    Public MSG_THETA_12 As String '�␳��
'    Public MSG_THETA_13 As String '�␳�ʒu1�@�ړ���
'    Public MSG_THETA_14 As String '�␳�ʒu2�@�ړ���
'    Public MSG_THETA_15 As String '�␳�ʒu�P�����킹�Ă���START�L�[�������Ă�������
'    Public MSG_THETA_16 As String '�␳�ʒu�Q�����킹�Ă���START�L�[�������Ă�������
'    Public MSG_THETA_17 As String '�␳�l���m�F����START�L�[�������Ă�������
'    Public MSG_THETA_18 As String 'Matching error
'    Public MSG_THETA_19 As String 'Pattern Matching error(Position1)
'    Public MSG_THETA_20 As String 'Pattern Matching error(Position1)
'    Public MSG_THETA_21 As String 'Pattern Matching OK(Position1)
'    Public MSG_THETA_22 As String 'Pattern Matching
'    ' �����ݸ�..
'    Public MSG_TRIM_01 As String '�͉��H�͈͊O�ł��B
'    Public MSG_TRIM_02 As String '���[�_�[�����^�]�̋N�����ł��܂���ł����B
'    Public MSG_TRIM_03 As String '�����𑱍s���܂����H
'    Public MSG_TRIM_04 As String '�C�j�V�����e�X�g�@���z�}
'    Public MSG_TRIM_05 As String '�t�@�C�i���e�X�g�@���z�}
'    Public MSG_TRIM_06 As String '�����|�W�V����
'    Public BTN_TRIM_01 As String '�����ް� �ر
'    Public BTN_TRIM_02 As String '��ڰ��ް��ҏW
'    Public BTN_TRIM_03 As String '�Ƽ��ýĕ��z�\��
'    Public BTN_TRIM_04 As String '̧���ýĕ��z�\��
'    Public PIC_TRIM_01 As String '�C�j�V�����e�X�g�@���z�}
'    Public PIC_TRIM_02 As String '�t�@�C�i���e�X�g�@���z�}
'    Public PIC_TRIM_03 As String '�Ǖi
'    Public PIC_TRIM_04 As String '�s�Ǖi
'    Public PIC_TRIM_05 As String '�ŏ�%
'    Public PIC_TRIM_06 As String '�ő�%
'    Public PIC_TRIM_07 As String '����%
'    Public PIC_TRIM_08 As String '�W���΍�
'    Public PIC_TRIM_09 As String '��R��
'    Public PIC_TRIM_10 As String '���z�}�ۑ� 
'    ' ��۰�ް..
'    Public MSG_LOADER_01 As String '�S��~�ُ픭����
'    Public MSG_LOADER_02 As String '���ْ�~�ُ픭����
'    Public MSG_LOADER_03 As String '�y�̏ᔭ����
'    Public MSG_LOADER_04 As String '�ݒ肪�������܂����B
'    Public MSG_LOADER_05 As String '۰�ް�װ
'    Public MSG_LOADER_06 As String '۰�ް����ۯ�
'    Public MSG_LOADER_07 As String '�p�^�[���}�b�`���O�G���[
'    Public MSG_LOADER_08 As String '�A��NG-HI�װ
'    Public MSG_LOADER_09 As String '�t���p���[���� Q Rate 10kHz
'    Public MSG_LOADER_10 As String '���[�U�[�p���[�΂���G���[
'    Public MSG_LOADER_11 As String '�t���p���[�����G���[
'    Public MSG_LOADER_12 As String '�����t���p���[����
'    Public MSG_LOADER_13 As String '�p���[����
'    Public MSG_LOADER_14 As String '���[�U�[�p���[�����G���[
'    Public MSG_LOADER_15 As String '�����^�]�I��
'    Public MSG_LOADER_16 As String '۰�ް�װ�ؽ�
'    Public MSG_LOADER_17 As String '�ڕ����Ɋ���c���Ă���ꍇ��
'    Public MSG_LOADER_18 As String '��菜���Ă�������
'    Public MSG_LOADER_19 As String '��i�킪�ς��܂���                          ' ###089
'    Public MSG_LOADER_20 As String '�m�f�r�o�{�b�N�X����m�f�����菜���Ă���    ' ###089
'    Public MSG_LOADER_21 As String '�m�f�r�o�{�b�N�X���t                            ' ###089
'    Public MSG_LOADER_22 As String 'START�L�[����OK�{�^�������Ō��_���A���܂��B     ' ###124
'    Public MSG_LOADER_23 As String '�����^�]�𒆎~���܂�                            ' ###124
'    Public MSG_LOADER_24 As String '�ڕ���N�����v����                              ' ###144
'    Public MSG_LOADER_25 As String '�ڕ���z������                                  ' ###144
'    Public MSG_LOADER_26 As String '�n���h�z������                                  ' ###144
'    Public MSG_LOADER_27 As String '�����n���h�z������                              ' ###158
'    Public MSG_LOADER_28 As String '���[�n���h�z������                              ' ###158
'    Public MSG_LOADER_29 As String '���u���Ɋ���c���Ă���ꍇ��                  ' ###158
'    Public MSG_LOADER_30 As String '�����^�]��~��                            �@�@�@' ###172
'    Public MSG_LOADER_31 As String 'OK�{�^�������ŃA�v���P�[�V�������I�����܂�      ' ###175
'    Public MSG_LOADER_32 As String '�ڕ���̊����菜���Ă���"                   ' ###184 
'    Public MSG_LOADER_33 As String '�ēx�����^�]�����s���ĉ�����"                   ' ###184 
'    Public MSG_LOADER_34 As String '�}�K�W��������o�Z���T�n�e�e�ʒu�܂�"         ' ###184 
'    Public MSG_LOADER_35 As String '�����Ă�������"                                 ' ###184 
'    Public MSG_LOADER_36 As String '�ڕ���̊����菜���ĉ�����"                 ' ###184 
'    '----- ###188�� -----
'    Public MSG_LOADER_37 As String '�ڕ����Ɋ���c���Ă��܂�" 
'    Public MSG_LOADER_38 As String 'START�L�[����OK�{�^��������"                 
'    Public MSG_LOADER_39 As String '�X�e�[�W�����_�ɖ߂��܂�"                 
'    '----- ###188�� -----
'    Public MSG_LOADER_40 As String '�m�f�����菜���ĉ�����"                     ' ###197     
'    Public MSG_LOADER_41 As String '�m�f�����菜���Ă���"                       ' ###197     
'    '----- ###240�� -----
'    Public MSG_LOADER_42 As String '�ڕ���Ɋ������܂���" 
'    Public MSG_LOADER_43 As String '���u���čēx���s���ĉ�����"                 
'    '----- ###240�� -----
'    '----- V1.18.0.0�H�� -----
'    Public MSG_LOADER_44 As String '�}�K�W���I���A�}�K�W��������START�L�[����"
'    Public MSG_LOADER_45 As String 'OK�{�^�������ŏ����𑱍s���܂�"
'    Public MSG_LOADER_46 As String 'RESET�L�[����Cancel�{�^�������ŏ������I�����܂�"
'    '----- V1.18.0.0�H�� -----
'    Public MSG_LOADER_47 As String '�v���[�u�`�F�b�N�G���[" V1.23.0.0�F
'    Public MSG_LOADER_48 As String '�T�C�N����~��" 'V4.0.0.0�R
'    Public MSG_LOADER_49 As String '�������o" 'V4.0.0.0�R
    Public MSG_LOADER_50 As String '��𓊓����Ă�������" 'V4.11.0.0�E

'    Public FRAME_TY2_01 As String 'è��ݸ��߲��
'    Public INFO_REC_TX As String '�o�^:[START]  �L�����Z��:[RESET]  �o�^+TX2:[HI+ADV]
'    Public INFO_REC_TY As String '�o�^:[START]  �L�����Z��:[RESET]  �o�^+TY2:[HI+ADV]
'    Public INFO_REC_TTHETA As String '�o�^:[START]  �L�����Z��:[RESET]
'    Public TITLE_TY2 As String '�X�e�b�v�T�C�Y(TY)�e�B�[�`���O
'    Public MSG_EXECUTE_TXTYLABEL As String 'TX,TY
'    Public LBL_STEP_STEP As String
'    Public LBL_STEP_GROUP As String
'    Public LBL_STEP_TY2 As String
'    Public LBL_ATT As String '������
'    Public LBL_FLCUR As String '�d���l

'    Public CIRTEACH_MSG00 As String '��
'    Public CIRTEACH_MSG01 As String '�T�[�L�b�g��_�w�x�����킹�Ă���START��L�[�������ĉ�����

'    Public CIRTY_MSG00 As String '��P�T�[�L�b�g��_
'    Public CIRTY_MSG01 As String '��P�O���[�v�̍ŏI�T�[�L�b�g��_
'    Public CIRTY_MSG02 As String '��Q�O���[�v�̑�P�T�[�L�b�g��_
'    Public CIRTY_MSG03 As String '����Ļ���(mm)

'    Public STEP_TITLE01 As String '�T�[�L�b�g���W
'    Public STEP_TITLE02 As String '�T�[�L�b�g�ԃC���^�[�o��
'    Public STEP_TITLE03 As String '�X�e�b�v�ԃC���^�[�o��

'    ' �����[�_�A���[�����b�Z�[�W 
'    Public MSG_LDALARM_00 As String     ' "����~"
'    Public MSG_LDALARM_01 As String     ' "�}�K�W���������A���[��"
'    Public MSG_LDALARM_02 As String     ' "���ꌇ���i����"
'    Public MSG_LDALARM_03 As String     ' "�n���h�P�z���A���[��"
'    Public MSG_LDALARM_04 As String     ' "�n���h�Q�z���A���[��"
'    Public MSG_LDALARM_05 As String     ' "�ڕ���z���Z���T�ُ�"
'    Public MSG_LDALARM_06 As String     ' "�ڕ���z���~�X"
'    Public MSG_LDALARM_07 As String     ' "���{�b�g�A���[��"
'    Public MSG_LDALARM_08 As String     ' "�H���ԊĎ��A���[��"
'    Public MSG_LDALARM_09 As String     ' "�G���x�[�^�ُ�"
'    Public MSG_LDALARM_10 As String     ' "�}�K�W������"
'    Public MSG_LDALARM_11 As String     ' "���_���A�^�C���A�E�g"
'    Public MSG_LDALARM_12 As String     ' "�N�����v�ُ�" '###125
'    Public MSG_LDALARM_13 As String     ' "�G�A�[���ቺ���o"
'    Public MSG_LDALARM_14 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_15 As String     ' "����`�A���[�� No."

'    Public MSG_LDALARM_16 As String     ' "����V�����_�^�C���A�E�g"
'    Public MSG_LDALARM_17 As String     ' "�n���h�P�V�����_�^�C���A�E�g"
'    Public MSG_LDALARM_18 As String     ' "�n���h�Q�V�����_�^�C���A�E�g"
'    Public MSG_LDALARM_19 As String     ' "�����n���h�z���~�X"
'    Public MSG_LDALARM_20 As String     ' "����n���h�z���~�X"
'    Public MSG_LDALARM_21 As String     ' "�m�f�r�o���t"
'    Public MSG_LDALARM_22 As String     ' "�ꎞ��~"
'    Public MSG_LDALARM_23 As String     ' "�h�A�I�[�v��"
'    Public MSG_LDALARM_24 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_25 As String     ' "�񖇎�茟�o"
'    Public MSG_LDALARM_26 As String     ' "�����㉺�@�\�A���[��"
'    Public MSG_LDALARM_27 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_28 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_29 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_30 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_31 As String     ' "����`�A���[�� No."
'    Public MSG_LDALARM_UD As String     ' "����`�A���[�� No."

'    Public MSG_LDGUID_00 As String     ' "����~�r�v��������܂����B"
'    Public MSG_LDGUID_01 As String     ' "�}�K�W�������K�̈ʒu���m�F���Ă��������B"
'    Public MSG_LDGUID_02 As String     ' "���ꌇ���i���w�肳�ꂽ�����������܂����B"
'    Public MSG_LDGUID_03 As String     ' "�z���Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_04 As String     ' "�z���Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_05 As String     ' "�z���Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_06 As String     ' "�z���Z���T���m�F���Ă��������B" "�g�b�v�v���[�g�ɃL�Y���̂����炪�������m�F���Ă��������B"
'    Public MSG_LDGUID_07 As String     ' "���{�b�g�A���[�����������܂����B"
'    Public MSG_LDGUID_08 As String     ' "�H���ԊĎ��Ń^�C���A�E�g���������܂����B"
'    Public MSG_LDGUID_09 As String     ' "�G���x�[�^�̃Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_10 As String     ' "�}�K�W�����o�h�O���|��Ă��邩�A�}�K�W�����Z�b�g���Ă��������B"
'    Public MSG_LDGUID_11 As String     ' "���_���A���Ƀ^�C���A�E�g���������܂����B"
'    Public MSG_LDGUID_12 As String     ' "�N�����v�ُ�" '###125
'    Public MSG_LDGUID_13 As String     ' "�G�A�[����������������Ă��邩�m�F���Ă��������B"
'    Public MSG_LDGUID_14 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_15 As String     ' "�z��O�A���[��"

'    Public MSG_LDGUID_16 As String     ' "�V�����_�Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_17 As String     ' "�V�����_�Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_18 As String     ' "�V�����_�Z���T���m�F���Ă��������B"
'    Public MSG_LDGUID_19 As String     ' "��z�����Ƀ^�C���A�E�g���������܂����B"
'    Public MSG_LDGUID_20 As String     ' "��z�����Ƀ^�C���A�E�g���������܂����B"
'    Public MSG_LDGUID_21 As String     ' "�m�f�r�o�a�n�w�����t�ł��B�����菜���Ă��������B"
'    Public MSG_LDGUID_22 As String     ' "�ꎞ��~���ł��B"
'    Public MSG_LDGUID_23 As String     ' "�h�A�I�[�v�������o����܂����B�h�A����Ă��������B"
'    Public MSG_LDGUID_24 As String     ' "�����n���h�̊����菜���čĎ��s���Ă��������B" V1.18.0.0�J
'    Public MSG_LDGUID_25 As String     ' "�����㉺�@�\���m�F���Ă��������B" 'V4.0.0.0-59
'    Public MSG_LDGUID_26 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_27 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_28 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_29 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_30 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_31 As String     ' "�z��O�A���[��"
'    Public MSG_LDGUID_UD As String     ' "�z��O�A���[��"

'    Public MSG_LDINFO_UD As String     ' "�z��O�A���[��"

'    'V1.13.0.0�E ADD START
'    ' ���h�c���[�_�[���b�Z�[�W 
'    Public LBL_IDREADER_TEACH_01 As String  ' �h�c�@���[�_�[�@�e�B�[�`���O
'    Public LBL_IDREADER_TEACH_02 As String  ' ��P �h�c�ǂݎ��ʒu
'    Public LBL_IDREADER_TEACH_03 As String  ' ��Q �h�c�ǂݎ��ʒu
'    Public LBL_IDREADER_TEACH_04 As String  ' �h�c���[�_�[
'    'V1.13.0.0�E ADD END
'    'V2.0.0.0�P
'    ' ���v���[�u�N���[�j���O���b�Z�[�W 
'    Public LBL_PROBECLEANING_TEACH As String ' �v���[�u�N���[�j���O�ʒu�e�B�[�`���O

'#End Region

'#Region "���b�Z�[�W��ݒ肷��(���{��/�p��)"
'    '''=========================================================================
'    '''<summary>���b�Z�[�W��ݒ肷��(���{��/�p��)</summary>
'    '''<param name="language">(INP)����(0:Japanese, 1:English, 2:Europe)</param>
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

'#Region "���{�ꃁ�b�Z�[�W��ݒ肷��"
'    '''=========================================================================
'    '''<summary>���{�ꃁ�b�Z�[�W��ݒ肷��</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesJapanese()

'        '-----------------------------------------------------------------------
'        '   �G���[���b�Z�[�W
'        '-----------------------------------------------------------------------
'        MSG_0 = "���łɂ��̃v���O�����͎��s����Ă��܂�"
'        MSG_11 = "%1�s�ڂƒ�RNO���d�����Ă��܂�"
'        MSG_14 = "�g���~���O�p�����[�^�f�[�^�����[�h���邩�V�K�쐬���Ă�������"
'        MSG_15 = "�w�肳�ꂽ�t�@�C���͑��݂��܂���"
'        MSG_16 = "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
'        MSG_21 = "�u���b�N�T�C�Y X �𐳂������͂��Ă�������"
'        MSG_22 = "�u���b�N�T�C�Y Y �𐳂������͂��Ă�������"
'        MSG_25 = "���샂�[�h�i�f�W�X�C�b�`�j�̐ݒ�� x0�`x6 �ɂ��Ă�������"
'        MSG_36 = "���H�͈͊O�ł�"
'        MSG_37 = "�Ώۂ��N���X���C���̒��S�ɍ��킹�āA�I��̈悪�N���X���C���̒��S���܂ނ悤�ɂ��Ă�������"
'        MSG_38 = "�p�^�[���o�^�̃e���v���[�g�O���[�v�ԍ��iPP37')���m�F���Ă�������"
'        MSG_39 = "�J�b�g�ʒu�␳�Ώۂ̒�R������܂���"
'        MSG_40 = "���~�����������ňꕔ�̃v���[�u�����ڐG���܂���ł���"
'        MSG_41 = "���~���������ɒB���܂����B���������͂ł��܂���ł���"
'        MSG_42 = "Z���̉������~�b�g�ɒB���܂����B���������͂ł��܂���ł����@"
'        MSG_43 = "�Ċm�F���ɔ�ڐG�����m���܂����B���������͂ł��܂���ł����@"
'        MSG_44 = "���������͊������܂����@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_45 = "���H�����f�[�^�����[�h���Ă��������@�@�@�@�@�@�@�@�@�@�@�@�@ "
'        MSG_46 = "���H����"

'        MSG_101 = "���㏑�����܂����@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_102 = "�A�v���P�[�V�������I�����Ă�낵���ł����H�@�@�@�@�@�@�@�@�@"
'        MSG_103 = "��낵���ł����H�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_104 = "�ҏW�����f�[�^���ۑ�����Ă��܂���@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_105 = "�O�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_106 = "���̏���ۑ����đO�̉�ʂɖ߂�܂��B��낵���ł����H�@�@�@"
'        MSG_108 = "�݌v���N���A���Ă���낵���ł����H�@�@�@�@�@�@�@�@�@�@�@�@�@"

'        MSG_114 = "�W�o�@�ُ킪�������܂����@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_115 = "�ҏW���̃f�[�^������܂��B���[�h���܂����H�@�@�@�@�@�@�@�@�@"
'        MSG_116 = "�f�[�^���[�h�m�F�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_117 = "�ҏW���̃f�[�^������܂��B�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_118 = "�␳���[�h�������ɐݒ肳��Ă���ꍇ�́A���s�ł��܂���B�@�@"
'        MSG_119 = "���p�p��1�`18�����ȓ��œ��͂��ĉ�����"
'        MSG_120 = "�L�����u���[�V�������I�����܂��B"
'        MSG_121 = "�f�[�^��ێ�����ꍇ�́u�͂��v���A�f�[�^��ێ����Ȃ��ꍇ�́u�������v�������ĉ������B"
'        MSG_122 = "��Ľ�߰�ނ̏���l���s���ł��B" & vbCrLf & vbCrLf & "ST2��Ă��܂ޏꍇ�A��Ľ�߰�ނ̏���l��%1mm/s�ł��B" & vbCrLf & "(�Ώۂ́AST2��Ă��O�̶�đS�Ăł��B)"
'        MSG_126 = "�p�^�[���F���G���[�ł��B�蓮���[�h�Ŏ��s���܂����H"
'        MSG_127 = "�p�^�[���F���G���[" 'V1.13.0.0�B

'        MSG_130 = "�g���~���O�f�[�^�t�@�C�����̓G���["
'        MSG_131 = "�g���~���O�f�[�^�t�@�C���o�̓G���["
'        MSG_132 = "���H�����̑��M�Ɏ��s���܂����B" + vbCrLf + "�ēx�f�[�^�����[�h���邩�A�ҏW��ʂ�����H�����̐ݒ���s���Ă��������B"
'        MSG_133 = "���ԃt�@�C���̓Ǎ��݂Ɏ��s���܂����B"
'        MSG_134 = "���ԃt�@�C���̏������݂Ɏ��s���܂����B"
'        MSG_135 = "�f�[�^�ҏW�G���[�B�f�[�^���ă��[�h���Ă��������B"
'        MSG_136 = "�V���A���|�[�g�n�o�d�m�G���["
'        MSG_137 = "�V���A���|�[�g�b�k�n�r�d�G���["
'        MSG_138 = "�V���A���|�[�g���M�G���["
'        MSG_139 = "�V���A���|�[�g��M�G���["
'        MSG_140 = "�e�k���̉��H�����̐ݒ肪����܂���B" + vbCrLf + "�ēx�f�[�^�����[�h���邩�A�ҏW��ʂ�����H�����̐ݒ���s���Ă��������B"
'        MSG_141 = "�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
'        MSG_142 = "���H�����t�@�C�����쐬���܂���"
'        MSG_143 = "�f�[�^�����[�h���܂���"
'        MSG_144 = "�f�[�^���[�h�m�f"
'        MSG_145 = "�f�[�^���Z�[�u���܂���"
'        MSG_146 = "�f�[�^�Z�[�u�m�f"
'        MSG_147 = "�e�k�։��H�����𑗐M���܂����B"
'        MSG_148 = "�e�k�փf�[�^���M���E�E�E�E�E�E"
'        MSG_149 = "�t�@�C�o���[�U�̉��H�������ύX����Ă���\��������܂��B���[�h���܂����H�@�@�@�@�@�@�@�@�@"
'        MSG_150 = "�e�k�ʐM�ُ�B�e�k�Ƃ̒ʐM�Ɏ��s���܂����B" + vbCrLf + "�e�k�Ɛ������ڑ��ł��Ă��邩�m�F���Ă��������B"
'        MSG_151 = "���H�����̐ݒ�Ɏ��s���܂����B"
'        '----- V1.18.0.0�B�� -----
'        MSG_152 = "������������Ɏ��s���܂����B"
'        MSG_153 = "��������Ɏ��s���܂����B"
'        MSG_154 = "����I�������Ɏ��s���܂����B"
'        '----- V1.18.0.0�B�� -----
'        MSG_155 = "��������N���A���Ă���낵���ł����H" 'V1.18.0.0�K
'        MSG_156 = "�v���[�u�`�F�b�N�G���["                 'V1.23.0.0�F
'        MSG_157 = "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂���" 'V1.23.0.0�@
'        MSG_158 = "���H�����t�@�C�����[�h�G���[�B" 'V2.0.0.0�D
'        MSG_159 = "�t�@�C�������݂��܂��B�㏑�����Ă��X�����ł����H" 'V4.0.0.0�H
'        '----- V4.0.0.0-30�� -----
'        MSG_160 = "�v���[�u�N���[�j���O�̃X�e�[�W�ʒu�����킹�ĉ������B"
'        MSG_161 = "�ړ��F[���]  ����F[START]  ���f�F[RESET]"
'        MSG_162 = "�v���[�u�N���[�j���O�̃v���[�u�ʒu�����킹�ĉ������B"
'        '----- V4.0.0.0-30�� -----

'        MSGERR_COVER_CLOSE = "�X���C�h�J�o�[�̎����N���[�Y�Ɏ��s���܂����B"
'        MSGERR_SEND_TRIMDATA = "�g���~���O�f�[�^�̐ݒ�Ɏ��s���܂����B" & vbCrLf & "�g���~���O�f�[�^�ɖ�肪�Ȃ����m�F���Ă��������B"

'        MSG_PP23 = "�v���[�uZ�I�t�Z�b�g�̋�����Z���̏㏸���������傫�����Ă�������"
'        MSG_PP24_1 = "�v���[�uZ���̏㏸������Z���ҋ@�ʒu�̋��������z���Ă��܂�"
'        MSG_PP24_2 = "Z���̏㏸���������A�ҋ@�ʒu�̋�����傫�����Ă�������"

'        '''' NET�̏ꍇ
'        MSG_TOTAL_CIRCUIT = "�T�[�L�b�g�P��"
'        '''' CHIP�̏ꍇ
'        MSG_TOTAL_REGISTOR = "��R�P��"

'        MSG_COM = "���l���͔͈̓G���[�i%1�`%2�j"
'        MSG_PR_COM1 = "���l���͔͈̓G���[�i%1�`%2�j"
'        MSG_PR04_1 = "%1�Ɠ����ԍ����w�肳��܂����B"
'        '
'        LOAD_MSG01 = "�w�肵���t�@�C����������܂���B"

'        MSG_CUT_1 = "�R�s�[�ł���͓̂����R�ԍ��Ԃ݂̂ł��B"

'        '(�f�[�^�ҏW��ʂ̃G���[MSG)
'        DATAEDIT_ERRMSG01 = "�RESISTOR��̏������s���ĉ�����"
'        DATAEDIT_ERRMSG02 = "�CUT��̏������s���ĉ�����"
'        DATAEDIT_ERRMSG03 = "���̓G���["
'        DATAEDIT_ERRMSG04 = "%1��%2���d�����Ă��܂�"
'        DATAEDIT_ERRMSG05 = "�p���X�����Ԃ� "
'        DATAEDIT_ERRMSG06 = " ���Őݒ肵�ĉ�����"
'        DATAEDIT_ERRMSG07 = "�p���[�g�̍Đݒ�"
'        DATAEDIT_ERRMSG08 = "�����_�.���1��ȏ���͂���Ă��܂�"
'        DATAEDIT_ERRMSG09 = "Q���[�g�̗��z�l�́u"
'        DATAEDIT_ERRMSG10 = "�v�ł��B"
'        DATAEDIT_ERRMSG11 = "���z�l�ɐݒ肵�܂����H"

'        INPUTAREA(0) = "���l�� "
'        INPUTAREA(1) = " �͈̔͂œ��͂��Ă�������"
'        INPUTAREA(2) = "���̓G���[ ["
'        INPUTAREA(3) = "]"
'        INPUTAREA(4) = "��d�o�^�G���[ ["
'        INPUTAREA(5) = "%1�s�ڂƃX�e�b�v�ԍ����d�����Ă��܂�"
'        INPUTAREA(6) = "�u���b�N���̍��v�� "
'        INPUTAREA(7) = " �ƂȂ�悤�ɐݒ肵�Ă�������"
'        INPUTAREA(8) = " XY�e�[�u���̉��͈͂𒴂��Ă��܂�"

'        CHKMSG(0) = "�u���b�N���̃f�[�^�̓N���A����܂�"
'        CHKMSG(1) = "�ʒu�␳���@�̃f�[�^�̓N���A����܂�"
'        CHKMSG(2) = "�␳�ʒu�P�`��]���S�܂ł̃f�[�^�̓N���A����܂�"
'        CHKMSG(3) = "��낵���ł����H"
'        CHKMSG(4) = "�m�F"
'        CHKMSG(5) = "�ҏW�f�[�^���N���A����ƁA�V�K�쐬���̏�Ԃɂ��ǂ�܂�"
'        CHKMSG(6) = "���s���܂����H"
'        CHKMSG(7) = "�폜���ꂽ�J�b�g�f�[�^�̓N���A����܂�"
'        CHKMSG(8) = "�m�f�}�[�L���O�̃f�[�^�͏�������܂�"
'        CHKMSG(9) = "�p���[�������[�h�̃f�[�^�͏�������܂�"
'        CHKMSG(10) = "�p���X�����Ԃ��@"
'        CHKMSG(11) = "�@�Őݒ肵�Ă�������"

'        '-----------------------------------------------------------------------
'        '   �^�C�g�����b�Z�[�W
'        '-----------------------------------------------------------------------
'        TITLE_1 = "�폜�m�F"
'        TITLE_2 = "��d�o�^�G���["
'        TITLE_3 = "���̓f�[�^�͈͊O"
'        TITLE_4 = "���~"
'        TITLE_5 = "�t�@�C���̊m�F"
'        TITLE_6 = "�K�{���ڃG���["
'        TITLE_7 = "�m�F"
'        TITLE_8 = "���͍��ڃG���["
'        TITLE_LASER = "���[�U�[����"
'        TITLE_LOGGING = "���M���O����"

'        '-----------------------------------------------------------------------
'        '   ���x�����b�Z�[�W
'        '-----------------------------------------------------------------------
'        ' ���C����ʃ��x��
'        MSG_MAIN_LABEL01 = "�����="
'        MSG_MAIN_LABEL02 = "NG��="
'        MSG_MAIN_LABEL03 = "IT H NG��="
'        MSG_MAIN_LABEL04 = "IT L NG��="
'        MSG_MAIN_LABEL05 = "FT H NG��="
'        MSG_MAIN_LABEL06 = "FT L NG��="
'        MSG_MAIN_LABEL07 = "OVER NG��="

'        ' �ꎞ��~��ʗp ###014
'        LBL_FINEADJ_001 = "�f�[�^�ҏW"
'        '----- ###204�� ----- 
'        LBL_FINEADJ_002 = "����"
'        LBL_FINEADJ_003 = "�O�F�\���Ȃ�"
'        LBL_FINEADJ_004 = "�P�F�m�f�̂ݕ\��"
'        LBL_FINEADJ_005 = "�Q�F�S�ĕ\��"
'        LBL_FINEADJ_006 = "�����"    ' V4.11.0.0�E
'        '----- ###204�� ----- 

'        'teaching
'        LBL_TEACHING_001 = "�����e�B�[�`���O"
'        LBL_TEACHING_002 = "���X�C�b�`:BP�I�t�Z�b�g, START:����"
'        LBL_TEACHING_003 = "���X�C�b�`:�J�b�g�X�^�[�g�ʒu�ړ�, START:����, HALT:�O��"
'        'cut pos teaching
'        LBL_CUTPOSTEACH_001 = "�J�b�g�␳�ʒu�e�B�[�`���O"
'        LBL_CUTPOSTEACH_002 = "�����|�W�V����"
'        LBL_CUTPOSTEACH_003 = "START: ����R, HALT: �O��R, ���X�C�b�`: BP�ړ�"
'        'LBL_CUTPOSTEACH_004 = "�p�^�[���o�^(&P)"
'        LBL_CUTPOSTEACH_004 = "�p�^�[���o�^"

'        'recog
'        LBL_RECOG_001 = "�o�^"
'        LBL_RECOG_002 = "��ݾ�"
'        LBL_RECOG_003 = "�����ϯ����s"
'        LBL_RECOG_004 = "�����͈͐ݒ�"
'        LBL_RECOG_005 = "����ڰēo�^"
'        LBL_RECOG_006 = "�o�^�I��"
'        LBL_RECOG_007 = "����ڰď���"
'        LBL_RECOG_008 = "�␳�|�W�V�����I�t�Z�b�g"
'        LBL_RECOG_009 = "����"
'        LBL_RECOG_010 = "XYð����߼޼��(����߼޼�ݑ���)"
'        LBL_RECOG_011 = "��󽲯�:�ʒu���킹, START����:����, HALT:������"
'        LBL_RECOG_012 = "�}�E�X�Ő��l�p�`���h���b�O������ʒu�����킹�܂��B�E�����݂��h���b�O���đ傫�������킹�܂��B"
'        LBL_RECOG_013 = "�}�E�X�ŉ��F���l�p�`���h���b�O������ʒu�����킹�܂��B�E�����݂��h���b�O���đ傫�������킹�܂��B"
'        LBL_RECOG_014 = "�ݒ�"
'        LBL_RECOG_015 = "XY�e�[�u���|�W�V����"
'        LBL_RECOG_016 = "�O���[�v�ԍ�"
'        LBL_RECOG_017 = "�p�^�[���ԍ�"
'        LBL_RECOG_018 = "臒l"
'        LBL_RECOG_019 = "�R���g���X�g"
'        LBL_RECOG_020 = "�P�x"

'        MSG_RECOG_001 = "�ݒ肪�������܂����B"
'        MSG_RECOG_002 = "�폜���Ă��ǂ��ł����H"
'        'data edit(plate)

'        ' ���ʑΉ�
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            LBL_PP_1 = "����"
'        Else
'            LBL_PP_1 = "�f�[�^��"
'        End If
'        LBL_PP_2 = "�g�������[�h"

'        LBL_PP_3 = "�X�e�b�v�����s�[�g"
'        LBL_PP_4 = "�v���[�g��"
'        LBL_PP_5 = "�u���b�N��"
'        LBL_PP_6 = "�v���[�g�Ԋu(mm)"
'        LBL_PP_8 = "�e�[�u��"
'        LBL_PP_9 = "�r�[��"
'        LBL_PP_10 = "�A�W���X�g"
'        LBL_PP_12 = "�m�f�}�[�L���O"
'        LBL_PP_13 = "�f�B���C�g����"
'        LBL_PP_14 = "�m�f����P��"
'        LBL_PP_15 = "�m�f����(%)"
'        LBL_PP_16 = "�v���[�u �y �I�t�Z�b�g(mm)"
'        LBL_PP_17 = "�v���[�u�X�e�b�v�㏸����(mm)"
'        LBL_PP_18 = "�v���[�u�ҋ@ �y �I�t�Z�b�g(mm)"
'        LBL_PP_19 = "��R���ѕ���"
'        LBL_PP_126 = "�P�u���b�N���@�T�[�L�b�g��"
'        LBL_PP_127 = "�T�[�L�b�g�T�C�Y(mm)"
'        LBL_PP_128 = "�Ǝ��ideg�j"
'        LBL_PP_20 = "�P�T�[�L�b�g���@��R��"
'        LBL_PP_21 = "�O���[�v��"
'        LBL_PP_22 = "�O���[�v�Ԋu"
'        LBL_PP_23 = "�`�b�v�T�C�Y(mm)"
'        '    LBL_PP_123 = "�X�e�b�v�I�t�Z�b�g��(mm)"
'        LBL_PP_123 = "�X�e�b�v(mm)"
'        LBL_PP_24 = "�u���b�N�T�C�Y(mm)"
'        LBL_PP_25 = "�u���b�N�Ԋu(mm)"
'        LBL_PP_26 = "�A���m�f�|�g�h�f�g ��R�u���b�N��"
'        LBL_PP_REVISE = "�ʒu�␳ "
'        LBL_PP_30 = "�ʒu�␳���[�h"
'        LBL_PP_31 = "�ʒu�␳���@"
'        LBL_PP_32 = "�␳�ʒu�P(mm)"
'        LBL_PP_33 = "�␳�ʒu�Q(mm)"
'        LBL_PP_34 = "�␳�ʒu�I�t�Z�b�g(mm)"
'        LBL_PP_35 = "�\�����[�h"
'        LBL_PP_36 = "��m�^�s�N�Z��"
'        LBL_PP_37 = "�p�^�[���ԍ�"
'        LBL_PP_38 = "�p�^�[���O���[�v�ԍ�"
'        LBL_PP_39 = "��]���S(mm)"
'        '    LBL_PP_40 = "�L�����u���[�V�������W�P(mm)"
'        '    LBL_PP_41 = "�L�����u���[�V�������W�Q(mm)"
'        '    LBL_PP_42 = "�L�����u���[�V���� �I�t�Z�b�g(mm)"
'        '    LBL_PP_43 = "�L�����u���[�V���� �p�^�[���ԍ�"
'        '    LBL_PP_44 = "�L�����u���[�V���� �J�b�g��(mm)"
'        '    LBL_PP_45 = "�L�����u���[�V���� �J�b�g���x(mm/s)"
'        '    LBL_PP_46 = "�L�����u���[�V���� �p���[�g(kHz)"
'        LBL_PP_CARIB = "�L�����u���[�V���� "
'        LBL_PP_40 = "���W�P(mm)"
'        LBL_PP_41 = "���W�Q(mm)"
'        LBL_PP_42 = "�I�t�Z�b�g(mm)"
'        LBL_PP_43 = "�p�^�[���ԍ�"
'        LBL_PP_CARIBGRPNO = "�O���[�v�ԍ�"
'        LBL_PP_44 = "�J�b�g��(mm)"
'        LBL_PP_45 = "�J�b�g���x(mm/s)"
'        LBL_PP_46 = "�p���[�g(kHz)"
'        LBL_PP_47 = "�␳�I�t�Z�b�g(mm)"
'        LBL_PP_48 = "�␳�p�^�[���ԍ�"
'        LBL_PP_49 = "�␳�J�b�g��(mm)"
'        LBL_PP_50 = "�␳�J�b�g���x(mm/s)"
'        LBL_PP_51 = "�␳���[�U�p���[�g(kHz)"
'        LBL_PP_52 = "�p�^�[���O���[�v"
'        LBL_PP_53 = "�g���~���O�m�f�J�E���^�i����j"
'        LBL_PP_54 = "���ꌇ���r�o�J�E���^�i����j"
'        LBL_PP_55 = "�A���g���~���O�m�f����"
'        LBL_PP_115 = "�ăv���[�r���O��"
'        LBL_PP_116 = "�ăv���[�r���O�ړ��ʁimm�j"
'        '    LBL_PP_117 = "�p���[�������[�h"
'        LBL_PP_117 = "�p���[�������[�h(x0,x1,x5)"
'        LBL_PP_118 = "�����ڕW�p���[�iW�j"
'        LBL_PP_119 = "�p���[���� �p���[�g�ikHz�j"
'        LBL_PP_120 = "�p���[�������e�͈́iW�j"
'        LBL_PP_121 = "�C�j�V�����n�j�e�X�g"
'        LBL_PP_122 = "��i��"
'        LBL_PP_124 = "4�[�q�I�[�v���`�F�b�N"
'        LBL_PP_125 = "�X�e�b�v�����s�[�g�ړ��ʁimm�j"
'        LBL_PP_126 = "�k�d�c����(�����^�]��)"
'        LBL_PP_127 = "�Ǝ��ideg�j"

'        LBL_PP_130 = "GP-IB����"
'        LBL_PP_131 = "�����ݒ�i�f���~�^�j"
'        LBL_PP_132 = "�����ݒ�i�^�C���A�E�g�j"
'        LBL_PP_133 = "�����ݒ�i�@��A�h���X�j"
'        LBL_PP_134 = "�������R�}���h"
'        LBL_PP_135 = "�g���K�R�}���h"
'        LBL_PP_136 = "GP-IB����"
'        LBL_PP_137 = "�����ݒ�i�@��A�h���X�j"
'        LBL_PP_138 = "���葬�x"
'        LBL_PP_139 = "���胂�[�h"

'        LBL_PP_140 = "�s�ƃI�t�Z�b�g�ideg�j"
'        LBL_PP_141 = "�s�Ɗ�ʒu�P�imm�j"
'        LBL_PP_142 = "�s�Ɗ�ʒu�Q�imm�j"

'        'data edit(step)
'        LBL_S_1 = "�ï�ߔԍ�"
'        LBL_S_2 = "��ۯ���"
'        LBL_S_3 = "�ï�ߊԲ������"
'        LBL_S_4 = "�ï��" & vbCrLf & "�ԍ�"
'        LBL_S_5 = "���WX"
'        LBL_S_6 = "���WY"
'        LBL_S_7 = "�ï��" & vbCrLf & "�ԍ�"
'        LBL_S_8 = "����Đ�"
'        LBL_S_9 = "��ٰ�ߊ�" & vbCrLf & " �������" '..2006/04/06..TM(TXTY�d�l�ύX)

'        '(TXTY�d�l�ύX)
'        LBL_G_1 = "��ٰ�ߔԍ�"
'        LBL_G_2 = "��R��"
'        LBL_G_3 = "��ٰ�ߊԲ������"

'        LBL_TY2_1 = "��ۯ��ԍ�"
'        LBL_TY2_2 = "�ï�ߋ���"

'        'data edit(registor)
'        MSG_REGISTER_LABEL01 = "��R" & vbCrLf & "�ԍ�"
'        MSG_REGISTER_LABEL02 = "����" & vbCrLf & "����"
'        MSG_REGISTER_LABEL03 = "�v���[�u�ԍ�"
'        MSG_REGISTER_LABEL04 = "EXTERNAL BIT"
'        MSG_REGISTER_LABEL05 = "�|�[�Y" & vbCrLf & "(ms)"
'        MSG_REGISTER_LABEL06 = "��" & vbCrLf & "��" & vbCrLf & "�l" & vbCrLf & "��"
'        MSG_REGISTER_LABEL07 = "�x�[�X" & vbCrLf & "��R�ԍ�"
'        MSG_REGISTER_LABEL08 = "�g���~���O�ڕW�l" & vbCrLf & "(ohm)"
'        MSG_REGISTER_LABEL09 = "�d���ω�" & vbCrLf & "�X���[�v"
'        MSG_REGISTER_LABEL10 = "�C�j�V�����e�X�g(%)"
'        MSG_REGISTER_LABEL11 = "�t�@�C�i���e�X�g(%)"
'        MSG_REGISTER_LABEL12 = "�J�b�g��"
'        MSG_REGISTER_LABEL13 = "�J�b�g" & vbCrLf & "�␳"
'        MSG_REGISTER_LABEL14 = "�\��" & vbCrLf & "���[�h"
'        MSG_REGISTER_LABEL15 = "�p�^�[��" & vbCrLf & "�ԍ�"
'        MSG_REGISTER_LABEL16 = "�J�b�g�␳�ʒu"
'        MSG_REGISTER_LABEL17 = "�m�f" & vbCrLf & "�L/��"
'        MSG_REGISTER_LABEL18 = "�Я� H"
'        MSG_REGISTER_LABEL19 = "�Я� L"
'        MSG_REGISTER_LABEL20 = "�C�j�V����OK�e�X�g(%)"
'        MSG_REGISTER_LABEL21 = "��R" & vbCrLf & "(%)"
'        MSG_REGISTER_LABEL22 = "�؏�" & vbCrLf & "�{��"

'        ''''2009/07/03 NET
'        MSG_REGISTER_LABEL25 = "����" & vbCrLf & "�����"
'        MSG_REGISTER_LABEL26 = "�g����" & vbCrLf & "���[�h"
'        MSG_REGISTER_LABEL27 = "�x�[�X" & vbCrLf & "��R"
'        'from TKY
'        MSG_REGISTER_LABEL33 = "�v���[�u�m�F�ʒu"
'        MSG_REGISTER_LABEL34 = "H-X" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL35 = "H-Y" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL36 = "L-X" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL37 = "L-Y" & vbCrLf & "(mm)"
'        '

'        'data edit(cut)
'        MSG_CUT_LABEL01 = "��R" & vbCrLf & "�ԍ�"
'        MSG_CUT_LABEL02 = "���" & vbCrLf & "�ԍ�"
'        MSG_CUT_LABEL03 = "�ިڲ" & vbCrLf & "���" & vbCrLf & "(ms)"
'        MSG_CUT_LABEL04 = "è��ݸ�P" & vbCrLf & "X (mm)"
'        MSG_CUT_LABEL05 = "è��ݸ�P" & vbCrLf & "Y (mm)"
'        MSG_CUT_LABEL06 = "����P" & vbCrLf & "X (mm)"
'        MSG_CUT_LABEL07 = "����P" & vbCrLf & "Y (mm)"
'        MSG_CUT_LABEL08 = "���" & vbCrLf & "��߰��" & vbCrLf & "(mm/s)"
'        MSG_CUT_LABEL09 = "Qڰ�" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL10 = "��ĵ�" & vbCrLf & "(%)"
'        MSG_CUT_LABEL11 = "��ٽ��" & vbCrLf & "����"
'        MSG_CUT_LABEL12 = "��ٽ��" & vbCrLf & "����" & vbCrLf & "(ns)"
'        MSG_CUT_LABEL13 = "LSw��ٽ��" & vbCrLf & "����" & vbCrLf & "(us)"
'        MSG_CUT_LABEL14 = "�����" & vbCrLf & "+" & vbCrLf & "����"
'        MSG_CUT_LABEL15 = "��Ē�1" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL16 = "R1" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL17 = "���P" & vbCrLf & "(%)"
'        MSG_CUT_LABEL18 = "��Ē�2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL19 = "R2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL20 = "��Ē�3" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL21 = "���ޯ��" & vbCrLf & "��"
'        MSG_CUT_LABEL22 = "����" & vbCrLf & "Ӱ��"
'        MSG_CUT_LABEL23 = "���2" & vbCrLf & "��߰��"
'        MSG_CUT_LABEL24 = "Qڰ�2" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL25 = "�΂�" & vbCrLf & "�p�x"
'        MSG_CUT_LABEL26 = "�߯�"
'        MSG_CUT_LABEL27 = "�X" & vbCrLf & "�e" & vbCrLf & "�b" & vbCrLf & "�v"
'        MSG_CUT_LABEL28 = "�{" & vbCrLf & "��"
'        MSG_CUT_LABEL29 = "ES�߲��" & vbCrLf & "(%)"
'        MSG_CUT_LABEL30 = "ES" & vbCrLf & "���艻��" & vbCrLf & "(%)"
'        MSG_CUT_LABEL31 = "ES��" & vbCrLf & "��Ē�" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL32 = "�{��" '"U���" & vbCrLf & "��а1"
'        MSG_CUT_LABEL33 = "������" '"U���" & vbCrLf & "��а2"
'        MSG_CUT_LABEL34 = "Qڰ�3" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL35 = "�ؑւ�" & vbCrLf & "�߲��(%)"
'        MSG_CUT_LABEL36 = "���藦" & vbCrLf & "(%)"
'        MSG_CUT_LABEL37 = "ES��" & vbCrLf & "���艻��" & vbCrLf & "(%)"
'        MSG_CUT_LABEL38 = "ES��" & vbCrLf & "�m�F��"
'        MSG_CUT_LABEL39 = "��ް��" & vbCrLf & "����" & vbCrLf & "(��m)"
'        MSG_CUT_LABEL40 = "��ĵ�" & vbCrLf & "�̾��" & vbCrLf & "(%)"

'        'teaching
'        MSG_TEACH_LABEL01 = "�ް��߲�ĵ̾��"
'        MSG_TEACH_LABEL02 = "���ݸ޽����߲��"
'        MSG_TEACH_LABEL03 = "��RNo."
'        MSG_TEACH_LABEL04 = "�e�B�[�`���O�I��"
'        MSG_TEACH_LABEL05 = "��R����"

'        'plobe position
'        MSG_PROBE_LABEL01 = "�蓮"
'        MSG_PROBE_LABEL02 = "����1�FZ���̂�"
'        MSG_PROBE_LABEL03 = "����2�FXY��"
'        MSG_PROBE_LABEL04 = "����X�^�[�g"
'        MSG_PROBE_LABEL05 = "�e�B�[�`���O�I��"
'        MSG_PROBE_LABEL06 = "�v���[�u�ʒu���킹"
'        MSG_PROBE_LABEL14 = "PB TEACH"
'        MSG_PROBE_MSG01 = "�����v���[�u�ʒu���킹�����s���܂�"
'        MSG_PROBE_MSG02 = "�����v���[�u�ʒu���킹"
'        MSG_PROBE_MSG03 = "���݈ʒu="
'        MSG_PROBE_MSG04 = "���~����="
'        MSG_PROBE_MSG05 = "�ʒu���킹���[�h"
'        MSG_PROBE_MSG06 = "���W"
'        MSG_PROBE_MSG07 = "�e�[�u���ړ�"
'        MSG_PROBE_MSG08 = "�擪�u���b�N"
'        MSG_PROBE_MSG09 = "���ԃu���b�N"
'        MSG_PROBE_MSG10 = "�ŏI�u���b�N"
'        MSG_PROBE_MSG11 = "��R�l����"
'        MSG_PROBE_MSG12 = "���d������"
'        MSG_PROBE_MSG13 = "�A������"
'        MSG_PROBE_MSG14 = "�S����"
'        MSG_PROBE_MSG15 = "�A�� ��~"
'        MSG_PROBE_MSG16 = "��R����G���["

'        ' frmMsgBox(��ʏI���m�F)
'        MSG_CLOSE_LABEL01 = "��ʏI���m�F"
'        MSG_CLOSE_LABEL02 = "�͂�(&Y)"
'        MSG_CLOSE_LABEL03 = "������(&N)"
'        ' frmReset(���_���A��ʂȂ�)
'        MSG_FRMRESET_LABEL01 = "���_���A��"
'        ' LASER_teaching
'        MSG_LASER_LABEL01 = "START:���[�U�[�ˏo"
'        MSG_LASER_LABEL02 = "HALT:���[�U�[��~"
'        MSG_LASER_LABEL03 = "���H�����ԍ�"
'        MSG_LASER_LABEL04 = "���H����"
'        MSG_LASER_LABEL05 = "Q SWITCH RATE (KHz)"
'        MSG_LASER_LABEL06 = "STEG�{��"
'        MSG_LASER_LABEL07 = "�d���l(mA)"
'        MSG_LASER_LABEL08 = "���H�����ԍ����w�肵�ĉ������B"

'        ' ���p���[����(FL�p) ###066
'        MSG_AUTOPOWER_01 = "�p���[�����J�n"
'        MSG_AUTOPOWER_02 = "���H�����ԍ�"
'        MSG_AUTOPOWER_03 = "���[�U�p���[�ݒ�l"
'        MSG_AUTOPOWER_04 = "�d���l"
'        MSG_AUTOPOWER_05 = "�p���[����������"
'        MSG_AUTOPOWER_06 = "�p���[��������I��" 'V1.16.0.0�G

'        DE_EXPLANATION(0) = "�O���@�@�F"
'        DE_EXPLANATION(1) = "�����@�@�F"
'        DE_EXPLANATION(2) = "�O�y�[�W�F"
'        DE_EXPLANATION(3) = "���y�[�W�F"
'        DE_EXPLANATION(4) = "���s�[�g�F"
'        DE_EXPLANATION(5) = "[��]�L�["
'        DE_EXPLANATION(6) = "[��]�L�["
'        DE_EXPLANATION(7) = "[��]�L�["
'        DE_EXPLANATION(8) = "[��]�L�["
'        DE_EXPLANATION(9) = "[Enter]�L�["
'        DE_EXPLANATION(10) = "������@�F"
'        DE_EXPLANATION(11) = "[Esc]�L�["

'        MSG_ONOFF(0) = "OFF"
'        MSG_ONOFF(1) = "ON"

'        '-----------------------------------------------------------------------
'        '   �{�^���\��
'        '-----------------------------------------------------------------------
'        LBL_CUT_COPYLINE = "�s�R�s�[(&L)"
'        LBL_CUT_COPYCOLUMN = "��R�s�[(&C)"

'        LBL_CMD_OK = "OK (&O)"
'        LBL_CMD_CANCEL = "�L�����Z�� (&Q)"
'        LBL_CMD_CLEAR = "�N���A(&K)"
'        LBL_CMD_LCOPY = "��R�s�[(&L)"

'        '-----------------------------------------------------------------------
'        '   �X�v���b�V�����b�Z�[�W
'        '-----------------------------------------------------------------------
'        MSG_SPRASH0 = "���_���A��"
'        MSG_SPRASH1 = "XY�g�����ʒu���Z�b�g��"
'        MSG_SPRASH2 = "�g�����ʒu�ړ���"
'        MSG_SPRASH3 = "START�L�[�������Ă�������"
'        MSG_SPRASH4 = "START�L�[�������Ă�������"
'        MSG_SPRASH5 = "�C���^�[���b�N����������Ă��܂�"
'        MSG_SPRASH6 = "����~���܂���"
'        MSG_SPRASH7 = "�m�F���Ĕ���~�����r�v�������Ă�������"
'        MSG_SPRASH8 = "������������"
'        MSG_SPRASH9 = "�X���C�h�J�o�[����Ă�������"
'        MSG_SPRASH10 = "➑̃J�o�[����Ă�������"
'        MSG_SPRASH11 = "���[�_�[���蓮���[�h�ɂ��Ă�������"
'        MSG_SPRASH12 = "�G�A�[���ቺ���o"
'        MSG_SPRASH13 = "�A��NG-HI�G���["
'        MSG_SPRASH14 = "�ăv���[�r���O���s"
'        MSG_SPRASH15 = "START�L�[�������ƃv���O�������I�����܂�"
'        MSG_SPRASH16 = "RESET�L�[�������ƃv���O�������I�����܂�"
'        MSG_SPRASH17 = "Cancel�{�^�������Ńv���O�������I�����܂�"
'        MSG_SPRASH18 = "���[�_�ʐM�^�C���A�E�g�G���["
'        MSG_SPRASH19 = "�S��~�ُ픭����"
'        MSG_SPRASH20 = "�T�C�N����~�ُ픭����"
'        MSG_SPRASH21 = "�y�̏ᔭ����"
'        MSG_SPRASH22 = "START�L�[�������Ǝ����^�]���J�n���܂�"
'        MSG_SPRASH23 = "�g���~���O���s���ɃG���[���������܂����B"
'        MSG_SPRASH24 = "���[�_���_���A��"
'        MSG_SPRASH25 = "�C���^�[���b�N�S������"
'        MSG_SPRASH26 = "�C���^�[���b�N�ꕔ������"
'        MSG_SPRASH27 = "➑̃J�o�[���J���܂���"
'        MSG_SPRASH28 = "�X���C�h�J�o�[���J���܂���"
'        MSG_SPRASH29 = "�J�o�[����邩�A�C���^�[���b�N�����X�C�b�`���n�m���Ă�������"
'        MSG_SPRASH30 = "PLC�X�e�[�^�X�ُ�"
'        MSG_SPRASH31 = "���ӁI�I�I"
'        MSG_SPRASH32 = "�X���C�h�J�o�[�������ŕ��܂�"
'        MSG_SPRASH33 = "RESET�L�[�������Ə������I�����܂�"
'        MSG_SPRASH34 = "➑̃J�o�[�܂��̓X���C�h�J�o�[���J���܂���"
'        MSG_SPRASH35 = "START�L�[�F�������s�CRESET�L�[�F�����I��"       ' ###073
'        MSG_SPRASH36 = "➑̃J�o�[�����"                             ' ###088
'        MSG_SPRASH37 = "���[�_���_���A������"                           ' ###137
'        MSG_SPRASH38 = "�X�e�[�W���_�ړ���"                             ' ###188
'        MSG_SPRASH39 = "�ꎞ��~���ł�"                                 ' V1.13.0.0�B
'        MSG_SPRASH40 = "����̂΂�������o���܂���"                   ' V1.13.0.0�J
'        MSG_SPRASH41 = "���莞�I�[�o���[�h�����o���܂���"               ' V1.13.0.0�J
'        MSG_SPRASH42 = "�ăv���[�r���O�G���["                           ' V1.13.0.0�J
'        MSG_SPRASH43 = "�O�ʔ����b�N�^�C���A�E�g"                       ' V1.18.0.1�G
'        MSG_SPRASH44 = "�O�ʔ����b�N�����^�C���A�E�g"                   ' V1.18.0.1�G
'        MSG_SPRASH45 = "���u���̊����菜���Ă�������"               ' V4.0.0.0-71
'        MSG_SPRASH46 = "���u���_���A��"                                 ' V4.0.0.0-83
'        MSG_SPRASH47 = "Home�ʒu�ړ���"                                 ' V4.0.0.0-83
'        MSG_SPRASH48 = "�p�q�R�[�h�ɑΉ������g���~���O�f�[�^������܂���"          ' V4.1.0.0�@
'        MSG_SPRASH49 = "�t�@�C�����m�F���Ă�������"                                ' V4.1.0.0�@
'        MSG_SPRASH50 = "PLC���[�o�b�e���[�A���[��"                      ' V4.1.0.0�F
'        MSG_SPRASH51 = "�d�r���������Ă�������"                         ' V4.1.0.0�F

'        '----- limit.frm�p -----
'        MSG_frmLimit_01 = "BP���~�b�g"
'        MSG_frmLimit_02 = "�J�b�g�X�^�[�g�|�C���g��BP�̉ғ��̈�𒴂��܂����B BP�I�t�Z�b�g���C�����Ă�������"
'        MSG_frmLimit_03 = "X�����~�b�g"
'        MSG_frmLimit_04 = "Y�����~�b�g"
'        MSG_frmLimit_05 = "Z�����~�b�g"
'        MSG_frmLimit_06 = "Z2�����~�b�g"
'        MSG_frmLimit_07 = "START�L�[���������AOK�{�^���������ĉ������B"

'        '-----------------------------------------------------------------------
'        '   INtime���G���[���b�Z�[�W 
'        '-----------------------------------------------------------------------
'        MSG_SRV_ALM = "�T�[�{�A���[��"
'        MSG_AXIS_X_SERVO_ALM = "X���T�[�{�A���[��"
'        MSG_AXIS_Y_SERVO_ALM = "Y���T�[�{�A���[��"
'        MSG_AXIS_Z_SERVO_ALM = "Z���T�[�{�A���[��"
'        MSG_AXIS_T_SERVO_ALM = "�Ǝ��T�[�{�A���[��"

'        ' ���n�G���[(�^�C���A�E�g)
'        MSG_TIMEOUT_AXIS_X = "X���^�C���A�E�g�G���["
'        MSG_TIMEOUT_AXIS_Y = "Y���^�C���A�E�g�G���["
'        MSG_TIMEOUT_AXIS_Z = "Z���^�C���A�E�g�G���["
'        MSG_TIMEOUT_AXIS_T = "�Ǝ��^�C���A�E�g�G���["
'        MSG_TIMEOUT_AXIS_Z2 = "Z2���^�C���A�E�g�G���["
'        MSG_TIMEOUT_ATT = "���[�^���A�b�e�l�[�^�^�C���A�E�g�G���["
'        MSG_TIMEOUT_AXIS_XY = "XY���^�C���A�E�g�G���["

'        ' ���n�G���[(�v���X���~�b�g�I�[�o�[)
'        MSG_STG_SOFTLMT_PLUS_AXIS_X = "X���v���X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_PLUS_AXIS_Y = "Y���v���X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z = "Z���v���X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_PLUS_AXIS_T = "�Ǝ��v���X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z2 = "Z2���v���X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_PLUS = "���v���X���~�b�g�I�[�o�["

'        ' ���n�G���[(�}�C�i�X���~�b�g�I�[�o�[)
'        MSG_STG_SOFTLMT_MINUS_AXIS_X = "X���}�C�i�X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_MINUS_AXIS_Y = "Y���}�C�i�X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z = "Z���}�C�i�X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_MINUS_AXIS_T = "�Ǝ��}�C�i�X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z2 = "Z2���}�C�i�X���~�b�g�I�[�o�["
'        MSG_STG_SOFTLMT_MINUS = "���}�C�i�X���~�b�g�I�[�o�["

'        ' ���n�G���[(���~�b�g���o)
'        MSG_AXS_LIM_X = "X�����~�b�g���o"
'        MSG_AXS_LIM_Y = "Y�����~�b�g���o"
'        MSG_AXS_LIM_Z = "Z�����~�b�g���o"
'        MSG_AXS_LIM_T = "�Ǝ����~�b�g���o"
'        MSG_AXS_LIM_Z2 = "Z2�����~�b�g���o"
'        MSG_AXS_LIM_ATT = "ATT�����~�b�g���o"

'        ' BP�n�G���[
'        MSG_BP_MOVE_TIMEOUT = "BP �^�C���A�E�g"
'        MSG_BP_GRV_ALARM_X = "�K���o�m�A���[��X"
'        MSG_BP_GRV_ALARM_Y = "�K���o�m�A���[��Y"
'        MSG_BP_HARD_LIMITOVER_LO = "BP���~�b�g�I�[�o�[(�ŏ����͈͈ȉ�)"
'        MSG_BP_HARD_LIMITOVER_HI = "BP���~�b�g�I�[�o�[(�ő���͈͈ȏ�)"
'        MSG_BP_LIMITOVER = "BP�ړ������ݒ胊�~�b�g�I�[�o�["
'        MSG_BP_SOFT_LIMITOVER = "BP�\�t�g���͈̓I�[�o�["
'        MSG_BP_BSIZE_OVER = "�u���b�N�T�C�Y�ݒ�I�[�o�[(�\�t�g���͈͊O)"

'        ' �J�o�[�J���o
'        MSG_OPN_CVR = "➑̃J�o�[�J���o"
'        MSG_OPN_SCVR = "�X���C�h�J�o�[�J���o"
'        MSG_OPN_CVRLTC = "�J�o�[�J���b�`���o"


'        MSG_INTIME_ERROR = "INTRIM�G���["

'        '-----------------------------------------------------------------------
'        '   �X�e�[�^�X�o�[���b�Z�[�W
'        '-----------------------------------------------------------------------
'        'data edit(plate 01)
'        '���ʑΉ�
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            CONST_PP(glNo_DataNo) = "���ʂ��w��i1:50x60/2:78x78�j"
'        Else
'            CONST_PP(glNo_DataNo) = "�f�[�^�����w��"
'        End If

'        '    CONST_PP(glMeasType) = "�g�������[�h���w��i0:��R/1:DC�d���j"
'        '    CONST_PP(glNo_PlateCntXDir) = "X�����̃v���[�g�����w��"
'        '    CONST_PP(glNo_PlateCntYDir) = "Y�����̃v���[�g�����w��"
'        '    CONST_PP(glPlateInvXDir) = "X�����̃v���[�g�Ԋu��mm�P�ʂŎw��"
'        '    CONST_PP(glPlateInvYDir) = "Y�����̃v���[�g�Ԋu��mm�P�ʂŎw��"
'        CONST_PP(glNo_DirStepRepeat) = "�e�[�u���̃X�e�b�v�������w��i0:����/1:X����/2:Y����/3:�`�b�v��+X�j" '"�e�[�u���̃X�e�b�v�������w��i0:����/1:X����/2:Y�����j"
'        CONST_PP(glNo_MeasType) = "�����ʂ��w��i0:��R�A1:DC�d��)"
'        CONST_PP(glNo_BlockCntXDir) = "X�����̃u���b�N�����w��"
'        CONST_PP(glNo_BlockCntYDir) = "Y�����̃u���b�N�����w��"
'        CONST_PP(glNo_TableOffSetXDir) = "X�����̃e�[�u���I�t�Z�b�g��mm�P�ʂŎw��"
'        CONST_PP(glNo_TableOffSetYDir) = "Y�����̃e�[�u���I�t�Z�b�g��mm�P�ʂŎw��"
'        CONST_PP(glNo_BpOffsetXDir) = "X�����̃r�[���|�W�V�����I�t�Z�b�g��mm�P�ʂŎw��"
'        CONST_PP(glNo_BpOffsetYDir) = "Y�����̃r�[���|�W�V�����I�t�Z�b�g��mm�P�ʂŎw��"
'        CONST_PP(glNo_AdjOffsetXDir) = "X�����̃A�W���X�g�|�C���g��mm�P�ʂŎw��"
'        CONST_PP(glNo_AdjOffsetYDir) = "Y�����̃A�W���X�g�|�C���g��mm�P�ʂŎw��"
'        CONST_PP(glNo_NGMark) = "NG�}�[�L���O�̎��s�L�����w��i0:���s���Ȃ�/1:���s����j"

'        If gSysPrm.stSPF.giDelayTrim2 = 1 Then
'            CONST_PP(glNo_DelayTrim) = "�f�B���C�g�����̎��s�L�����w��i0:���s���Ȃ�/1:���s����/2:�ިڲ���2�����s����j"
'        Else
'            CONST_PP(glNo_DelayTrim) = "�f�B���C�g�����̎��s�L�����w��i0:���s���Ȃ�/1:���s����j"
'        End If

'        CONST_PP(glNo_NgJudgeUnit) = "NG����̒P�ʂ��w��i0:�u���b�N�P��/1:�v���[�g�P�ʁj"
'        CONST_PP(glNo_NgJudgeLevel) = "NG����̔�����%�P�ʂŎw��"
'        CONST_PP(glNo_ZOffSet) = "�v���[�uZ���I�t�Z�b�g��mm�P�ʂŎw��"
'        CONST_PP(glNo_ZStepUpDist) = "�v���[�u�X�e�b�v���̏㏸������mm�P�ʂŎw��"
'        CONST_PP(glNo_ZWaitOffset) = "�v���[�uZ���̑ҋ@�ʒu��mm�P�ʂŎw��"
'        'data edit(plate 03)

'        '''''2009/07/03 NET�̂�
'        CONST_PP(glNo_LedCtrl) = "�P�O���[�v���@�T�[�L�b�g�����w��"
'        '    CONST_PP(glPP127X) = "�T�[�L�b�g�T�C�Y�w���w��"
'        '    CONST_PP(glPP127Y) = "�T�[�L�b�g�T�C�Y�x���w��"

'        CONST_PP(glNo_ResistDir) = "��R���ѕ������w��(0:X�C1:Y)"
'        If (gTkyKnd = KND_CHIP) Then
'            CONST_PP(glNo_ResistCntInGroup) = "�P�O���[�v����R�����w��"
'        ElseIf (gTkyKnd = KND_NET) Then
'            CONST_PP(glNo_ResistCntInGroup) = "�P�u���b�N���T�[�L�b�g���w��"
'        End If
'        CONST_PP(glNo_GroupCntInBlockXBp) = "�u���b�N���O���[�v���w���w��"
'        CONST_PP(glNo_GroupCntInBlockYStage) = "�u���b�N���O���[�v���x���w��"
'        CONST_PP(glNo_GroupItvXDir) = "�O���[�v�Ԋu�w���o�P�ʂŎw��"
'        CONST_PP(glNo_GroupItvYDir) = "�O���[�v�Ԋu�x���o�P�ʂŎw��"
'        CONST_PP(glNo_ChipSizeXDir) = "�`�b�v�T�C�Y�w���o�P�ʂŎw��"
'        CONST_PP(glNo_ChipSizeYDir) = "�`�b�v�T�C�Y�x���o�P�ʂŎw��"
'        CONST_PP(glNo_StepOffsetXDir) = "X�����̃X�e�b�v�I�t�Z�b�g�ʂ�mm�P�ʂŎw��"
'        CONST_PP(glNo_StepOffsetYDir) = "Y�����̃X�e�b�v�I�t�Z�b�g�ʂ�mm�P�ʂŎw��"
'        CONST_PP(glNo_BlockSizeReviseXDir) = "�u���b�N�T�C�Y�␳�w���o�P�ʂŎw��"
'        CONST_PP(glNo_BlockSizeReviseYDir) = "�u���b�N�T�C�Y�␳�x���o�P�ʂŎw��"
'        CONST_PP(glNo_BlockItvXDir) = "�u���b�N�Ԋu�w���o�P�ʂŎw��"
'        CONST_PP(glNo_BlockItvYDir) = "�u���b�N�Ԋu�x���o�P�ʂŎw��"
'        CONST_PP(glNo_ContHiNgBlockCnt) = "�A���m�f�|�g�h�f�g��R�u���b�N�����w��"
'        'data edit(plate 02)
'        CONST_PP(glNo_ReviseMode) = "�␳���[�h���w��(0:����,1:�蓮,2:����+����)"
'        CONST_PP(glNo_ManualReviseType) = "�蓮���[�h�̕␳���@���w��(0:�␳�Ȃ�, 1:1��̂�, 2:����)"
'        CONST_PP(glNo_ReviseCordnt1XDir) = "�ʒu�␳�ʒu�P��X���W�l��mm�P�ʂŎw��"
'        CONST_PP(glNo_ReviseCordnt1YDir) = "�ʒu�␳�ʒu�P��Y���W�l��mm�P�ʂŎw��"
'        CONST_PP(glNo_ReviseCordnt2XDir) = "�ʒu�␳�ʒu�Q��X���W�l��mm�P�ʂŎw��"
'        CONST_PP(glNo_ReviseCordnt2YDir) = "�ʒu�␳�ʒu�Q��Y���W�l��mm�P�ʂŎw��"
'        CONST_PP(glNo_ReviseOffsetXDir) = "�␳�|�W�V�����I�t�Z�b�g��X���W�l��mm�P�ʂŎw��"
'        CONST_PP(glNo_ReviseOffsetYDir) = "�␳�|�W�V�����I�t�Z�b�g��Y���W�l��mm�P�ʂŎw��"
'        CONST_PP(glNo_RecogDispMode) = "�F���f�[�^�\�����[�h���w��(0:�Ȃ�,1:CRT,2:CRT&�����)"
'        CONST_PP(glNo_PixelValXDir) = "X�����̃s�N�Z���l����mm�P�ʂŎw��"
'        CONST_PP(glNo_PixelValYDir) = "Y�����̃s�N�Z���l����mm�P�ʂŎw��"
'        CONST_PP(glNo_RevisePtnNo1) = "�␳�ʒu�P�̃p�^�[���ԍ����w��"
'        CONST_PP(glNo_RevisePtnNo2) = "�␳�ʒu�Q�̃p�^�[���ԍ����w��"
'        CONST_PP(glNo_RevisePtnGrpNo1) = "�␳�ʒu�P�̃p�^�[���O���[�v�ԍ����w��"
'        CONST_PP(glNo_RevisePtnGrpNo2) = "�␳�ʒu�Q�̃p�^�[���O���[�v�ԍ����w��"
'        CONST_PP(glNo_RotateXDir) = "�w�����̉�]���S���w��"
'        CONST_PP(glNo_RotateYDir) = "�x�����̉�]���S���w��"
'        'data edit(plate 04)
'        CONST_PP(glNo_CaribBaseCordnt1XDir) = "�w�����̃L�����u���[�V��������W�P���o�P�ʂŎw��"
'        CONST_PP(glNo_CaribBaseCordnt1YDir) = "�x�����̃L�����u���[�V��������W�P���o�P�ʂŎw��"
'        CONST_PP(glNo_CaribBaseCordnt2XDir) = "X�����̃L�����u���[�V��������W�Q���o�P�ʂŎw��"
'        CONST_PP(glNo_CaribBaseCordnt2YDir) = "Y�����̃L�����u���[�V��������W�Q���o�P�ʂŎw��"
'        CONST_PP(glNo_CaribTableOffsetXDir) = "�w�����̃L�����u���[�V�����e�[�u���I�t�Z�b�g���o�P�ʂŎw��"
'        CONST_PP(glNo_CaribTableOffsetYDir) = "�x�����̃L�����u���[�V�����e�[�u���I�t�Z�b�g���o�P�ʂŎw��"
'        CONST_PP(glNo_CaribPtnNo1) = "�L�����u���[�V�����p�^�[���P�o�^�m�����w��"
'        CONST_PP(glNo_CaribPtnNo2) = "�L�����u���[�V�����p�^�[���Q�o�^�m�����w��"
'        CONST_PP(glNo_CaribPtnNo1GroupNo) = "�L�����u���[�V�����p�^�[���P�o�^�O���[�v�m�����w��"
'        CONST_PP(glNo_CaribPtnNo2GroupNo) = "�L�����u���[�V�����p�^�[���Q�o�^�O���[�v�m�����w��"
'        CONST_PP(glNo_CaribCutLength) = "�L�����u���[�V�����J�b�g�����o�P�ʂŎw��"
'        CONST_PP(glNo_CaribCutSpeed) = "�L�����u���[�V�����J�b�g���x���o/s�P�ʂŎw��"
'        CONST_PP(glNo_CaribCutQRate) = "�L�����u���[�V�������[�U�p���[�g��kHz�P�ʂŎw��"
'        CONST_PP(glNo_CutPosiReviseOffsetXDir) = "�w�����̃J�b�g�_�␳�e�[�u���I�t�Z�b�g���o�P�ʂŎw��"
'        CONST_PP(glNo_CutPosiReviseOffsetYDir) = "�x�����̃J�b�g�_�␳�e�[�u���I�t�Z�b�g���o�P�ʂŎw��"
'        CONST_PP(glNo_CutPosiRevisePtnNo) = "�J�b�g�_�␳�p�^�[���o�^�m�����w��"
'        CONST_PP(glNo_CutPosiReviseCutLength) = "�J�b�g�_�␳�J�b�g�����o�P�ʂŎw��"
'        CONST_PP(glNo_CutPosiReviseCutSpeed) = "�J�b�g�_�␳���[�U�p���[�g��kHz�P�ʂŎw��"
'        CONST_PP(glNo_CutPosiReviseCutQRate) = "�J�b�g�_�␳�J�b�g���x���o/s�P�ʂŎw��"
'        CONST_PP(glNo_CutPosiReviseGroupNo) = "�p�^�[���O���[�v�ԍ����w��"
'        'data edit(plate 05)
'        '===�@432K�p�����[�^�̈׈�U�R�����g
'        '    CONST_PP(glNo_MaxTrimNgCount) = "�g���~���O�m�f�񐔂��w��"
'        '    CONST_PP(glNo_MaxBreakDischargeCount) = "���ꌇ���r�o�񐔂��w��"
'        '    CONST_PP(glNo_TrimNgCount) = "�A���g���~���O�m�f�������w��"
'        '    CONST_PP(glNo_InitialOkTestDo) = "�C�j�V�����n�j�e�X�g���w��(0:����, 1:�L��)"
'        '    CONST_PP(glNo_WorkSetByLoader) = "���[�_���Ŏw�肵���i����w��"
'        '===

'        CONST_PP(glNo_RetryProbeCount) = "�ăv���[�r���O�񐔂��w��"
'        CONST_PP(glNo_RetryProbeDistance) = "�ăv���[�r���O�ړ��ʂ�mm�P�ʂŎw��"
'        CONST_PP(glNo_PowerAdjustMode) = "�p���[�������[�h���w��(0:����, 1:�L��)"
'        CONST_PP(glNo_PowerAdjustTarget) = "�����ڕW�p���[��W�P�ʂŎw��"
'        CONST_PP(glNo_PowerAdjustQRate) = "�p���[�������̃��[�U�p���[�g��kHz�P�ʂŎw��"
'        CONST_PP(glNo_PowerAdjustToleLevel) = "�p���[�������e�͈͂�W�P�ʂŎw��"
'        CONST_PP(glNo_OpenCheck) = "4�[�q�I�[�v���`�F�b�N���w��(0:����, 1:�L��)"
'        CONST_PP(glNo_DistStepRepeat) = "�X�e�b�v�����s�[�g�̈ړ��ʂ�mm�P�ʂŎw��"
'        CONST_PP(glNo_LedCtrl) = "LED�̐�����@���w��(0:�펞ON, 1:�펞OFF, 2:�摜�F�����̂�)"
'        CONST_PP(glNo_RotateTheta) = "�Ǝ��p�x���w��"
'        '    CONST_PP(glPP128) = "�Ǝ��p�x���w��"
'        CONST_PP(glNo_GpibMode) = "GP-IB������w��(0:����, 1:�L��)"
'        CONST_PP(glNo_GpibDelim) = "�f���~�^�̃R�[�h���w��(0:CR+LF, 1:CR, 2:LF, 3:����)"
'        CONST_PP(glNo_GpibTimeOut) = "�^�C���A�E�g�`�F�b�N�̃��~�b�g��100ms�P�ʂŎw��"
'        CONST_PP(glNo_GpibAddress) = "�@��A�h���X���w��"
'        CONST_PP(glNo_GpibInitCmnd) = "�������R�}���h���w��"
'        CONST_PP(glNo_GpibInit2Cmnd) = "�������R�}���h���w��"
'        CONST_PP(glNo_GpibTrigCmnd) = "�g���K�R�}���h���w��"
'        CONST_PP(glNo_Gpib2Mode) = "GP-IB������w��(0:����, 1:�L��)"
'        CONST_PP(glNo_Gpib2Address) = "�@��A�h���X���w��"
'        CONST_PP(glNo_Gpib2MeasSpeed) = "���葬�x���w��(0:�ᑬ, 1:����)"
'        CONST_PP(glNo_Gpib2MeasMode) = "���胂�[�h���w��(0:���, 1:�΍�)"
'        CONST_PP(glNo_TThetaOffset) = "�s�Ǝ��p�x���w��"
'        CONST_PP(glNo_TThetaBase1XDir) = "�s�Ɗ���W�P��X���W�l���o�P�ʂŎw��"
'        CONST_PP(glNo_TThetaBase1YDir) = "�s�Ɗ���W�P��Y���W�l���o�P�ʂŎw��"
'        CONST_PP(glNo_TThetaBase2XDir) = "�s�Ɗ���W�Q��X���W�l���o�P�ʂŎw��"
'        CONST_PP(glNo_TThetaBase2YDir) = "�s�Ɗ���W�Q��Y���W�l���o�P�ʂŎw��"

'        'data edit(step)
'        CONST_SP(0) = "�X�e�b�v�ԍ����w��"
'        CONST_SP(1) = "�e�X�e�b�v�Ɋ܂܂��u���b�N�����w��"
'        CONST_SP(2) = "�e�X�e�b�v�Ԃ̃C���^�[�o����mm�P�ʂŎw��"
'        CONST_CA(0) = "�X�e�b�v�ԍ����w��"
'        CONST_CA(1) = "�T�[�L�b�g���W�w��mm�P�ʂŎw��"
'        CONST_CA(2) = "�T�[�L�b�g���W�x��mm�P�ʂŎw��"
'        CONST_CI(0) = "�X�e�b�v�ԍ����w��"
'        CONST_CI(1) = "�e�X�e�b�v�Ɋ܂܂��T�[�L�b�g�����w��"
'        CONST_CI(2) = "�e�T�[�L�b�g�Ԃ̃C���^�[�o����mm�P�ʂŎw��"

'        'data edit(group)
'        CONST_GP(0) = "�O���[�v�ԍ����w��"
'        CONST_GP(1) = "�e�O���[�v�Ɋ܂܂���R�����w��"
'        CONST_GP(2) = "�e�O���[�v�Ԃ̃C���^�[�o����mm�P�ʂŎw��"
'        CONST_TY2(0) = "�u���b�N�ԍ����w��"
'        CONST_TY2(1) = "X�̈ړ�������mm�P�ʂŎw��"
'        CONST_TY2(2) = "Y�̈ړ�������mm�P�ʂŎw��"

'        'data edit(resistor)
'        CONST_PR1 = "��R�ԍ����w��"
'        CONST_PR2 = "����A���胂�[�h���w��i0:����/1:�����x�j"
'        CONST_PR3 = "��R��������T�[�L�b�g�ԍ����w��"
'        CONST_PR4H = "�n�C���̃v���[�u�ԍ����w��"
'        CONST_PR4L = "���[���̃v���[�u�ԍ����w��"
'        CONST_PR4G = "�� %1 �A�N�e�B�u�K�[�h�ԍ����w��"
'        CONST_PR5 = "EXTERNAL�r�b�g�i16�r�b�g)���w��i16�i=1111111111111111�j"
'        CONST_PR6 = "EXTERNAL�r�b�g�o�͌���߰����т�msec�P�ʂŎw��"
'        MSG_19 = "EXTERNAL BIT��16�r�b�g�w�肵�Ă�������"
'        MSG_20 = "EXTERNAL BITS��1/0�œ��͂��Ă�������"
'        ''''2009/07/03 NET�̂�
'        CONST_PR7 = "�g�������[�h���w��i0:��Βl/1:���V�I�j"
'        CONST_PR8 = "���V�I�g���~���O�̃x�[�X��R�ԍ����w��"
'        CONST_PR9 = "�g���~���O�̖ڕW�l���w��"
'        CONST_PR9_1 = "�g���~���O�̔{�����w��"
'        CONST_PR10 = "�d���ω��X���[�v���w��i0:�{�X���[�v/1:�|�X���[�v�j"
'        CONST_PR11H = "�C�j�V�����e�X�g�̃n�C���~�b�g��%�P�ʂŎw��"
'        CONST_PR11L = "�C�j�V�����e�X�g�̃��[���~�b�g��%�P�ʂŎw��"
'        CONST_PR12H = "�t�@�C�i���e�X�g�̃n�C���~�b�g��%�P�ʂŎw��"
'        CONST_PR12L = "�t�@�C�i���e�X�g�̃��[���~�b�g��%�P�ʂŎw��"
'        CONST_PR13 = "�P��R���̃J�b�g�����w��"
'        CONST_PR14H = "�C�j�V����OK�e�X�g�̃n�C���~�b�g��%�P�ʂŎw��"
'        CONST_PR14L = "�C�j�V����OK�e�X�g�̃��[���~�b�g��%�P�ʂŎw��"
'        CONST_PR10 = "��R��%�P�ʂŎw��"
'        CONST_PR15 = "�؂�グ�{�����w��"
'        'data edit(cut)
'        CONST_CP2 = "�f�B���C�g������msec�P�ʂŎw��"
'        CONST_CP99X = "X�����̃e�B�[�`���O�|�C���g��mm�P�ʂŎw��"
'        CONST_CP99Y = "Y�����̃e�B�[�`���O�|�C���g��mm�P�ʂŎw��"
'        CONST_CP4X = "X�����̃X�^�[�g�|�C���g��mm�P�ʂŎw��"
'        CONST_CP4Y = "Y�����̃X�^�[�g�|�C���g��mm�P�ʂŎw��"
'        CONST_CP5 = "�J�b�g�X�s�[�h��mm/s�P�ʂŎw��"
'        CONST_CP5_2 = "2��ڂ̃J�b�g�X�s�[�h��mm/s�P�ʂŎw��"
'        CONST_CP6 = "���[�U�̂p�X�C�b�`���[�g��kHz�P�ʂŎw��"
'        CONST_CP6_2 = "2��ڂ̃��[�U�̂p�X�C�b�`���[�g��kHz�P�ʂŎw��"
'        CONST_CP7 = "�J�b�g�I�t�l��%�P�ʂŎw��"
'        CONST_CP7_1 = "�J�b�g�f�[�^����i���ω����j���%�P�ʂŎw��"
'        CONST_CP7_2 = "�J�b�g�I�t�I�t�Z�b�g�l��%�P�ʂŎw��"
'        CONST_CP50 = "�p���X������̎��s�L�����w��i0:���s���Ȃ�/1:���s����j"
'        CONST_CP51 = "�p���X�����Ԃ�nsec�P�ʂŎw��"
'        CONST_CP52 = "LSw�p���X�����Ԃ�usec�P�ʂŎw��"
'        CONST_CP9 = "�ő�J�b�e�B���O����mm�P�ʂŎw��"
'        CONST_CP11 = "�k�^�[���|�C���g��%�P�ʂŎw��"
'        CONST_CP12 = "�k�^�[���|�C���g�܂ł̍ő�J�b�e�B���O����mm�P�ʂŎw��"
'        CONST_CP13 = "�΂߃J�b�g�̊p�x��1�x�P�ʂŎw��"
'        CONST_CP14 = "�k�^�[����̍ő�J�b�e�B���O����mm�P�ʂŎw��"
'        CONST_CP15 = "�t�b�N�^�[����̃J�b�e�B���O����mm�P�ʂŎw��"
'        CONST_CP17 = "�C���f�b�N�X����mm�P�ʂŎw��"
'        CONST_CP18 = "�C���f�b�N�X�����w��"
'        CONST_CP19 = "�C���f�b�N�X�J�b�g�O�̑��胂�[�h�����w��i0:����/1:�����x�j"
'        CONST_CP30 = "�J�b�g�p�^�[�����w��"
'        CONST_CP31 = "�J�b�g�������w��"
'        CONST_CP20 = "�X�L�����J�b�g�̃s�b�`���w��"
'        CONST_CP21 = "�X�L�����J�b�g�̃X�e�b�v�������w��"
'        CONST_CP22 = "�X�L�����J�b�g�̖{�����w��"
'        CONST_CP23 = "�����}�[�L���O�̔{�����w��"
'        CONST_CP24 = "�����}�[�L���O�̕�������w��"
'        CONST_CP38 = "�G�b�W�Z���X�|�C���g���w��"
'        CONST_CP39 = "�G�b�W�Z���X�̔���ω������w��"
'        CONST_CP40 = "�G�b�W�Z���X��̃J�b�g�����w��"
'        CONST_CPR1 = "�k�^�[���|�C���g����̉~�ʒ���mm�P�ʂŎw��"
'        CONST_CPR2 = "�t�b�N�^�[���O�̉~�ʒ���mm�P�ʂŎw��"
'        CONST_CP55 = "�G�b�W�Z���X��̔���ω������w��"
'        CONST_CP56 = "�G�b�W�Z���X��̊m�F�񐔂��w��"
'        CONST_CP57 = "���_�[�Ԃ̋�������mm�P�ʂŎw��"
'        CONST_CP53 = "���[�U�̂p�X�C�b�`���[�g��kHz�P�ʂŎw��"
'        CONST_CP54 = "�ؑւ��|�C���g��%�P�ʂŎw��"

'        'data edit
'        STS_21 = "1:+X, 2:-Y, 3:-X, 4:+Y"
'        STS_22 = "1:+X+Y, 2:-Y+X, 3:-X-Y, 4:+Y-X, 5:+X-Y, 6:-Y-X, 7:-X+Y, 8:+Y+X"
'        STS_23 = "1:CW, 2:CCW"
'        STS_24 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, K:ES, M:MARKING" 'ES�ް�ޮ�
'        STS_26 = "���l�� 1:+X, 2:-X, 3:+Y, 4:-Y�œ��͂��Ă�������"
'        STS_69 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, S:ES2, M:MARKING" 'ES2�ް�ޮ�

'        'GP-IB���䖳���H
'        If gSysPrm.stCTM.giGP_IB_flg = 0 Then

'        Else
'            STS_24 = STS_24 & ",T:ST2, X:IX2"
'            STS_69 = STS_69 & ",T:ST2,, X:IX2"
'        End If
'        STS_27 = "�S�p20����(���p40����)�ȓ��œ��͂��Ă�������"
'        STS_28 = "���p�p��0�`20�����ȓ��œ��͂��ĉ�����"
'        STS_29 = "���p�p��0�`10�����ȓ��œ��͂��ĉ�����"

'        '-----------------------------------------------------------------------
'        '   ���b�Z�[�W
'        '-----------------------------------------------------------------------
'        ' �v���[�u�ʒu���킹���b�Z�[�W
'        MSG_PRB_XYMODE = "�w�x�e�[�u���ړ����[�h"
'        MSG_PRB_BPMODE = "�|�W�V���i�ړ����[�h"
'        MSG_PRB_ZTMODE = "�y�e�B�[�`���O���[�h"
'        MSG_PRB_THETA = "�ƒ������[�h"
'        ' �V�X�e���G���[���b�Z�[�W
'        MSG_ERR_ZNOTORG = "�y�����_�Z���T�[���n�m�ɂȂ��Ă��܂���"
'        ' ���[�U�[������ʐ�����
'        MSG_LASER_LASERON = "START:���[�U�[�ˏo"
'        MSG_LASER_LASEROFF = "HALT:���[�U�[��~"
'        MSG_LASEROFF = "���[�U�[��~��"
'        MSG_LASERON = "���[�U�[�ˏo��"
'        MSG_ATTRATE = "������" ' ###108
'        MSG_ERRQRATE = "�p���[�g�ݒ�l���m�F���Ă�������"
'        MSG_LOGERROR = "���O�t�@�C���̏����݂ŃG���[���������܂���"
'        ' �摜�o�^��ʐ�����
'        MSG_PATERN_MSG01 = "�ݒ肪�������܂����B"


'        ' ���샍�O���b�Z�[�W
'        MSG_OPLOG_WAKEUP = "�g���}���u�N��"
'        MSG_OPLOG_FUNC01 = "�f�[�^���[�h"
'        MSG_OPLOG_FUNC02 = "�f�[�^�Z�[�u"
'        MSG_OPLOG_FUNC03 = "�f�[�^�ҏW"
'        MSG_OPLOG_FUNC04 = "�V�X�e���p�����[�^�ҏW"
'        MSG_OPLOG_FUNC05 = "���[�U����"
'        MSG_OPLOG_FUNC06 = "���M���O"
'        MSG_OPLOG_FUNC07 = "�v���[�u�ʒu���킹"
'        MSG_OPLOG_FUNC08 = "�e�B�[�`���O"
'        MSG_OPLOG_FUNC20 = "�J�b�g�ʒu�␳�p�p�^�[���o�^"
'        MSG_OPLOG_FUNC09 = "�摜�o�^"
'        MSG_OPLOG_FUNC10 = "�g���}���u��~"
'        MSG_OPLOG_FUNC30 = "�T�[�L�b�g�e�B�[�`���O"
'        MSG_OPLOG_AUTO = "�����^�]"
'        MSG_OPLOG_LOADERINIT = "���[�_���_���A"
'        '----- V1.13.0.0�B�� -----
'        MSG_OPLOG_APROBEREC = "�I�[�g�v���[�u�o�^"
'        MSG_OPLOG_APROBEEXE = "�I�[�g�v���[�u���s"
'        MSG_OPLOG_IDTEACH = "�h�c�e�B�[�`���O"
'        MSG_OPLOG_SINSYUKU = "�L�k�␳(�摜�o�^)"
'        MSG_OPLOG_MAP = "�}�b�v�I��"
'        '----- V1.13.0.0�B�� -----
'        MSG_OPLOG_CLRTOTAL = "�W�v�N���A"
'        MSG_OPLOG_TRIMST = "�g���~���O"
'        MSG_OPLOG_TRIMRES = "�g���~���O��RESET SW�����ɂ���~"
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
'        MSG_OPLOG_FUNC08S = "�J�b�g�␳�ʒu�e�B�[�`���O"

'        '----- V1.18.0.1�@�� -----
'        MSG_F_EXR1 = "�O���J�����e�B�[�`���O(�P��R)"
'        MSG_F_EXTEACH = "�O���J�����e�B�[�`���O(�S��R)"
'        MSG_F_CARREC = "�L�����u���[�V����(�摜�o�^)"
'        MSG_F_CAR = "�L�����u���[�V����(�␳���s)"
'        MSG_F_CUTREC = "�O���J�����J�b�g�ʒu(�摜�o�^)"
'        MSG_F_CUTREV = "�O���J�����J�b�g�ʒu(�␳���s)"
'        MSG_OPLOG_CMD = "�R�}���h"
'        '----- V1.18.0.1�@�� -----

'        MSG_ERRTRIMVAL = "�g���~���O�ڕW�l�͈̔͂𒴂��Ă��܂�"
'        MSG_ERRCHKMEASM = "�J�b�g�f�[�^�̑��胂�[�h�Ɂu2:�O���v��I�����Ă���ꍇ�͕ύX�ł��܂���"
'        MSG_txtLog_BLOCKMOVE = "�u���b�N�ړ����[�h"

'        '(�g���~���O�p�����[�^����)
'        'data edit(resistor)
'        TRIMPARA(0) = "��R�ԍ�"
'        TRIMPARA(1) = "���葪��"
'        TRIMPARA(2) = "�v���[�u�ԍ�(HIGH)"
'        TRIMPARA(3) = "�v���[�u�ԍ�(LOW)"
'        TRIMPARA(4) = "�v���[�u�ԍ�(AG1)"
'        TRIMPARA(5) = "�v���[�u�ԍ�(AG2)"
'        TRIMPARA(6) = "�v���[�u�ԍ�(AG3)"
'        TRIMPARA(7) = "�v���[�u�ԍ�(AG4)"
'        TRIMPARA(8) = "�v���[�u�ԍ�(AG5)"
'        TRIMPARA(9) = "�g���~���O�ڕW�l"
'        TRIMPARA(52) = "��R"
'        TRIMPARA(53) = "�؂�グ�{��"
'        TRIMPARA(10) = "�C�j�V����OK�e�X�g(HIGH)"
'        TRIMPARA(11) = "�C�j�V����OK�e�X�g(LOW)"
'        TRIMPARA(12) = "�C�j�V�����e�X�g(HIGH)"
'        TRIMPARA(13) = "�C�j�V�����e�X�g(LOW)"
'        TRIMPARA(14) = "�t�@�C�i���e�X�g(HIGH)"
'        TRIMPARA(15) = "�t�@�C�i���e�X�g(LOW)"
'        TRIMPARA(16) = "�J�b�g��"
'        'data edit(cut)
'        TRIMPARA(17) = "�f�B���C�^�C��"
'        TRIMPARA(18) = "�e�B�[�`���O�|�C���gX"
'        TRIMPARA(19) = "�e�B�[�`���O�|�C���gY"
'        TRIMPARA(20) = "�X�^�[�g�|�C���gX"
'        TRIMPARA(21) = "�X�^�[�g�|�C���gY"
'        TRIMPARA(22) = "�J�b�g�X�s�[�h"
'        TRIMPARA(23) = "Q���[�g"
'        TRIMPARA(24) = "�J�b�g�I�t"
'        TRIMPARA(25) = "�p���X������"
'        TRIMPARA(26) = "�p���X������"
'        TRIMPARA(27) = "LSw�p���X������"
'        TRIMPARA(28) = "�J�b�g�p�^�[��"
'        TRIMPARA(29) = "�J�b�g����"
'        TRIMPARA(30) = "�J�b�g��1"
'        TRIMPARA(31) = "R1"
'        TRIMPARA(32) = "�^�[���|�C���g"
'        TRIMPARA(33) = "�J�b�g��2"
'        TRIMPARA(34) = "R2"
'        TRIMPARA(35) = "�J�b�g��3"
'        TRIMPARA(36) = "�C���f�b�N�X��"
'        TRIMPARA(37) = "���胂�[�h"
'        TRIMPARA(38) = "�J�b�g2�X�s�[�h"
'        TRIMPARA(39) = "Q���[�g2"
'        TRIMPARA(40) = "�΂ߊp�x"
'        TRIMPARA(41) = "�s�b�`"
'        TRIMPARA(42) = "�X�e�b�v"
'        TRIMPARA(43) = "�{��"
'        TRIMPARA(44) = "ES�|�C���g"
'        TRIMPARA(45) = "ES���艻��"
'        TRIMPARA(46) = "ES��J�b�g��"
'        TRIMPARA(47) = "�{��"
'        TRIMPARA(48) = "������"
'        TRIMPARA(49) = "Q���[�g3"
'        TRIMPARA(50) = "�ؑւ��|�C���g"
'        TRIMPARA(51) = "���藦"
'        TRIMPARA(52) = "EXTERNAL BIT"
'        TRIMPARA(53) = "�|�[�Y"
'        TRIMPARA(54) = "ES�㔻�藦" ''''NET�ł́u�g�������[�h�v
'        TRIMPARA(55) = "ES��m�F��" ''''NET�ł́u�x�[�X��R�v
'        TRIMPARA(56) = "��ް�ԋ���"
'        TRIMPARA(57) = "�J�b�g�I�t�I�t�Z�b�g"

'        MSG_BTN_CANCEL = "�C�����̃f�[�^�����̏�Ԃɖ߂��Ă���낵���ł����H"
'        MSG_AUTO_01 = "���샂�[�h"
'        MSG_AUTO_02 = "�}�K�W�����[�h"
'        MSG_AUTO_03 = "���b�g���[�h"
'        MSG_AUTO_04 = "�G���h���X���[�h"
'        MSG_AUTO_05 = "�f�[�^�t�@�C��"
'        MSG_AUTO_06 = "�o�^�ς݃f�[�^�t�@�C��"
'        MSG_AUTO_07 = "���X�g��1���"
'        MSG_AUTO_08 = "���X�g��1����"
'        MSG_AUTO_09 = "���X�g����폜"
'        MSG_AUTO_10 = "���X�g���N���A"
'        MSG_AUTO_11 = "���o�^��"
'        MSG_AUTO_12 = "OK"
'        MSG_AUTO_13 = "�L�����Z��"
'        MSG_AUTO_14 = "�f�[�^�I��"
'        MSG_AUTO_15 = "�o�^���X�g��S�č폜���܂��B"
'        MSG_AUTO_16 = "��낵���ł����H"
'        MSG_AUTO_17 = "�G���h���X���[�h���͕����̃f�[�^�t�@�C���͑I���ł��܂���B"
'        MSG_AUTO_18 = "�f�[�^�t�@�C����I�����Ă��������B"
'        MSG_AUTO_19 = "�ҏW���̃f�[�^��ۑ����܂����H"
'        MSG_AUTO_20 = "���H�����t�@�C�������݂��܂���B"

'        MSG_THETA_01 = "�␳�ʒu�P"
'        MSG_THETA_02 = "�␳�ʒu�Q"
'        MSG_THETA_03 = "�␳�l"
'        MSG_THETA_04 = "START"
'        MSG_THETA_05 = "CANCEL"
'        MSG_THETA_06 = "�w����W"
'        MSG_THETA_07 = "�����"
'        MSG_THETA_08 = "��]�␳�p�x"
'        MSG_THETA_09 = "�␳�ʒu1"
'        MSG_THETA_10 = "�␳�ʒu2"
'        MSG_THETA_11 = "�g�����ʒu"
'        MSG_THETA_12 = "�␳��"
'        MSG_THETA_13 = "�␳�ʒu1�@�ړ���"
'        MSG_THETA_14 = "�␳�ʒu2�@�ړ���"
'        MSG_THETA_15 = "�␳�ʒu�P�����킹�Ă���START�L�[�������Ă�������"
'        MSG_THETA_16 = "�␳�ʒu�Q�����킹�Ă���START�L�[�������Ă�������"
'        MSG_THETA_17 = "�␳�l���m�F����START�L�[�������Ă�������"
'        MSG_THETA_18 = "Matching error"
'        MSG_THETA_19 = "Pattern Matching error(Position1)"
'        MSG_THETA_20 = "Pattern Matching error(Position1)"
'        MSG_THETA_21 = "Pattern Matching OK(Position1)"
'        MSG_THETA_22 = "�����ϯ�ݸ�"

'        MSG_TRIM_01 = "�͉��H�͈͊O�ł��B"
'        MSG_TRIM_02 = "���[�_�[�����^�]�̋N�����ł��܂���ł����B"
'        MSG_TRIM_03 = "�����𑱍s���܂����H"
'        MSG_TRIM_04 = "�C�j�V�����e�X�g�@���z�}"
'        MSG_TRIM_05 = "�t�@�C�i���e�X�g�@���z�}"
'        MSG_TRIM_06 = "�����|�W�V����"
'        BTN_TRIM_01 = "�����ް� �ر"
'        BTN_TRIM_02 = "��ڰ��ް��ҏW"
'        BTN_TRIM_03 = "�Ƽ��ýĕ��z�\��"
'        BTN_TRIM_04 = "̧���ýĕ��z�\��"
'        PIC_TRIM_01 = "�C�j�V�����e�X�g�@���z�}"
'        PIC_TRIM_02 = "�t�@�C�i���e�X�g�@���z�}"
'        PIC_TRIM_03 = "�Ǖi"
'        PIC_TRIM_04 = "�s�Ǖi"
'        PIC_TRIM_05 = "�ŏ�%"
'        PIC_TRIM_06 = "�ő�%"
'        PIC_TRIM_07 = "����%"
'        PIC_TRIM_08 = "�W���΍�"
'        PIC_TRIM_09 = "��R��"
'        PIC_TRIM_10 = "���z�}�ۑ�"

'        MSG_LOADER_01 = "�S��~�ُ픭����"
'        MSG_LOADER_02 = "���ْ�~�ُ픭����"
'        MSG_LOADER_03 = "�y�̏ᔭ����"
'        MSG_LOADER_04 = "�ݒ肪�������܂���"
'        MSG_LOADER_05 = "���[�_�G���["
'        MSG_LOADER_06 = "���[�_�C���^�[���b�N"
'        MSG_LOADER_07 = "�p�^�[���}�b�`���O�G���["
'        MSG_LOADER_08 = "�A��NG-HI�װ"
'        MSG_LOADER_09 = "�t���p���[���� Q Rate 10kHz"
'        MSG_LOADER_10 = "���[�U�[�p���[�΂���G���["
'        MSG_LOADER_11 = "�t���p���[�����G���["
'        MSG_LOADER_12 = "�����t���p���[����"
'        MSG_LOADER_13 = "�p���[����"
'        MSG_LOADER_14 = "���[�U�[�p���[�����G���["
'        MSG_LOADER_15 = "�����^�]�I��"
'        MSG_LOADER_16 = "���[�_�A���[�����X�g"
'        MSG_LOADER_17 = "�ڕ����Ɋ���c���Ă���ꍇ��"
'        MSG_LOADER_18 = "��菜���Ă��������B"
'        MSG_LOADER_19 = "��i�킪�ς��܂���"                            ' ###089
'        MSG_LOADER_20 = "�m�f�r�o�{�b�N�X����m�f�����菜���Ă���"      ' ###089
'        MSG_LOADER_21 = "�m�f�r�o�{�b�N�X���t"                              ' ###089
'        MSG_LOADER_22 = "START�L�[����OK�{�^�������Ō��_���A���܂��B"       ' ###124
'        MSG_LOADER_23 = "�����^�]�𒆎~���܂�"                              ' ###124
'        '----- ###187�� -----
'        MSG_LOADER_24 = "�ڕ���N�����v����"
'        MSG_LOADER_25 = "�ڕ���z������"
'        MSG_LOADER_26 = "�n���h�z������"
'        MSG_LOADER_27 = "�����n���h�z������"
'        MSG_LOADER_28 = "���[�n���h�z������"
'        'MSG_LOADER_24 = "�ڕ���N�����v����"                                ' ###144
'        'MSG_LOADER_25 = "�ڕ���z������"                                    ' ###144
'        'MSG_LOADER_26 = "�n���h�z������"                                    ' ###144
'        'MSG_LOADER_27 = "�����n���h�z������"                                ' ###158
'        'MSG_LOADER_28 = "���[�n���h�z������"                                ' ###158
'        '----- ###187�� -----
'        MSG_LOADER_29 = "���u���Ɋ���c���Ă���ꍇ��"                    ' ###158
'        MSG_LOADER_30 = "�����^�]��~��"                                    ' ###172
'        MSG_LOADER_31 = "OK�{�^�������ŃA�v���P�[�V�������I�����܂�"        ' ###175
'        MSG_LOADER_32 = "�ڕ���̊����菜���Ă���"                      ' ###184 
'        MSG_LOADER_33 = "�ēx�����^�]�����s���ĉ�����"                      ' ###184 
'        MSG_LOADER_34 = "�}�K�W��������o�Z���T�n�e�e�ʒu�܂�"            ' ###184 
'        MSG_LOADER_35 = "�����Ă���"                                        ' ###184 
'        MSG_LOADER_36 = "�ڕ���̊����菜���ĉ�����"                    ' ###184 
'        '----- ###188�� -----
'        MSG_LOADER_37 = "�ڕ����Ɋ���c���Ă��܂�"
'        MSG_LOADER_38 = "START�L�[����OK�{�^��������"
'        MSG_LOADER_39 = "�X�e�[�W�����_�ɖ߂��܂�"
'        '----- ###188�� -----
'        MSG_LOADER_40 = "�m�f�����菜���ĉ�����"                        ' ###197 
'        MSG_LOADER_41 = "�m�f�����菜���Ă���"                          ' ###197 
'        '----- ###240�� -----
'        MSG_LOADER_42 = "�ڕ���Ɋ������܂���"
'        MSG_LOADER_43 = "���u���čēx���s���ĉ�����"
'        '----- ###240�� -----
'        '----- V1.18.0.0�H�� -----
'        MSG_LOADER_44 = "�}�K�W���I��"
'        MSG_LOADER_45 = "OK�{�^�������Ŏ����^�]�𑱍s���܂�"
'        MSG_LOADER_46 = "Cancel�{�^�������Ŏ����^�]���I�����܂�"
'        '----- V1.18.0.0�H�� -----
'        MSG_LOADER_47 = "�v���[�u�`�F�b�N�G���["                            ' V1.23.0.0�F
'        MSG_LOADER_48 = "�T�C�N����~��" 'V4.0.0.0�R
'        MSG_LOADER_49 = "�������o"       'V4.0.0.0�R
'        MSG_LOADER_50 = "��𓊓����Ă�������"       'V4.11.0.0�E

'        MSG_71 = "�z���~�X�ł��B����Z�b�g�������ĉ������B"
'        MSG_72 = "�J�o�[�L��̏ꍇ�̓X���C�h�J�o�[���J���āA����Z�b�g�������ĉ������B"
'        MSG_73 = "U-CUT�p�p�����[�^�t�@�C���͑��݂��܂���B"
'        MSG_74 = "U-CUT�p�p�����[�^�t�@�C�����J�����Ƃ��o���܂���ł����B"
'        MSG_75 = "U-CUT�p�p�����[�^�ł͂���܂���B"

'        ' NET�̂�
'        STEP_TITLE01 = "�T�[�L�b�g���W"
'        STEP_TITLE02 = "�O���[�v�ԃC���^�[�o��"
'        STEP_TITLE03 = "�X�e�b�v�ԃC���^�[�o��"
'        ' NET�̂�

'        LBL_STEP_STEP = "�X�e�b�v�f�[�^"
'        LBL_STEP_GROUP = "�O���[�v�f�[�^"
'        LBL_STEP_TY2 = "TY2�f�[�^"
'        LBL_ATT = "������"
'        LBL_FLCUR = "�d���l "

'        ' �����[�_�A���[�����b�Z�[�W 
'        MSG_LDALARM_00 = "����~"
'        MSG_LDALARM_01 = "�}�K�W���������A���[��"
'        MSG_LDALARM_02 = "���ꌇ���i����"
'        MSG_LDALARM_03 = "�n���h�P�z���A���[��"
'        MSG_LDALARM_04 = "�n���h�Q�z���A���[��"
'        MSG_LDALARM_05 = "�ڕ���z���Z���T�ُ�"
'        MSG_LDALARM_06 = "�ڕ���z���~�X"
'        MSG_LDALARM_07 = "���{�b�g�A���[��"
'        MSG_LDALARM_08 = "�H���ԊĎ��A���[��"
'        MSG_LDALARM_09 = "�G���x�[�^�ُ�"
'        'MSG_LDALARM_10 = "�}�K�W������"                                      '###073
'        MSG_LDALARM_10 = "�}�K�W�����͊����"                               '###073
'        MSG_LDALARM_11 = "���_���A�^�C���A�E�g"
'        MSG_LDALARM_12 = "�N�����v�ُ�" '###125
'        MSG_LDALARM_13 = "�G�A�[���ቺ���o"                                     'V4.0.0.0-54
'        MSG_LDALARM_14 = "����`�A���[�� No."
'        MSG_LDALARM_15 = "����`�A���[�� No."

'        MSG_LDALARM_16 = "����V�����_�^�C���A�E�g"
'        MSG_LDALARM_17 = "�n���h�P�V�����_�^�C���A�E�g"
'        MSG_LDALARM_18 = "�n���h�Q�V�����_�^�C���A�E�g"
'        MSG_LDALARM_19 = "�����n���h�z���~�X"
'        MSG_LDALARM_20 = "����n���h�z���~�X"
'        MSG_LDALARM_21 = "�m�f�r�o���t"
'        MSG_LDALARM_22 = "�ꎞ��~"
'        MSG_LDALARM_23 = "�h�A�I�[�v��"
'        MSG_LDALARM_24 = "�񖇎�茟�o"
'        MSG_LDALARM_25 = "�����㉺�@�\�A���[��" 'V4.0.0.0-59
'        MSG_LDALARM_26 = "����`�A���[�� No."
'        MSG_LDALARM_27 = "����`�A���[�� No."
'        MSG_LDALARM_28 = "����`�A���[�� No."
'        MSG_LDALARM_29 = "����`�A���[�� No."
'        MSG_LDALARM_30 = "����`�A���[�� No."
'        MSG_LDALARM_31 = "����`�A���[�� No."
'        MSG_LDALARM_UD = "����`�A���[�� No."

'        MSG_LDGUID_00 = "����~�r�v��������܂����B"
'        MSG_LDGUID_01 = "�}�K�W�������K�̈ʒu���m�F���Ă��������B"
'        'MSG_LDGUID_02 = "���ꌇ���i���w�肳�ꂽ�����������܂����B"             'V1.16.0.0�A
'        MSG_LDGUID_02 = "���ꌇ���i���������܂����B�����菜���Ă��������B"  'V1.16.0.0�A
'        MSG_LDGUID_03 = "�z���Z���T���m�F���Ă��������B"
'        MSG_LDGUID_04 = "�z���Z���T���m�F���Ă��������B"
'        MSG_LDGUID_05 = "�z���Z���T���m�F���Ă��������B"
'        MSG_LDGUID_06 = "�z���Z���T���m�F���Ă��������B" + vbCrLf + "�g�b�v�v���[�g�ɃL�Y���̂����炪�������m�F���Ă��������B"
'        MSG_LDGUID_07 = "���{�b�g�A���[�����������܂����B"
'        MSG_LDGUID_08 = "�H���ԊĎ��Ń^�C���A�E�g���������܂����B"
'        MSG_LDGUID_09 = "�G���x�[�^�̃Z���T���m�F���Ă��������B"
'        'MSG_LDGUID_10 = "�}�K�W�����o�h�O���|��Ă��邩�A�}�K�W�����Z�b�g���Ă��������B"        '###073
'        MSG_LDGUID_10 = "�}�K�W�����o�h�O���|��Ă��邩�A�}�K�W�����͊���Z�b�g���Ă��������B" '###073
'        '----- V1.18.0.0�I�� -----
'        ' ���[���a����(�}�K�W�����o�h�O�͂Ȃ��̂Ń��b�Z�[�W�ύX����)
'        If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
'            MSG_LDGUID_10 = "�}�K�W�����͊���Z�b�g���Ă��������B"
'        End If
'        '----- V1.18.0.0�I�� -----
'        MSG_LDGUID_11 = "���_���A���Ƀ^�C���A�E�g���������܂����B"
'        MSG_LDGUID_12 = "�N�����v�ُ킪�������܂����B" '###125
'        MSG_LDGUID_13 = "�G�A�[����������������Ă��邩�m�F���Ă��������B" 'V4.0.0.0-54
'        MSG_LDGUID_14 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_15 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"

'        MSG_LDGUID_16 = "�V�����_�Z���T���m�F���Ă��������B"
'        MSG_LDGUID_17 = "�V�����_�Z���T���m�F���Ă��������B"
'        MSG_LDGUID_18 = "�V�����_�Z���T���m�F���Ă��������B"
'        MSG_LDGUID_19 = "��z�����Ƀ^�C���A�E�g���������܂����B"
'        MSG_LDGUID_20 = "��z�����Ƀ^�C���A�E�g���������܂����B"
'        MSG_LDGUID_21 = "�m�f�r�o�a�n�w�����t�ł��B�����菜���Ă��������B"
'        MSG_LDGUID_22 = "�ꎞ��~���ł��B"
'        MSG_LDGUID_23 = "�h�A�I�[�v�������o����܂����B�h�A����Ă��������B"
'        MSG_LDGUID_24 = "�����n���h�̊����菜���čĎ��s���Ă��������B" ' V1.18.0.0�J
'        MSG_LDGUID_25 = "�����㉺�@�\�̃��~�b�g�Z���T���m�F���Ă��������B"  'V4.0.0.0-59
'        MSG_LDGUID_26 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_27 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_28 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_29 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_30 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_31 = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"
'        MSG_LDGUID_UD = "���[�J�ɃA���[���ԍ���₢���킹�Ă��������B"

'        MSG_LDINFO_UD = "�z��O�A���[��"

'    End Sub
'#End Region

'#Region "�p�ꃁ�b�Z�[�W��ݒ肷��"
'    '''=========================================================================
'    '''<summary>�p�ꃁ�b�Z�[�W��ݒ肷��</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesEnglish()

'        '-----------------------------------------------------------------------
'        '   �G���[���b�Z�[�W
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
'        MSG_103 = "Are you sure?�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_104 = "The editted data is not saving.�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@"
'        MSG_105 = "This screen returns to the before screen.Are you sure?�@�@�@�@�@�@"
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
'        MSG_127 = "Pattern match error." 'V1.13.0.0�B


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
'        '----- V1.18.0.0�B�� -----
'        MSG_152 = "Initial Processing Of Printing Failed."
'        MSG_153 = "The Printing Job Failed."
'        MSG_154 = "Printing Termination Processing Failed."
'        '----- V1.18.0.0�B�� -----
'        MSG_155 = "The number of substrates is initialized." + vbCrLf + "Is it all right ?" 'V1.18.0.0�K
'        MSG_156 = "Probe Check Error." 'V1.23.0.0�F
'        MSG_157 = "Bar Code Does Not Exist." 'V1.23.0.0�@
'        MSG_158 = "Condition File Read Error." 'V2.0.0.0�D
'        MSG_159 = "A File Exists. Is It Overwritten ?" 'V4.0.0.0�H
'        '----- V4.0.0.0-30�� -----
'        MSG_160 = "Please adjust the probe cleaning position.(XY TABLE)"
'        MSG_161 = "MOVE:[Arrow]  OK:[START]  CANCEL:[RESET]"
'        MSG_162 = "Please adjust the probe cleaning position.(PROBE)"
'        '----- V4.0.0.0-30�� -----

'        MSG_PP23 = "Please make the probe Z offset distance as longer than the upward distance. "
'        MSG_PP24_1 = "The probe Z-axis upward distance is exceeding the Z-axis waiting position."
'        MSG_PP24_2 = "Please make the waiting position distance as longer than the z-axis upward distance."

'        MSGERR_COVER_CLOSE = "Failure to close the slide cover."
'        MSGERR_SEND_TRIMDATA = "Failure to set the trimming data." & vbCrLf & "Please confirm the trimming data."

'        MSG_TOTAL_CIRCUIT = "The unit of the circuit."
'        MSG_TOTAL_REGISTOR = "The unit of the resistor."
'        MSG_COM = "The inputed number is not in the input number area.�i%1~%2�j"
'        MSG_PR_COM1 = "Invalid data. (%1 - %2)"
'        MSG_PR04_1 = "Same value as %1."

'        LOAD_MSG01 = "The specified file is not found."

'        MSG_CUT_1 = "Can't copy datas to another number resistor."

'        '(�f�[�^�ҏW��ʂ̃G���[MSG)
'        DATAEDIT_ERRMSG01 = "Please process resistor."
'        DATAEDIT_ERRMSG02 = "Please process cut."
'        DATAEDIT_ERRMSG03 = "Input Error!"
'        DATAEDIT_ERRMSG04 = "The %1 is already used in %2."
'        ''''2009/7/03 NET�̂�
'        DATAEDIT_ERRMSG05 = "Please set pulse width time in"
'        DATAEDIT_ERRMSG06 = "xx-xx."
'        DATAEDIT_ERRMSG07 = "A re-setup of Q rate."
'        DATAEDIT_ERRMSG08 = "The decimal point '.' is inputted once or more."
'        DATAEDIT_ERRMSG09 = "An ideal value of a Q rate is "
'        DATAEDIT_ERRMSG10 = " []."
'        DATAEDIT_ERRMSG11 = "Is it set as an ideal value?"
'        ''''2009/7/03 NET�̂�

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
'        '   �^�C�g�����b�Z�[�W
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
'        '   ���x�����b�Z�[�W
'        '-----------------------------------------------------------------------
'        '���C����ʃ��x��
'        MSG_MAIN_LABEL01 = "PLATE ="
'        MSG_MAIN_LABEL02 = "NG(%)="
'        MSG_MAIN_LABEL03 = "IT H NG(%)="
'        MSG_MAIN_LABEL04 = "IT L NG(%)="
'        MSG_MAIN_LABEL05 = "FT H NG(%)="
'        MSG_MAIN_LABEL06 = "FT L NG(%)="
'        MSG_MAIN_LABEL07 = "OVER NG(%)="

'        ' �ꎞ��~��ʗp ###014
'        LBL_FINEADJ_001 = "DATA EDIT"
'        '----- ###204�� ----- 
'        LBL_FINEADJ_002 = "ADJUSTMENT"
'        LBL_FINEADJ_003 = "0:No Display"
'        LBL_FINEADJ_004 = "1:Display only NG Logs"
'        LBL_FINEADJ_005 = "2:Display All Logs"
'        '----- ###204�� ----- 
'        LBL_FINEADJ_006 = "Substrate Set"    ' V4.11.0.0�E
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

'        '���ʑΉ�
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            LBL_PP_1 = "PLATE TYPE"
'        Else
'            LBL_PP_1 = "DATA NAME"
'        End If
'        LBL_PP_2 = "TRIM MODE"
'        LBL_PP_3 = "STEP��REPEAT"
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

'        ''''2009/07/03 NET �̂�
'        LBL_PP_126 = "CIRCUIT NUMBER" '"�P�O���[�v���@�T�[�L�b�g��"
'        LBL_PP_127 = "CIRCUIT SIZE(mm)" '�T�[�L�b�g�T�C�Y(mm)"
'        '..

'        LBL_PP_128 = "THETA (deg�j"

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
'        LBL_PP_36 = "PIXEL(��m)"
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
'        LBL_PP_125 = "STEP��REPEAT OFFSET�imm�j"
'        LBL_PP_126 = "LED CONTROL(AUTO MODE)"
'        LBL_PP_127 = "THETA (deg�j"
'        LBL_PP_130 = "GP-IB" 'GP-IB����
'        LBL_PP_131 = "DELIMITER" '�����ݒ�(�f���~�^)
'        LBL_PP_132 = "TIME OUT" '�����ݒ�(�^�C���A�E�g)
'        LBL_PP_133 = "ADDRESS" 'G�����ݒ�(�@��A�h���X)
'        LBL_PP_134 = "INI COMMAND" '�������R�}���h
'        LBL_PP_135 = "TRIGGER COMMAND" '�g���K�R�}���h
'        LBL_PP_136 = "GP-IB" 'GP-IB����
'        LBL_PP_137 = "ADDRESS" '�@��A�h���X
'        LBL_PP_138 = "MEASUREMENT SPEED" '���葬�x
'        LBL_PP_139 = "MEASUREMENT MODE" '���胂�[�h

'        LBL_PP_140 = "T_THETA(deg)" '(�s�ƃI�t�Z�b�g)
'        LBL_PP_141 = "T_THETA ADJUST POINT1(mm)" '(�s�Ɗ�ʒu�P)
'        LBL_PP_142 = "T_THETA ADJUST POINT2(mm)" '(�s�Ɗ�ʒu�P)

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


'        '(TXTY�d�l�ύX)
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
'        MSG_REGISTER_LABEL21 = "��R" & vbCrLf & "(%)"
'        MSG_REGISTER_LABEL22 = "Mag" & vbCrLf & ""
'        ''''2009/07/03 NET�̂�
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
'        MSG_CUT_LABEL32 = "Mag" '�h"U Cut" & vbCrLf & "Dummy1"
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
'        'frmMsgBox(��ʏI���m�F)
'        MSG_CLOSE_LABEL01 = "Exit?"
'        MSG_CLOSE_LABEL02 = "Yes(&Y)"
'        MSG_CLOSE_LABEL03 = "No(&N)"
'        'frmReset(���_���A��ʂȂ�)
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

'        ' ���p���[����(FL�p) ###066
'        MSG_AUTOPOWER_01 = "Start Power Adjustment"
'        MSG_AUTOPOWER_02 = "Condition No."
'        MSG_AUTOPOWER_03 = "Laser Power"
'        MSG_AUTOPOWER_04 = "Current"
'        MSG_AUTOPOWER_05 = "Power Adjustment Failed."
'        MSG_AUTOPOWER_06 = "Power Adjustment Normal End" 'V1.16.0.0�G

'        DE_EXPLANATION(0) = "PREV      :"
'        DE_EXPLANATION(1) = "NEXT      :"
'        DE_EXPLANATION(2) = "PREV PAGE :"
'        DE_EXPLANATION(3) = "NEXT PAGE :"
'        DE_EXPLANATION(4) = "Repeat    :"
'        DE_EXPLANATION(5) = "[��]key"
'        DE_EXPLANATION(6) = "[��]key"
'        DE_EXPLANATION(7) = "[��]key"
'        DE_EXPLANATION(8) = "[��]key"
'        DE_EXPLANATION(9) = "[Enter]key"
'        DE_EXPLANATION(10) = "Cancel    :"
'        DE_EXPLANATION(11) = "[Esc]key"
'        MSG_ONOFF(0) = "OFF"
'        MSG_ONOFF(1) = "ON"

'        '-----------------------------------------------------------------------
'        '   �{�^���\��
'        '-----------------------------------------------------------------------
'        LBL_CUT_COPYLINE = "Copy Line(&L)"
'        LBL_CUT_COPYCOLUMN = "Copy Column(&C)"

'        LBL_CMD_OK = "OK (&O)"
'        LBL_CMD_CANCEL = "Cancel (&Q)"
'        LBL_CMD_CLEAR = "Clear(&K)"
'        LBL_CMD_LCOPY = "LineCopy(&L)"

'        '-----------------------------------------------------------------------
'        '   �X�v���b�V�����b�Z�[�W
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
'        MSG_SPRASH39 = "Under Stop."                                    ' V1.13.0.0�B
'        MSG_SPRASH40 = "CV Error Detection."                            ' V1.13.0.0�J
'        MSG_SPRASH41 = "Over Load Detection."                           ' V1.13.0.0�J
'        MSG_SPRASH42 = "ReProbing Error."                               ' V1.13.0.0�J
'        MSG_SPRASH43 = "Front Door Lock Time Out"                       ' V1.18.0.1�G
'        MSG_SPRASH44 = "Front Door Unlock Time Out"                     ' V1.18.0.1�G
'        MSG_SPRASH45 = "Please remove substrate in EQ"                  ' V4.0.0.0-71
'        MSG_SPRASH46 = "Equipment Initializing"                         ' V4.0.0.0-83
'        MSG_SPRASH47 = "Moving to Home Position"                        ' V4.0.0.0-83
'        MSG_SPRASH48 = "Trimming data is not exist read from QR Code Reader."       ' V4.1.0.0�@
'        MSG_SPRASH49 = "Please confirm trimming data file."                         ' V4.1.0.0�@
'        MSG_SPRASH50 = "PLC Low Battery Detect"                         ' V4.1.0.0�F
'        MSG_SPRASH51 = "Please Change Battery"                          ' V4.1.0.0�F

'        '----- limit.frm�p -----
'        MSG_frmLimit_01 = "BEAM POSITION LIMIT"
'        MSG_frmLimit_02 = "Cut start point(s) is out of the BP moving area. Please correct BP offset."
'        MSG_frmLimit_03 = "X AXIS LIMIT"
'        MSG_frmLimit_04 = "Y AXIS LIMIT"
'        MSG_frmLimit_05 = "Z AXIS LIMIT"
'        MSG_frmLimit_06 = "Z2 AXIS LIMIT"
'        MSG_frmLimit_07 = "PUSH START SW or OK BUTTON"

'        '----- INtime���G���[���b�Z�[�W -----
'        MSG_SRV_ALM = "Servo Alarm."
'        MSG_AXIS_X_SERVO_ALM = "X Axis Servo Alarm."
'        MSG_AXIS_Y_SERVO_ALM = "Y Axis Servo Alarm."
'        MSG_AXIS_Z_SERVO_ALM = "Z Axis Servo Alarm."
'        MSG_AXIS_T_SERVO_ALM = "Theta Axis Servo Alarm."

'        ' ���n�G���[(�^�C���A�E�g)
'        MSG_TIMEOUT_AXIS_X = "X Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_Y = "Y Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_Z = "Z Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_T = "Theta Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_Z2 = "Z2 Axis Error(Time Out)"
'        MSG_TIMEOUT_ATT = "Rotary Attenuater Error(Time Out)"
'        MSG_TIMEOUT_AXIS_XY = "XY Axis Error(Time Out)"

'        ' ���n�G���[(�v���X���~�b�g�I�[�o�[)
'        MSG_STG_SOFTLMT_PLUS_AXIS_X = "X Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_Y = "Y Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z = "Z Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_T = "Theta Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z2 = "Z2 Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS = "Axis +Limit Over."

'        ' ���n�G���[(�}�C�i�X���~�b�g�I�[�o�[)
'        MSG_STG_SOFTLMT_MINUS_AXIS_X = "X Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS_AXIS_Y = "Y Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z = "Z Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS_AXIS_T = "Theta Axis -Limit Over."
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z2 = "Z2 Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS = "Axis -Limit Over."

'        ' ���n�G���[(���~�b�g���o)
'        MSG_AXS_LIM_X = "X-axis limit detection."
'        MSG_AXS_LIM_Y = "Y-axis limit detection."
'        MSG_AXS_LIM_Z = "Z-axis limit detection."
'        MSG_AXS_LIM_T = "Theta-axis limit detection."
'        MSG_AXS_LIM_Z2 = "Z2-axis limit detection."
'        MSG_AXS_LIM_ATT = "ATT-axis limit detection."

'        ' BP�n�G���[
'        MSG_BP_MOVE_TIMEOUT = "BP Time Out."
'        MSG_BP_GRV_ALARM_X = "Galvano Alarm X."
'        MSG_BP_GRV_ALARM_Y = "Galvano Alarm Y."
'        MSG_BP_HARD_LIMITOVER_LO = "BP Limit Over.(Minimum)"
'        MSG_BP_HARD_LIMITOVER_HI = "BP Limit Over.(Maximum)"
'        MSG_BP_LIMITOVER = "BP Moved Distance Setting Limit Exaggerated."
'        MSG_BP_SOFT_LIMITOVER = "Moveble BP Soft Range Exaggerated."
'        MSG_BP_BSIZE_OVER = "Exaggerated Setting Of Block Size."

'        ' �J�o�[�J���o
'        MSG_OPN_CVR = "Cover open detection."
'        MSG_OPN_SCVR = "Slidecover open detection."
'        MSG_OPN_CVRLTC = "Coveropen latch detection."

'        MSG_INTIME_ERROR = "INTRIM Error"

'        '-----------------------------------------------------------------------
'        '   �X�e�[�^�X�o�[���b�Z�[�W
'        '-----------------------------------------------------------------------
'        'data edit(plate 01)
'        '���ʑΉ�
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

'        '(ү���ޕύX)
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
'        CONST_PP(glNo_NGMark) = "NG Marking?�i0:NO, 1:YES�j"

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
'        CONST_PP(glNo_LedCtrl) = "Appoint the number of the circuits in one group." '"�P�O���[�v���@�T�[�L�b�g�����w��"
'        '    CONST_PP(glPP127X) = "Appoint circuit size X." '"�T�[�L�b�g�T�C�Y�w���w��"
'        '    CONST_PP(glPP127Y) = "Appoint circuit size Y." '"�T�[�L�b�g�T�C�Y�x���w��"
'        'data edit(plate 03)
'        CONST_PP(glNo_ResistDir) = "Appoint the direction of row resistor(0:X�C1:Y)"
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
'        '=== 436K�p�����[�^�̈׈�U�R�����g
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
'        CONST_PP(glNo_LedCtrl) = "Appoint a control method of LED.(0:Regular ON, 1:Regular OFF, 2:Only at the time of image recognition.)" '"LED�̐�����@���w��(0:�펞ON, 1:�펞OFF, 2:�摜�F�����̂�)"
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
'        CONST_CA(0) = "Appoint a step number." '"�X�e�b�v�ԍ����w��"
'        CONST_CA(1) = "Appoint circuit coordinate X by a mm unit." '"�T�[�L�b�g���W�w��mm�P�ʂŎw��"
'        CONST_CA(2) = "Appoint circuit coordinate Y by a mm unit." '"�T�[�L�b�g���W�x��mm�P�ʂŎw��"
'        CONST_CI(0) = "Appoint a step number." '"�X�e�b�v�ԍ����w��"
'        CONST_CI(1) = "Appoint the number of the circuits had by a step." '"�e�X�e�b�v�Ɋ܂܂��T�[�L�b�g�����w��"
'        CONST_CI(2) = "Appoint an interval between circuits by a mm unit." '"�e�T�[�L�b�g�Ԃ̃C���^�[�o����mm�P�ʂŎw��"

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

'        ''''2009/07/03 NET �̂�
'        CONST_PR7 = "Appoint a mode to trim. (0:Absolute value/1:Ratio)" '"�g�������[�h���w��i0:��Βl/1:���V�I�j"
'        CONST_PR8 = "Appoint a base resistance number of ratio trimming." '"���V�I�g���~���O�̃x�[�X��R�ԍ����w��"

'        CONST_PR9 = "Appoint the target value of trimming."
'        CONST_PR9_1 = "Appoint the magnification of trimming."

'        CONST_PR11H = "Appoint the high limit of the initial test by % unit."
'        CONST_PR11L = "Appoint the low limit of the initial test by % unit."
'        CONST_PR12H = "Appoint the high limit of the final test by % unit."
'        CONST_PR12L = "Appoint the low limit of the final test by % unit."
'        CONST_PR13 = "Appoint the cut No. in one resistor."
'        CONST_PR10 = "Appoint ��R by a % unit."
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

'        CONST_CP53 = "Appoint a Q switch rate of a laser by a kHz unit." '"���[�U�̂p�X�C�b�`���[�g��kHz�P�ʂŎw��"
'        CONST_CP54 = "Appoint a change point by a % unit." '"�ؑւ��|�C���g��%�P�ʂŎw��"
'        CONST_CP55 = "The change rate after the edge sense is specified. "
'        CONST_CP56 = "The confirmation frequency after the edge sense is specified. "
'        CONST_CP57 = "Please appoint distance between ladders by a micron unit."

'        'data edit
'        STS_21 = "1:+X, 2:-Y, 3:-X, 4:+Y"
'        STS_22 = "1:+X+Y, 2:-Y+X, 3:-X-Y, 4:+Y-X, 5:+X-Y, 6:-Y-X, 7:-X+Y, 8:+Y+X"
'        STS_23 = "1:CW, 2:CCW"
'        STS_24 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, K:ES, M:MARKING" 'ES�ް�ޮ�
'        STS_69 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, S:ES2, M:MARKING" 'ES2�ް�ޮ�

'        'GP-IB���䖳���H
'        If gSysPrm.stCTM.giGP_IB_flg = 0 Then

'        Else
'            '�߼޼��ݸޖ������ޯ���ǉ�
'            STS_24 = STS_24 & ", X:ST2, X:IX2"
'            STS_69 = STS_69 & ", X:ST2, X:IX2"
'        End If

'        STS_26 = "Please specify a value within the limits of 1 and 4 (1:+X, 2:-X, 3:+Y, 4:-Y)"
'        STS_27 = "Please input within the full size of 20 characters(half size of 40 characters)."

'        '-----------------------------------------------------------------------
'        '   ���b�Z�[�W
'        '-----------------------------------------------------------------------
'        '�v���[�u�ʒu���킹���b�Z�[�W
'        MSG_PRB_XYMODE = "The XY table movement mode."
'        MSG_PRB_BPMODE = "The positioner movement mode."
'        MSG_PRB_ZTMODE = "Z teaching mode"
'        MSG_PRB_THETA = "THETA movement mode."
'        '�V�X�e���G���[���b�Z�[�W
'        MSG_ERR_ZNOTORG = "The Z-axis origin point sensor is not turned on."
'        '���[�U�[������ʐ�����
'        MSG_LASER_LASERON = "START: Laser irradiation"
'        MSG_LASER_LASEROFF = "HALT: Laser stop"
'        MSG_LASEROFF = "The laser is under stop."
'        MSG_LASERON = "Under laser irradiation "
'        MSG_ATTRATE = "Rate"
'        MSG_ERRQRATE = "Please confirm the Q-rate preset value."
'        MSG_LOGERROR = "An error occurred in writing for the log file."
'        ' �摜�o�^��ʐ�����
'        MSG_PATERN_MSG01 = "Setting was competed."

'        '���샍�O���b�Z�[�W
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
'        '----- V1.13.0.0�B�� -----
'        MSG_OPLOG_APROBEREC = "AUTO PROBE RECOG"
'        MSG_OPLOG_APROBEEXE = "AUTO PROBE EXECUTE"
'        MSG_OPLOG_IDTEACH = "ID TEACH"
'        MSG_OPLOG_SINSYUKU = "ELASTIC COMPENSATION(RECOG)"
'        MSG_OPLOG_MAP = "MAP Select"
'        '----- V1.13.0.0�B�� -----
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

'        '----- V1.18.0.1�@�� -----
'        MSG_F_EXR1 = "ExCamera Teaching(R1)"
'        MSG_F_EXTEACH = "ExCamera Teaching(ALL)"
'        MSG_F_CARREC = "Calibration(RECOG)"
'        MSG_F_CAR = "Calibration(CORRECT)"
'        MSG_F_CUTREC = "ExCamCutPosition(RECOG)"
'        MSG_F_CUTREV = "ExCamCutPosition(CORRECT)"
'        MSG_OPLOG_CMD = " COMMAND"
'        '----- V1.18.0.1�@�� -----

'        'BLICK MOVE�����p���b�Z�[�W
'        MSG_txtLog_BLOCKMOVE = "BLOCKMOVE MODE"

'        MSG_ERRTRIMVAL = "I surpass the trimming targeted value."
'        MSG_ERRCHKMEASM = "It cannot change, when 2:exterior is chosen as the measurement mode of cut data."

'        '(�g���~���O�p�����[�^����)
'        'data edit(resistor)
'        TRIMPARA(0) = "RES No." '"��R�ԍ�"
'        TRIMPARA(1) = "JUDGMENT MEASUREMENT" '"���葪��"
'        TRIMPARA(2) = "PROBE No(HIGH)" '"�v���[�u�ԍ�(HIGH)"
'        TRIMPARA(3) = "PROBE No(LOW)" '"�v���[�u�ԍ�(LOW)"
'        TRIMPARA(4) = "PROBE No(AG1)" '"�v���[�u�ԍ�(AG1)"
'        TRIMPARA(5) = "PROBE No(AG2)" '"�v���[�u�ԍ�(AG2)"
'        TRIMPARA(6) = "PROBE No(AG3)" '"�v���[�u�ԍ�(AG3)"
'        TRIMPARA(7) = "PROBE No(AG4)" '"�v���[�u�ԍ�(AG4)"
'        TRIMPARA(8) = "PROBE No(AG5)" '"�v���[�u�ԍ�(AG5)"
'        TRIMPARA(9) = "TRIMMING TARGET VALUE" '"�g���~���O�ڕW�l"
'        TRIMPARA(52) = "��R"
'        TRIMPARA(53) = "Mag"
'        TRIMPARA(10) = "INITIAL OK TEST(HIGH)" '"�C�j�V����OK�e�X�g(HIGH)"
'        TRIMPARA(11) = "INITIAL OK TEST(LOW)" '"�C�j�V����OK�e�X�g(LOW)"
'        TRIMPARA(12) = "INITIAL TEST(HIGH)" '"�C�j�V�����e�X�g(HIGH)"
'        TRIMPARA(13) = "INITIAL TEST(LOW)" '"�C�j�V�����e�X�g(LOW)"
'        TRIMPARA(14) = "FINAL TEST(HIGH)" '"�t�@�C�i���e�X�g(HIGH)"
'        TRIMPARA(15) = "FINAL TEST(LOW)" '"�t�@�C�i���e�X�g(LOW)"
'        TRIMPARA(16) = "CUT NUM" '"�J�b�g��"
'        'data edit(cut)
'        TRIMPARA(17) = "DELAY TIME" '"�f�B���C�^�C��"
'        TRIMPARA(18) = "TEACHING POINT X" '"�e�B�[�`���O�|�C���gX"
'        TRIMPARA(19) = "TEACHING POINT Y" '"�e�B�[�`���O�|�C���gY"
'        TRIMPARA(20) = "START POINT X" '"�X�^�[�g�|�C���gX"
'        TRIMPARA(21) = "START POINT Y" '"�X�^�[�g�|�C���gY"
'        TRIMPARA(22) = "CUT SPEED" '"�J�b�g�X�s�[�h"
'        TRIMPARA(23) = "Q RATE" '"Q���[�g"
'        TRIMPARA(24) = "CUT OFF" '"�J�b�g�I�t"
'        TRIMPARA(25) = "PULSE WIDTH CUNTROL" '"�p���X������"
'        TRIMPARA(26) = "PULSE WIDTH TIME" '"�p���X������"
'        TRIMPARA(27) = "LSw PULSE WIDTH TIME" '"LSw�p���X������"
'        TRIMPARA(28) = "CUT PAT" '"�J�b�g�p�^�[��"
'        TRIMPARA(29) = "CUT DEC" '"�J�b�g����"
'        TRIMPARA(30) = "CUT LEN1" '"�J�b�g��1"
'        TRIMPARA(31) = "R1"
'        TRIMPARA(32) = "TURN POINT" '"�^�[���|�C���g"
'        TRIMPARA(33) = "CUT LEN2" '"�J�b�g��2"
'        TRIMPARA(34) = "R2"
'        TRIMPARA(35) = "CUT LEN3" '"�J�b�g��3"
'        TRIMPARA(36) = "INDEX No." '"�C���f�b�N�X��"
'        TRIMPARA(37) = "MEAS MODE" '"���胂�[�h"
'        TRIMPARA(38) = "CUT SPEED2" '"�J�b�g2�X�s�[�h"
'        TRIMPARA(39) = "Q RATE2" '"Q���[�g2"
'        TRIMPARA(40) = "ANGLE" '"�΂ߊp�x"
'        TRIMPARA(41) = "PITCH" '"�s�b�`"
'        TRIMPARA(42) = "STEP" '"�X�e�b�v"
'        TRIMPARA(43) = "LINES" '"�{��"
'        TRIMPARA(44) = "ES POINT" '"ES�|�C���g"
'        TRIMPARA(45) = "ES " '"ES���艻��"
'        TRIMPARA(46) = "ES-LEN" '"ES��J�b�g��"
'        TRIMPARA(47) = "MAG" '"U�J�b�g�_�~�[1"
'        TRIMPARA(48) = "CHARACTERS" '"U�J�b�g�_�~�[1"
'        TRIMPARA(49) = "Q RATE3" '"Q���[�g3"
'        TRIMPARA(50) = "CHANGE POINT" '"�ؑւ��|�C���g"

'        TRIMPARA(51) = "Abs. Ratio"
'        TRIMPARA(52) = "EXTERNAL BIT"
'        TRIMPARA(53) = "Pause Time"
'        TRIMPARA(54) = "ES- %(After)" '''' NET�ł́uTrim Mode�v
'        TRIMPARA(55) = "ES- Cnt(After)" '''' NET�ł́uBase res No.�v
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
'        MSG_TRIM_06 = "Auto Position" '"�����|�W�V����"
'        BTN_TRIM_01 = "Graph Data Clear" '"�����ް� �ر"
'        BTN_TRIM_02 = "Plate Data Editing" '"��ڰ��ް��ҏW"
'        BTN_TRIM_03 = "Initial Test Distribution Map" '"�Ƽ��ýĕ��z�\��"
'        BTN_TRIM_04 = "Final Test Distribution Map" '"̧���ýĕ��z�\��"
'        PIC_TRIM_01 = "INITIAL TEST DISTRIBUTION MAP"
'        PIC_TRIM_02 = "FINAL TEST DISTRIBUTION MAP"
'        PIC_TRIM_03 = "OK" '"�Ǖi"
'        PIC_TRIM_04 = "NG" '"�s�Ǖi"
'        PIC_TRIM_05 = "MIN %" '"�ŏ�%"
'        PIC_TRIM_06 = "MAX %" '"�ő�%"
'        PIC_TRIM_07 = "AVG %" '"����%"
'        PIC_TRIM_08 = "Std Dev" '"�W���΍�"
'        PIC_TRIM_09 = "Res Num" '"��R��"
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
'        '----- ###187�� -----
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
'        '----- ###187�� -----
'        MSG_LOADER_29 = "When A Substrate Is Left In The Equipment,"        ' ###158
'        MSG_LOADER_30 = "Under Automatic Driving Stop"                      ' ###172
'        MSG_LOADER_31 = "PUSH START SW(OK BUTTON) Then Quits the program."  ' ###175
'        MSG_LOADER_32 = "Please Perform Automatic Driving"                  ' ###184 
'        MSG_LOADER_33 = "After Removing Substrate."                         ' ###184 
'        MSG_LOADER_34 = "Please Lower A Magazine To A Substrate Detection"  ' ###184 
'        MSG_LOADER_35 = "Sensor-Off Position."                              ' ###184 
'        MSG_LOADER_36 = "Please Remove Substrate."                          ' ###184 
'        '----- ###188�� -----
'        MSG_LOADER_37 = "A Substrate Is Left On The XY Table"
'        MSG_LOADER_38 = "Push START SW(OK BUTTON) Then Origin Return "
'        MSG_LOADER_39 = ""
'        '----- ###188�� -----
'        MSG_LOADER_40 = "Please Remove NG Substrate"                        ' ###197 
'        MSG_LOADER_41 = "After Removing Substrate"                          ' ###197 
'        '----- ###240�� -----
'        MSG_LOADER_42 = "A Substrate Is Not Shown On The XY Table"
'        MSG_LOADER_43 = "Please Perform Again"
'        '----- ###240�� -----
'        '----- V1.18.0.0�H�� -----
'        MSG_LOADER_44 = "Magazine End"
'        MSG_LOADER_45 = "Processing Is Continued By OK Button,"
'        MSG_LOADER_46 = "Or Ended By Cancel Button."
'        '----- V1.18.0.0�H�� -----
'        MSG_LOADER_47 = "Probe Check Error"                                 ' V1.23.0.0�F
'        MSG_LOADER_48 = "During A Cycle Stop."      'V4.0.0.0�R
'        MSG_LOADER_49 = "Breaking Was Detected."    'V4.0.0.0�R
'        MSG_LOADER_50 = "Please Set Substrates."    'V4.11.0.0�E

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

'        ' �����[�_�A���[�����b�Z�[�W 
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
'        MSG_LDALARM_24 = "Two-Sheet Picking Detection." ' V1.18.0.0�J
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
'        'MSG_LDGUID_02 = "The Broken Or Missing Product Was Outbreak By The Specified Number."                  'V1.16.0.0�A
'        MSG_LDGUID_02 = "The Broken Or Missing Product Was Outbreak." + vbCrLf + "Please Remove A Substrate."   'V1.16.0.0�A
'        MSG_LDGUID_03 = "Please Check The Adsorption Sensor."
'        MSG_LDGUID_04 = "Please Check The Adsorption Sensor."
'        MSG_LDGUID_05 = "Please Check The Adsorption Sensor."
'        MSG_LDGUID_06 = "Please Check The Adsorption Sensor." + vbCrLf + "Please Confirm Whether A Top Plate Does Not Have A Wound And A Fragment Of A Board."
'        MSG_LDGUID_07 = "Robot Alarm Occurred."
'        MSG_LDGUID_08 = "Timeout Occurred Under Process Surveillance."
'        MSG_LDGUID_09 = "Please Check The Sensor Of An Elevator."
'        'MSG_LDGUID_10 = "Please Set A Magazine Whether Magazine Search DOGU Falls Down."              '###073
'        MSG_LDGUID_10 = "Please Set A Magazine Or A Substrate Whether Magazine Search DOGU Falls Down." '###073
'        '----- V1.18.0.0�I�� -----
'        ' ���[���a����(�}�K�W�����o�h�O�͂Ȃ��̂Ń��b�Z�[�W�ύX����)
'        If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
'            MSG_LDGUID_10 = "Please Set A Magazine Or A Substrate."
'        End If
'        '----- V1.18.0.0�I�� -----
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
'        MSG_LDGUID_24 = "Please Remove The Substrate Of A Supply Hand. Then Rerun." ' V1.18.0.0�J
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

'#Region "���b�Z�[�W��ݒ肷��(���{��/�p��) NET�p"
'    '''=========================================================================
'    '''<summary>���b�Z�[�W��ݒ肷��(���{��/�p��) NET�p</summary>
'    '''<param name="language">����ԍ�(0:���{�� 1:�p�� ���̑�:�p��)</param>
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

'#Region "���{�ꃁ�b�Z�[�W��ݒ肷�� NET�p"
'    '''=========================================================================
'    '''<summary>���{�ꃁ�b�Z�[�W��ݒ肷�� NET�p</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub MessagesJapanese()

'        '��Title
'        TITLE_TX = "�`�b�v�T�C�Y(TX)�e�B�[�`���O"
'        TITLE_TY = "�X�e�b�v�T�C�Y(TY)�e�B�[�`���O"
'        TITLE_ExCam = "�O���J�����ɂ���1��R�e�B�[�`���O"
'        TITLE_ExCam1 = "�O���J�����ɂ��e�B�[�`���O"
'        TITLE_T_Theta = "T�ƃe�B�[�`���O"
'        '��Frame
'        FRAME_TX_01 = "��P��R��_"
'        FRAME_TX_02 = "��P�O���[�v�̍ŏI��R��_"
'        FRAME_TY_01 = "��P�u���b�N��_"
'        FRAME_TY_02 = "��P�O���[�v�̍ŏI�u���b�N��_"
'        FRAME_TY_03 = "��Q�O���[�v�̑�P�u���b�N��_"
'        FRAME_C_VAL = "�␳�l"
'        FRAME_ExCam_01 = "�u���b�N�m���D�i�͈͂͂P�`�X�X�j"
'        FRAME_ExCam_02 = "�e�B�[�`���O�|�C���g"

'        '��Command Button
'        CMD_CANCEL = "Cancel"

'        '��FlexGrid
'        FXGRID_TITLE01 = "R No."
'        FXGRID_TITLE02 = "C No."
'        FXGRID_TITLE03 = "���� X"
'        FXGRID_TITLE04 = "���� Y"

'        '��Label
'        LBL_TXTY_TEACH_01 = "�w����W"
'        LBL_TXTY_TEACH_02 = "�����(��m)"
'        LBL_TXTY_TEACH_03 = "�␳��"
'        LBL_TXTY_TEACH_04 = "�␳�䗦"
'        LBL_TXTY_TEACH_05 = "�`�b�v�T�C�Y(mm)"
'        LBL_TXTY_TEACH_06 = "��ۯ�����(mm)"
'        LBL_TXTY_TEACH_07 = "�␳�O"
'        LBL_TXTY_TEACH_08 = "�␳��"
'        LBL_TXTY_TEACH_09 = "�O���[�v�C���^�[�o��"
'        LBL_TXTY_TEACH_10 = "��ۯ����ޕ␳(mm)"
'        LBL_TXTY_TEACH_11 = "�X�e�b�v�C���^�[�o��"
'        LBL_TXTY_TEACH_12 = "��P��_"
'        LBL_TXTY_TEACH_13 = "��Q��_"
'        LBL_TXTY_TEACH_14 = "�O���[�v"
'        LBL_Ex_Cam_01 = "�w��"
'        LBL_Ex_Cam_02 = "�x��"

'        INFO_REC = "�o�^:[START]  �L�����Z��:[RESET]"

'        INFO_MSG01 = "��P�T�[�L�b�g��_�����킹�Ă���START��L�[�������ĉ�����"
'        INFO_MSG02 = "�ŏI�T�[�L�b�g��_�����킹�Ă���START��L�[�������ĉ�����"
'        INFO_MSG03 = "�␳�l���m�F���ĢSTART��L�[���������ĉ�����"
'        INFO_MSG04 = "��P�u���b�N��_�����킹�Ă���START��L�[�������ĉ�����"
'        INFO_MSG05 = "�ŏI�u���b�N��_�����킹�Ă���START��L�[�������ĉ�����"
'        INFO_MSG06 = "��Q�O���[�v�̑�P�u���b�N��_�����킹�Ă���START��L�[�������ĉ�����"
'        INFO_REC = "�o�^:[START]  �L�����Z��:[RESET]"
'        INFO_ST = "�J�n:[START]  ���~:[RESET]"
'        INFO_MSG07 = "�R���\�[���̖��L�[�ňʒu�����킹�܂�"
'        INFO_MSG08 = "�@[START]  �F�ʒu����" & vbCrLf & "�@[RESET]�F���~"
'        INFO_MSG09 = "�@�e�B�[�`���O���I�����܂�"
'        INFO_MSG10 = "�@[START]�L�[�������ĉ�����" & vbCrLf & "�@[HALT]�L�[�łP�O�̃f�[�^�ɖ߂�܂�"
'        INFO_MSG11 = "�@�u���b�N������͂��ĉ�����" & vbCrLf & "�@[START]�L�[�������Ǝ��̏����֐i�݂܂�"
'        INFO_MSG12 = "�@�u���b�N������͂��ĉ�����" & vbCrLf & "�t�����g�J�o�[�����Ǝ��̏����֐i�݂܂�"
'        INFO_MSG13 = "�`�b�v�T�C�Y�@�e�B�[�`���O"
'        INFO_MSG14 = "�X�e�[�W�O���[�v�Ԋu�e�B�[�`���O"
'        INFO_MSG15 = "�X�e�b�v�I�t�Z�b�g�ʁ@�e�B�[�`���O"
'        INFO_MSG16 = "�@�@��ʒu�����킹�ĉ������B"
'        'INFO_MSG17 = "�@�@�ړ��F[���]  ����F[START]  ���f�F[RESET]" '& vbCrLf & "�@�@[HALT]�łP�O�̏����ɖ߂�܂��B"'V1.24.0.0�C
'        INFO_MSG17 = "�@�@�ړ��F[���]  ����F[START]  ���f�F[RESET]  �O��:[HALT]" 'V1.24.0.0�C

'        If (gTkyKnd = KND_CHIP) Then
'            INFO_MSG18 = "��1�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"
'            INFO_MSG20 = "�O���[�v�A�ŏI��R��ʒu�̃e�B�[�`���O"
'            INFO_MSG21 = "�O���[�v�A��1��R��ʒu�̃e�B�[�`���O"
'            INFO_MSG22 = "�ŏI�O���[�v�A�ŏI��R�ʒu�̃e�B�[�`���O"
'            INFO_MSG28 = "�O���[�v�A�ŏI�[�ʒu�̃e�B�[�`���O"
'            INFO_MSG29 = "�O���[�v�A�Ő�[�ʒu�̃e�B�[�`���O"
'            ERR_TXNUM_E = "��R�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
'        ElseIf (gTkyKnd = KND_NET) Then
'            INFO_MSG18 = "��1�O���[�v�A��1�T�[�L�b�g�ʒu�̃e�B�[�`���O"
'            INFO_MSG20 = "�O���[�v�A�ŏI�T�[�L�b�g��ʒu�̃e�B�[�`���O"
'            INFO_MSG21 = "�O���[�v�A��1�T�[�L�b�g��ʒu�̃e�B�[�`���O"
'            INFO_MSG22 = "�ŏI�O���[�v�A�ŏI�T�[�L�b�g�ʒu�̃e�B�[�`���O"
'            ERR_TXNUM_E = "�T�[�L�b�g�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
'        End If
'        ERR_TXNUM_C = "�T�[�L�b�g�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"

'        INFO_MSG19 = "��"
'        INFO_MSG23 = "�a�o�O���[�v�Ԋu�e�B�[�`���O"
'        INFO_MSG30 = "�T�[�L�b�g�Ԋu�e�B�[�`���O"
'        INFO_MSG31 = "�X�e�b�v�I�t�Z�b�g�ʂ̃e�B�[�`���O"
'        INFO_MSG32 = " (�s�w)"  '###084
'        INFO_MSG33 = " (�s�x)"  '###084

'        ERR_TXNUM_B = "�u���b�N�����P�̂��߂��̃R�}���h�͎��s�ł��܂���I"
'        ERR_TXNUM_S = "�X�e�b�v����񐔂�0�̂��߂��̃R�}���h�͎��s�ł��܂���I" 'V1.13.0.0�B
'        '----- V1.19.0.0-33�� -----
'        ERR_PROB_EXE = "�L���Ȓ�R(1-999)�f�[�^���Ȃ����߂��̃R�}���h�͎��s�ł��܂���I"
'        ERR_TEACH_EXE = "�L���Ȓ�R(1-999)���̓}�[�L���O��R�f�[�^���Ȃ����߂��̃R�}���h�͎��s�ł��܂���I"
'        '----- V1.19.0.0-33�� -----

'        '��OperationLogging
'        MSG_OPLOG_TX_START = "TX�e�B�[�`���O"
'        MSG_OPLOG_TY_START = "TY�e�B�[�`���O"
'        MSG_OPLOG_TY2_START = "TY2�e�B�[�`���O"
'        MSG_OPLOG_ExCam_START = "�O���J�����e�B�[�`���O(��1��R)"
'        MSG_OPLOG_ExCam1_START = "�O���J�����e�B�[�`���O(�S��R)"
'        MSG_OPLOG_CitTx_START = "�T�[�L�b�gTX�e�B�[�`���O"
'        MSG_OPLOG_CitTe_START = "�T�[�L�b�g�e�B�[�`���O"
'        MSG_OPLOG_T_THETA_START = "T�ƃe�B�[�`���O"

'        '(Pitch Size)
'        FRAME_PITCH = "XYZ/BP �s�b�`���ݒ�"
'        LBL_PICTH(0) = "LOW�s�b�`"
'        LBL_PICTH(1) = "HIGH�s�b�`"
'        LBL_PICTH(2) = "�|�[�Y����"

'        CIRTEACH_MSG00 = "��"
'        CIRTEACH_MSG01 = "�T�[�L�b�g��_�w�x�����킹�Ă���START��L�[�������ĉ�����"

'        CIRTY_MSG00 = "��P�T�[�L�b�g��_"
'        CIRTY_MSG01 = "��P�O���[�v�̍ŏI�T�[�L�b�g��_"
'        CIRTY_MSG02 = "��Q�O���[�v�̑�P�T�[�L�b�g��_"
'        CIRTY_MSG03 = "����Ļ���(mm)"

'        FRAME_TY2_01 = "�e�B�[�`���O�|�C���g"
'        If (gTkyKnd = KND_CHIP) Then
'            INFO_REC_TX = "�o�^:[START]  �L�����Z��:[RESET]" & vbCrLf & "�o�^+TX2:[HI+START] "
'            INFO_REC_TY = "�o�^:[START]  �L�����Z��:[RESET]" & vbCrLf & "�o�^+TY2:[HI+START] "
'            INFO_REC_TTHETA = "�o�^:[START]  �L�����Z��:[RESET]"
'        ElseIf (gTkyKnd = KND_NET) Then
'            INFO_REC_TX = "�o�^:[START]  �L�����Z��:[RESET]"
'            INFO_REC_TY = "�o�^:[START]  �L�����Z��:[RESET]"
'            INFO_REC_TTHETA = "�o�^:[START]  �L�����Z��:[RESET]"
'        End If

'        INFO_MSG24 = "��"
'        INFO_MSG25 = "�u���b�N�̃e�B�[�`���O"
'        INFO_MSG26 = "�ŏI"
'        INFO_MSG27 = "T�ƃe�B�[�`���O"
'        TITLE_TY2 = "�X�e�b�v�T�C�Y(TY2)�e�B�[�`���O"

'        'V1.13.0.0�E ADD START
'        LBL_IDREADER_TEACH_01 = "�h�c ���[�_�[ �e�B�[�`���O"
'        LBL_IDREADER_TEACH_02 = "��P �h�c�ǂݎ��ʒu"
'        LBL_IDREADER_TEACH_03 = "��Q �h�c�ǂݎ��ʒu"
'        LBL_IDREADER_TEACH_04 = "�h�c���[�_�["
'        'V1.13.0.0�E ADD END
'        LBL_PROBECLEANING_TEACH = "�v���[�u�N���[�j���O�ʒu�e�B�[�`���O" 'V4.0.0.0-30

'    End Sub
'#End Region

'#Region "�p�ꃁ�b�Z�[�W��ݒ肷�� NET�p"
'    '''=========================================================================
'    '''<summary>���b�Z�[�W��ݒ肷��(���{��/�p��) NET�p</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub MessagesEnglish()

'        '��Title
'        TITLE_TX = "Chip size (TX) Teaching"
'        TITLE_TY = "Step size (TY) Teaching"
'        TITLE_ExCam = "The 1st resistance teaching with an external camera"
'        TITLE_ExCam1 = "Teaching with an external camera."
'        TITLE_T_Theta = "T_Theta Teaching"

'        '��Frame
'        FRAME_TX_01 = "The 1st resistance datum point"
'        FRAME_TX_02 = "The last resistance datum point of the 1st group"
'        FRAME_TY_01 = "The 1st block datum point"
'        FRAME_TY_02 = "The last block datum point of the 1st group"
'        FRAME_TY_03 = "1st block reference point of the second group"
'        FRAME_C_VAL = "Correct Value"
'        FRAME_ExCam_01 = "Block No. (The range is 1-99.)"
'        FRAME_ExCam_02 = "Teaching Point"

'        '��Command Button
'        CMD_CANCEL = "Cancel"

'        '��FlexGrid
'        FXGRID_TITLE01 = "R No."
'        FXGRID_TITLE02 = "C No."
'        FXGRID_TITLE03 = "START X"
'        FXGRID_TITLE04 = "START Y"

'        '��Label
'        LBL_TXTY_TEACH_01 = "Appoint coordinate"
'        LBL_TXTY_TEACH_02 = "Difference quantity(��m)"
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
'        INFO_MSG08 = "OK�F[START]�@STOP�F[RESET]"
'        INFO_MSG09 = "Teaching is ended."
'        INFO_MSG10 = "Please push the [START] key. It can return to the data in front of one by the [HALT] key."
'        INFO_MSG11 = "Please input the number of blocks." & vbCrLf & "If the [START] key is pushed, it will progress to the next processing."
'        INFO_MSG12 = "Please input the number of blocks." & vbCrLf & "Next Process when the slide cover is shut."
'        INFO_MSG13 = "CHIP SIZE TEACHING"
'        INFO_MSG14 = "STAGE INTERVAL TEACHING"
'        INFO_MSG15 = "STEP OFFSET TEACHING"

'        INFO_MSG16 = "    Please unite a standard position."
'        'INFO_MSG17 = "    MOVE:[Arrow]  OK:[START]  CANCEL:[RESET]" '& vbCrLf & "    It returns to the processing before one by the HALT key."'V1.24.0.0�C
'        INFO_MSG17 = "    MOVE:[Arrow]  OK:[START]  CANCEL:[RESET]  PREV:[HALT]" ''V1.24.0.0�C

'        If (gTkyKnd = KND_CHIP) Then
'            INFO_MSG18 = "<Group No.1> The 1st resistance standard position." ''''2009/07/03 NET�ł́uresistance��circuit�v(18,20-22)
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
'        ERR_TXNUM_S = "Can't execute this command since the Step number is 0." 'V1.13.0.0�B
'        '----- V1.19.0.0-33�� -----
'        ERR_PROB_EXE = "Can't execute this command since there is no resistance data(1-999)."
'        ERR_TEACH_EXE = "Can't execute this command since there is no resistance data(1-999) or marking resistance data."
'        '----- V1.19.0.0-33�� -----

'        '��OperationLogging
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

'        'V1.13.0.0�E ADD START
'        LBL_IDREADER_TEACH_01 = "ID READER TEACHING"
'        LBL_IDREADER_TEACH_02 = "ID READER POTITION No1"
'        LBL_IDREADER_TEACH_03 = "ID READER POTITION No2"
'        LBL_IDREADER_TEACH_04 = "ID READER"
'        'V1.13.0.0�E ADD END
'        LBL_PROBECLEANING_TEACH = "PROBE CLEANING POSITION TEACHING" 'V4.0.0.0-30

'    End Sub

'#End Region

'#Region "�e�B�[�`���O����(START �܂��� RESET ������)"
'    '''=========================================================================
'    '''<summary>TX/TY�e�B�[�`���O����(START �܂��� RESET ������)</summary>
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
'    '   ���ٰ�݌Q
'    '**********************************************************
'#Region "�����׸ނ�蕶�����ݒ�"
'    '''=========================================================================
'    '''<summary>�����׸ނ�蕶�����ݒ�</summary>
'    '''<param name="language">(INP)����ԍ�(0:���{�� 1:�p�� ���̑�:�p��)</param>
'    '''<remarks>̫��۰�ގ��Ɏ��s����
'    '''         ��Ĉʒu�␳������ڰ��݂̂ݐݒ�</remarks>
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

'#Region "���{�ꕶ�����ݒ肷�� CutPosCorrect_Calibration"
'    '''=========================================================================
'    '''<summary>���{�ꕶ�����ݒ肷�� CutPosCorrect_Calibration</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesJapaneseCutPosCorrect_Calibration()
'        ' ��Ĉʒu�␳������ݒ�
'        FRM_CUT_POS_CORRECT_TITLE = "�J�b�g�ʒu�␳"

'        LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER = "�O���[�v����R��"

'        'LBL_CUT_POS_CORRECT_BLOCK_NO_X = "�u���b�NNo�iX�� 1�`99�j"
'        'LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "�u���b�NNo�iY�� 1�`99�j"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_X = "�u���b�NNo X��"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "�u���b�NNo Y��"

'        LBL_CUT_POS_CORRECT_OFFSET_X = "�J�b�g�ʒu�␳�e�[�u���I�t�Z�b�g�w[mm]"
'        LBL_CUT_POS_CORRECT_OFFSET_Y = "�J�b�g�ʒu�␳�e�[�u���I�t�Z�b�g�x[mm]"

'        ' �����ڰ��ݕ�����ݒ�
'        FRM_CALIBRATION_TITLE = "�L�����u���[�V����"

'        LBL_CALIBRATION_STANDERD_COORDINATES1X = "����W�P�w[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES1Y = "����W�P�x[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2X = "����W�Q�w[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2Y = "����W�Q�x[mm]"

'        LBL_CALIBRATION_TABLE_OFFSETX = "�e�[�u���I�t�Z�b�g�w[mm]"
'        LBL_CALIBRATION_TABLE_OFFSETY = "�e�[�u���I�t�Z�b�g�x[mm]"

'        LBL_CALIBRATION_GAP1X = "����W�P�w�����[mm]"
'        LBL_CALIBRATION_GAP1Y = "����W�P�x�����[mm]"
'        LBL_CALIBRATION_GAP2X = "����W�Q�w�����[mm]"
'        LBL_CALIBRATION_GAP2Y = "����W�Q�x�����[mm]"

'        LBL_CALIBRATION_GAINX = "�Q�C���␳�W���w"
'        LBL_CALIBRATION_GAINY = "�Q�C���␳�W���x"
'        LBL_CALIBRATION_OFFSETX = "�I�t�Z�b�g�␳�ʂw[mm]"
'        LBL_CALIBRATION_OFFSETY = "�I�t�Z�b�g�␳�ʂx[mm]"
'        LBL_CALIBRATION_001 = "�y�\���J�b�g���[�h(����W�P)�z"
'        LBL_CALIBRATION_002 = "�y�\���J�b�g���[�h(����W�Q)�z"
'        LBL_CALIBRATION_003 = "�y�摜�F�����[�h(����W�P)�z"
'        LBL_CALIBRATION_004 = "�y�摜�F�����[�h(����W�Q)�z"

'        MSG_CUT_POS_CORRECT_001 = "�u���b�NNo.�͂P�`�X�X�œ��͂��ĉ������B"
'        MSG_CUT_POS_CORRECT_002 = "�u���b�NNo.���͌�A[�u���b�N�ړ�]�������Ă��������B"
'        MSG_CUT_POS_CORRECT_003 = "�������J�n���܂��B" & vbCrLf & "[START]�F���s" & vbCrLf & "[RESET]�F���~"
'        MSG_CUT_POS_CORRECT_004 = "�y�x���z" & "[START]�F�\���J�b�g���s�C[RESET]�F���~"
'        MSG_CUT_POS_CORRECT_005 = "�\���J�b�g���s��" & vbCrLf & "[HALT]�F�ꎞ��~"
'        MSG_CUT_POS_CORRECT_006 = "[START]�F�\���J�b�g���s" & vbCrLf & "[RESET]�F���~" & vbCrLf & "[ADJ]�ꎞ��~����"
'        MSG_CUT_POS_CORRECT_007 = "[START]�F�摜�F�����s" & vbCrLf & "[RESET]�F���~"
'        MSG_CUT_POS_CORRECT_008 = "�摜�F�����s��" & vbCrLf & "[ADJ]�ꎞ��~"
'        MSG_CUT_POS_CORRECT_009 = "[START]�摜�F�����s" & vbCrLf & "[RESET]���~" & vbCrLf & "[ADJ]�ꎞ��~����"
'        MSG_CUT_POS_CORRECT_010 = "�␳�l�X�V�G���[" & vbCrLf & "�X�V���ʂ��͈͂𒴂��Ă��܂��B" & vbCrLf & "�ő�l�A�ŏ��l�ɕ␳����܂��B"
'        MSG_CUT_POS_CORRECT_011 = "�\���J�b�g�I��" & vbCrLf & "[START]�摜�F��" & vbCrLf & "[RESET]���~"
'        MSG_CUT_POS_CORRECT_012 = "�摜�F���I��"
'        MSG_CUT_POS_CORRECT_013 = "�摜�}�b�`���O�G���["
'        MSG_CUT_POS_CORRECT_014 = "�u���b�N�ԍ�����͌�A[START]�L�[�������Ă�������"
'        MSG_CUT_POS_CORRECT_015 = "�u���b�N�ԍ�����̓G���["
'        MSG_CUT_POS_CORRECT_016 = "�摜�F�����s��"
'        MSG_CUT_POS_CORRECT_017 = "���X�C�b�`:�ʒu����, [START]:����, [HALT]:�O��"
'        MSG_CUT_POS_CORRECT_018 = "���֌W��="
'        MSG_CUT_POS_CORRECT_019 = "�摜��������܂���"

'        MSG_CALIBRATION_001 = "�L�����u���[�V���������s���܂��B" & vbCrLf & "[START]�������Ă��������B" & vbCrLf & "[RESET]�F���~"
'        MSG_CALIBRATION_002 = "�O���J�����ł���ʂ����o���܂��B" & vbCrLf & "[START]�������Ă��������B" & vbCrLf & "[RESET]�F���~"
'        MSG_CALIBRATION_003 = "[START]�F�摜�F�����s�C[RESET]�F���~"
'        MSG_CALIBRATION_004 = "�O���J�����ł���ʂ����o���܂��B(����W�P)" & vbCrLf & "[START]�F����C[RESET]�F���~"
'        MSG_CALIBRATION_005 = "�O���J�����ł���ʂ����o���܂��B(����W�Q)" & vbCrLf & "[START]�F����C[RESET]�F���~"
'        MSG_CALIBRATION_006 = "�L�����u���[�V�������I�����܂��B" & vbCrLf & "�f�[�^��ێ�����ꍇ��[OK]���A�f�[�^��ێ����Ȃ��ꍇ��[Cancel]�������ĉ������B"
'        MSG_CALIBRATION_007 = "�摜�}�b�`���O�G���[" & vbCrLf & "�����𑱂���ꍇ��[OK]���A���~����ꍇ��[Cancel]�������ĉ������B"
'        MSG_CALIBRATION_008 = "Reset���܂��B [Start]:Reset���s , [Reset]:Cancel���s "      ' ' ###078 
'        ' Recog
'        LBL_CALIBRATION_STANDERD_COORDINATES1 = "����W�P[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2 = "����W�Q[mm]"
'        LBL_CALIBRATION_TABLE_OFFSET = "�e�[�u���I�t�Z�b�g[mm]"
'        LBL_CALIBRATION_CUT = "�����ڰ��ݶ��"
'        LBL_CALIBRATION_MOVE1 = "����W�P�ֈړ�"
'        LBL_CALIBRATION_MOVE2 = "����W�Q�ֈړ�"
'        LBL_CUT_POS_CORRECT_CUT = "�\���J�b�g"
'        LBL_CUT_POS_CORRECT_MOVE = "�O���J�����ֈړ�"

'        ' LOG
'        MSG_OPLOG_CUT_POS_CORRECT_START = "�J�b�g�ʒu�␳"
'        MSG_OPLOG_CALIBRATION_START = "�L�����u���[�V����"
'        MSG_OPLOG_CUT_POS_CORRECT_RECOG_START = "�J�b�g�ʒu�␳�摜�o�^"
'        MSG_OPLOG_CALIBRATION_RECOG_START = "�L�����u���[�V�����摜�o�^"
'    End Sub
'#End Region

'#Region "�p�ꕶ�����ݒ肷�� CutPosCorrect_Calibration"
'    '''=========================================================================
'    '''<summary>�p�ꕶ�����ݒ肷�� CutPosCorrect_Calibration</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesEnglishCutPosCorrect_Calibration()
'        ' ��Ĉʒu�␳������ݒ�
'        FRM_CUT_POS_CORRECT_TITLE = "Cut Position Correct"

'        LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER = "Resistor Number in Group"

'        'LBL_CUT_POS_CORRECT_BLOCK_NO_X = "Block Number (X Axis 1�`99)"
'        'LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "Block Number (Y Axis 1�`99)"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_X = "Block Number X Axis"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "Block Number Y Axis"

'        LBL_CUT_POS_CORRECT_OFFSET_X = "Cut Position Correct Table Offset X[mm]"
'        LBL_CUT_POS_CORRECT_OFFSET_Y = "Cut Position Correct Table Offset Y[mm]"

'        ' �����ڰ��ݕ�����ݒ�
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