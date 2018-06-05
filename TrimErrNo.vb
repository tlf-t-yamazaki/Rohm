'===============================================================================
'   Description  : �G���[�R�[�h�̒�`
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Option Strict Off
Option Explicit On
Module TrimErrNo
#Region "�G���[�R�[�h�̒�`"
    '===============================================================================
    '   ����`
    '===============================================================================
    '----- ���W���ԍ� -----
    Public Const AXIS_X As Short = 0                                    ' X��
    Public Const AXIS_Y As Short = 1                                    ' Y��
    Public Const AXIS_Z As Short = 2                                    ' Z��
    Public Const AXIS_T As Short = 3                                    ' Theta��
    Public Const AXIS_Z2 As Short = 4                                   ' Z2��
    Public Const AXIS_XY As Short = 5                                   ' XY�������w��

    '----- ���W���r�b�g�w�� -----
    Public Const STATUS_BIT_AXIS_X As Short = &H1                       ' X��
    Public Const STATUS_BIT_AXIS_Y As Short = &H2                       ' Y��
    Public Const STATUS_BIT_AXIS_Z As Short = &H4                       ' Z��
    Public Const STATUS_BIT_AXIS_T As Short = &H8                       ' Theta��
    Public Const STATUS_BIT_AXIS_Z2 As Short = &H10                     ' Z2��

    '----- �S���̃X�e�[�^�X�r�b�g(GET_ALLAXIS_STATUS�֐��̃X�e�[�^�X�r�b�g) -----
    Public Const BIT_AXIS_X_SERVO_ALM As UShort = &H1000                ' X���T�[�{�A���[��
    Public Const BIT_AXIS_Y_SERVO_ALM As UShort = &H2000                ' Y���T�[�{�A���[��
    Public Const BIT_AXIS_Z_SERVO_ALM As UShort = &H4000                ' Z���T�[�{�A���[��
    Public Const BIT_AXIS_T_SERVO_ALM As UShort = &H8000                ' Theta���T�[�{�A���[��
    Public Const BIT_AXIS_X_PLUS_LIMIT As UShort = &H1                  ' X���{���~�b�g
    Public Const BIT_AXIS_Y_PLUS_LIMIT As UShort = &H2                  ' Y���{���~�b�g
    Public Const BIT_AXIS_Z_PLUS_LIMIT As UShort = &H4                  ' Z���{���~�b�g
    Public Const BIT_AXIS_T_PLUS_LIMIT As UShort = &H8                  ' Theta���{���~�b�g
    Public Const BIT_AXIS_X_MINUS_LIMIT As UShort = &H10                ' X���|���~�b�g
    Public Const BIT_AXIS_Y_MINUS_LIMIT As UShort = &H20                ' Y���|���~�b�g
    Public Const BIT_AXIS_Z_MINUS_LIMIT As UShort = &H40                ' Z���|���~�b�g
    Public Const BIT_AXIS_T_MINUS_LIMIT As UShort = &H80                ' Theta���|���~�b�g
#End Region

