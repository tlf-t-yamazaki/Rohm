Attribute VB_Name = "TrimErrNo"
'===============================================================================
'   Description : �G���[�R�[�h�̒�`
'
'   Copyright(C) OMRON Laser Front 2011
'
'===============================================================================

'===============================================================================
'   ����`
'===============================================================================
'----- ���W���ԍ� -----
Public Const AXIS_X = 0                                         ' X��
Public Const AXIS_Y = 1                                         ' Y��
Public Const AXIS_Z = 2                                         ' Z��
Public Const AXIS_T = 3                                         ' Theta��
Public Const AXIS_Z2 = 4                                        ' Z2��
Public Const AXIS_XY = 5                                        ' XY�������w��

'----- ���W���r�b�g�w�� -----
Public Const STATUS_BIT_AXIS_X = &H1                            ' X��
Public Const STATUS_BIT_AXIS_Y = &H2                            ' Y��
Public Const STATUS_BIT_AXIS_Z = &H4                            ' Z��
Public Const STATUS_BIT_AXIS_T = &H8                            ' Theta��
Public Const STATUS_BIT_AXIS_Z2 = &H10                          ' Z2��

'----- �S���̃X�e�[�^�X�r�b�g(GET_ALLAXIS_STATUS�֐��̃X�e�[�^�X�r�b�g) -----
Public Const BIT_AXIS_X_SERVO_ALM = &H1000                      ' X���T�[�{�A���[��
Public Const BIT_AXIS_Y_SERVO_ALM = &H2000                      ' Y���T�[�{�A���[��
Public Const BIT_AXIS_Z_SERVO_ALM = &H4000                      ' Z���T�[�{�A���[��
Public Const BIT_AXIS_T_SERVO_ALM = &H8000                      ' Theta���T�[�{�A���[��
Public Const BIT_AXIS_X_PLUS_LIMIT = &H1                        ' X���{���~�b�g
Public Const BIT_AXIS_Y_PLUS_LIMIT = &H2                        ' Y���{���~�b�g
Public Const BIT_AXIS_Z_PLUS_LIMIT = &H4                        ' Z���{���~�b�g
Public Const BIT_AXIS_T_PLUS_LIMIT = &H8                        ' Theta���{���~�b�g
Public Const BIT_AXIS_X_MINUS_LIMIT = &H10                      ' X���|���~�b�g
Public Const BIT_AXIS_Y_MINUS_LIMIT = &H20                      ' Y���|���~�b�g
Public Const BIT_AXIS_Z_MINUS_LIMIT = &H40                      ' Z���|���~�b�g
Public Const BIT_AXIS_T_MINUS_LIMIT = &H80                      ' Theta���|���~�b�g



'===============================================================================
'   i-TKY�n/���[�U�v���̃G���[�R�[�h�i�ߒl�j��`(0�ԁ`100��)
'===============================================================================
Public Const cFRS_NORMAL = 0                                    ' ����
Public Const cFRS_ERR_ADV = 1                                   ' OK(ADV(START��))�� START/RESET�҂���
Public Const cFRS_ERR_START = 1                                 ' OK(START��)     �� START/RESET�҂���
Public Const cFRS_ERR_RST = 3                                   ' Cancel(RESET��) �� START/RESET�҂���
'Public Const cFRS_ERR_HALT = 4                                 ' HALT��
Public Const cFRS_ERR_HALT = 2                                  ' HALT��
Public Const cFRS_ERR_Z = 4                                     ' Z��
Public Const cFRS_TxTy = 5                                      ' TX2/TY2����
Public Const cFRS_ERR_NOP = 9                                   ' ���̑�(OcxJog����ďo���ւ̏�L�ȊO�̖߂�)

Public Const cFRS_ERR_CVR = -1                                  ' ➑̃J�o�[�J���o
Public Const cFRS_ERR_SCVR = -2                                 ' �X���C�h�J�o�[�J���o
Public Const cFRS_ERR_LATCH = -3                                ' �J�o�[�J���b�`���o

Public Const cFRS_ERR_EMG = -11                                 ' ����~
Public Const cFRS_ERR_DUST = -12                                ' �W�o�@�ُ팟�o
Public Const cFRS_ERR_AIR = -13                                 ' �G�A�[���G���[���o
Public Const cFRS_ERR_MVC = -14                                 ' Ͻ������މ�H��ԃG���[���o
Public Const cFRS_ERR_HW = -15                                  ' �n�[�h�E�F�A�G���[���o
Public Const cFRS_ERR_LDR = -16                                 ' ���[�_�A���[�����o
Public Const cFRS_ERR_LDR1 = -17                                ' ���[�_�A���[�����o(�S��~�ُ�)
Public Const cFRS_ERR_LDR2 = -18                                ' ���[�_�A���[�����o(�T�C�N����~)
Public Const cFRS_ERR_LDR3 = -19                                ' ���[�_�A���[�����o(�y�̏�)
Public Const cFRS_ERR_LDRTO = -20                               ' ���[�_�ʐM�^�C���A�E�g

