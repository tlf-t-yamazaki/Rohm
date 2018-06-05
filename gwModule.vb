'===============================================================================
'   Description  : INtime���C���^�[�t�F�[�X��`
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.IO                       'V4.4.0.0-0
Imports System.Runtime.InteropServices
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module gwModule
#Region "�萔/�ϐ���`"
    '===========================================================================
    '   �萔/�ϐ���`
    '===========================================================================
    Public gmhTky As System.Threading.Mutex = New System.Threading.Mutex(False, "iTKY")

    '-------------------------------------------------------------------------------
    '   ���� ON/OFF����p���ߔԍ�
    '-------------------------------------------------------------------------------
    'SL43xR����
    Public Const LAMP_START As Short = 0
    Public Const LAMP_RESET As Short = 1
    Public Const LAMP_Z As Short = 2
    Public Const LAMP_HALT As Short = 5
    'Public Const LAMP_READY As Short = 0
    'Public Const LAMP_START As Short = 1
    'Public Const LAMP_HALT As Short = 2
    'Public Const LAMP_RESET As Short = 3
    'Public Const LAMP_Z As Short = 5

    '-------------------------------------------------------------------------------
    '   Interlock switch bits (ADR. 0x21E8)
    '-------------------------------------------------------------------------------
    Public Const BIT_SLIDECOVER_CLOSE As Short = &H100S     ' B8 : �ײ�޶�ް��
    Public Const BIT_SLIDECOVER_OPEN As Short = &H200S      ' B9 : �ײ�޶�ް�J
    Public Const BIT_EMERGENCY_SW As Short = &H400S         ' B10: �ϰ�ުݼ�SW
    Public Const BIT_EMERGENCY_RESET As Short = &H800S      ' B11: �ϰ�ުݼ�ؾ��
    Public Const BIT_INTERLOCK_DISABLE As Short = &H1000S   ' B12: ����ۯ�����SW
    Public Const BIT_SERVO_ALARM As Short = &H2000S         ' B13: ���ޱװ�
    Public Const BIT_COVER_CLOSE As Short = &H4000S         ' B14: ��ް��
    '                                                       ' B15: ��ް&�ײ�޶�ް��

    Public Const INTERLOCK_STS_DISABLE_NO As Short = (0)    ' �C���^�[���b�N���(�����Ȃ�)
    Public Const INTERLOCK_STS_DISABLE_PART As Short = (1)  ' �C���^�[���b�N�ꕔ�����i�X�e�[�W���\�j
    Public Const INTERLOCK_STS_DISABLE_FULL As Short = 2    ' �C���^�[���b�N�S����
    Public Const SLIDECOVER_OPEN As Short = (1)             ' Bit0 : �X���C�h�J�o�[�F�I�[�v��
    Public Const SLIDECOVER_CLOSE As Short = (2)            ' Bit1 : �X���C�h�J�o�[�F�N���[�Y
    Public Const SLIDECOVER_MOVING As Short = (4)           ' Bit2 : �X���C�h�J�o�[�F���쒆

    '-------------------------------------------------------------------------------
    '   �V�O�i���^���[�R�F����(�W��)SL432R/SL436R����       ' ###007
    '   �@�蓮�^�]�� ������������� ���_��(���_���A����, ���f�B(�蓮))
    '   �A�C���^�[���b�N���������� ���_��(H/W�Ő���)
    '   �B�e�B�[�`���O������������ ���_��(�A���A�D��)
    '   �C���_���A�� ������������� �Γ_��
    '   �D����~�� ������������� �ԓ_���{�u�U�[�n�m �� H/W��������ׂȂ�
    '   �E�����^�]�� �������������
    '     ��)����^�]���@�@�@�F�Γ_��
    '     ��)�S�}�K�W���I�����F�ԓ_�Ł{�u�U�[�n�m
    '     ��)�A���[�����@�@�@�F�ԓ_�Ł{�u�U�[�n�m�i�A���A�D���D��j
    '-------------------------------------------------------------------------------
    '----- OUTPUT -----                                     ' ON���̈Ӗ�
    '                                                       ' B0 : ���g�p
    '                                                       ' :
    '                                                       ' B7 : ���g�p
    Public Const SIGOUT_GRN_ON As UShort = &H100            ' B8 : �Γ_��  (�����^�]��)
    Public Const SIGOUT_YLW_ON As UShort = &H200            ' B9 : ���_��  (�e�B�[�`���O��)(�C���^�[���b�N�������͉��_��)�@'###024
    Public Const SIGOUT_RED_ON As UShort = &H400            ' B10: �ԓ_��  (����~) �����g�p(H/W�Ő���)
    Public Const SIGOUT_GRN_BLK As UShort = &H800           ' B11: �Γ_��  (���_���A��)
    Public Const SIGOUT_YLW_BLK As UShort = &H1000          ' B12: ���_��  (�C���^�[���b�N������)�@'###024
    Public Const SIGOUT_RED_BLK As UShort = &H2000          ' B13: �ԓ_��  (�ُ�/�S�}�K�W���I��) ��+�u�U�[�P
    Public Const SIGOUT_BZ1_ON As UShort = &H4000           ' B14: �u�U�[�P(�ُ�) ��+�ԓ_��
    '                                                       ' B15: ���g�p

    '-------------------------------------------------------------------------------
    '   �g���d�w�s�a�h�s(���16�r�b�g ADR. 213A)
    '   �V�O�i���^���[�S�F����p�r�b�g ���������Ӱè�ޓa����
    '-------------------------------------------------------------------------------
    '----- OUTPUT -----                                     ' ON���̈Ӗ�
    '                                                       ' B0 (B16): ���g�p
    '                                                       ' :
    '                                                       ' B3 (B19): ���g�p
    '                                                       ' B4 (B20): ���g�p
    '                                                       ' :
    '                                                       ' B7 (B23): ���g�p

    Public Const EXTOUT_RED_ON As UShort = &H100            ' B8 (B24): �ԓ_��  (����~) �����g�p(H/W�Ő���)
    Public Const EXTOUT_RED_BLK As UShort = &H200           ' B9 (B25): �ԓ_��  (�ُ�) ��+�u�U�[�P
    Public Const EXTOUT_YLW_ON As UShort = &H400            ' B10(B26): ���F�_��(���_���A����, ���f�B(�蓮))
    Public Const EXTOUT_YLW_BLK As UShort = &H800           ' B11(B27): ���F�_��(���_���A��)

    Public Const EXTOUT_GRN_ON As UShort = &H1000           ' B12(B28): �Γ_��  (�����^�]��)
    Public Const EXTOUT_GRN_BLK As UShort = &H2000          ' B13(B29): �Γ_��  (-) �����g�p
    Public Const EXTOUT_BZ1_ON As UShort = &H4000           ' B14(B30): �u�U�[�P(�ُ�) ��+�ԓ_��
    '                                                       ' B15(B31): ���g�p
    '----- V1.18.0.1�F�� -----
    '-------------------------------------------------------------------------------
    '   �d�w�s�a�h�s(216A BIT4-7) �����[���a����(SL436R/SL436S)
    '-------------------------------------------------------------------------------
    '----- OUTPUT -----                                     ' ON���̈Ӗ�(SL436R�̏ꍇ)
    Public EXTOUT_EX_YLW_ON As UShort = &H10                ' B4 : ���_��(���[�U�Ǝ˒�)
    Public EXTOUT_EX_LOK_ON As UShort = &H20                ' B5 : �d�����b�N(�ω����E�����b�N)

    '----- INPUT -----                                      ' ON���̈Ӗ�(SL436R�̏ꍇ)
    Public EXTINP_EX_LOK_ON As UShort = &H10                ' B4 : �d�����b�N(�ω����E�����b�N)

    '----- ���̑� -----
    Public EX_LOK_STS As Integer = &H216A                   ' �d�����b�N�X�e�[�^�X�A�h���X(SL436R�̏ꍇ)
    Public EX_LOK_TOUT As Integer = (10 * 1000)             ' �d�����b�N�X�e�[�^�X�^�C���A�E�g�l(msec)

    '----- EL_Lock_OnOff�֐��̃��[�h -----
    Public EX_LOK_MD_ON As Integer = 1                      ' �d�����b�NON
    Public EX_LOK_MD_OFF As Integer = 0                     ' �d�����b�NOFF

    '----- V1.18.0.1�F�� -----
    'V5.0.0.6�@���g��I/O�ėp��`
    Public Const EXT_IN0 As UShort = &H10                   ' B04: ���[�U���t�P
    Public Const EXT_IN1 As UShort = &H20                   ' B05: ���[�U���t�Q
    Public Const EXT_IN2 As UShort = &H40                   ' B06: ���[�U���t�R
    Public Const EXT_IN3 As UShort = &H80                   ' B07: ���[�U���t�S
    'V5.0.0.6�@��

    '-------------------------------------------------------------------------------
    '   SL43xR����
    '-------------------------------------------------------------------------------
    ' Switch���
    Public Const cSTS_NOSW_ON As Integer = 0                ' �X�C�b�`�̓��͂Ȃ��������́AON��ɃL�����Z��
    Public Const cSTS_STARTSW_ON As Integer = 1             ' START�X�C�b�`ON
    Public Const cSTS_RESETSW_ON As Integer = 3             ' RESET�X�C�b�`ON
    Public Const cSTS_HALTSW_ON As Integer = 4              ' HALT�X�C�b�`ON

    '�X���C�h�J�o�[/�Œ�J�o�[�`�F�b�N���
    Public Const SLIDECOVER_CHECK_OFF As Integer = 1        ' �X���C�h�J�o�[�`�F�b�N���s��Ȃ��B��SL436R�p�ݒ�
    Public Const SLIDECOVER_CHECK_ON As Integer = 0         ' �X���C�h�J�o�[�`�F�b�N���s���B
    Public Const COVER_CHECK_OFF As Integer = 1             ' �Œ�J�o�[�`�F�b�N���s��Ȃ�  ###088
    Public Const COVER_CHECK_ON As Integer = 0              ' �Œ�J�o�[�`�F�b�N���s��      ###088

    '---------------------------
    ' ���M���O�֌W
    '---------------------------
    '�Z�N�V������
    Public Const cTRIMLOGcSECTNAME As String = "TRIMMODE_FILELOG_DATA_SET"
    Public Const cMEASLOGcSECTNAME As String = "MEASMODE_FILELOG_DATA_SET"
    Public Const cTRIMDISP1xcSECTNAME As String = "TRIMMODE1x_DISPLOG_DATA_SET"
    Public Const cTRIMDISPx0cSECTNAME As String = "TRIMMODEx0_DISPLOG_DATA_SET"
    Public Const cTRIMDISPx1cSECTNAME As String = "TRIMMODEx1_DISPLOG_DATA_SET"
    Public Const cMEASDISPcSECTNAME As String = "MEASMODE_DISPLOG_DATA_SET"
    '�L�[��
    Public Const cDATACOUNTcKEYNAME As String = "DATACOUNT"

    Public m_TrimlogFileFormat(20) As Integer           ' �g���~���O��-���O�t�@�C���ւ̏o�͑Ώہ����Ԑݒ�ϐ�
    Public m_MeaslogFileFormat(20) As Integer           ' ���莞-���O�t�@�C���ւ̏o�͑Ώہ����Ԑݒ�ϐ�
    Public m_TrimDisp1xFormat(20) As Integer            ' �g���~���O1x��-���O�\���o�͑Ώہ����Ԑݒ�ϐ�
    Public m_TrimDispx0Format(20) As Integer            ' �g���~���Ox0��-���O�\���o�͑Ώہ����Ԑݒ�ϐ�
    Public m_TrimDispx1Format(20) As Integer            ' �g���~���Ox1��-���O�\���o�͑Ώہ����Ԑݒ�ϐ�
    Public m_MeasDispFormat(20) As Integer              ' ���莞-���O�t�@�C���ւ̏o�͑Ώہ����Ԑݒ�ϐ�
#If False Then
    '���O�^�[�Q�b�g�萔
    Public Const LOGTAR_NOSET As Short = 0
    Public Const LOGTAR_DATE As Short = 1
    Public Const LOGTAR_MODE As Short = 2
    Public Const LOGTAR_LOTNO As Short = 3
    Public Const LOGTAR_CIRCUIT As Short = 4
    Public Const LOGTAR_RESISTOR As Short = 5
    Public Const LOGTAR_JUDGE As Short = 6
    Public Const LOGTAR_TARGET As Short = 7
    Public Const LOGTAR_INITIAL As Short = 8
    Public Const LOGTAR_FINAL As Short = 9
    Public Const LOGTAR_DEVIATION As Short = 10
    Public Const LOGTAR_UCUTPRMNO As Short = 11
    ''V4.1.0.0�F   Public Const LOGTAR_END As Short = 99
    Public Const LOGTAR_END As Short = 12 ' ����g�p���Ă��郋�[�v�񐔂ɂ���
#Else
    '���O�^�[�Q�b�g�񋓑�
    Public Enum LOGTAR As Integer       '#4.12.2.0�E
        NOSET
        [DATE]
        MODE
        LOTNO
        BLOCKX
        BLOCKY
        CIRCUIT
        RESISTOR
        JUDGE
        TARGET
        INITIAL
        FINAL
        DEVIATION
        UCUTPRMNO
        [END]
    End Enum