#Region "i-TKY�n�G���[�R�[�h�̒�`"
    '===============================================================================
    '   i-TKY�n/���[�U�v���̃G���[�R�[�h�i�ߒl�j��`(0�ԁ`100��)
    '   ��OcxSystem�Ɠ�����`�Ƃ��鎖
    '===============================================================================
    Public Const cFRS_NORMAL As Short = 0                               ' ����
    Public Const cFRS_ERR_ADV As Short = 1                              ' OK(ADV(START��))�� START/RESET�҂���
    Public Const cFRS_ERR_START As Short = 1                            ' OK(START��)     �� START/RESET�҂���
    Public Const cFRS_ERR_RST As Short = 3                              ' Cancel(RESET��) �� START/RESET�҂���
    Public Const cFRS_ERR_HALT As Short = 2                             ' HALT��
    Public Const cFRS_ERR_Z As Short = 4                                ' Z��
    Public Const cFRS_TxTy As Short = 5                                 ' TX2/TY2����
    Public Const cFRS_ERR_LOTEND As Short = 6                           ' LOTEND�p    '###126
    Public Const cFRS_ERR_OTHER As Short = 7                            ' ���̑��łR�ڂ̃{�^�����g�p����ꍇ�p 'V4.9.0.0�@

    Public Const cFRS_ERR_BTN_START As Short = 8                        ' START��    'V4.11.0.0�J

    Public Const cFRS_ERR_CVR As Short = -1                             ' ➑̃J�o�[�J���o
    Public Const cFRS_ERR_SCVR As Short = -2                            ' �X���C�h�J�o�[�J���o
    Public Const cFRS_ERR_LATCH As Short = -3                           ' �J�o�[�J���b�`���o

    Public Const cFRS_ERR_EMG As Short = -11                            ' ����~
    Public Const cFRS_ERR_DUST As Short = -12                           ' �W�o�@�ُ팟�o
    Public Const cFRS_ERR_AIR As Short = -13                            ' �G�A�[���G���[���o
    Public Const cFRS_ERR_MVC As Short = -14                            ' Ͻ������މ�H��ԃG���[���o
    Public Const cFRS_ERR_HW As Short = -15                             ' �n�[�h�E�F�A�G���[���o
    Public Const cFRS_ERR_LDR As Short = -16                            ' ���[�_�A���[�����o          
    Public Const cFRS_ERR_LDR1 As Short = -17                           ' ���[�_�A���[�����o(�S��~�ُ�)
    Public Const cFRS_ERR_LDR2 As Short = -18                           ' ���[�_�A���[�����o(�T�C�N����~) 
    Public Const cFRS_ERR_LDR3 As Short = -19                           ' ���[�_�A���[�����o(�y�̏�)
    Public Const cFRS_ERR_LDRTO As Short = -20                          ' ���[�_�ʐM�^�C���A�E�g          

    '----- IO����^�C���A�E�g(OcxSystem�Ŏg�p) -----
    Public Const cFRS_TO_SCVR_CL As Short = -21                         ' �^�C���A�E�g(�X���C�h�J�o�[�҂�)
    Public Const cFRS_TO_SCVR_OP As Short = -22                         ' �^�C���A�E�g(�X���C�h�J�o�[�J�҂�)
    Public Const cFRS_TO_SCVR_ON As Short = -23                         ' �^�C���A�E�g(�ײ�޶�ް�į�߰�s�҂�)
    Public Const cFRS_TO_SCVR_OFF As Short = -24                        ' �^�C���A�E�g(�ײ�޶�ް�į�߰�ߑ҂�)
    Public Const cFRS_TO_CLAMP_ON As Short = -25                        ' �^�C���A�E�g(�N�����v�n�m)
    Public Const cFRS_TO_CLAMP_OFF As Short = -26                       ' �^�C���A�E�g(�N�����v�n�e�e)
    Public Const cFRS_TO_PM_DW As Short = -27                           ' �^�C���A�E�g(�p���[���[�^���~�ړ�)
    Public Const cFRS_TO_PM_UP As Short = -28                           ' �^�C���A�E�g(�p���[���[�^�㏸�ړ�)
    Public Const cFRS_TO_PM_FW As Short = -29                           ' �^�C���A�E�g(�p���[���[�^����[�ړ�)
    Public Const cFRS_TO_PM_BK As Short = -30                           ' �^�C���A�E�g(�p���[���[�^�ҋ@�[�ړ�)

    '----- �t�@�C�����o�̓G���[ -----
    Public Const cFRS_FIOERR_INP As Short = -31                         ' �t�@�C�����̓G���[
    Public Const cFRS_FIOERR_OUT As Short = -32                         ' �t�@�C���o�̓G���[
    Public Const cFRS_COM_ERR As Short = -33                            ' �ʐM�G���[(PLC)
    Public Const cFRS_COM_FL_ERR As Short = -34                         ' �ʐM�G���[(FL)
    Public Const cFRS_FL_ERR_INP As Short = -35                         ' �e�k���ɉ��H�����̐ݒ肪�Ȃ�
    Public Const cFRS_FL_ERR_SET As Short = -36                         ' ���H�����ԍ��ݒ�G���[

    '----- Video.OCX�̃G���[ -----
    Public Const cFRS_VIDEO_PTN As Short = -40                          ' �p�^�[���F���G���[
    Public Const cFRS_VIDEO_PT1 As Short = -41                          ' �p�^�[���F���G���[(�␳�ʒu1)
    Public Const cFRS_VIDEO_PT2 As Short = -42                          ' �p�^�[���F���G���[(�␳�ʒu2)
    Public Const cFRS_VIDEO_COM As Short = -43                          ' �ʐM�G���[(CV3000)
    Public Const cFRS_VIDEO_INI As Short = -44                          ' ���������s���Ă��܂���
    Public Const cFRS_VIDEO_IN2 As Short = -45                          ' �������ς�
    Public Const cFRS_VIDEO_FRM As Short = -46                          ' �t�H�[���\����
    Public Const cFRS_VIDEO_PRP As Short = -47                          ' �v���p�e�B�l�s��
    Public Const cFRS_VIDEO_GRP As Short = -48                          ' ����ڰĸ�ٰ�ߔԍ��װ
    Public Const cFRS_VIDEO_MXT As Short = -49                          ' �e���v���[�g�� > MAX
    Public Const cFRS_VIDEO_UXP As Short = -50                          ' �\�����ʃG���[
    Public Const cFRS_VIDEO_UX2 As Short = -51                          ' �\�����ʃG���[2
    Public Const cFRS_MVC_UTL As Short = -52                            ' MvcUtil �G���[
    Public Const cFRS_MVC_PT2 As Short = -53                            ' MvcPt2 �G���[
    Public Const cFRS_MVC_10 As Short = -54                             ' Mvc10 �G���[

    '----- ���̑��̃G���[ -----
    Public Const cFRS_ERR_SHUTTER As Short = -60                        ' �O���V���b�^�[�G���[���o ###041
    Public Const cFRS_ERR_PLC As Short = -61                            ' PLC�X�e�[�^�X�ُ� ###041
    Public Const cFRS_TO_EXLOCK As Short = -62                          ' �d�����b�N�^�C���A�E�g V1.18.0.1�G

    '----- ��ʏ����p�̖߂�l -----                                  ' -80�ԑ��-90�ԑ��Usrpro,TKY���̉�ʏ����Ŏg�p
    Public Const cFRS_ERR_HING As Short = -80                           ' �A��NG-HIGH��G���[����
    Public Const cFRS_ERR_REPROB As Short = -81                         ' �ăv���[�r���O���s
    Public Const cFRS_FNG_DATA As Short = -85                           ' �f�[�^�����[�h
    Public Const cFRS_FNG_CMD As Short = -86                            ' ���R�}���h���s��
    Public Const cFRS_FNG_PASS As Short = -87                           ' �߽ܰ�ޓ��ʹװ
    Public Const cFRS_FNG_CPOS As Short = -88                           ' �J�b�g�ʒu�␳�Ώۂ̒�R���Ȃ�
    Public Const cFRS_FNG_PROBCHK As Short = -89                        ' �v���[�u�`�F�b�N�G���[ V1.23.0.0�F

    Public Const cFRS_TRIM_NG As Short = -90                            ' �g���~���ONG
    Public Const cFRS_ERR_TRIM As Short = -91                           ' �g���}�G���[
    Public Const cFRS_ERR_PTN As Short = -92                            ' �p�^�[���F���G���[(�p�^�[����������Ȃ�����)
    Public Const cFRS_ERR_PT2 As Short = -93                            ' �p�^�[���F���G���[(臒l�G���[)

    '----- ���̑��̃G���[ -----
    Public Const cERR_TRAP As Short = -99                               ' �g���b�v�G���[����
    Public Const cFRS_VACCUME_ERROR As Short = -100                     ' �z���G���[ V4.7.0.0�P