'----- IO����^�C���A�E�g -----
Public Const cFRS_TO_SCVR_CL = -21                              ' �^�C���A�E�g(�X���C�h�J�o�[�҂�)
Public Const cFRS_TO_SCVR_OP = -22                              ' �^�C���A�E�g(�X���C�h�J�o�[�J�҂�)
Public Const cFRS_TO_SCVR_ON = -23                              ' �^�C���A�E�g(�ײ�޶�ް�į�߰�s�҂�)
Public Const cFRS_TO_SCVR_OFF = -24                             ' �^�C���A�E�g(�ײ�޶�ް�į�߰�ߑ҂�)
Public Const cFRS_TO_CLAMP_ON = -25                             ' �^�C���A�E�g(�N�����v�n�m)
Public Const cFRS_TO_CLAMP_OFF = -26                            ' �^�C���A�E�g(�N�����v�n�e�e)
Public Const cFRS_TO_PM_DW = -27                                ' �^�C���A�E�g(�p���[���[�^���~�ړ�)
Public Const cFRS_TO_PM_UP = -28                                ' �^�C���A�E�g(�p���[���[�^�㏸�ړ�)
Public Const cFRS_TO_PM_FW = -29                                ' �^�C���A�E�g(�p���[���[�^����[�ړ�)
Public Const cFRS_TO_PM_BK = -30                                ' �^�C���A�E�g(�p���[���[�^�ҋ@�[�ړ�)

'----- �t�@�C�����o�̓G���[ -----
Public Const cFRS_FIOERR_INP = -31                              ' �t�@�C�����̓G���[
Public Const cFRS_FIOERR_OUT = -32                              ' �t�@�C���o�̓G���[
Public Const cFRS_COM_ERR = -33                                 ' �ʐM�G���[(PLC)
Public Const cFRS_COM_FL_ERR = -34                              ' �ʐM�G���[(FL)
Public Const cFRS_FL_ERR_INP = -35                              ' �e�k���ɉ��H�����̐ݒ肪�Ȃ�
Public Const cFRS_FL_ERR_SET = -36                              ' ���H�����ԍ��ݒ�G���[

'----- Video.OCX�̃G���[ -----
Public Const cFRS_VIDEO_PTN = -40                               ' �p�^�[���F���G���[
Public Const cFRS_VIDEO_PT1 = -41                               ' �p�^�[���F���G���[(�␳�ʒu1)
Public Const cFRS_VIDEO_PT2 = -42                               ' �p�^�[���F���G���[(�␳�ʒu2)
Public Const cFRS_VIDEO_COM = -43                               ' �ʐM�G���[(CV3000)
Public Const cFRS_VIDEO_INI = -44                               ' ���������s���Ă��܂���
Public Const cFRS_VIDEO_IN2 = -45                               ' �������ς�
Public Const cFRS_VIDEO_FRM = -46                               ' �t�H�[���\����
Public Const cFRS_VIDEO_PRP = -47                               ' �v���p�e�B�l�s��
Public Const cFRS_VIDEO_GRP = -48                               ' ����ڰĸ�ٰ�ߔԍ��װ
Public Const cFRS_VIDEO_MXT = -49                               ' �e���v���[�g�� > MAX
Public Const cFRS_VIDEO_UXP = -50                               ' �\�����ʃG���[
Public Const cFRS_VIDEO_UX2 = -51                               ' �\�����ʃG���[2
Public Const cFRS_MVC_UTL = -52                                 ' MvcUtil �G���[
Public Const cFRS_MVC_PT2 = -53                                 ' MvcPt2 �G���[
Public Const cFRS_MVC_10 = -54                                  ' Mvc10 �G���[

'----- ���̑��̃G���[ -----
Public Const cFRS_ERR_SHUTTER = -60                             ' �O���V���b�^�[�G���[���o ###801�D
Public Const cFRS_ERR_PLC = -61                                 ' PLC�X�e�[�^�X�ُ�

'----- V8.0.0.15�@(OcxSystem)�� -----
'----- �p�����[�^�G���[��(���b�Z�[�W�\���͂��Ȃ�) -----
Public Const cFRS_ERR_CMD_NOTSPT = -70                          ' ���T�|�[�g�R�}���h
Public Const cFRS_ERR_CMD_PRM = -71                             ' �p�����[�^�G���[
Public Const cFRS_ERR_CMD_LIM_L = -72                           ' �p�����[�^�����l�G���[
Public Const cFRS_ERR_CMD_LIM_U = -73                           ' �p�����[�^����l�G���[
Public Const cFRS_ERR_CMD_OBJ = -74                             ' �I�u�W�F�N�g���ݒ�(Utility��޼ު�đ�)
Public Const cFRS_ERR_CMD_EXE = -75                             ' �R�}���h���s�G���[(DllTrimFnc����99�ŕԂ��Ă������)
'                                                               ' (��)cFRS_ERR_CMD_EXE�`cFRS_ERR_CMD_NOTSPT�Ŕ��肵�Ă���ӏ������邽��
'                                                               ' �@�@�ǉ�����ꍇ�͒���(cFRS_ERR_CMD_EXE�����炵�Ĕԍ���U�蒼��)

'----- V8.0.0.15�@(OcxSystem)�� -----

'----- ��ʏ����p�̖߂�l -----                                 ' -80�ԑ��-90�ԑ��Usrpro,TKY���̉�ʏ����Ŏg�p
Public Const cFRS_ERR_HING = -80                                ' �A��NG-HIGH��G���[����
Public Const cFRS_ERR_REPROB = -81                              ' �ăv���[�r���O���s

Public Const cFRS_FNG_DATA = -85                                ' �f�[�^�����[�h
Public Const cFRS_FNG_CMD = -86                                 ' ���R�}���h���s��
Public Const cFRS_FNG_PASS = -87                                ' �߽ܰ�ޓ��ʹװ
Public Const cFRS_FNG_CPOS = -88                                ' �J�b�g�ʒu�␳�Ώۂ̒�R���Ȃ�

Public Const cFRS_TRIM_NG = -90                                 ' �g���~���ONG
Public Const cFRS_ERR_TRIM = -91                                ' �g���}�G���[
Public Const cFRS_ERR_PTN = -92                                 ' �p�^�[���F���G���[(�p�^�[����������Ȃ�����)
Public Const cFRS_ERR_PT2 = -93                                 ' �p�^�[���F���G���[(臒l�G���[)

'----- ���̑��̃G���[ -----
Public Const cERR_TRAP = -99                                    ' �ׯ�ߴװ����

'----- INtime���G���[(100�Ԉȍ~)�͉��L�̃G���[�R�[�h�~ -1 �ŕԂ�


'===============================================================================
'   INtime���G���[�R�[�h��`(100�Ԉȍ~)
'===============================================================================
'-----( �G���[�X�e�[�^�X )-----
'  �g�����u���b�N�������̃G���[ (LastError�ɐݒ肵�Adwerrno��Windows���֕ԐM)
Public Const ERR_CLEAR = 0                                      ' ���~�b�g��ԃN���A�i���~�b�g�G���[�Ȃǁj
Public Const ERR_ALREADY_SET = 1
Public Const ERR_TERMINATE = 2                                  ' �z�X�g��蒆�f�I��
Public Const ERR_RESET = 3                                      ' ���Z�b�g�I��
Public Const ERR_CONTACT = 5                                    ' �R���^�N�g�G���[

