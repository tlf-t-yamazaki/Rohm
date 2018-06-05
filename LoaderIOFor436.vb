'===============================================================================
'   Description  : �I�[�g���[�_�h�n����(SL436R/SL436S�p)
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
#Region "�y�ϐ���`�z"
    '===============================================================================
    '   �萔��`
    '===============================================================================
    '----- API��` -----
    'Private Declare Function GetPrivateProfileInt Lib "kernel32" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer
    'Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long

    '-------------------------------------------------------------------------------
    '   ���[�_�h�^�n�r�b�g��`(SL436R�p)
    '-------------------------------------------------------------------------------
    '----- �o�̓f�[�^(�g���}  �� ���[�_) -----
    Public Const LOUT_REDY As UShort = &H1                              ' B0 : �g���}�����f�B(0=Not Ready, 1=Ready) ��BIT�𔽓]���ďo�͂���
    Public Const LOUT_AUTO As UShort = &H2                              ' B1 : ���[�_���[�h�ؑւ�(0=�蓮, 1=����(�}�K�W���`�F�b�N))
    Public Const LOUT_STOP As UShort = &H4                              ' B2 : �g���}����~��(0=���쒆,1=��~��)
    Public Const LOUT_SUPLY As UShort = &H8                             ' B3 : ��v��(0=��v����,1=��v��)���A���^�]�J�n
    Public Const LOUT_TRM_NG As UShort = &H10                           ' B4 : �g���~���O�m�f  (0:����, 1:�m�f)
    'Public Const LOUT_PTN_NG As UShort = &H20                          ' B5 : �p�^�[���F���m�f(0:����, 1:�m�f) ###070
    Public Const LOUT_INTLOK_DISABLE As UShort = &H20                   ' B5 : �C���^�[���b�N����(0:�C���^�[���b�N��, 1:�C���^�[���b�N������(�S��/�ꕔ)) ###070
    Public Const LOUT_REQ_COLECT As UShort = &H40                       ' B6 : �����v��(0=�v����,1=�v���L)
    '###148    Public Const LOUT_NO_ALM As UShort = &H80                ' B7 : �g���}�A���[��(0:�A���[��, 1:����)
    Public Const LOUT_PROC_CONTINUE As UShort = &H80                    ' B7 : ����p���M��(0:�Ȃ�, 1:�p�����s)
    Public Const LOUT_ORG_BACK As UShort = &H100                        ' B8 : ���[�_���_���A�v��(0=���_���A���v��, 1=���_���A�v��)
    Public Const LOUT_STB As UShort = &H200                             ' B9 : �i��f�[�^���M(STB)
    Public Const LOUT_NG_DISCHRAGE As UShort = &H400                    ' B10: �m�f��r�o�v��(0=�m�f�r�o�v����, 1=�m�f�r�o�v��)
    Public Const LOUT_DISCHRAGE As UShort = &H800                       ' B11: �����ʒu������(0=�����łȂ�, 1=����)
    Public Const LOUT_STS_RUN As UShort = &H1000                        ' B12: �g���}�^�]��(0:�ꎞ��~, 1:�^�]��)
    Public Const LOUT_EMPTY_OPE As UShort = &H2000                      ' B13: ��^�]��(SL436R�p)
    Public Const LOUT_CYCL_STOP As UShort = &H2000                      ' B13: �T�C�N����~�v��(0=�v����,1=�v��)(SL436S�p) V4.0.0.0�R
    Public Const LOUT_VACCUME As UShort = &H4000                        ' B14: �z��(�蓮���[�h���L��)
    Public Const LOUT_CLAMP As UShort = &H8000                          ' B15: �ڕ���N�����v�J��(0=�J, 1=��)(�蓮���[�h���L��)

    '----- ���̓f�[�^(���[�_  �� �g���}) -----
    Public Const LINP_READY As UShort = &H1                             ' B0 : ���[�_�����f�B(0=Not Ready, 1=Ready) 
    Public Const LINP_AUTO As UShort = &H2                              ' B1 : ���[�_���[�h�ؑւ�(0=�蓮, 1=����)
    Public Const LINP_STOP As UShort = &H4                              ' B2 : ���[�_����~��(0=�������, 1=��~��)
    Public Const LINP_TRM_START As UShort = &H8                         ' B3 : �g���~���O�X�^�[�g�v��(0=�X�^�[�g��v��, 1=�X�^�[�g�v��) 
    '###148    Public Const LINP_RSV04 As UShort = &H10                 ' B4 : �\��
    Public Const LINP_NGTRAY_OUT_COMP As UShort = &H10                  ' B4 : NG�g���C�ւ̔r�o�����M��
    Public Const LINP_RSV05 As UShort = &H20                            ' B5 : �\��(SL436R�p)
    Public Const LIN_CYCL_STOP As UShort = &H20                         ' B5 : �T�C�N����~����(0=������,1=����)(SL436S�p) V4.0.0.0�R
    'V5.0.0.6�A    Public Const LINP_RSV06 As UShort = &H40                            ' B6 : �\��
    Public Const LINP_HST_MOVESUPPLY As Short = &H40S                    ' B6 : �����ʒu�ړ��w�� 'V5.0.0.6�A
    Public Const LINP_NO_ALM_RESTART As UShort = &H80                   ' B7 : ���[�_������(0=�A���[������, 1=����)
    Public Const LINP_ORG_BACK As UShort = &H100                        ' B8 : ���[�_���_���A����(0=���_���A������, 1=���_���A����)
    Public Const LINP_LOT_CHG As UShort = &H200                         ' B9 : ���b�g�؊��v��(0=���b�g�؊���v��, 1=���b�g�؊��v��(���t))
    Public Const LINP_END_MAGAZINE As UShort = &H400                    ' B10: �}�K�W���I��(0=�}�K�W����I��, 1=�}�K�W���I��)
    Public Const LINP_END_ALL_MAGAZINE As UShort = &H800                ' B11: �S�}�K�W���I��(0=�S�}�K�W����I��, 1=�S�}�K�W���I��)
    Public Const LINP_NG_FULL As UShort = &H1000                        ' B12: NG�r�o���t(0=NG�r�o�����t, 1=NG�r�o���t(����))
    Public Const LINP_DISCHRAGE As UShort = &H2000                      ' B13: �r�o�s�b�N����(0=�����łȂ�, 1=����)
    Public Const LINP_2PIECES As UShort = &H4000                        ' B14: �Q����茟�o(0=�Q����薢���o, 1=�Q����茟�o)�@
    Public Const LINP_WBREAK As UShort = &H8000                         ' B15: ����ꌟ�o(0=����ꖢ���o, 1=����ꌟ�o)�@

    '-------------------------------------------------------------------------------
    '   ���[�_�A���[����ԁ^�A���[���ڍג�`(SL436R�p)
    '-------------------------------------------------------------------------------
    '----- ���[�_�A���[����� SL436R�p(W110) -----
    Public Const LARM_ARM1 As UShort = &H100                            ' B8 : �y�̏�(���s�\)���A���[����ԉ�����X�^�[�g(W109.01)�ő��s
    Public Const LARM_ARM2 As UShort = &H200                            ' B9 : �T�C�N����~    �����Z�b�g(W109.03)�㌴�_���A���ăA�C�h�����
    Public Const LARM_ARM3 As UShort = &H400                            ' B10: �S��~�ُ�      ������

    '----- ���[�_�A���[���ڍ� SL436R�p(W115, W116) -----
    '                                                                   ' W115.00-W115.15(���s�s��)
    Public Const LARM_MSG000 As UShort = &H1                            ' B00: ����~
    Public Const LARM_MSG001 As UShort = &H2                            ' B01: �}�K�W���������A���[��
    Public Const LARM_MSG002 As UShort = &H4                            ' B02: ���ꌇ���i����
    Public Const LARM_MSG003 As UShort = &H8                            ' B03: �n���h�P�z���A���[��
    Public Const LARM_MSG004 As UShort = &H10                           ' B04: �n���h�Q�z���A���[��
    Public Const LARM_MSG005 As UShort = &H20                           ' B05: �ڕ���z���Z���T�ُ�
    Public Const LARM_MSG006 As UShort = &H40                           ' B06: �ڕ���z���~�X
    Public Const LARM_MSG007 As UShort = &H80                           ' B07: ���{�b�g�A���[��
    Public Const LARM_MSG008 As UShort = &H100                          ' B08: �H���ԊĎ��A���[��
    Public Const LARM_MSG009 As UShort = &H200                          ' B09: �G���x�[�^�ُ�
    Public Const LARM_MSG010 As UShort = &H400                          ' B10: �}�K�W������
    Public Const LARM_MSG011 As UShort = &H800                          ' B11: ���_���A�^�C���A�E�g
    Public Const LARM_MSG012 As UShort = &H1000                         ' B12: ���g�p
    Public Const LARM_MSG013 As UShort = &H2000                         ' B13: ���g�p
    Public Const LARM_MSG014 As UShort = &H4000                         ' B14: ���g�p
    Public Const LARM_MSG015 As UShort = &H8000                         ' B15: ���g�p

    '                                                                   ' W116.00-W116.15(���s��)
    Public Const LARM_MSG016 As UShort = &H1                            ' B00: ����V�����_�^�C���A�E�g
    Public Const LARM_MSG017 As UShort = &H2                            ' B01: �n���h�P�V�����_�^�C���A�E�g
    Public Const LARM_MSG018 As UShort = &H4                            ' B02: �n���h�Q�V�����_�^�C���A�E�g
    Public Const LARM_MSG019 As UShort = &H8                            ' B03: �����n���h�z���~�X
    Public Const LARM_MSG020 As UShort = &H10                           ' B04: ����n���h�z���~�X
    Public Const LARM_MSG021 As UShort = &H20                           ' B05: �m�f�r�o���t
    Public Const LARM_MSG022 As UShort = &H40                           ' B06: �ꎞ��~
    Public Const LARM_MSG023 As UShort = &H80                           ' B07: �h�A�I�[�v��
    Public Const LARM_MSG024 As UShort = &H100                          ' B08: ���g�p
    Public Const LARM_MSG025 As UShort = &H200                          ' B09: ���g�p
    Public Const LARM_MSG026 As UShort = &H400                          ' B10: ���g�p
    Public Const LARM_MSG027 As UShort = &H800                          ' B11: ���g�p
    Public Const LARM_MSG028 As UShort = &H1000                         ' B12: ���g�p
    Public Const LARM_MSG029 As UShort = &H2000                         ' B13: ���g�p
    Public Const LARM_MSG030 As UShort = &H4000                         ' B14: ���g�p
    Public Const LARM_MSG031 As UShort = &H8000                         ' B15: ���g�p

    '----- V2.0.0.0�E�� -----
    '-------------------------------------------------------------------------------
    '   �����[�_�V���A���ʐM�p�I�t�Z�b�g��`(SL436S�p)��
    '-------------------------------------------------------------------------------
    '----- ���[�_�A���[����� -----
    Public Const LOFS_W110 As Long = 110                                ' ���[�_�A���[�����(W110.08-10)
    Public Const LOFS_W115 As Long = 115                                ' ���[�_�A���[���ڍד��e(W115.00-W115.15(���s�s��))
    Public Const LOFS_W116 As Long = 116                                ' ���[�_�A���[���ڍד��e(W116.00-W116.15(���s��))

    '----- ���[�_�A���[���ڍ�(W115) -----                                       
    Public Const LDFS_ARM_AIR As Long = &H2000                          ' B13: �����ُ팟�o
    'V5.0.0.1-25��
    Public Const LDFS_SUPPLY_MG_FULL As Long = &H4000                   ' B14: �����}�K�W���t��
    Public Const LDFS_STORE_MG_FULL As Long = &H8000                    ' B15: ���[�}�K�W���t��
    'V5.0.0.1-25��

    '----- ���[�_���ʑ���f�o�C�X -----
    Public Const LOFS_W109 As Long = 109                                ' ���[�_���ʑ���f�o�C�X(W109.00-W109.15)
    Public Const LDDV_ARM_START As Long = &H2                           ' B01: �ꎞ��~���(���s�\�G���[��)����X�^�[�g����
    Public Const LDDV_ARM_RESET As Long = &H8                           ' B03: �A���[�����Z�b�g
    Public Const LDDV_BUZER_OFF As Long = &H10                          ' B04: �u�U�[OFF
    Public Const LDDV_CLR_PRDUCT As Long = &H80                         ' B07: ���Y���N���A
    Public Const LDDV_ALM_DSP As Long = &H400                           ' B10: �A���[�����b�Z�[�W�\����(ON:�\����,OFF:�\�����łȂ�)

    '----- �����ݒ��� -----                                 
    Public Const LOFS_H00 As Long = 0                                   ' (H0.00-H0.15)
    Public Const LHST_MAGAZINE As Long = &H1                            ' B00: ON=4�}�K�W��, OFF=2�}�K�W��

    '----- �������͏�� -----
    Public Const LOFS_W40S As Long = 40                                 ' (W40.00-W40.15)
    Public Const LDABS_HAND_ZORG As Long = &H4                          ' B02: �n���h�㉺HOME(���_)

    Public Const LOFS_W42S As Long = 42                                 ' (W42.00-W42.15)
    Public Const LDSTS_CLMP_X_ON As Long = &H1                          ' B00:�N�����vX��
    Public Const LDSTS_CLMP_Y_ON As Long = &H2                          ' B01:�N�����vY��
    Public Const LDSTS_BREAK_X_ON As Long = &H4                         ' B02:�������o(X) V4.0.0.0�R
    Public Const LDSTS_BREAK_Y_ON As Long = &H8                         ' B03:�������o(Y) V4.0.0.0�R
    Public Const LDSTS_VACUME As Long = &H10                            ' B04:�z���m�F(���[�N�L)

    Public Const LOFS_W43S As Long = 43                                 ' (W43.00-W43.15)
    Public Const LDSTS_MG1_SUBTSENS As Long = &H10                      ' B04:��������o(�}�K�W��1)
    Public Const LDSTS_MG2_SUBTSENS As Long = &H20                      ' B05:���[����o(�}�K�W��2)
    Public Const LDSTS_MG_EXIST As Long = &H40                          ' B06:�}�K�W���L
    Public Const LDSTS_NGFULL As Long = &H80                            ' B07:NG�r�oBOX���t


    Public Const LOFS_W44S As Long = 44                                 ' W44.00-W44.15

    '----- W52 -----
    Public Const LOFS_W52S As Long = 52                                 ' W52.00-W52.15
    Public Const OutSMG1Move_Sw As Long = &H1                           ' B00: MG1-���� -SW 
    Public Const OutSMG1on_Sw As Long = &H2                             ' B01: MG1-�㉺�ؑւ�-Sw
    Public Const OutSMG2Move_Sw As Long = &H4                           ' B02: MG2-���� -SW 
    Public Const OutSMG2on_Sw As Long = &H8                             ' B03: MG2-�㉺�ؑւ�-Sw

    Public Const LDABS_HAND1_VACUME As Long = &H100                     ' B08: �����n���h�z�� 
    Public Const LDABS_HAND1_ABSORB As Long = &H200                     ' B09: �����n���h�z���j�� 
    Public Const LDABS_HAND2_VACUME As Long = &H400                     ' B10: ���[�n���h�z��
    Public Const LDABS_HAND2_ABSORB As Long = &H800                     ' B11: ���[�n���h�z���j��

    '----- �G�A�[�o���u�֌W -----                                 
    Public Const LOFS_W53S As Long = 53                                 ' �G�A�[�o���u�֌W(W53.00-W53.15)
    Public Const LDABS_CLMP_X As Long = &H1                             ' B00: �N�����vX (ON�Ńo���uON �ēxON�Ńo���uOFF)
    Public Const LDABS_CLMP_Y As Long = &H2                             ' B01: �N�����vY (����)
    Public Const LDABS_VACUME As Long = &H4                             ' B02: �z�� (����)
    Public Const LDABS_ABSORB As Long = &H8                             ' B03: �j�� (����)

    '----- ���[�_���ʑ���f�o�C�X ----- 
    Public Const LOFS_W54S As Long = 54                                 ' �G�A�[�o���u�֌W(W54.00-W54.15)

    Public Const LOFS_W60S As Long = 60                                 ' W60.00-W60.15
    Public Const LDABS_HAND_CW_MOVE As Long = &H1                       ' B00: �������j�b�gCW(��)�o��
    Public Const LDABS_HAND_CCW_MOVE As Long = &H2                      ' B01: �������j�b�gCCW(��)�o��
    Public Const LDABS_HAND_UP As Long = &H4                            ' B02: �n���h�㉺CW(��)�o��
    Public Const LDABS_HAND_DOWN As Long = &H8                          ' B03: �n���h�㉺CCW(��)�o��

    '----- ��Έʒu�ړ� -----
    Public Const LOFS_W128S As Long = 128                                ' ���{�b�g����֌W(W128.00-W128.15)
    Public Const POSMOVES_START_ADR As Long = &H200                      ' ����J�n�A�h���X

    '----- ���{�b�g����֌W -----                                 
    Public Const LOFS_W359S As Long = 359                                ' ���{�b�g����֌W(W359.00-W359.15)
    Public Const LROBS_HAND_HOME As Long = &H1                           ' B00: �n���h�㉺�z�[���ʒu
    Public Const LROBS_HAND_SPLY1 As Long = &H2                          ' B01: �����n���h���~����
    Public Const LROBS_HAND_SPLY2 As Long = &H4                          ' B02: �����n���h�㏸����
    Public Const LROBS_HAND_STR2 As Long = &H10                          ' B04: ���[�n���h���~����
    Public Const LROBS_HAND_STR4 As Long = &H40                          ' B06: ���[�n���h�㏸����
    Public Const LROBS_HAND_STAGE As Long = &H80                         ' B07: �n���h�㉺�r�o���~
    Public Const LROBS_HAND_NG As Long = &H100                           ' B08: �n���h�㉺�㏸����

    '----- V2.0.0.0�E�� -----

    '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
    '----- PC ���� PLC�ʐM�p(SL436S�p) -----
    Public Const LOFS_W113S As Long = 113                               ' PC �� PLC�ʐM�p(W113.00-W113.15)
    Public Const LPCS_SET_SUBSTRATE As Long = &H10                      ' B04:��ǉ��v��(0=�v����,1=�v���L)

    Public Const LOFS_W114S As Long = 114                               ' PLC �� PC�ʐM�p(W114.00-W114.15)
    Public Const LPLCS_SET_LOTSTOPREADY As Long = &H2                   ' B01: LotStop��������
    Public Const LPLCS_SET_SUBSTRATE As Long = &H4                      ' B02: ��ǉ���������

    '----- V4.11.0.0�E�� -----

    '----- V4.0.0.0-26�� -----
    '----- �i��I�� ----- 
    Public Const LOFS_D241S As Long = 241                                ' �i��I��(D241 1(�i��1)�`10(�i��10))(�����Ȃ�1WORD)
    Public Const LOFS_D242S As Long = 242                                ' ���[�o�b�e���[�A���[��(W242 0����A1:���[�o�b�e���[�A���[��

    Public Const LOFS_W360S As Long = 360                                ' �i��I��BIT(W360.00-W360.15)��LangSelectTool�Ŏg�p(TKY���ł͖��g�p)
    '                                                                    ' B00:�i��1
    '                                                                    ' B01:�i��2
    '                                                                    '    :
    '                                                                    ' B08:�i��9
    '                                                                    ' B09:�i��10
    '                                                                    ' B10-15:�\��
    '----- ���W������ -----                                 
    Public Const LOFS_W361S As Long = 361                                ' ���W������BIT(W361.00-W361.15)
    '                                                                    ' B00:�i��1
    '                                                                    ' B01:�i��2
    '                                                                    '    :
    '                                                                    ' B08:�i��9
    '                                                                    ' B09:�i��10
    '                                                                    ' B10-15:�\��
    '----- �񖇎��Z���T�m�F�ʒu -----                                 
    Public Const LOFS_D700S As Long = 700                                ' ���W�f�[�^(�����t2WORD)
    '                                                                    ' D700:�i��1
    '                                                                    ' D702:�i��2
    '                                                                    '    :
    '                                                                    ' D716:�i��9
    '                                                                    ' D718:�i��10
    '                                                                    ' B10-15:�\��

    '----- ����Ή� -----                                 
    Public Const LOFS_D230S As Long = 230                                ' ���݂̊���(D230 0=�ʏ�, 1=�����)(�����Ȃ�1WORD)

    '----- V4.0.0.0-26�� -----
    '-------------------------------------------------------------------------------
    '   �����[�_�V���A���ʐM�p�I�t�Z�b�g��`(SL436R�p)��
    '-------------------------------------------------------------------------------
    ''----- ���[�_�A���[����� -----
    'Public Const LOFS_W110 As Long = 110                                ' ���[�_�A���[�����(W110.08-10)
    Public Const LDPL_STP_AUTODRIVE As Long = &H800                     ' B11: �����^�]���f(PLC �� PC) V1.23.0.0�I
    'Public Const LOFS_W115 As Long = 115                                ' ���[�_�A���[���ڍד��e(W115.00-W115.15(���s�s��))
    'Public Const LOFS_W116 As Long = 116                                ' ���[�_�A���[���ڍד��e(W116.00-W116.15(���s��))

    ''----- ���[�_���ʑ���f�o�C�X -----
    'Public Const LOFS_W109 As Long = 109                                ' ���[�_���ʑ���f�o�C�X(W109.00-W109.15)
    'Public Const LDDV_ARM_START As Long = &H2                           ' B01: �ꎞ��~���(���s�\�G���[��)����X�^�[�g����
    'Public Const LDDV_ARM_RESET As Long = &H8                           ' B03: �A���[�����Z�b�g
    'Public Const LDDV_BUZER_OFF As Long = &H10                          ' B04: �u�U�[OFF
    'Public Const LDDV_CLR_PRDUCT As Long = &H80                         ' B07: ���Y���N���A
    'Public Const LDDV_ALM_DSP As Long = &H400                           ' B10: �A���[�����b�Z�[�W�\����(ON:�\����,OFF:�\�����łȂ�) 'V1.18.0.0�M
    Public Const LDDV_STP_AUTODRIVE As Long = &H2000                    ' B13: �����^�]���f(PC �� PLC) V1.23.0.0�I

    '----- ���[�_���ʑ���f�o�C�X -----                                 ' ###001
    Public Const LOFS_W54 As Long = 54                                  ' �G�A�[�o���u�֌W(W54.00-W54.15)
    Public Const LDAB_CLMP_X As Long = &H1                              ' B00: �N�����vX (ON�Ńo���uON �ēxON�Ńo���uOFF)
    Public Const LDAB_CLMP_Y As Long = &H2                              ' B01: �N�����vY (����)
    Public Const LDAB_VACUME As Long = &H4                              ' B02: �z�� (����)
    Public Const LDAB_ABSORB As Long = &H8                              ' B03: �j�� (����)

    ''----- ###184�� -----
    ''----- �����ݒ��� -----                                 
    'Public Const LOFS_H00 As Long = 0                                   ' (H0.00-H0.15)
    'Public Const LHST_MAGAZINE As Long = &H1                            ' B00: ON=4�}�K�W��, OFF=2�}�K�W��

    '----- �������͏�� -----
    Public Const LOFS_W42 As Long = 42                                  ' (W42.00-W42.15)
    Public Const LDST_MG1_DOWN As Long = &H1                            ' B00:�}�K�W��1���� V1.23.0.0�D
    Public Const LDST_MG1_UP As Long = &H2                              ' B01:�}�K�W��1��� V1.23.0.0�D
    Public Const LDST_MG1_SUBTSENS As Long = &H100                      ' B08:����o(�}�K�W��1)
    Public Const LDST_MG2_SUBTSENS As Long = &H200                      ' B09:����o(�}�K�W��2)
    Public Const LDST_MG3_SUBTSENS As Long = &H400                      ' B10:����o(�}�K�W��3)
    Public Const LDST_MG4_SUBTSENS As Long = &H800                      ' B11:����o(�}�K�W��4)
    '----- V1.18.0.0�H�� -----
    Public Const LDST_MG1_EXIST As Long = &H1000                        ' B12:�}�K�W��1�L
    Public Const LDST_MG2_EXIST As Long = &H2000                        ' B13:�}�K�W��2�L
    Public Const LDST_MG3_EXIST As Long = &H4000                        ' B14:�}�K�W��3�L
    Public Const LDST_MG4_EXIST As Long = &H8000                        ' B15:�}�K�W��4�L
    Public Const LDST_MG2_REVERS As Long = &H8000                       ' B14:�}�K�W��2��/�\(1/0) �����[���a����
    '----- V1.18.0.0�H�� -----

    Public Const LOFS_W44 As Long = 44                                  ' (W44.00-W44.15)
    Public Const LDST_CLMP_X_ON As Long = &H1                           ' B00:�N�����vX�� V1.16.0.0�D
    Public Const LDST_CLMP_Y_ON As Long = &H2                           ' B01:�N�����vY�� V1.16.0.0�D
    Public Const LDST_BREAK_X_ON As Long = &H4                          ' B02:�������o(X) V4.0.0.0�R
    Public Const LDST_BREAK_Y_ON As Long = &H8                          ' B03:�������o(Y) V4.0.0.0�R
    Public Const LDST_VACUME As Long = &H10                             ' B04:�z���m�F(���[�N�L)
    '                                                                   ' B05:�Q���挟�o
    Public Const LDST_NGFULL As Long = &H40                             ' B06:NG�r�oBOX���t ###197
    '----- ###184�� -----

    '----- ###144�� -----
    '----- �G�A�[�o���u�֌W -----                                 
    Public Const LOFS_W53 As Long = 53                                  ' �G�A�[�o���u�֌W(W53.00-W53.15)
    Public Const LDAB_HAND_ORGMOVE As Long = &H1                        ' B00: �n���h���񌴓_�ړ�(ON�Ńo���uON �ēxON�Ńo���uOFF)
    Public Const LDAB_HAND_MOVE As Long = &H2                           ' B01: �n���h���񓮍�ړ�(����)
    Public Const LDAB_HAND1_DOWN As Long = &H4                          ' B02: �n���h1���~ (����)
    Public Const LDAB_HAND2_DOWN As Long = &H8                          ' B03: �n���h2���~ (����)
    Public Const LDAB_HAND1_VACUME As Long = &H10                       ' B04: �n���h1�z�� (����)
    Public Const LDAB_HAND1_ABSORB As Long = &H20                       ' B05: �n���h1�j�� (����)
    Public Const LDAB_HAND2_VACUME As Long = &H40                       ' B06: �n���h2�z�� (����)
    Public Const LDAB_HAND2_ABSORB As Long = &H80                       ' B07: �n���h2�j�� (����)
    '----- ###188�� -----
    Public Const LDAB_HAND1_ZORG As Long = &H400                        ' B10: �n���h1Z���_(����)
    Public Const LDAB_HAND1_ZMOVE As Long = &H800                       ' B11: �n���h1Z����(����)
    Public Const LDAB_HAND2_ZORG As Long = &H1000                       ' B12: �n���h2Z���_(����)
    Public Const LDAB_HAND2_ZMOVE As Long = &H2000                      ' B13: �n���h2Z����(����)
    '----- ###188�� -----
    '----- ###144�� -----

    '-----#### W52 ####----- 2013.01.28 '###182  
    Public Const LOFS_W52 As Long = 52                                  ' W52�A�h���X 
    Public Const OutMG1Move_Sw As Long = &H1                            ' MG1-���� -SW 
    Public Const OutMG1on_Sw As Long = &H2                              ' MG1-�㉺�ؑւ�-Sw
    Public Const OutMG2Move_Sw As Long = &H4                            ' MG2-���� -SW 
    Public Const OutMG2on_Sw As Long = &H8                              ' MG2-�㉺�ؑւ�-Sw
    Public Const OutMG3Move_Sw As Long = &H10                           ' MG3-���� -SW 
    Public Const OutMG3on_Sw As Long = &H20                             ' MG3-�㉺�ؑւ�-Sw
    Public Const OutMG4Move_Sw As Long = &H40                           ' MG4-���� -SW 
    Public Const OutMG4on_Sw As Long = &H80                             ' MG4-�㉺�ؑւ�-Sw
    '-----#### W52 ####-----  

    '----- ###197�� -----
    '----- ���{�b�g����֌W -----                                 
    Public Const LOFS_W339 As Long = 339                                ' ���{�b�g����֌W�֌W(W339.00-W339.15)
    Public Const LROB_HAND_HOME As Long = &H1                           ' B00: �n���h�z�[���ʒu
    Public Const LROB_HAND_SPLY1 As Long = &H2                          ' B01: �n���h�����ʒu1
    Public Const LROB_HAND_SPLY2 As Long = &H4                          ' B02: �n���h�����ʒu2
    Public Const LROB_HAND_SPLY3 As Long = &H8                          ' B03: �n���h�����ʒu3
    Public Const LROB_HAND_STR2 As Long = &H10                          ' B04: ���[�ʒu2
    Public Const LROB_HAND_STR3 As Long = &H20                          ' B05: ���[�ʒu3
    Public Const LROB_HAND_STR4 As Long = &H40                          ' B06: ���[�ʒu4
    Public Const LROB_HAND_STAGE As Long = &H80                         ' B07: �n���h���ڈʒu
    Public Const LROB_HAND_NG As Long = &H100                           ' B08: NG�ʒu
    '----- ��Έʒu�ړ� -----
    Public Const LOFS_W128 As Long = 128                                ' ���{�b�g����֌W�֌W(W128.00-W128.15)
    Public Const POSMOVE_START_ADR As Long = &H200                      ' ����J�n�A�h���X

    '----- ###197�� -----

    '----- V4.0.0.0�R�� -----
    '----- PC ���� PLC�ʐM�p(SL436R�p(SL436S��I/O)) -----                                 
    Public Const LOFS_W113 As Long = 113                                ' PC �� PLC�ʐM�p(W113.00-W113.15)
    Public Const LPC_CYCL_STOP As Long = &H1                            ' B00: �T�C�N����~(SL436S��I/O)
    Public Const LPC_LOT_STOP As Long = &H4                             ' B02: LOT���f�v��(SL436S)
    Public Const LPC_LOT_STOP_PREPARE As Long = &H8                     ' B03: LOT���f���������v��(SL436S)
    Public Const LPC_LOT_STOP_SUB_EXIST As Long = &H20                  ' B05: LOT���f����L��(SL436S)
    '                                                                   ' B01: �\��
    '                                                                   '  : :
    '                                                                   ' B15: �\��

    Public Const LOFS_W114 As Long = 114                                ' PLC �� PC�ʐM�p(W114.00-W114.15)
    Public Const LPLC_CYCL_STOP As Long = &H1                           ' B00: �T�C�N����~����(SL436S��I/O)
    '                                                                   ' B01: �\��
    '                                                                   '  : :
    '                                                                   ' B15: �\��
    '----- V4.0.0.0�R�� -----

    'V4.12.2.0�E     ��'V6.0.4.1�@
    Public Const LOFS_W192 As Long = 192                                ' PC �� PLC�ʐM�p(W192)
    Public Const LPC_CLAMP_EXEC As Long = &H80                          ' B07: �N�����v���s�L��
    'V4.12.2.0�E     ��'V6.0.4.1�@

    '----- ###188�� -----
    '-------------------------------------------------------------------------------
    '   ���֌W��`
    '-------------------------------------------------------------------------------
    '----- ����� -----
    Public Const MAX_AXIS As Integer = 4                                ' ���ő吔
    'Public Const AXIS_X As Integer = 0                                  ' X�� ��TrimErrNo.vb�ɒ�`�L
    'Public Const AXIS_Y As Integer = 1                                  ' Y��
    'Public Const AXIS_Z As Integer = 2                                  ' Z��
    'Public Const AXIS_T As Integer = 3                                  ' Theta��

    '----- �T�u�X�e�[�^�X�A�h���X -----
    Public Const ADRSUB_STS_X As Integer = &H2118                       ' X��
    Public Const ADRSUB_STS_Y As Integer = &H2158                       ' Y��
    Public Const ADRSUB_STS_Z As Integer = &H2198                       ' Z��
    Public Const ADRSUB_STS_T As Integer = &H21D8                       ' Theta��

    '----- ���T�u�X�e�[�^�X�A�h���X -----
    Public AxisSubStsAry(MAX_AXIS) As Integer

    '----- �T�u�X�e�[�^�X -----                                 
    Public Const SUBSTS_ORG As Integer = &H4000                         ' B14: ORG(ORG���͎�ON)

    '----- ###240�� -----
    '----- �z�����o�A�h���X(SL432R) -----
    Public Const VACUME_STS As Integer = &H216A                         ' �z�����o�A�h���X
    Public Const VACUME_STS_BIT As Long = &H4                           ' B02: �z�����o(0=OFF/1=ON)
    '----- ###240�� -----
    '----- ###188�� -----

    '----- V2.0.0.0�L�� -----
    '----- �T�[�{�A���[��(SL432R/SL436S) -----
    '-----<�X�e�[�^�X�A�h���X>-----
    Public Const SRVALM_XY_STS As Integer = &H210A                      ' �X�e�[�^�X�A�h���X(X,Y��)
    Public Const SRVALM_ZT_STS As Integer = &H213A                      ' �X�e�[�^�X�A�h���X(Z,�Ǝ�)
    Public Const SRVALM_Z2_STS As Integer = &H21BA                      ' �X�e�[�^�X�A�h���X(Z2��)

    '-----<�X�e�[�^�r�b�g>-----
    Public Const SRVALM_X_BIT As Long = &H10                            ' B04: X���T�[�{�A���[�� (0=����/1=ALM)
    Public Const SRVALM_Y_BIT As Long = &H20                            ' B05: Y���T�[�{�A���[�� (0=����/1=ALM)

    Public Const SRVALM_Z_BIT As Long = &H40                            ' B06: Z���T�[�{�A���[�� (0=����/1=ALM)
    Public Const SRVALM_T_BIT As Long = &H80                            ' B07: �Ǝ��T�[�{�A���[�� (0=����/1=ALM)

    Public Const SRVALM_Z2_BIT As Long = &H40                           ' B06: Z2���T�[�{�A���[�� (0=����/1=ALM)
    '----- V2.0.0.0�L�� -----

    '===============================================================================
    '   �ϐ���`
    '===============================================================================
    Public Const LTIMER_COUNT As Integer = 22                           ' ���[�_���^�C�}��
    Public LTimerDT(LTIMER_COUNT) As UShort                             ' ���[�_���^�C�}�f�[�^
    Public LoaderVer As String                                          ' ���[�_�o�[�W�������
    Public gLdWDate As UShort = 0                                       ' ���[�_�����M�f�[�^(���j�^�p)
    Public gLdRDate As UShort = 0                                       ' ���[�_����M�f�[�^(���j�^�p)

    '----- ���[�_�A���[����� -----
    Private Const LALARM_COUNT As Integer = 128                         ' �ő�A���[����
    Private strLoaderAlarm(LALARM_COUNT) As String                      ' �A���[��������
    Private strLoaderAlarmInfo(LALARM_COUNT) As String                  ' �A���[�����1
    Private strLoaderAlarmExec(LALARM_COUNT) As String                  ' �A���[�����(�΍�)
    Private AlarmCount As Integer                                       ' �A���[����
    Private iBefData(7) As Integer                                      ' �A���[�����ޔ���
    Public iBefData_ClampVac(7) As Integer

    '----- �t���O�� -----
    Private bFgTimeOut As Boolean                                       ' ���[�_�ʐM�^�C���A�E�g�t���O
    Private bFgActLink As Boolean                                       ' ���[�_�RS232C�L���t���O
    Private bFgLoaderAlarmFRM As Boolean                                ' ���[�_�A���[����ʕ\�����t���O

    '----- �t�@�C���p�X�� -----
    Private Const SysParamPath As String = "C:\TRIM\tky.ini"            ' �V�X�e���p�����[�^�p�X��

    '----- PLC�ʐM�p�I�u�W�F�N�g -----
    Private m_PlcIf As DllPlcIf.DllPlcInterface

    ' 'V4.12.2.0�I��'V6.0.5.0�E
    ' CIO=6 
    Public Const LIN_MONITOR_CLAMPOUT As UShort = &H1                           ' B00: �N�����v�o�͏�� 
    Public Const LIN_MONITOR_VACOUT As UShort = &H4                             ' B02: �z���o�͏�� 
    ' 'V4.12.2.0�I��'V6.0.5.0�E

#End Region

#Region "�y�I�[�g���[�_�h�n�p���\�b�h�z"
    '===============================================================================
    '   �I�[�g���[�_�h�n����
    '===============================================================================
#Region "�I�[�g���[�_�փf�[�^���o�͂���"
    '''=========================================================================
    ''' <summary>�I�[�g���[�_�փf�[�^���o�͂���</summary>
    ''' <param name="lOn"> (INP)On Bit</param>
    ''' <param name="lOff">(INP)Off Bit</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function SetLoaderIO(ByVal lOn As UShort, ByVal lOff As UShort) As Integer

        Dim r As Integer
        Dim strMSG As String
        'V4.12.2.0�I��'V6.0.5.0�E
        Dim ReadAddress As Integer
        Dim SerialReadData As Integer
        Dim tmplOn As UShort
        Dim tmplOff As UShort

        Try

            If ((lOff And LOUT_AUTO) = LOUT_AUTO) And (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then  '�蓮�ɐ؂�ւ���
                ' CIO=6
                ReadAddress = 6
                SerialReadData = 0
                tmplOn = 0
                tmplOff = 0

                m_PlcIf.ReadPlcCIO(ReadAddress, SerialReadData)
                '�N�����vON/OFF
                If SerialReadData And LIN_MONITOR_CLAMPOUT Then
                    tmplOff = LOUT_CLAMP
                Else
                    tmplOn = LOUT_CLAMP
                End If
                '�z��ON/OFF
                If SerialReadData And LIN_MONITOR_VACOUT Then
                    tmplOn = tmplOn Or LOUT_VACCUME
                Else
                    tmplOff = tmplOff Or LOUT_VACCUME
                End If

                ' �I�[�g���[�_�o��(OnBit, OffBit)
                r = ZATLDSET(tmplOn, tmplOff)
            End If
            'V4.12.2.0�I��'V6.0.5.0�E


            ' ���[�_�����M�f�[�^�ݒ�(���j�^�p)
            gLdWDate = gLdWDate And (lOff Xor &HFFFF)                   ' Bit Off 
            gLdWDate = gLdWDate Or lOn                                  ' Bit On 

            ' �I�[�g���[�_�o��(OnBit, OffBit)
            r = ZATLDSET(lOn, lOff)

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SetLoaderIO() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�I�[�g���[�_����f�[�^����͂���"
    '''=========================================================================
    ''' <summary>�I�[�g���[�_����f�[�^����͂���</summary>
    ''' <param name="LdIn">(OUT)���̓f�[�^</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function GetLoaderIO(ByRef LdIn As UShort) As Integer

        Dim r As Integer
        Dim iData As Integer
        Dim strMSG As String

        Try
            ' �I�[�g���[�_����
            r = ZATLDGET(iData)
            'Call INP16(&H219A, stt)
            LdIn = iData                                                ' ���[�_����M�f�[�^�ݒ�
            gLdRDate = iData                                            ' ���[�_����M�f�[�^�ݒ�(���j�^�p)
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.GetLoaderIO() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�I�[�g���[�_��������"
    '''=========================================================================
    ''' <summary>�I�[�g���[�_��������</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_Init() As Integer

        Dim r As Integer
        Dim LdIn As UShort
        Dim strMSG As String

        Try
            ' �h�n���j�^��\������
#If (cIOMONITOR = 1) Then
            Form1.ObjIOMon = New FrmIOMon()                             ' �I�u�W�F�N�g����
            Call Form1.ObjIOMon.show()                                  ' �h�n���j�^�\��(���[�h���X)
#End If
            ' ��������
            bFgActLink = False                                          ' ���[�_�L���t���O������
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then                   ' SL436�n�łȂ����NOP 
                Return (cFRS_NORMAL)
            End If

            ' �I�[�g���[�_��������
            r = GetLoaderIO(LdIn)                                       ' ���[�_����
            If ((LdIn And LINP_READY) <> LINP_READY) Then               ' ���[�_�� ?
                Return (cFRS_NORMAL)
            End If
            bFgActLink = True                                           ' ���[�_�RS232C�L���t���OON

            '' �����a����Ή������[�_�ɒʒm
            'ReDim iData(0)
            'iData(0) = giCustomFlg                                  ' (1)�����a�����@(1�ȊO)�ʏ퓮��
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4020", 1, iData)

            '' ���������Ή������[�_�ɒʒm
            'iData(0) = gdOPX2DFlg                                   ' (1)�����Ή��@(1�ȊO)�ʏ퓮��
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4021", 1, iData)

            '' �ڕ���z���A���[�����o�����[�_�ɒʒm
            'iData(0) = giOPVacFlg                                   '(0)�����@(1)�L��
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4022", 1, iData)

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�I�[�g���[�_�I������"
    '''=========================================================================
    ''' <summary>�I�[�g���[�_�I������</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_Term() As Integer

        Dim strMSG As String

        Try
            ' ���[�_�[�o��(ON=�Ȃ�,OFF=�g���}�����f�B)
            'V4.5.0.0�B���@'V4.4.0.0�@            Call SetLoaderIO(LOUT_REDY, &H0)                            '###098
            Call SetLoaderIO(LOUT_REDY, LOUT_AUTO)                            'V4.4.0.0�@
            'V4.5.0.0�B��

            ' �I�u�W�F�N�g�J��
            If (Form1.ObjIOMon Is Nothing = False) Then                 ' �h�n���j�^�\�� ?
                Call Form1.ObjIOMon.Close()                             ' �I�u�W�F�N�g�J��
                Call Form1.ObjIOMon.Dispose()                           ' ���\�[�X�J��
            End If

            ''#4.12.3.0�F��
            If m_PlcIf Is Nothing = False Then
                m_PlcIf.ClosePlcFinsPort()
            End If
            ''#4.12.3.0�F��

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_Term() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���_���A����(�I�[�g���[�_/�g���})"
    '''=========================================================================
    ''' <summary>���_���A����(�I�[�g���[�_/�g���})</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>���[�_���_���A��A�g���}���_���A���s��</remarks>
    '''=========================================================================
    Public Function Loader_OrgBack(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Loader_OrgBack(ByVal ObjSys As Object) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���_���A����(�I�[�g���[�_/�g���})
            r = Sub_CallFrmRset(ObjSys, cGMODE_ORG)                 ' ���_���A���� 
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_OrgBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�I�[�g���[�_���_���A���s�T�u���[�`��"
    '''=========================================================================
    ''' <summary>�I�[�g���[�_���_���A���s�T�u���[�`��</summary>
    ''' <param name="igMode">(INP)�������[�h</param>
    ''' '                         �����L��z��
    '''                             cGMODE_ORG     = ���_���A
    '''                             cGMODE_LDR_ORG = ���[�_���_���A
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_LDR  = ���[�_�A���[�����o
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
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
            '(2012/02/21)�I�[�g���[�_���Z�b�g������ǉ�

            ' �I�[�g���[�_���_���A����
            If (bFgActLink = False) Then                                ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            ' �V�O�i���^���[����(On=���_���A��,Off=�S�ޯ�)
            If (igMode = cGMODE_LDR_ORG) Then                           ' ���[�_���_���A���[�h ?
                ' �V�O�i���^���[����(On=���_���A��,Off=�S�ޯ�) ###007
                Select Case (gSysPrm.stIOC.giSignalTower)
                    Case SIGTOWR_NORMAL                                 ' �W��(���_���A��(�Γ_��))
                        'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͖��_��)
                        ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_BLK, &HFFFF)
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ZRN)
                        'V5.0.0.9�M �� V6.0.3.0�G

                    Case SIGTOWR_SPCIAL                                 ' ����(���_���A��(���F�_��))
                        'Call Form1.System1.SetSignalTower(EXTOUT_YLW_BLK, &HFFFF)
                End Select
            End If

            ' �_�~�[�ŃV���A���ʐM���{��PLC�Ƃ̒ʐM��Ԃ��`�F�b�N����


            ' ���[�_�o��(On=�g���}�����f�B+�g���}����, Off=���L�ȊO)
            'Call SetLoaderIO(LOUT_REDY + LOUT_NO_ALM, Not (LOUT_REDY + LOUT_NO_ALM))'###098
            '###148            Call SetLoaderIO(LOUT_NO_ALM, LOUT_REDY) '###098
            Call SetLoaderIO(0, LOUT_REDY) '###098

            ' ���[�_���_���A����
            Call SetLoaderIO(LOUT_ORG_BACK, &H0)                        ' ���[�_�o��(On=���[�_���_���A�v��, Off=0)

            ' �O��̃��[�_���_���A����Bit��Off��҂�
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(Form1.System1, LINP_ORG_BACK, False, True, True, 0, 0, 0)
            If (rtnCode <> cFRS_NORMAL) Then                            ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' �y�̏�(���s�\) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then  ' ###196
                    Call W_RESET()                                      ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                      ' �X�^�[�g�M�����o 
                    GoTo STP_RETRY
                End If
                '----- V2.0.0.0_22�� -----
                If (giMachineKd = MACHINE_KD_RS) Then                   ' Sl436S���̓X�e�[�W�����_�ɖ߂��Ȃ�
                    ''V4.1.0.0�H
                    ' �g���}�^�]��(���_���A��͌���ON�̂܂܁A�ꎞ��~�̓��[�_�̊�������쒆�ɒ�~�������ꍇ�Ɏg�p(�g�p���Ȃ�?))
                    Call SetLoaderIO(LOUT_STS_RUN, LOUT_ORG_BACK)               ' ���[�_�o��(On=�g���}�^�]��, Off=���[�_���_���A�v��)
                    ''V4.1.0.0�H

                    Return (rtnCode)
                End If
                '----- V2.0.0.0_22�� -----
                'Return (rtnCode)                                       ' ###163
                GoTo STP_END                                            ' ###163
            End If

            ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRS_Tick��X msec�Ԋu�Ŏ��s����)
            Sub_SetTimeoutTimer(TimerRS)

            ' ���[�_�̌��_���A������҂�
            Do
                ' ���[�_�A���[��/����~�`�F�b�N
                Call GetLoaderIO(LdIn)                                  ' ���[�_����
                If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                    'Return (cFRS_ERR_LDR)                              ' ###163 Return�l = ���[�_�A���[�����o
                    rtnCode = cFRS_ERR_LDR                              ' ###163
                    GoTo STP_END                                        ' ###163
                End If

                ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N 
                If (bFgTimeOut = True) Then                             ' �^�C���A�E�g ?
                    ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                    TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                    rtnCode = cFRS_ERR_LDRTO                            ' Return�l = ���[�_�ʐM�^�C���A�E�g
                    GoTo STP_END
                End If

                ''V4.1.0.0�I
                ' �C���^�[���b�N��Ԃ̕\������у��[�_�֒ʒm(SL436R) ###162
                r = Form1.DispInterLockSts()
                'V4.1.0.0�I
                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While ((LdIn And LINP_ORG_BACK) <> LINP_ORG_BACK)      ' ���_���A�����҂�


            ' �I������
STP_END:
            ''V4.1.0.0�H
            ' �g���}�^�]��(���_���A��͌���ON�̂܂܁A�ꎞ��~�̓��[�_�̊�������쒆�ɒ�~�������ꍇ�Ɏg�p(�g�p���Ȃ�?))
            Call SetLoaderIO(LOUT_STS_RUN, LOUT_ORG_BACK)               ' ���[�_�o��(On=�g���}�^�]��, Off=���[�_���_���A�v��)
            ''V4.1.0.0�H

            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            If (IsNothing(TimerRS) = False) Then                        ' ###173
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' �^�C�}�[��j������
            End If

            'V5.0.0.1-21��
            '���[�_���_���A�^�C���A�E�g�̏ꍇ�ɂ́A
            If rtnCode = cFRS_ERR_LDRTO Then
                Return (rtnCode)
            ElseIf rtnCode = cFRS_ERR_LDR Then
                ' "���[�_���_���A������","",""
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH37, "", "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
                Return (rtnCode)
            End If
            'V5.0.0.1-21��

            '----- ###163�� -----
            ' ���[�_�G���[�Ȃ�X�e�[�W�����_�ɖ߂�(�c���菜������)
            If (rtnCode <> cFRS_NORMAL) Then
                ' XYZ�Ǝ�������
                r = Form1.System1.EX_SYSINIT(gSysPrm, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (�����b�Z�[�W�͕\����)
                    Return (r)
                End If

                ' ��񌴓_�ʒu�ɃX�e�[�W���ړ�����
                If (giMachineKd = MACHINE_KD_RS) Then
                    If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                        r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                        If (r < cFRS_NORMAL) Then                           ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                            Return (r)
                        End If
                    End If
                End If

            End If
            '----- ###163�� -----

            ' �V�O�i���^���[����(On=���f�B(�蓮),Off=�S�ޯ�) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(���_��)
                    'V5.0.0.9�M �� V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(0, &HEFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(���F�_��)
                    'Call Form1.System1.SetSignalTower(EXTOUT_YLW_ON, &HEFFF)
            End Select

            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_Loader_OrgBack() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�g���}���G���[����������"
    '''=========================================================================
    ''' <summary>�g���}���G���[����������</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="gMode"> (INP)�������[�h(���L��z��)
    '''                           �EcGMODE_ERR_HING   (�A��HI-NG�G���[����)
    '''                           �EcGMODE_ERR_REPROBE(�ăv���[�r���O���s)
    '''                                                          </param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_TrimError(ByVal ObjSys As SystemNET, ByVal gMode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Loader_TrimError(ByVal ObjSys As Object, ByVal gMode As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���[�_����N�Ȃ�OP
            If (bFgActLink = False) Then                            ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            ' �g���}���G���[����������
            r = Sub_CallFrmRset(ObjSys, gMode)
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_TrimError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�I�[�g���[�_�p�g���}�G���[�����������T�u���[�`��"
    '''=========================================================================
    ''' <summary>�I�[�g���[�_�p�g���}�G���[�����������T�u���[�`��</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function Sub_Loader_TrimError() As Integer

        Dim r As Integer
        Dim LdIn As UShort
        Dim iData() As UShort
        'V6.0.0.0�Q        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            ' ���[�_�����Ȃ�NOP
            If (bFgActLink = False) Then                                ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            ' �I�[�g���[�_�p�g���}�G���[����������
            ReDim iData(0)
            iData(0) = &H2                                          ' ��Ϗ����������׸� 1�ɂ��Ȃ���۰�ް�������Ȃ�����
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)    ' ��ʃC�j�V�����C�Y����
            'Call wait(0.1)
            'iData(0) = &H0                                         ' ��Ϗ����������׸� 1�ɂ��Ȃ���۰�ް�������Ȃ�����
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)    ' ��ʃC�j�V�����C�Y����

            'Call SetSignalTower(4, 0)                              '۰�ޱװ� ���Z�b�g
            'Call SetSignalTower(3, 1)                              '������ܰ���_���AON

            ' ���[�_������(ON=�g���}������, OFF=���L�ȊO) 
            '###148            Call SetLoaderIO(LOUT_NO_ALM, Not (LOUT_NO_ALM))
            Call SetLoaderIO(0, 0)

            'ReDim iData(0)
            'iData(0) = &H2                      '۰�ް�װ�ؾ��
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)
            'Call wait(0.1)
            'iData(0) = &H0                      '����Ӱ��(�蓮���)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)
            'iData(0) = &H1                      '��Ϗ����������׸�
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4000", 1, iData)

            'iData(0) = &H4                      '�����^�]��ݾ�
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)
            'Call wait(0.2)
            'iData(0) = &H0                      '����Ӱ��(�蓮���)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "M4000", 1, iData)

            ' ���[�_���_���A��/�s�擾
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "M4992", 1, iData)
            If (iData(0) And &H800) Then
                ' ���[�_���_���A�̏ꍇ
                Call SetLoaderIO(LOUT_ORG_BACK, &H0)                        ' ���[�_�o��(On=���[�_���_���A�v��, Off=0)
                r = GetLoaderIO(LdIn)                                       ' ���[�_����
                ' ���[�_�̌��_���A������҂�
                Do
                    r = GetLoaderIO(LdIn)                                   ' ���[�_����
                    If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                        Return (cFRS_ERR_LDR)                               ' Return�l = ���[�_�A���[�����o
                    End If
                    System.Windows.Forms.Application.DoEvents()
                Loop While ((LdIn And LINP_ORG_BACK) <> LINP_ORG_BACK)      ' ���[�_���_���A�����҂�
                Call SetLoaderIO(&H0, LOUT_ORG_BACK)                        ' ���[�_�o��(On=0, Off=���[�_���_���A�v��)

            Else
                ' ���[�_���_���A�s�̏ꍇ
                Return (cFRS_ERR_LDR)                                       ' Return�l = ���[�_�A���[�����o
            End If

            'Call AbsVaccume(0)
            'Call ClampCtrl(0)
            'Call Adsorption(0)
            'Call SetSignalTower(3, 0)                                      ' ������ܰ���_���AOFF

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_Loader_TrimError() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_����/�蓮���[�h�ؑւ�����"
    '''=========================================================================
    ''' <summary>���[�_����/�蓮���[�h�ؑւ�����</summary>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="bFgAutoMode">  (OUT)���[�_�������[�h�t���O</param>
    ''' <param name="Md">           (INP)�ؑւ����[�h(1=�������[�h, 0=�蓮���[�h)</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
    '''          cFRS_ERR_RST  = RESET�L�[����
    '''          cFRS_ERR_CVR  = ➑̃J�o�[�J���o
    '''          cFRS_ERR_SCVR = �X���C�h�J�o�[�J���o
    '''          cFRS_ERR_EMG  = ����~ ��
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
            '   SL432R�n���̏���
            '-------------------------------------------------------------------
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R�n ? 
                If (Md = MODE_AUTO) Then                                ' ���[�_����/�蓮���[�h�ؑւ�
                    ' ���[�_�������[�h�ؑւ���
                    '    ' �V�O�i���^���[����(On=�����^�]�� , Off=�ُ�+�޻ް1)  ���V�O�i���^���[�����OcxSystem�ōs��
                    '    Call Form1.System1.SetSignalTower(EXTOUT_GRN_ON, &HFFFF)

                    ' ���[�_�[�o��(ON=�g���}���쒆, OFF=���ݸ�NG+����ݔF��NG+��ϰ�װ) ###035
                    If gbLoaderSecondPosition Then  'V5.0.0.6�A
                        Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_NG Or COM_STS_PTN_NG Or COM_STS_TRM_ERR Or COM_STS_TRM_COMP_SUPPLY)
                    Else
                        Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_NG Or COM_STS_PTN_NG Or COM_STS_TRM_ERR)
                    End If
                Else
                    ' ���[�_�蓮���[�h�ؑւ���

                    '    ' �V�O�i���^���[����(On=���f�B(�蓮),Off=�ُ�+�޻ް1)  ���V�O�i���^���[�����OcxSystem�ōs��
                    '    Call Form1.System1.SetSignalTower(EXTOUT_YLW_ON, &HFFFF)
                End If

                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   SL436R�n���̏���
            '-------------------------------------------------------------------
            ' ���[�_�����܂��̓��[�_�蓮���[�h�Ȃ�NOP
            If ((bFgActLink = False) Or (gbFgAutoOperation = False)) Then
                Return (cFRS_NORMAL)
            End If

            ' ���[�_����/�蓮���[�h�ؑւ�
            If (Md = MODE_AUTO) Then
                ' ���[�_�������[�h�ؑւ���
                ' ���[�_�̗L��(Ready/Not Ready)���`�F�b�N����
                bFgAutoMode = False                                     ' ���[�_�������[�h�t���OOFF
                r = GetLoaderIO(LdIn)                                   ' ���[�_����
                If ((LdIn And LINP_READY) = &H0) Then                   ' ���[�_�����H
                    Return (cFRS_NORMAL)
                End If
                ' ���[�_�������[�h�ؑւ�(�}�K�W���`�F�b�N)
                Call SetLoaderIO(LOUT_AUTO, &H0)                        ' ���[�_�o��(ON=����, OFF=�Ȃ�)
            Else
                ' ���[�_�蓮���[�h�ؑւ���
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ���[�_�o��(ON=�Ȃ�, OFF=����)
            End If

            ' ���[�_����̃��[�h�ؑ֊�����҂�
            If (Md = MODE_AUTO) Then                                    ' ���[�_�������[�h�ؑւ� ?
                bOnOff = True                                           ' On�҂�
            Else                                                        ' ���[�_�蓮���[�h�ؑւ���
                bOnOff = False                                          ' Off�҂�
            End If
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_AUTO, bOnOff, True, False, 0, 0, 0)
            'If (rtnCode = cFRS_ERR_LDR3) Then                           ' �y�̏�(���s�\) ? ###196 ###073
            If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                Call W_RESET()                                          ' �A���[�����Z�b�g�M�����o 
                Call W_START()                                          ' �X�^�[�g�M�����o 
                GoTo STP_RETRY
            End If

            ' ���[�_�������[�h�t���O�ݒ�
            If (rtnCode = cFRS_NORMAL) Then
                If (Md = MODE_AUTO) Then
                    ' �V�O�i���^���[����(On=�����^�]��,Off=�S�ޯ�) ###007
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                                     ' �W��(�Γ_��)
                            'V5.0.0.9�M �� V6.0.3.0�G(���[���a�d�l�͗Γ_��)
                            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                            'V5.0.0.9�M �� V6.0.3.0�G

                        Case SIGTOWR_SPCIAL                                     ' ����(�Γ_��)
                            'Call Form1.System1.SetSignalTower(EXTOUT_GRN_ON, &HFFFF)
                    End Select
                    bFgAutoMode = True                                  ' ���[�_�������[�h�t���OON
                Else
                    ' �V�O�i���^���[����(On=���f�B, Off=�S��) ###007
                    Select Case (gSysPrm.stIOC.giSignalTower)
                        Case SIGTOWR_NORMAL                             ' �W��(���_��)
                            'V5.0.0.9�M �� V6.0.3.0�G
                            ' Call Form1.System1.SetSignalTower(0, &HFFFF)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                            'V5.0.0.9�M �� V6.0.3.0�G

                        Case SIGTOWR_SPCIAL                             ' ����(���F�_��)
                            'Call Form1.System1.SetSignalTower(EXTOUT_YLW_ON, &HFFFF)
                    End Select
                    bFgAutoMode = False                                 ' ���[�_�������[�h�t���OOFF
                End If
            End If
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_ChangeMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�g���~���O�O����"
    '''=========================================================================
    ''' <summary>�g���~���O�O����</summary>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="bFgAutoMode">  (INP)���[�_�������[�h�t���O</param>
    ''' <param name="dblCSx">       (INP)�`�b�v�T�C�YX</param>
    ''' <param name="dblCSy">       (INP)�`�b�v�T�C�YY</param>
    ''' <param name="iTrimNgMAX">   (INP)�A���g���~���O�m�f�������</param>
    ''' <param name="iTrimNgCount"> (INP)�g���~���O�m�f�J�E���^(���)</param>
    ''' <param name="iTrimNgBoxCnt">(INP)�������r�o�J�E���^(���)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_TrimPreStart(ByVal ObjSys As SystemNET, ByVal bFgAutoMode As Boolean, ByVal dblCSx As Double, ByVal dblCSy As Double, _
                                        ByVal iTrimNgMAX As Integer, ByVal iTrimNgCount As Integer, ByVal iTrimNgBoxCnt As Integer) As Integer 'V6.0.0.0�Q
        'Public Function Loader_TrimPreStart(ByVal ObjSys As Object, ByVal bFgAutoMode As Boolean, ByVal dblCSx As Double, ByVal dblCSy As Double, _
        '                                    ByVal iTrimNgMAX As Integer, ByVal iTrimNgCount As Integer, ByVal iTrimNgBoxCnt As Integer) As Integer

        'Dim iData() As UShort
        Dim strMSG As String

        Try
            ' ���[�_�����܂��̓��[�_�蓮���[�h�Ȃ�NOP
            If (bFgActLink = False) Or (bFgAutoMode = False) Then
                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   �`�b�v�T�C�Y��0606�ȉ��̓��[�_�ɕi��f�[�^���M(STB)����
            '-------------------------------------------------------------------
            ' �璹�w�莞�̓`�b�v�T�C�Y��ݒ肵�Ȃ���(�`�b�v�T�C�Y=�`�b�v�T�C�Y���u���b�N��) ###136
            If (typPlateInfo.intDirStepRepeat = STEP_RPT_CHIPXSTPY) Then        ' �X�e�b�v�����s�[�g = �`�b�v��+X����
                dblCSx = typPlateInfo.dblChipSizeXDir / typPlateInfo.intBlockCntXDir
            ElseIf (typPlateInfo.intDirStepRepeat = STEP_RPT_CHIPYSTPX) Then    ' �X�e�b�v�����s�[�g = �`�b�v��+X����
                dblCSy = typPlateInfo.dblChipSizeYDir / typPlateInfo.intBlockCntYDir
            End If

            ' ###128
            If (((dblCSx <= (0.6 + 0.2)) And (dblCSy <= 0.3 + 0.1)) Or _
                    ((dblCSx <= (0.3 + 0.1)) And (dblCSy <= 0.6 + 0.2))) Then
                Call SetLoaderIO(LOUT_STB, &H0)                         ' ���[�_�o��(ON=�i��f�[�^���M(STB), OFF=�Ȃ�)
            Else
                Call SetLoaderIO(&H0, LOUT_STB)                         ' ���[�_�o��(ON=�Ȃ�, OFF=�i��f�[�^���M(STB))
            End If

            '' �A���g���~���O�m�f��������Z�b�g
            'ReDim iData(0)
            'iData(0) = iTrimNgMAX
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4004", 1, iData) ' D1004 �A�����ݸ�NG�������

            '' �g���~���O�m�f�J�E���^(���)�Z�b�g
            'iData(0) = iTrimNgCount
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4016", 1, iData)

            '' �������r�o�J�E���^(���)�Z�b�g
            'iData(0) = iTrimNgBoxCnt
            'Call QCPUQ_ActWrite(ActQCPUQ, ActSupport, "D4015", 1, iData)

            '' ���ꌇ���r�o����(M4003)/NG�r�o����(M4004)�ر
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

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_TrimPreStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_����̃g���~���O�X�^�[�g�҂�����(�g���~���O���s���p)"
    '''=========================================================================
    ''' <summary>���[�_����̃g���~���O�X�^�[�g�҂�����(�g���~���O���s���p)</summary>
    ''' <remarks>���[�_�֊�v���𑗐M���A���[�_����̃g���~���O�X�^�[�g�M����҂�</remarks>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="bFgAutoMode">  (INP)���[�_�������[�h�t���O</param>
    ''' <param name="iTrimResult">  (INP)�g���~���O����(�O��)
    '''                                   cFRS_NORMAL   = ����
    '''                                   cFRS_TRIM_NG  = �g���~���ONG
    '''                                   cFRS_ERR_PTN  = �p�^�[���F���G���[</param>
    ''' <param name="bFgMagagin">   (OUT)�}�K�W���I���t���O</param>
    ''' <param name="bFgAllMagagin">(OUT)�S�}�K�W���I���t���O</param>
    ''' <param name="bFgLot">       (OUT)���b�g�ؑւ��v���t���O</param>
    ''' <param name="bIniFlg">      (INP)�����t���O(0=����, 1=�g���~���O��,
    '''                                             2=�S�}�K�W���I��(���g�p), 3=�ŏI��̎�o)
    '''                                                                                        </param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
    '''          cFRS_ERR_RST  = RESET�L�[����
    '''          cFRS_ERR_CVR  = ➑̃J�o�[�J���o
    '''          cFRS_ERR_SCVR = �X���C�h�J�o�[�J���o
    '''          cFRS_ERR_EMG  = ����~ ��</returns> 
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
            ' ���[�_�����܂��̓��[�_�蓮���[�h�Ȃ�NOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   ����ȊO�Ȃ��r�o�������s��
            '-------------------------------------------------------------------
            If (bIniFlg <> 0) Then                                      ' ����ȊO ?
                rtnCode = Loader_WaitDischarge(ObjSys, bFgAutoMode, iTrimResult, bFgMagagin, bFgAllMagagin, bFgLot)
                '----- V4.0.0.0�R�����[���a����(SL436R/SL436S) -----
                If (rtnCode = cFRS_ERR_START) Then                      ' �T�C�N����~�Ŋ�Ȃ����s�w�� ?
                    GoTo STP_010                                        '  
                End If
                '----- V4.0.0.0�R�� -----
                If (rtnCode <> cFRS_NORMAL) Then                        ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                    Return (rtnCode)
                End If
                If (bIniFlg = 2) Then                                   ' �S�}�K�W���I���Ȃ�I��
                    Return (rtnCode)
                End If
            End If
STP_010:    ' V4.0.0.0�R
            ' �O��̃g���~���O�X�^�[�g�v��Bit/�r�o�s�b�N����Bit��Off��҂�
            If (bIniFlg = 0) Then                                       ' ���� ?
                '----- V4.0.0.0-26�� -----
                ' �i��ԍ��𑗏o����(����̂�)
                r = W_SubstrateType(1)
                '----- V4.0.0.0-26�� -----
                WaitBit = LINP_TRM_START
                OnBit = LOUT_SUPLY + LOUT_STOP                          ' ���[�_�o��BIT = ��v��+�g���}����~��
                '###148 OffBit = LOUT_DISCHRAGE                         ' OffBit = �����ʒu������
                OffBit = LOUT_DISCHRAGE + LOUT_PROC_CONTINUE            ' OffBit = �����ʒu������
            Else
                WaitBit = LINP_TRM_START
                OnBit = LOUT_DISCHRAGE + LOUT_STOP                      ' ���[�_�o��BIT = �����ʒu������+�g���}����~��
                '###148 OffBit = LOUT_SUPLY                             ' OffBit = ��v��
                OffBit = LOUT_SUPLY + LOUT_PROC_CONTINUE                ' OffBit = ��v��
            End If
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(ObjSys, WaitBit, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            If (rtnCode <> cFRS_NORMAL) Then                            ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' �y�̏�(���s�\) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                      ' �X�^�[�g�M�����o 
                    GoTo STP_RETRY
                End If
                Return (rtnCode)
            End If

            '-------------------------------------------------------------------
            '   �e�[�u����������ʒu�Ɉړ�����
            '-------------------------------------------------------------------
            If (bIniFlg <> 3) Then                                      ' �ŏI��̎�o�Ȃ�NOP V2.0.0.0�R
                Idx = typPlateInfo.intWorkSetByLoader - 1               ' Idx = ��i��ԍ� - 1
                If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then 'V5.0.0.9�G
                    ' TODO: ��������
                    ROUND4(gdClampLessTheta)    'V5.0.0.9�G  0.0����gdClampLessTheta�ɕύX
                    'V6.0.2.0�E��
                ElseIf (0 <> gSysPrm.stDEV.giTheta) AndAlso (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    '�X�e�[�W�ւ̊�������ɂ́A�p�x�w����g�p�ł���悤�ɂ���
                    ROUND4(gdClampLessTheta)    'V5.0.0.9�G  gdClampLessTheta�ɕύX
                    'V6.0.2.0�E��
                End If
                r = SMOVE2(gfBordTableInPosX(Idx), gfBordTableInPosY(Idx))
                If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                    rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' ���b�Z�[�W�\��
                    Return (rtnCode)
                End If
            End If                                                      ' V2.0.0.0�R

            '-------------------------------------------------------------------
            '   NG�r�oBOX���t�Ȃ烁�b�Z�[�W��\������ ###089
            '-------------------------------------------------------------------
            'If (iTrimResult <> cFRS_NORMAL) Then                        ' �g���~���O����(�O��)������łȂ����
            '    giNgBoxCounter = giNgBoxCounter + 1                     ' NG�r�oBOX�̎��[�����J�E���^�[��+ 1����
            '    DspNGTrayCount()    '###130
            'End If###148
            ' ###130
            r = JudgeNGtrayCount(Idx)                                   ' ###181 2013.01.24
            If (r < cFRS_NORMAL) Then Return (r) '                      ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 

            '' NG�r�oBOX���t ?
            'If (giNgBoxCounter > giNgBoxCount(Idx)) Then                ' NG�r�oBOX�̎��[�����J�E���^�[ > NG�r�oBOX�̎��[���� ?
            '    giAppMode = APP_MODE_LDR_ALRM                           ' �A�v�����[�h = ���[�_�A���[�����(�J�o�[�J�̃G���[�Ƃ��Ȃ���)
            '    Call COVERCHK_ONOFF(COVER_CHECK_OFF)                    ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ��� ###088

            '    ' "�m�f�r�o�{�b�N�X���t","�m�f�r�o�{�b�N�X����m�f�����菜���Ă���","START�L�[���������AOK�{�^���������ĉ������B"
            '    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
            '            MSG_LOADER_21, MSG_LOADER_20, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            '    If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 

            '    giAppMode = APP_MODE_TRIM                               ' �A�v�����[�h = �g���~���O��
            '    Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A ###088
            '    Call COVERCHK_ONOFF(COVER_CHECK_ON)                     ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ��� ###088
            '    giNgBoxCounter = 0                                      ' �uNG�r�oBOX�̎��[�����J�E���^�[�v������������
            'End If

            '-------------------------------------------------------------------
            '   ����    ����v���M���𑗐M���A�g���~���O�X�^�[�g�v����҂�
            '   ����ȊO�������ʒu�������M���𑗐M���A�g���~���O�X�^�[�g�v����҂�
            '-------------------------------------------------------------------
            ' ���[�_�֊�v���M��(����)�܂��͋����ʒu������(����ȊO)�𑗐M���� (�g���~���ONG, �p�^�[���F���G���[�͊�v���M���Ɠ����ɏo�͂���)
            OffBit = OffBit + LOUT_NG_DISCHRAGE + LOUT_TRM_NG           ' �u�m�f��r�o�v���v/ [�g���~���ONG](BIT��OFF����) ###089
            If (iTrimResult <> cFRS_NORMAL) Then                        ' ��P�ʂ̃g���~���O����(�O��)������łȂ����
                OnBit = OnBit + LOUT_NG_DISCHRAGE                       ' �u�m�f��r�o�v���vBIT��ON����
            End If
            If (iTrimResult = cFRS_TRIM_NG) Then                        ' ��P�ʂ̃g���~���O����(�O��) = �g���~���ONG ?
                OnBit = OnBit + LOUT_TRM_NG
            ElseIf (iTrimResult = cFRS_ERR_PTN) Then                    ' ��P�ʂ̃g���~���O����(�O��) = �p�^�[���F���G���[ ?
                'OnBit = OnBit + LOUT_PTN_NG                            ' ###070
                OnBit = OnBit + LOUT_TRM_NG                             ' ###070
            End If
            OffBit = OffBit And Not LOUT_SUPLY                          ' ��v��(�A���^�]�J�n)��OFF���Ȃ�  
            Call SetLoaderIO(OnBit, OffBit)                             ' ���[�_�o��(ON=��v���܂��͋����ʒu������+��ϒ�~��+��, OFF=�����ʒu�������܂��͊�v��)

            ' ���[�_����̃g���~���O�X�^�[�g�v����҂�
STP_RETRY2:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_TRM_START, True, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            'If (rtnCode = cFRS_ERR_LDR3) Then                           ' �y�̏�(���s�\) ? ###196 ###073
            If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                Call W_RESET()                                          ' �A���[�����Z�b�g�M�����o 
                Call W_START()                                          ' �X�^�[�g�M�����o 
                GoTo STP_RETRY2
            End If
            If (rtnCode = cFRS_NORMAL) Then                             ' ���� ? (���G���[�������̃��b�Z�[�W�͕\����)
                ' ���[�_�փg���}���쒆(��ϒ�~OFF)�𑗐M����  
                OnBit = OnBit And Not LOUT_SUPLY                        ' ��v��(�A���^�]�J�n)��OFF���Ȃ�  
                Call SetLoaderIO(&H0, OnBit)                            ' ���[�_�o��(ON=�Ȃ�, OFF=��v��+��ϒ�~��+��)
            End If
            ' ###130
            r = JudgeNGtrayCount(Idx)
            If (r < cFRS_NORMAL) Then Return (r) '                      ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)  ' ###181 2013.01.24

            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_WaitTrimStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_����̊�r�o�����҂�����(�g���~���O���s���p)"
    '''=========================================================================
    ''' <summary>���[�_����̊�r�o�����҂�����(�g���~���O���s���p)</summary>
    ''' <remarks>���[�_�֊�r�o�v���𑗐M���A���[�_����̊�r�o�����M����҂�</remarks>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="bFgAutoMode">  (INP)���[�_�������[�h�t���O</param>
    ''' <param name="iTrimResult">  (INP)�g���~���O����(�O��)
    '''                                   cFRS_NORMAL   = ����
    '''                                   cFRS_TRIM_NG  = �g���~���ONG
    '''                                   cFRS_ERR_PTN  = �p�^�[���F���G���[</param>
    ''' <param name="bFgMagagin">   (OUT)�}�K�W���I���t���O</param>
    ''' <param name="bFgAllMagagin">(OUT)�S�}�K�W���I���t���O</param>
    ''' <param name="bFgLot">       (OUT)���b�g�ؑւ��v���t���O</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_START= ����(�T�C�N����~�Ŋ�Ȃ����s�w��) V4.0.0.0�R
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
    '''          cFRS_ERR_RST  = RESET�L�[����
    '''          cFRS_ERR_CVR  = ➑̃J�o�[�J���o
    '''          cFRS_ERR_SCVR = �X���C�h�J�o�[�J���o
    '''          cFRS_ERR_EMG  = ����~ ��</returns> 
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
            ' ���[�_�����܂��̓��[�_�蓮���[�h�Ȃ�NOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If
            ' ###148
            ' NG�g���C�ɒu���Ă����画����s��
            r = DspNGTrayChk()

            ' �O��̔r�o�s�b�N����Bit��Off��҂�
STP_RETRY:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_DISCHRAGE, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            If (rtnCode <> cFRS_NORMAL) Then                            ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' �y�̏�(���s�\) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                      ' �X�^�[�g�M�����o 
                    GoTo STP_RETRY
                End If
                Return (rtnCode)
            End If
            'V4.11.0.0�G
            'V4.9.0.0�@�� 'High,Low�̗��������Ȃ����ꍇ�̒�~��ʗp
            'V4.10.0.0�I            If gKeiTyp = KEY_TYPE_RS Then                                   ' �V���v���g���}�̏ꍇ
            If (gMachineType = MACHINE_TYPE_436S) Then                                              ' �V���v���g���}�̏ꍇ
                'V4.9.0.0�@
                If giNgStop = 1 Then
                    If JudgeNgRate.CheckTimmingPlate = True Then
                        r = sub_JudgeLotStop()
                        If r = cFRS_ERR_RST Then
                            ' �����͉�ʂŒ��f�������ꂽ�Ƃ��̂�
                            iTrimResult = cFRS_TRIM_NG
                        ElseIf r = cFRS_ERR_LDRTO Then
                            Return r
                            'V5.0.0.1-31��
                        ElseIf r = cFRS_ERR_LDR Then
                            Return r
                            'V5.0.0.1-31��
                        End If
                    End If
                End If
            End If
            'V4.9.0.0�@��'V4.11.0.0�G

            '-------------------------------------------------------------------
            '   �e�[�u������r�o�ʒu�Ɉړ�����
            '-------------------------------------------------------------------
            Idx = typPlateInfo.intWorkSetByLoader - 1                   ' Idx = ��i��ԍ� - 1
            If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then 'V5.0.0.9�G
                'V6.0.5.0�D��
                'V4.12.2.0�D                ROUND4(gdCorrectTheta + gdClampLessTheta)               '5.0.1.0�H
                'V4.12.2.0�D                r = SMOVE2(gfBordTableOutPosX(Idx) + gfCorrectPosX - gdClampLessOffsetX,
                'V4.12.2.0�D                           gfBordTableOutPosY(Idx) + gfCorrectPosY - gdClampLessOffsetY) '#5.0.1.0�G
                'V6.1.2.0�C��
                If giClampLessOutPos = 0 Then
                    ROUND4(gdClampLessTheta)                                    'V4.12.2.0�D ���R�Ȋw�a�d�l�ɂ��N�����v���X����
                    r = SMOVE2(gfBordTableInPosX(Idx), gfBordTableInPosY(Idx))  'V4.12.2.0�D �r�o�ʒu�������ʒu�Ɠ����ɂ���
                Else
                    ROUND4(gdClampLessTheta)
                    r = SMOVE2(gfBordTableOutPosX(Idx) + gdClampLessOffsetX, gfBordTableOutPosY(Idx) + gdClampLessOffsetY)
                End If
                'V6.0.5.0�D��
                'V6.1.2.0�C��
            Else
                'V6.0.2.0�E��
                If (0 <> gSysPrm.stDEV.giTheta) AndAlso (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    'SL436R�ŃƗL�̏ꍇ�ɂ́A�w��p�x�ɉ�]����
                    'V6.0.5.0�D'                    ROUND4(gdCorrectTheta + gdClampLessTheta)
                    ROUND4(gdClampLessTheta)       'V6.0.5.0�D
                End If
                'V6.0.2.0�E��

                r = SMOVE2(gfBordTableOutPosX(Idx), gfBordTableOutPosY(Idx))
                End If
                If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' ���b�Z�[�W�\��
                Return (rtnCode)
            End If


            ''V4.9.0.0�@�� 'High,Low�̗��������Ȃ����ꍇ�̒�~��ʗp'V4.11.0.0�G
            ''V4.10.0.0�I            If gKeiTyp = KEY_TYPE_RS Then                                   ' �V���v���g���}�̏ꍇ
            'If (gMachineType = MACHINE_TYPE_436S) Then                                              ' �V���v���g���}�̏ꍇ
            '    'V4.9.0.0�@
            '    If giNgStop = 1 Then
            '        If JudgeNgRate.CheckTimmingPlate = True Then
            '            r = sub_JudgeLotStop()
            '            If r = cFRS_ERR_RST Then
            '                ' �����͉�ʂŒ��f�������ꂽ�Ƃ��̂�
            '                iTrimResult = cFRS_TRIM_NG
            '            End If
            '        End If
            '    End If
            'End If
            ''V4.9.0.0�@��'V4.11.0.0�G

            '----- V4.0.0.0�R�� -----
            '-------------------------------------------------------------------
            '   �T�C�N����~����(���[���a����(SL436R/SL436S))
            '-------------------------------------------------------------------
            If (bFgCyclStp = True) Then                                 ' �T�C�N����~�t���O ON ?
                r = CycleStop_Proc(ObjSys)                              ' �T�C�N����~����
                Call LAMP_CTRL(LAMP_HALT, False)                        ' �T�C�N����~�������I�������HALT�����v�͏�������
                bFgCyclStp = False              ' �T�C�N����~�t���OOFF
                Form1.btnCycleStop.Text = "CYCLE STOP OFF"                          'V5.0.0.4�@
                Form1.btnCycleStop.BackColor = System.Drawing.SystemColors.Control  'V5.0.0.4�@
                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                If ((r = cFRS_ERR_START) Or (r = cFRS_ERR_RST)) Then    ' ��Ȃ����s�܂���Cancel(RESET�L�[����) ?
                    Return (r)                                          ' Return�l = cFRS_ERR_START(��Ȃ����s), cFRS_ERR_RST(Cancel(RESET�L�[����))
                End If
            End If                                                      ' r = cFRS_NORMAL(����葱�s)�Ȃ珈�����s
            '----- V4.0.0.0�R�� -----

            '-------------------------------------------------------------------
            '   ��v���M��(����ȊO)�𑗐M���A�r�o�s�b�N������҂�
            '-------------------------------------------------------------------
            ' ��v���M���𑗐M����O�Ƀ��[�_����~��Bit��ON��҂�
STP_RETRY2:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_STOP, True, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            If (rtnCode <> cFRS_NORMAL) Then                            ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                'If (rtnCode = cFRS_ERR_LDR3) Then                       ' �y�̏�(���s�\) ? ###196 ###073
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                      ' �X�^�[�g�M�����o 
                    GoTo STP_RETRY2
                End If
                Return (rtnCode)
            End If

            ' ���[�_�֊�v���M��(����ȊO)�𑗐M���� (�g���~���ONG, �p�^�[���F���G���[�͊�v���M���Ɠ����ɏo�͂���)
            'OnBit = LOUT_SUPLY + LOUT_STOP                             ' ���[�_�o��BIT = ��v��+�g���}����~��
            '###127�@��
            If (bFgAllMagagin = True) Then
                OnBit = LOUT_REQ_COLECT + LOUT_STOP                     ' ���[�_�o��BIT = �����v��+�g���}����~��
                OffBit = LOUT_SUPLY
            Else
                OnBit = LOUT_SUPLY + LOUT_REQ_COLECT + LOUT_STOP        ' ���[�_�o��BIT = ��v��+�����v��+�g���}����~��
                OffBit = 0
            End If
            If (iTrimResult <> cFRS_NORMAL) Then                        ' ��P�ʂ̃g���~���O����(�O��)������łȂ����
                OnBit = OnBit + LOUT_NG_DISCHRAGE                       ' �u�m�f��r�o�v���vBIT��ON����
            End If
            If (iTrimResult = cFRS_TRIM_NG) Then                        ' ��P�ʂ̃g���~���O����(�O��) = �g���~���ONG ?
                OnBit = OnBit + LOUT_TRM_NG
            ElseIf (iTrimResult = cFRS_ERR_PTN) Then                    ' ��P�ʂ̃g���~���O����(�O��) = �p�^�[���F���G���[ ?
                'OnBit = OnBit + LOUT_PTN_NG                            ' ###070
                OnBit = OnBit + LOUT_TRM_NG                             ' ###070
            End If
            Call SetLoaderIO(OnBit, OffBit)                             ' ���[�_�o��(ON=��v��+��ϒ�~��+��, OFF=�Ȃ�)
            '###127  ��          Call SetLoaderIO(OnBit, &H0)                                ' ���[�_�o��(ON=��v��+��ϒ�~��+��, OFF=�Ȃ�)

            ' ���[�_����̔r�o�s�b�N������҂�
STP_RETRY3:
            rtnCode = Sub_WaitLoaderData(ObjSys, LINP_DISCHRAGE, True, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
            'If (rtnCode = cFRS_ERR_LDR3) Then                           ' �y�̏�(���s�\) ? ###196 ###073
            If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                Call W_RESET()                                          ' �A���[�����Z�b�g�M�����o 
                Call W_START()                                          ' �X�^�[�g�M�����o 
                GoTo STP_RETRY3
            End If
            If (rtnCode = cFRS_NORMAL) Then                             ' ���� ? (���G���[�������̃��b�Z�[�W�͕\����)
                ' ���[�_�փg���}���쒆(��ϒ�~OFF)�𑗐M����  
                OnBit = OnBit And Not LOUT_SUPLY                        ' ��v��(�A���^�]�J�n)��OFF���Ȃ�  
                Call SetLoaderIO(&H0, OnBit)                            ' ���[�_�o��(ON=�Ȃ�, OFF=��v��+��ϒ�~��+��)
            End If
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_WaitDischarge() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_����̃}�K�W���I��,�S�}�K�W���I��,���b�g�ؑւ��v���`�F�b�N"
    '''=========================================================================
    ''' <summary>���[�_����̃}�K�W���I��,�S�}�K�W���I��,���b�g�ؑւ��v���`�F�b�N</summary>
    ''' <param name="bFgMagagin">   (OUT)�}�K�W���I���t���O</param>
    ''' <param name="bFgAllMagagin">(OUT)�S�}�K�W���I���t���O</param>
    ''' <param name="bFgLot">       (OUT)���b�g�ؑւ��v���t���O</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
    '''          cFRS_ERR_RST  = RESET�L�[����
    '''          cFRS_ERR_CVR  = ➑̃J�o�[�J���o
    '''          cFRS_ERR_SCVR = �X���C�h�J�o�[�J���o
    '''          cFRS_ERR_EMG  = ����~ ��</returns> 
    '''=========================================================================
    Private Function LoaderBitCheck(ByRef bFgMagagin As Boolean, ByRef bFgAllMagagin As Boolean, ByRef bFgLot As Boolean) As Integer

        Dim r As Integer
        Dim LdIn As UShort
        Dim strMSG As String

        Try
            ' �S�}�K�W���I���`�F�b�N
            r = GetLoaderIO(LdIn)                                       ' ���[�_����
            If (LdIn And LINP_END_ALL_MAGAZINE) Then                    ' �S�}�K�W���I���H
                bFgAllMagagin = True
            Else
                bFgAllMagagin = False
            End If

            ' �}�K�W���I���`�F�b�N
            If (bFgMagagin = True) Then                                 ' �}�K�W���I���t���OON ?
                ' �O��̃}�K�W���I��Bit��Off��҂�
STP_RETRY:
                r = Sub_WaitLoaderData(Form1.System1, LINP_END_MAGAZINE, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                    'If (r = cFRS_ERR_LDR3) Then                         ' �y�̏�(���s�\) ? ###196 ###073
                    If (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDR2) Then  ' ###196
                        Call W_RESET()                                  ' �A���[�����Z�b�g�M�����o 
                        Call W_START()                                  ' �X�^�[�g�M�����o 
                        GoTo STP_RETRY
                    End If
                    Return (r)
                End If
            End If
            r = GetLoaderIO(LdIn)                                       ' ���[�_����
            If (LdIn And LINP_END_MAGAZINE) Then                        ' �}�K�W���I���H
                bFgMagagin = True
            Else
                bFgMagagin = False
            End If

            ' ���b�g�ؑւ��v���`�F�b�N  
            If (bFgLot = True) Then                                     ' ���b�g�ؑւ��v���t���OON ?
                ' �O��̃��b�g�ؑւ�Bit��Off��҂�
STP_RETRY2:
                r = Sub_WaitLoaderData(Form1.System1, LINP_LOT_CHG, False, True, False, bFgMagagin, bFgAllMagagin, bFgLot)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? (���G���[�������̃��b�Z�[�W�͕\����)
                    'If (r = cFRS_ERR_LDR3) Then                         ' �y�̏�(���s�\) ? ###196 ###073
                    If (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDR2) Then  ' ###196
                        Call W_RESET()                                  ' �A���[�����Z�b�g�M�����o 
                        Call W_START()                                  ' �X�^�[�g�M�����o 
                        GoTo STP_RETRY2
                    End If
                    Return (r)
                End If
            End If
            r = GetLoaderIO(LdIn)                                       ' ���[�_����
            If (LdIn And LINP_LOT_CHG) Then                             ' ���b�g�ؑւ��v�� �H
                bFgLot = True
            Else
                bFgLot = False
            End If

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.LoaderBitCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�����^�]�I�����b�Z�[�W�\������уV�O�i���^���[����(�S�}�K�W���I��,�����^�]OFF)"
    '''=========================================================================
    ''' <summary>�����^�]�I�����b�Z�[�W�\��(�����^�]��)�����
    '''          �V�O�i���^���[����(�S�}�K�W���I��,�����^�]OFF)</summary>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="bFgAutoMode">  (OUT)���[�_�������[�h�t���O</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_EMG  = ����~</returns> 
    '''=========================================================================
    Public Function Loader_EndAutoDrive(ByVal ObjSys As SystemNET, ByRef bFgAutoMode As Boolean) As Integer 'V6.0.0.0�Q 
        'V6.0.0.0�Q    Public Function Loader_EndAutoDrive(ByVal ObjSys As Object, ByRef bFgAutoMode As Boolean) As Integer

        Dim r As Integer
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            ' �V�O�i���^���[����
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R�n ? 
                ' SL436R�n��
                ' �V�O�i���^���[����(On=�S�}�K�W���I��, Off=�S�ޯ�)
                If (bFgAutoMode = True) And (giErrLoader = 0) Then      ' ###073 ###064

                    'V5.0.0.9�M ���@V6.0.3.0�G
                    ''----- V4.0.0.0-25�� -----
                    '' �S�}�K�W���I���̃V�O�i���^���[����
                    'If (giMachineKd = MACHINE_KD_RS) Then
                    '    ' �V�O�i���^���[����(On=�Γ_��+�u�U�[�P, Off=�S�ޯ�) SL436S��
                    '    Call Form1.System1.SetSignalTower(SIGOUT_GRN_BLK Or SIGOUT_BZ1_ON, &HFFFF)

                    'Else
                    '    ' �V�O�i���^���[����(On=�ԓ_��+�u�U�[�P, Off=�S�ޯ�) SL436R��
                    '    Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    'End If

                    ''Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)  ' �W��((�ԓ_��+�u�U�[�P)) ###007
                    ''----- V4.0.0.0-25�� -----
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)
                    'V5.0.0.9�M ���@V6.0.3.0�G

                End If

            Else
                ' SL432R�n��

            End If

            ' �����^�]�I�����b�Z�[�W�\��(�����^�]��)
            If (bFgAutoMode = True) And (giErrLoader = 0) Then          ' ###073
                r = Sub_CallFrmRset(ObjSys, cGMODE_LDR_END)             ' �����^�]�I�����b�Z�[�W�\�� & START�L�[�҂�
                'V5.0.0.9�M ���@V6.0.3.0�G
                'Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)  ' �ԓ_��+�u�U�[�POFF  V4.0.0.0-25 ###159
                '                Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON Or SIGOUT_GRN_BLK)     ' V4.0.0.0-25
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                'V5.0.0.9�M ���@V6.0.3.0�G

                If (r < cFRS_NORMAL) Then                               ' START�L�[�����ȊO ?
                    rtnCode = r                                         ' Return�l�ݒ� 
                End If
            End If

            ' �I������
STP_END:
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_EndAutoDrive() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_���^�C�}�[�l�Ǎ��ݏ���"
    '''=========================================================================
    ''' <summary>���[�_���^�C�}�[�l�Ǎ��ݏ���</summary>
    ''' <param name="LTimerDT">(OUT)�^�C�}�[�l�Ǎ��݈�</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_GetTimerValue(ByRef LTimerDT() As UShort) As Integer

        'Dim iData() As UShort
        Dim strMSG As String

        Try
            If (bFgActLink = False) Then                            ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            '' D4032 �������t�����n���h1��ډ��~�@���@�z��OFF
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4032", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4033 �������t�����n���h�z��OFF�@���@�ڕ���N�����v
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4033", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4034 �������t�ڕ���N�����v�@���@�����n���h�㏸
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4034", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' �� 'D4035 �������t�����n���h�㏸�@���@�����n���h2��ډ��~
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4035", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4036 �������t�����n���h2��ډ��~�@���@�ڕ���N�����v����
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4036", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4037 �������t�ڕ���N�����v�J�@���@�ʏ퉺�~
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4036", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4038�@�����n���h�ʏ퉺�~�@���@�t���O�����y�ыz��OFF
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4038", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4039�@�����n���h�ʏ�z��OFF�@���@�����n���h�㏸
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4039", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4040�@����ON����A�������t�����̋����j��
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4040", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4041�@�����n���h�^��j�󎞊�
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4041", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4042�@�����n���h�^��j�󎞊�
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4042", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4045�@�ڕ���N�����vON�@���@�z��ON
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4045", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4046�@�ڕ���z��ON�@���@�����^2�����̈ȏ㌟�o�҂�
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4046", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4049�@���[�n���h���~�@���@�ڕ���N�����v����
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4049", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4050�@�ڕ���N�����v�����@���@�z��OFF
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4050", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4051�@�ڕ���z��OFF�@���@���[�n���h�㏸
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4051", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4052�@�ڕ���N�����v�����@���@���[�n���h�㏸
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4052", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4053�@�����}�K�W���G�A�u���[����
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4053", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' �� 'D4055�@�����n���h����@���@�z���~�X�ُ̈팟�o�҂�
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4055", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' �� 'D4056�@�ڕ���z��ON�@���@�z���~�X�ُ̈팟�o�҂�
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4056", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' �� 'D4057�@���[�n���h����@���@�z���~�X�ُ̈팟�o�҂�
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4057", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1
            '' D4058�@���[�G���x�[�^���~����
            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D4058", 1, iData)
            'LTimerDT(i) = iData(0)
            'i = i + 1

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_GetTimerValue() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_���^�C�}�[�l�����ݏ���"
    '''=========================================================================
    ''' <summary>���[�_���^�C�}�[�l�����ݏ���</summary>
    ''' <param name="LTimerDT">(INP)�^�C�}�[�l</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_SetTimerValue(ByRef LTimerDT() As UShort) As Integer

        'Dim iData() As UShort
        'Dim i As Integer
        Dim strMSG As String

        Try
            If (bFgActLink = False) Then                            ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            '' D4032 �������t�����n���h1��ډ��~�@���@�z��OFF
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4032", 1, iData)
            'i = i + 1
            '' D4033 �������t�����n���h�z��OFF�@���@�ڕ���N�����v
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4033", 1, iData)
            'i = i + 1
            '' D4034 �������t�ڕ���N�����v�@���@�����n���h�㏸
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4034", 1, iData)
            'i = i + 1
            '' �� 'D4035 �������t�����n���h�㏸�@���@�����n���h2��ډ��~
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4035", 1, iData)
            'i = i + 1
            '' D4036 �������t�����n���h2��ډ��~�@���@�ڕ���N�����v����
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4036", 1, iData)
            'i = i + 1
            '' D4037 �������t�ڕ���N�����v�J�@���@�ʏ퉺�~
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4037", 1, iData)
            'i = i + 1
            '' D4038�@�����n���h�ʏ퉺�~�@���@�t���O�����y�ыz��OFF
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4038", 1, iData)
            'i = i + 1
            '' D4039�@�����n���h�ʏ�z��OFF�@���@�����n���h�㏸
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4039", 1, iData)
            'i = i + 1
            '' D4040�@����ON����A�������t�����̋����j��
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4040", 1, iData)
            'i = i + 1
            '' D4041�@�����n���h�^��j�󎞊�        ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4041", 1, iData)
            'i = i + 1
            '' D4042�@�����n���h�^��j�󎞊�
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4042", 1, iData)
            'i = i + 1
            '' D4045�@�ڕ���N�����vON�@���@�z��ON
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4045", 1, iData)
            'i = i + 1
            '' D4046�@�ڕ���z��ON�@���@�����^2�����̈ȏ㌟�o�҂�
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4046", 1, iData)
            'i = i + 1
            '' D4049�@���[�n���h���~�@���@�ڕ���N�����v����
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4049", 1, iData)
            'i = i + 1
            '' D4050�@�ڕ���N�����v�����@���@�z��OFF
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4050", 1, iData)
            'i = i + 1
            '' D4051�@�ڕ���z��OFF�@���@���[�n���h�㏸
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4051", 1, iData)
            'i = i + 1
            '' D4052�@�ڕ���N�����v�����@���@���[�n���h�㏸
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4052", 1, iData)
            'i = i + 1
            '' D4053�@�����}�K�W���G�A�u���[����
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4053", 1, iData)
            'i = i + 1
            '' �� 'D4055�@�����n���h����@���@�z���~�X�ُ̈팟�o�҂�
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4055", 1, iData)
            'i = i + 1
            '' �� 'D4056�@�ڕ���z��ON�@���@�z���~�X�ُ̈팟�o�҂�
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4056", 1, iData)
            'i = i + 1
            '' �� 'D4057�@���[�n���h����@���@�z���~�X�ُ̈팟�o�҂�
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4057", 1, iData)
            'i = i + 1
            '' D4058�@���[�G���x�[�^���~����
            'ReDim iData(0)
            'iData(0) = LTimerDT(i)
            'Call QCPUQ_ActWrite(Form1.ActQCPUQ, Form1.ActSupport, "D4058", 1, iData)
            'i = i + 1

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_SetTimerValue() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_�o�[�W�����擾"
    '''=========================================================================
    ''' <summary>���[�_�o�[�W�����擾</summary>
    ''' <param name="LoaderVer">(OUT)���[�_�o�[�W����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_GetVersion(ByRef LoaderVer As String) As Integer

        'Dim iData() As UShort
        Dim strMSG As String

        Try
            ' ���[�_�o�[�W�����擾
            If (bFgActLink = False) Then                            ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            'ReDim iData(0)
            'Call QCPUQ_ActRead(Form1.ActQCPUQ, Form1.ActSupport, "D5030", 1, iData)
            'LoaderVer = Format(iData(0) / 100, "0.00")
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_GetVersion() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '===============================================================================
    '   ���ʊ֐�
    '===============================================================================
#Region "���[�_����̉����f�[�^�҂�"
    '''=========================================================================
    ''' <summary>���[�_����̉����f�[�^�҂�</summary>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="WaitData">     (INP)�����҂�����f�[�^</param>
    ''' <param name="OnOff">        (INP)True=On�҂�, False=Off�҂�</param>
    ''' <param name="bReset">       (INP)REST�L�[�̗L��/����</param>
    ''' <param name="bOrgFlg">      (INP)���_���A�t���O</param>
    ''' <param name="bFgMagagin">   (OUT)�}�K�W���I���t���O</param>
    ''' <param name="bFgAllMagagin">(OUT)�S�}�K�W���I���t���O</param>
    ''' <param name="bFgLot">       (OUT)���b�g�ؑւ��v���t���O</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
    '''          cFRS_ERR_RST  = RESET�L�[����
    '''          cFRS_ERR_CVR  = ➑̃J�o�[�J���o
    '''          cFRS_ERR_SCVR = �X���C�h�J�o�[�J���o
    '''          cFRS_ERR_EMG  = ����~ ��
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
        Dim bFlgWbrk As Boolean = False                                 ' �^�C�}�[���Z�b�g�t���O 'V4.5.0.0�@
        Dim bFlg2Pce As Boolean = False                                 ' �^�C�}�[���Z�b�g�t���O 'V4.5.0.0�@
        Dim iCnt As Integer 'V1.25.0.2�E


        Try
            ' �����҂�����f�[�^��On/Off�҂� ?
            If (OnOff = True) Then
                WaitBit = WaitData                                      ' On�҂�
            Else
                WaitBit = 0                                             ' Off�҂�
            End If
            BreakFirst = 0 '###130
            TwoTakeFirst = 0    '###130

            ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRS_Tick��X msec�Ԋu�Ŏ��s����)
            Sub_SetTimeoutTimer(TimerRS)

            ' ���[�_����̉����f�[�^��҂�
            Do
                ' ���[�_�A���[��/����~�`�F�b�N
                r = GetLoaderIO(LdIn)                                   ' ���[�_
                If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                    rtnCode = cFRS_ERR_LDR                              ' Return�l = ���[�_�A���[�����o
                    GoTo STP_ERR_LDR                                    ' ���[�_�A���[���\����
                End If

                '----- 'V4.5.0.0�@�� -----
                '---------------------------------------------------------------
                '   ����ꌟ�o�`�F�b�N
                '---------------------------------------------------------------
                If ((LdIn And LINP_WBREAK) = LINP_WBREAK) Then          ' ����ꌟ�o ?
                    ' ����ꌟ�o����
                    If (bFlgWbrk = False) Then                          ' ���o�Ȃ����猟�o����ƂȂ�����^�C�}�[�����Z�b�g���� 
                        bFlgWbrk = True                                 ' �^�C�}�[���Z�b�gFLG ON
                    Else                                                ' ���o����Ō��o����̂܂܂Ȃ�^�C���A�E�g�Ď��𑱂���
                        bFlgWbrk = False                                ' �^�C�}�[���Z�b�gFLG OFF
                    End If
                Else
                    ' ����ꌟ�o�Ȃ�
                    bFlgWbrk = False                                    ' �^�C�}�[���Z�b�gFLG OFF
                End If

                '---------------------------------------------------------------
                '   �Q����茟�o�`�F�b�N�`�F�b�N
                '---------------------------------------------------------------
                If ((LdIn And LINP_2PIECES) = LINP_2PIECES) Then        ' �Q����茟�o ?
                    ' �Q����茟�o����
                    If (bFlg2Pce = False) Then                          ' ���o�Ȃ����猟�o����ƂȂ�����^�C�}�[�����Z�b�g���� 
                        bFlg2Pce = True                                 ' �^�C�}�[���Z�b�gFLG ON
                    Else                                                ' ���o����Ō��o����̂܂܂Ȃ�^�C���A�E�g�Ď��𑱂���
                        bFlg2Pce = False                                ' �^�C�}�[���Z�b�gFLG OFF
                    End If
                Else
                    ' �Q����茟�o�Ȃ�
                    bFlg2Pce = False                                    ' �^�C�}�[���Z�b�gFLG OFF
                End If
                '----- 'V4.5.0.0�@�� -----

                ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N 
                If (bFgTimeOut = True) Then                             ' �^�C���A�E�g ?
                    ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                    TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                    rtnCode = cFRS_ERR_LDRTO                            ' Return�l = ���[�_�ʐM�^�C���A�E�g
                    GoTo STP_ERR_LDR                                    ' �G���[���b�Z�[�W�\����
                End If

                ' RESET�L�[�����`�F�b�N
                If (bReset = True) Then                                 ' RESET�L�[�L�� ? 
                    r = Sub_ResetSWCheck(ObjSys)
                    If (r <> cFRS_NORMAL) Then
                        rtnCode = r                                     ' Return�l�ݒ�(cFRS_ERR_RST/cFRS_ERR_EMG)
                        GoTo STP_END
                    End If
                End If

                ' ➑̃J�o�[��/�X���C�h�J�o�[��/����~�`�F�b�N
                r = Check_CoverOpen(ObjSys, bOrgFlg)
                If (r <> cFRS_NORMAL) Then                              ' ➑̃J�o�[��/����~ ?(�G���[���b�Z�[�W�͕\����) 
                    rtnCode = r                                         ' Return�l�ݒ�
                    GoTo STP_END
                End If
                '###126
                ' �g���~���O�v��ON�҂��̂Ƃ��ɃI�[���}�K�W���I���Ō������M����OFF�����ꍇ�ɂ́A�Ō�̊���ُ�I�������ƔF������B
                If (WaitData = LINP_TRM_START) And (OnOff = True) Then      ' �g���~���O�X�^�[�g�v���҂� ?
                    If (((LdIn And LINP_STOP) = LINP_STOP) And ((LdIn And LINP_END_ALL_MAGAZINE) = LINP_END_ALL_MAGAZINE)) Then
                        rtnCode = cFRS_ERR_LOTEND                       ' Return�l�ݒ�(cFRS_ERR_RST/cFRS_ERR_EMG)
                        bFgAllMagagin = True                            ' V4.0.0.0�R
                        For iCnt = 1 To 3   'V1.25.0.2�E
                            '----- V1.16.0.0�J�� -----
                            ' ���[�_�A���[��/����~�`�F�b�N
                            r = GetLoaderIO(LdIn)                           ' ���[�_����
                            If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                                Console.WriteLine("Sub_WaitLoaderData() ���[�_�A���[��(LdIn=%f)", LdIn)
                                rtnCode = cFRS_ERR_LDR                      ' Return�l = ���[�_�A���[�����o
                                GoTo STP_ERR_LDR                            ' ���[�_�A���[���\����
                            End If
                            '----- V1.16.0.0�J�� -----
                            Call System.Threading.Thread.Sleep(100)     'V1.25.0.2�E
                        Next                'V1.25.0.2�E
                        GoTo STP_END
                    End If

                    '-----'V4.5.0.0�@�� -----
                    '-----------------------------------------------------------
                    '�u�g���~���O�X�^�[�g�v���҂��v�̂Ƃ��Ɂu����ꌟ�o�v����
                    '�u�Q����茟�o�v�Ȃ� ���[�_�ʐM�^�C���A�E�g�^�C�}�[�𐶐�������
                    '-----------------------------------------------------------
                    If ((bFlgWbrk = True) Or (bFlg2Pce = True)) Then    ' �^�C�}�[���Z�b�gFLG ON ?
                        ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                        TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                        TimerRS.Dispose()                               ' �^�C�}�[��j������
                        Sub_SetTimeoutTimer(TimerRS)                    ' �^�C�}�[���� 
                    End If
                    '----- 'V4.5.0.0�@�� -----

                    ''###130 
                    'If (((LdIn And LINP_2PIECES) = LINP_2PIECES)) Then ' B14: �Q����茟�o(0=�Q����薢���o, 1=�Q����茟�o)�@
                    '    If TwoTakeFirst = 0 Then   '###130
                    '        giTwoTakeCounter = giTwoTakeCounter + 1                                    ' �Q������̃J�E���g
                    '        TwoTakeFirst = 1
                    '        DspNGTrayCount()    '###130
                    '    End If
                    'Else
                    '    TwoTakeFirst = 0
                    'End If
                    '###130 
                    'If (((LdIn And LINP_WBREAK) = LINP_WBREAK)) Then  ' B15: ����ꌟ�o(0=����ꖢ���o, 1=����ꌟ�o)�@
                    '    If BreakFirst = 0 Then
                    '        BreakFirst = 1
                    '        giBreakCounter = giBreakCounter + 1                                      ' ���ꌇ����̃J�E���g
                    '        DspNGTrayCount()    '###130
                    '    End If
                    'Else
                    '    BreakFirst = 0
                    'End If
                End If

                '----- ###173�� -----
                ' NG�g���C�ɒu���Ă����画����s��(���_���A���[�h/���[�_���[�h�ؑֈȊO�̎��Ƀ`�F�b�N����
                If (WaitData <> LINP_ORG_BACK) And (WaitData <> LINP_AUTO) Then
                    r = DspNGTrayChk()                                  ' NG�g���C�ɒu���Ă����画����s�� ###148
                    If (r = cFRS_ERR_LDRTO) Then
                        ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                        TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                        rtnCode = cFRS_ERR_LDRTO                        ' Return�l = ���[�_�ʐM�^�C���A�E�g
                        GoTo STP_ERR_LDR                                ' �G���[���b�Z�[�W�\����
                    End If
                End If
                '----- ###173�� -----

                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While ((LdIn And WaitData) <> WaitBit)                 ' �����f�[�^�҂�

            ' �}�K�W���I��, �S�}�K�W���I��, ���b�g�ؑւ��`�F�b�N
            If (WaitData = LINP_TRM_START) And (OnOff = True) Then      ' �g���~���O�X�^�[�g�v���҂� ?
                rtnCode = LoaderBitCheck(bFgMagagin, bFgAllMagagin, bFgLot)
            End If

            ' �I������
STP_END:
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            TimerRS.Dispose()                                           ' �^�C�}�[��j������
            Return (rtnCode)

            ' ���[�_�G���[������
STP_ERR_LDR:
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            TimerRS.Dispose()                                           ' �^�C�}�[��j������
            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ���[�_�ʐM�^�C���A�E�g ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' �G���[���b�Z�[�W�\��
            Else
                ' ���[�_�A���[�����b�Z�[�W�쐬 & ���[�_�A���[����ʕ\��
                rtnCode = Loader_AlarmCheck(ObjSys, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            End If
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_WaitLoaderData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�쐬"
    '''=========================================================================
    ''' <summary>���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�쐬</summary>
    ''' <param name="TimerRS">(OUT)�^�C�}�[</param>
    '''=========================================================================
    Private Sub Sub_SetTimeoutTimer(ByRef TimerRS As System.Threading.Timer)

        Dim TimeVal As Integer
        Dim strMSG As String

        Try
            ' �^�C�}�[�l��ݒ肷��
            If (giOPLDTimeOutFlg = 0) Then                              ' ���[�_�ʐM�^�C���A�E�g���o���� ?
                TimeVal = System.Threading.Timeout.Infinite             ' �^�C�}�[�l = �Ȃ�
            Else
                TimeVal = giOPLDTimeOut                                 ' �^�C�}�[�l = ���[�_�ʐM�^�C���A�E�g����(msec)
            End If

            ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRS_Tick��TimeVal msec�Ԋu�Ŏ��s����)
            bFgTimeOut = False                                          ' ���[�_�ʐM�^�C���A�E�g�t���OOFF
            TimerRS = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerRS_Tick), Nothing, TimeVal, TimeVal)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SetTimeoutTimer() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�^�C�}�[�C�x���g(�w��^�C�}�Ԋu���o�߂������ɔ���)"
    '''=========================================================================
    ''' <summary>�^�C�}�[�C�x���g(�w��^�C�}�Ԋu���o�߂������ɔ���)</summary>
    ''' <param name="Sts">(INP)</param>
    '''=========================================================================
    Private Sub TimerRS_Tick(ByVal Sts As Object)

        Dim strMSG As String
        Dim r As Integer
        Dim lData As Long

        Try
            ' �V���A���ł̒ʐM��Ԃ��擾����
            '   �_�~�[�Ńf�[�^���擾���߂�l�𔻒肷��B
            r = W_Read(LOFS_W110, lData)                               ' ���[�_�A���[����Ԏ擾(W110.08-10)
            If (r <> 0) Then
                'Error
            End If

            bFgTimeOut = True                                           ' ���[�_�ʐM�^�C���A�E�g�t���OON
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.TimerRS_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "RESET�L�[�����`�F�b�N"
    '''=========================================================================
    ''' <summary>RESET�L�[�����`�F�b�N</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Private Function Sub_ResetSWCheck(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Private Function Sub_ResetSWCheck(ByVal ObjSys As Object) As Integer

        Dim sts As Long = 0                                         ' ###160
        Dim r As Long
        Dim strMSG As String

        Try
            ' RESET�L�[�����`�F�b�N
            '----- ###160�� -----
            'Call ZCONRST()                                          ' �R���\�[���L�[���b�`����
            'r = STARTRESET_SWCHECK(False, sts)                      ' RESET SW�����`�F�b�N
            'If (sts = cFRS_ERR_RST) Then                            ' RESET�L�[���� ?
            '    Call LAMP_CTRL(LAMP_HALT, False)                    ' HALT�����vOFF 
            '    Call LAMP_CTRL(LAMP_RESET, True)                    ' RESET�����vON
            '    Call ObjSys.OperationLogging(gSysPrm, MSG_OPLOG_TRIMRES, "") ' ���샍�O�o��("�g���~���O��RESET SW�����ɂ���~")
            '    Return (cFRS_ERR_RST)                               ' ExitFlag = Cancel(RESET��)
            'End If
            '----- ###160�� -----

            If (ObjSys.EmergencySwCheck()) Then                     ' ����~ ?
                r = cGMODE_EMG                                      ' Return�l = ����~���o
                GoTo STP_ERR
            End If
            Return (cFRS_NORMAL)

            ' �G���[���b�Z�[�W�\��
STP_ERR:
            r = Sub_CallFrmRset(ObjSys, r)                          ' �G���[���b�Z�[�W�\��
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_ResetSWCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "➑̃J�o�[��/�X���C�h�J�o�[�`�F�b�N"
    '''=========================================================================
    ''' <summary>➑̃J�o�[��/�X���C�h�J�o�[�`�F�b�N</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_CVR  = ➑̃J�o�[�J���o
    '''          cFRS_ERR_SCVR = �X���C�h�J�o�[�J���o
    '''          cFRS_ERR_EMG  = ����~</returns> 
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
            ' �C���^�[���b�N�����Ȃ�NOP
            If (bOrgFlg = True) Then
                r = ORG_INTERLOCK_CHECK(InterlockSts, sw)           ' �C���^�[���b�N��Ԏ擾
            Else
                r = INTERLOCK_CHECK(InterlockSts, sw)                   ' �C���^�[���b�N��Ԏ擾
            End If
            If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Then     ' �C���^�[���b�N�S���� ?
                Return (cFRS_NORMAL)                                ' Return�l = ����
            End If

            ' �J�o�[��Ԃ̔���
            If (r <> cFRS_NORMAL And r <> ERR_OPN_CVRLTC) Then
                ' �G���[���b�Z�[�W����
                'GoTo STP_ERR
            Else
                Return (cFRS_NORMAL)
            End If

            ' '' '' '' ➑̃J�o�[�`�F�b�N
            '' '' ''r = COVER_CHECK(coverSts)                             ' ����~ or ➑̃J�o�[�J�`�F�b�N
            '' '' ''If (coverSts = 0) Then                              ' ����~/➑̃J�o�[�J���o ?
            '' '' ''    GoTo STP_ERR                                        ' Return�l = ����~/➑̃J�o�[�J���o
            '' '' ''End If
            ' '' '' '' '' '' '' ➑̃J�o�[�`�F�b�N
            '' '' '' '' '' ''r = ObjSys.InterLockCheck()                             ' ����~ or ➑̃J�o�[�J�`�F�b�N
            '' '' '' '' '' ''If (r <> cFRS_NORMAL) Then                              ' ����~/➑̃J�o�[�J���o ?
            '' '' '' '' '' ''    GoTo STP_ERR                                        ' Return�l = ����~/➑̃J�o�[�J���o
            '' '' '' '' '' ''End If

            ' '' '' '' �X���C�h�J�o�[�`�F�b�N
            '' '' ''Call SLIDECOVER_CLOSECHK(sldcvrSts)                     ' �X���C�h�J�o�[�`�F�b�N
            '' '' ''If (sldcvrSts = 0) Then                                 ' �X���C�h�J�o�[�I�[�v�� ?
            '' '' ''    r = cGMODE_SCVRMSG                                  ' Return�l = ➑̃J�o�[�J���o
            '' '' ''    GoTo STP_ERR
            '' '' ''End If

            'Return (cFRS_NORMAL)

            ' �G���[���b�Z�[�W�\��
            'STP_ERR:
            If (r = ERR_EMGSWCH) Then                              ' ����~ ?
                Md = cGMODE_EMG
            ElseIf (r = ERR_OPN_CVR) Then                          ' ➑̃J�o�[�J���o ?
                Md = cGMODE_CVR_OPN
            ElseIf (r = ERR_OPN_SCVR) Then                          ' �X���C�h�J�o�[�J���o
                Md = cGMODE_SCVR_OPN
            Else
                Md = r
            End If
            '' '' ''If (r = cFRS_ERR_EMG) Then                              ' ����~ ?
            '' '' ''    Md = cGMODE_EMG
            '' '' ''ElseIf (r = cFRS_ERR_CVR) Then                          ' ➑̃J�o�[�J���o ?
            '' '' ''    Md = cGMODE_CVR_OPN
            '' '' ''Else                                                    ' �X���C�h�J�o�[�J���o
            '' '' ''    Md = cGMODE_SCVR_OPN
            '' '' ''End If

            r = Sub_CallFrmRset(ObjSys, Md)                         ' �G���[���b�Z�[�W�\��
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Check_CoverOpen() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###184�� -----
#Region "���_���A���̍ڕ���̊�Ȃ����`�F�b�N����"
    '''=========================================================================
    ''' <summary>���_���A���̍ڕ���̊�Ȃ����`�F�b�N����</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="Mode">   (INP)APP_MODE_LOADERINIT = ���[�_���_���A��
    '''                            APP_MODE_AUTO�@�@�@ = �����^�]��
    '''                            APP_MODE_FINEADJ �@ = �ꎞ��~(�T�C�N����~��Cancel�w�莞) V4.0.0.0�R
    ''' </param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function SubstrateCheck(ByVal ObjSys As SystemNET, ByVal Mode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function SubstrateCheck(ByVal ObjSys As Object, ByVal Mode As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""

        Try
            ' �ڕ���Ɋ������ꍇ�A��菜�����܂ő҂�(SL436R��)
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then Return (cFRS_NORMAL)
            '----- V4.0.0.0-27�� -----
            ' SL436S�Ō��_���A���̓`�F�b�N�Ȃ�
            If (giMachineKd = MACHINE_KD_RS) And (Mode = APP_MODE_LOADERINIT) Then
                Return (cFRS_NORMAL)
            End If
            '----- V4.0.0.0-27�� -----
            Do
                System.Threading.Thread.Sleep(300)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' �ڕ���Ɋ���Ȃ������`�F�b�N����
                Call ZCONRST()                                          ' �R���\�[���L�[���b�`����
                r = Sub_SubstrateCheck(ObjSys, Mode)                    ' ��Ȃ��`�F�b�N
                If (r = cFRS_NORMAL) Then Exit Do '                     ' ��Ȃ��Ȃ烋�[�v�𔲂��� 
                If (r < cFRS_NORMAL) Then                               ' �ُ�I�����x���̃G���[ ? 
                    Return (r)                                          ' �Ăяo�����ŃA�v���P�[�V���������I����
                End If
                '                                                       ' ��L(r = cFRS_ERR_RST)�Ȃ��菜�����܂ő҂�
            Loop While (1)

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SubstrateCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###197�� -----
#Region "���_���A����NG�r�oBOX���t���`�F�b�N����"
    '''=========================================================================
    ''' <summary>���_���A����NG�r�oBOX���t���`�F�b�N����</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function NgBoxCheck(ByVal ObjSys As SystemNET, ByVal Mode As Integer) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function NgBoxCheck(ByVal ObjSys As Object, ByVal Mode As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim strMSG As String = ""

        Try
            ' NG�r�oBOX�����t�̏ꍇ�A��菜�����܂ő҂�(SL436R��)
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then Return (cFRS_NORMAL)
            Do
                System.Threading.Thread.Sleep(300)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' NG�r�oBOX�����t�łȂ������`�F�b�N����
                Call ZCONRST()                                          ' �R���\�[���L�[���b�`����
                r = Sub_NgBoxCheck(ObjSys, Mode)                        ' NG�r�oBOX�����t�łȂ����`�F�b�N
                If (r = cFRS_NORMAL) Then Exit Do '                     ' ���t�łȂ��Ȃ烋�[�v�𔲂��� 
                If (r < cFRS_NORMAL) Then                               ' �ُ�I�����x���̃G���[ ? 
                    Return (r)                                          ' �Ăяo�����ŃA�v���P�[�V���������I����
                End If
                '                                                       ' ��L(r = cFRS_ERR_RST)�Ȃ��菜�����܂ő҂�
            Loop While (1)

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.NgBoxCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "NG�r�oBOX�����t�łȂ����`�F�b�N����"
    '''=========================================================================
    ''' <summary>NG�r�oBOX�����t�łȂ����`�F�b�N����</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="Mode">   (INP)APP_MODE_LOADERINIT = ���[�_���_���A��
    '''                            APP_MODE_AUTO�@�@�@ = �����^�]��</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
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
            ' NG�r�oBOX�����t�łȂ����`�F�b�N����
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                r = W_Read(LOFS_W43S, lData)                            ' �������͏�Ԏ擾(W43.00-W43.15)
                lBit = LDSTS_NGFULL
            Else
                ' SL436R��
                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                lBit = LDST_NGFULL
            End If

            ' G�r�oBOX�����t�Ȃ烁�b�Z�[�W�\��
            If (lData And lBit) Then                                    ' NG�r�oBOX���t ? V2.0.0.0�E
                '' �n���h�𓋍ڈʒu�Ɉړ�����(�c���菜������) �����_���A���ĂȂ��ƃn���h�͓����Ȃ�
                'Call W_HAND_STAGE()

                ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
                Call COVERCHK_ONOFF(COVER_CHECK_OFF)

                '----- V1.18.0.5�@��(�����͂����ōs��) -----
                '----- V1.18.0.1�G�� -----
                ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
                r = EL_Lock_OnOff(EX_LOK_MD_OFF)
                If (r = cFRS_TO_EXLOCK) Then                            ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                    rtn = cFRS_ERR_RST
                    GoTo STP_END                                        ' �I��������
                End If
                If (r < cFRS_NORMAL) Then                               ' �ُ�I�����x���̃G���[ ? 
                    Return (r)
                End If
                '----- V1.18.0.1�G�� -----
                '----- V1.18.0.5�@�� -----

                ' ���[�h�ɂ���ĕ\�����b�Z�[�W��ݒ肷��
                If (Mode = APP_MODE_AUTO) Then                          ' �����^�]�� ? 
                    strMSG = MSG_LOADER_21                              ' "�m�f�r�o�{�b�N�X���t"
                    strMS2 = MSG_LOADER_41                              ' "�m�f�����菜���Ă���"
                    strMS3 = MSG_LOADER_33                              ' "�ēx�����^�]�����s���ĉ�����"

                Else                                                    ' ���[�_���_���A��
                    strMSG = MSG_LOADER_21                              ' "�m�f�r�o�{�b�N�X���t"
                    strMS2 = MSG_LOADER_40                              ' "�m�f�����菜���ĉ�����"
                    strMS3 = ""                                         ' ""
                End If

                ' ���b�Z�[�W�\��(START�L�[�����҂�)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                rtn = cFRS_ERR_RST                                      ' Return�l = Cancel(RESET��)
            End If

STP_END:
            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()

            ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���

            '----- V1.18.0.5�@��(�d�����b�N�͎����^�]��(�g���~���O��)�̂ݍs��) -----
            If (Mode = APP_MODE_TRIM) Then                              ' �����^�]��(�g���~���O��) ? 
                '----- V1.18.0.1�G�� -----
                ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
                r = EL_Lock_OnOff(EX_LOK_MD_ON)
                If (r = cFRS_TO_EXLOCK) Then                            ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                    rtn = cFRS_ERR_RST
                End If
                If (r < cFRS_NORMAL) Then                               ' �ُ�I�����x���̃G���[ ? 
                    rtn = r
                End If
                '----- V1.18.0.1�G�� -----
            End If
            '----- V1.18.0.5�@�� -----

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_NgBoxCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###197�� -----
#Region "�ڕ���Ɋ���Ȃ������`�F�b�N����"
    '''=========================================================================
    ''' <summary>�ڕ���Ɋ���Ȃ������`�F�b�N����</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="Mode">   (INP)APP_MODE_LOADERINIT = ���[�_���_���A��
    '''                            APP_MODE_AUTO�@�@�@ = �����^�]��
    '''                            APP_MODE_FINEADJ �@ = �ꎞ��~(�T�C�N����~��Cancel�w�莞) V4.0.0.0�R
    ''' </param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
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

            'V4.12.2.0�G    'V6.0.5.0�B             ��
            ' �N�����v���X���͔O�̂��߃N�����vOFF����
            If (gSysPrm.stIOC.giClamp = 2) Then
                W_CLMP_ONOFF(0)                                         ' �N�����vOFF
            End If
            'V4.12.2.0�G    'V6.0.5.0�B             ��

            ' �ڕ���Ɋ���Ȃ������`�F�b�N����
            '----- V1.16.0.0�K�� -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            End If
            '----- V1.16.0.0�K�� -----
            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ��200ms���ƃ��[�N�L�����o����Ȃ��ꍇ������
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
                lBit = LDSTS_VACUME
            Else
                ' SL436R��
                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                lBit = LDST_VACUME
            End If

            '----- V1.16.0.0�K�� -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            End If
            '----- V1.16.0.0�K�� -----
            Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF)

            ' ���[�N�L�Ȃ烁�b�Z�[�W�\��
            If (lData And lBit) Then                                    ' �ڕ���Ɋ�L ? V2.0.0.0�E
                ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
                Call COVERCHK_ONOFF(COVER_CHECK_OFF)
                '----- V4.0.0.0�R�� -----
                ' �d�����b�N(�ω����E�����b�N)����������(�ꎞ��~(�T�C�N����~��Cancel�w�莞)) ���[���a����(SL436R/SL436S)
                r = EL_Lock_OnOff(EX_LOK_MD_OFF)                        ' �d�����b�N(�ω����E�����b�N)����������
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\����)
                    rtn = r                                         ' Return�l�ݒ� 
                    Return (rtn)
                End If
                '----- V4.0.0.0�R�� -----

                ' ���[�h�ɂ���ĕ\�����b�Z�[�W��ݒ肷��
                If (Mode = APP_MODE_AUTO) Then                          ' �����^�]�� ? 
                    strMSG = MSG_LOADER_32                              ' "�ڕ���̊����菜���Ă���"
                    strMS2 = MSG_LOADER_33                              ' "�ēx�����^�]�����s���ĉ�����"
                    strMS3 = ""                                         ' ""
                    '----- V4.0.0.0�R�� -----
                ElseIf (Mode = APP_MODE_FINEADJ) Then                   ' �ꎞ��~���[�h ?(�T�C�N����~��Cancel�w�莞) 
                    strMSG = MSG_LOADER_36                              ' "�ڕ���̊����菜���ĉ�����"
                    strMS2 = ""                                         ' ""
                    strMS3 = ""                                         ' ""
                    '----- V4.0.0.0�R�� -----
                Else                                                    ' ���[�_���_���A��
                    '----- ###188�� -----
                    ' X��,Y�������_�ɂ��邩�`�F�b�N����
                    bFlg = CheckAxisXYOrg()
                    If (bFlg = False) Then                              ' X��,Y�������_�ɂȂ� ?
                        strMSG = MSG_LOADER_37                          ' "�ڕ����Ɋ���c���Ă��܂�"
                        strMS2 = MSG_LOADER_38                          ' "START�L�[����OK�{�^��������"
                        strMS3 = MSG_LOADER_39                          ' "�X�e�[�W�����_�ɖ߂��܂�"
                    Else
                        strMSG = MSG_LOADER_36                          ' "�ڕ���̊����菜���ĉ�����"
                        strMS2 = ""                                     ' ""
                        strMS3 = ""                                     ' ""
                    End If
                    '----- ###188�� -----
                End If

                ' ���b�Z�[�W�\��(START�L�[�����҂�)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                rtn = cFRS_ERR_RST                                      ' Return�l = Cancel(RESET��)
            End If

            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()

            ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���

            '----- V4.0.0.0�R�� -----
            ' �ꎞ��~���[�h(�T�C�N����~��Cancel�w�莞)�Ȃ�d�����b�N���� ���[���a����(SL436R/SL436S)
            If (Mode = APP_MODE_FINEADJ) Then
                ' �d�����b�N(�ω����E�����b�N)�����b�N����
                r = EL_Lock_OnOff(EX_LOK_MD_ON)                         ' �d�����b�N
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ?(���b�Z�[�W�͕\����)
                    rtn = r                                             ' Return�l�ݒ�
                End If
                Return (rtn)
            End If
            '----- V4.0.0.0�R�� -----
            '----- ###188�� -----
            ' X��,Y�������_�ɂȂ����̓X�e�[�W�����_�ɖ߂�(�c���菜������)
            If (bFlg = False) Then                                          ' X��,Y�������_�ɂȂ� ?
                r = SUbXYOrg(ObjSys)
                If (r < cFRS_NORMAL) Then Return (r) '                      ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                Call W_CLMP_ONOFF(0)                                        ' �N�����v�n�e�e V1.16.0.0�D
            End If
            '----- ###188�� -----

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R�� -----
#Region "�ڕ���Ɋ�����鎖���`�F�b�N����(�T�C�N����~���̑��s�w�莞 SL436R�p)"
    '''=========================================================================
    ''' <summary>�ڕ���Ɋ�����鎖���`�F�b�N����(�T�C�N����~���̑��s�w�莞�p)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>cFRS_NORMAL    = ����(����葱�s)
    '''          cFRS_ERR_START = ����(��Ȃ����s)
    '''          cFRS_ERR_RST   = ��Ȃ���Cancel(RESET�L�[����)
    '''          cFRS_ERR_HALT  = �������o
    '''          ��L�ȊO=�G���[</returns>
    ''' <remarks>���[���a����(SL436R/SL436S)</remarks>
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
            ' �ڕ���Ɋ�����鎖���`�F�b�N����
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call ZABSVACCUME(1)                                     ' (�N�����vOFF�Ŋ���Â��̂��ӂ�������) 
                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            End If
            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ��200ms���ƃ��[�N�L�����o����Ȃ��ꍇ������
            If (giMachineKd = MACHINE_KD_RS) Then
                r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
            Else
                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
            End If

            ' �������ꍇ�̓N�����vOFF���Ȃ�
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                If (lData And LDST_VACUME) Then                         ' �ڕ���Ɋ�L ?
                    '                                                   ' �N�����vOFF���Ȃ�
                Else
                    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                    Call W_CLMP_ONOFF(0)                                ' �N�����vOFF
                End If
            End If

            ' �������ꍇ�͋z��OFF���Ȃ�
            If (lData And LDST_VACUME) Then                             ' �ڕ���Ɋ�L ?

            Else
                Call ZABSVACCUME(0)                                     ' �o�L���[���̐���(�z��OFF)
            End If

            ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)                            ' �d�����b�N(�ω����E�����b�N)����������
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                Return (r)
            End If

            ' ���b�Z�[�W�\��
            If (lData And LDST_VACUME) Then                             ' �ڕ���Ɋ������ꍇ
                rtn = cFRS_NORMAL                                       ' Return�l = ����(����葱�s)

                ' �����`�F�b�N
                r = R_BREAK_XY()
                If (r <> cFRS_NORMAL) Then
                    Call W_CLMP_ONOFF(0)                                ' �N�����vOFF
                    Call ZABSVACCUME(0)                                 ' �o�L���[���̐���(�z��OFF)
                    ' ���b�Z�[�W�\��(START�L�[�����҂�)
                    ' "�������o", "�ڕ����Ɋ���c���Ă���ꍇ��", "��菜���Ă�������"
                    r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                            MSG_LOADER_49, MSG_LOADER_17, MSG_LOADER_18, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (r < cFRS_NORMAL) Then Return (r) '              ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)
                    rtn = cFRS_ERR_HALT                                 ' Return�l = �������o(HALT�L�[�����Ŗ߂�)
                End If

            Else                                                        ' �ڕ���Ɋ���Ȃ��ꍇ

                ' ���b�Z�[�W�\��(START�L�[, RESET�L�[�����҂�)
                ' "�ڕ���Ɋ������܂���", "OK�{�^�������Ŏ����^�]�𑱍s���܂�", "Cancel�{�^�������Ŏ����^�]���I�����܂�"
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                        MSG_LOADER_42, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)
                If (r = cFRS_ERR_RST) Then
                    rtn = cFRS_ERR_RST                                  ' Return�l = ��Ȃ���Cancel(RESET�L�[����)
                Else
                    rtn = cFRS_ERR_START                                ' Return�l = ����(��Ȃ����s)
                End If
            End If

            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()

            ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         '�u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����

            ' �d�����b�N(�ω����E�����b�N)�����b�N����
            r = EL_Lock_OnOff(EX_LOK_MD_ON)                             ' �d�����b�N
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                rtn = r                                                 ' Return�l�ݒ� 
            End If

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateExistCheck2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R�� -----
    '----- ###240�� -----
#Region "�ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h�� SL436R�p)"
    '''=========================================================================
    ''' <summary>�ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h�� SL436R�p)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����
    '''          3=�����(Cancel(RESET��)�@
    '''          ��L�ȊO=�G���[</returns> 
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
            ' �ڕ���Ɋ�Ȃ��Ńg���~���O���s����Ȃ�NOP
            If (giTrimExe_NoWork = 0) Then
                Return (cFRS_NORMAL)
            End If

            'V4.12.2.0�G                 �� 'V6.0.5.0�B
            ' �N�����v���X���͔O�̂��߃N�����vOFF����
            If (gSysPrm.stIOC.giClamp = 2) Then
                W_CLMP_ONOFF(0)                                         ' �N�����vOFF
            End If
            'V4.12.2.0�G                 �� 'V6.0.5.0�B

            ' �ڕ���Ɋ�����鎖���`�F�b�N����
            '----- V1.16.0.0�K�� -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            End If
            '----- V1.16.0.0�K�� -----
            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ��200ms���ƃ��[�N�L�����o����Ȃ��ꍇ������
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
                lBit = LDSTS_VACUME
            Else
                ' SL436R��
                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                lBit = LDST_VACUME
            End If

            ''V5.0.0.9�L
            ' ''----- V1.16.0.0�K�� -----
            ''If (gSysPrm.stIOC.giClamp = 1) Then
            ''    Call W_CLMP_ONOFF(1)                                    ' �N�����vON
            ''    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
            ''    Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            ''End If
            ' ''----- V1.16.0.0�K�� -----
            ''V5.0.0.9�L
            Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF)

            ' ���[�N�����Ȃ烁�b�Z�[�W�\��
            If (lData And lBit) Then                                    ' �ڕ���Ɋ�L ? V2.0.0.0�E

            Else                                                        ' �ڕ���Ɋ�Ȃ�
                ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
                Call COVERCHK_ONOFF(COVER_CHECK_OFF)

                If (giSubExistMsgFlag = True) Then

                    ' �\�����b�Z�[�W��ݒ肷��
                    strMSG = MSG_LOADER_42                                  ' "�ڕ���Ɋ������܂���"
                    strMS2 = MSG_LOADER_43                                  ' "���u���čēx���s���ĉ�����"
                    strMS3 = ""                                             ' ""

                    ' ���b�Z�[�W�\��(START�L�[�����҂�)
                    r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                            strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                End If
                rtn = cFRS_ERR_RST                                      ' Return�l = �������Cancel(RESET��)�ŕԂ�
            End If

            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()

            ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateExistCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h�� SL432R�p)"
    '''=========================================================================
    ''' <summary>�ڕ���Ɋ�����鎖���`�F�b�N����(�蓮���[�h�� SL432R�p)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����
    '''          3=�����(Cancel(RESET��)�@
    '''          ��L�ȊO=�G���[</returns> 
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
            '----- V1.13.0.0�F�� -----
            ' SL432RW���� TRIM_EXEC_NOWORK=1�Ȃ�N�����vON��
            If (giMachineKd = KEY_TYPE_RW) And (giTrimExe_NoWork = 1) Then
                GoTo STP_010
            End If
            '----- V1.13.0.0�F�� -----

            ' �ڕ���Ɋ�Ȃ��Ńg���~���O���s���閔�͋z������Ȃ����͋z���Z���T�`�F�b�N�Ȃ��Ȃ�NOP
            If (giTrimExe_NoWork = 0) Or (gSysPrm.stIOC.giVacume = 0) Or (gSysPrm.stIOC.giRetryVaccume = 0) Then
                Return (cFRS_NORMAL)
            End If

STP_010:
            ' �N�����vON
            r = Form1.System1.ClampCtrl(gSysPrm, 1, giTrimErr)          ' V1.13.0.0�F
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                Return (r)
            End If
            'If (gSysPrm.stIOC.giClamp = 1) Then                         ' �N�����v���䂠�� ?
            '    r = CLAMP_CTRL(1, 1)                                    ' �N�����vON(X,Y) 
            '    r = ObjSys.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            '    If (r <> cFRS_NORMAL) Then                              ' �G���[ ? 
            '        Return (r)
            '    End If
            'End If

            ' �z��ON
            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
            System.Threading.Thread.Sleep(500)                          ' Wait(ms) 

            Call INP16(VACUME_STS, SubSts)                              ' �z���X�e�[�^�X�擾 
            'Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF) ###253

            ' ���[�N�����Ȃ烁�b�Z�[�W�\��
            If (SubSts And VACUME_STS_BIT) Then                         ' �z��ON(�ڕ���Ɋ�L) ?

            Else                                                        ' �ڕ���Ɋ�Ȃ�
                Call ZABSVACCUME(0)                                     ' �o�L���[���̐���(�z��OFF) ###253
                ' �\�����b�Z�[�W��ݒ肷��
                strMSG = MSG_LOADER_42                                  ' "�ڕ���Ɋ������܂���"
                strMS2 = MSG_LOADER_43                                  ' "���u���čēx���s���ĉ�����"
                strMS3 = ""                                             ' ""

                ' ���b�Z�[�W�\��(START�L�[�����҂�)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                rtn = cFRS_ERR_RST                                      ' Return�l = �������Cancel(RESET��)�ŕԂ�
                '----- V1.13.0.0�F�� -----
                ' �N�����vOFF
                r = Form1.System1.ClampCtrl(gSysPrm, 0, giTrimErr)
                If (r <> cFRS_NORMAL) Then                              ' �G���[ ? 
                    Return (r)
                End If
                '----- V1.13.0.0�F�� -----
            End If
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateExistCheck_432R() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###240�� -----
#Region "�}�K�W������o�Z���T��OFF�ł��鎖���`�F�b�N����"
    '''=========================================================================
    ''' <summary>�}�K�W������o�Z���T��OFF�ł��鎖���`�F�b�N����</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
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
            ' �}�K�W������o�Z���T��Ԃ��擾����
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                r = W_Read(LOFS_W43S, lData)                            ' �������͏�Ԏ擾(W43.00-W43.15)

                ' �}�K�W���̊���o�Z���T��OFF�ł��鎖���`�F�b�N����
                If (lData And LDSTS_MG1_SUBTSENS) Then                  ' ����o(�}�K�W��1) ? 
                    iFlg = 1
                End If
                If (lData And LDSTS_MG2_SUBTSENS) Then                  ' ����o(�}�K�W��2) ? 
                    iFlg = iFlg + 2
                End If

            Else
                ' SL436R��
                r = H_Read(LOFS_H00, lSts)                              ' �����ݒ��Ԏ擾(H0.00-H0.15)
                r = W_Read(LOFS_W42, lData)                             ' �������͏�Ԏ擾(W42.00-W42.15)

                ' �}�K�W��1-4�̊���o�Z���T��OFF�ł��鎖���`�F�b�N����
                If (lData And LDST_MG1_SUBTSENS) Then                       ' ����o(�}�K�W��1) ? 
                    iFlg = 1
                End If
                If (lData And LDST_MG2_SUBTSENS) Then                       ' ����o(�}�K�W��2) ? 
                    iFlg = iFlg + 2
                End If
                ' �}�K�W��3,4��4�}�K�W���̎��̂݃`�F�b�N����
                If (lSts And LHST_MAGAZINE) Then                            ' 4�}�K�W�� ? 
                    If (lData And LDST_MG3_SUBTSENS) Then                   ' ����o(�}�K�W��3) ? 
                        iFlg = iFlg + 4
                    End If
                    If (lData And LDST_MG4_SUBTSENS) Then                   ' ����o(�}�K�W��4) ? 
                        iFlg = iFlg + 8
                    End If
                End If
            End If

            ' ����o�Z���TON�Ȃ烁�b�Z�[�W�\��
            If (iFlg <> 0) Then
                ' "�}�K�W��������o�Z���T�n�e�e�ʒu�܂�","�����Ă���", "�ēx�����^�]�����s���ĉ�����"
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_LOADER_34, MSG_LOADER_35, MSG_LOADER_33, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                rtn = cFRS_ERR_RST                                      ' Return�l = Cancel(RESET��)
            End If

            Return (rtn)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_SubstrateSensorOffCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###184�� -----
    '----- V1.18.0.0�H�� -----
#Region "�}�K�W���̗L�����`�F�b�N����(���[���a����)"
    '''=========================================================================
    ''' <summary>�}�K�W���̗L�����`�F�b�N����(���[���a����)</summary>
    ''' <param name="Exist">(INP)0=�����`�F�b�N, 1=�L��`�F�b�N</param>
    ''' <returns>cFRS_NORMAL  = ����
    '''          cFRS_ERR_RST = �G���[
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
            '   �}�K�W���L��̏ꍇ
            '---------------------------------------------------------------
            ' �������͏�Ԃ��擾����
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Return (cFRS_NORMAL)
            Else
                ' SL436R��
                r = W_Read(LOFS_W42, lData)                             ' �������͏�Ԏ擾(W42.00-W42.15)
            End If

            ' �}�K�W��2�̗L�����`�F�b�N����
            If (lData And LDST_MG2_EXIST) Then                          ' �}�K�W��2�L ?
                If (Exist = 0) Then                                     ' �}�K�W��2�����`�F�b�N�Ȃ�
                    Return (cFRS_ERR_RST)                               ' Return�l = �G���[             
                End If

                '---------------------------------------------------------------
                '   �}�K�W�������̏ꍇ
                '---------------------------------------------------------------
            Else                                                        ' �}�K�W��2�����̏ꍇ ?
                If (Exist = 0) Then                                     ' �}�K�W��2�����`�F�b�N�Ȃ�
                    Return (cFRS_NORMAL)                                ' Return�l = ����  
                Else                                                    ' �}�K�W��2�L�`�F�b�N�Ȃ�
                    Return (cFRS_ERR_RST)                               ' Return�l = �G���[  
                End If
            End If

            '---------------------------------------------------------------
            '   �}�K�W��2�L��ŁA�}�K�W���L��`�F�b�N�Ȃ�
            '   �}�K�W��2���\���`�F�b�N����
            '---------------------------------------------------------------
            If (lData And LDST_MG2_REVERS) Then                         ' �}�K�W��2�� ?
                Return (cFRS_ERR_RST)                                   ' Return�l = �G���[  
            End If

            ' �}�K�W��1�̗L�����`�F�b�N����
            If (lData And LDST_MG1_EXIST) Then                          ' �}�K�W��1�L ?
                Return (cFRS_NORMAL)
            End If

            Return (cFRS_ERR_RST)                                       ' Return�l = �G���[  

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_MagazineExitCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.18.0.0�H�� -----
    '----- V1.23.0.0�D�� -----
#Region "�}�K�W�����������Ă��鎖���`�F�b�N����"
    '''=========================================================================
    ''' <summary>�}�K�W�����������Ă��鎖���`�F�b�N����</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Sub_MagazineDownCheck(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_MagazineDownCheck(ByVal ObjSys As Object) As Integer

        Dim TimerRS As System.Threading.Timer = Nothing
        Dim rtnCode As Integer = cFRS_NORMAL
        Dim r As Integer
        Dim lData As Long = 0
        Dim strMSG As String

        Try
            ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRS_Tick��X msec�Ԋu�Ŏ��s����)
            Sub_SetTimeoutTimer(TimerRS)

            ' �}�K�W��1�̉������҂�
            Do
                r = W_Read(LOFS_W42, lData)                             ' �������͏�Ԏ擾(W42.00-W42.15)
                If ((lData And LDST_MG1_DOWN) = LDST_MG1_DOWN) Then
                    rtnCode = cFRS_NORMAL                               ' Return�l = ����
                    GoTo STP_END
                End If

                ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N 
                If (bFgTimeOut = True) Then                             ' �^�C���A�E�g ?
                    rtnCode = cFRS_ERR_LDRTO                            ' Return�l = ���[�_�ʐM�^�C���A�E�g
                    GoTo STP_END
                End If

                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(300)                 ' Wait(msec)
            Loop While (1)

            ' �I������
STP_END:
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            If (IsNothing(TimerRS) = False) Then
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' �^�C�}�[��j������
            End If

            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' �^�C���A�E�g ?
                r = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)           ' �G���[���b�Z�[�W�\��
            End If

            Return (rtnCode)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(Sub_MagazineDownCheck)"
            MsgBox(strMSG)
            rtnCode = cFRS_NORMAL
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V1.23.0.0�D�� -----
    '----- ###188�� -----
#Region "�X�e�[�W�����_�ɖ߂�(�c���菜������)"
    '''=========================================================================
    ''' <summary>�X�e�[�W�����_�ɖ߂�(�c���菜������)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function SUbXYOrg(ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function SUbXYOrg(ByVal ObjSys As Object) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim sts As Integer = 0
        Dim strMSG As String

        Try
            ' �X�e�[�W�����_�ɖ߂�(�c���菜������)
            r = Sub_CallFrmRset(ObjSys, cGMODE_LDR_STAGE_ORG)
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SUbXYOrg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "X��,Y�������_�ɂ��邩�`�F�b�N����"
    '''=========================================================================
    ''' <summary>X��,Y�������_�ɂ��邩�`�F�b�N����</summary>
    ''' <returns>True= X,Y�������_, False=���_�łȂ�</returns>
    '''=========================================================================
    Public Function CheckAxisXYOrg() As Boolean

        Dim sts As Integer = 0
        Dim strMSG As String

        Try
            ' X,Y�e�[�u���Ȃ��Ȃ�NOP
            If (gSysPrm.stDEV.giXYtbl <> 3) Then Return (True)

            ' SL436S�Ȃ�NOP V2.0.0.0�M
            If (giMachineKd = MACHINE_KD_RS) Then Return (True)

            ' X��,Y�������_�ɂ��邩�`�F�b�N����
            GetAxisSubStatus(AXIS_X, sts)                               ' X���T�u�X�e�[�^�X�擾 
            If (sts And SUBSTS_ORG) Then                                ' X���͌��_ ? 
                GetAxisSubStatus(AXIS_Y, sts)                           ' Y���T�u�X�e�[�^�X�擾 
                If (sts And SUBSTS_ORG) Then                            ' Y���͌��_ ? 
                    Return (True)                                       ' Return�l = X,Y�������_ 
                End If
            End If

            Return (False)                                              ' Return�l = ���_�łȂ� 

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.CheckAxisXYOrg() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "���T�u�X�e�[�^�X��Ԃ�"
    '''=========================================================================
    ''' <summary>���T�u�X�e�[�^�X��Ԃ�</summary>
    ''' <param name="Axis">  (INP)�����</param>
    ''' <param name="SubSts">(OUT)���T�u�X�e�[�^�X</param>
    '''=========================================================================
    Public Sub GetAxisSubStatus(ByVal Axis As Integer, ByRef SubSts As Integer)

        Dim strMSG As String

        Try
            SubSts = 0
            Call INP16(AxisSubStsAry(Axis), SubSts)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.GetAxisSubStatus() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###188�� -----
    '----- V1.16.0.0�O�� -----
#Region "�z���m�F�X�e�[�^�X��Ԃ�"
    '''=========================================================================
    ''' <summary>�z���m�F�X�e�[�^�X��Ԃ�</summary>
    ''' <param name="Sts">(OUT)�X�e�[�^�X(1:�z���m�F, 0:�z�����m�F)</param>
    '''=========================================================================
    Public Sub GetVacumeStatus(ByRef Sts As Integer)

        Dim lData As Long = 0
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                ' �������͏�Ԏ擾(W42.00-W42.15)
                r = W_Read(LOFS_W42S, lData)
                If (lData And LDSTS_VACUME) Then                        ' �z���m�F(�ڕ���Ɋ�L) ?
                    Sts = 1
                Else
                    Sts = 0
                End If

            Else
                ' SL436R��
                ' �������͏�Ԏ擾(W44.00-W44.15)
                r = W_Read(LOFS_W44, lData)
                If (lData And LDST_VACUME) Then                         ' �z���m�F(�ڕ���Ɋ�L) ?
                    Sts = 1
                Else
                    Sts = 0
                End If
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.GetVacumeStatus() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.16.0.0�O�� -----
#Region "FrmReset���s�T�u���[�`��"
    '''=========================================================================
    ''' <summary>FrmReset���s�T�u���[�`��</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="gMode"> (INP)�������[�h</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Sub_CallFrmRset(ByVal ObjSys As SystemNET, ByVal gMode As Integer) As Integer   'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function Sub_CallFrmRset(ByVal ObjSys As Object, ByVal gMode As Integer) As Integer

        Dim r As Integer
        'V6.0.0.0�Q        Dim objForm As Object = Nothing
        Dim objForm As FrmReset = Nothing           'V6.0.0.0�Q
        Dim strMSG As String

        Try


            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            'If gMode = cGMODE_LDR_TMOUT Then
            '    ' �ꎞ��~�J�n���Ԃ�ݒ肷��(�I�v�V����)
            '    m_blnElapsedTime = True                             ' �o�ߎ��Ԃ�\������
            '    Call Set_PauseStartTime(StPauseTime)
            'End If
            '----- V4.11.0.0�C�� -----

            ' FrmReset��ʕ\��(�������[�h�ɑΉ����鏈�����s��)
            objForm = New FrmReset()
            Call objForm.ShowDialog(Nothing, gMode, ObjSys)
            r = objForm.sGetReturn()                                    ' Return�l�擾

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            'If gMode = cGMODE_LDR_TMOUT Then
            '    ' �ꎞ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����(�I�v�V����)
            '    Call Set_PauseTotalTime(StPauseTime)
            '    ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��(�I�v�V����)
            '    TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            'End If
            '----- V4.11.0.0�C�� -----

            ' �I�u�W�F�N�g�J��
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                    ' �I�u�W�F�N�g�J��
                Call objForm.Dispose()                                  ' ���\�[�X�J��
            End If

             'V6.0.5.0�I��
            ' ���_���A���̓N�����v���J��Ԃɂ��� ###001
            'V4.12.2.4�A            If (gMode = cGMODE_ORG) Or (gMode = cGMODE_LDR_ORG) Then
            Select Case (gMode)
                Case cGMODE_ORG, cGMODE_LDR_ORG
                    If (r = cFRS_NORMAL) Then                               ' ###163
                        '----- V1.16.0.0�D�� -----
                        Call W_CLMP_ONOFF(0)                                ' �N�����v�n�e�e(�J)  
                        'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                        'Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_CLMP_X + LDAB_CLMP_Y)
                        '----- V1.16.0.0�D�� -----
                    End If
                Case cGMODE_LDR_END                                         'V4.12.2.4�A �����^�]�I�����ǉ�
                    If (r = cFRS_ERR_START) Then
                        Call W_CLMP_ONOFF(0)                                ' �N�����v�n�e�e(�J)  
                    End If
            End Select
            'V4.12.2.4�A
             'V6.0.5.0�I��

            Return (r)                                                  ' Return(�G���[���̃��b�Z�[�W�͕\����) 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_CallFrmRset() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "FrmReset���g�p���Ďw��̃��b�Z�[�W��\������"
    '''=========================================================================
    ''' <summary>FrmReset���g�p���Ďw��̃��b�Z�[�W��\������ ###089</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="gMode"> (INP)�������[�h</param>
    ''' <param name="Md">    (INP)cFRS_ERR_START                = START�L�[�����҂�
    '''                           cFRS_ERR_RST                  = RESET�L�[�����҂�
    '''                           cFRS_ERR_START + cFRS_ERR_RST = START/RESET�L�[�����҂�</param>
    ''' <param name="BtnDsp">(INP)�{�^���\������/���Ȃ�</param>
    ''' <param name="Msg1">  (INP)�\�����b�Z�[�W�P</param>
    ''' <param name="Msg2">  (INP)�\�����b�Z�[�W�Q</param>
    ''' <param name="MSG3">  (INP)�\�����b�Z�[�W�R</param>
    ''' <param name="Col1">  (INP)���b�Z�[�W�F�P</param>
    ''' <param name="Col2">  (INP)���b�Z�[�W�F�Q</param>
    ''' <param name="Col3">  (INP)���b�Z�[�W�F�R</param>
    ''' <returns>cFRS_ERR_START = OK�{�^��(START�L�[)����
    '''          cFRS_ERR_RST   = Cancel�{�^��(RESET�L�[)����
    '''          ��L�ȊO       = �G���[</returns> 
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
            ' �p�����[�^�ݒ�
            MsgAry(0) = Msg1
            MsgAry(1) = Msg2
            MsgAry(2) = Msg3
            ColAry(0) = Col1
            ColAry(1) = Col2
            ColAry(2) = Col3

            ' FrmReset��ʕ\��(�w��̃��b�Z�[�W��\������)
            'V6.0.0.0�J            objForm = New FrmReset()
            Dim objForm As New FrmReset()   'V6.0.0.0�J
            Call objForm.ShowDialog(Nothing, gMode, ObjSys, MsgAry, ColAry, Md, BtnDsp)
            r = objForm.sGetReturn()                                    ' Return�l�擾

            ' �I�u�W�F�N�g�J��
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                    ' �I�u�W�F�N�g�J��
                Call objForm.Dispose()                                  ' ���\�[�X�J��
            End If

            Return (r)                                                  ' Return(�G���[���̃��b�Z�[�W�͕\����) 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_CallFrmMsgDisp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���b�Z�[�W�\������"
    '''=========================================================================
    ''' <summary>���b�Z�[�W�\������</summary>
    ''' <param name="ErrCode">(INP)�G���[�R�[�h</param>
    ''' <param name="ObjSys"> (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Private Function Sub_MsgDisp(ByVal ErrCode As Short, ByVal ObjSys As SystemNET) As Integer 'V6.0.0.0�Q
        'V6.0.0.0�Q    Private Function Sub_MsgDisp(ByVal ErrCode As Short, ByVal ObjSys As Object) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ����܂��̓^�C���A�E�g���̕\���ς̃G���[�R�[�h�Ȃ�NOP
            If ((ErrCode >= cFRS_NORMAL) Or _
               ((ErrCode <= cFRS_TO_SCVR_CL) And (ErrCode >= cFRS_TO_PM_BK))) Then
                Return (ErrCode)
            End If

            ' �G���[���b�Z�[�W�\��
            r = ObjSys.Form_AxisErrMsgDisp(System.Math.Abs(ErrCode))
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_MsgDisp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "NG�r�o�g���C�ɏ悹�������J�E���g���A�ݒ�l�𒴂��Ă���Ή�ʕ\������"
    '''=========================================================================    ###130 
    ''' <summary>NG�r�o�g���C�ɏ悹�������J�E���g���A�ݒ�l�𒴂��Ă���Ή�ʕ\������</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function JudgeNGtrayCount(ByVal Idx As Integer) As Integer

        Dim r As Integer = cFRS_NORMAL ' ###199
        Dim strMSG As String = ""       ' V6.1.1.0�L

        '----- ###199�� -----
        '�uNG�r�oBOX���t�v�̓Z���T�[�ōs�����߁ANG�r�oBOX�ւ̔r�o���̃`�F�b�N�͎~�߂�
        '----- V6.0.3.0_48�� -----
        ' �~�߂�̂��~�߂�  
        'Return (cFRS_NORMAL)
        '----- V6.0.3.0_48�� -----
        '----- ###199�� -----
        ' V6.0.4.0�B��
        ' NG�{�b�N�X�r�o�����̐ݒ肪�O�̏ꍇ�ɂ�NG�{�b�N�X�ւ̔r�o�����̃`�F�b�N�͍s��Ȃ�
        If giNgBoxCount(Idx) = 0 Then
            giNgBoxCounter = 0
            giBreakCounter = 0
            giTwoTakeCounter = 0
            Return (cFRS_NORMAL)
        End If
        ' V6.0.4.0�B��

        '-------------------------------------------------------------------
        '   NG�r�oBOX���t�Ȃ烁�b�Z�[�W��\������ ###089
        '-------------------------------------------------------------------
        ' NG�r�oBOX���t ?
        'If (giNgBoxCounter > giNgBoxCount(Idx)) Then                ' NG�r�oBOX�̎��[�����J�E���^�[ > NG�r�oBOX�̎��[���� ?
        If ((giNgBoxCounter + giBreakCounter + giTwoTakeCounter) >= giNgBoxCount(Idx)) Then ' NG�r�oBOX�̎��[�����J�E���^�[ > NG�r�oBOX�̎��[���� ? ###181 2013.01.24
            giAppMode = APP_MODE_LDR_ALRM                           ' �A�v�����[�h = ���[�_�A���[�����(�J�o�[�J�̃G���[�Ƃ��Ȃ���)
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                    ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ��� ###088
            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                Return (r)
            End If
            '----- V1.18.0.1�G�� -----
            '----- V6.1.1.0�L�� -----
            If (giAlmTimeDsp = 0) Then                                  ' ���[�_�A���[�����̎��ԕ\���̗L��(0=�\���Ȃ�, 1=�\������)�@
                strMSG = MSG_LOADER_21                                  '"�m�f�r�o�{�b�N�X���t"
            Else
                Call Get_NowYYMMDDHHMMSS(strMSG)
                strMSG = MSG_LOADER_21 + " " + strMSG
            End If
            '----- V6.1.1.0�L�� -----
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
            ''"�m�f�r�o�{�b�N�X���t","�m�f�r�o�{�b�N�X����m�f�����菜���Ă���","START�L�[���������AOK�{�^���������ĉ������B"V6.1.1.0�I
            ' "�m�f�r�o�{�b�N�X���t","�m�f�r�o�{�b�N�X����m�f�����菜����","START�L�[���������AOK�{�^���������ĉ������B"    V6.1.1.0�I
            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                    strMSG, MSG_LOADER_20, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
            If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 

            '----- ###189 -----
            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()
            If (r < cFRS_NORMAL) Then Return (r) '                  ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
            '----- '###189 -----
            giAppMode = APP_MODE_TRIM                               ' �A�v�����[�h = �g���~���O��
            Call COVERLATCH_CLEAR()                                 ' �J�o�[�J���b�`�̃N���A ###088
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                     ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ��� ###088

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����(���[���a����)
            r = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (r = cFRS_TO_EXLOCK) Then                            ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                               ' �ُ�I�����x���̃G���[ ? 
                Return (r)
            End If
            '----- V1.18.0.1�G�� -----

            Call ClearNGTrayCount()                                 ' NG�r�oBOX�̎��[�����J�E���^�[������������
        End If
    End Function

#End Region

#Region "NG�r�o�g���C�ɏ悹�������N���A����"
    '''=========================================================================    ###130 
    ''' <summary>NG�r�o�g���C�ɏ悹�������N���A����</summary>
    '''=========================================================================
    Public Sub ClearNGTrayCount()
        giNgBoxCounter = 0                                      ' �uNG�r�oBOX�̎��[�����J�E���^�[�v������������
        giBreakCounter = 0                                      ' ���ꌇ����̃J�E���g
        giTwoTakeCounter = 0                                    ' �Q������̃J�E���g
        DspNGTrayCount()
    End Sub
#End Region

#Region "NG�r�o�g���C�ɍڂ������̉�ʏ�̕\�����X�V����"
    '''=========================================================================    ###149 
    ''' <summary>NG�r�o�g���C�ɍڂ������̉�ʏ�̕\�����X�V����</summary>
    '''=========================================================================
    Public Sub DspNGTrayCount()
        'Form1.lblNgCount.Text = "NG Count = " + Str(giNgBoxCounter)
        'Form1.lblBreakCount.Text = "Break = " + Str(giBreakCounter)
        'Form1.lblTwoCount.Text = "Two = " + Str(giTwoTakeCounter)
        Form1.lblNgCount.Text = "NG Count = " + Str(giNgBoxCounter + giBreakCounter + giTwoTakeCounter)
    End Sub
#End Region

#Region "NG�g���C�ɒu���Ă����画����s��"
    '''=========================================================================    ' ###148 
    ''' <summary>NG�g���C�ɒu���Ă����画����s��</summary>
    '''=========================================================================
    Public Function DspNGTrayChk() As Integer

        Dim LdIn As UShort
        Dim r As Integer
        Dim Idx As Integer
        '----- ###173�� -----
        Dim strMSG As String
        Dim rtnCode As Integer = cFRS_NORMAL
        '----- ###173�� -----
        Dim TimerRS As System.Threading.Timer = Nothing     'V5.0.0.1�D

        Try
            '----- ###173�� -----
            If (bFgAutoMode = False) Then                                   ' �����^�]�łȂ����NOP ?
                Return (cFRS_NORMAL)
            End If
            '----- ###173�� -----

            ' NG�g���C�ւ̔r�o�����M����҂�
            r = GetLoaderIO(LdIn)
            If ((LdIn And LINP_NGTRAY_OUT_COMP) = LINP_NGTRAY_OUT_COMP) Then
                If (((LdIn And LINP_WBREAK) = LINP_WBREAK)) Then            ' B15: ����ꌟ�o(0=����ꖢ���o, 1=����ꌟ�o)�@
                    giBreakCounter = giBreakCounter + 1                     ' ���ꌇ����̃J�E���g
                ElseIf (((LdIn And LINP_2PIECES) = LINP_2PIECES)) Then      ' B14: �Q����茟�o(0=�Q����薢���o, 1=�Q����茟�o)�@
                    giTwoTakeCounter = giTwoTakeCounter + 1                 ' �Q������̃J�E���g
                Else
                    giNgBoxCounter = giNgBoxCounter + 1                     ' NG�r�oBOX�̎��[�����J�E���^�[��+ 1����
                    '----- V1.18.0.0�B�� -----
                    ' �s�Ǌ���X�V(���[���a����)
                    If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' �s�Ǌ�� = NG�{�b�N�X�ɔr�o�����������J�E���g���� 
                        stPRT_ROHM.Tol_NG_Sheet = stPRT_ROHM.Tol_NG_Sheet + 1
                    End If
                    '----- V1.18.0.0�B�� -----
                End If
                DspNGTrayCount()
                Idx = typPlateInfo.intWorkSetByLoader - 1                   ' Idx = ��i��ԍ� - 1
                If (Idx < 0) Then Return (rtnCode) ' ###173
                r = JudgeNGtrayCount(Idx)
                '----- ###200 -----
                ' NG�r�o�����t���̓A���[�����Z�b�g�M�����o���Ȃ�
                If ((LdIn And LINP_NG_FULL) = 0) Then                       ' B12: NG�r�o���t(0=NG�r�o�����t, 1=NG�r�o���t(����))
                    Call W_RESET()                                          ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                          ' �X�^�[�g�M�����o 
                End If
                'Call W_RESET()                                              ' �A���[�����Z�b�g�M�����o 
                'Call W_START()                                              ' �X�^�[�g�M�����o 
                '----- ###200 -----
                SetLoaderIO(LOUT_PROC_CONTINUE, 0)                          ' ����p���M��ON(0:�Ȃ�, 1:�p�����s)

                'V5.0.0.1�D��
                ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRS_Tick��X msec�Ԋu�Ŏ��s����)
                Sub_SetTimeoutTimer(TimerRS)
                'V5.0.0.1�D��

                ' NG�g���C�ւ̔r�o�����M����OFF��҂�
                Do
                    r = GetLoaderIO(LdIn)
                    If (((LdIn And LINP_NGTRAY_OUT_COMP) = 0)) Then         ' NG�g���C�ւ̔r�o�����M��OFF ?�@
                        SetLoaderIO(0, LOUT_PROC_CONTINUE)                  ' ����p���M��OFF(0:�Ȃ�, 1:�p�����s)
                        Exit Do
                    End If
                    '----- ###173�� -----
                    If (bFgTimeOut = True) Then                             ' �^�C���A�E�g ?
                        Return (cFRS_ERR_LDRTO)                             ' Return�l = ���[�_�ʐM�^�C���A�E�g
                    End If
                    System.Windows.Forms.Application.DoEvents()
                    Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
                    '----- ###173�� -----
                Loop
            End If

            '----- ###173�� -----
STP_EXIT:

            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            If (IsNothing(TimerRS) = False) Then
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' �^�C�}�[��j������
            End If

            Return (rtnCode)
            '----- ###173�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.DspNGTrayChk() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.18.0.1�G�� -----
#Region "�d�����b�N(�ω����E�����b�N)�����b�N�܂��͉�������(���[���a����(SL436R/SL436S))"
    '''=========================================================================
    ''' <summary>�d�����b�N(�ω����E�����b�N)�����b�N�܂��͉�������</summary>
    ''' <param name="Md">(INP)���[�h(0(EX_LOK_MD_OFF)=���b�N����,
    '''                               1(EX_LOK_MD_ON) =���b�N)</param>
    ''' <returns>cFRS_NORMAL     = ����
    '''          cFRS_TO_EXLOCK  = �O�ʔ����b�N�^�C���A�E�g
    '''          ��L�ȊO      = ���̑��̃G���[
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
            '   ��������
            '-------------------------------------------------------------------
            ' �d�����b�N�@�\�Ȃ��Ȃ�NOP
            If (giDoorLock = 0) Then Return (cFRS_NORMAL)

            ' �C���^�[���b�N�����Ȃ�NOP
            r = INTERLOCK_CHECK(InterlockSts, sw)                       ' �C���^�[���b�N��Ԏ擾
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' �C���^�[���b�N���łȂ� ?
                Return (cFRS_NORMAL)                                    ' Return�l = ����
            End If

            ' �^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerLock_Tick��X msec�Ԋu�Ŏ��s����)
            TimerTM_Create(TimerLock, EX_LOK_TOUT)

STP_RETRY:
            '-------------------------------------------------------------------
            '   �d�����b�N(�ω����E�����b�N)�����b�N�܂��͉�������
            '-------------------------------------------------------------------
            If (Md = EX_LOK_MD_ON) Then                                 ' ���b�N���[�h ? 
                Call EXTOUT1(EXTOUT_EX_LOK_ON, 0)                       ' �d�����b�N(�ω����E�����b�N)�����b�N����
                strTOUT = MSG_SPRASH43                                  '  "�O�ʔ����b�N�^�C���A�E�g"
            Else
                Call EXTOUT1(0, EXTOUT_EX_LOK_ON)                       ' �d�����b�N(�ω����E�����b�N)����������
                strTOUT = MSG_SPRASH44                                  '  "�O�ʔ����b�N�����^�C���A�E�g"
            End If

            '-------------------------------------------------------------------
            '   �d�����b�N�����b�N�܂��͉������ꂽ���`�F�b�N����
            '-------------------------------------------------------------------
            Do
                System.Threading.Thread.Sleep(100)                      ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' �d�����b�N�X�e�[�^�X�擾 
                Call INP16(EX_LOK_STS, Sts)

                ' �d�����b�N(�ω����E�����b�N)�����b�N�܂��͉������ꂽ���`�F�b�N����
                If (Md = EX_LOK_MD_ON) Then                             ' ���b�N���[�h ? 
                    If ((Sts And EXTINP_EX_LOK_ON) = EXTINP_EX_LOK_ON) Then
                        Exit Do                                         ' �d�����b�N�Ȃ�Exit
                    End If
                Else
                    If ((Sts And EXTINP_EX_LOK_ON) = 0) Then
                        Exit Do                                         ' �d�����b�N�����Ȃ�Exit
                    End If
                End If

                '-------------------------------------------------------------------
                '   �^�C���A�E�g�`�F�b�N 
                '-------------------------------------------------------------------
                bTOut = TimerTM_Sts()
                If (bTOut = True) Then                                  ' �^�C���A�E�g ? 
                    ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                    TimerTM_Stop(TimerLock)

                    ' �����v����
                    Call LAMP_CTRL(LAMP_START, True)                    ' START�����vON
                    Call LAMP_CTRL(LAMP_RESET, True)                    ' RESET�����vON
                    Call ZCONRST()                                      ' �R���\�[���L�[���b�`����

                    '  "�O�ʔ����b�N(or ����)�^�C���A�E�g" "START�L�[�F�������s�CRESET�L�[�F�����I��" 
                    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                            strTOUT, MSG_SPRASH35, "", System.Drawing.Color.Blue, System.Drawing.Color.Black, System.Drawing.Color.Black)
                    ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�)  
                    If (r < cFRS_NORMAL) Then Exit Do
                    If (r = cFRS_ERR_RST) Then
                        Call ZCONRST()                                  ' �R���\�[���L�[���b�`����
                        r = cFRS_TO_EXLOCK                              ' Return�l =  �d�����b�N�^�C���A�E�g
                        Exit Do
                    End If

                    ' �����v����
                    Call LAMP_CTRL(LAMP_START, False)                   ' START�����vOFF
                    Call LAMP_CTRL(LAMP_RESET, False)                   ' RESET�����vOFF
                    Call ZCONRST()                                      ' �R���\�[���L�[���b�`����

                    ' �^�C�}�[�J�n
                    Call TimerTM_Start(TimerLock, EX_LOK_TOUT)
                    GoTo STP_RETRY                                      ' ���g���C�� 

                End If

            Loop While (1)

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
            ' �^�C�}�[��j������
            TimerTM_Dispose(TimerLock)

            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.EL_Lock_OnOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.18.0.1�G�� -----
    '----- V4.0.0.0�R�� -----
#Region "�T�C�N����~����(���[���a����(SL436R/SL436S))"
    '''=========================================================================
    ''' <summary>�T�C�N����~����</summary>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>cFRS_NORMAL     = ����
    '''          cFRS_ERR_RST    = Cancel(RESET�L�[����)
    '''          cFRS_TO_EXLOCK  = �O�ʔ����b�N�^�C���A�E�g
    '''          ��L�ȊO        = ���̑��̃G���[
    ''' </returns> 
    '''=========================================================================
    Public Function CycleStop_Proc(ByVal ObjSys As SystemNET) As Integer        'V6.0.0.0�Q
        'V6.0.0.0�Q    Public Function CycleStop_Proc(ByVal ObjSys As Object) As Integer

        Dim RtnCode As Integer = cFRS_NORMAL
        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �T�C�N����~�v���M���𑗏o���ă��[�_����̃T�C�N����~������҂�
            ''V5.0.0.4�A 
            Dim gObjMSG As FrmWait = Nothing 'V6.0.0.0�Q
            'V6.0.0.0�Q            Dim gObjMSG As Object = Nothing
            If IsNothing(gObjMSG) = True Then
                gObjMSG = New FrmWait()
                Call gObjMSG.Show(Form1)
            End If
            ''V5.0.0.4�A 
            r = W_CycleStop(ObjSys, 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                ''V5.0.0.4�A 
                If IsNothing(gObjMSG) <> True Then
                    gObjMSG.MsgClose()
                    gObjMSG = Nothing
                End If
                ''V5.0.0.4�A 
                RtnCode = r                                             ' Return�l�ݒ� 
                GoTo STP_END                                            ' �����I���� 
            End If

            ' �V�O�i���^���[����(On=�Ȃ�, Off=�����^�]��(�Γ_��))
            'V5.0.0.9�M ���@V6.0.3.0�G
            ' Call Form1.System1.SetSignalTower(0, SIGOUT_GRN_ON)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_IDLE)
            'V5.0.0.9�M ���@V6.0.3.0�G

            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALT�����vOFF(SL436S�p)
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            Call W_CLMP_ONOFF(0)                                        ' �N�����vOFF 'V5.0.0.4�@ �P��ڂs�j�x���h�^�n�łn�m����ƂQ��ڂo�k�b�łn�e�e�o���Ȃ��̂łn�e�e���Ă����B
            '-------------------------------------------------------------------
            '   �d�����b�N(�ω����E�����b�N)����������
            '-------------------------------------------------------------------

STP_RETRY:
            RtnCode = cFRS_NORMAL
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        '�u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)                            ' �d�����b�N(�ω����E�����b�N)����������
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                RtnCode = r                                             ' Return�l�ݒ� 
                GoTo STP_END                                            ' �����I���� 
            End If
            Call ZCONRST()

            ''V5.0.0.4�A 
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            ''V5.0.0.4�A 

            '-------------------------------------------------------------------
            '   �u�T�C�N����~���v���b�Z�[�W��\�����āuSTART�L�[�v�uRESET�L�[�v
            '    �̉����҂�(���̊ԂɊ�����o���Ċm�F���\)
            '-------------------------------------------------------------------
            Dim md As Short = Globals_Renamed.giAppMode                 'V5.0.0.9�P
            Globals_Renamed.giAppMode = APP_MODE_IDLE                   'V5.0.0.9�P
            ' ���b�Z�[�W�\��(START�L�[, RESET�L�[�����҂�)
            ' "�T�C�N����~��", "OK�{�^�������Ŏ����^�]�𑱍s���܂�", "Cancel�{�^�������Ŏ����^�]���I�����܂�"
            r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                    MSG_LOADER_48, MSG_LOADER_45, MSG_LOADER_46, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            Globals_Renamed.giAppMode = md                              'V5.0.0.9�P
            If (r < cFRS_NORMAL) Then                                   ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                RtnCode = r                                             ' Return�l�ݒ� 
                GoTo STP_END                                            ' �����I����
            ElseIf (r = cFRS_ERR_RST) Then                              ' Cancel(RESET�L�[����) ? 
                RtnCode = cFRS_ERR_RST                                  ' Return�l = Cancel(RESET�L�[����)
            End If

            '-------------------------------------------------------------------
            '   �d�����b�N(�ω����E�����b�N)�����b�N����
            '-------------------------------------------------------------------
            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                RtnCode = r                                             ' Return�l�ݒ� 
                GoTo STP_END                                            ' �����I���� 
            End If

            ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         '�u�Œ�J�o�[�J�`�F�b�N����v�ɂ���

            ' �d�����b�N(�ω����E�����b�N)�����b�N����
            r = EL_Lock_OnOff(EX_LOK_MD_ON)                             ' �d�����b�N
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                RtnCode = r                                             ' Return�l�ݒ� 
                GoTo STP_END                                            ' �����I���� 
            End If

            '-------------------------------------------------------------------
            '   �uSTART�L�[�������v�͊������m�F
            '   �uRESET�L�[�������v�͊�Ȃ����m�F
            '-------------------------------------------------------------------
            If (RtnCode = cFRS_ERR_RST) Then                            ' RESET�L�[���� ?
STP_010:
                ' �ڕ���Ɋ���Ȃ������m�F(�ڕ���Ɋ������ꍇ�́A��菜�����܂ő҂�)
                r = SubstrateCheck(Form1.System1, APP_MODE_FINEADJ)     ' �� �ꎞ��~��ʃ��[�h��Call
                If (r < cFRS_NORMAL) Then                               ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                    RtnCode = r                                         ' Return�l�ݒ� 
                    GoTo STP_END                                        ' �����I����
                End If
            Else                                                        ' START�L�[������
                'V5.0.0.4�B  STP_010:        'V5.0.0.4�B�ړ�
                ' �ڕ���̊����/�Ȃ����m�F
                RtnCode = Sub_SubstrateExistCheck2(ObjSys)              ' RtnCode = cFRS_NORMAL(����葱�s), cFRS_ERR_START(��Ȃ����s), cFRS_ERR_RST(��Ȃ���Cancel(RESET�L�[����)
                If (RtnCode < cFRS_NORMAL) Then                         ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                    GoTo STP_END                                        ' �����I����
                End If
                '�u��Ȃ����s�v�w��Ȃ�ڕ���Ɋ���Ȃ������m�F��
                If (RtnCode = cFRS_ERR_START) Then
                    GoTo STP_010
                End If
                ' �������o�Ȃ�ēx������`�F�b�N
                If (RtnCode = cFRS_ERR_HALT) Then
                    GoTo STP_RETRY
                End If
            End If

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
STP_END:
            ' �T�C�N����~�v���M����OFF����
            r = W_CycleStop(ObjSys, 0)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?(���b�Z�[�W�͕\����)
                RtnCode = r                                             ' Return�l�ݒ� 
            End If

            ' �V�O�i���^���[����(On=�����^�]��(�Γ_��),Off=�S�ޯ�)
            'V5.0.0.9�M ���@V6.0.3.0�G
            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
            'V5.0.0.9�M ���@V6.0.3.0�G

            ' PLC���Ɠ�������邽�ߋz��OFF(I/O)���s��
            Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF)

            Return (RtnCode)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.CycleStop_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            RtnCode = cERR_TRAP
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R�� -----
    '----- V4.0.0.0-26�� -----
#Region "10�i��BCD�ϊ�"
    '''=========================================================================
    ''' <summary>10�i��BCD�ϊ�</summary>
    ''' <param name="strDec">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>cFRS_NORMAL     = ����
    '''          cFRS_ERR_RST    = Cancel(RESET�L�[����)
    '''          cFRS_TO_EXLOCK  = �O�ʔ����b�N�^�C���A�E�g
    '''          ��L�ȊO        = ���̑��̃G���[
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

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "LoaderIOFor436.DecToBcd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (0)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26�� -----
    '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
#Region "������{�^���������̏���"
    '''=========================================================================
    ''' <summary>������{�^���������̏���</summary>
    ''' <param name="ObjSys">       (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>cFRS_NORMAL     = ����
    '''          cFRS_ERR_RST    = Cancel(RESET�L�[����)
    '''          ��L�ȊO        = ���̑��̃G���[
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
            '   ��ǉ��v���M�����M(SL436S)
            '-------------------------------------------------------------------
            ' ���[�_�����܂��̓��[�_�蓮���[�h�Ȃ�NOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If
            ' ������t���OOFF�Ȃ�NOP
            If (gbChkSubstrateSet = False) Then
                Return (cFRS_NORMAL)
            End If
            ' 

            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(False)
            End If
            ''V5.0.0.1�C��
            ' ������{�^���̔w�i�F���D�F�ɂ���
            ' gbChkSubstrateSet = False                                   ' ������t���OOFF
            Form1.BtnSubstrateSet.Enabled = False                       ' ������{�^���񊈐���
            '            Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)

STP_RETRY:
            'V5.0.0.1�G
            If IsNothing(gObjMSG) = True Then
                gObjMSG = New FrmWait()
                Call gObjMSG.Show(Form1)
            End If
            'V5.0.0.1�G

            ' �^�C�}�[�l��ݒ肷��
            If (giOPLDTimeOutFlg = 0) Then                              ' ���[�_�ʐM�^�C���A�E�g���o���� ?
                TimeVal = System.Threading.Timeout.Infinite             ' �^�C�}�[�l = �Ȃ�
            Else
                TimeVal = giOPLDTimeOut                                 ' �^�C�}�[�l = ���[�_�ʐM�^�C���A�E�g����(msec)
            End If

            ' ��ǉ��v���M�����M
            lData = LPCS_SET_SUBSTRATE
            Call m_PlcIf.WritePlcWR(LOFS_W113S, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W113S, lData)

            '-------------------------------------------------------------------
            '   �^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬
            '   (TimerLock_Tick��X msec�Ԋu�Ŏ��s����)
            '-------------------------------------------------------------------
            TimerTM_Create(TimerLock, TimeVal)

            'V5.0.0.1�I���C���V�[�P���X�ŃG���[�`�F�b�N�����Ȃ��悤�ɐݒ肷��
            giErrLoader = cFRS_ERR_RST

            '-------------------------------------------------------------------
            '   ��ǉ��v�������҂�
            '-------------------------------------------------------------------
            Do
                System.Threading.Thread.Sleep(10)                       ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' ���[�_�A���[��/����~�`�F�b�N
                r = GetLoaderIO(LdIn)                                   ' ���[�_
                If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                    rtnCode = cFRS_ERR_LDR                              ' Return�l = ���[�_�A���[�����o
                    GoTo STP_ERR_LDR                                    ' ���[�_�A���[���\����
                End If

                ' ��ǉ��v�����������҂�
                Call m_PlcIf.ReadPlcWR(LOFS_W114S, lData)               ' ��ǉ��v�������擾
                Console.WriteLine("SubstrateSet_Proc() ReadPlcWR =" + lData.ToString("0"))
                If (lData And LPLCS_SET_SUBSTRATE) Then
                    Exit Do                                             ' ��ǉ��v�������Ȃ�Exit
                End If

                '-------------------------------------------------------------------
                '   �^�C���A�E�g�`�F�b�N 
                '-------------------------------------------------------------------
                bTOut = TimerTM_Sts()
                If (bTOut = True) Then                                  ' �^�C���A�E�g ? 
                    rtnCode = cFRS_ERR_LDRTO                            ' Retuen�l = ���[�_�ʐM�^�C���A�E�g 
                    GoTo STP_END
                End If

            Loop While (1)
            'V4.11.0.0�J
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            'V4.11.0.0�J

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            ' �ꎞ��~�J�n���Ԃ�ݒ肷��(�I�v�V����)
            m_blnElapsedTime = True                             ' �o�ߎ��Ԃ�\������
            Call Set_PauseStartTime(StPauseTime)
            '----- V4.11.0.0�C�� -----

            '-------------------------------------------------------------------
            '   ���b�Z�[�W��\������START�{�^���̉����҂�
            '-------------------------------------------------------------------
            ' �V�O�i���^���[����(On=�Ȃ�, Off=�����^�]��(�Γ_��))
            'V5.0.0.9�M ��
            'Call Form1.System1.SetSignalTower(0, SIGOUT_GRN_ON)
            Call Form1.System1.SetSignalTowerCtrl(ObjSys.SIGNAL_IDLE)
            'V5.0.0.9�M ��

            '�u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ��� 
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)

            Form1.TimerAdjust.Enabled = False ''V5.0.0.1�I

            ' "�J�o�[���J���Ċ���Z�b�g���Ă�������","�ĊJ����ꍇ��START�{�^���������ĉ������B","
            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_BTN_START, True, _
                    MSG_LOADER_50, MSG_LOADER_51, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            'r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
            '        MSG_LOADER_50, MSG_LOADER_51, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            If (r < cFRS_NORMAL) Then                                   ' ����~���̃G���[�Ȃ�G���[�߂�(�G���[���b�Z�[�W�͕\���ς�) 
                rtnCode = r                                             ' Return�l�ݒ� 
            End If

            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()

            ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         '�u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����

            ' �V�O�i���^���[����(On=�����^�]��(�Γ_��),Off=�S�ޯ�)
            'V5.0.0.9�M ��
            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
            'V5.0.0.9�M ��


            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            ' �ꎞ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����(�I�v�V����)
            Call Set_PauseTotalTime(StPauseTime)
            ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��(�I�v�V����)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C�� -----

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
STP_END:
            'V4.11.0.0�J
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            'V4.11.0.0�J

            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1�C��

            ' ������{�^���̔w�i�F���D�F�ɂ���
            gbChkSubstrateSet = False                                   ' ������t���OOFF
            Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
            'V5.0.0.1�A��
            If Form1.BtnADJ.BackColor = System.Drawing.Color.Yellow Then
                Form1.BtnSubstrateSet.Enabled = True                        ' ������{�^��������
            End If
            'V5.0.0.1�A��
            TimerTM_Stop(TimerLock)                                     ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerTM_Dispose(TimerLock)                                  ' �^�C�}�[��j������

            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ���[�_�ʐM�^�C���A�E�g ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' �G���[���b�Z�[�W�\��
            End If

            ' ��ǉ��v���M��OFF���M
            Call m_PlcIf.WritePlcWR(LOFS_W113S, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W113S, 0)

            'V5.0.0.1�I���C���V�[�P���X�ŃG���[�`�F�b�N����
            giErrLoader = cFRS_NORMAL

            Return (rtnCode)

            ' ���[�_�G���[������
STP_ERR_LDR:

            'V4.11.0.0�J
            If IsNothing(gObjMSG) <> True Then
                gObjMSG.MsgClose()
                gObjMSG = Nothing
            End If
            'V4.11.0.0�J

            TimerTM_Stop(TimerLock)                                     ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerTM_Dispose(TimerLock)                                  ' �^�C�}�[��j������
            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ���[�_�ʐM�^�C���A�E�g ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' �G���[���b�Z�[�W�\��
            Else
                ' ���[�_�A���[�����b�Z�[�W�쐬 & ���[�_�A���[����ʕ\��
                rtnCode = Loader_AlarmCheck(ObjSys, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                Call ZCONRST()     ''V5.0.0.1�I
                'V5.0.0.1�E
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then
                    Call W_START()                                  ' �X�^�[�g�M�����o 
                    GoTo STP_RETRY
                End If
                'V5.0.0.1�E
            End If
            Form1.BtnSubstrateSet.Enabled = True                        ' ������{�^�������� 'V4.11.0.0�O
            ''V5.0.0.1�C��
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1�C��
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.SubstrateSet_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            rtnCode = cERR_TRAP
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V4.11.0.0�E�� -----

    '===============================================================================
    '   �I�[�g���[�_�A���[������
    '===============================================================================
#Region "���[�_�A���[���`�F�b�N"
    '''=========================================================================
    ''' <summary>���[�_�A���[���`�F�b�N</summary>
    ''' <param name="ObjSys">            (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="bDspFlg">           (INP)�A���[�����X�g��\������(True)/�\�����Ȃ�(False)</param>
    ''' <param name="AlarmCount">        (OUT)�����A���[����</param>
    ''' <param name="strLoaderAlarm">    (OUT)�A���[��������</param>
    ''' <param name="strLoaderAlarmInfo">(OUT)�A���[�����1</param>
    ''' <param name="strLoaderAlarmExec">(OUT)�A���[�����(�΍�)</param>
    ''' <returns>�A���[�����
    '''          cFRS_NORMAL   = ����(�A���[������)
    '''          cFRS_ERR_LDR1 = �S��~�ُ�
    '''          cFRS_ERR_LDR2 = �T�C�N����~
    '''          cFRS_ERR_LDR3 = �y�̏�(���s�\)
    '''          cFRS_ERR_PLC  = PLC�X�e�[�^�X�ُ�
    '''          cFRS_ERR_RST  = �y�̏�(���s�\)��Cancel�w��
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
            ' ���[�_�����Ȃ�NOP
            If (bFgActLink = False) Then
                Return (cFRS_NORMAL)
            End If

            ' ���[�_���A���[����Ԃ��`�F�b�N����
            r = W_Read(LOFS_W110, lData)                                ' ���[�_�A���[����Ԏ擾(W110.08-10)
            If (r <> cFRS_NORMAL) Then
                ' PLC�X�e�[�^�X�ُ�̏ꍇ�̓G���[���X�g�ɂ�PLC�X�e�[�^�X�ُ��\������
                iRetc = cFRS_ERR_PLC                                    ' Return�l = PLC�X�e�[�^�X�ُ�

                ' ���[�_�A���[����ʂ�\������
                If (bDspFlg = True) Then                                ' ��ʕ\������ ?
                    r = Sub_CallFormLoaderAlarm(ObjSys, iRetc, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                    If (r = cFRS_ERR_RST) Then                          ' ���[�_�A���[�����o(�y�̏�)��Cancel�w��Ȃ� ###073
                        Call W_RESET()                                  ' �A���[�����Z�b�g���o 
                        Call W_START()                                  ' �X�^�[�g�M�����o 
                        iRetc = cFRS_ERR_RST                            ' Return�l = Cancel(RESET��)�Ƃ��� 
                    End If
                End If
            Else
                ' �A���[����Ԃ��`�F�b�N����
                iData(0) = lData
                If (iData(0) And LARM_ARM1) Then                        ' �y�̏ᔭ����
                    iRetc = cFRS_ERR_LDR3                               ' Return�l = ���[�_�A���[�����o(�y�̏�) 
                End If
                If (iData(0) And LARM_ARM2) Then                        ' �T�C�N����~�ُ픭����
                    iRetc = cFRS_ERR_LDR2                               ' Return�l = ���[�_�A���[�����o(�T�C�N����~)
                End If
                If (iData(0) And LARM_ARM3) Then                        ' �S��~�ُ픭����
                    iRetc = cFRS_ERR_LDR1                               ' Return�l = ���[�_�A���[�����o(�S��~�ُ�)
                End If

                ' ���[�_�A���[���ڍׂ��擾����(�A���[��������)
                If (iRetc <> cFRS_NORMAL) Then                          ' �A���[������ ?
                    r = W_Read(LOFS_W115, lData)                        ' ���[�_�A���[���ڍ׎擾(W115.00-W115.15(���s�s��))
                    iData(0) = lData
                    r = W_Read(LOFS_W116, lData)                        ' ���[�_�A���[���ڍ׎擾(W116.00-W116.15(���s��))
                    iData(1) = lData

                    ' �A���[�����e���ς�������`�F�b�N����
                    bFg = False
                    For i = 0 To 1
                        If (iBefData(i) <> iData(i)) Then               ' �A���[�����e���ς���� ? 
                            bFg = True
                        End If
                    Next

                    ' �A���[�����e���ς������A���[������iBefData()�ɑޔ�����
                    If (bFg) Then                                       ' �A���[�����e���ς������
                        ' �A���[������ޔ�����
                        For i = 0 To 1
                            iBefData(i) = iData(i)                      ' �A���[������ޔ�
                        Next

                        ' ���[�_�A���[�����b�Z�[�W���쐬����(AlmCount = �����A���[����)
                        AlarmCount = Loader_MakeAlarmStrings(iData, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        '
                        If iRetc = cFRS_ERR_LDR1 Then
                            If (iData(0) And &H8) Or (iData(0) And &H10) Or (iData(0) And &H20) Or (iData(0) And &H2000) Then
                                strLoaderAlarmExec(0) = strLoaderAlarmExec(0) + MSG_LOADER_52
                            End If
                            'V5.0.0.1�N��
                        ElseIf iRetc = cFRS_ERR_LDR3 Then
                            strLoaderAlarmExec(0) = strLoaderAlarmExec(0) + MSG_LOADER_53
                        End If
                        'V5.0.0.1�N��

                    End If

                    ' ���[�_�A���[����ʂ�\������
                    If (bDspFlg = True) Then                            ' ��ʕ\������ ?
                        'V5.0.0.1�B��
                        Form1.TimerAdjust.Enabled = False
                        'V5.0.0.1�B��
                        r = Sub_CallFormLoaderAlarm(ObjSys, iRetc, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        If (r = cFRS_ERR_RST) Then                      ' ���[�_�A���[�����o(�y�̏�)��Cancel�w��Ȃ� ###073
                            Call W_RESET()                              ' �A���[�����Z�b�g���o 
                            'Call W_START()                             ' �X�^�[�g�M�����o ###174(���s����̂ŃR�����g��)
                            iRetc = cFRS_ERR_RST                        ' Return�l = Cancel(RESET��)�Ƃ��� 
                        End If
                        'V5.0.0.1�B��
                        Call ZCONRST()
                        Form1.TimerAdjust.Enabled = True
                        'V5.0.0.1�B��
                    End If

                    ' ���펞
                Else
                    Call ClearBefAlarm()                                ' �A���[�����ޔ���N���A
                End If

            End If

            Return (iRetc)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_AlarmCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V2.0.0.0�A�� -----
#Region "���[�_�A���[���`�F�b�N(�����^�]���ȊO SL436RS�p)"
    '''=========================================================================
    ''' <summary>���[�_�A���[���`�F�b�N(�����^�]���ȊO SL436RS�p)</summary>
    ''' <param name="ObjSys">            (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <returns>�A���[�����
    '''          cFRS_NORMAL   = ����(�A���[������)
    '''          cFRS_ERR_LDR1 = �S��~�ُ�
    '''          cFRS_ERR_LDR2 = �T�C�N����~
    '''          cFRS_ERR_LDR3 = �y�̏�(���s�\)
    '''          cFRS_ERR_PLC  = PLC�X�e�[�^�X�ُ�
    '''          cFRS_ERR_RST  = �y�̏�(���s�\)��Cancel�w��
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
            '   ��������
            '-------------------------------------------------------------------
            ' SL436RS�łȂ����NOP
            If (giMachineKd <> MACHINE_KD_RS) Or (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                Return (cFRS_NORMAL)
            End If

            ' ���[�_�����Ȃ�NOP
            If (bFgActLink = False) Then
                Return (cFRS_NORMAL)
            End If

            ' �����^�]���Ȃ�NOP
            If (bFgAutoMode = True) Then
                Return (cFRS_NORMAL)
            End If

            ' ���[�_�A���[�������łȂ����NOP
            Call GetLoaderIO(LdIn)                                      ' ���[�_����
            If ((LdIn And LINP_NO_ALM_RESTART) = LINP_NO_ALM_RESTART) Then
                Return (cFRS_NORMAL)
            End If

            '-------------------------------------------------------------------
            '   ���[�_���A���[����Ԃ��`�F�b�N����(�A���[��������)
            '-------------------------------------------------------------------
            r = W_Read(LOFS_W110, lData)                                ' ���[�_�A���[����Ԏ擾(W110.08-10)
            If (r <> cFRS_NORMAL) Then
                iRetc = cFRS_ERR_PLC                                    ' Return�l = PLC�X�e�[�^�X�ُ�
                strMSG = MSG_SPRASH30                                   ' "PLC�X�e�[�^�X�ُ�"
                strMS2 = MSG_SPRASH16                                   ' "RESET�L�[�������ƃv���O�������I�����܂�"

            Else
                ' �A���[����Ԃ��`�F�b�N����
                iData(0) = lData
                If (iData(0) And LARM_ARM1) Then                        ' �y�̏ᔭ����
                    iRetc = cFRS_ERR_LDR3                               ' Return�l = ���[�_�A���[�����o(�y�̏�) 
                End If
                If (iData(0) And LARM_ARM2) Then                        ' �T�C�N����~�ُ픭����
                    iRetc = cFRS_ERR_LDR2                               ' Return�l = ���[�_�A���[�����o(�T�C�N����~)
                End If
                If (iData(0) And LARM_ARM3) Then                        ' �S��~�ُ픭����
                    iRetc = cFRS_ERR_LDR1                               ' Return�l = ���[�_�A���[�����o(�S��~�ُ�)
                End If

                ' ���[�_�A���[���ڍׂ��擾����(�A���[��������)
                If (iRetc <> cFRS_NORMAL) Then                          ' �A���[������ ?
                    r = W_Read(LOFS_W115, lData)                        ' ���[�_�A���[���ڍ׎擾(W115.00-W115.15(���s�s��))
                    iData(0) = lData
                    r = W_Read(LOFS_W116, lData)                        ' ���[�_�A���[���ڍ׎擾(W116.00-W116.15(���s��))
                    iData(1) = lData
                End If

                ' �����ُ팟�o�ȊO�̃A���[���͖�������
                If ((iData(0) And LDFS_ARM_AIR) = LDFS_ARM_AIR) Then
                    strMSG = MSG_SPRASH12                               ' "�G�A�[���ቺ���o"
                    strMS2 = MSG_SPRASH16                               ' "RESET�L�[�������ƃv���O�������I�����܂�"
                Else
                    iRetc = cFRS_NORMAL
                End If

            End If

            '-------------------------------------------------------------------
            '   ���b�Z�[�W�\��
            '-------------------------------------------------------------------
            If (iRetc <> cFRS_NORMAL) Then                              ' �A���[������ ? 
                ' ���b�Z�[�W�\��(RESET�L�[�����҂�)
                r = Sub_CallFrmMsgDisp(ObjSys, cGMODE_MSG_DSP, cFRS_ERR_RST, True, _
                        strMSG, strMS2, strMS3, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.SystemColors.ControlText)
                If (r < cFRS_NORMAL) Then                               ' ����~���̃G���[�Ȃ�
                    iRetc = r                                           ' Return�l���Đݒ肷��
                End If
            End If

            Return (iRetc)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_AlarmCheck_ManualMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V2.0.0.0�A�� -----
#Region "�A���[�����ޔ���N���A"
    '''=========================================================================
    ''' <summary>�A���[�����ޔ���N���A</summary>
    '''=========================================================================
    Public Sub ClearBefAlarm()

        Dim Len As Integer
        Dim i As Integer
        Dim strMSG As String

        Try
            ' �A���[�����ޔ��������������
            Len = iBefData.Length
            For i = 0 To (Len - 1)
                iBefData(i) = 0
            Next

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.ClearBefAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���[�_�A���[�����b�Z�[�W���쐬����"
    '''=========================================================================
    ''' <summary>���[�_�A���[�����b�Z�[�W���쐬����</summary>
    ''' <param name="iData">             (INP)���[�_�A���[���ڍ�</param>
    ''' <param name="strLoaderAlarm">    (OUT)�A���[��������</param>
    ''' <param name="strLoaderAlarmInfo">(OUT)�A���[�����1</param>
    ''' <param name="strLoaderAlarmExec">(OUT)�A���[�����(�΍�)</param>
    ''' <returns>�����A���[����</returns>
    '''=========================================================================
    Public Function Loader_MakeAlarmStrings(ByRef iData() As UShort, _
                                            ByRef strLoaderAlarm() As String, ByRef strLoaderAlarmInfo() As String, ByRef strLoaderAlarmExec() As String) As Integer

        Dim i As Integer
        Dim j As Integer
        Dim num As Integer
        Dim strMSG As String

        Try
            ' ���[�_�A���[�����b�Z�[�W���쐬����
            num = 0                                                                             ' �����A���[����
            For i = 0 To 1
                For j = 0 To 15
                    If (iData(i) And (2 ^ j)) Then                                              ' �A���[���r�b�gON ?
                        Select Case ((i * 16) + j)
                            Case 0 ' W115.00
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_00   ' ����~
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_00                         ' ����~�r�v��������܂����B

                            Case 1 ' W115.01
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_01   ' �}�K�W���������A���[��
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_01                         ' �}�K�W�������K�̈ʒu���m�F���Ă��������B

                            Case 2 ' W115.02
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_02   ' ���ꌇ���i����
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_02                         ' ���ꌇ���i���w�肳�ꂽ�����������܂����B

                            Case 3 ' W115.03
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_03   ' �n���h�P�z���A���[��
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_03                         ' �z���Z���T���m�F���Ă��������B

                            Case 4 ' W115.04
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_04   ' �n���h�Q�z���A���[��
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_04                         ' �z���Z���T���m�F���Ă��������B

                            Case 5 ' W115.05
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_05   ' �ڕ���z���Z���T�ُ�
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_05                         ' �z���Z���T���m�F���Ă��������B

                            Case 6 ' W115.06
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_06   ' �ڕ���z���~�X
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_06                         ' �z���Z���T���m�F���Ă��������B �g�b�v�v���[�g�ɃL�Y���̂����炪�������m�F���Ă��������B

                            Case 7 ' W115.07
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_07   ' ���{�b�g�A���[��
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_07                         ' ���{�b�g�A���[�����������܂����B

                            Case 8 ' W115.08
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_08   ' �H���ԊĎ��A���[��
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_08                         ' �H���ԊĎ��Ń^�C���A�E�g���������܂����B

                            Case 9 ' W115.09
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_09   ' �G���x�[�^�ُ�
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_09                         ' �G���x�[�^�̃Z���T���m�F���Ă��������B

                            Case 10 ' W115.10
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_10   ' �}�K�W������
                                strLoaderAlarmInfo(num) = ""
                                'strLoaderAlarmExec(num) = MSG_LDGUID_11                         ' �}�K�W�����o�h�O���|��Ă��邩�A�}�K�W�����Z�b�g���Ă��������BV6.0.3.0_41
                                strLoaderAlarmExec(num) = MSG_LDGUID_10                        ' �}�K�W�����o�h�O���|��Ă��邩�A�}�K�W�����Z�b�g���Ă��������BV6.0.3.0_41
                                giReqLotSelect = 1                                              ' �I�����Ƀ��b�g�G���h�A�p���̉�ʂ�\��   V6.0.3.0_38

                            Case 11 ' W115.11
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_11   ' ���_���A�^�C���A�E�g
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_11                         ' ���_���A���Ƀ^�C���A�E�g���������܂����B

                            Case 12 ' W115.12
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_12   ' �N�����v�ُ� ###125
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_12                         ' �N�����v�ُ킪�������܂��� ###125

                            Case 13 ' W115.13
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_13   ' �G�A�[���ቺ���o 'V2.0.0.0�B 
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_13                         ' �A�[����������������Ă��邩�m�F���Ă��������B 'V2.0.0.0�B 

                            Case 14 ' W115.14
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_14   ' �����}�K�W���A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_14                         ' �z��O�A���[��

                            Case 15 ' W115.15
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_15   ' ���[�}�K�W�����t�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_15                         ' �z��O�A���[��

                            Case 16 ' W116.00
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_16   ' ����V�����_�^�C���A�E�g
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_16                         ' �V�����_�Z���T���m�F���Ă��������B

                            Case 17 ' W116.01
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_17   ' �n���h�P�V�����_�^�C���A�E�g
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_17                         ' �V�����_�Z���T���m�F���Ă��������B

                            Case 18 ' W116.02
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_18   ' �n���h�Q�V�����_�^�C���A�E�g
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_18                         ' �V�����_�Z���T���m�F���Ă��������B

                            Case 19 ' W116.03
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_19   ' �����n���h�z���~�X
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_19                         ' ��z�����Ƀ^�C���A�E�g���������܂����B

                            Case 20 ' W116.04
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_20   ' ����n���h�z���~�X
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_20                         ' ��z�����Ƀ^�C���A�E�g���������܂����B

                            Case 21 ' W116.05
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_21   ' �m�f�r�o���t
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_21                         ' �m�f�r�o�a�n�w�����t�ł��B�����菜���Ă��������B

                            Case 22 ' W116.06
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_22   ' �ꎞ��~
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_22                         ' �ꎞ��~���ł��B

                            Case 23 ' W116.07
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_23   ' �h�A�I�[�v��
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_23                         ' �h�A�I�[�v�������o����܂����B�h�A����Ă��������B

                            Case 24 ' W116.08
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_24   ' �񖇎�茟�o�@' V1.18.0.0�J
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_24                         ' �����n���h�̊����菜���čĎ��s���Ă��������B ' V1.18.0.0�J

                            Case 25 ' W116.09
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_25   ' �����㉺�@�\�A���[�� 'V4.0.0.0-59
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_25                         '  �����㉺�@�\���m�F���Ă��������B 'V4.0.0.0-59

                            Case 26 ' W116.10
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_26   ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_26                         ' �z��O�A���[��

                            Case 27 ' W116.11
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_27   ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_27                         ' �z��O�A���[��

                            Case 28 ' W116.12
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_28   ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_28                         ' �z��O�A���[��

                            Case 29 ' W116.13
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_29   ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_29                         ' �z��O�A���[��

                            Case 30 ' W116.14
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_30   ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_30                         ' �z��O�A���[��

                            Case 31 ' W116.15
                                strLoaderAlarm(num) = "No." & ((i * 16) + j) & MSG_LDALARM_31   ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = ""
                                strLoaderAlarmExec(num) = MSG_LDGUID_31                         ' �z��O�A���[��

                            Case Else
                                strLoaderAlarm(num) = MSG_LDALARM_UD & ((i * 16) + j)           ' ����`�A���[�� No.
                                strLoaderAlarmInfo(num) = MSG_LDINFO_UD                         ' �z��O�A���[��
                                strLoaderAlarmExec(num) = MSG_LDGUID_UD                         ' ���[�J�ɃA���[���ԍ���₢���킹�Ă��������B
                        End Select
                        num = num + 1                                                           ' �����A���[���� + 1

                    End If

                Next j
            Next i

            Return (num)                                                                        ' Return�l = �����A���[����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_MakeAlarmStrings() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "FormLoaderAlarm���s�T�u���[�`��"
    '''=========================================================================
    ''' <summary>FormLoaderAlarm���s�T�u���[�`��</summary>
    ''' <param name="ObjSys">            (INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="AlarmKind">         (INP)�A���[�����(�S��~�ُ�, �T�C�N����~, �y�̏�, �A���[������)</param>
    ''' <param name="AlarmCount">        (INP)�����A���[����</param>
    ''' <param name="strLoaderAlarm">    (INP)�A���[��������</param>
    ''' <param name="strLoaderAlarmInfo">(INP)�A���[�����1(�����g�p)</param>
    ''' <param name="strLoaderAlarmExec">(INP)�A���[�����(�΍�)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
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
            ' ���[�_����N�Ȃ�OP
            If (bFgActLink = False) Then                                ' ���[�_���� ?
                Return (cFRS_NORMAL)
            End If

            ' �A�v�����[�h���u���[�_�A���[���\���v�ɂ��� ###088
            svAppMode = giAppMode
            giAppMode = APP_MODE_LDR_ALRM
            Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' �u�Œ�J�o�[�J�`�F�b�N�Ȃ��v�ɂ���

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����������(���[���a����)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�����^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ?
                Return (r)
            End If
            '----- V1.18.0.1�G�� -----

            ' �V�O�i���^���[����(On=�ُ�, Off=�S�ޯ�) ###191
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��(�ԓ_��)
                    'V5.0.0.9�M ���@V6.0.3.0�G(���[���a�d�l�͐ԓ_�Ł{�u�U�[�n�m)
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                    'V5.0.0.9�M ���@V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(�ԓ_��+�u�U�[�P)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            ' �A���[�����b�Z�[�W�\����ON�M�����o
            Call W_ALM_DSP()                                            ' V1.18.0.0�M

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            ' ��~�J�n���Ԃ�ݒ肷��(�I�v�V����)
            m_blnElapsedTime = True                             ' �o�ߎ��Ԃ�\������
            Call Set_PauseStartTime(StPauseTime)
            '----- V4.11.0.0�C�� -----

            ' ���[�_�A���[����ʂ�\������
            bFgLoaderAlarmFRM = True                                    ' ���[�_�A���[����ʕ\����ON
            'V6.0.0.0�J            objForm = New FormLoaderAlarm()
            Dim objForm As New FormLoaderAlarm()    'V6.0.0.0�J
            Call objForm.ShowDialog(Nothing, ObjSys, AlarmKind, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            rtn = objForm.sGetReturn()                                  ' Return�l�擾 ###200

            ' �I�u�W�F�N�g�J��
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                    ' �I�u�W�F�N�g�J��
                Call objForm.Dispose()                                  ' ���\�[�X�J��
            End If
            bFgLoaderAlarmFRM = False                                   ' ���[�_�A���[����ʕ\����OFF

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) ----
            ' ��~�I�����Ԃ�ݒ肵�ꎞ��~���Ԃ��W�v����(�I�v�V����)
            Call Set_PauseTotalTime(StPauseTime)
            ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��(�I�v�V����)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C�� -----

            '----- ###200�� ----- 
            '' �A�v�����[�h�����ɖ߂� ###088
            'giAppMode = svAppMode
            'Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A ###088
            'Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            '----- ###200�� ----- 

            ' �A���[�����b�Z�[�W�\����OFF�M�����o
            Call W_ALM_DSP()                                            ' V1.18.0.0�M

            ' �V�O�i���^���[����(On=0, Off=�ُ�) ###191
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' �W��
                    'V5.0.0.9�M �� V6.0.3.0�G
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                    'V5.0.0.9�M �� V6.0.3.0�G

                Case SIGTOWR_SPCIAL                                     ' ����(�ԓ_��+�u�U�[�P)
                    'r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            End Select

            '----- V1.18.0.7�@�� -----
            ' �����^�]��(�ꎞ��~���ȊO)�̓V�O�i���^���[����(�����^�]��(�Γ_��))���s��
            If ((bFgAutoMode = True) And (gObjADJ Is Nothing = True)) Then
                ' �V�O�i���^���[����(On=�����^�]��(�Γ_��),Off=�S�ޯ�)
                'V5.0.0.9�M �� V6.0.3.0�G
                ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                'V5.0.0.9�M �� V6.0.3.0�G

            End If
            '----- V1.18.0.7�@�� -----

            ' ----- V1.18.0.0�B�� -----
            ' �A���[����~����ݒ肷��(���[���a����)
            Call SetAlmEndTime()
            ' ----- V1.18.0.0�B�� -----

            '----- ###200�� ----- 
            ' NG�r�oBOX�����t�̏ꍇ�́A��菜�����܂ő҂�
            r = NgBoxCheck(Form1.System1, APP_MODE_LOADERINIT)
            If (r < cFRS_NORMAL) Then
                Return (r)
            End If

            ' ➑̃J�o�[���m�F����
            r = FrmReset.Sub_CoverCheck()
            If (r < cFRS_NORMAL) Then
                Return (r)
            End If

            giAppMode = svAppMode
            Call COVERLATCH_CLEAR()                                     ' �J�o�[�J���b�`�̃N���A ###088
            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �u�Œ�J�o�[�J�`�F�b�N����v�ɂ���
            '----- ###200�� ----- 

            '----- V1.18.0.1�G�� -----
            ' �d�����b�N(�ω����E�����b�N)����
            If (giAppMode = APP_MODE_FINEADJ) Then
                ' �ꎞ��~��ʂȂ�d�����b�N(�ω����E�����b�N)����������(���[���a����)
                r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            Else
                r = EL_Lock_OnOff(EX_LOK_MD_ON)
            End If
            If (r = cFRS_TO_EXLOCK) Then                                ' �u�O�ʔ����b�N�^�C���A�E�g�v�Ȃ�߂�l���uRESET�v�ɂ���
                r = cFRS_ERR_RST
                Return (r)
            End If
            If (r < cFRS_NORMAL) Then                                   ' �ُ�I�����x���̃G���[ ? 
                Return (r)
            End If
            '----- V1.18.0.1�G�� -----

            Return (rtn)                                                ' Return(�G���[���̃��b�Z�[�W�͕\����) ###200

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Sub_CallFormLoaderAlarm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '===============================================================================
    '   �I�[�g���[�_�V���A���ʐM�p�֐�(SysmacCompolet.dll�g�p)
    '===============================================================================
#Region "PLC�ʐM�ݒ�̏�����"
    '''=========================================================================
    ''' <summary>PLC�ʐM�ݒ�̏�����</summary>
    '''=========================================================================
    Public Function Init_PlcIF() As Integer
        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            MessageBox.Show("PLC OFFLINE DEBUG", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return (cFRS_NORMAL)
#End If
            m_PlcIf = New DllPlcIf.DllPlcInterface()

            'PLC�ʐM�A�h���X�ݒ�
            m_PlcIf.SetNetAddress(0)
            m_PlcIf.SetNodeAddress(240)
            m_PlcIf.SetUnitAddress(0)
            m_PlcIf.SetReceiveTimeLimit(1000)


            Return ret
        Catch ex As Exception
            Dim strMsg As String
            strMsg = ex.Message + "(Init_PlcIF)"
            MsgBox(strMsg)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try

    End Function
#End Region

#Region "�f�[�^����(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�f�[�^����(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="Offset">(INP)�I�t�Z�b�g(10�i)</param>
    ''' <param name="lData"> (OUT)���̓f�[�^(16�i)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function DM_Read(ByVal Offset As Long, ByRef lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �f�[�^����
            'txtCJDMValue.Text = Hex(SysmacCJ1.DM(CLng(txtCJDMOffset.Text)))
            'lData = Form1.SysmacCJ1.DM(Offset)
            'lData = Form1.SysmacCJ1.DM(Offset)

            ret = m_PlcIf.ReadPlcDM(Offset, lData)
            Return (ret)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(DM_Read)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try
    End Function
#End Region

#Region "�f�[�^�o��(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�f�[�^�o��(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="Offset">(INP)�I�t�Z�b�g(10�i)</param>
    ''' <param name="lData"> (INP)�o�̓f�[�^(16�i)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function DM_Write(ByVal Offset As Long, ByVal lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �f�[�^�o��
            'Form1.SysmacCJ1.DM(CLng(txtCJDMOffset.Text)) = CLng("&H" & txtCJDMValue.Text)
            'Form1.SysmacCJ1.DM(Offset) = lData
            ret = m_PlcIf.WritePlcDM(Offset, lData)
            Return (ret)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���)  
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(DM_Write)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try
    End Function
#End Region

#Region "�f�[�^����(WR=�����ێ������[�G���A)(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�f�[�^����(WR=�����ێ������[�G���A)(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="Offset">(INP)�I�t�Z�b�g(10�i)</param>
    ''' <param name="lData"> (OUT)���̓f�[�^(16�i)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function W_Read(ByVal Offset As Long, ByRef lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �f�[�^����
            'txtCJDMValue.Text = Hex(SysmacCJ1.DM(CLng(txtCJDMOffset.Text)))
            'lData = Form1.SysmacCJ1.DM(Offset)
            'lData = Form1.SysmacCJ1.DM(Offset)

            ret = m_PlcIf.ReadPlcWR(Offset, lData)
            Return (ret)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(W_Read)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try
    End Function
#End Region

#Region "�f�[�^�o��(WR=�����ێ������[�G���A)(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�f�[�^�o��(WR=�����ێ������[�G���A)(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="Offset">(INP)�I�t�Z�b�g(10�i)</param>
    ''' <param name="lData"> (INP)�o�̓f�[�^(16�i)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function W_Write(ByVal Offset As Long, ByVal lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �f�[�^�o��
            'Form1.SysmacCJ1.DM(CLng(txtCJDMOffset.Text)) = CLng("&H" & txtCJDMValue.Text)
            'Form1.SysmacCJ1.DM(Offset) = lData
            ret = m_PlcIf.WritePlcWR(Offset, lData)
            Return (ret)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���)  
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(W_Write)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try
    End Function
#End Region

#Region "�f�[�^����(HR=�d���ێ������[�G���A)(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�f�[�^����(HR=�d���ێ������[�G���A)(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="Offset">(INP)�I�t�Z�b�g(10�i)</param>
    ''' <param name="lData"> (OUT)���̓f�[�^(16�i)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    ''' <remarks>###184</remarks>
    '''=========================================================================
    Public Function H_Read(ByVal Offset As Long, ByRef lData As Long) As Integer

        Dim ret As Integer

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �f�[�^����
            ret = m_PlcIf.ReadPlcHR(Offset, lData)
            Return (ret)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(H_Read)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26�� -----
#Region "Long�^�f�[�^�o��(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>Long�^�f�[�^�o��(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="Offset">(INP)�I�t�Z�b�g(10�i)</param>
    ''' <param name="lData"> (INP)�o�̓f�[�^(16�i)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function DM_Write2W(ByVal Offset As Long, ByVal lData As Long) As Integer

        Dim ret As Integer
        Dim wkLong As Long = 0

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �f�[�^�o��
            wkLong = (lData >> 16) And &HFFFF
            ret = m_PlcIf.WritePlcDM(Offset + 1, wkLong)
            wkLong = lData And &HFFFF
            ret = m_PlcIf.WritePlcDM(Offset, wkLong)
            Return (ret)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���)  
        Catch ex As Exception
            Dim strMSG As String
            strMSG = ex.Message + "(DM_Write2W)"
            MsgBox(strMSG)
            Return (cFRS_COM_ERR)                                       ' �ʐM�G���[(PLC)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26�� -----
#Region "�u�U�[OFF�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�u�U�[OFF�M�����o(�I�[�g���[�_�V���A���ʐM) ###073</summary>
    '''=========================================================================
    Public Sub W_BzOff()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                ' �u�U�[OFF
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_BUZER_OFF)

            Else
                ' SL436R��
                ' �u�U�[OFF
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_BUZER_OFF)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_BzOff)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�^�[�g�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�X�^�[�g�M�����o(�I�[�g���[�_�V���A���ʐM) ###073</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_START()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �X�^�[�g�M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_START)

            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_START)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_START)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�A���[�����Z�b�g�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�A���[�����Z�b�g�M�����o(�I�[�g���[�_�V���A���ʐM) ###073</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_RESET()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �A���[�����Z�b�g�M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_RESET)

            Else
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ARM_RESET)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_RESET)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�M�� -----
#Region "�A���[�����b�Z�[�W�\�����M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�A���[�����b�Z�[�W�\�����M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_ALM_DSP()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �A���[�����b�Z�[�W�\�����M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ALM_DSP)
            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_ALM_DSP)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_ALM_DSP)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�M�� -----
#Region "�n���h�㉺�z�[���ʒu�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�㉺�z�[���ʒu�M�����o(�I�[�g���[�_�V���A���ʐM)V2.0.0.0�E</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND_HOME()

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �A���[���������̓��[�_���Ńn���h�㉺�z�[���ʒu�Ɉړ����邽�߉��L���폜
            '' �n���h�㉺�z�[���ʒu�M�����o
            'If (giMachineKd = MACHINE_KD_RS) Then
            '    ' SL436S��
            '    ' �Y��BIT�݂̂�ύX���邽�߁A����̒l��Ǎ��� 
            '    Call m_PlcIf.ReadPlcWR(LOFS_W359S, lData)

            '    If ((lData And LROBS_HAND_HOME) = LROBS_HAND_HOME) Then
            '        ' ����ON���Ă���Ȃ�OFF���� 
            '        lData = lData And (Not LROBS_HAND_HOME)
            '    Else
            '        ' ����OFF���Ă���Ȃ�ON����
            '        lData = lData Or LROBS_HAND_HOME
            '    End If
            '    Call m_PlcIf.WritePlcWR(LOFS_W359S, lData)

            'Else
            '    ' SL436R��
            '    Return
            'End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND_HOME)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###144�� -----
#Region "�n���h�P�z���M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�P�z���M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND1_VACUME()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �n���h�P�z���M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND1_VACUME)
            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND1_VACUME)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND1_VACUME)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�n���h�P�z���j��M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�P�z���j��M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND1_ABSORB()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �n���h�P�z���j��M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND1_ABSORB)
            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND1_ABSORB)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND1_ABSORB)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�n���h�Q�z�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�Q�z�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND2_VACUME()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �n���h�Q�z���M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND2_VACUME)
            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND2_VACUME)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND2_VACUME)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�n���h�Q�z���j��M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�Q�z���j��M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_HAND2_ABSORB()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �n���h�Q�z���j��M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W52S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W52S, LDABS_HAND2_ABSORB)
            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND2_ABSORB)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND2_ABSORB)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�N�����v�n�m/�n�e�e�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�N�����v�n�m/�n�e�e�M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="OnOff">(INP)1=�N�����v�n�m(��), 0=�N�����v�n�e�e(�J) V1.16.0.0�D</param>
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
                'V4.11.0.0�K��
                r = Form1.System1.ClampCtrl(gSysPrm, OnOff, giTrimErr)         '###060
                If (r <> cFRS_NORMAL) Then

                End If

                '' SL436S��
                ''----- V1.16.0.0�D�� -----
                '' �N�����v��Ԏ擾
                'Call m_PlcIf.ReadPlcWR(LOFS_W42S, lData)

                '' �N�����v�n�m�M�����o
                'If (OnOff = 1) Then                                     ' �N�����v�n�m��
                '    ' �N�����v��OFF ?
                '    If ((lData And LDSTS_CLMP_X_ON) = 0) Then lwData = LDABS_CLMP_X
                '    If ((lData And LDSTS_CLMP_Y_ON) = 0) Then lwData = lwData Or LDABS_CLMP_Y
                '    If (lwData = 0) Then Return '                       ' �N�����vX,Y�Ƃ�ON�Ȃ�NOP

                'Else                                                    ' �N�����v�n�e�e��
                '    ' �N�����v��ON ?
                '    If ((lData And LDSTS_CLMP_X_ON) = LDSTS_CLMP_X_ON) Then lwData = LDABS_CLMP_X
                '    If ((lData And LDSTS_CLMP_Y_ON) = LDSTS_CLMP_Y_ON) Then lwData = lwData Or LDABS_CLMP_Y
                '    If (lwData = 0) Then Return '                       '  �N�����vX,Y�Ƃ�OFF�Ȃ�NOP
                'End If

                '' �N�����v�n�m/�n�e�e�M�����o
                'Call m_PlcIf.WritePlcWR(LOFS_W53S, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W53S, lwData)
                'V4.11.0.0�K��

            Else
                ' SL436R��
                ''V5.0.0.8�E��
                r = Form1.System1.ClampCtrl(gSysPrm, OnOff, giTrimErr)         '###060
                If (r <> cFRS_NORMAL) Then

                End If
                ''V5.0.0.8�E��        ���R�����g

                ''----- V1.16.0.0�D�� -----
                '' �N�����v��Ԏ擾
                'Call m_PlcIf.ReadPlcWR(LOFS_W44, lData)

                '' �N�����v�n�m�M�����o
                'If (OnOff = 1) Then                                     ' �N�����v�n�m��
                '    ' �N�����v��OFF ?
                '    If ((lData And LDST_CLMP_X_ON) = 0) Then lwData = LDAB_CLMP_X
                '    If ((lData And LDST_CLMP_Y_ON) = 0) Then lwData = lwData Or LDAB_CLMP_Y
                '    If (lwData = 0) Then Return '                       ' �N�����vX,Y�Ƃ�ON�Ȃ�NOP

                'Else                                                    ' �N�����v�n�e�e��
                '    ' �N�����v��ON ?
                '    If ((lData And LDST_CLMP_X_ON) = LDST_CLMP_X_ON) Then lwData = LDAB_CLMP_X
                '    If ((lData And LDST_CLMP_Y_ON) = LDST_CLMP_Y_ON) Then lwData = lwData Or LDAB_CLMP_Y
                '    If (lwData = 0) Then Return '                       '  �N�����vX,Y�Ƃ�OFF�Ȃ�NOP
                'End If

                '' �N�����v�n�m/�n�e�e�M�����o
                'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W54, lwData)

                ''V5.0.0.8�E��

                ' �N�����v�n�m/�n�e�e�M�����o
                'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_CLMP_X + LDAB_CLMP_Y)
                '----- V1.16.0.0�D�� -----
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_CLMP_ONOFF)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�z���n�m/�n�e�e�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�z���n�m/�n�e�e�M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="OnOff">(INP)1=�n�m(��), 0=�n�e�e(�J) V1.18.0.0�N</param>
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
                ' SL436S��
                '----- V6.0.3.0_37�� -----
                If gVacuumIO = 1 Then
                    'V4.11.0.0�L��
                    ZABSVACCUME(OnOff)

                Else
                    '' �z����Ԏ擾
                    Call m_PlcIf.ReadPlcWR(LOFS_W42S, lData)

                    ' �z���n�m�M�����o
                    If (OnOff = 1) Then                                     ' �z���n�m��
                        ' �z����ON ?
                        If ((lData And LDSTS_VACUME) = LDSTS_VACUME) Then
                            Return
                        End If

                        ' �z���n�e�e�M�����o
                    Else                                                    ' �z���n�e�e��
                        ' �z����OFF ?
                        ''V4.1.0.0�P�@��
                        'If ((lData And LDSTS_VACUME) = 0) Then
                        '    Return
                        'End If
                        'V4.1.0.0�P�@��
                    End If

                    ' �z���n�m/�n�e�e�M�����o
                    Call m_PlcIf.WritePlcWR(LOFS_W53S, 0)
                    Call m_PlcIf.WritePlcWR(LOFS_W53S, LDABS_VACUME)

                End If
                '----- V6.0.3.0_37�� -----

            Else
                ' SL436R��
                '----- V1.18.0.0�N�� -----
                ' �z����Ԏ擾
          '      Call m_PlcIf.ReadPlcWR(LOFS_W44, lData)

                'V6.0.5.0�E��
                ' �z���n�m�M�����o
                If (OnOff = 1) Then                                     ' �z���n�m��
                    'V4.12.2.0�I��
                    Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��OFF)
                    '' �z����ON ?
                    'If ((lData And LDST_VACUME) = LDST_VACUME) Then
                    '    Return
                    'End If

                    ' �z���n�e�e�M�����o
                Else                                                    ' �z���n�e�e��
                    Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��OFF)

                    '' �z����OFF ?
                    'If ((lData And LDST_VACUME) = 0) Then
                    '    Return
                    'End If
                    'V4.12.2.0�I��

                End If
                '----- V1.18.0.0�N�� -----

                'V4.12.2.0�I��
                '' �z���n�m/�n�e�e�M�����o
                'Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                'Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_VACUME)
                'V4.12.2.0�I�� 
                'V6.0.5.0�E��

            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_VACUME_ONOFF)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�z���j��M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�z���j��M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub W_ABSORB()

        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            ' �z���j��M�����o
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                Call m_PlcIf.WritePlcWR(LOFS_W53S, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53S, LDABS_ABSORB)
            Else
                ' SL436R��
                Call m_PlcIf.WritePlcWR(LOFS_W54, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W54, LDAB_ABSORB)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_ABSORB)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###144�� -----
#Region "�}�K�W���㉺����" '###182
    '''=========================================================================
    ''' <summary>�}�K�W���㉺����</summary>
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
                '   SL436S��
                '---------------------------------------------------------------
                '�Y��BIT�݂̂�ύX���邽�߁A����̒l��Ǎ��� 
                Call m_PlcIf.ReadPlcWR(LOFS_W52S, SerialReadData)

                If (Mode = MG_UP) Then                                  '�}�K�W���㏸
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
                Else                                                    '�}�K�W�����~
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
                '   SL436R��
                '---------------------------------------------------------------
                '�Y��BIT�݂̂�ύX���邽�߁A����̒l��Ǎ��� 
                Call m_PlcIf.ReadPlcWR(LOFS_W52, SerialReadData)

                If (Mode = MG_UP) Then                                  '�}�K�W���㏸
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
                Else                                                    '�}�K�W�����~
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

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strmsg = "LoaderIOFor436.MGMoveJog() TRAP ERROR = " + ex.Message
            MsgBox(strmsg)
        End Try
    End Sub
#End Region

#Region "�}�K�W���㉺�����~" '###182
    '''=========================================================================
    ''' <summary>�}�K�W���㉺�����~</summary>
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
            ' SL436S��
            ' �Y��BIT�݂̂�ύX���邽�߁A����̒l��Ǎ��� 
            Call m_PlcIf.ReadPlcWR(LOFS_W52S, SerialReadData)

            OutData = SerialReadData And Not OutSMG1Move_Sw
            OutData = OutData And Not OutMG2Move_Sw

            Call m_PlcIf.WritePlcWR(LOFS_W52S, OutData)

        Else
            ' SL436R��
            '�Y��BIT�݂̂�ύX���邽�߁A����̒l��Ǎ��� 
            Call m_PlcIf.ReadPlcWR(LOFS_W52, SerialReadData)

            OutData = SerialReadData And Not OutMG1Move_Sw
            OutData = OutData And Not OutMG2Move_Sw
            OutData = OutData And Not OutMG3Move_Sw
            OutData = OutData And Not OutMG4Move_Sw

            Call m_PlcIf.WritePlcWR(LOFS_W52, OutData)
        End If

    End Sub
#End Region
    '----- ###188�� -----
#Region "�n���h�P�㏸(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�P�㏸(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks>�����n���h���������Ă���ꍇ�ɏ㏸������</remarks>
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
                ' SL436S��
                ' �n���h�P���オ���Ă���Ȃ�NOP
                Call m_PlcIf.ReadPlcWR(LOFS_W359S, lData)
                If ((lData And LROBS_HAND_HOME) = LROBS_HAND_HOME) Then Exit Sub

                ' ���[�_�蓮���[�h�ؑւ�
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ���[�_�o��(ON=�Ȃ�, OFF=����)

                ' �n���h�P�㏸�M�����o
                lData = lData Or LROBS_HAND_HOME
                Call m_PlcIf.WritePlcWR(LOFS_W359S, lData)

            Else
                ' SL436R��
                ' �n���h�P���オ���Ă���Ȃ�NOP
                Call m_PlcIf.ReadPlcWR(43, lData)
                If ((lData And LDAB_HAND1_ZMOVE) = 0) Then Exit Sub

                ' ���[�_�蓮���[�h�ؑւ� '###222
                Call SetLoaderIO(&H0, LOUT_AUTO)                            ' ���[�_�o��(ON=�Ȃ�, OFF=����)

                ' �n���h�P�㏸�M�����o
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND1_DOWN)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND1_UP)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�n���h�Q�㏸(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�Q�㏸(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks>���[�n���h���������Ă���ꍇ�ɏ㏸������</remarks>
    '''=========================================================================
    Public Sub W_HAND2_UP()

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                ' �n���h�Q���オ���Ă���Ȃ�NOP
                Call m_PlcIf.ReadPlcWR(LOFS_W359S, lData)
                If ((lData And LROBS_HAND_HOME) = LROBS_HAND_HOME) Then Exit Sub

                ' ���[�_�蓮���[�h�ؑւ�
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ���[�_�o��(ON=�Ȃ�, OFF=����)

                ' �n���h�Q�㏸�M�����o
                lData = lData Or LROBS_HAND_HOME
                Call m_PlcIf.WritePlcWR(LOFS_W359S, lData)

            Else
                ' SL436R��
                ' �n���h�Q���オ���Ă���Ȃ�NOP
                Call m_PlcIf.ReadPlcWR(43, lData)
                If ((lData And LDAB_HAND2_ZMOVE) = 0) Then Exit Sub

                ' ���[�_�蓮���[�h�ؑւ� '###222
                Call SetLoaderIO(&H0, LOUT_AUTO)                        ' ���[�_�o��(ON=�Ȃ�, OFF=����)

                ' �n���h�Q�㏸�M�����o
                Call m_PlcIf.WritePlcWR(LOFS_W53, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W53, LDAB_HAND2_DOWN)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_HAND2_UP)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###188�� -----
    '----- ###197�� -----
#Region "�n���h�ړ�(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�n���h�ړ�(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <remarks>�����n���h���������Ă���ꍇ�ɏ㏸������</remarks>
    '''=========================================================================
    Public Sub W_HAND_STAGE()

        Dim lData As Long = 0
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Exit Sub
#End If
            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
                ' SL436S��
                ' �n���h���ڈʒu�ւ̈ړ��I�� 
                Call m_PlcIf.WritePlcWR(LOFS_W339, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W339, LROB_HAND_STAGE)

                ' �n���h���ڈʒu�ֈړ�
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W128, POSMOVE_START_ADR)   ' �ړ��J�nBit��ON
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)                   ' �ړ��J�nBit��OFF 

            Else
                ' SL436R��
                ' �n���h���ڈʒu�ւ̈ړ��I�� 
                Call m_PlcIf.WritePlcWR(LOFS_W339, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W339, LROB_HAND_STAGE)

                ' �n���h���ڈʒu�ֈړ�
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W128, POSMOVE_START_ADR)   ' �ړ��J�nBit��ON
                Call m_PlcIf.WritePlcWR(LOFS_W128, 0)                   ' �ړ��J�nBit��OFF 
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(HAND_STAGE)"
            'MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###197�� -----
    '----- V1.23.0.0�I�� -----
#Region "�����^�]���f�n�m/�n�e�e�M�����o(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�����^�]���f�n�m/�n�e�e�M�����o(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <returns>cFRS_NORMAL   = ����
    '''          cFRS_ERR_LDR  = ���[�_�A���[�����o
    '''          cFRS_ERR_LDR1 = ���[�_�A���[�����o(�S��~�ُ�)
    '''          cFRS_ERR_LDR2 = ���[�_�A���[�����o(�T�C�N����~)
    '''          cFRS_ERR_LDR3 = ���[�_�A���[�����o(�y�̏�(���s�\))
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
            '----- V4.3.0.0�D�� -----
            ' SL436R���ő��z�ДňȑO(���[���a, KAMAYA�a)��PLC�������Ή��Ȃ̂Ń^�C���A�E�g�ƂȂ�B
            ' PLC����Version UP����K�v����B
            '----- V4.3.0.0�D�� -----

            ' �����^�]���fON(PC �� PLC)�M�����o
            Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W109, LDDV_STP_AUTODRIVE)

            ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRS_Tick��X msec�Ԋu�Ŏ��s����)
            Sub_SetTimeoutTimer(TimerRS)

            ' ���[�_�̎����^�]���fON(PC �� PLC)�M����҂�
            lData = 0
            Do
                ' ���[�_����̎����^�]���fON(PLC �� PC)�M����҂�
                r = W_Read(LOFS_W110, lData)
                If (r <> 0) Then
                    r = cFRS_ERR_LDRTO
                    GoTo STP_END
                End If

                ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N 
                If (bFgTimeOut = True) Then                             ' �^�C���A�E�g ?
                    ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                    TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                    rtnCode = cFRS_ERR_LDRTO                            ' Return�l = ���[�_�ʐM�^�C���A�E�g
                    GoTo STP_END
                End If

                System.Windows.Forms.Application.DoEvents()
                Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
            Loop While ((lData And LDPL_STP_AUTODRIVE) <> LDPL_STP_AUTODRIVE)

            ' �����^�]���fOFF(PC �� PLC)�M�����o
            Call m_PlcIf.WritePlcWR(LOFS_W109, 0)
            Call m_PlcIf.WritePlcWR(LOFS_W109, 0)

            ' �I������
STP_END:
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            If (IsNothing(TimerRS) = False) Then
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                TimerRS.Dispose()                                       ' �^�C�}�[��j������
            End If

            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Send_AutoStopToLoader() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V1.23.0.0�I�� -----
    '----- V4.0.0.0�R�� -----
#Region "�T�C�N����~�M���𑗏o���ă��[�_����̃T�C�N����~������҂�(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�T�C�N����~�v���M���𑗏o���ă��[�_����̃T�C�N����~������҂�</summary>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N�g</param>
    ''' <param name="OnOff"> (INP)1=�T�C�N����~�M��ON���o, 0=�T�C�N����~�M��OFF���o</param>
    ''' <returns>cFRS_NORMAL    = ����(����葱�s)
    '''          cFRS_ERR_START = ����(��Ȃ����s)
    '''          cFRS_ERR_RST   = Cancel(RESET�L�[����)
    '''          cFRS_ERR_LDR0  = ���[�_�ʐM�^�C���A�E�g��</returns>
    ''' <remarks>���[���a����(SL436R/SL436S)</remarks>
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
            '   ��������
            '-------------------------------------------------------------------
            ' ���[�_�����܂��̓��[�_�蓮���[�h�Ȃ�NOP
            If ((bFgActLink = False) Or (bFgAutoMode = False)) Then
                Return (cFRS_NORMAL)
            End If

RETRY_CYCLE:  ''V4.0.0.0-85
            rtnCode = cFRS_NORMAL
            '-------------------------------------------------------------------
            '   �T�C�N����~ON/OFF�M�����M(SL436S/SL436R)
            '-------------------------------------------------------------------
            ' �^�C�}�[�l��ݒ肷��
            If (giOPLDTimeOutFlg = 0) Then                              ' ���[�_�ʐM�^�C���A�E�g���o���� ?
                TimeVal = System.Threading.Timeout.Infinite             ' �^�C�}�[�l = �Ȃ�
            Else
                TimeVal = giOPLDTimeOut                                 ' �^�C�}�[�l = ���[�_�ʐM�^�C���A�E�g����(msec)
            End If

            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S��
                If (OnOff = 1) Then                                     ' �T�C�N����~�v��BIT ON ?
                    ' �T�C�N����~�v��BIT ON
                    Call SetLoaderIO(LOUT_CYCL_STOP, &H0)               ' ���[�_�o��(ON=�T�C�N����~�v��, OFF=�Ȃ�)
                Else
                    ' �T�C�N����~�v��BIT OFF
                    Call SetLoaderIO(&H0, LOUT_CYCL_STOP)               ' ���[�_�o��(ON=�Ȃ�, OFF=�T�C�N����~�v��)
                End If

                ' �T�C�N����~OFF�Ȃ牞���҂����Ȃ�
                If (OnOff = 0) Then
                    Return (cFRS_NORMAL)
                End If

            Else                                                        ' SL436R��
                ' ���oBIT��ݒ肷��
                If (OnOff = 1) Then                                     ' �T�C�N����~ON�M�����M ?
                    ' �T�C�N����~BIT ON
                    lData = LPC_CYCL_STOP
                Else                                                    ' �T�C�N����~OFF�M�����M
                    ' �T�C�N����~OFF
                    lData = 0
                End If

                ' �T�C�N����~ON/OFF�M�����M
                Call m_PlcIf.WritePlcWR(LOFS_W113, 0)
                Call m_PlcIf.WritePlcWR(LOFS_W113, lData)
                Console.WriteLine("W_CycleStop() WritePlcWR =" + lData.ToString("0")) ' For Debug
                lData = 0

                ' �T�C�N����~OFF�M�����M�Ȃ牞���҂����Ȃ�
                If (OnOff = 0) Then
                    Return (cFRS_NORMAL)
                End If
            End If

            '-------------------------------------------------------------------
            '   �^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬
            '   (TimerLock_Tick��X msec�Ԋu�Ŏ��s����)
            '-------------------------------------------------------------------
            TimerTM_Create(TimerLock, TimeVal)

            '-------------------------------------------------------------------
            '   �T�C�N����~�����҂�(SL436S/SL436R)
            '-------------------------------------------------------------------
            Do
                System.Threading.Thread.Sleep(10)                       ' Wait(ms)
                System.Windows.Forms.Application.DoEvents()

                ' �T�C�N����~�����҂�(SL436S/SL436R)
                If (giMachineKd = MACHINE_KD_RS) Then                   ' SL436S ? 
                    ' ���[�_�A���[��/����~�`�F�b�N
                    r = GetLoaderIO(LdIn)                               ' ���[�_I/O����
                    If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                        rtnCode = cFRS_ERR_LDR                          ' Return�l = ���[�_�A���[�����o
                        GoTo STP_ERR_LDR                                ' ���[�_�A���[���\����
                    End If
                    ' �T�C�N����~�������`�F�b�N����(SL436S��)
                    If ((LdIn And LIN_CYCL_STOP) = LIN_CYCL_STOP) Then
                        Exit Do                                         ' �T�C�N����~�����Ȃ�Exit
                    End If

                Else
                    ' �T�C�N����~ON/OFF�������`�F�b�N����(SL436R��)
                    Call m_PlcIf.ReadPlcWR(LOFS_W114, lData)            ' �T�C�N����~�����擾
                    Console.WriteLine("W_CycleStop() ReadPlcWR =" + lData.ToString("0")) ' For Debug
                    If (lData And LPLC_CYCL_STOP) Then
                        Exit Do                                         ' �T�C�N����~�����Ȃ�Exit
                    End If
                End If

                '-------------------------------------------------------------------
                '   �^�C���A�E�g�`�F�b�N 
                '-------------------------------------------------------------------
                bTOut = TimerTM_Sts()
                If (bTOut = True) Then                                  ' �^�C���A�E�g ? 
                    rtnCode = cFRS_ERR_LDRTO                            ' Retuen�l = ���[�_�ʐM�^�C���A�E�g 
                    Exit Do
                End If

            Loop While (1)

            '-------------------------------------------------------------------
            '   �㏈��
            '-------------------------------------------------------------------
STP_END:
            TimerTM_Stop(TimerLock)                                     ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerTM_Dispose(TimerLock)                                  ' �^�C�}�[��j������

            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ���[�_�ʐM�^�C���A�E�g ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' �G���[���b�Z�[�W�\��
            End If

            Return (rtnCode)

            ' ���[�_�G���[������
STP_ERR_LDR:
            TimerTM_Stop(TimerLock)                                     ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerTM_Dispose(TimerLock)                                  ' �^�C�}�[��j������
            If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ���[�_�ʐM�^�C���A�E�g ?
                rtnCode = Sub_CallFrmRset(ObjSys, cGMODE_LDR_TMOUT)     ' �G���[���b�Z�[�W�\��
            Else
                ' ���[�_�A���[�����b�Z�[�W�쐬 & ���[�_�A���[����ʕ\��
                rtnCode = Loader_AlarmCheck(ObjSys, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                'V4.0.0.0-85�@��
                If (rtnCode = cFRS_ERR_LDR3) Or (rtnCode = cFRS_ERR_LDR2) Then ' ###196
                    Call W_RESET()                                      ' �A���[�����Z�b�g�M�����o 
                    Call W_START()                                      ' �X�^�[�g�M�����o 
                    GoTo RETRY_CYCLE
                End If
                'V4.0.0.0-85�@��
            End If
            Return (rtnCode)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.W_CycleStop() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            rtnCode = cERR_TRAP
            GoTo STP_END
        End Try
    End Function
#End Region

#Region "�����`�F�b�N(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�����`�F�b�N(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <returns>cFRS_NORMAL    = ����(�����Ȃ�)
    '''          cFRS_ERR_RST   = �������o
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
            ' �����`�F�b�N
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ? 
                r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
                System.Threading.Thread.Sleep(500)                      ' 500msec�҂��čēx���[�h����
                r = W_Read(LOFS_W42S, lData)                            ' �������͏�Ԏ擾(W42.00-W42.15)
                If ((lData And LDSTS_BREAK_X_ON) = 0) And ((lData And LDSTS_BREAK_Y_ON) = 0) Then
                    Return (cFRS_NORMAL)                                ' �������o�łȂ���ΐ��탊�^�[��
                End If
                Return (cFRS_ERR_RST)

            Else                                                        ' SL436R��
                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                System.Threading.Thread.Sleep(500)                      ' 500msec�҂��čēx���[�h����
                r = W_Read(LOFS_W44, lData)                             ' �������͏�Ԏ擾(W44.00-W44.15)
                If ((lData And LDST_BREAK_X_ON) = 0) And ((lData And LDST_BREAK_Y_ON) = 0) Then
                    Return (cFRS_NORMAL)                                ' �������o�łȂ���ΐ��탊�^�[��
                End If
                Return (cFRS_ERR_RST)
            End If

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(R_BREAK_XY)"
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V4.0.0.0�R�� -----
    '----- V4.0.0.0-26�� -----
#Region "�i��ԍ��𑗏o����(�I�[�g���[�_�V���A���ʐM)"
    '''=========================================================================
    ''' <summary>�i��ԍ��𑗏o����(�I�[�g���[�_�V���A���ʐM)</summary>
    ''' <param name="OnOff">0=�i��ԍ�������, 1=�i��ԍ��𑗏o����</param>
    ''' <returns>cFRS_NORMAL    = ����
    '''          ��L�ȊO       = �G���[
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
            ' ��i��ԍ��𑗏o����(SL436S/SL436R)
            If (giMachineKd <> MACHINE_KD_RS) Then                                  ' SL436R ?
                ' ��SL436R��

            Else
                ' ��SL436S��
                ' ��i��ԍ��Ɗ�^�C�v��ݒ肷��
                If (OnOff = 1) Then                                                 ' �i��ԍ��𑗏o ?
                    lData = typPlateInfo.intWorkSetByLoader                         ' ��i��ԍ�(1-10)
                    lTpData = glSubstrateType(typPlateInfo.intWorkSetByLoader - 1)  ' ����Ή�(0=�ʏ�, 1=�����(�X���[�z�[��))
                Else
                    lData = 0                                                       ' ��i��ԍ�������
                    lTpData = 0                                                     ' ����Ή�(0=�ʏ�)
                End If

                ' ��i��ԍ��𑗏o����
                r = DM_Write(LOFS_D241S, lData)                                     ' �i��I��(D241 1(�i��1)�`10(�i��10))
                ' ��^�C�v�𑗏o����
                r = DM_Write(LOFS_D230S, lTpData)                                   ' ���݂̊���(D230 0=�ʏ�, 1=�����)
            End If

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������(�G���[���b�Z�[�W��ex.Message�ɐݒ肳���) 
        Catch ex As Exception
            strMSG = ex.Message + "(W_SubstrateType)"
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V4.0.0.0-26�� -----
    '===============================================================================
    '   �V�X�e���p�����[�^�h�n����
    '===============================================================================
#Region "�V�X�p����胍�[�_�p�e��ݒ�l��ݒ肷��"
    '''=========================================================================
    ''' <summary>�V�X�p����胍�[�_�p�e��ݒ�l��ݒ肷�� V4.0.0.0-26</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
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
            '   �����f�[�^�����[�_���ɑ��M����(SL436S)
            '-------------------------------------------------------------------
            If (giMachineKd <> MACHINE_KD_RS) Then                      ' SL436S�łȂ����NOP
                Return (cFRS_NORMAL)
            End If

            ' �i��I��������������
            lData = 0
            r = DM_Write(LOFS_D241S, lData)                             ' �i��I��������(D241 1(�i��1)�`10(�i��10))
            r = DM_Write(LOFS_W360S, lData)                             ' �i��I��BIT������(W360.00-W360.15)

            ' ���ʂ�����������
            lData = 0
            r = DM_Write(LOFS_D230S, lData)                             ' ���݂̊���(D230 0=�ʏ�, 1=�����)

            ' �񖇎��Z���T�m�F�ʒu��ݒ肷��
            For Idx = 0 To (MAXWORK_KND - 1)
                lData = gfTwoSubPickChkPos(Idx)                         ' �񖇎��Z���T�m�F�ʒu���W(mm)
                r = DM_Write2W(LOFS_D700S + Idx * 2, lData)             ' ���W�f�[�^1-10(D700-D718 �����t2WORD)
            Next Idx

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_PutParameterFromSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�V�X�e���p�����[�^���烍�[�_���^�C�}�[�l����������"
    '''=========================================================================
    ''' <summary>�V�X�e���p�����[�^���烍�[�_���^�C�}�[�l����������</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_PutTimerValFromSysparam() As Integer

        'Dim r As Integer
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �^�C�}�[�l���V�X�e���p�����[�^����Ǎ���
            'r = Loader_GetTimerValFromSysparam(LTimerDT())

            ' ���[�_���^�C�}�[�l���������ށ@
            ' ��������ǉ�����


            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_PutTimerValFromSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�V�X�e���p�����[�^���烍�[�_���^�C�}�[�l��Ǎ���"
    '''=========================================================================
    ''' <summary>�V�X�e���p�����[�^���烍�[�_���^�C�}�[�l��Ǎ���</summary>
    ''' <param name="LTimerDT">(OUT)�^�C�}�[�l�Ǎ��݈�</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_GetTimerValFromSysparam(ByRef LTimerDT() As UShort) As Integer

        Dim sPath As String                                                     ' �t�@�C����
        Dim sSect As String                                                     ' �Z�N�V������
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' �^�C�}�[�l���V�X�e���p�����[�^����Ǎ��ށ@�@�����ȉ��C������
            sPath = SysParamPath                                                ' �t�@�C����
            sSect = "LOADER_TIMER"                                              ' �Z�N�V������
            LTimerDT(0) = GetPrivateProfileInt(sSect, "D4032", 2, sPath)        ' �������t�����n���h1��ډ��~�@���@�z��OFF
            LTimerDT(1) = GetPrivateProfileInt(sSect, "D4033", 5, sPath)        ' �������t�����n���h�z��OFF�@���@�ڕ���N�����v
            LTimerDT(2) = GetPrivateProfileInt(sSect, "D4034", 2, sPath)        ' �������t�ڕ���N�����v�@���@�����n���h�㏸
            LTimerDT(3) = GetPrivateProfileInt(sSect, "D4035", 2, sPath)        ' �������t�����n���h�㏸�@���@�����n���h2��ډ��~
            LTimerDT(4) = GetPrivateProfileInt(sSect, "D4036", 2, sPath)        ' �������t�����n���h2��ډ��~�@���@�ڕ���N�����v����
            LTimerDT(5) = GetPrivateProfileInt(sSect, "D4037", 2, sPath)        ' �������t�ڕ���N�����v�J�@���@�ʏ퉺�~
            LTimerDT(6) = GetPrivateProfileInt(sSect, "D4038", 0, sPath)        ' �����n���h�ʏ퉺�~�@���@�t���O�����y�ыz��OFF
            LTimerDT(7) = GetPrivateProfileInt(sSect, "D4039", 2, sPath)        ' �����n���h�ʏ�z��OFF�@���@�����n���h�㏸
            LTimerDT(8) = GetPrivateProfileInt(sSect, "D4040", 10, sPath)       ' ����ON����A�������t�����̋����j��
            LTimerDT(9) = GetPrivateProfileInt(sSect, "D4041", 1, sPath)        ' �����n���h�^��j�󎞊�
            LTimerDT(10) = GetPrivateProfileInt(sSect, "D4042", 10, sPath)      ' �����n���h�^��j�󎞊ԁiX2D�j
            LTimerDT(11) = GetPrivateProfileInt(sSect, "D4045", 3, sPath)       ' �ڕ���N�����vON�@���@�z��ON
            LTimerDT(12) = GetPrivateProfileInt(sSect, "D4046", 1, sPath)       ' �ڕ���z��ON�@���@�����^2�����̈ȏ㌟�o�҂�
            LTimerDT(13) = GetPrivateProfileInt(sSect, "D4049", 2, sPath)       ' ���[�n���h���~�@���@�ڕ���N�����v����
            LTimerDT(14) = GetPrivateProfileInt(sSect, "D4050", 1, sPath)       ' �ڕ���N�����v�����@���@�z��OFF
            LTimerDT(15) = GetPrivateProfileInt(sSect, "D4051", 2, sPath)       ' �ڕ���z��OFF�@���@���[�n���h�㏸
            LTimerDT(16) = GetPrivateProfileInt(sSect, "D4052", 1, sPath)       ' �ڕ���N�����v�����@���@���[�n���h�㏸
            LTimerDT(17) = GetPrivateProfileInt(sSect, "D4053", 5, sPath)       ' �����}�K�W���G�A�u���[����
            LTimerDT(18) = GetPrivateProfileInt(sSect, "D4055", 10, sPath)      ' �����n���h����@���@�z���~�X�ُ̈팟�o�҂�
            LTimerDT(19) = GetPrivateProfileInt(sSect, "D4056", 10, sPath)      ' �ڕ���z��ON�@���@�z���~�X�ُ̈팟�o�҂�
            LTimerDT(20) = GetPrivateProfileInt(sSect, "D4057", 10, sPath)      ' ���[�n���h����@���@�z���~�X�ُ̈팟�o�҂�
            LTimerDT(21) = GetPrivateProfileInt(sSect, "D4058", 5, sPath)       ' ���[�G���x�[�^���~����

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_GetTimerValFromSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���[�_���^�C�}�[�l���V�X�e���p�����[�^�ɏ�����"
    '''=========================================================================
    ''' <summary>���[�_���^�C�}�[�l���V�X�e���p�����[�^�ɏ�����</summary>
    ''' <param name="LTimerDT">(INP)�^�C�}�[�l�̔z��</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Loader_PutTimerValToSysparam(ByRef LTimerDT() As UShort) As Integer

        Dim sPath As String                                                     ' �t�@�C����
        Dim sSect As String                                                     ' �Z�N�V������
        Dim s As String
        Dim strMSG As String

        Try
#If cPLCcOFFLINEcDEBUG Then
            Return (cFRS_NORMAL)
#End If
            ' ���[�_���^�C�}�[�l���V�X�e���p�����[�^�ɏ�����
            sPath = SysParamPath                                                ' �t�@�C����
            sSect = "LOADER_TIMER"                                              ' �Z�N�V������

            s = CStr(LTimerDT(0))
            Call WritePrivateProfileString(sSect, "D4032", s, sPath)            ' �������t�����n���h1��ډ��~�@���@�z��OFF
            s = CStr(LTimerDT(1))
            Call WritePrivateProfileString(sSect, "D4033", s, sPath)            ' �������t�����n���h�z��OFF�@���@�ڕ���N�����v
            s = CStr(LTimerDT(2))
            Call WritePrivateProfileString(sSect, "D4034", s, sPath)            ' �������t�ڕ���N�����v�@���@�����n���h�㏸
            s = CStr(LTimerDT(3))
            Call WritePrivateProfileString(sSect, "D4035", s, sPath)            ' �������t�����n���h�㏸�@���@�����n���h2��ډ��~
            s = CStr(LTimerDT(4))
            Call WritePrivateProfileString(sSect, "D4036", s, sPath)            ' �������t�����n���h2��ډ��~�@���@�ڕ���N�����v����
            s = CStr(LTimerDT(5))
            Call WritePrivateProfileString(sSect, "D4037", s, sPath)            ' �������t�ڕ���N�����v�J�@���@�ʏ퉺�~
            s = CStr(LTimerDT(6))
            Call WritePrivateProfileString(sSect, "D4038", s, sPath)            ' �����n���h�ʏ퉺�~�@���@�t���O�����y�ыz��OFF
            s = CStr(LTimerDT(7))
            Call WritePrivateProfileString(sSect, "D4039", s, sPath)            ' �����n���h�ʏ�z��OFF�@���@�����n���h�㏸
            s = CStr(LTimerDT(8))
            Call WritePrivateProfileString(sSect, "D4040", s, sPath)            ' ����ON����A�������t�����̋����j��
            s = CStr(LTimerDT(9))
            Call WritePrivateProfileString(sSect, "D4041", s, sPath)            ' �����n���h�^��j�󎞊�
            s = CStr(LTimerDT(10))
            Call WritePrivateProfileString(sSect, "D4042", s, sPath)            ' �����n���h�^��j�󎞊ԁiX2D�j
            s = CStr(LTimerDT(11))
            Call WritePrivateProfileString(sSect, "D4045", s, sPath)            ' �ڕ���N�����vON�@���@�z��ON
            s = CStr(LTimerDT(12))
            Call WritePrivateProfileString(sSect, "D4046", s, sPath)            ' �ڕ���z��ON�@���@�����^2�����̈ȏ㌟�o�҂�
            s = CStr(LTimerDT(13))
            Call WritePrivateProfileString(sSect, "D4049", s, sPath)            ' ���[�n���h���~�@���@�ڕ���N�����v����
            s = CStr(LTimerDT(14))
            Call WritePrivateProfileString(sSect, "D4050", s, sPath)            ' �ڕ���N�����v�����@���@�z��OFF
            s = CStr(LTimerDT(15))
            Call WritePrivateProfileString(sSect, "D4051", s, sPath)            ' �ڕ���z��OFF�@���@���[�n���h�㏸
            s = CStr(LTimerDT(16))
            Call WritePrivateProfileString(sSect, "D4052", s, sPath)            ' �ڕ���N�����v�����@���@���[�n���h�㏸
            s = CStr(LTimerDT(17))
            Call WritePrivateProfileString(sSect, "D4053", s, sPath)            ' �����}�K�W���G�A�u���[����
            s = CStr(LTimerDT(18))
            Call WritePrivateProfileString(sSect, "D4055", s, sPath)            ' �����n���h����@���@�z���~�X�ُ̈팟�o�҂�
            s = CStr(LTimerDT(19))
            Call WritePrivateProfileString(sSect, "D4056", s, sPath)            ' �ڕ���z��ON�@���@�z���~�X�ُ̈팟�o�҂�
            s = CStr(LTimerDT(20))
            Call WritePrivateProfileString(sSect, "D4057", s, sPath)            ' ���[�n���h����@���@�z���~�X�ُ̈팟�o�҂�
            s = CStr(LTimerDT(21))
            Call WritePrivateProfileString(sSect, "D4058", s, sPath)            ' ���[�G���x�[�^���~����

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Loader_PutTimerValToSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "���̑��̃V�X�p����Ǎ���"
    '''=========================================================================
    ''' <summary>���̑��̃V�X�p����Ǎ���</summary>
    ''' <param name="sPath">(INP)�t�@�C����</param>
    ''' <param name="sSect">(INP)�Z�N�V������</param>
    ''' <param name="sKey"> (INP)�L�[��</param>
    ''' <param name="Data"> (OUT)�f�[�^</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function Get_SystemParameterShort(ByRef sPath As String, ByRef sSect As String, ByRef sKey As String, ByRef Data As Short) As Integer

        Dim strMSG As String

        Try
            Data = GetPrivateProfileInt(sSect, sKey, 0, sPath)
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "LoaderIOFor436.Get_SystemParameterShort() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    ''' <summary>
    ''' PLC�̃��[�o�b�e���[�A���[���Ď�
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CheckPLCLowBatteryAlarm()
        Dim r As Integer
        Dim lData As Long

        If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0�E
            ' SL436S��
            r = DM_Read(LOFS_D242S, lData)
            If (lData <> 0) Then
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH50, MSG_SPRASH51, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            End If
        End If

    End Sub

    'V4.9.0.0�@��
    ''' <summary>
    ''' Lot���f����BIT��ON/OFF����    ' W113.02��ON/OFF����
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
    'V4.9.0.0�@��

    ''V4.11.0.0�G��
    ''' <summary>
    ''' Lot���f���������v��BIT��ON/OFF����    ' W113.03��ON/OFF����
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
    ''' LotStopReady�M����҂�
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
            ' ���[�_�A���[��/����~�`�F�b�N
            GetLoaderIO(LdIn)                                   ' ���[�_
            If ((LdIn And LINP_NO_ALM_RESTART) <> LINP_NO_ALM_RESTART) Then
                rtnCode = cFRS_ERR_LDR                              ' Return�l = ���[�_�A���[�����o
                GoTo STP_ERR_LDR 'V5.0.0.1�F
            End If

            Call m_PlcIf.ReadPlcWR(LOFS_W114S, lData)               ' Lot��~�v�������擾
            If (lData And LPLCS_SET_LOTSTOPREADY) Then
                rtnCode = cFRS_NORMAL
                Exit Do
            End If

            ' ���[�_�ʐM�^�C���A�E�g�`�F�b�N 
            If (bFgTimeOut = True) Then                             ' �^�C���A�E�g ?
                ' �R�[���o�b�N���\�b�h�̌ďo�����~����
                TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
                rtnCode = cFRS_ERR_LDRTO                            ' Return�l = ���[�_�ʐM�^�C���A�E�g
                Exit Do
            End If

            System.Windows.Forms.Application.DoEvents()
            Call System.Threading.Thread.Sleep(1)                   ' Wait(msec)
        Loop While (True)

STP_ERR_LDR:
        ' �R�[���o�b�N���\�b�h�̌ďo�����~����
        TimerRS.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        TimerRS.Dispose()                                           ' �^�C�}�[��j������
        If (rtnCode = cFRS_ERR_LDRTO) Then                          ' ���[�_�ʐM�^�C���A�E�g ?
            r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_TMOUT)     ' �G���[���b�Z�[�W�\��
        ElseIf (rtnCode = cFRS_ERR_LDR) Then
            ' ���[�_�A���[�����b�Z�[�W�쐬 & ���[�_�A���[����ʕ\��
            r = Loader_AlarmCheck(Form1.System1, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            'V5.0.0.1�F
            If (r = cFRS_ERR_LDR3) Then
                Call W_START()                                  ' �X�^�[�g�M�����o 
                GoTo STP_RETRY
                'V5.0.0.1�F
                '    'V5.0.0.1-31��
                'ElseIf (r = cFRS_ERR_LDR1) Then
                '    rtnCode = r
            End If
            ''V5.0.0.1-31��
        End If

        Return (rtnCode)

    End Function


    ''' <summary>
    ''' Lot���f��������L��BIT��ON/OFF����    ' W113.05��ON/OFF����
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
                '��L���`�F�b�N
                r = Form1.SubstrateExistCheck(Form1.System1)
                giSubExistMsgFlag = True
                If (r = cFRS_NORMAL) Then                              ' �G���[ ? 
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
    ''' Lot���f��������L��BIT��ON/OFF����    ' W113.05��ON/OFF����
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub ClampAndVacuum(ByVal mode As Integer)

        Dim lData As Long = 0
        'V5.0.0.4�@        Dim r As Integer

        If (mode = 1) Then
            '----- V1.16.0.0�K�� -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                '                    Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            End If
            '----- V1.16.0.0�K�� -----
            '            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
            W_VACUME_ONOFF(1)
        Else
            '----- V1.16.0.0�K�� -----
            If (gSysPrm.stIOC.giClamp = 1) Then
                Call W_CLMP_ONOFF(1)                                    ' �N�����vON
                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                Call W_CLMP_ONOFF(0)                                    ' �N�����vON
                '                    Call W_CLMP_ONOFF(0)                                    ' �N�����vOFF
            End If
            W_VACUME_ONOFF(0)
            '----- V1.16.0.0�K�� -----
            '            Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��ON)
        End If

    End Sub
    ''V4.11.0.0�G��


#End Region
    'V4.11.0.0�G
    ''' <summary>
    ''' �z����Ԃ��`�F�b�N����IO��Ԃ����킹��
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
            Call ZABSVACCUME(1)                                         ' �o�L���[���̐���(�z��ON)
        Else
            Call ZABSVACCUME(0)                                         ' �o�L���[���̐���(�z��ON)

        End If

    End Sub



    ''V4.11.0.0�G��
    ''' <summary>
    ''' W113��OFF����    
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