#End Region

#Region "INtime���G���[�R�[�h��`"
    '===============================================================================
    '   INtime���G���[�R�[�h��`(100�Ԉȍ~)
    '   (��)OcxSystem�����INtime���G���[(100�Ԉȍ~)�͉��L�̃G���[�R�[�h�~ -1 �ŕԂ�
    '===============================================================================
    '-----( �G���[�X�e�[�^�X )-----
    '  �g�����u���b�N�������̃G���[ (LastError�ɐݒ肵�Adwerrno��Windows���֕ԐM)
    Public Const ERR_CLEAR As Short = 0                                 ' ���~�b�g��ԃN���A�i���~�b�g�G���[�Ȃǁj
    Public Const ERR_ALREADY_SET As Short = 1
    Public Const ERR_TERMINATE As Short = 2                             ' �z�X�g��蒆�f�I��
    Public Const ERR_RESET As Short = 3                                 ' ���Z�b�g�I��
    Public Const ERR_CONTACT As Short = 5                               ' �R���^�N�g�G���[

    ' �\�t�g�E�F�A�G���[
    'Public Const ERR_EMGSWCH As Short = 102                            ' ����~
    'Public Const ERR_SRV_ALM As Short = 103                            ' �T�[�{�A���[��
    'Public Const ERR_AXS_LIM As Short = 104                            ' XYZ�����~�b�g���o

    '�X�e�[�W�G���[
    '  �^�C���A�E�g�G���[�ɂ��ẮA�x�[�X+�Ώ̎��r�b�g(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
    '  �̃G���[�R�[�h�ŕԂ��B
    'Public Const ERR_TIMEOUT_BASE As Short = 100                       ' �^�C���A�E�g�G���[(�x�[�X�ԍ�)
    ' X���^�C���A�E�gAs Short =101
    ' Y���^�C���A�E�gAs Short =102
    ' Z���^�C���A�E�gAs Short =104
    ' Theta���^�C���A�E�gAs Short =108
    ' Z2���^�C���A�E�gAs Short =116
    ' ��������or�̒l
    ' X,Y���^�C���A�E�gAs Short =103

    'Public Const ERR_TIMEOUT_X As Short = 101                          ' X���^�C���A�E�g
    'Public Const ERR_TIMEOUT_Y As Short = 102                          ' Y���^�C���A�E�g
    'Public Const ERR_TIMEOUT_Z As Short = 103                          ' Z���^�C���A�E�g
    'Public Const ERR_TIMEOUT_T As Short = 104                          ' �Ǝ��^�C���A�E�g
    'Public Const ERR_SOFTLIMIT_X As Short = 105                        ' X���\�t�g���~�b�g
    'Public Const ERR_SOFTLIMIT_Y As Short = 106                        ' Y���\�t�g���~�b�g
    'Public Const ERR_SOFTLIMIT_Z As Short = 107                        ' Z���\�t�g���~�b�g
    'Public Const ERR_TIMEOUT_ATT As Short = 108                        ' ATT �� �^�C���A�E�g���Ɉړ�
    'Public Const ERR_TIMEOUT_Z2 As Short = 109                         ' Z2���^�C���A�E�g
    'Public Const ERR_BP_XLIMIT As Short = 110                          ' BP X�����͈̓I�[�o�[�i���g�p�j
    'Public Const ERR_BP_YLIMIT As Short = 111                          ' BP Y�����͈̓I�[�o�[(���g�p�j
    'Public Const ERR_BP_MOVE_TIMEOUT As Short = 112                    ' BP �^�C���A�E�g

    '----- �\�t�g���~�b�g�G���[(OcxSystem�Ŏg�p) -----
    Public Const ERR_SOFTLIMIT_X As Short = 105                         ' X���\�t�g���~�b�g
    Public Const ERR_SOFTLIMIT_Y As Short = 106                         ' Y���\�t�g���~�b�g
    Public Const ERR_SOFTLIMIT_Z As Short = 107                         ' Z���\�t�g���~�b�g
    Public Const ERR_SOFTLIMIT_Z2 As Short = 113                        ' Z2���\�t�g���~�b�g
    Public Const ERR_BP_XLIMIT As Short = 110                           ' BP X���\�t�g���~�b�g�G���[
    Public Const ERR_BP_YLIMIT As Short = 111                           ' BP Y���\�t�g���~�b�g�G���[

    Public Const ERR_INTIME_BASE As Short = 100                         ' INtime���G���[�x�[�X�ԍ�(INtime���G���[�R�[�h�̍ŏ��l)
    ' 200�ɂ�������ERR_TIMEOUT_ATT�i108�j�������Ă���?
    '----- �n�[�h�E�F�A�G���[ -----
    Public Const ERR_EMGSWCH As Short = 201                             ' ����~
    Public Const ERR_SRV_ALM As Short = 202                             ' �T�[�{�A���[��
    'Public Const ERR_AXS_LIM As Short = 203                            ' XYZ�����~�b�g���o �i���ۂɂ͖��g�p�j
    Public Const ERR_AXS_LIM_X As Short = 203                           ' X�����~�b�g���o
    Public Const ERR_AXS_LIM_Y As Short = 204                           ' Y�����~�b�g���o
    Public Const ERR_AXS_LIM_Z As Short = 205                           ' Z�����~�b�g���o
    Public Const ERR_AXS_LIM_T As Short = 206                           ' �Ǝ����~�b�g���o
    Public Const ERR_AXS_LIM_ATT As Short = 207                         ' ATT�����~�b�g���o
    Public Const ERR_AXS_LIM_Z2 As Short = 208                          ' Z2�����~�b�g���o

    Public Const ERR_OPN_CVR As Short = 209                             ' ➑̃J�o�[�J���o
    Public Const ERR_OPN_SCVR As Short = 210                            ' �X���C�h�J�o�[�J���o
    Public Const ERR_OPN_CVRLTC As Short = 211                          ' �J�o�[�J���b�`���o
    Public Const ERR_AXS_LIM As Short = 220                             ' �e���̃��~�b�g���o(���~�b�g���B��)

    '----- ���W�J���G���[ -----
    Public Const ERR_CMD_NOTSPT As Short = 301                          ' ���T�|�[�g�R�}���h
    Public Const ERR_CMD_PRM As Short = 302                             ' �p�����[�^�G���[
    Public Const ERR_CMD_LIM_L As Short = 303                           ' �p�����[�^�����l�G���[
    Public Const ERR_CMD_LIM_U As Short = 304                           ' �p�����[�^����l�G���[
    Public Const ERR_RT2WIN_SEND As Short = 305                         ' INTime��Windows���M�G���[
    Public Const ERR_RT2WIN_RECV As Short = 306                         ' INTime��Windows��M�G���[
    Public Const ERR_WIN2RT_SEND As Short = 307                         ' Windows��INTime���M�G���[
    Public Const ERR_WIN2RT_RECV As Short = 308                         ' Windows��INTime��M�G���[
    Public Const ERR_SYS_BADPOINTER As Short = 309                      ' �s���|�C���^
    Public Const ERR_SYS_FREE_MEMORY As Short = 310                     ' �������̈�̊J���G���[
    Public Const ERR_SYS_ALLOC_MEMORY As Short = 311                    ' �������̈�̊m�ۃG���[
    Public Const ERR_CALC_OVERFLOW As Short = 320                       ' �I�[�o�[�t���[
    Public Const ERR_INTIME_NOTMOVE As Short = 350                      ' INTRIM���N�����Ă��Ȃ�

    '----- �n�[�h�E�F�AIO�n�G���[�R�[�h�F400�ԑ� -----
    Public Const ERR_CSLLAMP_SETNO As Short = 401                       ' �R���\�[���n�����v�ԍ��w��G���[
    Public Const ERR_SIGTWRLAMP_SETNO As Short = 402                    ' �V�O�i���^���[�����v�ԍ��w��G���[
    Public Const ERR_SIGTWRLAMP_SETMODENO As Short = 402                ' �V�O�i���^���[�����v�_��/�_�Ń��[�h�w��G���[
    Public Const ERR_BIT_ONOFF As Short = 403                           ' �r�b�g�I��/�I�t�w��G���[
    Public Const ERR_SETPRM_WAITSWNON As Short = 404                    ' ���͑҂��ΏۃX�C�b�`�̎w�肪�S�ĂȂ��ɂȂ��Ă���B

    '----- ����n�G���[�R�[�h�F500�ԑ� -----
    Public Const ERR_MEAS_RANGESET_TYPE As Short = 501                  ' ���背���W�ݒ�G���[�F�w�背���W�ݒ�^�C�v�Ȃ�
    Public Const ERR_MEAS_SETRNG_NO As Short = 502                      ' ���背���W�ݒ�G���[�F�Ώۃ����W�Ȃ�
    Public Const ERR_MEAS_SETRNG_LO As Short = 503                      ' ���背���W�ݒ�G���[�F�ŏ������W�ȉ�
    Public Const ERR_MEAS_SETRNG_HI As Short = 504                      ' ���背���W�ݒ�G���[�F�ő僌���W�ȏ�
    Public Const ERR_MEAS_RNG_NOTSET As Short = 505                     ' ���背���W�ݒ�G���[�F�����W���ݒ�
    Public Const ERR_MEAS_FAIL As Short = 506                           ' ����G���[
    Public Const ERR_MEAS_FAST_R As Short = 507                         ' �����x��R����G���[
    Public Const ERR_MEAS_HIGHPRECI_R As Short = 508                    ' �����x��R����G���[
    Public Const ERR_MEAS_FAST_V As Short = 509                         ' �����x�d������G���[
    Public Const ERR_MEAS_HIGHPRECI_V As Short = 510                    ' �����x�d������G���[
    Public Const ERR_MEAS_TARGET As Short = 511                         ' ����ڕW�l�ݒ�G���[
    Public Const ERR_MEAS_TARGET_LO As Short = 512                      ' ����ڕW�l�ݒ�G���[�F�ŏ��ڕW�l�ȉ�
    Public Const ERR_MEAS_TARGET_HI As Short = 513                      ' ����ڕW�l�ݒ�G���[�F�ő�ڕW�l�ȉ�
    Public Const ERR_MEAS_SCANNER As Short = 514                        ' ����X�L���i�ݒ�G���[�F�s���X�L���i�ԍ�
    Public Const ERR_MEAS_SCANNER_LO As Short = 515                     ' ����X�L���i�ݒ�G���[�F�ŏ��X�L���i�ԍ��ȉ�
    Public Const ERR_MEAS_SCANNER_HI As Short = 516                     ' ����X�L���i�ݒ�G���[�F�ő�X�L���i�ԍ��ȏ�
    Public Const ERR_MEAS_RNG_SHORT As Short = 517                      ' ����l�F�����W�V���[�g�i0.01�ȉ��j
    Public Const ERR_MEAS_RNG_OVER As Short = 518                       ' ����l�F�����W�I�[�o�[�i67000000.0�ȏ�j
    Public Const ERR_MEAS_SPAN As Short = 519                           ' ����͈͊O
    Public Const ERR_MEAS_SPAN_SHORT As Short = 520                     ' ����͈͊O�F�V���[�g-��R(0x6666)/�d��(0x3333)�ȉ�
    Public Const ERR_MEAS_SPAN_OVER As Short = 521                      ' ����͈͊O�F�I�[�o�[-��R(0xCCCC)/�d��(0x6666)�ȏ�
    Public Const ERR_MEAS_INVALID_SLOPE As Short = 522                  ' �X���[�v�ݒ�G���[
    Public Const ERR_MEAS_COUNT As Short = 523                          ' ����񐔎w��G���[
    Public Const ERR_MEAS_SETMODE As Short = 524                        ' ���莞�̐ݒ胂�[�h�iMfset���[�h�j�G���[
    Public Const ERR_MEAS_AUTORNG_OVER As Short = 525                   ' �I�[�g�����W����:��d������͈̓I�[�o�[�i���d���̈�j
    Public Const ERR_MEAS_K2VAL_SHORT As Short = 526                    ' ���d������:K2�ݒ�l0.4����
    Public Const ERR_MEAS_K2VAL_OVER As Short = 527                     ' ���d������:K2�ݒ�l0.8�ȏ�
    Public Const ERR_MEAS_SCANSET_TIMEOUT As Short = 528                ' �X�L���i�ݒ芮���^�C���A�E�g
    Public Const ERR_MEAS_RANGESET_TIMEOUT As Short = 529               ' �����W�ݒ�񐔂��I�[�o�����ꍇ�̃^�C���A�E�g
    '----- V1.13.0.0�J�� -----
    Public Const ERR_MEAS_CV As Short = 530                             ' ����΂�����o
    Public Const ERR_MEAS_OVERLOAD As Short = 531                       ' �I�[�o���[�h���o
    Public Const ERR_MEAS_REPROBING As Short = 532                      ' �ăv���[�r���O�G���[
    '----- V1.13.0.0�J�� -----

    '----- BP�n�G���[�R�[�h�F600�ԑ� -----
    Public Const ERR_BP_MAXLINEANUM_LO As Short = 601                   ' ���j�A���e�B�␳�F�ŏ��ԍ��ȉ�
    Public Const ERR_BP_MAXLINEANUM_HI As Short = 602                   ' ���j�A���e�B�␳�F�ő�ԍ��ȏ�
    Public Const ERR_BP_LOGICALNUM_LO As Short = 603                    ' �ی��ݒ�F�ŏ��ی��l�ԍ��ȉ�
    Public Const ERR_BP_LOGICALNUM_HI As Short = 604                    ' �ی��ݒ�F�ő�ی��l�ԍ��ȉ�
    Public Const ERR_BP_LIMITOVER As Short = 605                        ' BP�ړ������ݒ�F���~�b�g�I�[�o�[
    Public Const ERR_BP_HARD_LIMITOVER_LO As Short = 606                ' BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ŏ����͈͈ȉ��j
    Public Const ERR_BP_HARD_LIMITOVER_HI As Short = 607                ' BP�ړ������ݒ�F���~�b�g�I�[�o�[�i�ő���͈͈ȏ�j
    Public Const ERR_BP_SOFT_LIMITOVER As Short = 608                   ' �\�t�g���͈̓I�[�o�[
    Public Const ERR_BP_BSIZE_OVER As Short = 609                       ' �u���b�N�T�C�Y�ݒ�I�[�o�[�i�\�t�g���͈̓I�[�o�[�j
    Public Const ERR_BP_SIZESET As Short = 610                          ' BP�T�C�Y�ݒ�G���[
    Public Const ERR_BP_MOVE_TIMEOUT As Short = 611                     ' BP �^�C���A�E�g
    Public Const ERR_BP_GRV_ALARM_X As Short = 620                      ' �K���o�m�A���[��X
    Public Const ERR_BP_GRV_ALARM_Y As Short = 621                      ' �K���o�m�A���[��Y
    Public Const ERR_BP_GRVMOVE_HARDERR As Short = 622                  ' �|�W�V���j���O���쎞�̃K���o�m����ُ�i�w�ߒl�ƌ��ݒl�s��v�j
    Public Const ERR_BP_GRVSET_AXIS As Short = 630                      ' �|�W�V���j���O���쎞�̍��W�ݒ�or���W�擾�G���[
    Public Const ERR_BP_GRVSET_MOVEMODE As Short = 631                  ' �K���o�m���[�h�ݒ莞�̓��샂�[�h�ݒ�l�s���i�|�W�V���j���OAs Short =0�A�g���~���OAs Short =1�ȊO�̒l���ݒ肳��Ă���j
    Public Const ERR_BP_GRVSET_SPEEDSHORT As Short = 632                ' �K���o�m���x�w��F�ŏ��X�s�[�h����
    Public Const ERR_BP_GRVSET_SPEEDOVER As Short = 633                 ' �K���o�m���x�w��F�ő�X�s�[�h�I�[�o�[

    '----- �g���~���O/�J�b�g�n�G���[�R�[�h:700�ԑ� -----
    Public Const ERR_CUT_NOT_SUPPORT As Short = 701                     ' ���T�|�[�g�J�b�g�`��
    Public Const ERR_CUT_PARAM_LEN As Short = 702                       ' �J�b�g�p�����[�^�G���[�F�J�b�g���ݒ�G���[
    Public Const ERR_CUT_PARAM_LEN_LO As Short = 703                    ' �J�b�g�p�����[�^�G���[�F�J�b�g���ŏ��l�ȉ�
    Public Const ERR_CUT_PARAM_LEN_HI As Short = 704                    ' �J�b�g�p�����[�^�G���[�F�J�b�g���ő�l�ȏ�
    Public Const ERR_CUT_PARAM_CORR As Short = 706                      ' �J�b�g�p�����[�^�G���[�F�p�����[�^���փG���[
    Public Const ERR_CUT_PARAM_SPD As Short = 707                       ' �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�G���[
    Public Const ERR_CUT_PARAM_SPD_LO As Short = 708                    ' �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�I�[�o�[
    Public Const ERR_CUT_PARAM_SPD_HI As Short = 709                    ' �J�b�g�p�����[�^�G���[�F�X�s�[�h�ݒ�V���[�g
    Public Const ERR_CUT_PARAM_DIR As Short = 710                       ' �J�b�g�p�����[�^�G���[�F�J�b�g����
    Public Const ERR_CUT_PARAM_CUTCNT As Short = 711                    ' �J�b�g�p�����[�^�G���[�F�J�b�g�{��
    Public Const ERR_CUT_PARAM_CHRSIZE As Short = 712                   ' �J�b�g�p�����[�^�G���[�F�����T�C�Y�w��G���[
    Public Const ERR_CUT_PARAM_CUTANGLE As Short = 713                  ' �J�b�g�p�����[�^�G���[�F�p�x�w��G���[
    Public Const ERR_CUT_PARAM_CHARSET As Short = 714                   ' �J�b�g�p�����[�^�G���[�F������w��G���[
    Public Const ERR_CUT_PARAM_STRLEN As Short = 715                    ' �J�b�g�p�����[�^�G���[�F�}�[�L���O�����񒷎w��G���[
    Public Const ERR_CUT_PARAM_NOHAR As Short = 716                     ' �J�b�g�p�����[�^�G���[�F�w�蕶���Ȃ�
    Public Const ERR_CUT_PARAM_NORES As Short = 717                     ' �J�b�g�p�����[�^�G���[�F��R�ԍ��s��
    Public Const ERR_CUT_PARAM_CUTMODE As Short = 718                   ' �J�b�g�p�����[�^�G���[�F�J�b�g���[�h�s���iCUT_MODE_NORMAL(1)�`CUT_MODE_NANAME(4)�ȊO�w��j
    Public Const ERR_CUT_PARAM_PARCENT As Short = 719                   ' �J�b�g�p�����[�^�G���[�F�|�C���g�ݒ�
    Public Const ERR_CUT_PARAM_BP As Short = 720
    Public Const ERR_L1_LENGTH_LOW As Short = 721                       ' �k�P�J�b�g�������l���B�G���[ 'V1.22.0.0�@
    Public Const ERR_L2_LENGTH_LOW As Short = 722                       ' �k�Q�J�b�g�������l���B�G���[ 'V1.22.0.0�@
    Public Const ERR_L3_LENGTH_LOW As Short = 723                       ' �k�R�J�b�g�������l���B�G���[ 'V1.22.0.0�@

    Public Const ERR_CUT_RATIOPRM_BASERNO As Short = 730                ' ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R�ԍ��w��G���[
    Public Const ERR_CUT_RATIOPRM_BASER_NG As Short = 731               ' ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R����NG
    Public Const ERR_CUT_RATIOPRM_MODENOT2 As Short = 732               ' ���V�I�p�����[�^�G���[�F�v�Z�p�x�[�X��R��Ratio���[�h2�łȂ�
    Public Const ERR_CUT_RATIOPRM_CALCFORM As Short = 733               ' ���V�I�p�����[�^�G���[�F�v�Z�p�����s��
    Public Const ERR_CUT_RATIOPRM_CANTSET As Short = 734                ' ���V�I�p�����[�^�G���[�F�f�[�^�̓o�^���o���Ȃ�
    Public Const ERR_CUT_RATIOPRM_GRPNO As Short = 735                  ' ���V�I�p�����[�^�G���[�F�O���[�v�ԍ��w��G���[

    Public Const ERR_CUT_UCUTPRM_NOPRMRES As Short = 740                ' U�J�b�g�p�����[�^�G���[�F��R���p�����[�^�G���A�̐ݒ�Ȃ�
    Public Const ERR_CUT_UCUTPRM_NOPRMCUT As Short = 741                ' U�J�b�g�p�����[�^�G���[�F�J�b�g���p�����[�^�G���A�̐ݒ�Ȃ�
    Public Const ERR_CUT_UCUTPRM_NORES As Short = 742                   ' U�J�b�g�p�����[�^�G���[�F�Ώے�R�Ȃ�
    Public Const ERR_CUT_UCUTPRM_NODATA As Short = 743                  ' U�J�b�g�p�����[�^�G���[�F�p�����[�^�f�[�^���ݒ肳��Ă��Ȃ�
    Public Const ERR_CUT_UCUTPRM_NOMODE As Short = 744                  ' U�J�b�g�p�����[�^�G���[�F�w�胂�[�h�Ȃ�
    Public Const ERR_CUT_UCUTPRM_TBLIDX As Short = 745                  ' U�J�b�g�p�����[�^�G���[�F�e�[�u���̃C���f�b�N�X���s��

    Public Const ERR_CUT_CIRCUIT_NO As Short = 750                      ' �T�[�L�b�g�p�����[�^�G���[�F�T�[�L�b�g�ԍ��w��G���[

    Public Const ERR_TRIMRESULT_TESTNO As Short = 760                   ' �g���~���O���ʁF�擾�Ώی��ʔԍ��w��G���[�i�e�X�g���{�����傫���j
    Public Const ERR_TRIMRESULT_RESNO As Short = 761                    ' �g���~���O���ʁF�擾�Ώے�R�J�n�ԍ��w��G���[�i�o�^��R�����傫���J�n�ԍ��j
    Public Const ERR_TRIMRESULT_TOTALRESNO As Short = 762               ' �g���~���O���ʁF�擾�Ώے�R�ԍ��w��G���[�i�o�^��R�����傫���j
    Public Const ERR_TRIMRESULT_CUTNO As Short = 763                    ' �g���~���O���ʁF�擾�ΏۃJ�b�g�J�n�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j
    Public Const ERR_TRIMRESULT_TOTALCUTNO As Short = 764               ' �g���~���O���ʁF�擾�ΏۃJ�b�g�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j
    Public Const ERR_TRIMRESULT_ESRESCUTNO As Short = 765               ' �g���~���O���ʁF�擾�Ώے�R�ԍ�or�J�b�g�ԍ��w��G���[�i�ő�J�b�g�����傫���J�n�ԍ��j

    Public Const ERR_TRIM_COTACTCHK As Short = 770                      ' �R���^�N�g�`�F�b�N�G���[
    Public Const ERR_TRIM_OPNSRTCHK As Short = 771                      ' �I�[�v���V���[�g�`�F�b�N�G���[
    Public Const ERR_TRIM_CIR_NORES As Short = 772                      ' �T�[�L�b�g�g���~���O-���Ώے�R�Ȃ�

    Public Const ERR_MARKVFONT_ANGLE As Short = 780                     ' �t�H���g�ݒ�F�p�x�w��G���[
    Public Const ERR_MARKVFONT_MAXPOINT As Short = 781                  ' �t�H���g�ݒ�F�|�C���g���I�[�o�[

    '----- ���[�U�n�G���[�R�[�h:800�ԑ� -----
    Public Const ERR_LSR_PARAM_ONOFF As Short = 801                     ' ���[�UON/OFF�p�����[�^�G���[
    Public Const ERR_LSR_PARAM_QSW As Short = 802                       ' Q���[�g�p�����[�^�G���[
    Public Const ERR_LSR_PARAM_QSW_LO As Short = 803                    ' Q���[�g�p�����[�^�G���[�F�ŏ�Q���[�g�ȉ�
    Public Const ERR_LSR_PARAM_QSW_HI As Short = 804                    ' Q���[�g�p�����[�^�G���[�F�ŏ�Q���[�g�ȏ�
    Public Const ERR_LSR_PARAM_FLCUTCND As Short = 830                  ' FL�p���H�����ԍ��ݒ�G���[
    Public Const ERR_LSR_PARAM_FLPRM As Short = 831                     ' FL�p�p�����[�^�G���[�i�����ʎw��G���[�j
    Public Const ERR_LSR_PARAM_FLTIMEOUT As Short = 832                 ' FL�p�ݒ�^�C���A�E�g
    Public Const ERR_LSR_STATUS_STANBY As Short = 833                   ' FL:Stanby error,ES:POWER ON READY error
    Public Const ERR_LSR_STATUS_OSCERR As Short = 850                   ' FL:Error occured,:ES:LD Alarm

    '----- �X�e�[�W�n�G���[�R�[�h: -----
    Public Const ERR_STG_STATUS As Short = 901                          ' �X�e�[�W�n�ŃX�e�[�^�X�G���[����
    Public Const ERR_STG_NOT_EXIST As Short = 902                       ' �ΏۃX�e�[�W(X/Y/Z/Z2/Theta)���݂����B
    Public Const ERR_STG_ORG_NONCOMPLETION As Short = 903               ' ���_���A������
    'Public Const ERR_SOFTLIMIT_X As Short = 105                        ' X���\�t�g���~�b�g
    'Public Const ERR_SOFTLIMIT_Y As Short = 106                        ' Y���\�t�g���~�b�g
    'Public Const ERR_SOFTLIMIT_Z As Short = 107                        ' Z���\�t�g���~�b�g
    Public Const ERR_STG_MOVESEQUENCE As Short = 905                    ' �X�e�[�W����V�[�P���X�ݒ�G���[
    Public Const ERR_STG_THETANOTEXIST As Short = 906                   ' �ƍڕ���Ȃ�
    Public Const ERR_STG_INIT_X As Short = 910                          ' X�����_���A�G���[
    Public Const ERR_STG_INIT_Y As Short = 911                          ' Y�����_���A�G���[
    Public Const ERR_STG_INIT_Z As Short = 912                          ' Z�����_���A�G���[
    Public Const ERR_STG_INIT_T As Short = 913                          ' Theta�����_���A�G���[
    Public Const ERR_STG_INIT_Z2 As Short = 914                         ' Z2�����_���A�G���[
    Public Const ERR_STG_MOVE_X As Short = 915                          ' X���X�e�[�W����ُ�F�w�ߒl !As Short = �ړ��ʒu
    Public Const ERR_STG_MOVE_Y As Short = 916                          ' Y���X�e�[�W����ُ�F�w�ߒl !As Short = �ړ��ʒu
    Public Const ERR_STG_MOVE_Z As Short = 917                          ' Z���X�e�[�W����ُ�F�w�ߒl !As Short = �ړ��ʒu
    Public Const ERR_STG_MOVE_T As Short = 918                          ' Theta���X�e�[�W����ُ�F�w�ߒl !As Short = �ړ��ʒu
    Public Const ERR_STG_MOVE_Z2 As Short = 919                         ' Z2���X�e�[�W����ُ�F�w�ߒl !As Short = �ړ��ʒu

    Public Const ERR_STG_SOFTLMT_PLUS As Short = 930                    ' �v���X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_MINUS As Short = 931                   ' �}�C�i�X���~�b�g�I�[�o�[

    '----- �^�C���A�E�g�G���[ -----
    '  �^�C���A�E�g�G���[�ɂ��ẮA�x�[�X+�Ώ̎��r�b�g(X:0x01,Y:0x02,Z:0x04,T:0x08,Z2:0x10)
    '  �̃G���[�R�[�h�ŕԂ��B
    Public Const ERR_TIMEOUT_BASE As Short = 950
    ' �^�C���A�E�g�G���[(�x�[�X�ԍ�)
    ' X���^�C���A�E�gAs Short =951
    ' Y���^�C���A�E�gAs Short =952
    ' Z���^�C���A�E�gAs Short =954
    ' Theta���^�C���A�E�gAs Short =958
    ' Z2���^�C���A�E�gAs Short =966
    ' ��������or�̒l
    ' X,Y���^�C���A�E�gAs Short =953

    '----- ���n�G���[�x�[�X�ԍ� -----
    Public Const ERR_AXIS_X_BASE As Short = 1000                        ' X���n�G���[�x�[�X�ԍ�
    Public Const ERR_AXIS_Y_BASE As Short = 2000                        ' Y���n�G���[�x�[�X�ԍ�
    Public Const ERR_AXIS_Z_BASE As Short = 3000                        ' Z���n�G���[�x�[�X�ԍ�
    Public Const ERR_AXIS_T_BASE As Short = 4000                        ' Theta���n�G���[�x�[�X�ԍ�
    Public Const ERR_AXIS_Z2_BASE As Short = 5000                       ' Z2���n�G���[�x�[�X�ԍ�

    '----- �^�C���A�E�g -----
    Public Const ERR_TIMEOUT_AXIS_X As Short = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_X                      ' X���^�C���A�E�g
    Public Const ERR_TIMEOUT_AXIS_Y As Short = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Y                      ' Y���^�C���A�E�g
    Public Const ERR_TIMEOUT_AXIS_Z As Short = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Z                      ' Z���^�C���A�E�g
    Public Const ERR_TIMEOUT_AXIS_T As Short = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_T                      ' �Ǝ��^�C���A�E�g
    Public Const ERR_TIMEOUT_AXIS_Z2 As Short = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_Z2                    ' Z2���^�C���A�E�g
    Public Const ERR_TIMEOUT_AXIS_XY As Short = ERR_TIMEOUT_BASE + STATUS_BIT_AXIS_X + STATUS_BIT_AXIS_Y ' XY���^�C���A�E�g
    Public Const ERR_TIMEOUT_ATT As Short = 108                                                          ' ���[�^���A�b�e�l�[�^�^�C���A�E�g

    '----- �T�[�{�A���[�� -----
    Public Const ERR_AXIS_X_SERVO_ALM As Short = ERR_AXIS_X_BASE + 0                                     ' X���T�[�{�A���[��
    Public Const ERR_AXIS_Y_SERVO_ALM As Short = ERR_AXIS_Y_BASE + 0                                     ' Y���T�[�{�A���[��
    Public Const ERR_AXIS_Z_SERVO_ALM As Short = ERR_AXIS_Z_BASE + 0                                     ' Z���T�[�{�A���[��
    Public Const ERR_AXIS_T_SERVO_ALM As Short = ERR_AXIS_T_BASE + 0                                     ' �Ǝ��T�[�{�A���[��

    '----- ���v���X���~�b�g�I�[�o�[ -----
    Public Const ERR_STG_SOFTLMT_PLUS_AXIS_X As Short = ERR_AXIS_X_BASE + ERR_STG_SOFTLMT_PLUS           ' X���v���X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Y As Short = ERR_AXIS_Y_BASE + ERR_STG_SOFTLMT_PLUS           ' Y���v���X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Z As Short = ERR_AXIS_Z_BASE + ERR_STG_SOFTLMT_PLUS           ' Z���v���X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_PLUS_AXIS_T As Short = ERR_AXIS_T_BASE + ERR_STG_SOFTLMT_PLUS           ' �Ǝ��v���X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_PLUS_AXIS_Z2 As Short = ERR_AXIS_Z2_BASE + ERR_STG_SOFTLMT_PLUS         ' Z2���v���X���~�b�g�I�[�o�[

    '----- ���}�C�i�X���~�b�g�I�[�o�[ -----
    Public Const ERR_STG_SOFTLMT_MINUS_AXIS_X As Short = ERR_AXIS_X_BASE + ERR_STG_SOFTLMT_MINUS         ' X���}�C�i�X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Y As Short = ERR_AXIS_Y_BASE + ERR_STG_SOFTLMT_MINUS         ' Y���}�C�i�X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Z As Short = ERR_AXIS_Z_BASE + ERR_STG_SOFTLMT_MINUS         ' Z���}�C�i�X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_MINUS_AXIS_T As Short = ERR_AXIS_T_BASE + ERR_STG_SOFTLMT_MINUS         ' �Ǝ��}�C�i�X���~�b�g�I�[�o�[
    Public Const ERR_STG_SOFTLMT_MINUS_AXIS_Z2 As Short = ERR_AXIS_Z2_BASE + ERR_STG_SOFTLMT_MINUS       ' Z2���}�C�i�X���~�b�g�I�[�o�[

    '----- IO�n�G���[�R�[�h: -----
    Public Const ERR_IO_PCINOTFOUND As Short = 10000                    ' PCI�{�[�h�����o�ł���
    Public Const ERR_IO_NOTGET_RTCCMOSDATA As Short = 10001             ' RTC�ް��ǂݍ��ݎ��s

    '----- GPIB�n�G���[�R�[�h -----
    Public Const ERR_GPIB_PARAM As Short = 11001                        ' GPIB�p�����[�^�G���[
    Public Const ERR_GPIB_TCPSOCKET As Short = 11002                    ' GPIB-TCP/IP���M�G���[
    Public Const ERR_GPIB_EXEC As Short = 11003                         ' GPIB�R�}���h���s�G���[

#End Region

End Module