' �\�t�g�E�F�A�G���[
'Public Const ERR_EMGSWCH = 102                                  ' ����~
'Public Const ERR_SRV_ALM = 103                                  ' �T�[�{�A���[��
'Public Const ERR_AXS_LIM = 104                                  ' XYZ�����~�b�g���o

'�X�e�[�W�G���[
'  �^�C���A�E�g�G���[�ɂ��ẮA�x�[�X+�Ώ̎��r�b�g(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
'  �̃G���[�R�[�h�ŕԂ��B
'Public Const ERR_TIMEOUT_BASE = 100                             ' �^�C���A�E�g�G���[(�x�[�X�ԍ�)
                                                                ' X���^�C���A�E�g=101
                                                                ' Y���^�C���A�E�g=102
                                                                ' Z���^�C���A�E�g=104
                                                                ' Theta���^�C���A�E�g=108
                                                                ' Z2���^�C���A�E�g=116
                                                                ' ��������or�̒l
                                                                ' X,Y���^�C���A�E�g=103

'Public Const ERR_TIMEOUT_X = 101                               ' X���^�C���A�E�g
'Public Const ERR_TIMEOUT_Y = 102                               ' Y���^�C���A�E�g
'Public Const ERR_TIMEOUT_Z = 103                               ' Z���^�C���A�E�g
'Public Const ERR_TIMEOUT_T = 104                               ' �Ǝ��^�C���A�E�g
'Public Const ERR_SOFTLIMIT_X = 105                            ' X���\�t�g���~�b�g
'Public Const ERR_SOFTLIMIT_Y = 106                            ' Y���\�t�g���~�b�g
'Public Const ERR_SOFTLIMIT_Z = 107                            ' Z���\�t�g���~�b�g
'Public Const ERR_TIMEOUT_ATT = 108                              ' ATT �� �^�C���A�E�g���Ɉړ�
'Public Const ERR_TIMEOUT_Z2 = 109                              ' Z2���^�C���A�E�g
'Public Const ERR_BP_XLIMIT = 110                               ' BP X�����͈̓I�[�o�[�i���g�p�j
'Public Const ERR_BP_YLIMIT = 111                               ' BP Y�����͈̓I�[�o�[(���g�p�j
'Public Const ERR_BP_MOVE_TIMEOUT = 112                         ' BP �^�C���A�E�g

'----- �\�t�g���~�b�g�G���[(OcxSystem�Ŏg�p) -----
Public Const ERR_SOFTLIMIT_X = 105                              ' X���\�t�g���~�b�g
Public Const ERR_SOFTLIMIT_Y = 106                              ' Y���\�t�g���~�b�g
Public Const ERR_SOFTLIMIT_Z = 107                              ' Z���\�t�g���~�b�g
Public Const ERR_SOFTLIMIT_Z2 = 113                             ' Z2���\�t�g���~�b�g
Public Const ERR_BP_XLIMIT = 110                                ' BP X���\�t�g���~�b�g�G���[
Public Const ERR_BP_YLIMIT = 111                                ' BP Y���\�t�g���~�b�g�G���[
Public Const ERR_SOFTLIMIT_T = 112                              ' THETA���\�t�g���~�b�g '###801�@

Public Const ERR_INTIME_BASE = 100                              ' INtime���G���[�x�[�X�ԍ�(INtime���G���[�R�[�h�̍ŏ��l)
                                                                ' 200�ɂ�������ERR_TIMEOUT_ATT�i108�j�������Ă���
'----- �n�[�h�E�F�A�G���[ -----
Public Const ERR_EMGSWCH = 201                                  ' ����~
Public Const ERR_SRV_ALM = 202                                  ' �T�[�{�A���[��
'Public Const ERR_AXS_LIM = 203                                 ' XYZ�����~�b�g���o �i���ۂɂ͖��g�p�j
Public Const ERR_AXS_LIM_X = 203                                ' X�����~�b�g���o
Public Const ERR_AXS_LIM_Y = 204                                ' Y�����~�b�g���o
Public Const ERR_AXS_LIM_Z = 205                                ' Z�����~�b�g���o
Public Const ERR_AXS_LIM_T = 206                                ' �Ǝ����~�b�g���o
Public Const ERR_AXS_LIM_ATT = 207                              ' ATT�����~�b�g���o
Public Const ERR_AXS_LIM_Z2 = 208                               ' Z2�����~�b�g���o
Public Const ERR_OPN_CVR = 209                                  ' ➑̃J�o�[�J���o
Public Const ERR_OPN_SCVR = 210                                 ' �X���C�h�J�o�[�J���o
Public Const ERR_OPN_CVRLTC = 211                               ' �J�o�[�J���b�`���o
Public Const ERR_INTERLOCK_STSCHG = 212                         ' �C���^�[���b�N��Ԃ̕ύX

Public Const ERR_AXS_LIM = 220                                  ' �e���̃��~�b�g���o(���~�b�g���B��)
    
Public Const ERR_HARD_FAILURE = 250                             ' �n�[�h�E�F�A�̒v���I�Ȍ̏�

Public Const ERR_CLAMP_MOVE_TIMEOUT = 260                       ' �N�����v �^�C���A�E�g

'----- ���W�J���G���[ -----
Public Const ERR_CMD_NOTSPT = 301                               ' ���T�|�[�g�R�}���h
Public Const ERR_CMD_PRM = 302                                  ' �p�����[�^�G���[
Public Const ERR_CMD_LIM_L = 303                                ' �p�����[�^�����l�G���[
Public Const ERR_CMD_LIM_U = 304                                ' �p�����[�^����l�G���[
Public Const ERR_RT2WIN_SEND = 305                              ' INTime��Windows���M�G���[
Public Const ERR_RT2WIN_RECV = 306                              ' INTime��Windows��M�G���[
Public Const ERR_WIN2RT_SEND = 307                              ' Windows��INTime���M�G���[
Public Const ERR_WIN2RT_RECV = 308                              ' Windows��INTime��M�G���[
Public Const ERR_SYS_BADPOINTER = 309                           ' �s���|�C���^
Public Const ERR_SYS_FREE_MEMORY = 310                          ' �������̈�̊J���G���[
Public Const ERR_SYS_ALLOC_MEMORY = 311                         ' �������̈�̊m�ۃG���[
Public Const ERR_CALC_OVERFLOW = 320                            ' �I�[�o�[�t���[

Public Const ERR_INTIME_NOTMOVE = 350                           ' INTRIM���N�����Ă��Ȃ�

'----- �n�[�h�E�F�AIO�n�G���[�R�[�h�F400�ԑ� -----
Public Const ERR_CSLLAMP_SETNO = 401                            ' �R���\�[���n�����v�ԍ��w��G���[
Public Const ERR_SIGTWRLAMP_SETNO = 402                         ' �V�O�i���^���[�����v�ԍ��w��G���[
Public Const ERR_SIGTWRLAMP_SETMODENO = 402                     ' �V�O�i���^���[�����v�_��/�_�Ń��[�h�w��G���[
Public Const ERR_BIT_ONOFF = 403                                ' �r�b�g�I��/�I�t�w��G���[
Public Const ERR_SETPRM_WAITSWNON = 404                         ' ���͑҂��ΏۃX�C�b�`�̎w�肪�S�ĂȂ��ɂȂ��Ă���B

'----- ����n�G���[�R�[�h�F500�ԑ� -----
Public Const ERR_MEAS_RANGESET_TYPE = 501                       ' ���背���W�ݒ�G���[�F�w�背���W�ݒ�^�C�v�Ȃ�
Public Const ERR_MEAS_SETRNG_NO = 502                           ' ���背���W�ݒ�G���[�F�Ώۃ����W�Ȃ�
Public Const ERR_MEAS_SETRNG_LO = 503                           ' ���背���W�ݒ�G���[�F�ŏ������W�ȉ�
Public Const ERR_MEAS_SETRNG_HI = 504                           ' ���背���W�ݒ�G���[�F�ő僌���W�ȏ�
Public Const ERR_MEAS_RNG_NOTSET = 505                          ' ���背���W�ݒ�G���[�F�����W���ݒ�
Public Const ERR_MEAS_FAIL = 506                                ' ����G���[
Public Const ERR_MEAS_FAST_R = 507                              ' �����x��R����G���[
Public Const ERR_MEAS_HIGHPRECI_R = 508                         ' �����x��R����G���[
Public Const ERR_MEAS_FAST_V = 509                              ' �����x�d������G���[
Public Const ERR_MEAS_HIGHPRECI_V = 510                         ' �����x�d������G���[
Public Const ERR_MEAS_TARGET = 511                              ' ����ڕW�l�ݒ�G���[
Public Const ERR_MEAS_TARGET_LO = 512                           ' ����ڕW�l�ݒ�G���[�F�ŏ��ڕW�l�ȉ�
Public Const ERR_MEAS_TARGET_HI = 513                           ' ����ڕW�l�ݒ�G���[�F�ő�ڕW�l�ȉ�
Public Const ERR_MEAS_SCANNER = 514                             ' ����X�L���i�ݒ�G���[�F�s���X�L���i�ԍ�
Public Const ERR_MEAS_SCANNER_LO = 515                          ' ����X�L���i�ݒ�G���[�F�ŏ��X�L���i�ԍ��ȉ�
Public Const ERR_MEAS_SCANNER_HI = 516                          ' ����X�L���i�ݒ�G���[�F�ő�X�L���i�ԍ��ȏ�
Public Const ERR_MEAS_RNG_SHORT = 517                           ' ����l�F�����W�V���[�g�i0.01�ȉ��j
Public Const ERR_MEAS_RNG_OVER = 518                            ' ����l�F�����W�I�[�o�[�i67000000.0�ȏ�j
Public Const ERR_MEAS_SPAN = 519                                ' ����͈͊O
Public Const ERR_MEAS_SPAN_SHORT = 520                          ' ����͈͊O�F�V���[�g-��R(0x6666)/�d��(0x3333)�ȉ�
Public Const ERR_MEAS_SPAN_OVER = 521                           ' ����͈͊O�F�I�[�o�[-��R(0xCCCC)/�d��(0x6666)�ȏ�
Public Const ERR_MEAS_INVALID_SLOPE = 522                       ' �X���[�v�ݒ�G���[
Public Const ERR_MEAS_COUNT = 523                               ' ����񐔎w��G���[
Public Const ERR_MEAS_SETMODE = 524                             ' ���莞�̐ݒ胂�[�h�iMfset���[�h�j�G���[
Public Const ERR_MEAS_AUTORNG_OVER = 525                        ' �I�[�g�����W����:��d������͈̓I�[�o�[�i���d���̈�j
Public Const ERR_MEAS_K2VAL_SHORT = 526                         ' ���d������:K2�ݒ�l0.4����
Public Const ERR_MEAS_K2VAL_OVER = 527                          ' ���d������:K2�ݒ�l0.8�ȏ�
Public Const ERR_MEAS_SCANSET_TIMEOUT = 528                     ' �X�L���i�ݒ芮���^�C���A�E�g

'----- BP�n�G���[�R�[�h�F600�ԑ� -----
Public Const ERR_BP_MAXLINEANUM_LO = 601                        ' ���j�A���e�B�␳�F�ŏ��ԍ��ȉ�
Public Const ERR_BP_MAXLINEANUM_HI = 602                        ' ���j�A���e�B�␳�F�ő�ԍ��ȏ�
Public Const ERR_BP_LOGICALNUM_LO = 603                         ' �ی��ݒ�F�ŏ��ی��l�ԍ��ȉ�
Public Const ERR_BP_LOGICALNUM_HI = 604                         ' �ی��ݒ�F�ő�ی��l�ԍ��ȉ�
Public Const ERR_BP_LIMITOVER = 605                             ' BP�ړ������ݒ�F���~�b�g�I�[�o�[
Public Const ERR_BP_HARD_LIMITOVER_LO = 606                     ' BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ŏ����͈͈ȉ��j
Public Const ERR_BP_HARD_LIMITOVER_HI = 607                     ' BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ő���͈͈ȏ�j
Public Const ERR_BP_SOFT_LIMITOVER = 608                        ' �\�t�g���͈̓I�[�o�[
Public Const ERR_BP_BSIZE_OVER = 609                            ' �u���b�N�T�C�Y�ݒ�I�[�o�[�i�\�t�g���͈̓I�[�o�[�j
Public Const ERR_BP_SIZESET = 610                               ' BP�T�C�Y�ݒ�G���[
Public Const ERR_BP_MOVE_TIMEOUT = 611                          ' BP �^�C���A�E�g
Public Const ERR_BP_GRV_ALARM_X = 620                           ' �K���o�m�A���[��X
Public Const ERR_BP_GRV_ALARM_Y = 621                           ' �K���o�m�A���[��Y
Public Const ERR_BP_GRVMOVE_HARDERR = 622                       ' �|�W�V���j���O���쎞�̃K���o�m����ُ�i�w�ߒl�ƌ��ݒl�s��v�j
Public Const ERR_BP_GRVSET_AXIS = 630                           ' �|�W�V���j���O���쎞�̍��W�ݒ�or���W�擾�G���[
Public Const ERR_BP_GRVSET_MOVEMODE = 631                       ' �K���o�m���[�h�ݒ莞�̓��샂�[�h�ݒ�l�s���i�|�W�V���j���O=0�A�g���~���O=1�ȊO�̒l���ݒ肳��Ă���j
Public Const ERR_BP_GRVSET_SPEEDSHORT = 632                     ' �K���o�m���x�w��F�ŏ��X�s�[�h����
Public Const ERR_BP_GRVSET_SPEEDOVER = 633                      ' �K���o�m���x�w��F�ő�X�s�[�h�I�[�o�[
Public Const ERR_BP_GRVSET_MISMATCH = 634                       ' �|�W�V���j���O���쎞�̃K���o�m����ُ�i�ݒ�l�ɑ΂��ݒ肳�ꂽ�l(�n�[�h����Ǐo�����l�j���s��v�j

'----- �g���~���O/�J�b�g�n�G���[�R�[�h:700�ԑ� -----
Public Const ERR_CUT_NOT_SUPPORT = 701                          ' ���T�|�[�g�J�b�g�`��
Public Const ERR_CUT_PARAM_LEN = 702                            ' �J�b�g�p�����[�^�G���[�F�J�b�g���ݒ�G���[
Public Const ERR_CUT_PARAM_LEN_LO = 703                         ' �J�b�g�p�����[�^�G���[�F�J�b�g���ŏ��l�ȉ�
Public Const ERR_CUT_PARAM_LEN_HI = 704                         ' �J�b�g�p�����[�^�G���[�F�J�b�g���ő�l�ȏ�
Public Const ERR_CUT_PARAM_CORR = 706                           ' �J�b�g�p�����[�^�G���[�F�p�����[�^���փG���[
Public Const ERR_CUT_PARAM_SPD = 707                            ' �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�G���[
Public Const ERR_CUT_PARAM_SPD_LO = 708                         ' �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�I�[�o�[
Public Const ERR_CUT_PARAM_SPD_HI = 709                         ' �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�V���[�g
Public Const ERR_CUT_PARAM_DIR = 710                            ' �J�b�g�p�����[�^�G���[�F�J�b�g����
Public Const ERR_CUT_PARAM_CUTCNT = 711                         ' �J�b�g�p�����[�^�G���[�F�J�b�g�{��
Public Const ERR_CUT_PARAM_CHRSIZE = 712                        ' �J�b�g�p�����[�^�G���[�F�����T�C�Y�w��G���[
Public Const ERR_CUT_PARAM_CUTANGLE = 713                       ' �J�b�g�p�����[�^�G���[�F�p�x�w��G���[
Public Const ERR_CUT_PARAM_CHARSET = 714                        ' �J�b�g�p�����[�^�G���[�F������w��G���[
Public Const ERR_CUT_PARAM_STRLEN = 715                         ' �J�b�g�p�����[�^�G���[�F�}�[�L���O�����񒷎w��G���[
Public Const ERR_CUT_PARAM_NOHAR = 716                          ' �J�b�g�p�����[�^�G���[�F�w�蕶���Ȃ�
Public Const ERR_CUT_PARAM_NORES = 717                          ' �J�b�g�p�����[�^�G���[�F��R�ԍ��s��
Public Const ERR_CUT_PARAM_CUTMODE = 718                        ' �J�b�g�p�����[�^�G���[�F�J�b�g���[�h�s���iCUT_MODE_NORMAL(1)�`CUT_MODE_NANAME(4)�ȊO�w��j
Public Const ERR_CUT_PARAM_PARCENT = 719                        ' �J�b�g�p�����[�^�G���[�F�|�C���g�ݒ�
Public Const ERR_CUT_PARAM_BP = 720

Public Const ERR_CUT_RATIOPRM_BASERNO = 730                     ' ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R�ԍ��w��G���[
Public Const ERR_CUT_RATIOPRM_BASER_NG = 731                    ' ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R����NG
Public Const ERR_CUT_RATIOPRM_MODENOT2 = 732                    ' ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R��Ratio���[�h2�łȂ�
Public Const ERR_CUT_RATIOPRM_CALCFORM = 733                    ' ���V�I�p�����[�^�G���[�F�v�Z�p�����s��
Public Const ERR_CUT_RATIOPRM_CANTSET = 734                     ' ���V�I�p�����[�^�G���[�F�f�[�^�̓o�^���o���Ȃ�
Public Const ERR_CUT_RATIOPRM_GRPNO = 735                       ' ���V�I�p�����[�^�G���[�F�O���[�v�ԍ��w��G���[

Public Const ERR_CUT_UCUTPRM_NOPRMRES = 740                     ' U�J�b�g�p�����[�^�G���[�F��R���p�����[�^�G���A�̐ݒ�Ȃ�
Public Const ERR_CUT_UCUTPRM_NOPRMCUT = 741                     ' U�J�b�g�p�����[�^�G���[�F�J�b�g���p�����[�^�G���A�̐ݒ�Ȃ�
Public Const ERR_CUT_UCUTPRM_NORES = 742                        ' U�J�b�g�p�����[�^�G���[�F�Ώے�R�Ȃ�
Public Const ERR_CUT_UCUTPRM_NODATA = 743                       ' U�J�b�g�p�����[�^�G���[�F�p�����[�^�f�[�^���ݒ肳��Ă��Ȃ�
Public Const ERR_CUT_UCUTPRM_NOMODE = 744                       ' U�J�b�g�p�����[�^�G���[�F�w�胂�[�h�Ȃ�
Public Const ERR_CUT_UCUTPRM_TBLIDX = 745                       ' U�J�b�g�p�����[�^�G���[�F�e�[�u���̃C���f�b�N�X���s��

Public Const ERR_CUT_CIRCUIT_NO = 750                           ' �T�[�L�b�g�p�����[�^�G���[�F�T�[�L�b�g�ԍ��w��G���[

Public Const ERR_TRIMRESULT_TESTNO = 760                        ' �g���~���O���ʁF�擾�Ώی��ʔԍ��w��G���[�i�e�X�g���{�����傫���j
Public Const ERR_TRIMRESULT_RESNO = 761                         ' �g���~���O���ʁF�擾�Ώے�R�J�n�ԍ��w��G���[�i�o�^��R�����傫���J�n�ԍ��j
Public Const ERR_TRIMRESULT_TOTALRESNO = 762                    ' �g���~���O���ʁF�擾�Ώے�R�ԍ��w��G���[�i�o�^��R�����傫���j
Public Const ERR_TRIMRESULT_CUTNO = 763                         ' �g���~���O���ʁF�擾�ΏۃJ�b�g�J�n�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j
Public Const ERR_TRIMRESULT_TOTALCUTNO = 764                    ' �g���~���O���ʁF�擾�ΏۃJ�b�g�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j
Public Const ERR_TRIMRESULT_ESRESCUTNO = 765                    ' �g���~���O���ʁF�擾�Ώے�R�ԍ�or�J�b�g�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j

Public Const ERR_TRIM_COTACTCHK = 770                           ' �R���^�N�g�`�F�b�N�G���[
Public Const ERR_TRIM_OPNSRTCHK = 771                           ' �I�[�v���V���[�g�`�F�b�N�G���[
Public Const ERR_TRIM_CIR_NORES = 772                           ' �T�[�L�b�g�g���~���O-���Ώے�R�Ȃ�

Public Const ERR_MARKVFONT_ANGLE = 780                          ' �t�H���g�ݒ�F�p�x�w��G���[
Public Const ERR_MARKVFONT_MAXPOINT = 781                       ' �t�H���g�ݒ�F�|�C���g���I�[�o�[

Public Const ERR_TRIMMODE_UNMATCH = 790                         ' �g���~���O���[�h�s��v�F���V�I�g���~���O�Ώۂ�����̂Ƀf�B���C���[�h���ݒ肳��Ă���

'----- ���[�U�n�G���[�R�[�h:800�ԑ� -----
Public Const ERR_LSR_PARAM_ONOFF = 801                          ' ���[�UON/OFF�p�����[�^�G���[
Public Const ERR_LSR_PARAM_QSW = 802                            ' Q���[�g�p�����[�^�G���[
Public Const ERR_LSR_PARAM_QSW_LO = 803                         ' Q���[�g�p�����[�^�G���[�F�ŏ�Q���[�g�ȉ�
Public Const ERR_LSR_PARAM_QSW_HI = 804                         ' Q���[�g�p�����[�^�G���[�F�ŏ�Q���[�g�ȏ�
Public Const ERR_LSR_PARAM_FLCUTCND = 830                       ' FL�p���H�����ԍ��ݒ�G���[
Public Const ERR_LSR_PARAM_FLPRM = 831                          ' FL�p�p�����[�^�G���[�i�����ʎw��G���[�j
Public Const ERR_LSR_PARAM_FLTIMEOUT = 832                      ' FL�p�ݒ�^�C���A�E�g
Public Const ERR_LSR_STATUS_STANBY = 833                        ' FL:Stanby error,ES:POWER ON READY error
Public Const ERR_LSR_STATUS_OSCERR = 850                        ' FL:Error occured,:ES:LD Alarm

'----- �X�e�[�W�n�G���[�R�[�h: -----
Public Const ERR_STG_STATUS = 901                               ' �X�e�[�W�n�ŃX�e�[�^�X�G���[����
Public Const ERR_STG_NOT_EXIST = 902                            ' �ΏۃX�e�[�W(X/Y/Z/Z2/Theta)���݂����B
Public Const ERR_STG_ORG_NONCOMPLETION = 903                    ' ���_���A������
'Public Const ERR_SOFTLIMIT_X = 105                             ' X���\�t�g���~�b�g
'Public Const ERR_SOFTLIMIT_Y = 106                             ' Y���\�t�g���~�b�g
'Public Const ERR_SOFTLIMIT_Z = 107                             ' Z���\�t�g���~�b�g
Public Const ERR_STG_MOVESEQUENCE = 905                         ' �X�e�[�W����V�[�P���X�ݒ�G���[
Public Const ERR_STG_THETANOTEXIST = 906                        ' �ƍڕ���Ȃ�
Public Const ERR_STG_INIT_X = 910                               ' X�����_���A�G���[
Public Const ERR_STG_INIT_Y = 911                               ' Y�����_���A�G���[
Public Const ERR_STG_INIT_Z = 912                               ' Z�����_���A�G���[
Public Const ERR_STG_INIT_T = 913                               ' Theta�����_���A�G���[
Public Const ERR_STG_INIT_Z2 = 914                              ' Z2�����_���A�G���[
Public Const ERR_STG_MOVE_X = 915                               ' X���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
Public Const ERR_STG_MOVE_Y = 916                               ' Y���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
Public Const ERR_STG_MOVE_Z = 917                               ' Z���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
Public Const ERR_STG_MOVE_T = 918                               ' Theta���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu
Public Const ERR_STG_MOVE_Z2 = 919                              ' Z2���X�e�[�W����ُ�F�w�ߒl != �ړ��ʒu

Public Const ERR_STG_SOFTLMT_PLUS = 930                         ' �v���X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_MINUS = 931                        ' �}�C�i�X���~�b�g�I�[�o�[

'----- �^�C���A�E�g�G���[ -----
'  �^�C���A�E�g�G���[�ɂ��ẮA�x�[�X+�Ώ̎��r�b�g(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
'  �̃G���[�R�[�h�ŕԂ��B
Public Const ERR_TIMEOUT_BASE = 950
                                                                ' �^�C���A�E�g�G���[(�x�[�X�ԍ�)
                                                                ' X���^�C���A�E�g=951
                                                                ' Y���^�C���A�E�g=952
                                                                ' Z���^�C���A�E�g=954
                                                                ' Theta���^�C���A�E�g=958
                                                                ' Z2���^�C���A�E�g=966
                                                                ' ��������or�̒l
                                                                ' X,Y���^�C���A�E�g=953
                                                                                                                           
Public Const ERR_AXIS_X_BASE = 1000                             ' X���n�G���[�x�[�X�ԍ�
Public Const ERR_AXIS_Y_BASE = 2000                             ' Y���n�G���[�x�[�X�ԍ�
Public Const ERR_AXIS_Z_BASE = 3000                             ' Z���n�G���[�x�[�X�ԍ�
Public Const ERR_AXIS_T_BASE = 4000                             ' Theta���n�G���[�x�[�X�ԍ�
Public Const ERR_AXIS_Z2_BASE = 5000                            ' Z2���n�G���[�x�[�X�ԍ�

                                                                                                                      
Public Const ERR_TIMEOUT_AXIS_X = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_X                      ' X���^�C���A�E�g
Public Const ERR_TIMEOUT_AXIS_Y = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Y                      ' Y���^�C���A�E�g
Public Const ERR_TIMEOUT_AXIS_Z = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Z                      ' Z���^�C���A�E�g
Public Const ERR_TIMEOUT_AXIS_T = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_T                      ' �Ǝ��^�C���A�E�g
Public Const ERR_TIMEOUT_AXIS_Z2 = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Z2                    ' Z2���^�C���A�E�g
Public Const ERR_TIMEOUT_AXIS_XY = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_X + STATUS_BIT_AXIS_Y ' XY���^�C���A�E�g
Public Const ERR_TIMEOUT_ATT = 108                                                          ' ���[�^���A�b�e�l�[�^�^�C���A�E�g

Public Const ERR_AXIS_X_SERVO_ALM = ERR_AXIS_X_BASE + 0                                     ' X���T�[�{�A���[��
Public Const ERR_AXIS_Y_SERVO_ALM = ERR_AXIS_Y_BASE + 0                                     ' Y���T�[�{�A���[��
Public Const ERR_AXIS_Z_SERVO_ALM = ERR_AXIS_Z_BASE + 0                                     ' Z���T�[�{�A���[��
Public Const ERR_AXIS_T_SERVO_ALM = ERR_AXIS_T_BASE + 0                                     ' �Ǝ��T�[�{�A���[��

Public Const ERR_STG_SOFTLMT_PLUS_AXIS_X = ERR_AXIS_X_BASE + ERR_STG_SOFTLMT_PLUS           ' X���v���X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Y = ERR_AXIS_Y_BASE + ERR_STG_SOFTLMT_PLUS           ' Y���v���X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Z = ERR_AXIS_Z_BASE + ERR_STG_SOFTLMT_PLUS           ' Z���v���X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_T = ERR_AXIS_T_BASE + ERR_STG_SOFTLMT_PLUS           ' �Ǝ��v���X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Z2 = ERR_AXIS_Z2_BASE + ERR_STG_SOFTLMT_PLUS         ' Z2���v���X���~�b�g�I�[�o�[

Public Const ERR_STG_SOFTLMT_MINUS_AXIS_X = ERR_AXIS_X_BASE + ERR_STG_SOFTLMT_MINUS         ' X���}�C�i�X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Y = ERR_AXIS_Y_BASE + ERR_STG_SOFTLMT_MINUS         ' Y���}�C�i�X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Z = ERR_AXIS_Z_BASE + ERR_STG_SOFTLMT_MINUS         ' Z���}�C�i�X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_T = ERR_AXIS_T_BASE + ERR_STG_SOFTLMT_MINUS         ' �Ǝ��}�C�i�X���~�b�g�I�[�o�[
Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Z2 = ERR_AXIS_Z2_BASE + ERR_STG_SOFTLMT_MINUS       ' Z2���}�C�i�X���~�b�g�I�[�o�[

'----- IO�n�G���[�R�[�h: -----
Public Const ERR_IO_PCINOTFOUND = 10000                         ' PCI�{�[�h�����o�ł���
Public Const ERR_IO_NOTGET_RTCCMOSDATA = 10001                  ' RTC�ް��ǂݍ��ݎ��s

'----- GPIB�n�G���[�R�[�h -----
Public Const ERR_GPIB_PARAM = 11001                             ' GPIB�p�����[�^�G���[
Public Const ERR_GPIB_TCPSOCKET = 11002                         ' GPIB-TCP/IP���M�G���[
Public Const ERR_GPIB_EXEC = 11003                              ' GPIB�R�}���h���s�G���[