#End If

    '���O�^�C�v�萔
    Public Const LOGTYPE_DISP As Short = 1
    Public Const LOGTYPE_FILE As Short = 2

    '-------------------------------------------------------------------------------
    '   ���[�_�[�h�^�n�r�b�g(SL432R�p ADR. 219A) '###035
    '-------------------------------------------------------------------------------
    '----- �g���}�[  �� ���[�_�[ -----
    ' ��Bit0�`Bit4,Bit7���W����
    Public Const COM_STS_TRM_STATE As UShort = &H1US        ' B0 : �g���}��~(0:��~,1:���쒆)
    Public Const COM_STS_TRM_NG As UShort = &H2US           ' B1 : �g���~���O�m�f(0:����, 1:NG)
    Public Const COM_STS_PTN_NG As UShort = &H4US           ' B2 : �p�^�[���F���G���[(0:����, 1:�G���[)
    Public Const COM_STS_TRM_ERR As UShort = &H8US          ' B3 : �g���}�G���[(0:����, 1:�G���[)
    Public Const COM_STS_TRM_READY As UShort = &H10US       ' B4 : �g���}���f�B(0:ɯ���ި, 1:��ި)
    Public Const COM_STS_LOT_END As UShort = &H20US         ' B5 : ۯďI���M��(��ROHM����)
    '                                                       ' B5 : ���g�p
    Public Const COM_STS_TRM_COMP_SUPPLY As Short = &H40S   ' B6 : �����ʒu�ړ�����'V5.0.0.6�A
    Public Const COM_STS_ABS_ON As UShort = &H80US          ' B7 : �ڕ������ߊJ��(0:��, 1:�J)

    '----- ���[�_�[  �� �g���}�[ -----
    ' ��Bit0�`Bit3�܂ł��W����
    Public Const cHSTcRDY As UShort = &H1US                 ' B0 : ���۰�ް�L��(0:��, 1:�L)
    Public Const cHSTcAUTO As UShort = &H2US                ' B1 : ���۰�ްӰ��(1=����Ӱ��, 0=�蓮Ӱ��)
    Public Const cHSTcSTATE As UShort = &H4US               ' B2 : ���۰�ް���쒆(0=���쒆, 1=��~)
    Public Const cHSTcTRMCMD As UShort = &H8US              ' B3 : ��ϰ����(1=��ϰ����) ��ׯ�

    '----- ���[�_���o�͊֘A(SL432R�n�p��߼��) -----
    Public giHostMode As Short                              ' ۰��Ӱ��(0:�蓮Ӱ��, 1:����Ӱ��)
    Public giHostMode_tmp As Short                          ' ۰�މ^�]Ӱ��(�װђ�~İ�َ��ԗp)
    Public Const cHOSTcMODEcAUTO As Short = 1               '  1:����Ӱ��
    Public Const cHOSTcMODEcMANUAL As Short = 0             '  0:�蓮Ӱ��
    Public gbHostConnected As Boolean                       ' �z�X�g�ڑ����(True=�ڑ�(۰�ޗL), False=���ڑ�(۰�ޖ�))
    Public giHostRun As Short                               ' ۰�ޓ��쒆(0:��~, 1:���쒆)
    Public gdwATLDDATA As UInteger                          ' ���[�_�o�̓f�[�^
    Public gDebugHostCmd As UInteger                        ' ���[�_���̓f�[�^(���ޯ�ޗp)
    Public gwPrevHcmd As UInteger                           ' ���[�_���̓f�[�^�ޔ���

    Public Const cLoggingIT As Short = 1
    Public Const cLoggingFT As Short = 2

    '----- �ő�l/�ŏ��l -----
    '' '' ''Public Const cMAXcMARKINGcSTRLEN As Integer = 18        ' �}�[�L���O������ő咷(byte)
    '' '' ''Public Const cMAXcSENDcPRMCNT As Integer = 32           ' VB��INTRIM�̑��M�R�}���h�p�����[�^�ő吔
    '' '' ''Public Const cResultMax As Integer = 256                ' �g���~���O���ʃf�[�^�̍ő�z��
    '' '' ''Public Const cResultAry As Integer = 999                ' �g���~���O���ʃf�[�^�̍ő吔
    '' '' ''Public Const cAxisMAX As Integer = 4                    ' �ő厲��
    '' '' ''Public Const cRsultTky As Integer = 4                   ' TKY�߂�l
    '' '' ''Public Const cRetAxisBpPos As Integer = 5               ' �e���ABP�̌��ݒl

    ' OCSLLN  CONSOLE OUT PORT
    Public Const OCSLLN_SLIDECOVER_OPEN As Short = 8 'b8
    Public Const OCSLLN_SLIDECOVER_CLOSE As Short = 9 'b9
    Public Const OCSLLN_ABS_VACCUME As Short = 10 'b10

    ' INPUT OUTPUT
    Public Const INP_MAX As Short = 5
    Public Const INP_ICSLSS As Short = 0 ' [0]:�R���\�[��SW�Z���X
    Public Const INP_IITLKS As Short = 1 ' [1]:�C���^�[���b�N�֌WSW�Z���X
    Public Const INP_AUTLODL As Short = 2 ' [2]:�I�[�g���[�_LO
    Public Const INP_AUTLODH As Short = 3 ' [3]:�I�[�g���[�_HI
    Public Const INP_ATTNATE As Short = 4 ' [4]:�Œ�A�b�e�l�[�^

    'Global Const OUT_MAX = 5
    Public Const OUT_MAX As Short = 4
    Public Const OUT_OCSLLN As Short = 0 ' [0]:�R���\�[������
    Public Const OUT_OSYSCTL As Short = 1 ' [1]:�T�[�{�p���[
    Public Const OUT_AUTLODL As Short = 2 ' [2]:�I�[�g���[�_LO
    Public Const OUT_AUTLODH As Short = 3 ' [3]:�I�[�g���[�_HI
    Public Const OUT_SIGNALT As Short = 4 ' [4]:�V�O�i���^���[

    Public Const OUT_CONSLOE As Short = 0

    ' ERROR STATUS
    Public Const ERR_SUCCESS As Short = 0 ' �G���[�Ȃ�
    'Public Const ERR_EMGSWCH As Short = 1 ' ����~
    'Public Const ERR_SRV_ALM As Short = 2 ' �T�[�{�A���[��

    ' ���_�ʒu�ی�
    Public Const DIMENSION_1 As Short = 0 ' ��1�ی�(x>0,y>0)    2 | 1
    Public Const DIMENSION_2 As Short = 1 ' ��2�ی�(x<0,y>0) -----+------
    Public Const DIMENSION_3 As Short = 2 ' ��3�ی�(x>0,y<0)    4 | 3
    Public Const DIMENSION_4 As Short = 3 ' ��4�ی�(x<0,y<0)

    ' �p���[����(NET�p)
    Private Const READ_MAX As Integer = 20              ' �p���[�ǂݎ�莞�̒l�𓾂邽�߂̑����
    Private Const READ_DEL As Integer = 5               ' �p���[�ǂݎ�莞�̒l�𓾂邽�߂ɂl�`�w�C�l�h�m���珜����
    Private Const READ_OK_WID As Integer = 333 * 2      ' �p���[�ǂݎ�莞�̂΂�����e�͈́m���j�^���̓r�b�g�l�n

    Public Const FULL_POWER_LIMIT As Double = 0.5       ' �t���p���[�������e�l(�}0.5W)

    ' ����ۯ��ւ̈ړ��ʒu�擾�p�̍\���̌`����`
    Public Structure TRIM_GETNEXTXY
        Dim intCDir As Short                            ' ���ߕ���
        Dim intStepR As Short                           ' �ï��&��߰�
        Dim dblstgx As Double                           ' �ð��X�ʒu
        Dim dblstgy As Double                           ' �ð��Y�ʒu
        Dim dblCSx As Double                            ' ���߻���X
        Dim dblCSy As Double                            ' ���߻���Y
        Dim intxy1 As Short                             ' ��ۯ��ʒu
        Dim intxy2 As Short                             ' ��ۯ��ʒu
        Dim dblgspacex As Double                        ' ��ٰ�ߊԊuX
        Dim dblgspacey As Double                        ' ��ٰ�ߊԊuY
        Dim intpxy1 As Short                            ' ��ڰĈʒu
        Dim intpxy2 As Short                            ' ��ڰĈʒu
        Dim dblblockx As Double                         ' ��ۯ�X
        Dim dblblocky As Double                         ' ��ۯ�Y
        Dim dblStepOffX As Double                       ' �ï�ߵ̾��X
        Dim dblStepOffY As Double                       ' �ï�ߵ̾��Y
        Dim dblStrp As Double                           ' �X�e�b�v�C���^�[�o��
        Dim dblblockIntervalx As Double                 ' ��ۯ��ԊuX
        Dim dblblockIntervaly As Double                 ' ��ۯ��ԊuY
        Dim intBlockCntXDir As Short                    ' ��ۯ����w
        Dim intBlockCntYDir As Short                    ' ��ۯ����x
        '----- <CHIP�̂�> -----
        Dim intArrayCntX As Short                       ' �z��
        Dim intArrayCntY As Short                       ' �z��
        '----- <NET�̂�> -----
        Dim dblADDSZX As Double
        Dim dblADDSZY As Double
        Dim intptxy1 As Short                           ' ��ڰĈʒu
        Dim intptxy2 As Short                           ' ��ڰĈʒu
        Dim dblptspacex As Double                       ' ��ٰ�ߊԊuX
        Dim dblptspacey As Double                       ' ��ٰ�ߊԊuY
    End Structure

    ' �I�[�g���[�_�֘A�f�[�^����
    Public Structure ATLD_DATA
        Dim iLED As Short                               ' �o�b�N���C�g�Ɩ�(0:�펞�n�m,1:�펞�n�e�e,2:�摜�F����)
        Dim iLOT As Short                               ' ���b�g�؊�(0:OFF,1:ON)
    End Structure
    Public stATLD As ATLD_DATA                          ' �I�[�g���[�_�֘A�f�[�^

    '-------------------------------------------------------------------------------
    '   �t�@�C�o�[���[�U�p��`
    '-------------------------------------------------------------------------------
    '----- ���U���� -----
    Public Const OSCILLATOR_FL As Integer = 3           ' FL(̧��ްڰ��)

    '----- �g���}�[���H�����p��` -----
    'V5.0.0.8�@    Public Const cCNDNUM As Integer = 8                 ' 1��Ă̍ő���H������
    '#5.0.0.8�@    '                                                   ' ���H�����\���̂̃C���f�b�N�X
    '#5.0.0.8�@    Public Const CUT_CND_L1 As Integer = 0              ' L1���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L2 As Integer = 1              ' L2���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L3 As Integer = 2              ' L3���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L4 As Integer = 3              ' L4���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L1_RET As Integer = 4          ' L1_Return/Retrace���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L2_RET As Integer = 5          ' L2_Return/Retrace���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L3_RET As Integer = 6          ' L3_Return/Retrace���H�����ݒ�
    '#5.0.0.8�@    Public Const CUT_CND_L4_RET As Integer = 7          ' L4_Return/Retrace���H�����ݒ�

    Public stCND As TrimCondInfo                        ' �g���}�[���H����(�`����`��Rs232c.vb�Q��)

    '----- ���H�����ԍ�31(�t���p���[����p)�ޔ��� ----- ' ###072
    Public FLCnd31Curr As Integer                       ' �d���l(mA)
    Public FLCnd31Freq As Double                        ' ���g��(KHz)
    Public FLCnd31Steg As Integer                       ' STEG�g�`

    '----- RS232C�|�[�g����`
    Public stCOM As ComInfo                             ' �|�[�g���(�`����`��Rs232c.vb�Q��)
    Public Const cTIMEOUT As Long = 10000               ' �����҃^�C�}�l(ms)

    '----- �f�t�H���g -----
    'V6.0.0.1�E    Public Const POWERADJUST_TARGET As Double = 1.0     ' �ڕW�p���[(W)            LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_TAERGET ���g�p����
    'V6.0.0.1�E    Public Const POWERADJUST_LEVEL As Double = 0.5      ' ���e�͈�(�}W)             LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_LEVEL�� �g�p����
    'V6.0.0.1�E    Public Const POWERADJUST_CURRENT As Short = 1200    ' �d���l(mA) V2.0.0.0_23   LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_CURRENT ���g�p����
    'V6.0.0.1�E    Public Const POWERADJUST_STEG As Short = 10         ' STEG1�`8   V2.0.0.0_23   LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_STEG ���g�p����

    '----- FL�p�p���[������� ###066 -----
    Public Structure POWER_ADJUST_INFO                  ' FL�p�p���[�������`����`
        Dim CndNumAry() As Integer                      ' ���H�����ԍ��z��(0=����, 1=�L��) 
        Dim AdjustTargetAry() As Double                 ' �����ڕW�p���[�z�� 
        Dim AdjustLevelAry() As Double                  ' �p���[�������e�͈͔z��
        Dim AttInfoAry() As Integer                     ' �Œ�ATT���(0:�Œ�ATT Off, 1:�Œ�ATT On)�z�� V1.14.0.0�A

        ' ���̍\���̂�����������ɂ́A"Initialize" ���Ăяo���Ȃ���΂Ȃ�܂���B 
        Public Sub Initialize()
            ReDim CndNumAry(MAX_BANK_NUM - 1)
            ReDim AdjustTargetAry(MAX_BANK_NUM - 1)
            ReDim AdjustLevelAry(MAX_BANK_NUM - 1)
            ReDim AttInfoAry(MAX_BANK_NUM - 1)          ' V1.14.0.0�A
        End Sub
    End Structure
    Public stPWR As POWER_ADJUST_INFO                   ' FL�p�p���[������� ###192 
    Public stPWR_LSR As POWER_ADJUST_INFO               ' FL�p�p���[�������(���[�U�R�}���h�p) V1.14.0.0�A

    '----- ���̑� -----
    Public Const MINCUR_CND_NUM As Integer = 30         ' ���H�����ԍ�(�ŏ��d���l�ݒ�p)
    Public Const ADJ_CND_NUM As Integer = 0             ' ���H�����ԍ�(�ꎞ��~��ʗp) ###237

    '-------------------------------------------------------------------------------
    '   ���̑�
    '-------------------------------------------------------------------------------
    Public gManualThetaCorrection As Boolean            ' �V�[�^�␳���s�t���O(True=�V�[�^�␳�����s����, False=�V�[�^�␳�����s����)
    '                                                   ' �␳���[�h=1(�蓮)�ŕ␳���@=1(1��̂�)�̔���Ɏg�p

    '-------------------------------------------------------------------------------
    '   ���M���O�������ʎ擾�萔
    '-------------------------------------------------------------------------------
    Public Const RSLTTYP_TRIMJUDGE As Integer = 0           ' �g���~���O���茋��
    Public Const RSLTTYP_INTIAL_TEST As Integer = 1         ' �C�j�V�����e�X�g����
    Public Const RSLTTYP_FINAL_TEST As Integer = 2          ' �t�@�C�i���e�X�g����
    Public Const RSLTTYP_RATIO_TARGET As Integer = 3        ' ���V�I����
    '----- V1.18.0.0�E�� -----
    Public Const RSLTTYP_CIRCUIT As Integer = 4             ' �T�[�L�b�g����
    Public Const RSLTTYP_IX2_CUTCOUNT As Integer = 5        ' �J�b�g��(IX2)
    Public Const RSLTTYP_IX2_MEAS As Integer = 6            ' ����l(IX2)
    Public Const RSLTTYP_ES2_MEASCOUNT As Integer = 7       ' ����l��(ES2)
    Public Const RSLTTYP_ES2_MEAS As Integer = 8            ' ����l  (ES2)
    '----- V1.18.0.0�E�� -----

#End Region

#Region "�g���~���O�f�[�^��`"
    '===========================================================================
    '   �g���~���O�v��/�����f�[�^��`
    '===========================================================================
    '----- �v��/�����f�[�^(VB����INtime) ----- ���ȉ��͖��g�p�̂��ߕs�v
    Public AppInfo As S_CMD_DAT                         ' �v���f�[�^(�R�}���h)(VB��INtime)
    Public ResInfo As S_RES_DAT                         ' �����f�[�^(�R�}���h)(VB��INtime)

    '----- �g���~���O�v���f�[�^(VB��INtime) -----
    '(2011/2/2)
    '   ���I�z�񂪍\���̃����o�ɂ���ꍇ�A�����������̌Ăяo�����K�v�ɂȂ�B
    '   ���[�J�������o�Ő錾�㏉�������s���Ă��A�x���������Ȃ��̂ŁA
    '   �p�u���b�N�����o�Ƃ��č\���̂�錾����B�i�ڍג�����\�Ȃ烍�[�J���֏C���j
    'Public stTPLT As TRIM_PLATE_DATA                     ' �v���[�g�f�[�^
    'Public stTGPI As TRIM_DAT_GPIB                      ' GPIB�ݒ�f�[�^
    Public stTGPI As TRIM_PLATE_GPIB                     ' GPIB�ݒ�f�[�^ ###002
    Public stTGPI2 As TRIM_PLATE_GPIB2                   ' GPIB�ݒ�f�[�^ ###229
    Public stTGPI3 As TRIM_PLATE_GPIB3                   ' �O�������[�ؑ֋@GPIB�ݒ�f�[�^ V6.1.1.0�D

    'Public stTREG As PRM_RESISTOR                       ' ��R�f�[�^
    Public stTCUT As TRIM_CUT_DATA                       ' �J�b�g�f�[�^
    'Public stTREG As TRIM_DAT_RESISTOR                  ' ��R�f�[�^
    'Public stTCUT As TRIM_DAT_CUT                       ' �J�b�g�f�[�^
    '                                                   ' �J�b�g�p�����[�^ 
    'Public stCutST As PRM_CUT_ST                        ' ST cut�p�����[�^
    'Public stCutL As PRM_CUT_L                          ' L cut�p�����[�^
    'Public stCutHK As PRM_CUT_HOOK                      ' HOOK cut�p�����[�^
    'Public stCutIX As PRM_CUT_INDEX                     ' INDEX cut�p�����[�^
    'Public stCutIX2 As PRM_CUT_INDEX                    ' INDEX2 cut�p�����[�^
    'Public stCutSC As PRM_CUT_SCAN                      ' SCAN cut�p�����[�^
    Public stCutMK As PRM_CUT_MARKING                   ' Letter Marking�p�����[�^
    ''Public stCutC As TRIM_DAT_CUT_C                     ' C cut�p�����[�^
    'Public stCutES As PRM_CUT_ES                        ' ES cut�p�����[�^
    ''Public stCutE2 As TRIM_DAT_CUT_ES2                  ' ES2 cut�p�����[�^
    ''Public stCutZ As TRIM_DAT_CUT_Z                     ' Z cut(NOP)�p�����[�^

    'Public stCutST As TRIM_DAT_CUT_ST                   ' ST cut�p�����[�^
    'Public stCutL As TRIM_DAT_CUT_L                     ' L cut�p�����[�^
    'Public stCutHK As TRIM_DAT_CUT_HOOK                 ' HOOK cut�p�����[�^
    'Public stCutIX As TRIM_DAT_CUT_INDEX                ' INDEX cut�p�����[�^
    'Public stCutIX2 As TRIM_DAT_CUT_INDEX               ' INDEX2 cut�p�����[�^
    'Public stCutSC As TRIM_DAT_CUT_SCAN                 ' SCAN cut�p�����[�^
    'Public stCutMK As TRIM_DAT_CUT_MARKING              ' Letter Marking�p�����[�^
    '' ''Public stCutC As TRIM_DAT_CUT_C                     ' C cut�p�����[�^
    'Public stCutES As TRIM_DAT_CUT_ES                   ' ES cut�p�����[�^
    '' ''Public stCutE2 As TRIM_DAT_CUT_ES2                  ' ES2 cut�p�����[�^
    ''Public stCutZ As TRIM_DAT_CUT_Z                     ' Z cut(NOP)�p�����[�^

    Public Const RMax As Integer = 512
    Public stUCutPrm(RMax) As S_UCUTPARAM               ' U Cut �p�����[�^

    Public gStCutPosCorrData As CUTPOS_CORRECT_DATA

    '----- �����f�[�^(�g���~���O���ʃf�[�^)(VB��INtime) -----

    '' ''Public gwTrimResult(MaxCntResist) As UShort                  ' OK/NG����(0:�����{, 1:OK, 2:ITNG, 3:FTNG, 4:SKIP, 5:RATIO 6:NON��)
    '' ''Public gfInitialTest(MaxCntResist) As Double                 ' IT ��R�l
    '' ''Public gfFinalTest(MaxCntResist) As Double                   ' FT ��R�l
    '' ''Public gfTargetVal(MaxCntResist) As Double                   ' ���V�I�ڕW�l

    '---------------------------------------------------------------------------
    '   ��f�B���C(�f�B���C�g�����Q)�p�f�[�^(CHIP�p) V1.23.0.0�E
    '---------------------------------------------------------------------------
    ' �ިڲ���2���s����NG�����p�\���̌`����`
    Public Structure DELAYTRIM2_NGCNT_RESISTOR              ' �u���b�N���~��R���� 
        'Dim intNGCnt As Short                              ' ���g�p�̂��߃R�����g�� V1.23.0.0�E
        Dim intITHiNgCnt As Short                           ' IT HI NG��(1�u���b�N1�J�b�g�Ȃ̂ōő�ł�1���Z�b�g����� �ȉ����l)
        Dim intITLoNgCnt As Short                           ' IT LO NG��
        Dim intFTHiNgCnt As Short                           ' FT HI NG��
        Dim intFTLoNgCnt As Short                           ' FT LO NG��
        Dim intOverNgCnt As Short                           ' ���ް�ݼސ�
        Dim intTotalNGCnt As Short                          ' NG��
        Dim intTotalOkCnt As Short                          ' OK��
        Dim dblInitialTest As Double                        ' IT����l 
    End Structure

    ' �ިڲ���2���s����NG�����p�\���̌`����`(��R����(0 ORG)) V1.23.0.0�E
    Public Structure DELAYTRIM2_NGCNT
        Dim intNgFlag As Short                              ' NG�t���O(0=OK, 1=IT/FT NG)  
        Dim tNgCheck() As DELAYTRIM2_NGCNT_RESISTOR         ' NG�`�F�b�N�p�z��(��R����)

        ' �\���̂̏�����
        Public Sub Initialize(ByVal RegCount As Integer)
            ReDim tNgCheck(RegCount - 1)                    ' 0-255
        End Sub
    End Structure

    ' �ިڲ���2���s����NG�����p�\���̌`����`(�u���b�N��X,Y��) V1.23.0.0�E
    Public Structure DELAYTRIM2_INFO
        Dim NgAry(,) As DELAYTRIM2_NGCNT                    ' �u���b�N��X,Y��(0 ORG)

        ' �\���̂̏�����
        Public Sub Initialize(ByVal BlkXCount As Integer, ByVal BlkYCount As Integer)
            ReDim NgAry(BlkXCount - 1, BlkYCount - 1)       ' 0-255
        End Sub
    End Structure

    'V4.9.0.0�@��
    Public CutPointTable As LaserFront.Trimmer.DefTrimFnc.CUTPOINTCALC
    Public DEFAULT_CUTFILE As String = "C:\TRIM\CutPoint.cut"
    'V4.9.0.0�@��
    ' �ިڲ���2���s����NG�����p�\���� V1.23.0.0�E
    Public stDelay2 As DELAYTRIM2_INFO = Nothing

    '---------------------------------------------------------------------------
    '   �g���~���O�����p
    '---------------------------------------------------------------------------
    Public gRegistorCnt As Short                            ' ��R���i�}�[�L���O���܂�)
    Public gwCircuitCount As Short                          ' �T�[�L�b�g�� 
    'Public NowCntResist As Integer                         ' ���ۂ̒�R��

    Public gfITest_Par(999) As Double                       ' IT �덷
    Public gfITest_ParMag(999) As Double                    ' IT �덷
    Public gfFTest_Par(999) As Double                       ' FT �덷

    Public Const MaxCntCut As Short = 30                    ' �ő嶯Đ�
    'Public MaxCutNum As Integer                            ' �ő嶯Đ� 20 or 30

    'Public gfIndex2TestNum(MaxCntCut) As Short             ' Index2��R�l(��) V1.18.0.0�E
    Public gfIndex2TestNum(MaxCntCut) As UShort             ' Index2��R�l(��) V1.18.0.0�E UShort�łȂ��Ɛ������ݒ肳��Ȃ�
    Public gfIndex2ResultNum(128, MaxCntCut) As Short       ' Index2��R�l(��)
    Public gfIndex2Test(MaxCntCut) As Double                ' Index2��R�l(����l)
    Public gfIndex2Result(128, MaxCntCut) As Double         ' Index2��R�l(����l)

    Public gEsCutFileType As Short                          ' ES��Ă�ES2�œǂݍ��ނ��̔��f�׸� (0)ES (1)ES2

    '@@@@@(2011/02/04)@@@@
    '   ���쌟�؂��K�v�B
    '   VB.NET�̃}�l�[�W�h�R�[�h����ADLL�̃A���}�l�[�W�h�R�[�h���Ăяo����
    '   StructLayout�Ń}�[�V��������K�v������B
    '   ���L�͓��I�z��ɂȂ��Ă��邽�߁A���쌟�؂̏㏈�����C������
    Public Structure EsTestResultMeas
        Dim dblMeas() As Double                             ' ����l
    End Structure

    Public Structure EsTestResult
        Dim bEsExsit As Boolean                             ' ES��Ă̑����׸�(True:1���ȏ゠��, False:1�����Ȃ�)
        Dim iCutNo() As EsTestResultMeas                    ' ��Ĕԍ�
    End Structure

    'UPGRADE_WARNING: �z�� gtyESTestResult �̉����� 1 ���� 0 �ɕύX����܂����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' ���N���b�N���Ă��������B
    Public gtyESTestResult(999) As EsTestResult             ' ES����l���ʊi�[
    Public giTrimResult0x(999) As Short                     ' 1:High 2:Low 3:�C�R�[��
    'Public gflgResetStart As Boolean                        ' �������t���O
    'Public gLoggingStart As Boolean



    '----- �t���O -----
    Public g_blnTxTyExec As Boolean                         ' TX/TY���s�t���O
    Public g_blnTx2Ty2Exec As Boolean                       ' TX2/TY2���s�t���O

#End Region

#Region "�u���b�N�P�ʂ̃g���~���O�������s��"
    '''=========================================================================
    '''<summary>�u���b�N�P�ʂ̃g���~���O�������s��</summary>
    '''<param name="xmode">      (INP)�g�������[�h(x0 - x5)</param>
    '''<param name="iPowerCycle">(INP)���g��(Hz)</param>
    '''<param name="intCutIndex">(INP)����ް����ޯ��(1�`)(�ިڲ���2���L��)</param>
    '''<param name="intNgFlag">  (INP)��ۯ���NG��R�̗L��(0:����,1:�L��)(�ިڲ���2���L��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function TrimBlockExe(ByVal xmode As Short, ByVal iPowerCycle As Short, ByRef intCutIndex As Short, ByRef intNgFlag As Short) As Integer

        Dim iCutIndex As Short
        Dim iNgFlag As Short
        Dim iResIndex As Short = 1                                  ' V1.23.0.0�E
        Dim r As Integer
        Dim strMSG As String
        Dim bFlg As Boolean

        Try
            ResInfo.Initialize()

            iCutIndex = 0
            iNgFlag = 0

            ' �f�B���C�g����2�p�̃f�[�^��ݒ肷��(CHIP��)
            If (gTkyKnd = KND_CHIP) Then                            ' CHIP ? 
                If m_blnDelayCheck Then                             ' �ިڲ���2 ?
                    If xmode = 0 Then                               ' �~0���[�h�̎������O���׸ނ�ݒ肷��B
                        ' �ިڲ���2���s���́A��ۯ�No.����NG�׸ނ�ݒ肷��B(�O��Ă܂ł�NG������ꍇ��1���ݒ肳��Ă���B)
                        iNgFlag = intNgFlag                         ' ��ۯ���NG��R�̗L��(0:����,1:�L��)(�ިڲ���2���L��)
                    Else
                        iNgFlag = 0
                    End If
                    iCutIndex = intCutIndex                         ' ����ް����ޯ��(1�`)(�ިڲ���2���L��)
                End If
            End If

            '-------------------------------------------------------------------
            '   �u���b�N�P�ʂ̃g���~���O����
            '-------------------------------------------------------------------
            '----- V1.18.0.1�F�� -----
            ' ���[�U�Ǝ˒��̃V�O�i���^���[(���F)�_��(���ݸ�Ӱ�� = x0,x1,x5��)(���[���a����)
            If (gSysPrm.stCTM.giSPECIAL = customROHM) And ((xmode < 2) Or (xmode = 5)) Then
                Call EXTOUT1(EXTOUT_EX_YLW_ON, 0)
            End If
            ' �u���b�N�P�ʂ̃g���~���O����
            r = TRIMBLOCK(xmode, iPowerCycle, iResIndex, iCutIndex, iNgFlag)
            ' ���[�U�Ǝ˒��̃V�O�i���^���[(���F)����(���ݸ�Ӱ�� = x0,x1,x5)(���[���a����)
            If (gSysPrm.stCTM.giSPECIAL = customROHM) And ((xmode < 2) Or (xmode = 5)) Then
                Call EXTOUT1(0, EXTOUT_EX_YLW_ON)
            End If
            '----- V1.18.0.1�F�� -----
            '----- V1.13.0.0�J�� -----
            'r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)      ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)'###032
            ' ����΂�����o/�I�[�o���[�h���o�`�F�b�N
            bFlg = IS_CV_OverLoadErrorCode(r)
            If (bFlg = False) Then                                  ' ����΂�����o/�I�[�o���[�h���o�Ȃ烁�b�Z�[�W�\�����Ȃ� 
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' �G���[�Ȃ�G���[���^�[��(���b�Z�[�W�\���ς�)'###032
            End If
            '----- V1.13.0.0�J�� -----

            TrimBlockExe = r

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.TrimBlockExe() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            TrimBlockExe = cERR_TRAP
        End Try

    End Function

#End Region
    '----- V1.13.0.0�J�� -----
#Region "�߂�l������΂�����o/�I�[�o���[�h���o���`�F�b�N����"
    '''=========================================================================
    ''' <summary>�߂�l������΂�����o/�I�[�o���[�h���o���`�F�b�N����</summary>
    ''' <param name="RtnCode">(INP)TrimBlockExe()�̖߂�l</param>
    ''' <returns>True  = ����΂�����o/�I�[�o���[�h���o
    '''          False = ��L�ȊO
    ''' </returns>
    '''=========================================================================
    Public Function IS_CV_OverLoadErrorCode(ByVal RtnCode As Integer) As Boolean 'V1.23.0.0�F

        Dim r As Boolean
        Dim strMSG As String

        Try
            Select Case (RtnCode)
                Case ERR_MEAS_CV, ERR_MEAS_OVERLOAD, ERR_MEAS_REPROBING
                    r = True
                Case Else
                    r = False
            End Select
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "gwModule.IS_CV_OverLoadErrorCode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (False)
        End Try
    End Function
#End Region
    '----- V1.13.0.0�J�� -----
#Region "���O�o�͑Ώۂ̎擾"
    '''=========================================================================
    '''<summary>���O�o�͑Ώۂ�ݒ�t�@�C������擾���p�����[�^��ݒ肷��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TrimLogging_SetLogTarget(ByRef logFileFormat() As Integer, ByVal sectName As String, ByVal ObjUtil As Object)
        Dim cnt As Integer
        Dim strTargetName As String
        Dim filePath As String

        Try
            filePath = "c:\TRIM\LogTargetFile.ini"
            logFileFormat(0) = CInt(GetPrivateProfileString_S(sectName, cDATACOUNTcKEYNAME, filePath, "1"))

            For cnt = 1 To logFileFormat.Length() - 1
                '�ݒ�t�@�C���̌Ăяo��
                strTargetName = GetPrivateProfileString_S(sectName, (cnt).ToString(), filePath, "")

                '�擾��������r
                Select Case strTargetName
                    Case "DATE"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_DATE
                        logFileFormat(cnt) = LOGTAR.DATE             '#4.12.2.0�E
                    Case "MODE"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_MODE
                        logFileFormat(cnt) = LOGTAR.MODE             '#4.12.2.0�E
                    Case "LOTNO"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_LOTNO
                        logFileFormat(cnt) = LOGTAR.LOTNO            '#4.12.2.0�E

                    Case "BLOCKX"                   '#4.12.2.0�E
                        logFileFormat(cnt) = LOGTAR.BLOCKX
                    Case "BLOCKY"                   '#4.12.2.0�E 
                        logFileFormat(cnt) = LOGTAR.BLOCKY

                    Case "CIRCUIT"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_CIRCUIT
                        logFileFormat(cnt) = LOGTAR.CIRCUIT          '#4.12.2.0�E
                    Case "RESISTOR"
                        '#4.12.2.0�E                       logFileFormat(cnt) = LOGTAR_RESISTOR
                        logFileFormat(cnt) = LOGTAR.RESISTOR         '#4.12.2.0�E
                    Case "JUDGE"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_JUDGE
                        logFileFormat(cnt) = LOGTAR.JUDGE            '#4.12.2.0�E
                    Case "TARGET"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_TARGET
                        logFileFormat(cnt) = LOGTAR.TARGET           '#4.12.2.0�E
                    Case "INITIAL"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_INITIAL
                        logFileFormat(cnt) = LOGTAR.INITIAL          '#4.12.2.0�E
                    Case "FINAL"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_FINAL
                        logFileFormat(cnt) = LOGTAR.FINAL            '#4.12.2.0�E
                    Case "DEVIATION"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_DEVIATION
                        logFileFormat(cnt) = LOGTAR.DEVIATION        '#4.12.2.0�E
                    Case "UCUTPRMNO"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_UCUTPRMNO
                        logFileFormat(cnt) = LOGTAR.UCUTPRMNO        '#4.12.2.0�E
                    Case "END"
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_END
                        logFileFormat(cnt) = LOGTAR.END              '#4.12.2.0�E
                    Case ""
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_NOSET
                        logFileFormat(cnt) = LOGTAR.NOSET            '#4.12.2.0�E
                    Case Else
                        '#4.12.2.0�E                        logFileFormat(cnt) = LOGTAR_END
                        logFileFormat(cnt) = LOGTAR.END              '#4.12.2.0�E
                End Select

                'if 
            Next
        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)

        End Try
    End Sub
#End Region

#Region "�v���[�g�f�[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>�v���[�g�f�[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType">(INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="wreg">      (INP)��R��</param>
    '''<param name="bpofx">     (OUT)BP�ʒu�̾��X</param>
    '''<param name="bpofy">     (OUT)BP�ʒu�̾��Y</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataPlate(ByRef intTkyType As Short, ByRef wreg As Short, ByRef bpofx As Double, ByRef bpofy As Double) As Short

        Dim intNgUnit As Short
        Dim intNgStd As Short
        Dim intCDir As Short
        Dim r As Integer
        Dim strMSG As String
        Dim stTPLT As TRIM_PLATE_DATA                                   ' �v���[�g�f�[�^

        Try
            ' �v���[�g�f�[�^��ݒ肷��
            SendTrimDataPlate = cFRS_NORMAL                             ' Return�l = ����
            With stTPLT
                ' ��ެ���߲�Ĉʒu�̾��X,Y�ݒ�
                .fAdjustOffsetX = typPlateInfo.dblAdjOffSetXDir
                .fAdjustOffsetY = typPlateInfo.dblAdjOffSetYDir
                ' BP�ʒu�̾��X,Y�ݒ�
                .fBPOffsetX = typPlateInfo.dblBpOffSetXDir
                .fBPOffsetY = typPlateInfo.dblBpOffSetYDir
                bpofx = .fBPOffsetX
                bpofy = .fBPOffsetY

                ' �ިڲ���=2�̏ꍇ(CHIP�̂�)�ɂ́A��Đ�������/ڼ�Ӱ�ނ̗L���������s��
                .wDelayTrim = typPlateInfo.intDelayTrim                 ' �ިڲ��ю擾				
                If .wDelayTrim = 2 Then
                    If Not m_blnDelayCheck Then
                        ' �ިڲ�����Ŷ�Đ�������łȂ��ꍇ�A����ڼ�Ӱ�ނ̒�R�����݂���ꍇ�ɂ́AwDelayTrim=0�Ƃ��Ď��s����B
                        .wDelayTrim = 0
                    End If
                End If

                intNgUnit = typPlateInfo.intNgJudgeUnit                 ' NG����P�ʎ擾(0:BLOCK, 1:PLATE)
                If intNgUnit = 0 Then                                   ' �u���b�N�P�ʔ��肩�H
                    .wCircuitCnt = gwCircuitCount
                Else
                    .wCircuitCnt = 1
                End If
                intNgStd = typPlateInfo.intNgJudgeLevel                 ' NG JUDGEMENT RATE 0-100%
                .fNgCriterion = CDbl(intNgStd)                          ' NG���藦

                'If (giMarkingMode = 1) Then                             ' NG�}�[�L���O���� ?�@###012
                '    .wRegistCnt = wreg + 1                              ' 1000�ԕ���ݒ�
                'Else
                '    .wRegistCnt = wreg
                'End If
                .wRegistCnt = wreg                                      ' ###012

                '@@@@(2011/02/04)�ꎞ�I�ɏ�����ǉ�
                '   �T�[�L�b�g����1�̏ꍇ�́A' �T�[�L�b�g����R����R���͑S��R���Ƃ���
                If (.wCircuitCnt <= 1) Then
                    .wResCntInCrt = wreg                                ' �T�[�L�b�g����R��
                Else                                                    ' ###164
                    .wResCntInCrt = typPlateInfo.intResistCntInGroup    ' ###164 �T�[�L�b�g����R��
                End If                                                  ' ###164

                .fZStepPos = typPlateInfo.dblZStepUpDist                ' Z���ï��&��߰Ĉʒu
                .fZTrimPos = typPlateInfo.dblZOffSet                    ' Z������Ĉʒu(�̾�Ĉʒu)

                intCDir = typPlateInfo.intResistDir                     ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
                If (0 = intCDir) Then
                    .fReProbingX = 0
                    .fReProbingY = typPlateInfo.dblRetryProbeDistance   ' ����۰��ݸ�Y�ړ���
                Else
                    .fReProbingY = 0
                    .fReProbingX = typPlateInfo.dblRetryProbeDistance   ' ����۰��ݸ�X�ړ���
                End If
                .wReProbingCnt = typPlateInfo.intRetryProbeCount        ' ����۰��ݸމ�

                .wInitialOK = typPlateInfo.intInitialOkTestDo           ' �Ƽ��OKýėL��
                .wNGMark = typPlateInfo.intNGMark                       ' NGϰ�ݸނ���/���Ȃ�
                .w4Terminal = typPlateInfo.intOpenCheck                 ' 4�[�q�������������/���Ȃ�

                ' ۷�ݸ�Ӱ��(0:���Ȃ�, 1:INITIAL TEST, 2:FINAL TEST, 3:INITIAL + FINAL)	
                If (gSysPrm.stLOG.giLoggingMode = 0) Then
                    .wLogMode = gSysPrm.stLOG.giLoggingMode
                Else
                    .wLogMode = gSysPrm.stLOG.giLoggingDataKind
                End If

                gLogMode = .wLogMode                                    ' ۷�ݸ�Ӱ�ޑޔ� ###150
                .bTrimCutEnd = True                                     ' �J�b�g�I�t�ڕW�ő�l�ɓ��B������J�b�g���I������iTRUE�j/���Ȃ��iFALSE�j
                '----- V1.13.0.0�A�� -----
                .intNgJudgeStop = typPlateInfo.intNgJudgeStop           ' NG���莞��~(OVRERLOAD/CV�G���[�p)
                .dblReprobeVar = typPlateInfo.dblReprobeVar             ' ����۰��ݸނ΂����
                .dblReprobePitch = typPlateInfo.dblReprobePitch         ' ����۰��ݸ��߯�
                .dblLwPrbStpDwDist = typPlateInfo.dblLwPrbStpDwDist     ' �����v���[�u�X�e�b�v���~����
                .dblLwPrbStpUpDist = typPlateInfo.dblLwPrbStpUpDist     ' �����v���[�u�X�e�b�v�㏸����
                '----- V1.13.0.0�A�� -----
            End With

            ' �v���[�g�f�[�^��INtime���ɑ��M����
            r = TRIMDATA_PLATE(stTPLT, intTkyType)
            If (r <> cFRS_NORMAL) Then                                  ' ���M���s ?
                strMSG = "Ptate Data Send Error(r=" + r.ToString + ")"
                Call Form1.System1.TrmMsgBox(gSysPrm, "Ptate Data Send Error 1    ", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataPlate = 1
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataPlate() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataPlate = cERR_TRAP
        End Try

    End Function
#End Region

#Region "�v���[�g�f�[�^(GP-IB�ݒ�p�ް�)��INtime���ɑ��M����"
    '''=========================================================================
    ''' <summary>�v���[�g�f�[�^(GP-IB�ݒ�p�ް�)��INtime���ɑ��M����</summary>
    ''' <param name="intTkyType">(INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    ''' <param name="wreg">      (I/O)��R��</param>
    ''' <param name="Type">      (INP)�^�C�v(0=GPIB, 1=GBIB2) ###229</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataPlate2(ByRef intTkyType As Short, ByRef wreg As Short, ByVal Type As Integer) As Short ' ###229
        '   Public Function SendTrimDataPlate2(ByRef intTkyType As Short, ByRef wreg As Short) As Short

        Dim s As String
        Dim n As Short
        Dim nn As Short
        Dim r As Integer
        ''Dim stTGPI As TRIM_DAT_GPIB                                    ' GPIB�ݒ�f�[�^
        'stTGPI.prmGPIB.Initialize()
        Dim strMSG As String

        Try
            ' �v���[�g�f�[�^(GP-IB�ݒ�p�ް�)��ݒ肷��
            SendTrimDataPlate2 = cFRS_NORMAL                                ' Return�l = ����
            If (Type = 0) Then                                              ' ###229

                With stTGPI
                    .wGPIBmode = typPlateInfo.intGpibCtrl                   ' GP-IB����(0:���Ȃ� 1:����(ADEX�p))
                    '----- ###229�� -----
                    ' �V�X�p����GP-IB����Ȃ�����GP-IB���䂠��(2:�ėp)�Ȃ�GP-IB����(ADEX)�Ȃ��Ƃ��� 
                    If (gSysPrm.stCTM.giGP_IB_flg = 0) Or (gSysPrm.stCTM.giGP_IB_flg = 2) Then
                        .wGPIBmode = 0                                      ' GP-IB����Ȃ��Ƃ��� 
                    End If
                    '----- ###229�� -----
                    .wDelim = typPlateInfo.intGpibDefDelimiter              ' �����(0:CR+LF 1:CR 2:LF 3:NONE)
                    .wTimeout = typPlateInfo.intGpibDefTimiout              ' ��ѱ��(0�`1000)
                    .wAddress = typPlateInfo.intGpibDefAdder                ' �@����ڽ(0�`30)
                    .wMeasurementMode = typPlateInfo.intGpibMeasMode        ' ����Ӱ��(0:��΁A1:�΍�)

                    ' �\��(MAX6byte)(�O�ׁ̈A���������Ă���)
                    s = ""
                    For n = 0 To (6 - 1)
                        .strI(n) = 0 : Next
                    nn = Len(s)
                    If nn <= 6 Then
                        s = s & Space(6 - nn)
                    End If
                    For n = 1 To 6
                        .wReserve(n - 1) = Asc(Mid(s, n, 1))
                    Next

                    ' �����������(MAX40byte)
                    s = typPlateInfo.strGpibInitCmnd1 & typPlateInfo.strGpibInitCmnd2
                    For n = 0 To (40 - 1)
                        .strI(n) = 0 : Next
                    nn = Len(s)
                    If nn <= 40 Then
                        s = s & Space(40 - nn)
                    End If
                    For n = 1 To 40
                        .strI(n - 1) = Asc(Mid(s, n, 1))
                    Next

                    ' �ض޺����(MAX10byte)
                    s = typPlateInfo.strGpibTriggerCmnd
                    For n = 0 To (10 - 1)
                        .strT(n) = 0 : Next
                    nn = Len(s)
                    If nn <= 10 Then
                        s = s & Space(10 - nn)
                    End If
                    For n = 1 To 10
                        .strT(n - 1) = Asc(Mid(s, n, 1))
                    Next

                End With

                ' �v���[�g�f�[�^(GP-IB�ݒ�p�ް�)��INtime���ɑ��M���� ###002
                r = TRIMDATA_GPIB(stTGPI, intTkyType)
                If (r <> cFRS_NORMAL) Then                                  ' ���M���s ?
                    Call Form1.System1.TrmMsgBox(gSysPrm, "GP-IB Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataPlate2 = 8
                    Exit Function
                End If

                '----- ###229�� -----
            Else
                ' �v���[�g�f�[�^(GP-IB�ݒ�p�ް�)��ݒ肷��
                With stTGPI2
                    .wGPIBflg = bGpib2Flg                                  ' GP-IB����(0:���Ȃ� 1:����)
                    'V5.0.0.6�Q .wGPIBdlm = typGpibInfo.wDelim                            ' �����(0:CR+LF 1:CR 2:LF 3:NONE)
                    'V5.0.0.6�Q INTIME�ȍ~�AIDELIM�AGpibSetDelim�́A�����(0:NONE 1:CR+LF 2:CR 3:LF )�̏��ԂȂ̂ł����ŏC������B
                    .wGPIBdlm = typGpibInfo.wDelim + 1                      'V5.0.0.6�Q
                    If .wGPIBdlm = 4 Then                                   'V5.0.0.6�Q
                        .wGPIBdlm = 0                                       'V5.0.0.6�Q
                    End If                                                  'V5.0.0.6�Q
                    .wGPIBtout = typGpibInfo.wTimeout                        ' ��ѱ��(1�`32767)
                    .wGPIBdev = typGpibInfo.wAddress                        ' �@����ڽ(0�`30)
                    .wEOI = typGpibInfo.wEOI                                ' EOI(0:�g�p���Ȃ�, 1:�g�p����)
                    .wPause1 = typGpibInfo.wPause1                          ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
                    .wPause2 = typGpibInfo.wPause2                          ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
                    .wPause3 = typGpibInfo.wPause3                          ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
                    .wPauseT = typGpibInfo.wPauseT                          ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
                    .wRsv = typGpibInfo.wRev                                ' �\��
                    ' GPIB������p�����[�^��ݒ肷��
                    SetGpibByteParam(typGpibInfo.strI, .cGPIBinit, PLT_GPIB2_INI_SZ)         ' �������R�}���h������1
                    SetGpibByteParam(typGpibInfo.strI2, .cGPIBini2, PLT_GPIB2_INI_SZ)       ' �������R�}���h������2
                    SetGpibByteParam(typGpibInfo.strI3, .cGPIBini3, PLT_GPIB2_INI_SZ)       ' �������R�}���h������3
                    SetGpibByteParam(typGpibInfo.strT, .cGPIBtriger, PLT_GPIB2_TRG_SZ)         ' �g���K�[������
                    SetGpibByteParam(typGpibInfo.strName, .cGPIBName, PLT_GPIB2_NAM_SZ)   ' �@�햼������
                    SetGpibByteParam(typGpibInfo.wReserve, .cRsv, PLT_GPIB2_RSV_SZ) ' �\��������
                End With

                ' �v���[�g�f�[�^(GP-IB�ݒ�p(�ėp)�ް�)��INtime���ɑ��M����
                r = TRIMDATA_GPIB2(stTGPI2, intTkyType)
                If (r <> cFRS_NORMAL) Then                                  ' ���M���s ?
                    Call Form1.System1.TrmMsgBox(gSysPrm, "GP-IB2 Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataPlate2 = 9
                    Exit Function
                End If
            End If
            '----- ###229�� -----

            '----- V6.1.1.0�D�� -----
            ' �O�������[�ؑ֋@�p(GP-IB�ݒ�p�f�[�^)��INtime���ɑ��M����
            r = SendTrimDataGpib3(intTkyType)
            If (r <> cFRS_NORMAL) Then
                SendTrimDataPlate2 = r
                Exit Function
            End If
            '----- V6.1.1.0�D�� -----

            'If giMarkingMode = 1 Then                                   ' NG�}�[�L���O���� ?�@###012
            '    wreg = wreg + 1                                         ' �}�[�L���O��񕪑��₷
            'End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataPlate2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataPlate2 = cERR_TRAP
        End Try

    End Function
#End Region

    '----- V6.1.1.0�D�� -----
#Region "�O�������[�ؑ֋@�p(GP-IB�ݒ�p�f�[�^)��INtime���ɑ��M����"
    '''=========================================================================
    ''' <summary>�O�������[�ؑ֋@�p(GP-IB�ݒ�p�f�[�^)��INtime���ɑ��M����</summary>
    ''' <param name="intTkyType">(INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataGpib3(ByRef intTkyType As Short) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �O�������[�ؑ֋@�p(GP-IB�ݒ�p�f�[�^)��ݒ肷��
            SendTrimDataGpib3 = cFRS_NORMAL                             ' Return�l = ����

            ' �V�X�p����GP-IB����Ȃ��̎����M���Ȃ��FGP-IB���䂠��(1:ADEX,2:�ėp)
            If gSysPrm.stCTM.giGP_IB_flg <> 2 Then
                Exit Function
            End If
            With stTGPI3
                .wScannerUse = typGpibInfo.wScannerUse                  ' �X�L���i�g�p�L��(0:�g�p���Ȃ�, 1:�g�p����)
                .wScannerAddress = typGpibInfo.wScannerAddress          ' �@��A�h���X(0�`30)
                .wScannerTimeout = typGpibInfo.wScannerTimeout          ' �^�C���A�E�g�l(100ms�P��)
                .wScannerPauseT = typGpibInfo.wScannerPauseT            ' �g���K�R�}���h���M��|�[�Y����(1�`32767msec)
            End With

            ' �v���[�g�f�[�^(GP-IB�ݒ�p(�ėp)�ް�)��INtime���ɑ��M����
            r = TRIMDATA_GPIB3(stTGPI3, intTkyType)
            If (r <> cFRS_NORMAL) Then                                  ' ���M���s ?
                'Call Form1.System1.TrmMsgBox(gSysPrm, "�X�L���i�[�̏��������G���[�I�����܂����B", MsgBoxStyle.OkOnly, TITLE_4)
                Call Form1.System1.TrmMsgBox(gSysPrm, "GP-IB3 Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataGpib3 = 10
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataGpib3() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataGpib3 = cERR_TRAP
        End Try

    End Function
#End Region
    '----- V6.1.1.0�D�� -----
    '----- ###229�� -----
#Region "GPIB������p�����[�^��ݒ肷��"
    '''=========================================================================
    '''<summary>GPIB������p�����[�^��ݒ肷��</summary>
    '''<param name="InpData"> (INP)���̓f�[�^</param>
    ''' <param name="OutData">(OUT)�o�̓f�[�^</param>
    ''' <param name="OutSz">  (OUT)�o�̓f�[�^�T�C�Y</param>
    '''=========================================================================
    Private Sub SetGpibByteParam(ByRef InpData As String, ByRef OutData() As Byte, ByVal OutSz As Integer)

        Dim Idx As Integer
        Dim Sz As Integer
        Dim strDat As String
        Dim strMSG As String

        Try
            ' �o�̓f�[�^��null�N���A
            strDat = InpData
            For Idx = 0 To OutSz - 1
                OutData(Idx) = 0
            Next Idx

            ' ���̓f�[�^�̌���ɋ󔒂�ݒ肷�� 
            Sz = Len(InpData)
            If (Sz <= OutSz) Then
                strDat = strDat + Space(OutSz - Sz)
            End If

            ' �o�̓f�[�^��1�����Âݒ肷��
            For Idx = 1 To OutSz
                OutData(Idx - 1) = Asc(Mid(strDat, Idx, 1))
            Next Idx

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SetGpibByteParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###229�� ----
#Region "��R�f�[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>��R�f�[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="i">          (INP)��R�f�[�^�E�C���f�b�N�X</param>
    '''<param name="intRegIndex">(OUT)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="wreg">       (INP)��R��</param>  
    '''<param name="RNO">        (OUT)��R�ԍ�</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataRegistor(ByRef intTkyType As Short, ByRef i As Short, _
                                         ByRef intRegIndex As Short, ByRef wreg As Short, ByRef RNO As Short) As Short
        'Public Function SendTrimDataRegistor(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                     ByRef intRegIndex As Short, ByRef wreg As Short, ByRef RNO As Short) As Short


        Dim r As Integer
        Dim stTRES As New TRIM_RESISTOR_DATA()                         ' ��R�f�[�^
        Dim strMSG As String

        Try
            '-----------------------------------------------------------------------
            '   ��R�f�[�^��ݒ肷��
            '-----------------------------------------------------------------------
            SendTrimDataRegistor = cFRS_NORMAL                  ' Return�l = ����
            stTRES.Initialize()

            '' CHIP,NET�̂݉��L�͑���
            'If giMarkingMode = 1 And i = wreg Then              ' �}�[�L���O����ŁA���ޯ�����Ō�̏ꍇ�@###012
            '    intRegIndex = 1000                              ' �z��̓}�[�L���O���w���悤�ɂ���
            'Else
            '    intRegIndex = i
            'End If
            intRegIndex = i                                     ' ###012

            With stTRES
                'V4.7.0.0-22                If (gTkyKnd = KND_TKY) Then
                If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                    '-----------------------------------------------------------
                    '   TKY��
                    '-----------------------------------------------------------
                    Dim isBaseExist As Boolean
                    Dim n As Integer

                    ' ���V�I�g���~���O ?
                    .wRatioMode = typResistorInfoArray(intRegIndex).intTargetValType    ' �ڕW�l�w��i0:��Βl,1:���V�I,2:�v�Z���j
                    .wBaseReg = typResistorInfoArray(intRegIndex).intBaseResNo          ' �x�[�X��R�ԍ�
                    If (typResistorInfoArray(intRegIndex).intTargetValType = 1) Or _
                       ((typResistorInfoArray(intRegIndex).intTargetValType >= 3) And (typResistorInfoArray(intRegIndex).intTargetValType <= 9)) Then
                        ' �ڕW�l�w�肪���V�I�̏ꍇ�A 
                        ' �x�[�X��R�����V�I�w��łȂ����`�F�b�N����
                        isBaseExist = False
                        For n = 1 To wreg                       ' ��R�����J��Ԃ� 
                            If typResistorInfoArray(n).intResNo = typResistorInfoArray(intRegIndex).intBaseResNo Then   '�x�[�X��R?
                                If typResistorInfoArray(n).intTargetValType Then
                                    Call Form1.System1.TrmMsgBox(gSysPrm, "Base resistor is also ratio mode. Base reg =" + Str(n), vbExclamation + vbOKOnly, gAppName)
                                    SendTrimDataRegistor = 2    ' Return�l = 2(�x�[�X��R�����V�I�w��) 
                                    Exit Function
                                End If
                                isBaseExist = True
                                Exit For
                            End If
                        Next
                        If isBaseExist = False Then             ' �x�[�X��R�Ȃ� ?
                            Call Form1.System1.TrmMsgBox(gSysPrm, "Ratio trimming base resistor could not be found." + _
                                                           "PR1=" + Str(typResistorInfoArray(intRegIndex).intResNo) + _
                                                           ",PR8=" + Str(typResistorInfoArray(intRegIndex).intBaseResNo), _
                                                  vbExclamation + vbOKOnly, gAppName)
                            SendTrimDataRegistor = 3            ' Return�l = 3(���V�I�w�莞�x�[�X��R�Ȃ�) 
                            Exit Function
                        End If
                        .fTargetVal = typResistorInfoArray(intRegIndex).dblTrimTargetVal

                    ElseIf typResistorInfoArray(intRegIndex).intTargetValType = 2 Then
                        ' �ڕW�l�w�肪2(�v�Z��)�̏ꍇ�A���V�I���[�h�Q�v�Z����INtime���ɑ��M����
                        Dim sRatio2 As S_RATIO2EXP
                        Dim strExp As String

                        sRatio2.RNO = typResistorInfoArray(intRegIndex).intResNo
                        strExp = typResistorInfoArray(intRegIndex).strRatioTrimTargetVal
                        strExp = UCase(strExp)
                        If Len(strExp) > 100 Then
                            MsgBox("check Expression length", vbOKOnly, "DEBUG")
                            strExp = Left(strExp, 100)
                        End If
                        ' ���V�I���[�h�Q�v�Z����INtime���ɑ��M����
                        sRatio2.strExp = strExp
                        Call RATIO2EXP(sRatio2.RNO, sRatio2.strExp)
                        .fTargetVal = 0.0#                                ' �g���~���O�ڕW�l
                    Else
                        ' �ڕW�l�w�肪��Βl�ɂ��g���~���O�̏ꍇ�A�g���~���O�ڕW�l��ݒ肷��
                        .fTargetVal = typResistorInfoArray(intRegIndex).dblTrimTargetVal
                    End If

                    ' �J�b�g�ʒu�␳�@�\�L�� ?
                    If (Form1.stFNC(F_CUTPOS).iDEF = 1) Then
                        .wCorrectFlg = typResistorInfoArray(i).intCutReviseMode    ' �J�b�g�ʒu�␳�t���O(0:�␳���Ȃ�, 1:�␳����)
                    Else
                        .wCorrectFlg = 0                                           ' �J�b�g�ʒu�␳���Ȃ�
                    End If

                ElseIf (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then
                    '-----------------------------------------------------------
                    '   CHIP/NET��
                    '-----------------------------------------------------------
                    .fTargetVal = CDbl(typResistorInfoArray(intRegIndex).dblTrimTargetVal)            ' �g���~���O�ڕW�l(ohm)
                    .fCutMag = CDbl(typResistorInfoArray(intRegIndex).dblCutOffRatio)           ' �؏グ�{
                End If

                '---------------------------------------------------------------
                '   TKY/CHIP/NET����
                '---------------------------------------------------------------
                .wResNo = typResistorInfoArray(intRegIndex).intResNo                ' ��R�ԍ�(1-999=�g���~���O, 1000-9999=�}�[�L���O)
                .wMeasMode = typResistorInfoArray(intRegIndex).intResMeasMode       ' ���胂�[�h(0�F��R�A1�F�d��)
                .wMeasType = typResistorInfoArray(intRegIndex).intResMeasType       ' ����^�C�v(0:���� ,1:�����x�A�Q�F�O��)
                '----- ###229�� -----
                ' �V�X�p����GP-IB���䂠��(2:�ėp)�ő���^�C�v���O���Ȃ�GP-IB����(�ėp)�t���O�𐧌䂠��Ƃ���
                If (gSysPrm.stCTM.giGP_IB_flg = 2) And (.wMeasType = 2) Then
                    bGpib2Flg = 1                                                   ' GP-IB����(�ėp)�t���O(0=����Ȃ�, 1=���䂠��)
                End If
                '----- ###229�� -----
                .wCircuit = typResistorInfoArray(intRegIndex).intCircuitGrp         ' �T�[�L�b�g(��R��������T�[�L�b�g�ԍ�)
                .wHiProbNo = typResistorInfoArray(intRegIndex).intProbHiNo          ' �n�C���v���[�u�ԍ�
                .wLoProbNo = typResistorInfoArray(intRegIndex).intProbLoNo          ' ���[���v���[�u�ԍ�
                .w1stAG = typResistorInfoArray(intRegIndex).intProbAGNo1            ' ��1�A�N�e�B�u�K�[�h�ԍ�
                .w2ndAG = typResistorInfoArray(intRegIndex).intProbAGNo2            ' ��2�A�N�e�B�u�K�[�h�ԍ�
                .w3rdAG = typResistorInfoArray(intRegIndex).intProbAGNo3            ' ��3�A�N�e�B�u�K�[�h�ԍ�
                .w4thAG = typResistorInfoArray(intRegIndex).intProbAGNo4            ' ��4�A�N�e�B�u�K�[�h�ԍ�
                .w5thAG = typResistorInfoArray(intRegIndex).intProbAGNo5            ' ��5�A�N�e�B�u�K�[�h�ԍ�
                '----- ###239��-----
                '.dwExBits = CLng(typResistorInfoArray(intRegIndex).strExternalBits)' External Bits(���ꂾ��"11110000"��0x11110000�ƂȂ�(��������0x00f0))
                strMSG = typResistorInfoArray(intRegIndex).strExternalBits.Trim()   ' �O��̋󔒂��폜
                .dwExBits = Convert.ToUInt32(strMSG, 2)                             ' External Bits(2�i��������𐔒l�ɕϊ�)
                '----- ###239��-----
                .wPauseTime = typResistorInfoArray(intRegIndex).intPauseTime        ' External Bits �o�͌�|�[�Y�^�C��
                .wSlope = typResistorInfoArray(intRegIndex).intSlope                ' �X���[�v(1:�d��+�X���[�v, 2:�d��-�X���[�v, 4:��R+�X���[�v, 5:��R�}�C�i�X�X���[�v) 
                ' IT�Я�ʲ/۰(%)
                .fITLimitH = CDbl(typResistorInfoArray(intRegIndex).dblInitTest_HighLimit)
                .fITLimitL = CDbl(typResistorInfoArray(intRegIndex).dblInitTest_LowLimit)
                ' FT�Я�ʲ/۰(%)
                .fFTLimitH = CDbl(typResistorInfoArray(intRegIndex).dblFinalTest_HighLimit)
                .fFTLimitL = CDbl(typResistorInfoArray(intRegIndex).dblFinalTest_LowLimit)
                .wInitialOK = typResistorInfoArray(intRegIndex).intInitialOkTestDo   '�����n�j����(0:���Ȃ�,1:����) V5.0.0.6�H
                .wCutCnt = typResistorInfoArray(intRegIndex).intCutCount            ' �J�b�g��
                '----- V1.13.0.0�A�� -----
                .intCvMeasNum = typResistorInfoArray(intRegIndex).intCvMeasNum      ' CV �ő呪���
                .intCvMeasTime = typResistorInfoArray(intRegIndex).intCvMeasTime    ' CV �ő呪�莞��(ms) 
                .dblCvValue = typResistorInfoArray(intRegIndex).dblCvValue          ' CV CV�l 
                .intOverloadNum = typResistorInfoArray(intRegIndex).intOverloadNum  ' ���ް۰�� ��
                .dblOverloadMin = typResistorInfoArray(intRegIndex).dblOverloadMin  ' ���ް۰�� �����l
                .dblOverloadMax = typResistorInfoArray(intRegIndex).dblOverloadMax  ' CV ���ް۰�� ����l
                '----- V1.13.0.0�A�� -----
                'V4.0.0.0-39�@��
                .wPauseTimeFT = typResistorInfoArray(intRegIndex).wPauseTimeFT      'FT�O�̃|�[�Y�^�C�� 
                'V4.0.0.0-39�@��
                ''V4.8.0.0�A
                .dblInsideEndChgRate = typResistorInfoArray(intRegIndex).dblInsideEndChgRate
                .wInsideEndChkCount = typResistorInfoArray(intRegIndex).intInsideEndChkCount
                ''V4.8.0.0�A

                'V6.0.0.2�@��
                .wModeSelect = typResistorInfoArray(intRegIndex).intULowResOutputType                 '///< �d�����[�h�F�O�F
                .wCurSel = typResistorInfoArray(intRegIndex).intULowResCurrentVal                     '///< ����d���F0:�W���A1:�PA�A2:2A�A3:3A
                'V6.0.0.2�@��

                RNO = .wResNo                                                       ' ��R�ԍ���Ԃ�
            End With

            '-----------------------------------------------------------------------
            '   ��R�f�[�^��INtime���ɑ��M����
            '-----------------------------------------------------------------------
            r = TRIMDATA_RESISTOR(stTRES, i)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "Resistor Data Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataRegistor = 5
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataRegistor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataRegistor = cERR_TRAP
        End Try

    End Function
#End Region

#Region "���H�����̐ݒ�"
    '''=========================================================================
    '''<summary>���H������ݒ肷��</summary>
    '''<param name="stCutCon">(OUT)���H�����ݒ�\����</param>
    '''<param name="resNo">   (INP)��R�f�[�^�E�C���f�b�N�X</param>
    '''<param name="cutNo">   (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function SetCutConditionData(ByRef stCutCon() As S_CUT_CONDITION, ByVal resNo As Integer, ByVal cutNo As Integer) As Integer

        Dim CndNum As Integer
        Dim Idx As Integer
        Dim strCutType As String
        Dim strMSG As String

        Try
            ' ��������
            SetCutConditionData = cFRS_NORMAL
            strCutType = typResistorInfoArray(resNo).ArrCut(cutNo).strCutType.Trim()

            ' ���H�����ݒ�\���̂�����������
            'For Idx = 0 To (cCNDNUM - 1)
            For Idx = 0 To (MaxCndNum - 1)  'V5.0.0.8�@
                stCutCon(Idx).cutSetNo = 0                          ' ���H�����ԍ�
                'stCutCon(Idx).cutSpd = 0.0                          ' �J�b�g�X�s�[�h
                'stCutCon(Idx).cutQRate = 0.0                        ' �J�b�gQ���[�g
                stCutCon(Idx).cutSpd = 1.0                          ' �J�b�g�X�s�[�h ��###004 �ő̃��[�U��INTime���Ńp�����[�^�G���[�ƂȂ邽��
                stCutCon(Idx).cutQRate = 1.0                        ' �J�b�gQ���[�g  ��###004 
                stCutCon(Idx).bUse = False                          ' INTRIM���ł̂ݎg�p
            Next Idx

            '   '-------------------------------------------------------------------
            '   '   ���H�����\���̂�ݒ肷��(FL��)
            '   '-------------------------------------------------------------------
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                ' FL���͉��H�����ԍ��e�[�u��������H�����ԍ���Q���[�g��ݒ肷��(�J�b�g�X�s�[�h�̓f�[�^����ݒ�)
                ' ���H����1�͑S�J�b�g�������ɐݒ肷��
                CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1)
                stCutCon(CUT_CND_L1).cutSetNo = CndNum
                stCutCon(CUT_CND_L1).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                stCutCon(CUT_CND_L1).cutQRate = stCND.Freq(CndNum)

                ' ���H����2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
                ' HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                   (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                   (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0�@
                    ' ���H����2
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L2)
                    stCutCon(CUT_CND_L2).cutSetNo = CndNum
                    stCutCon(CUT_CND_L2).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L2).cutQRate = stCND.Freq(CndNum)
                End If

                ' ���H����3��HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0�@
                    ' ���H����3
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L3)
                    stCutCon(CUT_CND_L3).cutSetNo = CndNum
                    stCutCon(CUT_CND_L3).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed3
                    stCutCon(CUT_CND_L3).cutQRate = stCND.Freq(CndNum)
                    '----- V1.22.0.0�@�� -----
                    ' U�J�b�g/U�J�b�g(���g���[�X)���͑��x3��ݒ肷��(�f�[�^�ҏWV1.19.0.0�ȍ~�őΉ�)
                    ''----- ###194�� -----
                    '' U�J�b�g���͑��x3�ɑ��x1��ݒ肷��
                    'If (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0�@
                    '    stCutCon(CUT_CND_L3).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                    'End If
                    ''----- ###194�� -----
                    '----- V1.22.0.0�@�� -----
                End If

                ' ���H����4�͌���͖��g�p(�\��)

                ' ���H����5�`8�̓��^�[��/���g���[�X�p 
                ' ���H����5(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)��
                If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                   (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                    ' ���H����5�̏����ԍ����J�b�g�f�[�^�̉��H����2���ݒ肷��
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(1)               ' V4.2.0.0�A
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1_RET)   ' V4.2.0.0�A
                    stCutCon(CUT_CND_L1_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L1_RET).cutQRate = stCND.Freq(CndNum)
                End If

                ' ���H����5,6(L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)��
                If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                    ' ���H����5�̏����ԍ����J�b�g�f�[�^�̉��H����3���ݒ肷��
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(2)               ' V4.2.0.0�A
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1_RET)   ' V4.2.0.0�A
                    stCutCon(CUT_CND_L1_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed3
                    stCutCon(CUT_CND_L1_RET).cutQRate = stCND.Freq(CndNum)
                    ' ���H����6�̏����ԍ����J�b�g�f�[�^�̉��H����4���ݒ肷��
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(3)               ' V4.2.0.0�A
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L2_RET)   ' V4.2.0.0�A
                    stCutCon(CUT_CND_L2_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed4
                    stCutCon(CUT_CND_L2_RET).cutQRate = stCND.Freq(CndNum)
                End If

                '----- V1.22.0.0�@�� -----
                ' ���H����5,6,7��U�J�b�g(���g���[�X)���ɐݒ肷��
                If (strCutType = CNS_CUTP_Ut) Then
                    ' ���H����5�̏����ԍ����J�b�g�f�[�^�̉��H����4���ݒ肷��
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(3)               ' V4.2.0.0�A
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1_RET)   ' V4.2.0.0�A
                    stCutCon(CUT_CND_L1_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed4
                    stCutCon(CUT_CND_L1_RET).cutQRate = stCND.Freq(CndNum)
                    ' ���H����6�̏����ԍ����J�b�g�f�[�^�̉��H����5���ݒ肷��
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(4)               ' V4.2.0.0�A
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L2_RET)   ' V4.2.0.0�A
                    stCutCon(CUT_CND_L2_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed5
                    stCutCon(CUT_CND_L2_RET).cutQRate = stCND.Freq(CndNum)
                    ' ���H����7�̏����ԍ����J�b�g�f�[�^�̉��H����6���ݒ肷��
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(5)               ' V4.2.0.0�A
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L3_RET)   ' V4.2.0.0�A
                    stCutCon(CUT_CND_L3_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L3_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed6
                    stCutCon(CUT_CND_L3_RET).cutQRate = stCND.Freq(CndNum)
                End If
                '----- V1.22.0.0�@�� -----

                ' ���H����8�͌���͖��g�p(�\��)

                '-------------------------------------------------------------------
                '   ���H�����\���̂�ݒ肷��(FL�ȊO)
                '   FL�łȂ����̓J�b�g�f�[�^����J�b�g�X�s�[�h/Q���[�g��ݒ肷��
                '-------------------------------------------------------------------
            Else
                ' �ʏ�J�b�g�p(���H����1�`4)
                ' ���H����1�͑S�J�b�g�������ɐݒ肷��
                stCutCon(CUT_CND_L1).cutSetNo = 0                                           ' ���H����1
                stCutCon(CUT_CND_L1).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                stCutCon(CUT_CND_L1).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate

                ' ���H����2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
                ' HOOK�J�b�g, U�J�b�g���ɐݒ肷�� '###023 
                If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                   (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                   (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0�@
                    ' ���H����2
                    stCutCon(CUT_CND_L2).cutSetNo = 0
                    stCutCon(CUT_CND_L2).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                    stCutCon(CUT_CND_L2).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate
                End If

                ' ���H����3��HOOK�J�b�g, U�J�b�g���ɐݒ肷�� '###023 
                If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0�@
                    ' ���H����3
                    stCutCon(CUT_CND_L3).cutSetNo = 0
                    stCutCon(CUT_CND_L3).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                    stCutCon(CUT_CND_L3).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate
                End If

                ' ���H����4�͌���͖��g�p(�\��)

                ' ���^�[��/���g���[�X�p(���H����5�`6) '###023
                ' ���H����5��(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)���ɐݒ肷��
                If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                   (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Or _
                   (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                    ' ���H����5���J�b�g�f�[�^���ݒ肷��
                    stCutCon(CUT_CND_L1_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L1_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    ' ���H����6���J�b�g�f�[�^���ݒ肷��
                    stCutCon(CUT_CND_L2_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L2_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                End If

                '----- V1.22.0.0�@�� -----
                ' ���H����5,6,7��(U�J�b�g(��ڰ�)���ɐݒ肷��
                If (strCutType = CNS_CUTP_Ut) Then
                    ' ���H����5���J�b�g�f�[�^���ݒ肷��(���x2��Q���[�g2��ݒ肷��)
                    stCutCon(CUT_CND_L1_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L1_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    ' ���H����6���J�b�g�f�[�^���ݒ肷��(���x2��Q���[�g2��ݒ肷��)
                    stCutCon(CUT_CND_L2_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L2_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    ' ���H����7���J�b�g�f�[�^���ݒ肷��(���x2��Q���[�g2��ݒ肷��)
                    stCutCon(CUT_CND_L3_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L3_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L3_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    '----- V1.22.0.0�@�� -----
                End If

                ' ���H����8�͌���͖��g�p(�\��)

            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SetCutConditionData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SetCutConditionData = cERR_TRAP
        End Try
    End Function

#End Region

    '----- V2.0.0.0_26�� -----
#Region "���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��"
    '''=========================================================================
    ''' <summary>���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��</summary>
    ''' <param name="FileVer">(INP)�f�[�^�t�@�C���o�[�W����</param>
    ''' <returns>cFRS_NORMAL  = ����
    '''          ��L�ȊO �@�@= �G���[</returns> 
    '''=========================================================================
    Public Function SetCutDataCndInfFromCndNum(ByVal FileVer As Double) As Integer

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   ��������
            '------------------------------------------------------------------
            ' FL�łȂ�����SL436S�łȂ����NOP
            'If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Or (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)
            ' FL�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL) ' V4.0.0.0-28

            ' �f�[�^�t�@�C���o�[�W������Ver10.10�ȏ�Ȃ�NOP
            If (FileVer >= FileIO.FILE_VER_10_10) Then Return (cFRS_NORMAL)

            '------------------------------------------------------------------
            '   ���H�����ԍ�����J�b�g�f�[�^��Q���[�g,�d���l,STEG�{����ݒ肷��
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' �P�u���b�N����R�����ݒ肷�� 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' �J�b�g�����ݒ肷��
                    ' �J�b�g�^�C�v�擾
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

                    ' Q���[�g1,�d���l1,STEG�{��1�͖������ɐݒ肷��
                    ' ���H����1
                    CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1)
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate = stCND.Freq(CndNum)
                    typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(0) = stCND.Steg(CndNum)
                    typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(0) = stCND.Curr(CndNum)

                    ' Q���[�g2,�d���l2,STEG�{��2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
                    ' HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                       (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                       (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Then
                        ' ���H����2
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(1) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(1) = stCND.Curr(CndNum)
                    End If

                    ' Q���[�g3,�d���l3,STEG�{��3��HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Then
                        ' ���H����3
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(2) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(2) = stCND.Curr(CndNum)
                    End If

                    ' ���H����4�͌���͖��g�p(�\��)

                    ' ���H����5�`8�̓��^�[��/���g���[�X�p 
                    ' Q���[�g5,�d���l5,STEG�{��5��(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)���ɐݒ肷��
                    If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                       (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                        ' ���H����5
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(4) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(4) = stCND.Curr(CndNum)
                    End If

                    ' Q���[�g6,�d���l6,STEG�{��6��L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)���ɐݒ肷��
                    If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                        ' ���H����6
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(5) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(5) = stCND.Curr(CndNum)
                    End If

                    ' ���H����7,8�͌���͖��g�p(�\��)

                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return�l�ݒ� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "gwModule.SetCutDataCndInfFromCndNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try
    End Function
#End Region
    '----- V2.0.0.0_26�� -----

#Region "�J�b�g�f�[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>�J�b�g�f�[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="i">          (INP)��R�f�[�^�E�C���f�b�N�X</param>
    '''<param name="j">          (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>
    '''<param name="intRegIndex">(INP)��R�ԍ�</param>
    '''<param name="startpx">    (OUT)�J�b�g�X�^�[�g���WX</param> 
    '''<param name="startpy">    (OUT)�J�b�g�X�^�[�g���WY</param> 
    '''<param name="m">          (OUT)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (OUT)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<param name="cut_type">   (OUT)�J�b�g�`��(1:st, 2:L, 3:HK, 4:IX ��)</param>   
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCut(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
                                    ByRef intRegIndex As Short, ByRef startpx As Double, ByRef startpy As Double, _
                                    ByRef m As Short, ByRef mm As Short, ByRef cut_type As Short) As Short

        Dim r As Integer
        Dim s As String
        Dim strMSG As String

        Try
            SendTrimDataCut = cFRS_NORMAL                                       ' Return�l = ����
            'stTCUT.Initialize()                                                ' �J�b�g�f�[�^�\���̏�����

            ' �}�[�L���O�f�[�^�̔���
            If (intRegIndex = 1000) Then
                m = intRegIndex
                'mm = j
            Else
                '' �w���R�ԍ��A��Ĕԍ�m�ƶ���ް��z��ԍ�mm��Ԃ�
                'Call GetResistorCutAddress(intRegIndex, j, m, mm)
                m = i
            End If
            mm = j

            ' ��ʂŐݒ肵����Č`���INtime�ɓn���l�ɕύX
            s = typResistorInfoArray(m).ArrCut(mm).strCutType                   ' ��Č`��
            cut_type = Form1.Utility1.GetCutTypeNum(s.Trim())                   ' ��Č`���Ď�ʂɕϊ�(�O��̋󔒂��폜)

            ' �J�b�g�f�[�^��ݒ肷��
            With stTCUT
                .wCutNo = j                                                     ' �J�b�g�ԍ�
                .wDelayTime = typResistorInfoArray(m).ArrCut(mm).intDelayTime   ' ��d������㑪��x������
                .wCutType = CnvCutTypeNum(cut_type)                             ' �J�b�g�`��(1:st, 2:L, 3:HK, 4:IX ��)
                '----- V1.22.0.0�@�� -----
                .wMoveMode = typResistorInfoArray(m).ArrCut(mm).intMoveMode     ' ���샂�[�h(0:�ʏ탂�[�h,1:�e�B�[�`, 2:�����J�b�g���[�h)
                '----- V1.22.0.0�@�� -----
                .fCutStartX = typResistorInfoArray(m).ArrCut(mm).dblStartPointX ' �J�b�g�X�^�[�g���WX
                .fCutStartY = typResistorInfoArray(m).ArrCut(mm).dblStartPointY ' �J�b�g�X�^�[�g���WY
                'V5.0.0.6�I���O���J�����J�b�g�ʒu�␳�ʂ̉��Z
                If giTeachpointUse = 1 Then
                    If (Not GetCutStartPointAddTeachPoint(m, mm, .fCutStartX, .fCutStartY)) Then
                        .fCutStartX = typResistorInfoArray(m).ArrCut(mm).dblStartPointX ' �J�b�g�X�^�[�g���WX
                        .fCutStartY = typResistorInfoArray(m).ArrCut(mm).dblStartPointY ' �J�b�g�X�^�[�g���WY
                        Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + m.ToString() + "] CUT=[" + mm.ToString() + "]")
                    End If
                End If
                'V5.0.0.6�I��
                startpx = .fCutStartX                                           ' �J�b�g�X�^�[�g���WX��Ԃ�
                startpy = .fCutStartY                                           ' �J�b�g�X�^�[�g���WY��Ԃ�
                '                                                               ' �J�b�g�I�t %
                If (gSysPrm.stCTM.giSPECIAL = customKOA) Then
                    .fCutOff = typResistorInfoArray(m).ArrCut(mm).dblCutOff + typResistorInfoArray(m).ArrCut(mm).dblCutOffOffset + 100.0#
                Else
                    .fCutOff = typResistorInfoArray(m).ArrCut(mm).dblCutOff + 100.0#
                End If

                'V6.0.0.2�B��
                .fAveDataRate = typResistorInfoArray(m).ArrCut(mm).dblJudgeLevel
                ''----- ###176�� -----
                '' �ؑփ|�C���g(%)��ݒ肷��(CHIP�p)
                'If (gTkyKnd = KND_CHIP) And (j = 1) Then
                '    .fAveDataRate = typResistorInfoArray(m).ArrCut(mm).dblJudgeLevel
                'Else
                '    .fAveDataRate = 0.0
                'End If
                ''If (gTkyKnd = KND_TKY) Then
                ''    .fAveDataRate = typResistorInfoArray(m).ArrCut(mm).dblJudgeLevel ' ���ω���(0.0�`100.0, 0%)(���g�p)
                ''End If
                ''----- ###176�� -----
                'V6.0.0.2�B��

                '�|�W�V���j���O�Ȃ��J�b�g�p�����[�^���t '###005
                .wDoPosition = typResistorInfoArray(m).ArrCut(mm).intDoPosition

                ' �J�b�g��|�[�Y�^�C���i�����jV1.18.0.3�@
                .wCutAftPause = typResistorInfoArray(m).ArrCut(mm).intCutAftPause

                ' ���H�����̐ݒ�
                SetCutConditionData(.CutCnd, m, mm)

            End With

            ' �J�b�g�f�[�^��INtime���ɑ��M����
            r = TRIMDATA_CUTDATA(stTCUT, i, j)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "Cut Data Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCut = 6
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCut = cERR_TRAP
        End Try
    End Function

#End Region

#Region "�J�b�g�ԍ���ϊ�����"
    '''=========================================================================
    '''<summary>�J�b�g�ԍ���ϊ�����</summary>
    '''<param name="cut_type">(INP)�J�b�g�ԍ�</param>
    '''<returns>�ϊ���̃J�b�g�ԍ�</returns>
    '''=========================================================================
    Private Function CnvCutTypeNum(ByVal cut_type As Short) As UShort

        Dim r As UShort

        Select Case cut_type
            Case 1, 6, 8, 10, 12, 14, 28                        ' ST, RETURN/RETRACE + NANAME, ST2
                r = 1
            Case 2, 7, 9, 11, 13, 15                            ' L, (RETURN or RETRACE) + NANAME
                r = 2
            Case 18                                             ' U�J�b�g(���g���[�X) 'V1.22.0.0�@ 
                r = 17                                          ' U�J�b�g�Ƃ���       'V1.22.0.0�@
            Case Else
                r = cut_type
        End Select
        Return (r)
    End Function

#End Region

#Region "ST Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>ST Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="cut_type">   (INP)�J�b�g�`��(1:st, 2:L, 3:HK, 4:IX ��)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<param name="lenxy">      (OUT)�J�b�e�B���O��</param>
    '''<param name="dirxy">      (OUT)�J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutST(ByRef intTkyType As Short, _
                                      ByRef cut_type As Short, ByRef m As Short, ByRef mm As Short, _
                                      ByRef lenxy As Double, ByRef dirxy As Short, _
                                      ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutST(ByRef intTkyType As Short, _
        '                                  ByRef cut_type As Short, ByRef m As Short, ByRef mm As Short, _
        '                                  ByRef mCut1 As Short, ByRef lenxy As Double, ByRef dirxy As Short, _
        '                                  ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutST(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                  ByRef cut_type As Short, ByRef m As Short, ByRef mm As Short, _
        '                                  ByRef mCut1 As Short, ByRef lenxy As Double, ByRef dirxy As Short, _
        '                                  ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutST As PRM_CUT_ST                                                   ' ST cut�p�����[�^
        Dim strMSG As String

        Try
            ' ST Cut�p�����[�^��ݒ肷��
            SendTrimDataCutST = cFRS_NORMAL                                         ' Return�l = ����
            With stCutST
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' �J�b�e�B���O��
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle             ' �J�b�g�p�x
                '.angle = 0.0#                                                       ' �J�b�g�p�x(0�`359)
                .RtOfs = typResistorInfoArray(m).ArrCut(mm).dblReturnPos            ' RT�I�t�Z�b�g(mm) V1.16.0.0�@
                lenxy = .Length                                                     ' �J�b�e�B���O����Ԃ�
                lenxy2 = 0
                dirxy2 = 0
                '.DIR = mCut1                                                        ' �J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)
                'dirxy = .DIR                                                        ' �J�b�g������Ԃ�

                Select Case cut_type
                    Case 1
                        .MODE = 0                                                   ' ���샂�[�h = �ʏ�
                    Case 6
                        .MODE = 1                                                   ' ���샂�[�h = ���^�[��
                    Case 8
                        .MODE = 2                                                   ' ���샂�[�h = ���g���[�X
                    Case 10
                        .MODE = 4                                                   ' ���샂�[�h = �΂߃J�b�g
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                    Case 12
                        .MODE = 5                                                   ' ���샂�[�h = �΂� + ���^�[��
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                    Case 14
                        .MODE = 6                                                   ' ���샂�[�h = �΂� + ���g���[�X
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                    Case 28
                        .MODE = 0                                                   ' �]����ST�Ɠ��l
                End Select
            End With

            ' ST Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutST(stCutST, m, mm)
            If (r <> cFRS_NORMAL) Then                                              ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "ST Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutST = 7
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutST() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutST = cERR_TRAP
        End Try
    End Function

#End Region

#Region "L Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>L Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="cut_type">   (INP)�J�b�g�`��(1:st, 2:L, 3:HK, 4:IX ��)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<param name="lenxy">      (OUT)�J�b�e�B���O��</param>
    '''<param name="dirxy">      (OUT)�J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)�J�b�e�B���O��2</param>
    '''<param name="dirxy2">     (OUT)L�^�[������(1:+X, 2:-X, 3:+Y, 4:-Y)</param>         
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutL(ByRef intTkyType As Short, ByRef cut_type As Short, _
                                      ByRef m As Short, ByRef mm As Short, _
                                      ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutL(ByRef intTkyType As Short, ByRef cut_type As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, ByRef mCut2 As Short, _
        '                                  ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutL(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, ByRef cut_type As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, ByRef mCut2 As Short, _
        '                                  ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutL As PRM_CUT_L                                                     ' L Cut�p�����[�^
        Dim strMSG As String

        Try
            ' L Cut�p�����[�^��ݒ肷��
            SendTrimDataCutL = cFRS_NORMAL                                          ' Return�l = ����
            With stCutL
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength            ' L1�ő�J�b�e�B���O��
                .r = typResistorInfoArray(m).ArrCut(mm).dblR1                       ' �^�[���̉~�ʔ��a
                .turn = typResistorInfoArray(m).ArrCut(mm).dblLTurnPoint            ' L�^�[���|�C���g(0.0�`100.0(%))
                .L2 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthL           ' L2�ő�J�b�e�B���O��
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle             ' �J�b�g�p�x
                .tdir = typResistorInfoArray(m).ArrCut(mm).intLTurnDir              ' L��ݕ���(1:CW, 2:CCW) ���ύX
                .RtOfs = typResistorInfoArray(m).ArrCut(mm).dblReturnPos            ' RT�I�t�Z�b�g(mm) V1.16.0.0�@
                lenxy = .L1                                                         ' L1�ő�J�b�e�B���O����Ԃ�
                lenxy2 = .L2                                                        ' L2�ő�J�b�e�B���O����Ԃ�

                Select Case cut_type
                    Case 2
                        .MODE = 0                                                   ' ���샂�[�h = �ʏ�
                    Case 7
                        .MODE = 1                                                   ' ���샂�[�h = ���^�[��
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                    Case 9
                        .MODE = 2                                                   ' ���샂�[�h = ���g���[�X
                    Case 11
                        .MODE = 4                                                   ' ���샂�[�h = �΂�
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                    Case 13
                        .MODE = 5                                                   ' ���샂�[�h = �΂� + ���^�[��
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                    Case 15
                        .MODE = 6                                                   ' ���샂�[�h = �΂� + ���g���[�X
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' �J�b�g�p�x
                End Select
            End With

            ' L Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutL(stCutL, m, mm)
            If (r <> cFRS_NORMAL) Then                                              ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "L Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutL = 8
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutL = cERR_TRAP
        End Try
    End Function
#End Region

#Region "HOOK Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>HOOK Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="cut_type">   (INP)�J�b�g�`��(1:st, 2:L, 3:HK, 4:IX ��)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<param name="lenxy">      (OUT)�J�b�e�B���O��</param>
    '''<param name="dirxy">      (OUT)�J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)�J�b�e�B���O��2</param>
    '''<param name="dirxy2">     (OUT)L�^�[������(1:+X, 2:-X, 3:+Y, 4:-Y)</param>         
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutHOOK(ByRef intTkyType As Short, ByRef cut_type As Short, _
                                         ByRef m As Short, ByRef mm As Short, _
                                         ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutHK As PRM_CUT_HOOK                                             ' HOOK Cut�p�����[�^
        Dim strMSG As String

        Try
            ' HOOK Cut�p�����[�^��ݒ肷��
            SendTrimDataCutHOOK = cFRS_NORMAL                                   ' Return�l = ����
            With stCutHK
                .MODE = typResistorInfoArray(m).ArrCut(mm).intMoveMode          ' ���샂�[�h(0:NOM, 1:���^�[��, 2:���g���[�X, 3:�΂�)
                '----- V1.22.0.0�@�� -----
                If (cut_type = 18) Then                                         ' U CUT(RETRACE) ? 
                    .MODE = 2                                                   ' ���샂�[�h = 2(���g���[�X)
                End If
                '----- V1.22.0.0�@�� -----
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' �΂߃J�b�g�p�x(0�`359)
                .tdir = typResistorInfoArray(m).ArrCut(mm).intLTurnDir          ' L��ݕ���(1:CW, 2:CCW) ���ύX
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' L1 �ő�J�b�e�B���O��
                .r1 = typResistorInfoArray(m).ArrCut(mm).dblR1                  ' �^�[��1�̉~�ʔ��a
                .turn = typResistorInfoArray(m).ArrCut(mm).dblLTurnPoint        ' L�^�[���|�C���g
                .L2 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthL       ' L2 �ő�J�b�e�B���O��
                .r2 = typResistorInfoArray(m).ArrCut(mm).dblR2                  ' �^�[��2�̉~�ʔ��a
                .L3 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthHook    ' L3 �ő�J�b�e�B���O��
                lenxy = .L1                                                     ' L1 �ő�J�b�e�B���O����Ԃ�
                lenxy2 = .L2                                                    ' L2 �ő�J�b�e�B���O����Ԃ�
            End With

            ' HOOK Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutHK(stCutHK, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                If cut_type = 3 Then                                            ' HOOK Cut ? 
                    Call Form1.System1.TrmMsgBox(gSysPrm, "HOOK Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataCutHOOK = 9
                ElseIf cut_type = 17 Then                                       ' U Cut ? 
                    Call Form1.System1.TrmMsgBox(gSysPrm, "U Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataCutHOOK = 14
                End If
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutHOOK() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutHOOK = cERR_TRAP
        End Try

    End Function
#End Region

#Region "INDEX Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>INDEX Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  param> 
    '''<param name="lenxy">      (OUT)�C���f�b�N�X��</param>
    '''<param name="dirxy">      (OUT)�J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)0��Ԃ�</param>
    '''<param name="dirxy2">     (OUT)0��Ԃ�</param>         
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutINDEX(ByRef intTkyType As Short, _
                                          ByRef m As Short, ByRef mm As Short, _
                                          ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutINDEX(ByRef intTkyType As Short, _
        '                                      ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                      ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutINDEX(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                      ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                      ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutIX As PRM_CUT_INDEX                                            ' INDEX Cut�p�����[�^
        Dim strMSG As String

        Try
            ' INDEX Cut�p�����[�^��ݒ肷��
            SendTrimDataCutINDEX = cFRS_NORMAL                                  ' Return�l = ����
            With stCutIX
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' �΂߃J�b�g�p�x(0�`359)
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength    ' �C���f�b�N�X��
                .maxindex = typResistorInfoArray(m).ArrCut(mm).intIndexCnt      ' �C���f�b�N�X��
                .measMode = typResistorInfoArray(m).ArrCut(mm).intMeasMode      ' ���胂�[�h(0:��R, 1:�d��)
                .measType = typResistorInfoArray(m).ArrCut(mm).intMeasType      ' ���蔻��^�C�v 0:����, 1:�����x, 2:�O��)
                .LimitLen = typResistorInfoArray(m).ArrCut(mm).dblLimitLen      ' IX�J�b�g�̃��~�b�g�� 'V1.18.0.0�C
                '----- V6.1.1.0�E�� (�C���f�b�N�X�v�J�b�g�p) -----
                .lines = typResistorInfoArray(m).ArrCut(mm).intCutCnt           ' �V�t�g��(1�`n)               (SCAN�J�b�g�̖{����g�p)
                .pitch = typResistorInfoArray(m).ArrCut(mm).dblPitch            ' �V�t�g��(�}0.0001�`20.0000(mm))(SCAN�J�b�g�̃s�b�`��g�p)
                '----- V6.1.1.0�E�� -----

                '----- ###229�� -----
                ' �V�X�p����GP-IB���䂠��(2:�ėp)�ő���^�C�v���O���Ȃ�GP-IB����(�ėp)�t���O�𐧌䂠��Ƃ���
                If (gSysPrm.stCTM.giGP_IB_flg = 2) And (.measType = 2) Then
                    bGpib2Flg = 1                                               ' GP-IB����(�ėp)�t���O(0=����Ȃ�, 1=���䂠��)
                End If
                '----- ###229�� -----
                lenxy = .Length                                                 ' �C���f�b�N�X����Ԃ�
                lenxy2 = 0
                dirxy2 = 0
                '.DIR = mCut1                                                    ' �J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)
                'dirxy = .DIR                                                    ' �J�b�g������Ԃ�
            End With

            ' INDEX Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutIX(stCutIX, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "IX Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutINDEX = 10
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutINDEX() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutINDEX = cERR_TRAP
        End Try

    End Function
#End Region

#Region "INDEX2 Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>INDEX2 Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<param name="lenxy">      (OUT)�C���f�b�N�X��</param>
    '''<param name="dirxy">      (OUT)�J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)0��Ԃ�</param>
    '''<param name="dirxy2">     (OUT)0��Ԃ�</param>         
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutINDEX2(ByRef intTkyType As Short, _
                                           ByRef m As Short, ByRef mm As Short, _
                                           ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutINDEX2(ByRef intTkyType As Short, _
        '                                       ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                       ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutINDEX2(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                       ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                       ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutIX2 As PRM_CUT_INDEX                                           ' INDEX2 cut�p�����[�^
        Dim strMSG As String

        Try
            ' INDEX2 Cut(�߼޼��ݸޖ������ޯ�����)�p�����[�^��ݒ肷��
            SendTrimDataCutINDEX2 = cFRS_NORMAL                                 ' Return�l = ����
            With stCutIX2
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength    ' �C���f�b�N�X��
                .maxindex = typResistorInfoArray(m).ArrCut(mm).intIndexCnt      ' �C���f�b�N�X��
                .measMode = typResistorInfoArray(m).ArrCut(mm).intMeasMode      ' ���胂�[�h(0:��R, 1:�d��, 3:�O��)
                .measType = typResistorInfoArray(m).ArrCut(mm).intMeasType      ' ����^�C�v 0:����, 1:�����x, 3:)
                .LimitLen = typResistorInfoArray(m).ArrCut(mm).dblLimitLen      ' IX�J�b�g�̃��~�b�g�� 'V1.18.0.0�C
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' �΂߃J�b�g�p�x(0�`359)
                lenxy = .Length                                                 ' �C���f�b�N�X����Ԃ�
                lenxy2 = 0
                dirxy2 = 0
                '.DIR = mCut1                                                    ' �J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)
                'dirxy = .DIR                                                    ' �J�b�g������Ԃ�
            End With

            ' INDEX2 Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutIX(stCutIX2, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "IX2 Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutINDEX2 = 10
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutINDEX2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutINDEX2 = cERR_TRAP
        End Try

    End Function

#End Region

#Region "SCAN Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>SCAN Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutSCAN(ByRef intTkyType As Short, _
                                         ByRef m As Short, ByRef mm As Short) As Short
        'Public Function SendTrimDataCutSCAN(ByRef intTkyType As Short, _
        '                                     ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short
        'Public Function SendTrimDataCutSCAN(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                     ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short

        Dim r As Integer
        'Dim Cdir As Short
        Dim stCutSC As PRM_CUT_SCAN                                             ' SCAN Cut�p�����[�^
        Dim strMSG As String

        Try
            ' SCAN Cut(�߼޼��ݸޖ������ޯ�����)�p�����[�^��ݒ肷��
            SendTrimDataCutSCAN = cFRS_NORMAL                                   ' Return�l = ����
            With stCutSC
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength    ' �J�b�e�B���O��
                .pitch = typResistorInfoArray(m).ArrCut(mm).dblPitch            ' �s�b�`
                .lines = typResistorInfoArray(m).ArrCut(mm).intCutCnt           ' �{��
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' �΂߃J�b�g�p�x(0�`359)
                .sdir = typResistorInfoArray(m).ArrCut(mm).intStepDir            ' �X�e�b�v����(1:��(+X), 2:��(-X), 3:��(+Y), 4:��(-Y))
                'Cdir = typResistorInfoArray(m).ArrCut(mm).intStepDir            ' �X�e�b�v����(1:��(+X), 2:��(-X), 3:��(+Y), 4:��(-Y))
                '.sdir = Form1.Utility1.ConvertVbdir2Rtdir(Cdir)                         ' �ï�ߕ����ϊ�(SCAN�J�b�g�p)(1:+X,2:-Y,3:-X,4:+Y)��(1:+X,4:-Y,2:-X,3:+Y)
                ''.DIR = mCut1                                                    
            End With

            ' SCAN Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutSC(stCutSC, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "SCAN Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutSCAN = 11
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutSCAN() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutSCAN = cERR_TRAP
        End Try

    End Function
#End Region

#Region "�����}�[�L���O�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>�����}�[�L���O�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<param name="mCut1">      (INP)�J�b�g����(1:+X, 2:-X, 3:+Y, 4:-Y)</param> 
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutMarking(ByRef intTkyType As Short, _
                                            ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short
        'Public Function SendTrimDataCutMarking(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                        ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short

        Dim r As Integer
        Dim s As String
        Dim n As Short
        Dim nn As Short
        Dim markDir(4) As Short
        'Dim stCutMK As PRM_CUT_MARKING                                         ' Letter Marking�p�����[�^
        Dim strMSG As String

        Try
            ' ����ϰ�ݸރp�����[�^��ݒ肷��
            SendTrimDataCutMarking = cFRS_NORMAL                                ' Return�l = ����
            'stCutMK.Initialize()


            ' ����ϰ�ݸޕ���
            'markDir(0) = 1                                                      ' 0
            'markDir(1) = 1                                                      ' +X 0
            'markDir(2) = 4                                                      ' -Y 270
            'markDir(3) = 3                                                      ' -X 180
            'markDir(4) = 2                                                      ' +Y 90

            ' �����}�[�L���O�p�����[�^��ݒ肷��
            With stCutMK
                '.DIR = markDir(mCut1)
                .DIR = mCut1 / 90
                .magnify = Val(typResistorInfoArray(m).ArrCut(mm).dblZoom)      ' �{��
                s = typResistorInfoArray(m).ArrCut(mm).strChar                  ' �}�[�L���O������
                For n = 0 To (cMAXcMARKINGcSTRLEN - 1)
                    .str(n) = 0 : Next
                nn = Len(s)
                If nn > cMAXcMARKINGcSTRLEN Then nn = cMAXcMARKINGcSTRLEN
                For n = 1 To nn
                    .str(n - 1) = Asc(Mid(s, n, 1))
                Next
            End With

            ' �����}�[�L���O�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutMK(stCutMK, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "Marking Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutMarking = 12
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutMarking() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutMarking = cERR_TRAP
        End Try

    End Function
#End Region

#Region "ES Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>ES Cut�p�����[�^��INtime���ɑ��M����</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutES(ByRef intTkyType As Short, _
                                      ByRef m As Short, ByRef mm As Short) As Short
        'Public Function SendTrimDataCutES(ByRef intTkyType As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short
        '    'Public Function SendTrimDataCutES(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short

        Dim r As Integer
        Dim stCutES As PRM_CUT_ES                                               ' ES cut�p�����[�^
        Dim strMSG As String

        Try
            ' ES Cut�p�����[�^��ݒ肷��
            SendTrimDataCutES = cFRS_NORMAL                                     ' Return�l = ����
            With stCutES
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' L1 �ő�J�b�e�B���O��
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' �΂߃J�b�g�p�x(0�`359)
                '.DIR = mCut1
                .EsPoint = typResistorInfoArray(m).ArrCut(mm).dblESPoint        ' ES�|�C���g
                .ESWide = typResistorInfoArray(m).ArrCut(mm).dblESJudgeLevel    ' ES����ω���
                .ESWide2 = typResistorInfoArray(m).ArrCut(mm).dblESChangeRatio  ' ES��ω���(0.0�`100.0%)
                .EScount = typResistorInfoArray(m).ArrCut(mm).intESConfirmCnt   ' ES��m�F��(0�`20)
                .CTcount = typResistorInfoArray(m).ArrCut(mm).intCTcount        ' ���޾ݽ��A��NG�m�F�񐔁@
                .wJudgeNg = typResistorInfoArray(m).ArrCut(mm).intJudgeNg       ' NG���肷��/���Ȃ��i0:TRUE/1:FALSE�j
            End With

            ' ES Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutES(stCutES, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "ES Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutES = 20
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutES() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutES = cERR_TRAP
        End Try

    End Function
#End Region

#Region "ES Cut�p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>ES Cut�p�����[�^��INtime���ɑ��M���� V1.14.0.0�@</summary>
    '''<param name="intTkyType"> (INP)TKY���(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)��R�f�[�^�E�C���f�b�N�X</param> 
    '''<param name="mm">         (INP)�J�b�g�f�[�^�E�C���f�b�N�X</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutES0(ByRef intTkyType As Short, _
                                      ByRef m As Short, ByRef mm As Short) As Short

        Dim r As Integer
        Dim stCutES0 As PRM_CUT_ES0                                               ' ES cut�p�����[�^
        Dim strMSG As String

        Try
            ' ES Cut�p�����[�^��ݒ肷��
            SendTrimDataCutES0 = cFRS_NORMAL                                     ' Return�l = ����
            With stCutES0
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' L1 �ő�J�b�e�B���O��
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' �΂߃J�b�g�p�x(0�`359)
                '.DIR = mCut1
                .EsPoint = typResistorInfoArray(m).ArrCut(mm).dblESPoint        ' ES�|�C���g
                .ESWide = typResistorInfoArray(m).ArrCut(mm).dblESJudgeLevel    ' ES����ω���
                .L2 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthES       ' L2 �d�r��J�b�g��(0.0001�`20.0000(mm))
                .wJudgeNg = typResistorInfoArray(m).ArrCut(mm).intJudgeNg       ' NG���肷��/���Ȃ��i0:TRUE/1:FALSE�j
            End With

            ' ES Cut�p�����[�^��INtime���ɑ��M����
            r = TRIMDATA_CutES0(stCutES0, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "ES0 Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutES0 = 20
                Exit Function
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutES0() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutES0 = cERR_TRAP
        End Try

    End Function
#End Region

#Region "UCut�p�����[�^�̑��M"
    '''=========================================================================
    '''<summary>UCut�p�����[�^�̑��M</summary>
    '''<param name="sParamFn">(INP)U�J�b�g�f�[�^��</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''<remarks>TKY���ڐA�FTKY��p�Ƃ��邩�͌�Ɍ���</remarks>
    '''=========================================================================
    Public Function SendUCutParam(ByVal sParamFn As String) As Short

        Dim sPath As String
        Dim RNO As Short
        Dim RATIO As Double
        Dim NOM As Double
        Dim LTP As Double
        Dim LTP2 As Double
        Dim L1 As Double
        Dim L2 As Double
        Dim r As Double
        Dim V As Double
        Dim n As Short
        'Dim fn As Short
        Dim indx As Short
        Dim EL As Short
        Dim maxcnt As Short
        Dim prevrno As Short
        Dim s As String
        Dim nlines As Integer

        ' �t�J�b�g�f�[�^������ ?
        If (sParamFn = "") Then
            SendUCutParam = 0
            Exit Function
        End If

        'sParamFn = Trim(sParamFn)
        sParamFn = sParamFn.Trim()
        If (sParamFn = "") Then
            MsgBox("UCUT DATA FILE NAME ERROR", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT DATA ERROR")
            SendUCutParam = 1
            Exit Function
        End If

        ' R�ԍ��A�����l���ALTP�AL1�AL2�ARADIUS�AV
        '----- V1.19.0.0�D�� -----
        'sPath = gStrTrimFileName                                        ' sPath = �g���~���O�f�[�^�� 
        'n = InStrRev(sPath, "\")
        'If n = 0 Then
        '    sPath = My.Application.Info.DirectoryPath
        'Else
        '    sPath = Left(sPath, n)
        'End If
        'If Right(sPath, 1) <> "\" Then                                  ' sPath = �g���~���O�f�[�^�p�X��+ "\" 
        '    sPath = sPath & "\"
        'End If
        'sPath = sPath & sParamFn & ".csv"

        sPath = sParamFn                                                ' sPath = U�J�b�g�f�[�^��
        '----- V1.19.0.0�D�� -----
        'If Dir(sPath) = "" Then
        If (False = IO.File.Exists(sPath)) Then
            SendUCutParam = 2
            MsgBox("UCUT DATA ERROR", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT DATA FILE READ ERROR")
            Exit Function
        End If

        'On Error GoTo ErrTrap
        Try                                                             'V4.4.0.0-0
            'fn = FreeFile()
            'FileOpen(fn, sPath, OpenMode.Input)
            Using sr As New StreamReader(sPath, Encoding.GetEncoding("Shift_JIS")) ' �S�p�Ȃ��݊����ێ�  V4.4.0.0-0
                indx = -1
                nlines = 0
                prevrno = -1
                'Do While Not EOF(fn)
                Do While (False = sr.EndOfStream)                       'V4.4.0.0-0
                    's = LineInput(fn)
                    s = sr.ReadLine()                                   'V4.4.0.0-0
                    If n < 1 And s = "" Then Exit Do ' �f�[�^�ŌオNULL�̏ꍇ�͋����I�ɏI��������B
                    nlines = nlines + 1
                    'If Left(s, 1) <> ";" Then GoTo ErrFile
                    If (False = s.StartsWith(";")) Then GoTo ErrFile '   V4.4.0.0-0
                    'If EOF(fn) Then Exit Do
                    If (True = sr.EndOfStream) Then Exit Do '            V4.4.0.0-0
                    's = LineInput(fn)
                    s = sr.ReadLine()                                   'V4.4.0.0-0
                    nlines = nlines + 1
                    'If Left(s, 1) <> ";" Then GoTo ErrFile
                    If (False = s.StartsWith(";")) Then GoTo ErrFile '   V4.4.0.0-0
                    'If EOF(fn) Then Exit Do
                    If (True = sr.EndOfStream) Then Exit Do '            V4.4.0.0-0
                    's = LineInput(fn)
                    s = sr.ReadLine()                                   'V4.4.0.0-0
                    nlines = nlines + 1
                    'If Left(s, 1) <> ";" Then GoTo ErrFile
                    If (False = s.StartsWith(";")) Then GoTo ErrFile '   V4.4.0.0-0

                    'Do While Not EOF(fn)
                    Do While (False = sr.EndOfStream)                   'V4.4.0.0-0

                        s = sr.ReadLine()                               'V4.4.0.0-0
                        Dim splt() As String
                        Try
                            splt = s.Replace(" ", String.Empty).Split(","c)
                        Catch ex As Exception
                            GoTo ErrData
                        End Try

                        Select Case gSysPrm.stSPF.giUCutKind
                            Case 1 ' �d�q�Z��
                                If (9 <> splt.Length) Then GoTo ErrData 'V4.4.0.0-0
                                'Input(fn, RNO)
                                If (False = Short.TryParse(splt(0), RNO)) Then GoTo ErrData
                                'Input(fn, n)
                                If (False = Short.TryParse(splt(1), n)) Then GoTo ErrData
                                'Input(fn, RATIO)
                                If (False = Double.TryParse(splt(2), RATIO)) Then GoTo ErrData
                                'Input(fn, NOM)
                                If (False = Double.TryParse(splt(3), NOM)) Then GoTo ErrData
                                'Input(fn, LTP)
                                If (False = Double.TryParse(splt(4), LTP)) Then GoTo ErrData
                                'Input(fn, L1)
                                If (False = Double.TryParse(splt(5), L1)) Then GoTo ErrData
                                'Input(fn, L2)
                                If (False = Double.TryParse(splt(6), L2)) Then GoTo ErrData
                                'Input(fn, r)
                                If (False = Double.TryParse(splt(7), r)) Then GoTo ErrData
                                'Input(fn, V)
                                If (False = Double.TryParse(splt(8), V)) Then GoTo ErrData
                                nlines = nlines + 1

                                If n < 1 Then Exit Do

                                If LTP < 0.0# OrElse LTP > 100.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= LTP <= 100.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If NOM < 0.0001 OrElse NOM > 1000000000 Then '0.1m���`�P�f��
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0001 <= NOM <= 1000000000", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L1 < 0.0# OrElse L1 > 20.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= L1 <= 20.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L2 < 0.0# OrElse L2 > 20.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= L2 <= 20.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If r < 0.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " r >= 0.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If V < 0.1 OrElse V > 409.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " V >= 0.1", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L1 < r Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " L1 < r" & vbCrLf & "L1 length must be greater than r or equal.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L2 < (r * 2) Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " L2 < (r*2)" & vbCrLf & "L2 length must be greater than (r*2) or equal.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If RNO <> prevrno Then
                                    indx = indx + 1
                                End If
                                prevrno = RNO

                                EL = n - 1

                                stUCutPrm(indx).EL(EL).RNO = RNO
                                stUCutPrm(indx).EL(EL).NOM = NOM
                                stUCutPrm(indx).EL(EL).PRM_UNIT.RATIO = RATIO
                                stUCutPrm(indx).EL(EL).PRM_UNIT.LTP = LTP
                                stUCutPrm(indx).EL(EL).PRM_UNIT.L1 = L1
                                stUCutPrm(indx).EL(EL).PRM_UNIT.L2 = L2
                                stUCutPrm(indx).EL(EL).PRM_UNIT.r = r
                                stUCutPrm(indx).EL(EL).PRM_UNIT.V = V
                                stUCutPrm(indx).EL(EL).PRM_UNIT.Flg = 1

                                ' ��ʁ������̏ꍇ
                            Case 2 '����
                                If (5 <> splt.Length) Then GoTo ErrData 'V4.4.0.0-0
                                'Input(fn, RNO)
                                If (False = Short.TryParse(splt(0), RNO)) Then GoTo ErrData
                                'Input(fn, n)
                                If (False = Short.TryParse(splt(1), n)) Then GoTo ErrData
                                'Input(fn, RATIO)
                                If (False = Double.TryParse(splt(2), RATIO)) Then GoTo ErrData
                                'Input(fn, LTP)
                                If (False = Double.TryParse(splt(3), LTP)) Then GoTo ErrData
                                'Input(fn, LTP2)
                                If (False = Double.TryParse(splt(4), LTP2)) Then GoTo ErrData
                                nlines = nlines + 1
                                If n < 1 Then Exit Do

                                If LTP < 0.0# OrElse LTP > 100.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= LTP <= 100.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If LTP2 < 0.0# OrElse LTP2 > 100.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= LTP2 <= 100.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If RNO <> prevrno Then
                                    indx = indx + 1
                                End If
                                prevrno = RNO

                                EL = n - 1
                                stUCutPrm(indx).EL(EL).RNO = RNO
                                stUCutPrm(indx).EL(EL).PRM_UNIT.Flg = 1
                                stUCutPrm(indx).EL(EL).PRM_UNIT.RATIO = RATIO
                                stUCutPrm(indx).EL(EL).PRM_UNIT.LTP = LTP
                                stUCutPrm(indx).EL(EL).PRM_UNIT.LTP2 = LTP2
                        End Select
                    Loop

                Loop

                'FileClose(fn)
                'On Error GoTo 0
            End Using

            maxcnt = indx + 1

            ' UCUT�p�����[�^�ݒ�(���[�h(0=�������A���P�[�g))
            'Call UCUT_PARAMSET(0, maxcnt, 0, 0, 0, stUCutPrm(0).EL(0).PRM_UNIT)
            Call UCUT_PARAMSET(0, 0, maxcnt, 0, 0, stUCutPrm(0).EL(0).PRM_UNIT) ' V1.19.0.0�D

            For indx = 0 To maxcnt - 1
                For EL = 0 To 19
                    If stUCutPrm(indx).EL(EL).PRM_UNIT.Flg Then
                        ' �����t�J�b�g��ʂɂ��؂蕪��
                        Select Case gSysPrm.stSPF.giUCutKind
                            Case 1 ' �d�q�Z��
                                Call UCUT_PARAMSET(1, 1, stUCutPrm(indx).EL(EL).RNO, indx, EL, stUCutPrm(indx).EL(EL).PRM_UNIT)
                            Case 2 '����
                                Call UCUT_PARAMSET(1, 2, stUCutPrm(indx).EL(EL).RNO, indx, EL, stUCutPrm(indx).EL(EL).PRM_UNIT)
                        End Select
                    End If
                Next
            Next

            Exit Function

        Catch ex As Exception
            'ErrTrap:
            '            Resume ErrTrap1
            'ErrTrap1:
            '            On Error GoTo 0
            MsgBox("UCUT DATA ERROR", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT DATA FILE READ ERROR")
            SendUCutParam = 2
            Exit Function
        End Try

ErrFile:
        'FileClose(fn)
        'On Error GoTo 0
        MsgBox("File format error", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT PARAMETER FILE")
        SendUCutParam = 3
        Exit Function

ErrData:
        'FileClose(fn)
        'On Error GoTo 0
        SendUCutParam = 4
        Exit Function

    End Function

#End Region

#Region "��Ă��s��(CUT2)"
    '''=========================================================================
    '''<summary>��Ă��s��(CUT2)</summary>
    '''<param name="dblLength">(INP)�ړ���</param>
    '''<param name="intDir">   (INP)����</param> 
    '''<param name="dblSpeed"> (INP)���x</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function cut(ByVal dblLength As Double, ByVal intDir As Short, ByVal dblSpeed As Double) As Integer

        Dim strMSG As String

        Try
            ' ������ĕ�����1�x�P�ʂɂȂ����Ƃ��g�p
            If (1 = intDir) Then
                intDir = 180
            ElseIf (2 = intDir) Then
                intDir = 90
            ElseIf (3 = intDir) Then
                intDir = 270
            End If

#If cOFFLINEcDEBUG = 1 Then
        cut = 0
#Else
            cut = CUT2(dblLength, dblSpeed, intDir)
#End If
            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.cut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            cut = cERR_TRAP
        End Try

    End Function

#End Region

#Region "���_���A����(����SW�����҂���߼�ݑΉ�)"
    '''=========================================================================
    '''<summary>���_���A����(����SW�����҂���߼�ݑΉ�)</summary>
    '''<returns>0:����, 0�ȊO:�G���[</returns> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Function sResetStart() As Short

        Dim strMSG As String
        Dim r As Short
        Dim coverSts As Long                                            ' ###215

        Try
            ' ���_���A
            sResetStart = cFRS_NORMAL                                   ' Return�l = ����
            '----- ###215��----- 
            r = Form1.DispInterLockSts()
            ' �C���^�[���b�N�S����/�ꕔ�����ŁA�J�o�[�ُ͈�Ƃ���(SL436R��) 
            If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                'V4.0.0.0�MIf (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gKeiTyp <> KEY_TYPE_RS) Then  'V4.0.0.0�M

                r = COVER_CHECK(coverSts)                               ' �Œ�J�o�[��Ԏ擾(0=�Œ�J�o�[�J, 1=�Œ�J�o�[��))
                If (coverSts = 1) Then                                  ' �Œ�J�o�[�� ?
                    ' �n�[�h�E�F�A�G���[(�J�o�[�X�C�b�`�I��)���b�Z�[�W�\��
                    Call Form1.System1.Form_Reset(cGMODE_ERR_HW, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                    Return (cFRS_ERR_HW)                                ' �A�v�������I��
                End If
            End If
            '----- ###215�� -----

            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' �Œ�J�o�[�`�F�b�N���� ###088
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                ' SL436�n�̏ꍇ
                Call SLIDECOVERCHK(SLIDECOVER_CHECK_OFF)                ' �X���C�h�J�o�[�`�F�b�N�Ȃ�

                '----- ###185�� ----- 
                ' �ڕ���Ɋ������ꍇ�́A��菜�����܂ő҂�(���[�_�����_���A�ł��Ȃ�����)
                r = SubstrateCheck(Form1.System1, APP_MODE_LOADERINIT)
                If (r < cFRS_NORMAL) Then
                    sResetStart = r                                     ' Return�l�ݒ�
                    Exit Function
                End If
                '----- ###185�� ----- 
                '----- ###197�� ----- 
                ' NG�r�oBOX�����t�̏ꍇ�́A��菜�����܂ő҂�(���[�_�����_���A�ł��Ȃ�����)
                r = NgBoxCheck(Form1.System1, APP_MODE_LOADERINIT)
                If (r < cFRS_NORMAL) Then
                    sResetStart = r                                     ' Return�l�ݒ�
                    Exit Function
                End If
                '----- ###197�� ----- 
                Call W_RESET()                                          ' �A���[�����Z�b�g���o ###073
                Call W_START()                                          ' �X�^�[�g�M�����o ###073
                r = Loader_OrgBack(Form1.System1)                       ' ���_���A����(�I�[�g���[�_/�g���})
                ' ������V3.1.0.0�@ 2014/11/28 ���_���A���ɑ�񌴓_�ʒu�Ɉړ�����B
#If START_KEY_SOFT Then
                If gbStartKeySoft Then
                    Call ZCONRST()
                End If
#End If
                '----- V2.0.0.0�G�� -----
                If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then        ' �G���[ ?
                    sResetStart = r                                     ' Return�l�ݒ�
                    Exit Function
                End If
                If (r = cFRS_ERR_RST) Then
                    sResetStart = r                                     ' Return�l�ݒ� 'V2.0.0.0�M
                    Exit Function
                End If

                ' ��񌴓_�ʒu�ɃX�e�[�W���ړ�����
                If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                    'Home�ʒu�ړ�
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_STAGE_HOMEMOVE)
                    'SubHomeMove()
                    'r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                    'If (r < cFRS_NORMAL) Then                           ' �G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                    '    sResetStart = r                                 ' Return�l�ݒ�
                    '    Exit Function
                    'End If
                End If
                '----- V2.0.0.0�G�� -----
                ' ������V3.1.0.0�@ 2014/11/28 ���_���A���ɑ�񌴓_�ʒu�Ɉړ�����B
            Else
                ' SL432�n�̏ꍇ
                Call SLIDECOVERCHK(SLIDECOVER_CHECK_ON)                 ' �X���C�h�J�o�[�`�F�b�N���� ###088
                r = Form1.System1.Form_Reset(cGMODE_ORG, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                '----- V1.13.0.0�G�� -----
                If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then        ' �G���[ ?
                    sResetStart = r                                     ' Return�l�ݒ�
                    Exit Function
                End If
                ' ��񌴓_�ʒu�ɃX�e�[�W���ړ�����
                'V5.0.0.6�A                If (r = cFRS_NORMAL) And ((gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0)) Then
                'V5.0.0.6�A                r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                If (r = cFRS_NORMAL) And ((GetLoaderBordTableOutPosX() <> 0) Or (GetLoaderBordTableOutPosY() <> 0)) Then
                    r = Form1.System1.EX_SMOVE2(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                End If
                '----- V1.13.0.0�G�� -----
            End If
            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then            ' �G���[ ?
                sResetStart = r                                         ' Return�l�ݒ�
                Exit Function
            End If
            sResetStart = r

            ' ����SW�����҂�(��߼��)���ͽײ�޶�ް��������݂��Ȃ��̂�
            ' ү���ޕ\����ײ�޶�ް�J�҂�
            If ((Form1.System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0) And _
                ((gSysPrm.stSPF.giWithStartSw = 1) And gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then ' ����ۯ����Ž���SW�����҂�(��߼��) ?
                r = Form1.System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then        ' �G���[ ?
                    sResetStart = r                                     ' Return�l�ݒ�
                    Exit Function
                End If
            End If

            ' �N�����v/�z��OFF
            If (bFgAutoMode = False) Then                               ' �����^�]���̓N�����v�y�уo�L���[��OFF���Ȃ� ###107
                r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)
            End If

            'V5.0.0.9�M
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_IDLE)
            'V5.0.0.9�M

            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                sResetStart = r                                         ' Return�l�ݒ�
                Exit Function
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "gwModule.sResetStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            sResetStart = cERR_TRAP                                     ' Return�l = ��O�G���[
        End Try

    End Function
#End Region

#Region "���_�ʒu�ړ�(��Έړ�)"
    '''=========================================================================
    '''<summary>���_�ʒu�ړ�(��Έړ�)</summary>
    '''<returns>0:����, 0�ȊO:�G���[</returns> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Function sResetTrim() As Short

        Dim strMSG As String
        Dim r As Short

        Try
            ' ���_�ʒu�ړ�
            sResetTrim = cFRS_NORMAL                        ' Return�l = ����
            r = Form1.System1.Form_Reset(cGMODE_ORG_MOVE, gSysPrm, giAppMode, False, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            If (r <> cFRS_NORMAL) Then                      ' �G���[ ? 
                If (r <= cFRS_ERR_EMG) Then                 ' ����~�� ? 
                    Call Form1.AppEndDataSave()
                    Call Form1.AplicationForcedEnding()     ' �����I��
                End If
                sResetTrim = r                              ' Return�l�ݒ�
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "gwModule.sResetTrim() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "Z�ړ�(����/���)"
    '''=========================================================================
    '''<summary>Z�ړ�(����/���)</summary>
    '''<param name="z"> (INP)�ړ���</param> 
    '''<param name="MD">(INP)0=���Έړ�, 1=��Έړ�</param> 
    '''<returns>0=����, 0�ȊO:�G���[</returns>
    '''=========================================================================
    Public Function EX_ZMOVE(ByVal z As Double, ByVal MD As Integer) As Integer

        Dim r As Integer
        Dim opt As Integer                                  ' Z�����_���A�����׸�(0:��, 1:Z�� 2:Z��)
        Dim strMSG As String

        Try
            ' Z�Ȃ��Ȃ�NOP
            If (gSysPrm.stDEV.giPrbTyp = 0) Then
                EX_ZMOVE = cFRS_NORMAL
                Exit Function
            End If

            ' Z�����_���A�����׸ނ�ݒ肷��
            If (MD = 1) And (z = 0.0#) Then                 ' ���_�ړ� ?
                opt = 1                                     ' �׸� = Z�����_���A�����L��
            Else
                opt = 0                                     ' �׸� = Z�����_���A��������
            End If

            ' Z�ړ�
            r = ZZMOVE(z, MD)
            EX_ZMOVE = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)

            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "gwModule.EX_ZMOVE() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            EX_ZMOVE = cERR_TRAP                            ' Return�l = ��O�G���[
        End Try

    End Function

#End Region

#Region "Z2�ړ�(����/���)"
    '''=========================================================================
    '''<summary>Z2�ړ�(����/���)</summary>
    '''<param name="z"> (INP)�ړ���</param> 
    '''<param name="MD">(INP)0=���Έړ�, 1=��Έړ�</param> 
    '''<returns>0=����, 0�ȊO:�G���[</returns>
    '''=========================================================================
    Public Function EX_ZMOVE2(ByVal z As Double, ByVal MD As Integer) As Integer

        Dim r As Integer
        Dim opt As Integer                                  ' Z�����_���A�����׸�(0:��, 1:Z�� 2:Z��)
        Dim strMSG As String

        Try
            ' ������۰�ނȂ��Ȃ�NOP
            If ((gSysPrm.stDEV.giPrbTyp And 2) = 0) Then
                EX_ZMOVE2 = cFRS_NORMAL
                Exit Function
            End If

            ' Z�����_���A�����׸ނ�ݒ肷��
            If (MD = 1) And (z = 0.0#) Then                 ' ���_�ړ� ?
                opt = 1                                     ' �׸� = Z�����_���A�����L��
            Else
                opt = 0                                     ' �׸� = Z�����_���A��������
            End If

            ' Z2�ړ�
            r = ZZMOVE2(z, MD)
            EX_ZMOVE2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)

            Exit Function

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "gwModule.EX_ZMOVE2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            EX_ZMOVE2 = cERR_TRAP                           ' Return�l = ��O�G���[
        End Try

    End Function

#End Region

#Region "�ï��X,Y�̒l���Čv�Z����"
    '''=========================================================================
    '''<summary>�ï��X,Y�̒l���Čv�Z����</summary>
    '''<param name="intFirstCnt">(INP)</param>
    '''<param name="blnUpdate">  (INP)</param> 
    '''<returns></returns>
    '''=========================================================================
    Public Function GetTy2StepPos(ByRef intFirstCnt As Short, ByRef blnUpdate As Boolean) As Short

        Dim intForCnt As Short
        Dim fTblOffsetX As Double
        Dim fTblOffsetY As Double
        Dim blockx As Double
        Dim blocky As Double
        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim gspacex As Double
        Dim gspacey As Double
        Dim dblStepOffX As Double
        Dim dblStepOffY As Double
        Dim blockIntervalx As Double
        Dim blockIntervaly As Double
        Dim iCDir As Short
        Dim iStepRepeat As Short
        Dim BNX As Short
        Dim BNY As Short
        Dim GNx As Short
        Dim GNy As Short
        Dim BlkNum As Short
        Dim tSetNextXY As TRIM_GETNEXTXY                ' ����ۯ��ւ̈ړ��ʒu�擾�p�̍\����
        Dim dblX As Double
        Dim dblY As Double
        Dim intGrpCnt As Short
        Dim intCntBlockNum As Short
        Dim GrpNum As Short

        intGrpCnt = 1
        intCntBlockNum = 0

        ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
        iCDir = typPlateInfo.intResistDir
        ' �ï��&��߰Ă̎擾(0:�Ȃ��A1:X, 2:Y)
        iStepRepeat = typPlateInfo.intDirStepRepeat
        ' ð��وʒu�̾�Ă̎擾
        fTblOffsetX = typPlateInfo.dblTableOffsetXDir : fTblOffsetY = typPlateInfo.dblTableOffsetYDir

        ' ��ۯ����ނ̎擾
        Call CalcBlockSize(blockx, blocky)
        ' ���߻���X,Y
        dblCSx = typPlateInfo.dblChipSizeXDir
        dblCSy = typPlateInfo.dblChipSizeYDir
        ' ��ٰ�߲������X,Y
        gspacex = typPlateInfo.dblBpGrpItv
        gspacey = typPlateInfo.dblStgGrpItvY
        'gspacex = typPlateInfo.dblGroupItvXDir
        'gspacey = typPlateInfo.dblGroupItvYDir
        ' �ï�ߵ̾��X,Y
        dblStepOffX = typPlateInfo.dblStepOffsetXDir
        dblStepOffY = typPlateInfo.dblStepOffsetYDir

        ' ��ۯ����̎擾
        BNX = typPlateInfo.intBlockCntXDir
        BNY = typPlateInfo.intBlockCntYDir

        ' �O���[�v��X,Y
        GNx = typPlateInfo.intGroupCntInBlockXBp
        GNy = typPlateInfo.intGroupCntInBlockYStage
        ' ��ۯ��ԊuX,Y
        blockIntervalx = typPlateInfo.dblBlockItvXDir
        blockIntervaly = typPlateInfo.dblBlockItvYDir

        If iCDir = 1 Then
            BlkNum = BNX
        Else
            BlkNum = BNY
        End If

        If iCDir = 1 Then
            GrpNum = GNx
        Else
            GrpNum = GNy
        End If

        MaxTy2 = BlkNum
        intCntBlockNum = intFirstCnt - 1

        For intForCnt = intFirstCnt To BlkNum - 1

            With tSetNextXY
                .intCDir = iCDir
                .intStepR = iStepRepeat
                .dblblockx = blockx
                .dblblocky = blocky
                .dblCSx = dblCSx
                .dblCSy = dblCSy
                .dblgspacex = gspacex
                .dblgspacey = gspacey
                .dblStepOffX = dblStepOffX
                .dblStepOffY = dblStepOffY
                .intpxy1 = 0
                .intpxy2 = 0
                If iCDir = 1 Then
                    .intxy1 = 1
                    .intxy2 = 0
                Else
                    .intxy1 = 0
                    .intxy2 = 1
                End If

                .dblStrp = 0
                If GrpNum > 1 Then

                    If typStepInfoArray(1).intSP2 > 0 Then

                        If typStepInfoArray(intGrpCnt).intSP2 - 1 = intCntBlockNum Then

                            If intGrpCnt = 1 Then
                                .dblStrp = typStepInfoArray(intGrpCnt).dblSP3
                                If iCDir = 1 Then
                                    .intpxy1 = typStepInfoArray(intGrpCnt).intSP1
                                Else
                                    .intpxy2 = typStepInfoArray(intGrpCnt).intSP1
                                End If
                            Else

                                If iCDir = 1 Then
                                    .dblStrp = typStepInfoArray(intGrpCnt).dblSP3
                                    .intpxy1 = typStepInfoArray(intGrpCnt).intSP1
                                Else
                                    .dblStrp = typStepInfoArray(intGrpCnt).dblSP3
                                    .intpxy2 = typStepInfoArray(intGrpCnt).intSP1
                                End If
                            End If

                            intGrpCnt = intGrpCnt + 1

                            intCntBlockNum = -1

                        End If
                    End If

                End If

                .dblblockIntervalx = blockIntervalx
                .dblblockIntervaly = blockIntervaly

            End With

            ' ����ۯ��ړ��ʒu(X,Y)���擾����B
            Call GetNextBlockXYdata(tSetNextXY, dblX, dblY)
            typTy2InfoArray(intForCnt).intTy21 = intForCnt  ' �u���b�N�ԍ� 
            If iCDir = 0 Then                               ' �X�e�b�v���� 
                typTy2InfoArray(intForCnt).dblTy22 = dblY.ToString("0.0000")
            Else
                typTy2InfoArray(intForCnt).dblTy22 = dblX.ToString("0.0000")
            End If

            If blnUpdate Then
                ' ���L�͌���
                '' ''If frmDataEdit.FGridTy2.Rows > MaxTy2 And frmDataEdit.FGridTy2.Cols > 2 Then
                '' ''    ' X���W�ύX��
                '' ''    frmDataEdit.FGridTy2.set_TextMatrix(intForCnt, 1, typTy2InfoArray(intForCnt).dblTy22.ToString("0.0000"))
                '' ''End If
            End If
            intCntBlockNum = intCntBlockNum + 1
        Next

        typTy2InfoArray(BlkNum).dblTy22 = "0.0000"
    End Function

#End Region

#Region "������ۯ��ʒuX,Y�̒l�Z�o"
    '''=========================================================================
    '''<summary>������ۯ��ʒuX,Y�̒l�Z�o</summary>
    '''<param name="tSetNextXY"></param>
    '''<param name="X"></param>
    '''<param name="y"></param>
    '''<remarks>���ݸޏ�������XY�̎Z�o�������s�Ȃ�</remarks>
    '''=========================================================================
    Private Sub GetNextBlockXYdata(ByRef tSetNextXY As TRIM_GETNEXTXY, ByRef X As Double, ByRef y As Double)

        On Error GoTo ErrExit

        With tSetNextXY
            ' ���̃u���b�N��XY�ړ�
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    ' ���ߕ������������s���B
                    If (0 = .intCDir) Then              ' X����
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp

                    Else                                ' Y����
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If

                    End If

                Case 1 ' x��, y��

                    ' ���ߕ������������s���B
                    If (0 = .intCDir) Then              ' X����
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp
                    Else                                ' Y����
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If

                    End If

                Case 2 ' x��, y��
                    ' ���ߕ������������s���B
                    If (0 = .intCDir) Then              ' X����
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp
                    Else                                ' Y����
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If

                    End If

                Case 3 ' x��, y��
                    '���ߕ������������s���B
                    If (0 = .intCDir) Then              ' X����
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (3 = .intStepR) Then         ' 1��R���炵
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp
                    Else                                ' Y����
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' �ï��&��߰Ă̊m�F���s�Ȃ��B
                        If (4 = .intStepR) Then         ' 1��R���炵
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If

                    End If

            End Select

        End With

        Exit Sub

        '''''   2009/07/02 minato
        ''''        ���L��NET�̏����B
        ''''        NET��CHIP�ŏ������قȂ邽�߁A��قǃR�[�h���}�[�W
        ''''    With tSetNextXY
        ''''
        ''''        ' ���̃u���b�N��XY�ړ�
        ''''        Select Case giBpDirXy
        ''''
        ''''            Case 0:     ' x��, y��
        ''''
        ''''                    ''���ߕ������������s���B
        ''''                    If (0 = .intCDir) Then         ' X����
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (3 = .intStepR) Then         ' 1��R���炵
        ''''                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        End If
        ''''                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
        ''''                    Else  'Y����
        ''''                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblStrp + .dblADDSZX
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (4 = .intStepR) Then         ' 1��R���炵
        ''''                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        End If
        ''''
        ''''                    End If
        ''''            Case 1:     ' x��, y��
        ''''
        ''''                    ''���ߕ������������s���B
        ''''                    If (0 = .intCDir) Then         ' X����
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (3 = .intStepR) Then         ' 1��R���炵
        ''''                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        End If
        ''''                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
        ''''                    Else  'Y����
        ''''                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (4 = .intStepR) Then         ' 1��R���炵
        ''''                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        End If
        ''''                    End If
        ''''
        ''''            Case 2:     ' x��, y��
        ''''                    ''���ߕ������������s���B
        ''''                    If (0 = .intCDir) Then         ' X����
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (3 = .intStepR) Then         ' 1��R���炵
        ''''                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        End If
        ''''
        ''''                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
        ''''                    Else  'Y����
        ''''                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblADDSZX
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (4 = .intStepR) Then         ' 1��R���炵
        ''''                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        End If
        ''''                    End If
        ''''
        ''''            Case 3:     ' x��, y��
        ''''
        ''''                    ''���ߕ������������s���B
        ''''                    If (0 = .intCDir) Then         ' X����
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (3 = .intStepR) Then         ' 1��R���炵
        ''''                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        End If
        ''''                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
        ''''                    Else  'Y����
        ''''                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
        ''''
        ''''                        ''�ï��&��߰Ă̊m�F���s�Ȃ��B
        ''''                        If (4 = .intStepR) Then         ' 1��R���炵
        ''''                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        End If
        ''''
        ''''                    End If
        ''''
        ''''        End Select
        ''''
        ''''    End With
        ''''    Exit Sub

ErrExit:
        MsgBox("GetNextBlockXYdata" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub

#End Region

#Region "���ײĐ���(�i�d�l)"
    '''=========================================================================
    '''<summary>���ײĐ���(�i�d�l)</summary>
    '''<param name="MODE">(INP) True=ON, False=OFF</param>
    '''=========================================================================
    Public Sub PATO_IO_OUT(ByRef MODE As Boolean)

        Dim strDATA As String

        ' �i�d�l
        If gSysPrm.stCTM.giSPECIAL = customSUSUMU Then
            If MODE = True Then
                strDATA = "&H1"                         ' ON
            Else
                strDATA = "&H0"                         ' OFF
            End If

#If NOTINTIME Then
#Else
#If cOFFLINEcDEBUG = 1 Then
#Else
            Call OUT16(Val("&H21f8"), Val(strDATA))
#End If
#End If
        End If
    End Sub

#End Region

#Region "GP-IB����������ލ쐬����"
    '''=========================================================================
    '''<summary>GP-IB����������ލ쐬����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub MakeGPIBinit()

        Dim strIniData As String

        With typPlateInfo
            strIniData = ""

            ' ���葬�x�̐ݒ�
            If .intGpibMeasSpeed = 0 Then                       ' �ݒ葬�x=�ᑬ
                strIniData = strIniData & "W0"
            Else ' �ݒ葬�x=����
                strIniData = strIniData & "W1"
            End If

            ' ���胂�[�h�Ő؂�ւ�
            If .intGpibMeasMode = 0 Then
                strIniData = strIniData & "FR"                  ' ���胂�[�h=���
                strIniData = strIniData & "LL00000" & "LH15000" ' ����/������~�b�g�̐ݒ�
            Else
                strIniData = strIniData & "FD"                  ' ���胂�[�h=�΍�
                strIniData = strIniData & "DL-5000" & "DH+5000" ' ����/������~�b�g�̐ݒ�
            End If

            .strGpibInitCmnd1 = strIniData
            .strGpibInitCmnd2 = ""

        End With

    End Sub

#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090605 minato
    ''''    �ePrivate�֐����狤�ʊ֐��Ƃ��Ē�`�B
    ''''    OcxUtility�֎������邩�͍��㌟��
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

#Region "�l�̌ܓ����s��"
    '''=========================================================================
    '''<summary>�l�̌ܓ����s��</summary>
    '''<param name="dblSrc">���ް�</param>
    '''<returns>�����ް�</returns>
    '''=========================================================================
    Public Function RoundOff(ByRef dblSrc As Double) As Double
        Dim str_Renamed As String

        str_Renamed = dblSrc.ToString("0.0000")
        RoundOff = CDbl(str_Renamed)

    End Function
#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090605 minato
    ''''    �ePrivate�֐����狤�ʊ֐��Ƃ��Ē�`�B
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    '==========================================================
#Region "����W�̒�����ð��وړ�"
    '''=========================================================================
    '''<summary>����W�̒�����ð��وړ�</summary>
    '''<param name="dblStd1X">(INP)����W1��X</param>
    '''<param name="dblStd1Y">(INP)����W1��Y</param>
    '''<param name="dblStd2X">(INP)����W2��X</param>
    '''<param name="dblStd2Y">(INP)����W2��Y</param>  
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function XYTableMoveCenter(ByRef dblStd1X As Double, ByRef dblStd1Y As Double, ByRef dblStd2X As Double, ByRef dblStd2Y As Double) As Short

        Dim r As Integer
        Dim dblX As Double
        Dim dblY As Double
        Dim dblRotX As Double
        Dim dblRotY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim dblBPX As Double
        Dim dblBPY As Double
        Dim intCDir As Short
        Dim intTableType As Short
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim dblTOffsX As Double
        Dim dblTOffsY As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim strMSG As String

        Try

            r = cFRS_NORMAL
            dblRotX = 0
            dblRotY = 0

            ' �f�[�^�擾
            ''''(2010/11/16) ����m�F�㉺�L�R�����g�͍폜
            'dblTrimPosX = gStartX                           ' ����߼޼��X,Y�擾
            'dblTrimPosY = gStartY
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                 ' ����߼޼��X,Y�擾
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            dblTOffsX = typPlateInfo.dblCaribTableOffsetXDir    ' �����ڰ���ð��ٵ̾��
            dblTOffsY = typPlateInfo.dblCaribTableOffsetYDir
            Call CalcBlockSize(dblBSX, dblBSY)                  ' ��ۯ����ގZ�o
            intTableType = gSysPrm.stDEV.giXYtbl
            intCDir = typPlateInfo.intResistDir                 ' �`�b�v���ѕ����擾(CHIP-NET�̂�)

            ' �ƕ␳��ѵ̾��X,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            If ((dblStd2X >= dblStd1X) And (dblStd2Y >= dblStd1Y)) Then

                dblBPX = dblBSX / 2
                dblBPY = dblBSY / 2
                ' giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
                ' 0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
                ' ���ݸވʒu���W + �����ڰ���ð��ٵ̾�� + �␳�ʒu (+or-) ��]���a (+or-) ð��ٕ␳��
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 ' x��, y��
                        dblX = dblTrimPosX + dblTOffsX + Del_x + dblRotX + dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y + dblRotY + dblBPY

                    Case 1 ' x��, y��
                        dblX = dblTrimPosX + dblTOffsX + Del_x - dblRotX - dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y + dblRotY + dblBPY

                    Case 2 ' x��, y��
                        dblX = dblTrimPosX + dblTOffsX + Del_x + dblRotX + dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y - dblRotY - dblBPY

                    Case 3 ' x��, y��
                        dblX = dblTrimPosX + dblTOffsX + Del_x - dblRotX - dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y - dblRotY - dblBPY
                End Select

                ' �e�[�u����Βl�ړ�
                r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)

            End If
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "gwModule.XYTableMoveCenter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '''''=========================================================================
    '''''�y�@�@�\�z�擪��ۯ��̒����ֈړ�
    '''''�y���@���z---
    '''''�y�߂�l�z(True)OK (False)NG
    '''''�y���@�L�z
    '''''=========================================================================
    ''''Public Function XYTableMoveTopBlock(x_tblOff As Double, y_tblOff As Double) As Boolean
    ''''
    ''''    Dim bRetc As Boolean        '�߂�l
    ''''    Dim dblTrimPosX As Double   '����߼޼��X
    ''''    Dim dblTrimPosY As Double   '����߼޼��Y
    ''''    Dim dblBSX As Double        '��ۯ�����X
    ''''    Dim dblBSY As Double        '��ۯ�����Y
    ''''    Dim dblBsoX As Double       '��ۯ����޵̾��X
    ''''    Dim dblBsoY As Double       '��ۯ����޵̾��Y
    ''''    Dim Del_x As Double         '�ƕ␳��X
    ''''    Dim Del_y As Double         '�ƕ␳��Y
    ''''    Dim dblRotX As Double       '��]���aX
    ''''    Dim dblRotY As Double       '��]���aY
    ''''    Dim dblX As Double          '�ړ����WX
    ''''    Dim dblY As Double          '�ړ����WY
    ''''
    ''''    bRetc = False
    ''''    dblRotX = 0
    ''''    dblRotY = 0
    ''   ' ����߼޼��X,Y�擾
    ''       dblTrimPosX = gStartX
    ''       dblTrimPosY = gStartY
    ''''''    Call CalcBlockSize(dblBSX, dblBSY)              '��ۯ����ގZ�o
    '''''
    ''''    '��ۯ����޵̾�ĎZ�o(��ۯ�����/2 ��ۯ��̏ی���XY�Ƃ���1 ð��ق̏ی���1)
    ''''    dblBsoX = (dblBSX / 2#) * 1 * 1                 'Table.BDirX * Table.dir
    ''''    dblBsoY = (dblBSY / 2) * 1                      'Table.BDirY;
    '' '' �ƕ␳��ѵ̾��X,Y
    ' ''    Del_x = gfCorrectTrimPosX
    ' ''    Del_y = gfCorrectTrimPosY
    ''''    'giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
    ''''    '0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
    ''''    '���ݸވʒu���W (+or-) ��]���a + ð��ٵ̾�� (+or-) ��ۯ����޵̾�� + ð��ٕ␳��
    ''''    Select Case giBpDirXy
    ''''        Case 0          ' x��, y��
    ''''            dblX = dblTrimPosX + dblRotX + x_tblOff + dblBsoX + Del_x
    ''''            dblY = dblTrimPosY + dblRotY + y_tblOff + dblBsoY + Del_y
    ''''        Case 1          ' x��, y��
    ''''            dblX = dblTrimPosX - dblRotX + x_tblOff - dblBsoX + Del_x
    ''''            dblY = dblTrimPosY + dblRotY + y_tblOff + dblBsoY + Del_y
    ''''        Case 2          ' x��, y��
    ''''            dblX = dblTrimPosX + dblRotX + x_tblOff + dblBsoX + Del_x
    ''''            dblY = dblTrimPosY - dblRotY + y_tblOff - dblBsoY + Del_y
    ''''        Case 3          ' x��, y��
    ''''            dblX = dblTrimPosX - dblRotX + x_tblOff - dblBsoX + Del_x
    ''''            dblY = dblTrimPosY - dblRotY + y_tblOff - dblBsoY + Del_y
    ''''    End Select
    ''''
    ''''    KJPosX = dblX
    ''''    KJPosY = dblY
    ''''    Call XYtableMove(dblX, dblY)
    ''''    bRetc = True
    ''''    XYTableMoveTopBlock = bRetc
    ''''End Function

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090608 minato
    ''''    ���W���[���֐��֕ύX���邽�߂Ɉړ�
    ''''    GlbChip.bas���
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

#Region "������ҏW"
    '''=========================================================================
    '''<summary>������ҏW</summary>
    '''<param name="sD1">(INP)������1</param>
    '''<param name="sD2">(INP)������2</param>
    '''<param name="sD3">(INP)������3</param>
    '''<param name="sD4">(INP)������4</param>  
    '''<returns>�ҏW��̕�����</returns>
    '''=========================================================================
    Public Function StrMSGReplace(ByRef sD1 As String, ByRef sD2 As Object, ByRef sD3 As Object, ByRef sD4 As String) As String

        Dim sBuff As String

        'Select Case gSysPrm.stTMN.giMsgTyp
        '    Case 0                                      ' Japanese
        '        sBuff = sD1 & sD2 & " �` " & sD3 & sD4
        '    Case Else                                   ' US/EU
        '        sBuff = sD1 & sD2 & " and " & sD3 & sD4
        'End Select
        sBuff = sD1 & sD2 & gwModule_001 & sD3 & sD4
        StrMSGReplace = sBuff

    End Function
#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090415 minato
    ''''    ���W���[���֐��֕ύX���邽�߂Ɉړ�
    ''''    tky.bas���
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/



    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090417 minato
    ''''    basTheta.bas���ړ��BbasTheta�͍폜����
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

    '-------------------------------------------------------------------------------
    '   �Ǝ��̕␳���s��
    '-------------------------------------------------------------------------------
    Public Function CorrectTheta() As Short

        '----------- Video.OCX���g�p����悤�ɏC��

        '        Dim reviseMode As Short
        '        Dim manualReviseType As Short
        '        Dim r As Short

        '        reviseMode = typPlateInfo.intReviseMode ' �␳���[�h
        '        manualReviseType = typPlateInfo.intManualReviseType ' �蓮�␳�^�C�v
        '        gbRotCorrectCancel = 0

        '        Dim mCorrect As System.Windows.Forms.Form
        '        If reviseMode = 1 And manualReviseType = 0 Then

        '        Else
        '            ' ADV�{�^�������҂��AZ,BP,XY �����ʒu�ړ�
        '            '        gMode = 7
        '            '        frmReset.Show vbModal
        '            r = Form1.System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, LoadDTFlag)
        '            If (r <= cFRS_ERR_EMG) Then
        '                '�����I��
        '                Call Form1.AppEndDataSave()
        '                Call Form1.AplicationForcedEnding()
        '            ElseIf (r = cFRS_ERR_RST) Then
        '                ' ���Z�b�g�@or �\�t�g��̗�O�G���[����
        '                CorrectTheta = r
        '                Exit Function
        '            End If

        '#If VIDEO_CAPTURE_OVERLAY Then
        '			If gVideoStarted Then
        '			' �I�[�o�[���COFF
        '			Call Trim_OverLayOff
        '			Form1.Refresh
        '			End If
        '#End If

        '            mCorrect = New CorrectPos
        '            If gbRotCorrectCancel = 0 Then
        '                mCorrect.ShowDialog()
        '            End If
        '            'UPGRADE_NOTE: �I�u�W�F�N�g mCorrect ���K�x�[�W �R���N�g����܂ł��̃I�u�W�F�N�g��j�����邱�Ƃ͂ł��܂���B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' ���N���b�N���Ă��������B
        '            mCorrect = Nothing

        '            Form1.Refresh()

        '#If VIDEO_CAPTURE_OVERLAY Then
        '			If gVideoStarted Then
        '			Call Trim_OverLayOn
        '			End If
        '#End If
        '        End If

        '        If gbRotCorrectCancel = 0 Then
        '            ' ����I��
        '        Else
        '            ' �p�^�[���}�b�`NG or ���[�U�[�L�����Z��
        '        End If

    End Function

#Region "BP����ۯ��̉E��Ɉړ�����"
    '''=========================================================================
    '''<summary>BP����ۯ��̉E��Ɉړ�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub BpMoveOrigin_Ex()

        Dim dblBpOffsX As Double
        Dim dblBpOffsY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double

        ' ��ۯ����ގ擾
        Call CalcBlockSize(dblBSX, dblBSY)
        ' BP�ʒu�̾��X,Y�ݒ�
        dblBpOffsX = typPlateInfo.dblBpOffSetXDir
        dblBpOffsY = typPlateInfo.dblBpOffSetYDir
        ' BP����ۯ��̉E��Ɉړ�����(BSIZE()��BPOFF()���s)
        Call Form1.System1.BpMoveOrigin(gSysPrm, dblBSX, dblBSY, dblBpOffsX, dblBpOffsY)

    End Sub
#End Region

    '----- �ȉ��A���g�p�̂��ߍ폜

    '#Region "HaltSw�ňꎞ��~��̃R���\�[�����͎擾����"
    '    '==========================================================
    '    '(2011/06/13)
    '    '   frmPtnCalibration�ȂǗl�X���œ�������������B
    '    '   ���b�Z�[�W�\�������ɋ��ʐ����������A���ʂ̃��W���[���Ƃ���
    '    '   �g�p�ł���悤�ɕύX��AgwModule�ֈړ�������
    '    '
    '    '   FUNC    :   HaltSw �������̃R���\�[�����͑҂�
    '    '   PARAM   :
    '    '   RET     :   
    '    '               cSTS_STARTSW_ON As Integer = 1  ' START�X�C�b�`ON
    '    '               cSTS_RESETSW_ON As Integer = 3  ' RESET�X�C�b�`ON
    '    '   COMMENT :
    '    '==========================================================
    '    Public Function HaltSwMoveWait(ByVal objCtrl As Object, ByVal lblMsg As String) As Integer

    '        Dim sts As Integer
    '        Dim ret As Integer
    '        HaltSwMoveWait = 0

    '        Try
    '            'If Form1.System1.AdjReqSw() Then
    '            ret = HALT_SWCHECK(sts)
    '            If sts = cSTS_HALTSW_ON Then
    '                objCtrl.Text = lblMsg
    '                'Me.lblInfo.Text = lblMsg
    '                sts = cSTS_STARTSW_ON
    '#If cOFFLINEcDEBUG = 1 Then
    '                If (MsgBoxResult.Ok = MsgBox("OK=START CANCEL=RESET", MsgBoxStyle.OkCancel)) Then
    '                    sts = cSTS_STARTSW_ON
    '                Else
    '                    sts = cSTS_RESETSW_ON 
    '                End If
    '#Else
    'RETRY_SWWAIT:
    '                ret = STARTRESET_SWWAIT(sts)
    '                If (sts = cSTS_STARTSW_ON) Then
    '                    '���̂܂܎��s
    '                ElseIf (sts = cSTS_RESETSW_ON) Then
    '                    Call LAMP_CTRL(LAMP_RESET, True)
    '                    If Form1.System1.TrmMsgBox(gSysPrm, "OK:[START] CANCEL:[HALT]", _
    '                            MsgBoxStyle.OkCancel, FRM_CALIBRATION_TITLE) Then
    '                        sts = cSTS_NOSW_ON
    '                    Else
    '                        '�ēx�҂���Ԃɓ���B
    '                        GoTo RETRY_SWWAIT
    '                    End If
    '                    Call LAMP_CTRL(LAMP_RESET, False)
    '                End If
    '#End If
    '                'Call Form1.System1.sLampOnOff(LAMP_HALT, False)
    '                Call LAMP_CTRL(LAMP_HALT, False)
    '            End If

    '            HaltSwMoveWait = sts

    '        Catch ex As Exception
    '            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
    '        End Try
    '    End Function
    '#End Region


#Region "�^�[���A�ؑփ|�C���g�̃p�����[�^��INtime���ɑ��M����"
    '''=========================================================================
    '''<summary>�^�[���A�ؑփ|�C���g�p�����[�^��INtime���ɑ��M����</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function SendTrimDataCutPoint() As Short

        Dim r As Integer
        Dim strMSG As String
        Dim strName As String
        Dim strDirName As String
        Dim CutPointFilePath As String

        Try

            If giChangePoint = 0 Then
                Return cFRS_NORMAL
            End If

            strName = ""
            CutPointTable.Initialize()
            '�f�[�^�t�@�C���̓��e��ǂݍ���ŁA�\���̃e�[�u���ɃZ�b�g
            strName = System.IO.Path.GetFileNameWithoutExtension(gStrTrimFileName)
            strDirName = System.IO.Path.GetDirectoryName(gStrTrimFileName)
            CutPointFilePath = strDirName + "\" + strName + ".cut"
            ReadCutDataFile(CutPointFilePath, CutPointTable)

            ' �p�����[�^��INtime���ɑ��M����
            r = LaserFront.Trimmer.DefTrimFnc.CUTPOINTPARAM(CutPointTable)
            If (r <> cFRS_NORMAL) Then                                          ' ���M���s ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "SendTrimDataCutPoint Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutPoint = 1
            End If

            Exit Function


            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutPoint() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutPoint = cERR_TRAP
        End Try

    End Function

    ''' <summary>
    ''' �w��̃t�@�C������ؑփ|�C���g�A�^�[���|�C���g��ǂݍ���
    ''' </summary>
    ''' <param name="CutPointFilePath"></param>
    ''' <param name="CutPointTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadCutDataFile(ByVal CutPointFilePath As String, ByRef CutPointTable As LaserFront.Trimmer.DefTrimFnc.CUTPOINTCALC) As Integer
        Dim intFileNo As Integer
        Dim strDAT As String
        Dim loopcnt As Integer
        Dim stArraData As String()
        Dim strMSG As String
        Dim tmpStr As String


        Try
            loopcnt = 0
            ' �e�L�X�g�t�@�C�����I�[�v������
            intFileNo = FreeFile()                                  ' �g�p�\�ȃt�@�C���i���o�[���擾
            FileOpen(intFileNo, CutPointFilePath, OpenMode.Input)

            ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ��܂��B
            Do While Not EOF(intFileNo)
                strDAT = LineInput(intFileNo)                       ' 1�s�ǂݍ���
                tmpStr = strDAT.Substring(0, 1)
                If (tmpStr = ";") Then
                    Continue Do
                End If
                stArraData = strDAT.Split(",")
                CutPointTable.dblCutInitParam(loopcnt) = stArraData(1)
                CutPointTable.dblCutParam(loopcnt) = stArraData(2)
                loopcnt = loopcnt + 1
                If loopcnt >= 10 Then
                    Exit Do
                End If
            Loop
            CutPointTable.func = giChangePoint

            FileClose(intFileNo)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "i-TKY.ReadCutDataFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            ReadCutDataFile = cERR_TRAP
        End Try

    End Function

    ''' <summary>
    ''' �^�[���|�C���g�A�ؑփ|�C���g�t�@�C�����쐬
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MakeutPointFileData(ByVal gStrTrimFileName As String) As String
        Dim strName As String
        Dim strDirName As String

        MakeutPointFileData = ""

        '�f�[�^�t�@�C���̓��e��ǂݍ���ŁA�\���̃e�[�u���ɃZ�b�g
        strName = System.IO.Path.GetFileNameWithoutExtension(gStrTrimFileName)
        strDirName = System.IO.Path.GetDirectoryName(gStrTrimFileName)
        MakeutPointFileData = strDirName + "\" + strName + ".cut"

        Return MakeutPointFileData

    End Function

#End Region

End Module