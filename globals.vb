'===============================================================================
'   Description  : �O���[�o���萔�̒�`
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Runtime.CompilerServices                                 ' V6.0.3.0�L For Extension()
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc                                  'V4.9.0.0�@
Imports LaserFront.Trimmer.DllJog                                       'V6.0.0.0�G
Imports LaserFront.Trimmer.DllSysPrm.SysParam
Imports LaserFront.Trimmer.TrimData.DataManager                         'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources                                    'V4.4.0.0-0
Imports TrimClassLibrary.IniFile.Tky                                    'V2.0.0.0�I
Imports TrimControlLibrary                                              '#4.12.2.0�@

Module Globals_Renamed

#Region "�O���[�o���萔/�ϐ��̒�`"
    '===========================================================================
    '   �O���[�o���萔/�ϐ��̒�`
    '===========================================================================
    '-------------------------------------------------------------------------------
    '   DLL��`
    '-------------------------------------------------------------------------------
    '----- WIN32 API -----
    ' �E�B���h�E�\���̑����API
    Public Declare Function SetWindowPos Lib "user32" (ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
    Public Const HWND_TOPMOST As Short = -1                 ' �E�B���h�E���őO�ʂɕ\��
    Public Const SWP_NOSIZE As Short = &H1S                 ' ���݂̃T�C�Y���ێ�
    Public Const SWP_NOMOVE As Short = &H2S                 ' ���݂̈ʒu���ێ�

    '---------------------------------------------------------------------------
    '   �A�v���P�[�V������/�A�v���P�[�V�������/�A�v���P�[�V�������[�h
    '---------------------------------------------------------------------------
    '----- �����I���p�A�v���P�[�V���� -----
    Public Const APP_FORCEEND As String = "c:\Trim\ForceEndProcess.exe"

    '----- �p�X�@-----
    Public Const OCX_PATH As String = "c:\Trim\ocx\"        '----- OCX�o�^�p�X
    Public Const DLL_PATH As String = "c:\Trim\"            '----- DLL�o�^�p�X

    Friend Const TKY_INI As String = DLL_PATH & "tky.ini"               'V6.0.0.1�A
    Friend Const TKYSYS_INI As String = DLL_PATH & "TKYSYS.INI"         'V6.0.0.1�A

    '----- �A�v���P�[�V������ -----
    Public Const APP_TKY As String = "TKY"
    Public Const APP_CHIP As String = "TKYCHIP"
    Public Const APP_NET As String = "TKYNET"

    '----- �A�v���P�[�V������� -----
    Public Const KND_TKY As Short = 0
    Public Const KND_CHIP As Short = 1
    Public Const KND_NET As Short = 2

    Public Const MACHINE_TYPE_SL432 As String = "SL432R"                 ' �n��
    Public Const MACHINE_TYPE_SL436 As String = "SL436R"                 ' �n��
    Public Const MACHINE_TYPE_SL436S As String = "SL436S"                ' �n��

    '----- V1.13.0.0�F�� -----
    ' ���u�^�C�v(0=Sl43xR, 1=SL432RW)
    Public Const KEY_TYPE_R As Short = 0
    Public Const KEY_TYPE_RW As Short = 1
    Public Const KEY_TYPE_RS As Short = 2                               'V2.0.0.0�I
    '----- V1.13.0.0�F�� -----

    Public gAppName As String                               ' �A�v���P�[�V������
    Public gTkyKnd As Short                                 ' �A�v���P�[�V�������

    '----- �摜�\���v���O�����̕\���ʒu -----
    'Public Const FORM_X As Integer = 4                     ' �R���g���[���㕔���[���WX ###050
    'Public Const FORM_Y As Integer = 20                    ' �R���g���[���㕔���[���WY ###050
    Public Const FORM_X As Integer = 0                      ' �R���g���[���㕔���[���WX ###050
    Public Const FORM_Y As Integer = 0                      ' �R���g���[���㕔���[���WY ###050

    '�T�u�t�H�[���̕\���ʒu�ڈ�̕\���ʒu�I�t�Z�b�g
    'Public Const DISPOFF_SUBFORM_TOP As Integer = 12
    Public Const DISPOFF_SUBFORM_TOP As Integer = 0         'V4.10.0.0�B

    '----- �V�O�i���^���[������ -----                     ' ###007
    Public Const SIGTOWR_NORMAL As Short = 0                ' �W���R�F����
    Public Const SIGTOWR_SPCIAL As Short = 1                ' �S�F����(�������Ӱè�ޓa����)

    '----- �A�v���P�[�V�������[�h ----- (��)OcxSystem��`�ƈ�v������K�v�L��
    'V5.0.0.9�P    Public giAppMode As Short
    Private _giAppMode As Short         'V5.0.0.9�P  ��
    Public Property giAppMode As Short
        Get
            Return _giAppMode
        End Get
        Set(value As Short)
            If (Form1.Instance IsNot Nothing) Then
                Select Case (value)
                    Case APP_MODE_IDLE, APP_MODE_LOAD, APP_MODE_SAVE, APP_MODE_EDIT,
                        APP_MODE_LOGGING, APP_MODE_FINEADJ, APP_MODE_LDR_ALRM

                        Form1.Instance.SetLblDoorOpen(True)

                    Case Else
                        Form1.Instance.SetLblDoorOpen(False)
                End Select
            End If
            _giAppMode = value
        End Set
    End Property                        'V5.0.0.9�P  ��

    Public Const APP_MODE_IDLE As Short = 0                 ' �g���}���u�A�C�h����
    Public Const APP_MODE_LOAD As Short = 1                 ' �t�@�C�����[�h(F1)
    Public Const APP_MODE_SAVE As Short = 2                 ' �t�@�C���Z�[�u(F2)
    Public Const APP_MODE_EDIT As Short = 3                 ' �ҏW���      (F3)
    '                                                       ' ��
    Public Const APP_MODE_LASER As Short = 5                ' ���[�U�[����  (F5)
    Public Const APP_MODE_LOTCHG As Short = 6               ' ���b�g�ؑ�    (F6) �����[�U�v���Ή�
    Public Const APP_MODE_PROBE As Short = 7                ' �v���[�u      (F7)
    Public Const APP_MODE_TEACH As Short = 8                ' �e�B�[�`���O  (F8)
    Public Const APP_MODE_RECOG As Short = 9                ' �p�^�[���o�^  (F9)
    Public Const APP_MODE_EXIT As Short = 10                ' �I�� �@�@�@�@ (F11)
    Public Const APP_MODE_TRIM As Short = 11                ' �g���~���O��
    Public Const APP_MODE_TRIM_AUTO As Short = 111          ' �����^�]�̃g���~���O�� 'V4.7.0.0�P
    Public Const APP_MODE_CUTPOS As Short = 12              ' ��Ĉʒu�␳   (S-F8)
    Public Const APP_MODE_PROBE2 As Short = 13              ' �v���[�u2     (F10) �����[�U�v���Ή�
    Public Const APP_MODE_LOGGING As Short = 14             ' ���M���O      (F6) 
    '----- V1.13.0.0�B�� -----
    ' TKY�n�I�v�V����
    Public Const APP_MODE_APROBEREC As Short = 15           ' �I�[�g�v���[�u�o�^
    Public Const APP_MODE_APROBEEXE As Short = 16           ' �I�[�g�v���[�u���s
    Public Const APP_MODE_IDTEACH As Short = 17             ' �h�c�e�B�[�`���O
    Public Const APP_MODE_SINSYUKU As Short = 18            ' �L�k�␳(�摜�o�^)
    Public Const APP_MODE_MAP As Short = 19                 ' �L�k�␳(���s�u���b�N�I��)
    '----- V1.13.0.0�B�� -----

    Public Const APP_MODE_MAINTENANCE As Short = 20         ' �����e�i���X���[�h
    Public Const APP_MODE_PROBE_CLEANING As Short = 21      ' �v���[�u�N���[�j���O�@�\�e�B�[�`���O 

    ' CHIP,NET�n
    Public Const APP_MODE_TTHETA As Short = 40              ' �s��(�Ɗp�x�␳)�e�B�[�`���O
    Public Const APP_MODE_TX As Short = 41                  ' TX�e�B�[�`���O
    Public Const APP_MODE_TY As Short = 42                  ' TY�e�B�[�`���O
    Public Const APP_MODE_TY2 As Short = 43                 ' TY2�e�B�[�`���O
    Public Const APP_MODE_EXCAM_R1TEACH As Short = 44       ' �O���J����R1�e�B�[�`���O�y�O���J�����z
    Public Const APP_MODE_EXCAM_TEACH As Short = 45         ' �O���J�����e�B�[�`���O�y�O���J�����z
    Public Const APP_MODE_CARIB_REC As Short = 46           ' �摜�o�^(�L�����u���[�V�����␳�p)�y�O���J�����z
    Public Const APP_MODE_CARIB As Short = 47               ' �L�����u���[�V�����y�O���J�����z
    Public Const APP_MODE_CUTREVISE_REC As Short = 48       ' �摜�o�^(�J�b�g�ʒu�␳�p)�y�O���J�����z
    Public Const APP_MODE_CUTREVIDE As Short = 49           ' �J�b�g�ʒu�␳�y�O���J�����z
    Public Const APP_MODE_AUTO As Short = 50                ' �����^�]�@�@�@
    Public Const APP_MODE_LOADERINIT As Short = 51          ' ���[�_���_���A
    Public Const APP_MODE_LDR_ALRM As Short = 52            ' ���[�_�A���[�����    '###088
    Public Const APP_MODE_FINEADJ As Short = 53             ' �ꎞ��~���          '###088

    Public Const APP_MODE_INTEGRATED As Short = 54           ' �����o�^����   'V4.10.0.0�B

    ' NET�n
    Public Const APP_MODE_CIRCUIT As Short = 60             ' �T�[�L�b�g�e�B�[�`���O

    '---------------------------------------------------------------------------
    '----- �@�\�I���`�e�[�u���̲��ޯ����` -----          '                         TKY CHIP NET
    '                                                       '                (��:�W��,��:��߼��,�~:����߰�)
    Public Const F_LOAD As Short = 0                        ' LOAD�{�^��              ��  ��   ��
    Public Const F_SAVE As Short = 1                        ' SAVE�{�^��              ��  ��   ��
    Public Const F_EDIT As Short = 2                        ' EDIT�{�^��              ��  ��   ��
    Public Const F_LASER As Short = 3                       ' LASER�{�^��             ��  ��   ��
    Public Const F_LOG As Short = 4                         ' LOGGING�{�^��           ��  ��   ��
    Public Const F_PROBE As Short = 5                       ' PROBE�{�^��             ��  ��   ��
    Public Const F_TEACH As Short = 6                       ' TEACH�{�^��             ��  ��   ��
    Public Const F_CUTPOS As Short = 7                      ' CUTPOS�{�^��            ��  �~   �~
    Public Const F_RECOG As Short = 8                       ' RECOG�{�^��             ��  ��   ��
    ' CHIP,NET�n
    Public Const F_TTHETA As Short = 9                      ' T�ƃ{�^��               �~  ��   ��
    Public Const F_TX As Short = 10                         ' TX�{�^��                �~  ��   ��
    Public Const F_TY As Short = 11                         ' TY�{�^��                �~  ��   ��
    Public Const F_TY2 As Short = 12                        ' TY2�{�^��               �~  ��   ��
    Public Const F_EXR1 As Short = 13                       ' �O�����R1è��ݸ�����    �~  ��   ��
    Public Const F_EXTEACH As Short = 14                    ' �O�����è��ݸ�����      �~  ��   ��
    Public Const F_CARREC As Short = 15                     ' �����ڰ��ݕ␳�o�^����  �~  ��   ��
    Public Const F_CAR As Short = 16                        ' �����ڰ�������          �~  ��   ��
    Public Const F_CUTREC As Short = 17                     ' ��ĕ␳�o�^����         �~  ��   ��
    Public Const F_CUTREV As Short = 18                     ' ��Ĉʒu�␳����         �~  ��   ��
    ' NET�n
    Public Const F_CIRCUIT As Short = 19                    ' �����è��ݸ�����        �~  �~   ��

    ' SL436R CHIP,NET�n 
    Public Const F_AUTO As Short = 20                       ' AUTO�{�^��              -   ��   ��
    Public Const F_LOADERINI As Short = 21                  ' LOADER INIT�{�^��       -   ��   ��

    '----- V1.13.0.0�B�� -----
    ' TKY�n�I�v�V����
    Public Const F_APROBEREC As Short = 22                  ' ��ăv���[�u�o�^����     ��  �~   �~
    Public Const F_APROBEEXE As Short = 23                  ' ��ăv���[�u���s����     ��  �~   �~
    Public Const F_IDTEACH As Short = 24                    ' IDè��ݸ�����           ��  �~   �~
    Public Const F_SINSYUKU As Short = 25                   ' �L�k�o�^����            ��  �~   �~
    Public Const F_MAP As Short = 26                        ' MAP�I��                 ��  �~   �~
    '----- V1.13.0.0�B�� -----

    ' ������ V3.1.0.0�B 2014/12/02
    Public Const F_LOT As Short = 27                        ' ���b�g�ԍ��{�^��
    Public Const F_MAINTENANCE As Short = 28                ' �����e�i���X�{�^��
    Public Const F_PROBE_CLEANING As Short = 29             ' �v���[�u�N���[�j���O�{�^��
    ' ������ V3.1.0.0�B 2014/12/02

    Public Const F_INTEGRATED As Short = 30                 ' �����o�^�����{�^�� 'V4.10.0.0�B
    Public Const F_RECOG_ROUGH As Short = 31                ' ���t�A���C�����g�p�摜�o�^�{�^��  'V5.0.0.9�C
    Public Const F_FOLDEROPEN As Short = 32                ' �t�H���_�\���{�^��(���Y�Ǘ��f�[�^) V6.1.4.0�E

    Public Const MAX_FNCNO As Short = 33                    ' �@�\�I���`�e�[�u���̃f�[�^��

    '----- V6.1.4.0�B��(KOA EW�aSL432RD�Ή�) -----
    '' ���O�\����̃T�C�Y�ƈʒu(�W����)
    'Public Const TXTLOG_SIZEX_ORG As Integer = 614
    'Public Const TXTLOG_SIZEY_ORG As Integer = 642
    'Public Const TXTLOG_LOCATIONX_ORG As Integer = 653
    'Public Const TXTLOG_LOCATIONY_ORG As Integer = 49
    'Public Const GRPMODE_LOCATIONX_ORG As Integer = 653
    'Public Const GRPMODE_LOCATIONY_ORG As Integer = 691
    'Public Const TABCMD_LOCATIONX_ORG As Integer = 610
    'Public Const TABCMD_LOCATIONY_ORG As Integer = 184
    'Public Const CMDEND_LOCATIONX_ORG As Integer = 1114
    'Public Const CMDEND_LOCATIONY_ORG As Integer = 967

    ' ���O�\����̃T�C�Y�ƈʒu(KOA EW�a)
    Public Const TXTLOG_SIZEX_KOAEW As Integer = 628
    Public Const TXTLOG_SIZEY_KOAEW As Integer = 669
    Public Const TXTLOG_LOCATIONX_KOAEW As Integer = 650
    Public Const TXTLOG_LOCATIONY_KOAEW As Integer = 49
    Public Const GRPMODE_LOCATIONX_KOAEW As Integer = 650
    Public Const GRPMODE_LOCATIONY_KOAEW As Integer = 718
    Public Const TABCMD_LOCATIONX_KOAEW As Integer = 650
    Public Const TABCMD_LOCATIONY_KOAEW As Integer = 791
    Public Const CMDEND_LOCATIONX_KOAEW As Integer = 1110
    Public Const CMDEND_LOCATIONY_KOAEW As Integer = 982
    '----- V6.1.4.0�B�� -----

    '---------------------------------------------------------------------------
    '   �ő�l/�ŏ��l
    '---------------------------------------------------------------------------
    Public Const cMAXOptFlgNUM As Short = 5                 ' OcxSystem�p���߲ٵ�߼�݂̐� (�ő�5��)

    '----- �e���͍��ڂ͈̔� -----
    Public Const gMIN As Short = 0
    Public Const gMAX As Short = 1

    '----- ZZMOVE()�̈ړ��w�� -----
    Public Const MOVE_RELATIVE As Short = 0                 ' ���Βl�w�� 
    Public Const MOVE_ABSOLUTE As Short = 1                 ' ��Βl�w��

    '----- ZINPSTS()�̓��͉ӏ��w��  -----
    Public Const GET_CONSOLE_INPUT As Short = 1             ' �R���\�[��
    Public Const GET_INTERLOCK_INPUT As Short = 2           ' �C���^�[���b�N

    '----- �摜�o�^�p�p�����[�^ -----
    Public Const PTN_NUM_MAX As Short = 50                  ' �e���v���[�g�ԍ�(1-50)
    Public Const GRP_NUM_MAX As Short = 999                 ' ����ڰĸ�ٰ�ߔԍ�(1-999)

    Public Const INIT_THRESH_VAL As Double = 0.7            ' 臒l�����l
    Public Const INIT_CONTRAST_VAL As Integer = 216         ' �R���g���X�g�����l
    Public Const INIT_BRIGHTNESS_VAL As Integer = 0         ' �P�x�����l
    Public Const MIN_CONTRAST_VAL As Integer = 0            ' �R���g���X�g�ŏ��l
    Public Const MAX_CONTRAST_VAL As Integer = 511          ' �R���g���X�g�ő�l
    Public Const MIN_BRIGHTNESS_VAL As Integer = -128       ' �P�x�ŏ��l
    Public Const MAX_BRIGHTNESS_VAL As Integer = 127        ' �P�x�ő�l

    '----- ���[�_�p ----- 
    Public Const LALARM_COUNT As Integer = 128              ' �ő�A���[����
    Public Const MG_UP As Integer = 1                       ' �}�K�W���t�o      2013.01.28  '###182
    Public Const MG_DOWN As Integer = 0                     ' �}�K�W���c�n�v�m  2013.01.28  '###182

    '----- �}�[�L���O��R�ԍ� -----
    Public Const MARKING_RESNO_SET As Integer = 1000        ' ��R�ԍ�1000�Ԉȍ~�̓}�[�L���O�p�̒�R�ԍ�
    Public Const cMAXcMARKINGcSTRLEN As Short = 18          ' �}�[�L���O������ő咷(byte)
    Public Const cResultAry As Integer = 999                ' �g���~���O���ʃf�[�^�̍ő吔 V1.23.0.0�E

    '---------------------------------------------------------------------------
    '   �V�X�e���p�����[�^(�`����DllSysprm.dll�Œ�`)
    '---------------------------------------------------------------------------
    Public tkyIni As New TkyIni()                           ' �V�X�e���p�����[�^�A�N�Z�X�I�u�W�F�N�g 'V2.0.0.0�I�@
    Public gKeiTyp As Integer = tkyIni.TMENU.KEITYP.Get(Of Integer)()   'V2.0.0.0�I�@���u�^�C�v(0=R�^�C�v, 1=RW�^�C�v, 2=RS�^�C�v) 
#Region "giChipEditEx"  'V4.10.0.0�A      ��
    Private _giChipEditEx As Integer? = Nothing
    ''' <summary>SL436R��CHIP�̏ꍇ��TrimDataEditorEx���g�p���Ȃ�=0,�g�p����=0�łȂ�</summary>
    ''' <value>tky.ini[TMENU]CHIP_EDITEX�̒l��ݒ肷��</value>
    ''' <returns>�g�p���Ȃ�=0,�g�p����=0�łȂ�</returns>
    ''' <remarks>'V4.10.0.0�A</remarks>
    Public Property giChipEditEx() As Integer
        Get
            If (False = _giChipEditEx.HasValue) Then
                'If (KEY_TYPE_R <> gKeiTyp) OrElse (MACHINE_TYPE_SL436 <> gSysPrm.stTMN.gsKeimei) OrElse (KND_CHIP <> gTkyKnd) Then
                If (KEY_TYPE_R <> gKeiTyp) OrElse (KND_CHIP <> gTkyKnd) Then    'V4.10.0.0�E
                    ' R�ł͂Ȃ��A�܂���CHIP�ł͂Ȃ�
                    _giChipEditEx = 0       ' �g�p���Ȃ�
                End If
            End If
            Return _giChipEditEx
        End Get
        Set(ByVal value As Integer)
            If (KEY_TYPE_R <> gKeiTyp) OrElse (KND_CHIP <> gTkyKnd) Then        'V4.10.0.0�E
                ' R�ł͂Ȃ��A�܂���CHIP�ł͂Ȃ�
                _giChipEditEx = 0       ' �g�p���Ȃ�
            Else
                _giChipEditEx = value
            End If
        End Set
    End Property
#End Region             'V4.10.0.0�A      ��
    Public gMachineType As Integer  'V4.10.0.0�I
    Public gDllSysprmSysParam_definst As New LaserFront.Trimmer.DllSysPrm.SysParam
    Public gSysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM                  ' �V�X�e���p�����[�^
    Public OptVideoPrm As LaserFront.Trimmer.DllSysPrm.SysParam.OPT_VIDEO_PRM            ' Video.ocx�p�I�v�V������`
    Public giTrimExe_NoWork As Short = 0                    ' �蓮���[�h���A�ڕ���Ɋ�Ȃ��Ńg���~���O���s����(0)/���Ȃ�(1)�@###240
    Public giTenKey_Btn As Short = 0                        ' �ꎞ��~��ʂł́uTen Key On/Off�v�{�^���̏����l(0:ON(����l), 1:OFF)�@###268
    Public giBpAdj_HALT As Short = 0                        ' �ꎞ��~��ʂł́uBP�I�t�Z�b�g��������/���Ȃ��v(0:��������(����l), 1:�������Ȃ�)�@###269
    Public giMachineKd As Integer = 0                       ' ���u�^�C�v(0=Sl43xR, 1=SL432RW) V1.13.0.0�F
    Public giFullCutZpos As Integer = 0                     ' �����J�b�g���[�h(x5���[�h)����Z�̈ʒu�w��(0=OFF�ʒu, 1=STEP�ʒu, 2=ON�ʒu) V3.0.0.1�@
    Public gdblStg2ndOrgX As Double = 0.0                   ' �X�e�[�W��񌴓_�ʒuX V1.13.0.0�G
    Public gdblStg2ndOrgY As Double = 0.0                   ' �X�e�[�W��񌴓_�ʒuY V1.13.0.0�G
    Public gdblStgOrgMoveX As Double = 0.0                  ' ���������APP�I�����̃X�e�[�W�ʒuX V1.13.0.0�I
    Public gdblStgOrgMoveY As Double = 0.0                  ' ���������APP�I�����̃X�e�[�W�ʒuY V1.13.0.0�I
    Public giAutoPwr As Short = 0                           ' �I�[�g�p���[������(1=����/0=���Ȃ�) (FL�p) 'V1.14.0.0�A
    Public giNoDspAttRate As Short = 0                      ' ��������(0=�\������/1=�\�����Ȃ�)(FL�ȊO) 'V1.16.0.0�M
    Public giAutoModeDataSelect As Short = 0                ' �����^�]���̃f�[�^�I����ʂɃ��[�h�ς݃f�[�^����\��(0=���Ȃ�(�W��)/1=����) V1.18.0.0�G
    Public giAutoModeContinue As Short = 0                  ' �����^�]���̃}�K�W���I�����A�}�K�W���������Ď����^�]��(0=���s���Ȃ�(�W��), 1=���s����)) V1.18.0.0�H
    Public giBtnPwrCtrl As Short = 0                        ' Power Ctrl ON/OFF�{�^����1(�\������), 0(�\�����Ȃ�) V1.18.0.4�@
    Public gsEDIT_DIGITNUM As String = "0.00000"            ' ���������ҏW�p V1.16.0.0�B
    '----- V4.0.0.0-40�� -----
    ' �X�e�[�WY�̌��_�ʒu
    Public giStageYOrg As Integer = 0                       ' �X�e�[�WY�̌��_�ʒu(0=��(�W��), 1=��(SL436S��))
    Public giStageYDir As Integer = 1                       ' �X�e�[�WY�̈ړ�����(CW(1), CCW(-1))

    Public Const STGY_ORG_DW As Integer = 0                 ' ��(�W��)
    Public Const STGY_ORG_UP As Integer = 1                 ' ��(SL436S��)

    Public Const STGY_DIR_CW As Integer = 1                 ' CW(�W��)
    Public Const STGY_DIR_CCW As Integer = -1               ' CCW(SL436S��)
    '----- V4.0.0.0-40�� -----

    Public giSummary_Log As Integer = 0                     ' �T�}�����O�o��(0=�o�͂��Ȃ�, 1=�o�͂���(�V�i�W�[)) V1.22.0.0�C              
    Public giFLPrm_Ass As Short = 0                         ' ���H�����t�@�C���̎w����@(0=�g���~���O�f�[�^��, 1=1�Œ�) V2.0.0.0�D
    Public gsFLPrmFile As String = ""                       ' ���H�����t�@�C�����@V2.0.0.0�D
    Public giDspScreenKeybord As Short = 0                  ' �N���[���L�[�{�[�h�\���̗L��(0=�\������, 1=�\�����Ȃ�) 'V2.0.0.0�F(V1.22.0.0�G)
    Public giBtn_EdtLock As Short = 0                       ' EDIT LOCK/UNLOCK�{�^����(0=�\�����Ȃ�, 1=�\������) �V���v���g���}�p V2.0.0.0_25
    Public dblCalibHoseiX As Double = 0.0                   ' �L�����u���[�V�����␳���@'V1.20.0.0�F
    Public gsComPort As String = "COM6"                     ' COM�|�[�g�ԍ�(�o�[�R�[�h���[�_�p) V1.23.0.0�@
    Public giProbeCheck As Short = 0                        ' �v���[�u�`�F�b�N�@�\�̗L��/����(0=����, 1=�L��) V1.23.0.0�F
    Public giDspCmdName As Short = 0                        ' ��ʂɃR�}���h����\��(0=���Ȃ�, 1=����) V1.18.0.1�@
    Public giDoorLock As Short = 0                          ' �d�����b�N�@�\(0=�Ȃ�, 1=����)V1.18.0.1�G
    Public giPltCountMode As Short = 0                      ' ��������J�E���g���郂�[�h (�O�F�ʏ�, �P�Fx3,x5���[�h�ł��J�E���g����) V6.0.3.0�@
    Public giAfterDecPoint As Short = 0                     ' �g���~���O�ڕW�l�̏����������w��(�O�F�ʏ�(5��), �P�F7��) V6.0.3.0�@

    '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
    Public giTargetOfs As Short = 0                         ' �ڕW�l�I�t�Z�b�g�̗L��/����(0=����, 1=�L��)�@
    Public giPwrChkPltNum As Short = 0                      ' �I�[�g�p���[�`�F�b�N������w��̗L/��(0=�Ȃ�, 1=����) 
    Public giPwrChkTime As Short = 0                        ' �I�[�g�p���[�`�F�b�N����(��)�w��̗L/��(0=�Ȃ�, 1=����) 
    Public giTrimTimeOpt As Short = 0                       ' �^�N�g�\�����Ɉꎞ��~���Ԃ�(0=�܂߂�(�W��), 1=�܂߂Ȃ�)
    Public giStartBlkAss As Short = 0                       ' �g���~���O�J�n�u���b�N�ԍ��w��̗L��/����(0=����, 1=�L��)�@
    Public giSubstrateInvBtn As Short = 0                   ' �ꎞ��~��ʂł́u������v�{�^���̗L��/����(0=����, 1=�L��)�@
    '----- V4.11.0.0�@�� -----

    '----- V6.1.4.0�@��(KOA EW�aSL432RD�Ή�) �y���[�U�[�p���[���j�^�����O�@�\�z-----
    Public giLotChange As Short = 0                         ' ���b�g�ؑւ��@�\�̗L�� (�O�F�Ȃ�, �P�F����)
    Public giFileMsgNoDsp As Short = 0                      ' �t�@�C���o�[�W�����`�F�b�N���̃��b�Z�[�W�\���̗L/��(0:����(�W��), 1:�Ȃ�)
    Public gbQRCodeReaderUse As Boolean = False             ' �p�q�R�[�h���[�_�g�p/�s�g�p�@V6.1.4.0_22
    Public gbQRCodeReaderUseTKYNET As Boolean = False       ' TKY-NET�̂p�q�R�[�h���[�_�g�p/�s�g�p�@'V6.1.4.10�A
    Public giLaserrPowerMonitoring As Integer               ' ���[�U�[�p���[�̃��j�^�����O�t���O�@0�F����,1�F�����^�]�J�n��,2�F�G���g���[���b�g��,999�B���R�}���h�蓮�ł����s V6.1.4.0_35
    Public gdFullPowerLimit As Double                       ' ���[�U�[�p���[�̃��j�^�����O���~�b�g
    Public gdFullPowerQrate As Double                       ' ���[�U�[�p���[�̃��j�^�����OQ���[�g10KHz
    Public gbLaserPowerMonitoring As Boolean = False        ' ���[�U�[�p���[�̃��j�^�����O���s�L��
    '----- V6.1.4.0�@�� -----
    'V6.1.4.2�@���g���~���O�J�b�g�ʒu�Y���b��\�t�g[�����L�����u���[�V�����␳���s]
    Public giAutoCalibration As Integer = 0                 ' �����L�����u���[�V�����␳���s0:�����A>0�F���s
    Public giAutoCalibCounter As Integer = 0                ' �����L�����u���[�V�����␳���s�J�E���^�[
    Public giAutoCalibPlateCounter As Integer = 0           ' �����L�����u���[�V�����p������J�E���^�[
    Public gbAutoCalibration As Boolean = False             ' �����L�����u���[�V�����␳ ���s��:True ���s����:False
    Public gbAutoCalibrationResult As Boolean = True        ' �����L�����u���[�V�����␳���� ����I��:True �ُ�I��:False
    Public gbAutoCalibrationExecute As Boolean = False      ' �����L�����u���[�V�����␳���� ���s:True ����:False
    Public gbAutoCalibrationLog As Boolean = False          ' �J�b�g�ʒu���ꃍ�O�o��
    'V6.1.4.2�@��
    '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�)�y�p�q�R�[�h���[�h�@�\�z -----
    Public giQrCodeType As Short = 0                        ' QR_CODE�^�C�v(0=���[���a, 1=KOA EW�a)
    Public Enum QrCodeType As Integer
        Rome = 0
        KoaEw = 1
    End Enum
    '----- V6.1.4.0_22�� -----
    '----- V6.1.4.0�B�� -----
    ' gSysPrm.stLOG.giLoggingType2��` 
    Public Enum LogType2 As Integer                         ' ����۸ޏo�͒P��(0:��R��,1:��Ė� )���ѓd�H�a�������������ݖ��g�p?
        Reg = 0                                             ' ��R��(�W��)
        Cut = 1                                             ' �J�b�g�� 
        Reg_KoaEw = 2                                       ' ��R��(KOA EW����)
    End Enum
    '----- V6.1.4.0�B�� -----

    Public giCpk_Disp_Off As Boolean                        ' �b�o�j�\���I�t'V5.0.0.4�C
    Public gbControllerInterlock As Boolean                 ' �O���@��ɂ��C���^�[���b�N�̗L���i�^�cKOA�a�̉��x�R���g���[���̃C���^�[���b�N�Ɏg�p�j'V5.0.0.6�@
    Public gbLoaderSecondPosition As Boolean                ' �r�k�S�R�Q�q�Ń��[�_�̑�Q���_��L���ɂ���B'V5.0.0.6�A

    Public giTeachpointUse As Short = 0                     ' =0:KOA-EW�^�C�v(�e�B�[�`�|�C���g�ҏW�\���^�C�v)�A=1:�i�W���j�e�B�[�`�|�C���g��\���i�e�B�[�`�|�C���g�G���A�����[�N�Ŏg�p)
    Public giFixAttOnly As Short = 0                        ' �t�@�C�o�[���[�U�ŌŒ�A�b�e�l�[�^�g�p����A���Ȃ�      V6.0.1.021�@
    Public gdblNgStopRate As Double = 0                     ' ��P����NG���̐ݒ�l      'V4.5.0.5�@ 'V4.12.2.0�H�@'V6.0.5.0�C
    '----- V6.1.1.0�@��(SL432R���̃I�v�V����) -----
    Public giBuzerOn As Short = 0                           ' �g���~���O�I�����̃u�U�[���̗L��(0=�炳�Ȃ�, 1=�炷)�@
    '----- V6.1.1.0�@�� -----
    Public giNgCountAss As Short = 0                        ' NG������%����NG���Ƃ���(0=%(�W��),1=NG��) V6.1.2.0�A
    Public giAlmTimeDsp As Short = 0                        ' ���[�_�A���[�����̎��ԕ\���̗L��(0=�\���Ȃ�, 1=�\������) V6.1.1.0�L

    Public giAlarmOnOff As Short = 0                        ' �A���[����ON/OFF�{�^���\���̗L/��(1/0) SL436R���L�� V6.1.1.0�C
    Public giDispEndTime As Short = 0                       ' �����^�]�I�����̎���(�\�����Ȃ�(0)/����(1)) SL436R���L�� V6.1.1.0�B

    '----- ONLINE -----
    Public Const TYPE_OFFLINE As Short = 0                  ' OFFLINE
    Public Const TYPE_ONLINE As Short = 1                   ' ONLINE
    Public Const TYPE_MANUAL As Short = 2                   ' SLIDE COVER+XY�ړ�����

    '----- ProbeType -----
    Public Const TYPE_PROBE_NON As Short = 0                ' NON
    Public Const TYPE_PROBE_STD As Short = 1                ' STANDARD

    '----- XY Table Exist Flag -----
    Public Const TYPE_XYTABLE_NON As Short = 0              ' NON
    Public Const TYPE_XYTABLE_X As Short = 1                ' X Only
    Public Const TYPE_XYTABLE_Y As Short = 2                ' Y Only
    Public Const TYPE_XYTABLE_XY As Short = 3               ' XY

    '----- �z����ײ���� -----
    Public Const VACCUME_ERRRETRY_OFF As Short = 0          ' Not retry
    Public Const VACCUME_ERRRETRY_ON As Short = 1           ' Retry
    Public Const RET_VACCUME_RETRY As Short = 1
    Public Const RET_VACCUME_CANCEL As Short = 2

    '----- ���ϲ�� -----
    Public Const customROHM As Short = 1                    ' ۰ѓa�����d�l
    Public Const customASAHI As Short = 2                   ' �����d�q�a�����d�l
    Public Const customSUSUMU As Short = 3                  ' �i�a�����d�l
    Public Const customKOA As Short = 4                     ' KOA(���̗�)�a�����d�l
    Public Const customKOAEW As Short = 5                   ' KOA(EW)�a�����d�l
    Public Const customNORI As Short = 6                    ' �m���^�P�a�����d�l
    Public Const customTAIYO As Short = 7                   ' ���z�Гa�����d�l�@�@V1.23.0.0�@
    Public Const customWALSIN As Short = 8                  ' WALSIN�a�����d�l�@�@V4.11.0.0�A

    '----- �p���[���[�^�̃f�[�^�擾�擾 -----
    Public Const PM_DTTYPE_NONE As Short = 0                ' �Ȃ�
    Public Const PM_DTTYPE_IO As Short = 1                  ' �h�^�n�ǎ��
    Public Const PM_DTTYPE_USB As Short = 2                 ' �t�r�a

    '----- V1.22.0.0�C�� -----
    '----- �T�}�����O�o�� -----
    Public Const SUMMARY_NONE As Short = 0                  ' �o�͂��Ȃ�
    Public Const SUMMARY_OUT As Short = 1                   ' �o�͂���(�V�i�W�[)
    '----- V1.22.0.0�C�� -----

    '----- V2.0.0.0�B�� -----
    ' ���u�^�C�v(giMachineKd)
    Public Const MACHINE_KD_R As Short = 0                  ' SL43xR
    Public Const MACHINE_KD_RW As Short = 1                 ' SL432RW
    Public Const MACHINE_KD_RS As Short = 2                 ' SL43xRS
    '----- V8.0.0.16�A�� -----
    ' ���u�^�C�v(gMachineType) V4.10.0.0�I��
    Public Const MACHINE_TYPE_432R As Short = 1             ' SL432R
    Public Const MACHINE_TYPE_436R As Short = 2             ' SL436R
    Public Const MACHINE_TYPE_432RW As Short = 3            ' SL432RW
    Public Const MACHINE_TYPE_436S As Short = 4             ' SL436S
    'V4.10.0.0�I��
    'V5.0.0.6�@    Public Const TRIM_MODE_STEP_AND_REPEAT As Integer = 6   'V4.1.0.0�A
    '---------------------------------------------------------------------------
    '   �X�e�[�W����֌W
    '---------------------------------------------------------------------------
    ' �X�e�b�v����
    Public Const STEP_RPT_NON As Short = 0      ' �X�e�b�v�����s�[�g�����i�Ȃ��j
    Public Const STEP_RPT_X As Short = 1        ' �X�e�b�v�����s�[�g�����iX�����j
    Public Const STEP_RPT_Y As Short = 2        ' �X�e�b�v�����s�[�g�����iY�����j
    Public Const STEP_RPT_CHIPXSTPY As Short = 3 ' �X�e�b�v�����s�[�g�����iX�����`�b�v���X�e�b�v�{Y�����j�`�b�v���{�w����
    Public Const STEP_RPT_CHIPYSTPX As Short = 4 ' �X�e�b�v�����s�[�g�����iY�����`�b�v���X�e�b�v�{X�����j�`�b�v���{�x����

    ' BP�����
    Public Const BP_DIR_RIGHTUP As Short = 0    ' BP��E��i�v���X�����j�����@�@�@ 1 �Q �Q 0
    Public Const BP_DIR_LEFTUP As Short = 1     ' BP�����i�v���X�����j�����@�@�@�@|�Q|�Q|
    Public Const BP_DIR_RIGHTDOWN As Short = 2  ' BP��E���i�v���X�����j����        |�Q|�Q|
    Public Const BP_DIR_LEFTDOWN As Short = 3   ' BP������i�v���X�����j�����@�@�@ 3�@�@�@ 2

    Public Const BP_DIR_RIGHT As Short = 0      ' BP-X������E
    Public Const BP_DIR_LEFT As Short = 1       ' BP-X�������

    Public Const BLOCK_END As Short = 1         ' �u���b�N�I�� 
    Public Const PLATE_BLOCK_END As Short = 2   ' �v���[�g�E�u���b�N�I��

    '----- V4.0.0.0-35�� -----
    ' SL436S����Z�ʒu�̃f�t�H���g�l
    Public Const Z_ON_POS_SIMPLE As Double = 1.0
    Public Const Z_STEP_POS_SIMPLE As Double = 1.0
    Public Const Z_OFF_POS_SIMPLE As Double = 0.0
    '----- V4.0.0.0-35�� -----

    '----- V1.24.0.0�@�� -----
    ' Walsin�a����F�V�[�^�Ή�
    Public Const BPSIZE_6060 As Integer = 6060                          ' 60*60(20)
    Public Const BSZ_6060_OFSX As Double = 30.0#
    Public Const BSZ_6060_OFSY As Double = 10.0#
    '----- V1.24.0.0�@�� -----

    '----- V1.24.0.0�@�� -----
    ' KOA-EW�a�����Ή�
    Public Const BPSIZE_6030 As Integer = 6030                          ' 60*60(30)

    '----- V4.4.0.0�@�� -----
    ' ���H�G���A��90mm�Ή�
    Public Const BPSIZE_90 As Integer = 90                          ' Area 90mm
    '----- V4.4.0.0�@�� -----

    '----- ���̑� -----
    ' FLSET�֐��̃��[�h
    Public Const FLMD_CNDSET As Integer = 0                 ' ���H�����ݒ�
    Public Const FLMD_BIAS_ON As Integer = 1                ' BIAS ON
    Public Const FLMD_BIAS_OFF As Integer = 2               ' BIAS OFF(LaserOff�֐�����BIAS OFF�͂��Ă���)

    'V1.13.0.0�C 
    ' Z2����p�ɒǉ� 
    Public Const Z2OFF As Integer = 0
    Public Const Z2ON As Integer = 1
    Public Const Z2STEP As Integer = 2
    ''V1.13.0.0�C

    '---------------------------------------------------------------------------
    '   ����t���O
    '---------------------------------------------------------------------------
    Public gCmpTrimDataFlg As Short                         ' �f�[�^�X�V�t���O(0=�X�V�Ȃ�, 1=�X�V����)
    Public giTrimErr As Short                               ' ��ϰ �װ �׸� ���װ���͸���߸����OFF����ϓ��쒆OFF��۰�ް�ɑ��M���Ȃ�
    '                                                       ' B0 : �z���װ(EXIT)
    '                                                       ' B1 : ���̑��װ
    '                                                       ' B2 : �W�o�@�װь��o
    '                                                       ' B3 : ���ЯĤ���װ�����ѱ��
    '                                                       ' B4 : ����~
    '                                                       ' B5 : ������װ

    Public gLoadDTFlag As Boolean                            ' �ް�۰�ލ��׸�(False:�ް���۰��, True:�ް�۰�ލ�)
    Public gbInitialized As Boolean                         ' True=���_���A��, False=���_���A��
    'Public bFgfrmDistribution As Boolean                    ' ���Y���̕\���׸�(TRUE:�\�� FALSE:��\��)
    Public gLoggingHeader As Boolean                        ' ۸�ͯ�ް�����ݎw���׸�(TRUE:�o��)
    Public gESLog_flg As Boolean                            ' ES���O�t���O(Flase=���OOFF, True=���OON)
    '' '' ''Public giAdjKeybord As Short                             ' �g���~���O��ADJ�@�\�L�[�{�[�h���(0:���͂Ȃ� 1:�� 2:�� 3:�E 4:�� )
    Public gPrevInterlockSw As Short

    Public gbCanceled As Boolean ' ���@�e��ʏ�����Private�Ŏ��� 

    Public giSubExistMsgFlag As Boolean                      ' ��L����Ԃ��Ƃ��Ƀ��b�Z�[�W��\�����Ȃ� 'V4.11.0.0�G
    Public gDataUpdateFlag As Boolean = False               'V6.1.4.5�@ �J�b�g�I�t�l�A�d�r�|�C���g�l���͉�ʂŃf�[�^�X�V������True�ɂ���B

    '-------------------------------------------------------------------------------
    '   �I�u�W�F�N�g��`
    '-------------------------------------------------------------------------------
    '----- VB6��OCX -----
    'Public ObjSys As Object                                 ' OcxSystem.ocx
    'Public ObjUtl As Object                                 ' OcxUtility.ocx
    'Public ObjHlp As Object                                 ' OcxAbout.ocx
    'Public ObjPas As Object                                 ' OcxPassword.ocx
    'Public ObjMTC As Object                                 ' OcxManualTeach.ocx
    'Public ObjTch As Object                                 ' Teach.ocx
    'Public ObjPrb As Object                                 ' Probe.ocx
    'Public ObjVdo As Object                                 ' Video.ocx
    'Public ObjPrt As Object                                ' OcxPrint.ocx
    Public ObjMON(32) As Object
    Public gparModules As MainModules                                   ' �e�����\�b�h�ďo���I�u�W�F�N�g(OcxSystem�p) '###061
    Public ObjCrossLine As New TrimClassLibrary.TrimCrossLineClass()    ' �␳�N���X���C���\���p�I�u�W�F�N�g ###232 
    Public TrimClassCommon As New TrimClassLibrary.Common()             ' ���ʊ֐�
    Public commandtutorial As New TrimClassLibrary.CommandTutorial()    ' V2.0.0.0�I�R�}���h���{��ԊǗ��N���X 
    Public frmAutoObj As FormDataSelect2                                ' V6.1.4.0�I �����^�]�t�H�[���I�u�W�F�N�g(���b�g�ؑւ��@�\�p)
    Public ObjQRCodeReader As New QRCodeReader()                        ' V6.1.4.0_22
    Public Property FormMain As Form1                       ' ��̫�Ĳݽ�ݽ���g�p���Ȃ��悤��    V6.1.4.0�A

    '---------------------------------------------------------------------------
    ' �g���~���O���샂�[�h
    '---------------------------------------------------------------------------
    Public Const TRIM_MODE_ITTRFT As Integer = 0    '�C�j�V�����e�X�g�{�g���~���O�{�t�@�C�i���e�X�g���s
    Public Const TRIM_MODE_TRFT As Integer = 1      '�g���~���O�{�t�@�C�i���e�X�g���s
    Public Const TRIM_MODE_FT As Integer = 2        '�t�@�C�i���e�X�g���s�i����j
    Public Const TRIM_MODE_MEAS As Integer = 3      '������s
    Public Const TRIM_MODE_POSCHK As Integer = 4    '�|�W�V�����`�F�b�N
    Public Const TRIM_MODE_CUT As Integer = 5       '�J�b�g���s
    Public Const TRIM_MODE_STPRPT As Integer = 6    '�X�e�b�v�����s�[�g���s
    Public Const TRIM_MODE_TRIMCUT As Integer = 7   '�g���~���O���[�h�ł̃J�b�g���s


    '-------------------------------------------------------------------------------
    ' �g���~���O����
    '-------------------------------------------------------------------------------
    '----- �g���~���O���ʒl�iINTRIM�Őݒ�j
    '//Trim result
    '//0:�����{   1:OK       2:ITNG      3:FTNG     4:SKIP
    '//5:RATIO    6:ITHI NG  7:ITLO NG   8:FTHI NG  9:FTLO NG
    '//10:        11:        12:         13:        14:
    '//15:�ٌ`�ʕt���ɂ��SKIP
    Public Const RSLT_NO_JUDGE As Integer = 0
    Public Const RSLT_OK As Integer = 1
    Public Const RSLT_IT_NG As Integer = 2
    Public Const RSLT_FT_NG As Integer = 3
    Public Const RSLT_SKIP As Integer = 4
    Public Const RSLT_RATIO As Integer = 5
    Public Const RSLT_IT_HING As Integer = 6
    Public Const RSLT_IT_LONG As Integer = 7
    Public Const RSLT_FT_HING As Integer = 8
    Public Const RSLT_FT_LONG As Integer = 9
    Public Const RSLT_RANGEOVER As Integer = 10
    Public Const RSLT_OPENCHK_NG As Integer = 20
    Public Const RSLT_SHORTCHK_NG As Integer = 21
    Public Const RSLT_IKEI_SKIP As Integer = 15

    '----- ���Y�Ǘ��O���t�t�H�[���I�u�W�F�N�g
    Public gObjFrmDistribute As Object                      ' frmDistribute

    '----- ���Y�Ǘ����p�z�� -----
    Public Const MAX_FRAM1_ARY As Integer = 15              ' ���x���z��
    '                                                       ' ���Y�Ǘ����̃��x���z��̃C���f�b�N�X 
    Public Const FRAM1_ARY_GO As Integer = 0                ' GO��(�T�[�L�b�g�� or ��R��)
    Public Const FRAM1_ARY_NG As Integer = 1                ' NG��(�T�[�L�b�g�� or ��R��)
    Public Const FRAM1_ARY_NGPER As Integer = 2             ' NG%
    Public Const FRAM1_ARY_PLTNUM As Integer = 3            ' PLATE��
    Public Const FRAM1_ARY_REGNUM As Integer = 4            ' RESISTOR��
    Public Const FRAM1_ARY_ITHING As Integer = 5            ' IT HI NG��
    Public Const FRAM1_ARY_FTHING As Integer = 6            ' FT HI NG��
    Public Const FRAM1_ARY_ITLONG As Integer = 7            ' IT LO NG��
    Public Const FRAM1_ARY_FTLONG As Integer = 8            ' FT LO NG��
    Public Const FRAM1_ARY_OVER As Integer = 9              ' OVER��
    Public Const FRAM1_ARY_ITHINGP As Integer = 10          ' IT HI NG%
    Public Const FRAM1_ARY_FTHINGP As Integer = 11          ' FT HI NG%
    Public Const FRAM1_ARY_ITLONGP As Integer = 12          ' IT LO NG%
    Public Const FRAM1_ARY_FTLONGP As Integer = 13          ' FT LO NG%
    Public Const FRAM1_ARY_OVERP As Integer = 14            ' OVER NG%

    Public Fram1LblAry(MAX_FRAM1_ARY) As System.Windows.Forms.Label     ' ���Y�Ǘ����̃��x���z��

    '-------------------------------------------------------------------------------
    '   gMode(OcxSystem��frmReset()�̏������[�h)
    '-------------------------------------------------------------------------------
    Public Const cGMODE_ORG As Short = 0                    '  0 : ���_���A
    Public Const cGMODE_ORG_MOVE As Short = 1               '  1 : ���_�ʒu�ړ�
    Public Const cGMODE_START_RESET As Short = 2            '  2 : ����m�F���(START/RESET�҂�)
    '                                                       '  3 :
    '                                                       '  4 :
    Public Const cGMODE_EMG As Short = 5                    '  5 : ����~���b�Z�[�W�\��
    '                                                       '  6 :
    Public Const cGMODE_SCVR_OPN As Short = 7               '  7 : �g���~���O���̃X���C�h�J�o�[�J���b�Z�[�W�\��
    Public Const cGMODE_CVR_OPN As Short = 8                '  8 : �g���~���O����➑̃J�o�[�J���b�Z�[�W�\��
    Public Const cGMODE_SCVRMSG As Short = 9                '  9 : �X���C�h�J�o�[�J���b�Z�[�W�\��(�g���~���O���ȊO)
    Public Const cGMODE_CVRMSG As Short = 10                ' 10 : ➑̃J�o�[�J�m�F���b�Z�[�W�\��(�g���~���O���ȊO)
    Public Const cGMODE_ERR_HW As Short = 11                ' 11 : �n�[�h�E�F�A�G���[(�J�o�[�����Ă܂�)���b�Z�[�W�\��
    Public Const cGMODE_ERR_HW2 As Short = 12               ' 12 : �n�[�h�E�F�A�G���[���b�Z�[�W�\��
    Public Const cGMODE_CVR_LATCH As Short = 13             ' 13 : �J�o�[�J���b�`���b�Z�[�W�\��
    Public Const cGMODE_CVR_CLOSEWAIT As Short = 14         ' 14 : ➑̃J�o�[�N���[�Y�������̓C���^�[���b�N�����҂�
    Public Const cGMODE_ERR_DUST As Short = 20              ' 20 : �W�o�@�ُ팟�o���b�Z�[�W�\��
    Public Const cGMODE_ERR_AIR As Short = 21               ' 21 : �G�A�[���G���[���o���b�Z�[�W�\��

    Public Const cGMODE_ERR_HING As Short = 40              ' 40 : �A��HI-NG�װ(ADV�������҂�)
    Public Const cGMODE_SWAP As Short = 41                  ' 41 : �����(START�������҂�)
    Public Const cGMODE_XYMOVE As Short = 42                ' 42 : �I������ð��وړ��m�F(START�������҂�)
    Public Const cGMODE_ERR_REPROBE As Short = 43           ' 43 : �ăv���[�r���O���s(START�������҂�) SL436R�p
    Public Const cGMODE_LDR_ALARM As Short = 44             ' 44 : ���[�_�A���[������   SL436R�p
    Public Const cGMODE_LDR_START As Short = 45             ' 45 : �����^�]�J�n(START�������҂�)   SL436R�p
    Public Const cGMODE_LDR_TMOUT As Short = 46             ' 46 : ���[�_�ʐM�^�C���A�E�g  SL436R�p
    Public Const cGMODE_LDR_END As Short = 47               ' 47 : �����^�]�I��(START�������҂�)   SL436R�p
    Public Const cGMODE_LDR_ORG As Short = 48               ' 48 : ���[�_���_���A  SL436R�p
    Public Const cGMODE_ERR_CUTOFF_TURNING As Short = 49    ' 49 : �����J�b�g�I�t�������s��

    Public Const cGMODE_AUTO_LASER As Short = 50            ' 50 : �������[�U�p���[����
    Public Const cGMODE_QUEST_NEW_CONTINUE As Short = 51    ' 51 : �����^�]�J�n���̖₢���킹  ' V6.0.3.0_27

    Public Const cGMODE_LDR_CHK As Short = 60               ' 60 : ���[�_��ԃ`�F�b�N(�N����۰�ގ���Ӱ��/���쒆)
    Public Const cGMODE_LDR_ERR As Short = 61               ' 61 : ���[�_��ԃG���[(۰�ގ�����۰�ޖ�)
    Public Const cGMODE_LDR_MNL As Short = 62               ' 62 : �J�o�[�J��̃��[�_�蓮���[�h����
    Public Const cGMODE_LDR_WKREMOVE As Short = 63          ' 63 : �c���菜�����b�Z�[�W  SL436R�p
    Public Const cGMODE_LDR_RSTAUTO As Short = 64           ' 64 : �����^�]���~���b�Z�[�W  SL436R�p ###124
    Public Const cGMODE_LDR_WKREMOVE2 As Short = 65         ' 65 : �c���菜�����b�Z�[�W(APP�I��)  SL436R�p ###175
    Public Const cGMODE_LDR_STAGE_ORG As Short = 66         ' 66 : �X�e�[�W���_�ړ� SL436R�p ###188
    Public Const cGMODE_LDR_MAGAGINE_EXCHG As Short = 67    ' 67 : �}�K�W���������b�Z�[�W SL436R�p V1.18.0.0�H
    Public Const cGMODE_LDR_CHK_AUTO As Short = 67          ' 67 : ���[�_��ԃ`�F�b�N(�����^�]��),���[�_�������ɐ؂�ւ��܂ő҂� SL432R�p V6.1.4.0�@
    '                                                              ��DllSystem�Ɣԍ����K�b�`���R���Ă��適�{���Ȃ�DllSystem��gMode�ɍ��킹��x�L                 
    Public Const cGMODE_ERR_RATE_NG As Short = 68           ' 68 : High,Low�̗��������Ȃ����ꍇ�̒�~��ʗp   'V4.9.0.0�@
    Public Const cGMODE_ERR_TOTAL_CLEAR As Short = 69       ' 69 : �W�v�̃g�[�^�����N���A���邩�̊m�F         'V4.9.0.0�@

    Public Const cGMODE_OPT_START As Short = 70             ' 70 : ���ݸފJ�n���̽���SW�����҂�
    Public Const cGMODE_OPT_END As Short = 71               ' 71 : ���ݸޏI�����̽ײ�޶�ް�J�҂�

    Public Const cGMODE_MSG_DSP As Short = 90               ' 90 : �w�胁�b�Z�[�W�\��(START�L�[�����҂�)

    Public Const cGMODE_STAGE_HOMEMOVE As Short = 91        ' 91 :�X�e�[�WHome�ʒu�ړ�      'V4.0.0.0-83
    Public Const cGMODE_MSG_DSP3 As Short = 92              ' 92 : �w�胁�b�Z�[�W�\��(START,RESET�������҂�)�@'V6.1.1.0�@

    ' ���~�b�g�Z���T�[& ���G���[ & �^�C���A�E�g���b�Z�[�W
    ' ��cGMODE_TO_AXISX As Short = 101�ȍ~�� TrimErrNo.vb�Ɉړ�

    '---------------------------------------------------------------------------
    '   �␳�N���X���C���\���p�p�����[�^
    '---------------------------------------------------------------------------
    'Public gstCLC As CLC_PARAM                              ' �␳�N���X���C���\���p�p�����[�^ V4.0.0.0-21

    '---------------------------------------------------------------------------
    '   �t�@�C���p�X�֌W
    '---------------------------------------------------------------------------
    Public gStrTrimFileName As String                       ' ���ݸ��ް�̧�ٖ�

    ''''    lib.bas�@�ł����g�p����Ă��Ȃ��B
    Public gsDataLogPath As String

    Public gbCutPosTeach As Boolean                         ' CutPosTeach(�\����:True, ��\��:False)

    '---------------------------------------------------------------------------
    '   �ϐ���`
    '---------------------------------------------------------------------------

    '----- �p�^�[���F���p -----
    Public giTempGrpNo As Integer                           ' �e���v���[�g�O���[�v�ԍ�(1�`999)
    Public giTempNo As Integer                              ' �e���v���[�g�ԍ�

    '----- �J�b�g�ʒu�␳�p�\���� -----
    Public Structure CutPosCorrect_Info                     ' �p�^�[���o�^���
        Dim intFLG As Short                                 ' �J�b�g�ʒu�␳�t���O(0:���Ȃ�, 1:����)
        Dim intGRP As Short                                 ' �p�^�[����ٰ�ߔԍ�(1-999)
        Dim intPTN As Short                                 ' �p�^�[���ԍ�(1-50)
        Dim dblPosX As Double                               ' �p�^�[���ʒuX(�␳�ʒu�e�B�[�`���O�p)
        Dim dblPosY As Double                               ' �p�^�[���ʒuY(�␳�ʒu�e�B�[�`���O�p)
        Dim intDisp As Short                                ' �p�^�[���F�����̌����g�\��(0:�Ȃ�, 1:����)
    End Structure

    Public Const MaxRegNum As Short = 256                   ' ��R���̍ő�l
    Public Const MaxCutNum As Short = 30                    ' �J�b�g�̍ő�l
    Public Const MaxDataNum As Short = 7681                 ' ��R��*�J�b�g�̍ő吔+1
    Public stCutPos(MaxRegNum + 1) As CutPosCorrect_Info        ' �p�^�[���o�^���

    Public giCutPosRNum As Short                            ' �J�b�g�ʒu�␳�����R��
    'Public giCutPosRSLT(MaxRegNum) As Short                 ' �p�^�[���F������(0:�␳�Ȃ�, 1:OK, 2:NG�����)
    'Public gfCutPosDRX(MaxRegNum) As Double                 ' �Y����X
    'Public gfCutPosDRY(MaxRegNum) As Double                 ' �Y����Y
    Public gfCutPosCoef(MaxRegNum) As Double                '  ��v�x

    '----- �ƕ␳�p -----
    Public gfCorrectPosX As Double                          ' �ƕ␳����XYð��ق����X(mm) ��ThetaCorrection()�Őݒ�
    Public gfCorrectPosY As Double                          ' �ƕ␳����XYð��ق����Y(mm)
    Public gdCorrectTheta As Double                         ' �ƕ␳���̕␳�p�x                     'V5.0.0.9�H
    Public gbInPattern As Boolean                           ' �ʒu�␳������
    Public gbRotCorrectCancel As Short                      ' 0:OK, n < 0: �ʒu�␳���L�����Z������ or �ʒu�␳�G���[
    Public gbCorrectDone As Boolean = False                 'V4.10.0.0�B �I�y���[�^�e�B�[�`���O�ȑf�� ��x�w�x���␳���s�������͎��{���Ȃ��ׂɎg�p
    Public gbIntegratedMode As Boolean = False              'V4.10.0.0�B ���݈ꊇ�e�B�[�`���O���[�h�̎� True
    Public gbSubstExistChkDone As Boolean = False           'V4.10.0.0�B �I�y���[�^�e�B�[�`���O�ȑf�� ��x��݉׃`�F�b�N���s�������͎��{���Ȃ��ׂɎg�p
    '----- V1.13.0.0�B�� -----
    '----- �I�[�g�v���[�u�p -----
    Public gfStgOfsX As Double = 0.0                        ' XY�e�[�u���I�t�Z�b�gX(mm) ���I�[�g�v���[�u���s�R�}���h(FrmMatrix())�Őݒ�
    Public gfStgOfsY As Double = 0.0                        ' XY�e�[�u���I�t�Z�b�gY(mm)
    '----- V1.13.0.0�B�� -----

    '----- �f�W�^���r�v -----
    'Public gDigH As Short                                   ' �f�W�^���r�v(Hight)
    'Public gDigL As Short                                   ' �f�W�^���r�v(Low)
    'Public gDigSW As Short                                  ' �f�W�^���r�v
    Public gPrevTrimMode As Short                           ' �f�W�^���r�v�l�ޔ���

    '----- GPIB�p -----
    Public giGpibDefAdder As Short = 21                     ' �����ݒ�(�@����ڽ)

    '----- ���̑� -----
    Public giIX2LOG As Short = 0                            ' IX2���O(0=����, 1=�L��)�@###231
    Public giPrint As Short = 0                             ' PRINT�{�^��(0=����, 1=�L��) V1.18.0.0�B
    Public giTablePosUpd As Short = 0                       ' �e�[�u��1,2���W���X�V����/���Ȃ�(VIDEO.OCX�p�I�v�V����)�@###234
    Public giPassWord_Lock As Integer = 0                   ' V1.14.0.0�@

    ' ������ V3.1.0.0�A 2014/12/01
    Public giMeasurement As Short = 0                       ' ������@�i0=���܂Œʂ�A1=�C�j�V�����e�X�g�A2=�t�@�C�i���e�X�g�j
    Public gdRESISTOR_MIN As Double                         ' ��R�ŏ��l
    Public gdRESISTOR_MAX As Double                         ' ��R�ő�l
    ' ������ V3.1.0.0�A 2014/12/01

    'V4.6.0.0�@
    Public giManualSeq As Integer                           ' �蓮���s���̃N�����v�z���V�[�P���X���R�}���h���s�ɍ��킹��B
    'V4.6.0.0�@
    'V4.6.0.0�A
    Public giTimeLotOnly As Integer                         ' Lot���̂ݐ��Y���̎��ԃJ�E���g���s��
    'V4.6.0.0�A

    Public giRateDisp As Integer                                            'V4.8.0.1�@ NG���̕\���������܂�\�����̐ؑւ�
    Public LOT_COND_FILENAME As String = "C:\TRIM\LotStopCondition.ini"     'V4.8.0.1�@
    Public giChangePoint As Integer                         'V4.9.0.0�A �ؑփ|�C���g�A�^�[���|�C���g�ؑ֋@�\�L���A�����ݒ�
    Public giNgStop As Integer                               ''V4.9.0.0�@
    Public giClampSeq As Integer                                            '
    Public giClampLessStage As Integer                      ' �N�����v���X�ڕ���(0�łȂ��ꍇ�N�����v���X)       'V5.0.0.9�B
    Public gdClampLessOffsetX As Double                     ' �N�����v���X�������������ڈʒu��I�t�Z�b�gX     'V5.0.0.9�E
    Public gdClampLessOffsetY As Double                     ' �N�����v���X�������������ڈʒu��I�t�Z�b�gY     'V5.0.0.9�E
    Public giClampLessRoughCount As Integer                 ' �N�����v���X���t�A���C�����g��                 'V5.0.0.9�E
    Public gdClampLessTheta As Double                       ' �N�����v���X�ڕ���̎�������������ڃƊp�x    'V5.0.0.9�H
    Public giClampLessOutPos As Integer                     ' �N�����v���X���̔r�o�ʒu�w��    'V6.1.2.0�C


    ''''    ��������False�ɐݒ肵�Ă��邪�ATrue�ɐݒ肳��邱�Ƃ͂Ȃ��B
    ''''    �t���O�Ƃ��ċ@�\�͂��Ă��Ȃ��̂ŁA�R�[�h�m�F�̏�폜�B
    'Public OKFlag As Boolean                    'OK�{�^�������̗L��

    ''''    �������̂�
    'Public gRegisterExceptMarkingCnt As Short '��R���i�}�[�L���O��������) @@@007
    'Public gsSystemPassword As String
    'Public gLoggingEnd As Boolean

    ' '' '' ''----- ���Y�Ǘ���� -----
    '' '' ''Public glCircuitNgTotal As Integer                      ' �s�ǃT�[�L�b�g��
    '' '' ''Public glCircuitGoodTotal As Integer                    ' �Ǖi�T�[�L�b�g��
    '' '' ''Public glPlateCount As Integer                          ' �v���[�g������
    '' '' ''Public glGoodCount As Integer                           ' �Ǖi��R��
    '' '' ''Public glNgCount As Integer                             ' �s�ǒ�R��
    '' '' ''Public glITHINGCount As Integer                         ' IT HI NG��
    '' '' ''Public glITLONGCount As Integer                         ' IT LO NG��
    '' '' ''Public glFTHINGCount As Integer                         ' FT HI NG��
    '' '' ''Public glFTLONGCount As Integer                         ' FT LO NG��
    '' '' ''Public glITOVERCount As Integer                         ' IT���ް�ݼސ�


    Public gfPreviousPrbBpX As Double                       ' BP�_�����W��̈ʒuX (BSIZE+BPOFFSET����)
    Public gfPreviousPrbBpY As Double                       '                   Y

    ''''------------------------------------------------

    ''''---------------------------------------------------
    ''''�@090413 minato
    ''''    ProbeTeach�Őݒ肵�AResistorGraph�Ŏg�p���Ă���̂݁B
    ''''    �����ŏo����悤�Ɍ������B
    '---------------------------------------------------------------------------
    '   �S��R����̃O���t�\���p
    '---------------------------------------------------------------------------
    Public giMeasureResistors As Short                      ' ��R��
    Public giMeasureResiNum(512) As Double                  ' ��R�ԍ�
    Public gfMeasureResiOhm(512) As Double                  ' ���肵����R�l
    Public gfResistorTarget(512) As Double                  ' �ڕW�l
    Public gfMeasureResiPos(2, 512) As Double               ' �J�b�g�X�^�[�g�|�C���g
    Public giMeasureResiRst(512) As Short                   ' �g���~���O����

    Public Const cMEASUREcOK As Short = 1                   ' OK
    Public Const cMEASUREcIT As Short = 2                   ' IT ERROR
    Public Const cMEASUREcFT As Short = 3                   ' FT ERROR
    Public Const cMEASUREcNA As Short = 4                   ' ������


    '===============================================================================
    Public ExitFlag As Short
    Public gMode As Short '���[�h

    'INI�t�@�C���擾�f�[�^
    ''''(2010/11/16) ����m�F�㉺�L�R�����g�͍폜
    'Public gStartX As Double '�v���[�u�����lX
    'Public gStartY As Double '�v���[�u�����lY

    ' ���[�U�[����
    ''''    frmReset�ALASER_teaching�@�Ŏg�p
    Public gfLaserContXpos As Double
    Public gfLaserContYpos As Double

    '�摜�n���h��
    'Public mlHSKDib As Integer '����
    '�\���ʒu
    'Public mtDest As RECT
    'Public mtSrc As RECT
    'Public gVideoStarted As Boolean

    ''----- ����Ӱ�� ----- (��)OcxSystem��`�ƈ�v������K�v�L��
    'Public giAppMode As Short

    ''�f�[�^�ҏW�p�X���[�h�֘A
    'Public gbPassSucceeded As Boolean

    'Public gLoggingHeader As Boolean                    ' ͯ�ް�����ݎw���׸�(TRUE:�o��)
    'Public gbLogHeaderWrite As Boolean ' ���O�̃w�b�_�o�̓t���O @@@082

    'Public giOpLogFileHandle As Short ' ���샍�O�t�@�C���̃n���h��
    'Public gwTrimmerStatus As Short ' �z�X�g�ʐM�X�e�[�^�X�ێ�

    '''' ���M���O�t���O�@09/09/09  SysParam����ڍs


    Public Const KUGIRI_CHAR As Short = &H9S ' TAB

    'Public gbInPattern As Boolean ' �ʒu�␳������
    'Public gbRotCorrectCancel As Short ' 0:OK, n < 0: �ʒu�␳���L�����Z������ or �ʒu�␳�G���[
    ''Public gfCorrectPosX As Double                          ' �g�����|�W�V�����␳�lX 
    'Public gfCorrectPosY As Double                          ' �g�����|�W�V�����␳�lY
    'Public gbPreviousPrbPos As Boolean ' �v���[�u�ʒu���킹��BP/STAGE���W���L�����Ă���
    'Public gsCutTypeName(256) As String ' �J�b�g�^�C�v���e�[�u��
    'Public gtimerCoverTimeUp As Boolean

    ''BP���j�A���e�B�[�␳�l
    'Public Const cMAXcBPcLINEARITYcNUM As Short = 21


    ''''2009/05/29 minato
    ''''    LoaderAlarm.bas�폜�ɂ��ꎞ�ړ�
    ''''===============================================
    '' ''Public iLoaderAlarmKind As Short ' �װю��(1:�S��~�ُ� 2:���ْ�~ 3:�y�̏� 0:�װі���)
    '' ''Public iLoaderAlarmNum As Short ' �������̱װѐ�
    '' ''Public strLoaderAlarm() As String ' �װѕ�����
    '' ''Public strLoaderAlarmInfo() As String ' �װя��1
    '' ''Public strLoaderAlarmExec() As String ' �װя��2(�΍�)
    ''''===============================================

    'Public gbInitialized As Boolean

    '----- ���z�}�p -----
    Public Const MAX_SCALE_NUM As Integer = 999999999           ' ���̍ő�l
    Public Const MAX_SCALE_RNUM As Integer = 12                 ' ���̕\����R��

    Public gDistRegNumLblAry(12) As System.Windows.Forms.Label     ' ���z�O���t��R���z��
    Public gDistGrpPerLblAry(12) As System.Windows.Forms.Label     ' ���z�O���t%�z��
    Public gDistShpGrpLblAry(12) As System.Windows.Forms.Label     ' ���z�O���t�z��

    'V4.0.0.0�K��
    Public gGoodChip As System.Windows.Forms.Label                  ' ���z�O���tGoodChip��
    Public gNgChip As System.Windows.Forms.Label                    ' ���z�O���tNGChip��
    Public gMinValue As System.Windows.Forms.Label                  ' ���z�O���t�ŏ��l
    Public gMaxValue As System.Windows.Forms.Label                  ' ���z�O���t�ő�l
    Public gAverageValue As System.Windows.Forms.Label              ' ���z�O���t���ϒl
    Public gDeviationValue As System.Windows.Forms.Label            ' ���z�O���t�W���΍�
    Public gGraphAccumulationTitle As System.Windows.Forms.Label    ' ���z�O���t�^�C�g��
    Public gRegistUnit As System.Windows.Forms.Label                ' ���z�O���t��R�P��

    'Public gSimpleDistRegNumLblAry(12) As System.Windows.Forms.Label     ' ���z�O���t��R���z��
    'Public gSimpleDistGrpPerLblAry(12) As System.Windows.Forms.Label     ' ���z�O���t%�z��
    'Public gSimpleDistShpGrpLblAry(12) As System.Windows.Forms.Label     ' ���z�O���t�z��
    'V4.0.0.0�K��

    Public glRegistNum(12) As Integer                            ' ���z�O���t��R��
    Public glRegistNumIT(12) As Integer                          ' ���z�O���t��R�� �Ƽ��ý�
    Public glRegistNumFT(12) As Integer                          ' ���z�O���t��R�� ̧���ý�

    Public lOkChip As Integer                                   ' OK��
    Public lNgChip As Integer                                   ' NG��
    Public dblMinIT As Double                                   ' �ŏ��l�Ƽ��
    Public dblMaxIT As Double                                   ' �ő�l�Ƽ��
    Public dblMinFT As Double                                   ' �ŏ��ļ���
    Public dblMaxFT As Double                                   ' �ő�ļ���
    '' '' ''Public dblGapIT As Double                                   ' �ώZ�덷�Ƽ��
    '' '' ''Public dblGapFT As Double                                   ' �ώZ�덷̧���

    Public dblAverage As Double                                 ' ���ϒl
    Public dblDeviationIT As Double                             ' �W���΍�(IT)
    Public dblDeviationFT As Double                             ' �W���΍�(FT)

    Public dblAverageIT As Double                               ' IT���ϒl
    Public dblAverageFT As Double                               ' FT���ϒl
    Public HEIHOUIT As Double                                   ' �����΍�
    Public HEIHOUFT As Double                                   ' �����΍�

    '    Public Const SIMPLE_PICTURE_SIZEX As Long = 6400 '6400            ' Video��ʃT�C�YX(�V���v���g���}��)
    '    Public Const SIMPLE_PICTURE_SIZEY As Long = 6400 '6400            ' Video��ʃT�C�YY(�V���v���g���}��)
    Public Const SIMPLE_PICTURE_SIZEX As Long = 426 '6400            ' Video��ʃT�C�YX(�V���v���g���}��)
    Public Const SIMPLE_PICTURE_SIZEY As Long = 426 '6400            ' Video��ʃT�C�YY(�V���v���g���}��)

    'Public Const NORMAL_PICTURE_SIZEX As Long = 9600            ' Video��ʃT�C�YX(�e�B�[�`���O�A�m�[�}����)
    'Public Const NORMAL_PICTURE_SIZEY As Long = 7200            ' Video��ʃT�C�YY(�e�B�[�`���O�A�m�[�}����)
    Public Const NORMAL_PICTURE_SIZEX As Long = 640            ' Video��ʃT�C�YX(�e�B�[�`���O�A�m�[�}����)
    Public Const NORMAL_PICTURE_SIZEY As Long = 480            ' Video��ʃT�C�YY(�e�B�[�`���O�A�m�[�}����)

    'V4.0.0.0�I
    Public Const NORMAL_SIZE As Long = 0                        ' �ʏ�̑傫���T�C�Y�̉摜�\��
    Public Const SIMPLE_SIZE As Long = 1                        ' �������T�C�Y�̉摜�\��

    Public CROSS_LINEX As Long                                  ' �V���v���g���}��ʗp�N���X���C���ʒuX
    Public CROSS_LINEY As Long                                  ' �V���v���g���}��ʗp�N���X���C���ʒuY

    Public iGlobalJogMode As Integer                            ' Jog���̓��샂�[�h�I��
    Public giJogButtonEnable As Integer                         ' Jog�{�^���̗L������'V4.0.0.0-78
    Public CleaningPosZ As Double                               ' �N���[�j���O�ʒuZ
    Public gProbeCleaningCounter As Long = 0                    ' �v���[�u�N���[�j���O�p�J�E���^
    Public gProbeCleaningSpan As Long                           ' �v���[�u�N���[�j���O�Ԋu
    Public Const giClampModeIO As Integer = 0                   ' �N�����v�����IO�����������؂�ւ���    'V4.8.0.0�@
    Public gfrmAdjustDisp As Integer = 0                        ' FineAdjust��ʕ\����
    Public giNGCountInPlate As Integer = 0                      ' 1�����NG��     'V6.0.1.0�N

    '----- V6.0.3.0�F�� -----
    Public gAdjustCutoffCount As Long                           ' �J�b�g�I�t�����u���b�N�p�J�E���^�[ 
    Public gAdjustCutoffFunction As Long                        ' �J�b�g�I�t�����@�\�L������
    Public giCutOffLogOut As Long                               ' �J�b�g�I�t�������O�o�͗L��

    '----- �J�b�g�I�t�����@�\�p�\���� -----
    Public Structure CutOffAdjust_Info                          ' �J�b�g�I�t�����@�\�p�\����
        Dim dblAdjustCutOff_Exec As Integer                     ' �J�b�g�I�t�������s����A���Ȃ�
        Dim TargetA As Double                                   ' �J�b�g�I�t�����p�ڕW�l
        Dim OrgCutOff As Double                                 ' �J�n���̃J�b�g�I�t�l
        Dim dblAdjustCutOff As Double                           ' �������J�b�g�I�t�l
    End Structure
    Public stCutOffAdjust As CutOffAdjust_Info                  ' �J�b�g�I�t�����@�\�p�\����

    Public Enum AdjustStat
        ADJUST_DISABLE = 0
        ADJUST_EXEC
        ADJUST_ALREADY
        ADJUST_FINISHED
    End Enum
    Public gVacuumIO As Integer                                 ' V6.0.3.0_37
    '----- V6.0.3.0�F�� -----

    Public giReqLotSelect As Integer                            ' �}�K�W������ŃX�^�[�g�����Ƃ��Ƀ��b�g�I�����p�����̑I�����郁�b�Z�[�W��\�����邩�ǂ���  V6.0.3.0_38

    ''' <summary>
    ''' �X�e�[�W�ړ����̑��x�ݒ� 
    ''' </summary>
    Public Enum StageSpeed As Integer
        NormalSpeed = 0             '  0:�ʏ푬�x
        StepRepeatSpeed             '  1:StepAndRepeat�����x 
    End Enum

#End Region

#Region "�O���[�o���ϐ��̒�`"
    '===========================================================================
    '   �O���[�o���ϐ��̒�`
    '===========================================================================
    Public gbThetaCorrectionLogOut As Boolean = False               'V4.7.3.2�@

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''' 2009/04/13 minato
    ''''    TKY�ł͎g�p���Ă���O���[�o���ϐ�
    ''''    ���ʉ��ׁ̈ATKY�p�Ƃ��Ă͐錾���������

    '----- �A���^�]�p(SL436R�p) -----
    Public gbFgAutoOperation As Boolean = False                     ' �����^�]�t���O(True:�����^�]��, False:�����^�]���łȂ�) 
    Public gsAutoDataFileFullPath() As String                       ' �A���^�]�o�^�f�[�^�t�@�C�����z��
    Public giAutoDataFileNum As Short                               ' �A���^�]�o�^�f�[�^�t�@�C����
    Public giActMode As Short                                       ' �A���^�]���샂�[�h(0:϶޼��Ӱ�� 1:ۯ�Ӱ�� 2:����ڽӰ��)
    Public Const MODE_MAGAZINE As Short = 0                         ' �}�K�W�����[�h
    Public Const MODE_LOT As Short = 1                              ' ���b�g���[�h
    Public Const MODE_ENDLESS As Short = 2                          ' �G���h���X���[�h
    '                                                               ' �ؑւ����[�h(1=�������[�h, 0=�蓮���[�h)
    Public Const MODE_MANUAL As Integer = 0                         ' �蓮���[�h
    Public Const MODE_AUTO As Integer = 1                           ' �������[�h
    Public giErrLoader As Short = 0                                 ' ���[�_�A���[�����o(0:�����o 0�ȊO:�G���[�R�[�h) ###073
    Public gbFgContinue As Boolean = False                          ' �����^�]�p���t���O(True:�p����, False:�p���łȂ�) 'V1.18.0.0�B(���[���a����)

    '                                                               ' �ȉ��̓V�X�p�����ݒ肷��
    Public giOPLDTimeOutFlg As Integer                              ' ���[�_�ʐM�^�C���A�E�g���o(0=���o����, 1=���o����)
    Public giOPLDTimeOut As Integer                                 ' ���[�_�ʐM�^�C���A�E�g����(msec)
    Public giOPVacFlg As Integer                                    ' �蓮���[�h���̍ڕ���z���A���[�����o(0=���o����, 1=���o����)
    Public giOPVacTimeOut As Integer                                ' �蓮���[�h���̍ڕ���z���A���[���^�C���A�E�g����(msec)

    Public Const MAXWORK_KND As Integer = 10                        ' �v���[�g�f�[�^�̊�i��̐�
    Public giLoaderSpeed As Integer                                 ' ���[�_�������x
    Public giLoaderPositionSetting As Integer                       ' ���[�_�ʒu�ݒ�I��ԍ�(1-10)
    Public gfBordTableOutPosX(0 To MAXWORK_KND - 1) As Double       ' ���[�_��e�[�u���r�o�ʒuX
    Public gfBordTableOutPosY(0 To MAXWORK_KND - 1) As Double       ' ���[�_��e�[�u���r�o�ʒuY
    Public gfBordTableInPosX(0 To MAXWORK_KND - 1) As Double        ' ���[�_��e�[�u�������ʒuX
    Public gfBordTableInPosY(0 To MAXWORK_KND - 1) As Double        ' ���[�_��e�[�u�������ʒuY
    '----- V4.0.0.0-26�� -----
    Public gfTwoSubPickChkPos(0 To MAXWORK_KND - 1) As Double       ' �񖇎��Z���T�m�F�ʒu���W(mm)
    Public glSubstrateType(0 To MAXWORK_KND - 1) As Long            ' ����Ή�(0=�ʏ�, 1=�����(�X���[�z�[��))
    '----- V4.0.0.0-26�� -----

    Public giNgBoxCount(0 To MAXWORK_KND - 1) As Integer            ' NG�r�oBOX�̎��[����(��i�핪)   ###089
    Public giNgBoxCounter As Integer = 0                            ' NG�r�oBOX�̎��[�����J�E���^�[     ###089

    Public giBreakCounter As Integer = 0                            ' ���ꌇ�������̎��[�����J�E���^�[     ###130 
    Public giTwoTakeCounter As Integer = 0                          ' �Q����蔭���̎��[�����J�E���^�[     ###130 

    Public m_lTrimResult As Integer = cFRS_NORMAL                   ' ��P�ʂ̃g���~���O����(SL436R�����^�]����NG�r�oBOX�̎��[�����J�E���g�p) ###089
    '                                                               ' cFRS_NORMAL (����)
    '                                                               ' cFRS_TRIM_NG(�g���~���ONG)
    '                                                               ' cFRS_ERR_PTN(�p�^�[���F���G���[) ���Ȃ�
    Public bFgAutoMode As Boolean = False                           ' ���[�_�������[�h�t���O
    Public gbCycleStop As Boolean = False                           ' �T�C�N����~�����L��V5.0.0.4�@
    Public bFgCyclStp As Boolean = False                            ' �T�C�N����~�t���O V4.0.0.0�R
    Public iExcamCutBlockNo_X As Integer                            ' �O���J�����J�b�g�ʒu�摜�o�^�̃u���b�NNoX����璹�Ή��ׂ̈ɂP�ɌŒ肷��B'V1.25.0.0�K
    '----- �A���^�]�p(SL436R�p) -----
    Public m_lTrimNgCount As Integer = 0                            ' �A���g���~���O�m�f�����J�E���^�[(�����^�]�p KOA EW�aSL432RD�Ή�) V6.1.4.0�H

    Public iInverseStepY As Integer                                 ' Y�����̃X�e�b�v���t�]���� 'V4.12.0.0�@�@'V6.1.2.0�A

    Public giGazouClrTime As Integer                                ' �摜�\���N���A�p�̃^�C�}    'V4.12.2.2�D 'V6.0.5.0�G

    ' V6.0.3.0_47
    Public giMoveModeWheelDisable As Integer = 0                    ' ���C����ʂ�MoveMode��MouseWheel��L���Ƃ��邩�̐ݒ�
    'Public strPlateDataFileFullPath() As String             ' �A���^�]�o�^ؽ����߽������z��
    'Public intPlateDataFileNum As Short                     ' �A���^�]�o�^ؽ����߽������
    'Public intActMode As Short                              ' �A���^�]����Ӱ��(0:϶޼��Ӱ�� 1:ۯ�Ӱ�� 2:����ڽӰ��)

    'Public INTRTM_Ver As String 'INtime Version
    'Public LMP_No As String 'LMP No

    Public giJogWaitMode As Integer                                 ' Jog���쎞�̃X�e�[�W�߂�҂��L�� 0:����A1:�Ȃ�   'V6.1.2.0�@


    '' '' ''Public gfX_2IT As Double ' IT�W���΍��Z�o�p���[�N
    '' '' ''Public gfX_2FT As Double ' FT�W���΍��Z�o�p���[�N

    Public glITTOTAL As Long                                        ' IT�v�Z�Ώې� ###138
    Public glFTTOTAL As Long                                        ' FT�v�Z�Ώې� ###138

    'Public gbEditPassword As Short ' �f�[�^���͎��̃p�X���[�h�v��(0:�� 1:�L)
    Public gITNx() As Double                                        'IT ����덷(�X)
    Public gFTNx() As Double                                        'FT ����덷(�X)

    Public gITNx_cnt As Integer                                     'IT �Z�o�pܰ���
    Public gITNg_cnt As Integer                                     'IT NG���L�^
    Public gFTNx_cnt As Integer                                     'FT �Z�o�pܰ���
    Public gFTNg_cnt As Integer                                     'FT NG���L�^
    'Public giXmode As Short
    Public gLogMode As Integer                                      '۷�ݸ�Ӱ��(0:���Ȃ�, 1:INITIAL TEST, 2:FINAL TEST, 3:INITIAL + FINAL) ###150 

    Public StepTab_Mode As Short                                    '(0)Step (1)Group
    Public StepFGMove As Short                                      '(0)�Ȃ��@(1)�ï�߸�د�ފԈړ�����[->]  (2)�ï�߸�د�ފԈړ�����[<-]
    Public StepTitle(2) As Short                                    '(0)���͂���@(1)���͂Ȃ�

    '--ROHM--
    Public giLoginPass As Boolean                                   '�N�����߽ܰ�ޓ���(False)NG (True)OK

    Public sIX2LogFilePath As String                                ' IX2 LOĢ���߽��
    Public gsESLogFilePath As String                                ' ES LOĢ���߽��

    ' frmFineAdjust.vb�ł̂ݎg�p����ϐ�
    '   �t�H�[���I����ɒl�̎擾���K�v�Ȃ��߁A
    '   �O���[�o���ŕϐ���ݒ肷��B
    Public gCurPlateNo As Integer
    Public gCurBlockNo As Integer
    '#4.12.2.0�E    Public gFrmEndStatus As Integer
    Friend gCurPlateNoX As Integer
    Friend gCurPlateNoY As Integer
    Friend gCurBlockNoX As Integer
    Friend gCurBlockNoY As Integer
    '#4.12.2.0�E                         ��

    '----- ���O��ʕ\���p -----�@                                   '###013
    Public gDspClsCount As Integer                                  ' ���O��ʕ\���N���A�����
    Public gDspCounter As Integer                                   ' ���O��ʕ\��������J�E���^

    '----- �ꎞ��~��ʗp -----
    Public gbExitFlg As Boolean                                     '###014
    Public gbTenKeyFlg As Boolean = True                            ' �e���L�[���̓t���O ###057
    Public gbChkboxHalt As Boolean = True                           ' ADJ�{�^�����(ON=ADJ ON, OFF=ADJ OFF) ###009
    Public gbHaltSW As Boolean = False                              ' HALT SW��ԑޔ� ###255
    Public gbChkSubstrateSet As Boolean = False                     ' ������{�^����� V4.11.0.0�E
    'V6.0.0.0�O    Public gObjADJ As Object = Nothing                              ' �ꎞ��~��ʃI�u�W�F�N�g ###053
    Public gObjADJ As frmFineAdjust = Nothing                       ' �ꎞ��~��ʃI�u�W�F�N�g      'V6.0.0.0�O

    'Public gObjMSG As Object = Nothing                              ' ������҂����b�Z�[�W�\���p
    Public gObjMSG As FrmWait = Nothing                              ' ������҂����b�Z�[�W�\���p

    Public gbLastSubstrateSet As Boolean = False                    ' �ŏI������@'V4.11.0.0�O


    '----- EXTOUT LED����r�b�g -----                               '###061
    Public glLedBit As Long                                         ' LED����r�b�g(EXTOUT) 

    '----- GP-IB���� -----
    Public bGpib2Flg As Integer = 0                                 ' GP-IB����(�ėp)�t���O(0=����Ȃ�, 1=���䂠��) ###229
    'V1.13.0.0�D
    'Public Const BLOCK_COUNT_MAX As Integer = 256          ' �u���b�N���ő吔   'V5.0.0.8�@ TrimDataEditor��

    Public Coordinates(BLOCK_COUNT_MAX, BLOCK_COUNT_MAX) As POINTD
    Public OriginalBlock(BLOCK_COUNT_MAX, BLOCK_COUNT_MAX) As POINTD
    'Public SelectBlock(BLOCK_COUNT_MAX, BLOCK_COUNT_MAX) As Integer            'V5.0.0.8�@ TrimDataEditor��
    'V1.13.0.0�D

    ''' <summary>�����Ώۃu���b�N [1ORG] (0:�������Ȃ�, 1:��������)</summary>
    Friend ProcBlock(,) As Integer = Nothing                            '#4.12.2.0�A

    Public INFO_MSG18, INFO_MSG20, ERR_TXNUM_E As String                'V4.4.0.0-0

    'V2.0.0.0�L�@��
    '----- �r���؂茟�o�p�\���� -----�@
    Public Structure MidiumCut_Struct                     ' �r���؂茟�o�p�\���� 
        Dim DetectFunc As Long                                  ' �r���؂茟�o�t���O(0:���Ȃ�, 1:����)
        Dim JudgeCount As Long                                  ' �r���؂茟�o����� (��)
        Dim dblChangeRate As Double                             ' �r���؂茟�o�ω��� (%)
    End Structure

    Public MidiumCut As MidiumCut_Struct
    'V2.0.0.0�L�@��

    'V2.0.0.0�M�@��
    Public glProbeRetryCount As Long
    'V2.0.0.0�M�@��
#If START_KEY_SOFT Then
    Public gbStartKeySoft As Boolean
#End If
    '----- V2.0.0.0_30�� -----
    ' �ҏW��ʂ��ł���܂ł̈ꎞ�I�ȕۑ��p�ϐ��F�{���̓g���~���O�f�[�^�ϐ�����擾���� 
    '
    Public globaldblCleaningPosX As Double
    Public globaldblCleaningPosY As Double
    Public globaldblCleaningPosZ As Double
    '----- V2.0.0.0_30�� -----

    '----- V2.0.0.0_29�� -----
    '--------------------------------------------------------------------------
    '   ��PCL�p�����[�^(SL436S�p)�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure PCLAXIS_Info
        Dim FL As Integer                                   ' �����x�iFL�j
        Dim FH As Integer                                   ' ���쑬�x�iFH�j
        Dim DrvRat As Integer                               ' �������[�g
        Dim Magnif As Integer                               ' �{��
    End Structure
    Public stPclAxisPrm(2, 2) As PCLAXIS_Info               ' X,Y��PCL�p�����[�^(��, FH/STE&PREPEAT) 0 ORG
    '----- V2.0.0.0_29�� -----

    Public INTERNAL_CAMERA As Integer = 0                   ' �������  'V3.0.0.0�B
    Public EXTERNAL_CAMERA As Integer = 1                   ' �O�����  'V3.0.0.0�B

    '----- V4.0.0.0-58�� -----
    ' �g���}�[���H�����\����(���[�N) 
    Public gwkCND As TrimCondInfo                           ' �g���}�[���H����(�`����`��Rs232c.vb�Q��)
    Public gwkCndCount As Integer                           ' �o�^�� 
    Public gwkPower(MAX_BANK_NUM) As Double
    '----- V4.0.0.0-58�� -----

    'V4.9.0.0�@
    Public Const UNIT_BLOCK As Integer = 0                  ' �u���b�N�P��
    Public Const UNIT_PLATE As Integer = 1                  ' ��P��
    Public Const UNIT_LOT As Integer = 2                    ' Lot�P��

    Public Const UNIT_LO_NG As Integer = 0                  ' Low-NG
    Public Const UNIT_HI_NG As Integer = 1                  ' High-NG
    Public Const UNIT_OPEN_NG As Integer = 2                ' Open-NG

    Public Structure NG_RATE_STOP_Info
        Dim CheckTimmingPlate As Boolean                     '����̃^�C�~���O�����
        Dim CheckTimmingBlock As Boolean                    '����̃^�C�~���O�u���b�N����
        Dim CheckYeld As Boolean
        Dim CheckOverRange As Boolean
        Dim CheckITHI As Boolean
        Dim CheckITLO As Boolean
        Dim CheckFTHI As Boolean
        Dim CheckFTLO As Boolean

        Dim SelectUnit As Integer                           ' �P�ʂ̑I��Block/Plate/Lot
        Dim ValYield As Double                              ' �����܂蔻�聓
        Dim ValOverRange As Double                          ' OverRange���聓
        Dim ValITHI As Double                               ' ITHI���聓
        Dim ValITLO As Double                               ' ITLO���聓
        Dim ValFTHI As Double                               ' FTHI���聓
        Dim ValFTLO As Double                               ' FTLO���聓
    End Structure
    Public JudgeNgRate As NG_RATE_STOP_Info

    Public NGJudgeResult As Integer
    'V4.9.0.0�@

    Public bAllMagazineFinFlag As Boolean                   ' �S�}�K�W���I����� 'V5.0.0.1�A
    Public bEmergencyOccurs As Boolean = False              ' V1.25.0.5�A
    Public gbAutoOperating As Boolean = False               ' �����^�]������s�t���O V6.0.3.0_31

    Public flgLoginPWD As Short                             ' ۸޲��߽ܰ�ޓ��̗͂L��(0:��, 1:�L) V6.0.3.0_42

#End Region

    '========================================================================================
    '   �W���O����p�ϐ���`(�s�w/�s�x�e�B�[�`���O������)
    '========================================================================================
#Region "�W���O����p�ϐ���`"
    '-------------------------------------------------------------------------------
    '   �W���O����p��`
    '-------------------------------------------------------------------------------
    Public giCurrentNo As Integer                               ' �������̍s�ԍ�(�O���b�h�\���p)

    '----- JOG����p�p�����[�^�`����`(OcxJOG���g�p���Ȃ��ꍇ) -----
    Public Structure JOG_PARAM
        Dim Md As Short                                         ' �������[�h(0:XYð��وړ�, 1:BP�ړ�, 2:�L�[���͑҂����[�h)
        Dim Md2 As Short                                        ' ���̓��[�h(0:������ݓ���, 1:�ݿ�ٓ���)
        Dim Opt As UShort                                       ' �I�v�V����(�L�[�̗L��(1)/����(0)�w��)
        '                                                       '  BIT0:START�L�[
        '                                                       '  BIT1:RESET�L�[
        '                                                       '  BIT2:Z�L�[
        '                                                       '  BIT3:
        '                                                       '  BIT4:���g�p
        '                                                       '  BIT5:HALT�L�[
        '                                                       '  BIT6:���g�p
        '                                                       '  BIT7-15:���g�p
        Dim Flg As Short                                        ' �e��ʂ�OK/Cancel���݉����׸�(cFRS_ERR_ADV, cFRS_ERR_RST)
        Dim PosX As Double                                      ' BP or ð��� X�ʒu
        Dim PosY As Double                                      ' BP or ð��� Y�ʒu
        Dim BpOffX As Double                                    ' BP�̾��X 
        Dim BpOffY As Double                                    ' BP�̾��Y
        Dim BszX As Double                                      ' ��ۯ�����X 
        Dim BszY As Double                                      ' ��ۯ�����Y
        'V6.0.0.0�J        Dim TextX As Object                                     ' BP or ð��� X�ʒu�\���p÷���ޯ��
        Dim TextX As Control                                     ' BP or ð��� X�ʒu�\���p÷���ޯ��  'V6.0.0.0�J
        'V6.0.0.0�J        Dim TextY As Object                                     ' BP or ð��� Y�ʒu�\���p÷���ޯ��
        Dim TextY As Control                                     ' BP or ð��� Y�ʒu�\���p÷���ޯ��  'V6.0.0.0�J
        Dim cgX As Double                                       ' �ړ���X 
        Dim cgY As Double                                       ' �ړ���Y
        Dim bZ As Boolean                                       ' Z�L�[  (True:ON, False:OFF)

        'V6.0.0.0�J        Dim BtnHI As Object                                     ' HI�{�^��
        Dim BtnHI As Button                                     ' HI�{�^��     'V6.0.0.0�J
        'V6.0.0.0�J        Dim BtnZ As Object                                      ' Z�{�^��
        Dim BtnZ As Button                                      ' Z�{�^��      'V6.0.0.0�J
        'V6.0.0.0�J        Dim BtnSTART As Object                                  ' START�{�^��
        Dim BtnSTART As Button                                  ' START�{�^��  'V6.0.0.0�J
        'V6.0.0.0�J        Dim BtnHALT As Object                                   ' HALT�{�^��
        Dim BtnHALT As Button                                   ' HALT�{�^��   'V6.0.0.0�J
        'V6.0.0.0�J        Dim BtnRESET As Object                                  ' RESET�{�^��
        Dim BtnRESET As Button                                  ' RESET�{�^��

        Dim TenKey() As Button          'V6.0.0.0-22
        Dim KeyDown As Keys             'V6.0.0.0-22

        Dim CurrentNo As Integer                                ' �������̍s�ԍ�(�O���b�h�\���p)

        Public Sub ResetButtonStyle()   'V6.0.0.0-22
            For Each btn As Button In TenKey
                btn.FlatStyle = FlatStyle.Standard
            Next
            TenKey(0).Parent.Select()

            KeyDown = Keys.None
        End Sub
    End Structure

    '----- ZINPSTS�֐�(�R���\�[������)�ߒl -----
    Public Const CONSOLE_SW_START As UShort = &H1           ' bit 0(01)  : START       0/1=������/����
    Public Const CONSOLE_SW_RESET As UShort = &H2           ' bit 1(02)  : RESET       0/1=������/����
    Public Const CONSOLE_SW_ZSW As UShort = &H4             ' bit 2(04)  : Z_ON/OFF_SW 0/1=������/����
    Public Const CONSOLE_SW_ZDOWN As UShort = &H8           ' bit 3(08)  : Z_DOWN      1=��ԃZ���X
    Public Const CONSOLE_SW_ZUP As UShort = &H10            ' bit 4(10)  : Z_UP        1=��ԃZ���X
    Public Const CONSOLE_SW_HALT As UShort = &H20           ' bit 5(20)  : HALT        0/1=������/����

    '----- �R���\�[���L�[SW -----
    'Public Const cBIT_ADV As UShort = &H1US                 ' START(ADV)�L�[
    'Public Const cBIT_HALT As UShort = &H2US                ' HALT�L�[
    'Public Const cBIT_RESET As UShort = &H8US               ' RESET�L�[
    'Public Const cBIT_Z As UShort = &H20US                  ' Z�L�[
    Public Const cBIT_HI As UShort = &H100US                ' HI�L�[

    '----- �������[�h��` -----
    Public Const MODE_STG As Integer = 0                    ' XY�e�[�u�����[�h
    Public Const MODE_BP As Integer = 1                     ' BP���[�h
    Public Const MODE_KEY As Integer = 2                    ' �L�[���͑҂����[�h

    '----- �v���[�u���[�h/�T�u���[�h��` -----
    'Public Const MODE_STG      As Integer = 0              ' XY�e�[�u�����[�h
    'Public Const MODE_BP       As Integer = 1              ' BP���[�h
    Public Const MODE_Z As Integer = 2                      ' ZӰ��
    Public Const MODE_TTA As Integer = 3                    ' ��Ӱ��
    Public Const MODE_Z2 As Integer = 4                     ' Z2Ӱ��

    Public Const MODE_PRB As Integer = 10                   ' �ڐG�ʒu�m�F���[�h
    Public Const MODE_RECOG As Integer = 20                 ' �ƕ␳�蓮�ʒu�������[�h
    ' ���A�v�����[�h�́u�g���~���O���v
    Public Const MODE_POSOFS As Integer = 21                ' �␳�|�W�V�����I�t�Z�b�g�������[�h
    ' ���A�v�����[�h�́u�p�^�[���o�^(�ƕ␳)�v

    '----- ���̓��[�h -----
    Public Const MD2_BUTN As Integer = 0                    ' ��ʃ{�^������
    Public Const MD2_CONS As Integer = 1                    ' �R���\�[������
    Public Const MD2_BOTH As Integer = 2                    ' ����

    '----- �s�b�`�ő�l/�ŏ��l -----
    Public Const cPT_LO As Double = 0.001                   ' �߯��ŏ��l(mm)
    Public Const cPT_HI As Double = 0.1                     ' �߯��ő�l(mm)
    Public Const cHPT_LO As Double = 0.01                   ' HIGH�߯��ŏ��l(mm)
    Public Const cHPT_HI As Double = 5.0#                   ' HIGH�߯��ő�l(mm)
    Public Const cPAU_LO As Double = 0.05                   ' �|�[�Y�ŏ��l(sec)
    Public Const cPAU_HI As Double = 1.0#                   ' �|�[�Y�ő�l(sec)

    '----- �Y���� -----
    Public Const IDX_PIT As Short = 0                       ' �߯�
    Public Const IDX_HPT As Short = 1                       ' HIGH�߯�
    Public Const IDX_PAU As Short = 2                       ' �|�[�Y

    '----- ���̑� -----
    'Private dblTchMoval(3) As Double                           ' �߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time(Sec))
    Private InpKey As UShort                                    ' �ݿ�ٷ����͈� 
    Private cin As UShort                                       ' �ݿ�ٓ��͒l
    Private bZ As Boolean                                       ' Z�L�[ �ޔ��� (True:ON, False:OFF)
    Private bHI As Boolean                                      ' HI�L�[(True:ON, False:OFF)

    Private mPIT As Double                                      ' �ړ��߯�
    Private X As Double                                         ' �ړ��߯�(X)
    Private Y As Double                                         ' �ړ��߯�(Y)
    Private NOWXP As Double                                     ' BP���ݒlX(�۽ײݕ␳�p)
    Private NOWYP As Double                                     ' BP���ݒlY(�۽ײݕ␳�p)
    Private mvx As Double                                       ' BP/ð��ٓ��̈ʒuX
    Private mvy As Double                                       ' BP/ð��ٓ��̈ʒuY
    Private mvxBk As Double                                     ' BP/ð��ٓ��̈ʒuX(�ޔ�p)
    Private mvyBk As Double                                     ' BP/ð��ٓ��̈ʒuY(�ޔ�p)
#End Region

    '========================================================================================
    '   �i�n�f�����ʏ����p���ʊ֐�
    '========================================================================================
#Region "�����ݒ菈��"
    '''=========================================================================
    '''<summary>�����ݒ菈��</summary>
    '''<param name="stJOG">       (INP)JOG����p�p�����[�^</param>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    '''=========================================================================
    Public Sub JogEzInit(ByVal stJOG As JOG_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double)

        Dim strMSG As String

        Try
            ' �ړ��s�b�`�X���C�_�[�����ݒ�
            If (stJOG.Md = MODE_BP) Then                            ' ���[�h = 1(BP�ړ�) ?
                dblTchMoval(IDX_PIT) = gSysPrm.stSYP.gBpPIT         ' BP�p�߯��ݒ�
                dblTchMoval(IDX_HPT) = gSysPrm.stSYP.gBpHighPIT
                dblTchMoval(IDX_PAU) = gSysPrm.stSYP.gPitPause
            Else
                dblTchMoval(IDX_PIT) = gSysPrm.stSYP.gPIT           ' XY�e�[�u���p�߯��ݒ�
                dblTchMoval(IDX_HPT) = gSysPrm.stSYP.gStageHighPIT
                dblTchMoval(IDX_PAU) = gSysPrm.stSYP.gPitPause
            End If
            Call XyzBpMovingPitchInit(TBarLowPitch, TBarHiPitch, TBarPause, _
                                      LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            Call Form1.System1.SetSysParam(gSysPrm)                 ' �V�X�e���p�����[�^�̐ݒ�(OcxSystem�p)

            InpKey = 0

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.JogEzInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "BP/XY�e�[�u����JOG����(Do Loop�Ȃ�)"
    '''=========================================================================
    '''<summary>BP/XY�e�[�u����JOG���� ###047</summary>
    '''<param name="stJOG">       (INP)JOG����p�p�����[�^</param>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    '''<returns>cFRS_ERR_ADV = OK(START��) 
    '''         cFRS_ERR_RST = Cancel(RESET��)
    '''         cFRS_ERR_HLT = HALT��
    '''         -1�ȉ�       = �G���[</returns>
    ''' <remarks>JogEzInit�֐���Call�ςł��邱��</remarks>
    '''=========================================================================
    Public Function JogEzMove_Ex(ByRef stJOG As JOG_PARAM, ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double) As Integer

        Dim strMSG As String
        Dim r As Short

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            X = 0.0 : Y = 0.0                                           ' �ړ��߯�X,Y
            mvx = stJOG.PosX : mvy = stJOG.PosY                         ' BP or ð��وʒuX,Y
            mvxBk = stJOG.PosX : mvyBk = stJOG.PosY
            ' �L�����u���[�V�������s/�J�b�g�ʒu�␳�y�O���J�����z�� �����΍��W��\�����邽�߃N���A���Ȃ�
            ' �g���~���O���̈ꎞ��~��ʂ��N���A���Ȃ�
            If (giAppMode <> APP_MODE_CARIB_REC) And (giAppMode <> APP_MODE_CUTREVIDE) And _
               (giAppMode <> APP_MODE_FINEADJ) And (giAppMode <> APP_MODE_LDR_ALRM) Then   'V1.16.0.0�I   '###088
                '(giAppMode <> APP_MODE_TRIM) Then                      '###088
                stJOG.cgX = 0.0# : stJOG.cgY = 0.0#                     ' �ړ���X,Y
            End If

            'If (giAppMode = APP_MODE_TRIM) Then                        '###088
            If (giAppMode = APP_MODE_FINEADJ) Then                      '###088
                mvx = stJOG.cgX - stJOG.BpOffX : mvy = stJOG.cgY - stJOG.BpOffY
                mvxBk = mvx : mvyBk = mvy
            End If

            Call Init_Proc(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' ���݂̈ʒu��\������(÷���ޯ���̔w�i�F��������(���F)�ɐݒ肷��)
            Call DispPosition(stJOG, 1)
            'Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            'Call Me.Focus()                                            ' �t�H�[�J�X��ݒ肷��(�e���L�[���͂̂���)
            ''                                                          ' KeyPreview�v���p�e�B��True�ɂ���ƑS�ẴL�[�C�x���g���܂��t�H�[�����󂯎��悤�ɂȂ�B
            '---------------------------------------------------------------------------
            '   �R���\�[���{�^�����̓R���\�[���L�[����̃L�[���͏������s��
            '---------------------------------------------------------------------------
            ' �V�X�e���G���[�`�F�b�N
            r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
            If (r <> cFRS_NORMAL) Then Return (r)

            ' ���b�Z�[�W�|���v
            System.Windows.Forms.Application.DoEvents()

            '----- ###232�� -----
            '' �␳�N���X���C���\������(BP�ړ����[�h��Teach��)
            'If (stJOG.Md = MODE_BP) Then                                ' ���[�h = 1(BP�ړ�) ?
            '    NOWXP = 0.0 : NOWYP = 0.0
            '    If (gSysPrm.stCRL.giDspFlg = 1) Then                    ' �␳�N���X���C���\�� ?
            '        If (gSysPrm.stCRL.giDspFlg = 1) And _
            '           (giAppMode = APP_MODE_TEACH) Then                ' �␳�N���X���C���\�� ?
            '            Call ZGETBPPOS(NOWXP, NOWYP)                    ' BP���݈ʒu�擾
            '            gstCLC.x = NOWXP                                ' BP�ʒuX(mm)
            '            gstCLC.y = NOWYP                                ' BP�ʒuY(mm)
            '            Call CrossLineCorrect(gstCLC)                   ' �␳�N���X���C���\��
            '        End If
            '    End If
            'End If
            '----- ###232�� -----

            ' �R���\�[���{�^�����̓R���\�[���L�[����̃L�[����
            Call ReadConsoleSw(stJOG, cin)                              ' �L�[����

            '-----------------------------------------------------------------------
            '   ���̓L�[�`�F�b�N
            '-----------------------------------------------------------------------
            If (cin And CONSOLE_SW_RESET) Then                          ' RESET SW ?
                ' RESET SW������
                If (stJOG.Opt And CONSOLE_SW_RESET) Then                ' RESET�L�[�L�� ?
                    Return (cFRS_ERR_RST)                               ' Return�l = Cancel(RESET��)
                End If

                ' HALT SW������
            ElseIf (cin And CONSOLE_SW_HALT) Then                       ' HALT SW ?
                If (stJOG.Opt And CONSOLE_SW_HALT) Then                 ' �I�v�V����(0:HALT�L�[����, 1:HALT�L�[�L��)
                    r = cFRS_ERR_HALT                                   ' Return�l = HALT��
                    GoTo STP_END
                End If

                ' START SW������
            ElseIf (cin And CONSOLE_SW_START) Then                      ' START SW ?
                ''V4.0.0.0-86
                If (gKeiTyp = KEY_TYPE_RS) Then
                    r = GetLaserOffIO(True) 'V5.0.0.1�K
                    If r = 1 Then
                        ''V5.0.0.1�H��
                        r = cFRS_NORMAL
                        Call ZCONRST()
                        ''V5.0.0.1�H��
                    Else
                        If (stJOG.Opt And CONSOLE_SW_START) Then                ' START�L�[�L�� ?
                            r = cFRS_ERR_START                                  ' Return�l = OK(START��) 
                            GoTo STP_END
                        End If
                    End If
                Else
                    If (stJOG.Opt And CONSOLE_SW_START) Then                ' START�L�[�L�� ?
                        r = cFRS_ERR_START                                  ' Return�l = OK(START��) 
                        GoTo STP_END
                    End If
                End If
                ''V4.0.0.0-86


                ' Z SW��ON����OFF(����OFF����ON)�ɐؑւ������
            ElseIf (stJOG.bZ <> bZ) Then
                If (stJOG.Opt And CONSOLE_SW_ZSW) Then                  ' Z�L�[�L�� ?
                    r = cFRS_ERR_Z                                      ' Return�l = Z��ON/OFF
                    stJOG.bZ = bZ                                       ' ON/OFF
                    GoTo STP_END
                End If

                ' ���SW������
            ElseIf cin And &H1E00US Then                                ' ���SW
                '�u�L�[���͑҂����[�h�v�Ȃ牽�����Ȃ�
                If (stJOG.Md = MODE_KEY) Then

                Else
                    If cin And &H100US Then                             ' HI SW ? 
                        mPIT = dblTchMoval(IDX_HPT)                     ' mPIT = �ړ������߯�
                    Else
                        mPIT = dblTchMoval(IDX_PIT)                     ' mPIT = �ړ��ʏ��߯�
                    End If

                    ' XY�e�[�u����Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)
                    r = cFRS_NORMAL
                    If (stJOG.Md = MODE_STG) Then                       ' ���[�h = XY�e�[�u���ړ� ?
                        ' XY�e�[�u����Βl�ړ�
                        r = Sub_XYtableMove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then                      ' �װ ?
                            If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                Return (r)                              ' ����ЯĴװ�ȊO�ʹװ����
                            End If
                        End If

                        '  ���[�h = BP�ړ��̏ꍇ
                    ElseIf (stJOG.Md = MODE_BP) Then
                        ' BP��Βl�ړ�
                        r = Sub_BPmove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then                      ' BP�ړ��G���[ ?
                            If (Form1.System1.IsSoftLimitBP(r) = False) Then
                                Return (r)                              ' ����ЯĴװ�ȊO�ʹװ����
                            End If
                        End If
                    End If

                    ' �\�t�g���~�b�g�G���[�̏ꍇ�� HI SW�ȊO��OFF����
                    If (r <> cFRS_NORMAL) Then                          ' �װ ?
                        If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then
                            InpKey = cBIT_HI                            ' HI SW ON
                        Else
                            InpKey = 0                                  ' HI SW�ȊO��OFF
                        End If
                        r = cFRS_NORMAL                                 ' Retuen�l = ���� ###143 
                        stJOG.ResetButtonStyle()                        'V6.0.0.0-22
                    End If

                    ' ���݂̈ʒu��\������
                    Call DispPosition(stJOG, 1)
                    'Call Form1.System1.WAIT(SysPrm.stSYP.gPitPause)    ' Wait(sec)'###251
                    Call Form1.System1.WAIT(dblTchMoval(IDX_PAU))       ' Wait(sec)'###251
                End If

                InpKey = CType(CtrlJog.MouseClickLocation.Clear(InpKey), UShort)    'V6.0.0.0�G
                stJOG.KeyDown = Keys.None                                           'V6.0.0.0-23
            End If

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
STP_END:
            'stJOG.PosX = mvx                                            ' �ʒuX,Y�X�V
            'stJOG.PosY = mvy
            Return (r)                                                  ' Return�l�ݒ� 

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.JogEzMove_Ex() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[ 
        End Try

    End Function
#End Region

#Region "BP/XY�e�[�u����JOG����"
    '''=========================================================================
    '''<summary>BP/XY�e�[�u����JOG����</summary>
    '''<param name="stJOG">       (INP)JOG����p�p�����[�^</param>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    ''' <param name="commonMethods">���C���t�H�[���ɐݒ肷��JOG����֐�</param>
    '''<returns>cFRS_ERR_ADV = OK(START��) 
    '''         cFRS_ERR_RST = Cancel(RESET��)
    '''         cFRS_ERR_HLT = HALT��
    '''         -1�ȉ�       = �G���[</returns>
    ''' <remarks>JogEzInit�֐���Call�ςł��邱��</remarks>
    '''=========================================================================
    Public Function JogEzMove(ByRef stJOG As JOG_PARAM, ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label,
                         ByRef dblTchMoval() As Double,
                         ByVal commonMethods As ICommonMethods) As Integer      'V6.0.0.0�J  ���� ICommonMethods �ǉ�
        'V6.0.0.0�J                         ByRef dblTchMoval() As Double) As Integer 

        Dim strMSG As String
        Dim r As Short

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            X = 0.0 : Y = 0.0                                   ' �ړ��߯�X,Y
            mvx = stJOG.PosX : mvy = stJOG.PosY                 ' BP or ð��وʒuX,Y
            mvxBk = stJOG.PosX : mvyBk = stJOG.PosY
            ' �L�����u���[�V�������s/�J�b�g�ʒu�␳�y�O���J�����z�� �����΍��W��\�����邽�߃N���A���Ȃ�
            ' �g���~���O���̈ꎞ��~��ʂ��N���A���Ȃ�
            If (giAppMode <> APP_MODE_CARIB_REC) And (giAppMode <> APP_MODE_CUTREVIDE) And _
               (giAppMode <> APP_MODE_FINEADJ) And (giAppMode <> APP_MODE_CARIB) Then             ' V1.14.0.0�F ###088
                '(giAppMode <> APP_MODE_TRIM) Then              '###088
                stJOG.cgX = 0.0# : stJOG.cgY = 0.0#             ' �ړ���X,Y
            End If
            stJOG.Flg = -1
            InpKey = 0
            Call Init_Proc(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' ���݂̈ʒu��\������(÷���ޯ���̔w�i�F��������(���F)�ɐݒ肷��)
            Call DispPosition(stJOG, 1)
            'Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            'Call Me.Focus()                                     ' �t�H�[�J�X��ݒ肷��(�e���L�[���͂̂���)
            ''                                                   ' KeyPreview�v���p�e�B��True�ɂ���ƑS�ẴL�[�C�x���g���܂��t�H�[�����󂯎��悤�ɂȂ�B

            ' ���C���t�H�[����JOG����֐���ݒ肷��      'V6.0.0.0�J
            Form1.Instance.SetActiveJogMethod(AddressOf commonMethods.JogKeyDown,
                                              AddressOf commonMethods.JogKeyUp,
                                              AddressOf commonMethods.MoveToCenter)
            '---------------------------------------------------------------------------
            '   �R���\�[���{�^�����̓R���\�[���L�[����̃L�[���͏������s��
            '---------------------------------------------------------------------------
            Do
                ' �V�X�e���G���[�`�F�b�N
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then GoTo STP_END

                ' ���b�Z�[�W�|���v
                '  ��VB.NET�̓}���`�X���b�h�Ή��Ȃ̂ŁA�{���̓C�x���g�̊J���ȂǂłȂ��A
                '    �X���b�h�𐶐����ăR�[�f�B���O������̂��������B
                '    �X���b�h�łȂ��Ă��A�Œ�Ń^�C�}�[�𗘗p����B
                System.Windows.Forms.Application.DoEvents()
                System.Threading.Thread.Sleep(10)               ' CPU�g�p���������邽�߃X���[�v

                '----- ###232�� -----
                '' �␳�N���X���C���\������(BP�ړ����[�h��Teach��)
                'If (stJOG.Md = MODE_BP) Then                    ' ���[�h = 1(BP�ړ�) ?
                '    NOWXP = 0.0 : NOWYP = 0.0
                '    If (gSysPrm.stCRL.giDspFlg = 1) Then        ' �␳�N���X���C���\�� ?
                '        If (gSysPrm.stCRL.giDspFlg = 1) And _
                '           (giAppMode = APP_MODE_TEACH) Then    ' �␳�N���X���C���\�� ?
                '            Call ZGETBPPOS(NOWXP, NOWYP)        ' BP���݈ʒu�擾
                '            gstCLC.x = NOWXP                    ' BP�ʒuX(mm)
                '            gstCLC.y = NOWYP                    ' BP�ʒuY(mm)
                '            Call CrossLineCorrect(gstCLC)       ' �␳�N���X���C���\��
                '        End If
                '    End If
                'End If
                '----- ###232�� -----

                ' �R���\�[���{�^�����̓R���\�[���L�[����̃L�[����
                Call ReadConsoleSw(stJOG, cin)                  ' �L�[����

                '-----------------------------------------------------------------------
                '   ���̓L�[�`�F�b�N
                '-----------------------------------------------------------------------
                If (cin And CONSOLE_SW_RESET) Then              ' RESET SW ?
                    ' RESET SW������
                    If (stJOG.Opt And CONSOLE_SW_RESET) Then    ' RESET�L�[�L�� ?
                        r = cFRS_ERR_RST                        ' Return�l = Cancel(RESET��)
                        Exit Do
                    End If

                    ' HALT SW������
                ElseIf (cin And CONSOLE_SW_HALT) Then           ' HALT SW ?
                    If (stJOG.Opt And CONSOLE_SW_HALT) Then     ' �I�v�V����(0:HALT�L�[����, 1:HALT�L�[�L��)
                        r = cFRS_ERR_HALT                       ' Return�l = HALT��
                        Exit Do
                    End If

                    ' START SW������
                    'V6.1.4.2�@                ElseIf (cin And CONSOLE_SW_START) Then          ' START SW ?
                ElseIf (cin And CONSOLE_SW_START) Or gbAutoCalibration Then          ' START SW ? 'V6.1.4.2�@[�����L�����u���[�V�����␳���s]
                    If (stJOG.Opt And CONSOLE_SW_START) Then    ' START�L�[�L�� ?
                        'stJOG.PosX = mvx                       ' �ʒuX,Y�X�V
                        'stJOG.PosY = mvy
                        r = cFRS_ERR_START                      ' Return�l = OK(START��) 
                        Exit Do
                    End If

                    ' Z SW��ON����OFF(����OFF����ON)�ɐؑւ������
                ElseIf (stJOG.bZ <> bZ) Then
                    If (stJOG.Opt And CONSOLE_SW_ZSW) Then      ' Z�L�[�L�� ?
                        r = cFRS_ERR_Z                          ' Return�l = Z��ON/OFF
                        stJOG.bZ = bZ                           ' ON/OFF
                        Exit Do
                    End If

                    ' ���SW������
                ElseIf cin And &H1E00US Then                    ' ���SW
                    '�u�L�[���͑҂����[�h�v�Ȃ牽�����Ȃ�
                    If (stJOG.Md = MODE_KEY) Then

                    Else
                        If cin And &H100US Then                     ' HI SW ? 
                            mPIT = dblTchMoval(IDX_HPT)             ' mPIT = �ړ������߯�
                        Else
                            mPIT = dblTchMoval(IDX_PIT)             ' mPIT = �ړ��ʏ��߯�
                        End If

                        ' XY�e�[�u����Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)
                        r = cFRS_NORMAL
                        If (stJOG.Md = MODE_STG) Then                ' ���[�h = XY�e�[�u���ړ� ?
                            ' XY�e�[�u����Βl�ړ�
                            r = Sub_XYtableMove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                            If (r <> cFRS_NORMAL) Then              ' �װ ?
                                If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                    GoTo STP_END                    ' ����ЯĴװ�ȊO�ʹװ����
                                End If
                            End If

                            '  ���[�h = BP�ړ��̏ꍇ
                        ElseIf (stJOG.Md = MODE_BP) Then
                            ' BP��Βl�ړ�
                            r = Sub_BPmove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                            If (r <> cFRS_NORMAL) Then              ' BP�ړ��G���[ ?
                                If (Form1.System1.IsSoftLimitBP(r) = False) Then
                                    GoTo STP_END                    ' ����ЯĴװ�ȊO�ʹװ����
                                End If
                            End If
                        End If

                        ' �\�t�g���~�b�g�G���[�̏ꍇ�� HI SW�ȊO��OFF����
                        If (r <> cFRS_NORMAL) Then                  ' �װ ?
                            If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then
                                InpKey = cBIT_HI                    ' HI SW ON
                            Else
                                InpKey = 0                          ' HI SW�ȊO��OFF
                            End If
                            stJOG.ResetButtonStyle()                'V6.0.0.0-22
                        End If

                        ' ���݂̈ʒu��\������
                        Call DispPosition(stJOG, 1)
                        'V1.18.0.3�D                        Call Form1.System1.WAIT(SysPrm.stSYP.gPitPause)    ' Wait(sec)
                        Call Form1.System1.WAIT(dblTchMoval(IDX_PAU))    ' Wait(sec) V1.18.0.3�D
                    End If

                    InpKey = CType(CtrlJog.MouseClickLocation.Clear(InpKey), UShort)    'V6.0.0.0�G
                    stJOG.KeyDown = Keys.None                                           'V6.0.0.0-23
                End If

            Loop While (stJOG.Flg = -1)

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
            ' ���W�\���p÷���ޯ���̔w�i�F�𔒐F�ɐݒ肷��
            Call DispPosition(stJOG, 0)

            ' �e��ʂ���OK/Cancel���݉��� ?
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            ' OK(START��)�Ȃ�ʒuX,Y�X�V
            If (r = cFRS_ERR_START) Then                            ' OK(START��) ?
                stJOG.PosX = mvx                                    ' �ʒuX,Y�X�V
                stJOG.PosY = mvy
            End If

STP_END:
            Call ZCONRST()                                          ' �ݿ�ٷ�ׯ����� 
            Return (r)                                              ' Return�l�ݒ� 

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.JogEzMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[ 

        Finally
            Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)    'V6.0.0.0�J
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
        End Try
    End Function
#End Region

#Region "�����ݒ菈��"
    '''=========================================================================
    '''<summary>�����ݒ菈��</summary>
    '''<param name="stJOG">       (INP)JOG����p�p�����[�^</param>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    '''=========================================================================
    Private Sub Init_Proc(ByVal stJOG As JOG_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double)

        Dim strMSG As String

        Try

            ' �ړ��s�b�`�X���C�_�[�ݒ�(�O��ݒ肵���l)
            Call XyzBpMovingPitchInit(TBarLowPitch, TBarHiPitch, TBarPause, _
                                      LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' �{�^���L��/�����ݒ�
            If (stJOG.Opt And CONSOLE_SW_HALT) Then                 ' HALT�L�[�L��/����
                stJOG.BtnHALT.Enabled = True
            Else
                stJOG.BtnHALT.Enabled = False
            End If
            If (stJOG.Opt And CONSOLE_SW_START) Then                ' START�L�[�L��/����
                stJOG.BtnSTART.Enabled = True
            Else
                stJOG.BtnSTART.Enabled = False
            End If
            If (stJOG.Opt And CONSOLE_SW_RESET) Then                ' RESET�L�[�L��/����
                stJOG.BtnRESET.Enabled = True
            Else
                stJOG.BtnRESET.Enabled = False
            End If
            If (stJOG.Opt And CONSOLE_SW_ZSW) Then                  ' Z�L�[�L��/����
                stJOG.BtnZ.Enabled = True
            Else
                stJOG.BtnZ.Enabled = False
            End If

            ' Z�L�[/HI�L�[��ԓ��ޔ�
            bZ = stJOG.bZ                                           ' Z�L�[�ޔ�
            If (bZ = False) Then                                    ' Z�{�^���̔w�i�F��ݒ�
                stJOG.BtnZ.BackColor = System.Drawing.SystemColors.Control ' �w�i�F = �D�F
                stJOG.BtnZ.Text = "Z Off"
            Else
                stJOG.BtnZ.BackColor = System.Drawing.Color.Yellow        ' �w�i�F = ���F
                stJOG.BtnZ.Text = "Z On"
            End If

            If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then ' HI�L�[��Ԏ擾
                bHI = True
                InpKey = InpKey Or cBIT_HI                          ' HI SW ON
            Else
                bHI = False
                InpKey = InpKey And Not cBIT_HI                     ' HI SW OFF
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.Init_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "��ʃ{�^�����̓R���\�[���L�[����̃L�[����"
    '''=========================================================================
    '''<summary>��ʃ{�^�����̓R���\�[���L�[����̃L�[����</summary>
    '''<param name="stJOG">(INP)JOG����p�p�����[�^</param>
    '''<param name="cin">  (OUT)�R���\�[�����͒l</param>
    '''=========================================================================
    Private Sub ReadConsoleSw(ByRef stJOG As JOG_PARAM, ByRef cin As UShort)

        Dim r As Integer
        Dim sw As Long
        Dim strMSG As String

        Try
            ' HALT�L�[���̓`�F�b�N
            r = HALT_SWCHECK(sw)
            If (sw <> 0) Then                                           ' HALT�L�[���� ?
                If (stJOG.Opt And CONSOLE_SW_HALT) Then                 ' HALT�L�[�L�� ?
                    cin = CONSOLE_SW_HALT
                    Exit Sub
                End If
            End If

            ' Z�L�[���̓`�F�b�N
            r = Z_SWCHECK(sw)                                           ' Z�X�C�b�`�̏�Ԃ��`�F�b�N����
            If (sw <> 0) Then                                           ' Z�L�[���� ?
                If (stJOG.Opt And CONSOLE_SW_ZSW) Then                  ' Z�L�[�L�� ?
                    Call SubBtnZ_Click(stJOG)
                    Exit Sub
                End If
            End If

            ' START/RESET�L�[���̓`�F�b�N
            r = STARTRESET_SWCHECK(False, sw)                           ' START/RESET�L�[�����`�F�b�N(�Ď��Ȃ����[�h)

            ' �R���\�[�����͒l�ɕϊ����Đݒ�
            If (sw = cFRS_ERR_START) Then                               ' START�L�[���� ?
                If (stJOG.Opt And CONSOLE_SW_START) Then                ' START�L�[�L�� ?
                    cin = CONSOLE_SW_START
                    Exit Sub
                End If
            ElseIf (sw = cFRS_ERR_RST) Then                             ' RESET�L�[���� ?
                If (stJOG.Opt And CONSOLE_SW_RESET) Then                ' RESET�L�[�L�� ?
                    cin = CONSOLE_SW_RESET
                    Exit Sub
                End If
                '    ElseIf (sw = CONSOLE_SW_ZSW) Then                          ' Z�L�[���� ?
                '        If (stJOG.opt And CONSOLE_SW_ZSW) Then                  ' Z�L�[�L�� ?
                '            cin = CONSOLE_SW_ZSW
                '        End If
            End If

            ' �u��ʃ{�^�����́v
            cin = InpKey                                                ' ��ʃ{�^������

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.ReadConsoleSw() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���W�\��"
    '''=========================================================================
    '''<summary>���W�\��</summary>
    '''<param name="stJOG">(INP)JOG����p�p�����[�^</param>
    '''<param name="Md">   (INP)0=�w�i�F�𔒐F�ɐݒ�, 1=�w�i�F��������(���F)�ɐݒ�</param>
    '''=========================================================================
    Private Sub DispPosition(ByVal stJOG As JOG_PARAM, ByVal MD As Integer)

        Dim xPos As Double = 0.0                    ' ###232
        Dim yPos As Double = 0.0                    ' ###232
        Dim OffPosX As Double                       ' V1.13.0.0�E
        Dim OffPosY As Double                       ' V1.13.0.0�E
        Dim strMSG As String

        Try
            '�u�L�[���͑҂����[�h�v�Ȃ�NOP
            If (stJOG.Md = MODE_KEY) Then Exit Sub

            ' �␳�ʒu�e�B�[�`���O�Ȃ�O���b�h�ɕ\������
            If (giAppMode = APP_MODE_CUTPOS) Then
                'V6.0.0.0�J                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 2, (stJOG.PosX + stJOG.cgX).ToString("0.0000"))
                'V6.0.0.0�J                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 3, (stJOG.PosY + stJOG.cgY).ToString("0.0000"))
                Exit Sub

                ' �J�b�g�ʒu�␳�y�O���J�����z�Ȃ�O���b�h�ɑ��΍��W��\������
            ElseIf (giAppMode = APP_MODE_CUTREVIDE) Then
                'V4.7.0.0�I                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 3, (stJOG.cgX).ToString("0.0000"))    ' �����X
                'V4.7.0.0�I                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 4, (stJOG.cgY).ToString("0.0000"))    ' �����Y
                Exit Sub
            End If

            ' �e�L�X�g�{�b�N�X�ɍ��W��\������
            If (MD = 0) Then
                ' �L�����u���[�V�������s���͔w�i�F���D�F�ɐݒ�
                If (giAppMode = APP_MODE_CARIB_REC) Then
                    ' �w�i�F���D�F�ɐݒ�
                    stJOG.TextX.BackColor = System.Drawing.SystemColors.Control
                    stJOG.TextY.BackColor = System.Drawing.SystemColors.Control
                Else
                    ' �w�i�F�𔒐F�ɐݒ�
                    stJOG.TextX.BackColor = System.Drawing.Color.White
                    stJOG.TextY.BackColor = System.Drawing.Color.White
                End If
            Else
                ' �L�����u���[�V�������s���͑��΍��W��\��
                'If (giAppMode = APP_MODE_CARIB_REC) Then   ' V1.14.0.0�F
                If (giAppMode = APP_MODE_CARIB) Then        ' V1.14.0.0�F
                    stJOG.TextX.Text = stJOG.cgX.ToString("0.0000")
                    stJOG.TextY.Text = stJOG.cgY.ToString("0.0000")
                ElseIf (giAppMode = APP_MODE_IDTEACH) Then
                    '                    Call XYtableMoveOffsetPosition(OffPosX, OffPosY)
                    Dim DirX As Integer = 1, DirY As Integer = 1
                    TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)
                    frmIDReaderTeach.ObjOmronIDReader.GetIDReaderPotision(gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, typPlateInfo.dblTableOffsetXDir, typPlateInfo.dblTableOffsetYDir, gfCorrectPosX, gfCorrectPosY, OffPosX, OffPosY)
                    stJOG.TextX.Text = ((stJOG.PosX + stJOG.cgX) * DirX + OffPosX).ToString("0.0000")
                    stJOG.TextY.Text = ((OffPosY - (stJOG.PosY + stJOG.cgY) * DirY) * -1).ToString("0.0000")
                    frmIDReaderTeach.ToucFinderUp()
                Else
                    ' ���̑��̃��[�h���͐�΍��W��\��
                    stJOG.TextX.Text = (stJOG.PosX + stJOG.cgX).ToString("0.0000")
                    stJOG.TextY.Text = (stJOG.PosY + stJOG.cgY).ToString("0.0000")
                    '----- ###232�� -----
                    ' �g���~���O���̈ꎞ��~��ʕ\�����Ȃ�␳�N���X���C����\������
                    If (giAppMode = APP_MODE_FINEADJ) Or (giAppMode = APP_MODE_TX) Then
                        'xPos = Double.Parse(stJOG.TextX.Text)
                        'yPos = Double.Parse(stJOG.TextY.Text)
                        Call ZGETBPPOS(xPos, yPos)
                        ObjCrossLine.CrossLineDispXY(xPos, yPos)
                    End If
                    '----- ###232�� -----
                End If
                ' �w�i�F��������(���F)�ɐݒ�
                stJOG.TextX.BackColor = System.Drawing.Color.Yellow
                stJOG.TextY.BackColor = System.Drawing.Color.Yellow
            End If

            stJOG.TextX.Refresh()
            stJOG.TextY.Refresh()

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.DispPosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�e�B�[�`���O�r�v�擾"
    '''=========================================================================
    ''' <summary>�e�B�[�`���O�r�v�擾</summary>
    ''' <param name="SysPrm">(INP)�V�X�e���p�����[�^</param>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N</param>
    ''' <returns>0=OFF, 1:ON</returns>
    '''=========================================================================
    Private Function Z_TEACHSTS(ByVal SysPrm As SYSPARAM_PARAM, ByVal ObjSys As Object) As Long

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �f�[�^���� & ON�r�b�g�`�F�b�N
            If (SysPrm.stIOC.giTeachSW = 1) Then                    ' �e�B�[�`���OSW���䂠�� ?
                r = ObjSys.Inp_And_Check_Bit(SysPrm.stIOC.glTS_In_Adr, SysPrm.stIOC.glTS_In_ON, SysPrm.stIOC.giTS_In_ON_ST)
                If (r = 1) Then                                     ' TEACH_SW ON ?
                    r = 1                                           ' TEACH_SW ON
                Else
                    r = 0                                           ' TEACH_SW OFF
                End If

            Else
                r = 1                                               ' TEACH_SW ON
            End If
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.Z_TEACHSTS() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (1)
        End Try
    End Function
#End Region

#Region "BP��Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)"
    '''=========================================================================
    ''' <summary>BP��Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)</summary>
    ''' <param name="SysPrm">(INP)�V�X�e���p�����[�^</param>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N</param>
    ''' <param name="ObjUtl">(INP)OcxUtility�I�u�W�F�N</param>
    ''' <param name="stJOG"> (I/O)JOG����p�p�����[�^</param>
    ''' <returns>0=����, 0�ȊO:�G���[</returns>
    '''=========================================================================
    Private Function Sub_BPmove(ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, ByVal ObjSys As Object, ByVal ObjUtl As Object, ByRef stJOG As JOG_PARAM) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' BP�ړ��ʂ̎Z�o(��X,Y)
            mvxBk = mvx                                             ' ���݂̈ʒu�ޔ�
            mvyBk = mvy
            If ((cin And CtrlJog.MouseClickLocation.Move) = &H0) Then           'V6.0.0.0�G
                Call ObjUtl.GetBPmovePitch(cin, X, Y, mPIT, mvx, mvy, SysPrm.stDEV.giBpDirXy)
            Else
                'V6.0.0.0�G              ��
                Dim dirX As Double = 0.0
                Dim dirY As Double = 0.0
                Dim tmpX As Double = 0.0
                Dim tmpY As Double = 0.0
                ObjUtl.GetBPmovePitch(cin, dirX, dirY, 1.0, tmpX, tmpY, SysPrm.stDEV.giBpDirXy)   ' �������擾

                X = Math.Abs(CtrlJog.MouseClickLocation.DistanceX) * Math.Sign(dirX)
                Y = Math.Abs(CtrlJog.MouseClickLocation.DistanceY) * Math.Sign(dirY)
                mvx -= X
                mvy -= Y
                'V6.0.0.0�G              ��
            End If

            ' BP��Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)
            r = ObjSys.BPMOVE(SysPrm, stJOG.BpOffX, stJOG.BpOffY, stJOG.BszX, stJOG.BszY, mvx, mvy, 1)
            If (r <> cFRS_NORMAL) Then                              ' �װ�Ȃ�װ����(���b�Z�[�W�\���ς�)
                If (ObjSys.IsSoftLimitBP(r) = False) Then
                    GoTo STP_END                                    ' ����ЯĴװ�ȊO�ʹװ����
                End If
                mvx = mvxBk                                         ' BP����ЯĴװ����BP�ʒu��߂�
                mvy = mvyBk
                GoTo STP_END                                        ' BP����ЯĴװ
            End If

            stJOG.cgX = stJOG.cgX + (-1 * X)                        ' BP�ړ���X�X�V (���ړ��ʂ͔��]���Ă���̂�-1���|����)
            stJOG.cgY = stJOG.cgY + (-1 * Y)                        ' BP�ړ���Y�X�V

STP_END:
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.Sub_BPmove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "XY�e�[�u����Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)"
    '''=========================================================================
    ''' <summary>XY�e�[�u����Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)</summary>
    ''' <param name="SysPrm">(INP)�V�X�e���p�����[�^</param>
    ''' <param name="ObjSys">(INP)OcxSystem�I�u�W�F�N</param>
    ''' <param name="ObjUtl">(INP)OcxUtility�I�u�W�F�N</param>
    ''' <param name="stJOG"> (I/O)JOG����p�p�����[�^</param>
    ''' <returns>0=����, 0�ȊO:�G���[</returns>
    '''=========================================================================
    Private Function Sub_XYtableMove(ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, ByVal ObjSys As Object, ByVal ObjUtl As Object, ByRef stJOG As JOG_PARAM) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' XY�e�[�u���ړ��ʂ̎Z�o(��X,Y)
            mvxBk = X                                               ' ���݂̈ʒu�ޔ�
            mvyBk = Y
            'Call ObjUtl.GetXYmovePitch(cin, X, Y, mPIT)
            If ((cin And CtrlJog.MouseClickLocation.Move) = &H0) Then           'V6.0.0.0�G
                Call TrimClassCommon.GetXYmovePitch(cin, X, Y, mPIT, giStageYDir) ' V4.0.0.0-51
            Else
                'V6.0.0.0�G              ��
                Dim dirX As Double = 0.0
                Dim dirY As Double = 0.0
                TrimClassCommon.GetXYmovePitch(cin, dirX, dirY, 1.0, giStageYDir)   ' �������擾

                X = -(Math.Abs(CtrlJog.MouseClickLocation.DistanceX) * Math.Sign(dirX)) 'V6.0.0.0-24 -() �ǉ�
                Y = -(Math.Abs(CtrlJog.MouseClickLocation.DistanceY) * Math.Sign(dirY)) 'V6.0.0.0-24 -() �ǉ�
                'V6.0.0.0�G              ��
            End If

            ' XY�e�[�u����Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)
            'V6.1.2.0�@��
            'r = ObjSys.XYtableMove(SysPrm, mvx + X, mvy + Y)
            r = ObjSys.XYtableMove(SysPrm, mvx + X, mvy + Y, giJogWaitMode)
            'V6.1.2.0�@��
            If (r <> cFRS_NORMAL) Then                              ' �װ�Ȃ�װ����(���b�Z�[�W�\���ς�)
                If (ObjSys.IsSoftLimitXY(r) = False) Then
                    GoTo STP_END                                    ' ����ЯĴװ�ȊO�ʹװ����
                End If
                X = mvxBk                                           ' ����ЯĴװ����X,Y�ʒu��߂�
                Y = mvyBk
                GoTo STP_END                                        ' ����ЯĴװ
            End If
            'V4.0.0.0-80    '���������x�����������Ȃ�΍�
            mvx = mvx + 0.00000001
            mvy = mvy + 0.00000001
            mvx = mvx + X
            mvy = mvy + Y
            stJOG.cgX = stJOG.cgX + X                               ' �e�[�u���ړ���X,Y�X�V
            stJOG.cgY = stJOG.cgY + Y
            stJOG.cgX = stJOG.cgX + 0.00000001                               ' �e�[�u���ړ���X,Y�X�V
            stJOG.cgY = stJOG.cgY + 0.00000001
            'stJOG.cgX = stJOG.cgX + X                               ' �e�[�u���ړ���X,Y�X�V
            'stJOG.cgY = stJOG.cgY + Y
            'V4.0.0.0-80

STP_END:
            Return (r)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.Sub_XYtableMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '========================================================================================
    '   �{�^������������(�i�n�f������)
    '========================================================================================
#Region "HALT�{�^������������"
    '''=========================================================================
    '''<summary>HALT�{�^������������</summary>
    '''=========================================================================
    Public Sub SubBtnHALT_Click()
        InpKey = CONSOLE_SW_HALT
    End Sub
#End Region

#Region "START�{�^������������"
    '''=========================================================================
    '''<summary>START�{�^������������</summary>
    '''=========================================================================
    Public Sub SubBtnSTART_Click()
        InpKey = CONSOLE_SW_START
    End Sub
#End Region

#Region "RESET�{�^������������"
    '''=========================================================================
    '''<summary>RESET�{�^������������</summary>
    '''=========================================================================
    Public Sub SubBtnRESET_Click()
        InpKey = CONSOLE_SW_RESET
    End Sub
#End Region

#Region "Z�{�^������������"
    '''=========================================================================
    '''<summary>RESET�{�^������������</summary>
    '''<param name="stJOG">(INP)JOG����p�p�����[�^</param>
    '''=========================================================================
    Public Sub SubBtnZ_Click(ByVal stJOG As JOG_PARAM)

        Dim strMSG As String

        Try
            If (stJOG.BtnZ.BackColor = System.Drawing.Color.Yellow) Then    ' Z SW ON ?
                stJOG.BtnZ.BackColor = System.Drawing.SystemColors.Control
                stJOG.BtnZ.Text = "Z Off"
                InpKey = InpKey And Not CONSOLE_SW_ZSW                      ' Z SW OFF
                bZ = False                                                  ' Z�L�[�ޔ���
            Else
                stJOG.BtnZ.BackColor = System.Drawing.Color.Yellow
                stJOG.BtnZ.Text = "Z On"
                InpKey = InpKey Or CONSOLE_SW_ZSW                           ' Z SW ON
                bZ = True                                                   ' Z�L�[�ޔ���
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.SubBtnZ_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "HI�{�^������������"
    '''=========================================================================
    '''<summary>HI�{�^������������</summary>
    '''<param name="stJOG">(INP)JOG����p�p�����[�^</param>
    '''=========================================================================
    Public Sub SubBtnHI_Click(ByVal stJOG As JOG_PARAM)

        ' �w�i�F��ؑւ���
        If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then   ' �w�i�F = ���F ?
            ' �w�i�F���f�t�H���g�ɂ���
            stJOG.BtnHI.BackColor = System.Drawing.SystemColors.Control
            InpKey = InpKey And Not cBIT_HI                             ' HI SW OFF
        Else
            ' �w�i�F�����F�ɂ���
            stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow
            InpKey = InpKey Or cBIT_HI                                  ' HI SW ON
        End If

    End Sub
#End Region

#Region "InpKey���擾����"
    '''=========================================================================
    '''<summary>InpKey���擾����</summary>
    '''<param name="IKey">(OUT)InpKey</param>
    '''=========================================================================
    Public Sub GetInpKey(ByRef IKey As UShort) '###057
        IKey = InpKey
    End Sub
#End Region

#Region "InpKey��ݒ肷��"
    '''=========================================================================
    '''<summary>InpKey��ݒ肷��</summary>
    '''<param name="IKey">(INP)InpKey</param>
    '''=========================================================================
    Public Sub PutInpKey(ByVal IKey As UShort) '###057
        InpKey = IKey
    End Sub
#End Region

#Region "�J�����摜�\��PictureBox�N���b�N�ʒu��JOG�o�R�ŉ摜�Z���^�[�Ɉړ�����"
    ''' <summary>�J�����摜�\��PictureBox�N���b�N�ʒu��JOG�o�R�ŉ摜�Z���^�[�Ɉړ�����</summary>
    ''' <param name="distanceX"></param>
    ''' <param name="distanceY"></param>
    ''' <param name="stJOG">'V6.0.0.0-23</param>
    ''' <remarks>'V6.0.0.0�G</remarks>
    Friend Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal, ByRef stJOG As JOG_PARAM)
        stJOG.KeyDown = Keys.Execute                                    'V6.0.0.0-23
        InpKey = (InpKey Or CtrlJog.MouseClickLocation.GetInpKey(distanceX, distanceY))
    End Sub
#End Region

#Region "���{�^��������"
    '''=========================================================================
    '''<summary>���{�^��������</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SubBtnJOG_0_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22  
        'V6.0.0.0-22    Public Sub SubBtnJOG_0_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H1000US                         ' +Y ON
    End Sub
    Public Sub SubBtnJOG_0_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_0_MouseUp()
        InpKey = InpKey And Not &H1000US                    ' +Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_1_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_1_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H800US                          ' -Y ON
    End Sub
    Public Sub SubBtnJOG_1_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_1_MouseUp()
        InpKey = InpKey And Not &H800US                     ' -Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_2_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_2_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H400US                          ' +X ON
    End Sub
    Public Sub SubBtnJOG_2_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_2_MouseUp()
        InpKey = InpKey And Not &H400US                     ' +X OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_3_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_3_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H200US                          ' -X ON
    End Sub
    Public Sub SubBtnJOG_3_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_3_MouseUp()
        InpKey = InpKey And Not &H200US                     ' -X OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_4_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_4_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &HA00US                          ' -X -Y ON
    End Sub
    Public Sub SubBtnJOG_4_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_4_MouseUp()
        InpKey = InpKey And Not &HA00US                     ' -X -Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_5_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_5_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &HC00US                          ' +X -Y ON
    End Sub
    Public Sub SubBtnJOG_5_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_5_MouseUp()
        InpKey = InpKey And Not &HC00US                     ' +X -Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_6_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_6_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H1400US                         ' +X +Y ON
    End Sub
    Public Sub SubBtnJOG_6_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_6_MouseUp()
        InpKey = InpKey And Not &H1400US                    ' +X +Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_7_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_7_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H1200US                         ' -X +Y ON
    End Sub
    Public Sub SubBtnJOG_7_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_7_MouseUp()
        InpKey = InpKey And Not &H1200US                    ' -X +Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub
#End Region

    '========================================================================================
    '   �i�n�f�����ʏ����p�g���b�N�o�[����
    '========================================================================================
#Region "�g���b�N�o�[�̃X���C�_�[��ʏ����l�\��"
    '''=========================================================================
    '''<summary>�g���b�N�o�[�̃X���C�_�[��ʏ����l�\��</summary>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    '''=========================================================================
    Public Sub XyzBpMovingPitchInit(ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                                    ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                                    ByRef TBarPause As System.Windows.Forms.TrackBar, _
                                    ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                                    ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                                    ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                                    ByRef dblTchMoval() As Double)

        Dim minval As Short

        ' LOW�߯������͈͊O�Ȃ�͈͓��ɕύX����
        If (dblTchMoval(IDX_PIT) < cPT_LO) Then dblTchMoval(IDX_PIT) = cPT_LO
        If (dblTchMoval(IDX_PIT) > cPT_HI) Then dblTchMoval(IDX_PIT) = cPT_HI

        ' LOW�߯��̖ڐ���ݒ肷��
        If (dblTchMoval(IDX_PIT) < 0.002) Then                          ' ����\�ɂ��ŏ��ڐ���ݒ肷��
            minval = 1                                                  ' �ڐ�1�`
        Else
            minval = 2                                                  ' �ڐ�2�`
        End If

        TBarLowPitch.TickFrequency = dblTchMoval(IDX_PIT) * 1000        ' 0.001mm�P��
        TBarLowPitch.Maximum = 100                                      ' �ڐ�1(or 2)�`100(0.001m�`0.1mm)
        TBarLowPitch.Minimum = minval
        '###110
        TBarLowPitch.Value = dblTchMoval(IDX_PIT) * 1000        ' 0.001mm�P��

        ' HIGH�߯������͈͊O�Ȃ�͈͓��ɕύX����
        If (dblTchMoval(IDX_HPT) < cHPT_LO) Then dblTchMoval(IDX_HPT) = cHPT_LO
        If (dblTchMoval(IDX_HPT) > cHPT_HI) Then dblTchMoval(IDX_HPT) = cHPT_HI

        ' HIGH�߯��̖ڐ���ݒ肷��
        TBarHiPitch.TickFrequency = dblTchMoval(IDX_HPT) * 100          ' 0.01mm�P��
        TBarHiPitch.Maximum = 500                                       ' �ڐ�1�`100(0.01m�`5.00mm)
        TBarHiPitch.Minimum = 1
        '###110
        TBarHiPitch.Value = dblTchMoval(IDX_HPT) * 100          ' 0.01mm�P��

        ' Pause Time���͈͊O�Ȃ�͈͓��ɕύX����
        If (dblTchMoval(IDX_PAU) < cPAU_LO) Then dblTchMoval(IDX_PAU) = cPAU_LO
        If (dblTchMoval(IDX_PAU) > cPAU_HI) Then dblTchMoval(IDX_PAU) = cPAU_HI

        ' Pause Time�̖ڐ���ݒ肷��
        TBarPause.TickFrequency = dblTchMoval(IDX_PAU) * 20             ' 0.5�b�P��
        TBarPause.Maximum = 20                                          ' �ڐ�1�`20(0.05�b�`1.00�b)
        TBarPause.Minimum = 1
        '###110
        TBarPause.Value = dblTchMoval(IDX_PAU) * 20             ' 0.5�b�P��

        ' �ړ��s�b�`��\������
        LblTchMoval0.Text = dblTchMoval(IDX_PIT).ToString("0.0000")
        LblTchMoval1.Text = dblTchMoval(IDX_HPT).ToString("0.0000")
        LblTchMoval2.Text = dblTchMoval(IDX_PAU).ToString("0.0000")

    End Sub
#End Region

#Region "�g���b�N�o�[�̃X���C�_�[�ړ�����"
    '''=========================================================================
    '''<summary>�g���b�N�o�[�̃X���C�_�[�ړ�����</summary>
    '''<param name="Index">       (INP)0=LOW�߯�, 1=HIGH�߯�, 2=Pause</param>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    '''=========================================================================
    Public Sub SetSliderPitch(ByRef Index As Short, _
                              ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                              ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                              ByRef TBarPause As System.Windows.Forms.TrackBar, _
                              ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                              ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                              ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                              ByRef dblTchMoval() As Double)

        Dim lVal As Integer

        ' BP�̈ړ��s�b�`����ݒ肷��
        Select Case Index
            Case IDX_PIT    ' LOW�߯�
                lVal = TBarLowPitch.Value                       ' �ײ�ޖڐ��l�擾
                dblTchMoval(Index) = 0.001 * lVal               ' LOW�߯��l�ύX
                LblTchMoval0.Text = dblTchMoval(Index).ToString("0.0000")
                LblTchMoval0.Refresh()

            Case IDX_HPT    ' HIGH�߯�
                lVal = TBarHiPitch.Value                        ' �ײ�ޖڐ��l�擾
                dblTchMoval(Index) = 0.01 * lVal                ' HIGH�߯��l�ύX
                LblTchMoval1.Text = dblTchMoval(Index).ToString("0.0000")
                LblTchMoval1.Refresh()

            Case IDX_PAU    ' Pause Time
                lVal = TBarPause.Value                          ' �ײ�ޖڐ��l�擾
                dblTchMoval(Index) = 0.05 * lVal                ' �ړ��s�b�`�Ԃ̃|�[�Y�l�ύX
                LblTchMoval2.Text = dblTchMoval(Index).ToString("0.0000")
                LblTchMoval2.Refresh()
        End Select

    End Sub
#End Region

    '========================================================================================
    '   �i�n�f�����ʏ����p�e���L�[���͏���
    '========================================================================================
#Region "�e���L�[�_�E���T�u���[�`��"
    '''=========================================================================
    '''<summary>�e���L�[�_�E���T�u���[�`��</summary>
    ''' <param name="KeyCode">(INP)�L�[�R�[�h</param>
    '''=========================================================================
    Public Sub Sub_10KeyDown(ByVal KeyCode As Keys, ByRef stJOG As JOG_PARAM)  'V6.0.0.0�K   'V6.0.0.0-22
        'V6.0.0.0�J    Public Sub Sub_10KeyDown(ByVal KeyCode As Short)

        Dim strMSG As String
        Try
            With stJOG
                If (Keys.None <> .KeyDown) Then
                    Exit Sub 'V6.0.0.0-22
                Else
                    .KeyDown = KeyCode
                End If

                ' Num Lock��
                Select Case (KeyCode)
                    Case System.Windows.Forms.Keys.NumPad2                      ' ��  (KeyCode =  98(&H62)
                        .TenKey(0).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &H1000                               ' +Y ON(��)
                    Case System.Windows.Forms.Keys.NumPad8                      ' ��  (KeyCode = 104(&H68)
                        .TenKey(1).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &H800                                ' -Y ON(��)
                    Case System.Windows.Forms.Keys.NumPad4                      ' ��  (KeyCode = 100(&H64)
                        .TenKey(2).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &H400                                ' +X ON(��)
                    Case System.Windows.Forms.Keys.NumPad6                      ' ��  (KeyCode = 102(&H66)
                        .TenKey(3).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &H200                                ' -X ON(��)
                    Case System.Windows.Forms.Keys.NumPad9                      ' PgUp(KeyCode = 105(&H69)
                        .TenKey(4).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &HA00                                ' -X -Y ON
                    Case System.Windows.Forms.Keys.NumPad7                      ' Home(KeyCode = 103(&H67))
                        .TenKey(5).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &HC00                                ' +X -Y ON
                    Case System.Windows.Forms.Keys.NumPad1                      ' End(KeyCode =   97(&H61)
                        .TenKey(6).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &H1400                               ' +X +Y ON
                    Case System.Windows.Forms.Keys.NumPad3                      ' PgDn(KeyCode =  99(&H63)
                        .TenKey(7).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        InpKey = InpKey Or &H1200                               ' -X +Y ON
                    Case System.Windows.Forms.Keys.NumPad5                      ' 5�� (KeyCode = 101(&H65)
                        .TenKey(8).FlatStyle = FlatStyle.Flat               'V6.0.0.0�K
                        'Call BtnHI_Click(sender, e)                             ' HI�{�^�� ON/OFF
                End Select
            End With

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.Sub_10KeyDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�e���L�[�A�b�v�T�u���[�`��"
    '''=========================================================================
    '''<summary>�e���L�[�A�b�v�T�u���[�`��</summary>
    ''' <param name="KeyCode">(INP)�L�[�R�[�h</param>
    '''=========================================================================
    Public Sub Sub_10KeyUp(ByVal KeyCode As Keys, ByRef stJOG As JOG_PARAM)    'V6.0.0.0�K   'V6.0.0.0-22
        'V6.0.0.0�J    Public Sub Sub_10KeyUp(ByVal KeyCode As Short)

        Dim strMSG As String

        Try
            With stJOG
                'V6.0.0.0-23                If (KeyCode <> .KeyDown) Then
                'V6.0.0.0-23                    Exit Sub 'V6.0.0.0-22
                'V6.0.0.0-23                End If

                ' Num Lock��
                Select Case (KeyCode)
                    Case System.Windows.Forms.Keys.NumPad2                      ' ��  (KeyCode =  98(&H62)
                        .TenKey(0).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &H1000                          ' +Y OFF
                    Case System.Windows.Forms.Keys.NumPad8                      ' ��  (KeyCode = 104(&H68)
                        .TenKey(1).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &H800                           ' -Y OFF
                    Case System.Windows.Forms.Keys.NumPad4                      ' ��  (KeyCode = 100(&H64)
                        .TenKey(2).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &H400                           ' +X OFF
                    Case System.Windows.Forms.Keys.NumPad6                      ' ��  (KeyCode = 102(&H66)
                        .TenKey(3).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &H200                           ' -X OFF
                    Case System.Windows.Forms.Keys.NumPad9                      ' PgUp(KeyCode = 105(&H69)
                        .TenKey(4).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &HA00                           ' -X -Y OFF
                    Case System.Windows.Forms.Keys.NumPad7                      ' Home(KeyCode = 103(&H67))
                        .TenKey(5).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &HC00                           ' +X -Y OFF
                    Case System.Windows.Forms.Keys.NumPad1                      ' End(KeyCode =   97(&H61)
                        .TenKey(6).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &H1400                          ' +X +Y OFF
                    Case System.Windows.Forms.Keys.NumPad3                      ' PgDn(KeyCode =  99(&H63)
                        .TenKey(7).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        InpKey = InpKey And Not &H1200                          ' -X +Y OFF
                    Case System.Windows.Forms.Keys.NumPad5                      ' 5�� (KeyCode = 101(&H65)
                        .TenKey(8).FlatStyle = FlatStyle.Standard               'V6.0.0.0�K
                        'V6.0.1.0�B      ��
                    Case Keys.None
                        InpKey = (InpKey And cBIT_HI)
                        .ResetButtonStyle()
                        'V6.0.1.0�B      ��
                End Select

                .TenKey(0).Parent.Select()  'V6.0.0.0�K
                .KeyDown = Keys.None        'V6.0.0.0-22

            End With
            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.Sub_10KeyUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '===========================================================================
    '   �O���[�o�����\�b�h��`
    '===========================================================================
#Region "�@�B�n�̃p�����[�^�ݒ�"
    '''=========================================================================
    '''<summary>�@�B�n�̃p�����[�^�ݒ�</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetMechanicalParam()

        Dim BpSoftLimitX As Integer
        Dim BpSoftLimitY As Integer

        With gSysPrm.stDEV
            ' ���ʑΉ�
            If gSysPrm.stCTM.giSPECIAL = customASAHI And typPlateInfo.strDataName = "2" Then
                .gfTrimX = .gfTrimX2                            ' TRIM POSITION X(mm)
                .gfTrimY = .gfTrimY2                            ' TRIM POSITION Y(mm)
                .gfExCmX = .gfExCmX2                            ' Externla Camera Offset X(mm)
                .gfExCmY = .gfExCmY2                            ' Externla Camera Offset Y(mm)
                .gfRot_X1 = .gfRot_X2                           ' ��]���S X
                .gfRot_Y1 = .gfRot_Y2                           ' ��]���S Y
                '(2010/11/16)���L�����͕s�v
                'Else
                '    gSysPrm.stDEV.gfTrimX = gSysPrm.stDEV.gfTrimX   ' TRIM POSITION X(mm)
                '    gSysPrm.stDEV.gfTrimY = gSysPrm.stDEV.gfTrimY   ' TRIM POSITION Y(mm)
                '    gSysPrm.stDEV.gfExCmX = gSysPrm.stDEV.gfExCmX   ' Externla Camera Offset X(mm)
                '    gSysPrm.stDEV.gfExCmY = gSysPrm.stDEV.gfExCmY   ' Externla Camera Offset Y(mm)
                '    gSysPrm.stDEV.gfRot_X1 = gSysPrm.stDEV.gfRot_X1 ' ��]���S X
                '    gSysPrm.stDEV.gfRot_Y1 = gSysPrm.stDEV.gfRot_Y1 ' ��]���S Y
            End If
            ''''(2010/11/16) ����m�F�㉺�L�R�����g�͍폜
            'gStartX = gSysPrm.stDEV.gfTrimX
            'gStartY = gSysPrm.stDEV.gfTrimY

            'BpSize����BP�̃\�t�g���~�b�g�iBP�̃\�t�g�ғ��͈́j��ݒ�
            Select Case (.giBpSize)
                Case 0
                    BpSoftLimitX = 50
                    BpSoftLimitY = 50
                Case 1
                    BpSoftLimitX = 80
                    BpSoftLimitY = 80
                Case 2
                    BpSoftLimitX = 100
                    BpSoftLimitY = 60
                Case 3
                    BpSoftLimitX = 60
                    BpSoftLimitY = 100
                Case BPSIZE_6060 'V1.24.0.0�@
                    'BpSoftLimitX = 60
                    'BpSoftLimitY = 60
                    BpSoftLimitX = 67
                    BpSoftLimitY = 67
                Case BPSIZE_90                  'V4.4.0.0�@
                    BpSoftLimitX = BPSIZE_90
                    BpSoftLimitY = BPSIZE_90
                Case Else
                    BpSoftLimitX = 80
                    BpSoftLimitY = 80
            End Select

            '''''2009/07/23 minato
            ''''    �g�����|�W�V�������ύX����Ă��邽�߁A
            ''''    INTRTM���̃V�X�e���p�����[�^���X�V����K�v������B
            Call ZSYSPARAM2(.giPrbTyp, .gfSminMaxZ2, .giZPTimeOn, .giZPTimeOff, _
                        .giXYtbl, .gfSmaxX, .gfSmaxY, gSysPrm.stIOC.glAbsTime, _
                        .gfTrimX, .gfTrimY, BpSoftLimitX, BpSoftLimitY)

            '----- V1.23.0.0�J�� -----
            ' ��񌴓_�ʒu���Đݒ肷��
            Call ZSETPOS2(gSysPrm.stDEV.gfPos2X, gSysPrm.stDEV.gfPos2Y, gSysPrm.stDEV.gfPos2Z)
            '----- V1.23.0.0�J�� -----
        End With
    End Sub
#End Region

#Region "U��Ď��s���ʎ擾"
    '''=========================================================================
    '''<summary>U��Ď��s���ʎ擾</summary>
    '''<param name="rn">(INP) ��R�ԍ�</param>
    '''<param name="s"> (OUT) ���s����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function RetrieveUCutResult(ByVal rn As Short, ByRef s As String) As Short

        Dim cn As Short
        Dim n As Short
        Dim f As Double
        Dim r As Integer

        s = ""
        RetrieveUCutResult = 0

        If gSysPrm.stSPF.giUCutKind = 0 Then
            Exit Function
        End If

        On Error GoTo ErrTrap

        For cn = 1 To typResistorInfoArray(rn).intCutCount
            s = typResistorInfoArray(rn).ArrCut(cn).strCutType          ' Cut pattern
            If (s = "H") Or (s = "I") Then                              ' UCUT or UCUT(���g���[�X) V1.22.0.0�@
                s = ""
                '  U��Ď��s���ʎ擾
                r = UCUT_RESULT(rn, cn, n, f)
                If (r <> 0) Then
                    MsgBox("Internal error  X001-" & Str(r), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                    RetrieveUCutResult = 1
                    Exit Function
                End If

                If n = 255 Then                                         ' 255 ��UCUT���s���Ă��Ȃ��ꍇ
                    's = Form1.Utility1.sFormat(f, "0.000000", 10 + 7) & " n** "    ' V1.22.0.0�@
                    's = " n** " + Form1.Utility1.sFormat(f, "0.000000", 10 + 7)     ' V1.22.0.0�@
                    s = " n** " + f.ToString("0.000000").PadLeft(10 + 7)     ' V4.0.0.0�G
                ElseIf n >= 0 And n <= 19 Then                          ' ���펞(n=0-19) 
                    n = n + 1
                    ' ���������l(0.000000) + U�J�b�g�e�[�u���ԍ�
                    's = Form1.Utility1.sFormat(f, "0.000000", 10 + 7) & " " & "n" & n.ToString("00") & " " ' V1.22.0.0�@
                    's = "n" & n.ToString("00") & " " + Form1.Utility1.sFormat(f, "0.000000", 10 + 7)        ' V1.22.0.0�@
                    s = "n" & n.ToString("00") & " " + f.ToString("0.000000").PadLeft(10 + 7)   ' V4.0.0.0�G
                ElseIf n = 254 Then                                     ' �p�����[�^�e�[�u���ɊY�������R�ԍ������������ꍇ
                    's = Form1.Utility1.sFormat(f, "0.000000", 10 + 7) & " n** "    ' V1.22.0.0�@
                    's = " n** " + Form1.Utility1.sFormat(f, "0.000000", 10 + 7)     ' V1.22.0.0�@
                    s = " n** " + f.ToString("0.000000").PadLeft(10 + 7)    ' V4.0.0.0�G
                Else                                                    ' �ςȒl
                    RetrieveUCutResult = 2
                    Exit Function
                End If
            Else
                s = ""
            End If
        Next

        Exit Function

ErrTrap:
        Resume ErrTrap1
ErrTrap1:
        Dim er As Integer
        er = Err.Number
        On Error GoTo 0
        MsgBox("Internal error X002-" & Str(er), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
    End Function
#End Region

#Region "�ް���R�ԍ����ڼ��ް���R�ԍ����擾����"
    '''=========================================================================
    '''<summary>�ް���R�ԍ����ڼ��ް���R�ԍ����擾����</summary>
    '''<param name="br">(INP) ��R�ԍ�</param>
    '''<returns>0�ȏ�=ڼ��ް���R�ԍ�, -1=�Ȃ�</returns>
    '''=========================================================================
    Public Function GetRatio1BaseNum(ByVal br As Short) As Short

        Dim n As Short

        For n = 1 To gRegistorCnt
            ' �x�[�X��R ?
            If typResistorInfoArray(n).intResNo = br Then
                GetRatio1BaseNum = n
                Exit Function
            End If
        Next
        GetRatio1BaseNum = -1

    End Function
#End Region

#Region "�O���[�v��,�u���b�N��,�`�b�v��(��R��),�`�b�v�T�C�Y���擾����(�s�w/�s�x�e�B�[�`���O�p)"
    '''=========================================================================
    ''' <summary>�O���[�v��,�u���b�N��,�`�b�v��(��R��),�`�b�v�T�C�Y���擾����</summary>
    ''' <param name="AppMode">  (INP)���[�h</param>
    ''' <param name="Gn">       (OUT)�O���[�v��</param>
    ''' <param name="RnBn">     (OUT)�`�b�v��(�s�w�e�B�[�`���O��)�܂���
    '''                              �u���b�N��(�s�x�e�B�[�`���O��)</param>
    ''' <param name="DblChipSz">(OUT)�`�b�v�T�C�Y</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function GetChipNumAndSize(ByVal AppMode As Short, ByRef Gn As Short, ByRef RnBn As Short, ByRef DblChipSz As Double) As Short

        Dim ChipNum As Short                                        ' �`�b�v��(��R��)
        Dim ChipSzX As Double                                       ' �`�b�v�T�C�YX
        Dim ChipSzY As Double                                       ' �`�b�v�T�C�YY
        Dim strMSG As String

        Try
            ' �O����(CHIP/NET����)
            ChipNum = typPlateInfo.intResistCntInGroup              ' �`�b�v��(��R��) = 1�O���[�v��(1�T�[�L�b�g��)��R��
            ChipSzX = typPlateInfo.dblChipSizeXDir                  ' �`�b�v�T�C�YX,Y
            ChipSzY = typPlateInfo.dblChipSizeYDir

            ' �v���[�g�f�[�^����O���[�v��, �u���b�N��, �`�b�v��(��R��), �`�b�v�T�C�Y���擾����
            If (AppMode = APP_MODE_TX) Then
                '----- �s�w�e�B�[�`���O�� -----
                ' �`�b�v��(��R��)��Ԃ�
                RnBn = ChipNum                                      ' 1�O���[�v��(1�T�[�L�b�g��)��R�����Z�b�g
                ' �O���[�v����Ԃ�
                Gn = typPlateInfo.intGroupCntInBlockXBp             ' �a�o�O���[�v��(�T�[�L�b�g��)���Z�b�g
                ' �`�b�v�T�C�Y��Ԃ�
                If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���т�X���� ?
                    DblChipSz = System.Math.Abs(ChipSzX)
                Else
                    DblChipSz = System.Math.Abs(ChipSzY)
                End If

            Else
                '----- �s�x�e�B�[�`���O�� -----
                ' �O���[�v����Ԃ�
                Gn = typPlateInfo.intGroupCntInBlockYStage          ' �u���b�N��Stage�O���[�v�����Z�b�g
                ' �u���b�N���ƃ`�b�v�T�C�Y��Ԃ�
                If (typPlateInfo.intResistDir = 0) Then             ' �`�b�v���т�X���� ?
                    RnBn = typPlateInfo.intBlockCntYDir             ' �u���b�N��Y���Z�b�g
                    DblChipSz = System.Math.Abs(ChipSzY)            ' �`�b�v�T�C�YY���Z�b�g
                Else
                    RnBn = typPlateInfo.intBlockCntXDir             ' �u���b�N��X���Z�b�g
                    DblChipSz = System.Math.Abs(ChipSzX)            ' �`�b�v�T�C�YX���Z�b�g
                End If
            End If

            strMSG = "GetChipNumAndSize() Gn=" + Gn.ToString("0") + ", RnBn=" + RnBn.ToString("0") + ", ChipSZ=" + DblChipSz.ToString("0.00000")
            Console.WriteLine(strMSG)
            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.GetChipNumAndSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- V1.19.0.0-33�� -----
#Region "�L���Ȓ�R�����邩�ǂ����`�F�b�N����"
    '''=========================================================================
    ''' <summary>�L���Ȓ�R�����擾����</summary>
    ''' <param name="AppMode">  (INP)���[�h</param>
    ''' <returns>cFRS_NORMAL=����, cFRS_ERR_RST=�L���Ȓ�R���Ȃ�</returns>
    '''=========================================================================
    Public Function CheckValidRegistance(ByVal AppMode As Short) As Short

        Dim Rn As Short                                                 ' ��R��
        Dim strMSG As String

        Try
            ' �L���Ȓ�R�����邩�ǂ����`�F�b�N����
            For Rn = 1 To gRegistorCnt                                  ' ��R������������
                If (AppMode = APP_MODE_PROBE) Then                      ' �v���[�u�R�}���h ?
                    If (typResistorInfoArray(Rn).intResNo < 1000) Then  ' ��R�ԍ�(1�`999)�������
                        Return (cFRS_NORMAL)                            ' Return�l = ����
                    End If
                Else                                                    ' �e�B�[�`���O�R�}���h��
                    If (typResistorInfoArray(Rn).intResNo < 6000) Or _
                       ((typResistorInfoArray(Rn).intResNo >= 8000) And (typResistorInfoArray(Rn).intResNo <= 9999)) Then
                        Return (cFRS_NORMAL)                            ' Return�l = ����
                    End If

                End If
            Next Rn

            Return (cFRS_ERR_RST)                                       ' Return�l = Cancel(�L���Ȓ�R���Ȃ�)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.CheckValidRegistance() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- V1.19.0.0-33�� -----
#Region "�������V�I���̃x�[�X��R�ԍ�(�z��̓Y��)��Ԃ�"
    '''=========================================================================
    '''<summary>�������V�I���̃x�[�X��R�ԍ�(�z��̓Y��)��Ԃ�</summary>
    '''<param name="rr">(INP) ��R�ԍ�</param> 
    '''<param name="br">(OUT) �x�[�X��R�ԍ�(�z��̓Y��)</param>  
    '''<remarks>�����a�����������V�I�@�\(TKY���ڐA)</remarks>
    '''=========================================================================
    Public Sub GetRatio3Br(ByRef rr As Short, ByRef br As Short)

        Dim i As Short
        Dim wRn As Short
        Dim wGn As Short
        Dim wBr As Short
        Dim wBr2 As Short

        ' �������V�I���[�h(3�`9)�łȂ���Βʏ�̃x�[�X��R�ԍ���Ԃ�
        wRn = typResistorInfoArray(rr).intResNo                         ' ��R�ԍ�
        wGn = typResistorInfoArray(rr).intTargetValType                 ' �ڕW�l��ʁi0:��Βl, 1:���V�I�A2�F�v�Z��, 3�`9:��ٰ�ߔԍ��j
        wBr = GetRatio1BaseNum(typResistorInfoArray(rr).intBaseResNo)   ' �x�[�X��R�ԍ�(�Y��)
        wBr2 = -1
        If (wGn < 3) Or (wGn > 9) Then                                  ' �������V�I���[�h(3�`9)�łȂ� ? 
            GoTo STP_END
        End If

        ' �������V�I�Ȃ瑊���ٰ�ߔԍ�����������
        For i = 1 To gRegistorCnt                                       ' ��R�����J��Ԃ�
            If (wRn <> typResistorInfoArray(i).intResNo) Then           ' ��R�ԍ�=�������g��SKIP
                If (wGn = typResistorInfoArray(i).intTargetValType) Then            ' �����ٰ�ߔԍ� ?
                    wBr2 = GetRatio1BaseNum(typResistorInfoArray(i).intBaseResNo)   ' �x�[�X��R�ԍ�(�Y��)
                    Exit For
                End If
            End If
        Next i

        ' �x�[�X��R��FT�l�̑傫�������x�[�X��R�ԍ��Ƃ���
        If (wBr2 < 0) Then GoTo STP_END '                               ' �����ٰ�ߔԍ���������Ȃ����� ?
        If (gfFinalTest(wBr2) > gfFinalTest(wBr)) Then                  ' �����FT�l���傫�� ?
            wBr = wBr2
        End If

STP_END:
        'br = wBr                                                       ' �x�[�X��R�ԍ���Ԃ�
        br = wBr - 1                                                    ' �x�[�X��R�ԍ���Ԃ� ###244

    End Sub
#End Region

#Region "���V�I(�v�Z��)���̃x�[�X��R�ԍ�(�z��̓Y��)��Ԃ�"
    '''=========================================================================
    '''<summary>���V�I(�v�Z��)���̃x�[�X��R�ԍ������R�f�[�^�̔z��̓Y����Ԃ�###123</summary>
    '''<param name="br">(INP)�x�[�X��R�ԍ�(�z��̓Y��)</param> 
    '''<param name="rr">(OUT)��R�f�[�^�̔z��̓Y��(1 ORG)</param> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetRatio2Rn(ByVal br As Short, ByRef rr As Short)

        Dim Rn As Short

        ' �x�[�X��R�ԍ�����������
        For Rn = 1 To gRegistorCnt                                      ' ��R�����J��Ԃ�
            If (typResistorInfoArray(Rn).intBaseResNo = br) Then
                rr = Rn
                Exit Sub
            End If
        Next Rn

    End Sub
#End Region

#Region "Z/Z2�ړ�(ON/OFF) "
    '''=========================================================================
    '''<summary>Z/Z2�ړ�(ON/OFF) </summary>
    '''<param name="MD">  (INP)Ӱ��(0 = OFF�ʒu�ړ�, 1 = ON�ʒu�ړ�)</param> 
    '''<param name="Z2ON">(INP)Z2 ON�ʒu(OPTION)</param>  
    '''<remarks>0=����, 0�ȊO=�G���[</remarks>
    '''=========================================================================
    Public Function Sub_Probe_OnOff(ByVal MD As Integer, Optional ByVal Z2ON As Double = 0.0#) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �y�v���[�u���I���ʒu�ֈړ�
            Sub_Probe_OnOff = cFRS_NORMAL                       ' Return�l = ����
            If (MD = 1) Then                                    ' ON ?
                r = Form1.System1.EX_PROBON(gSysPrm)                   ' Z ON�ʒu�ֈړ�
                If (r <> cFRS_NORMAL) Then                      ' �װ ?
                    Sub_Probe_OnOff = r                         ' Return�l = ����~��(��ү���ނ͕\����)
                    Exit Function
                End If
                If ((gSysPrm.stDEV.giPrbTyp And 2) = 2) Then    ' ������۰�ނȂ��Ȃ�NOP
                    r = Form1.System1.EX_PROBON2(gSysPrm, Z2ON)        ' Z2 ON�ʒu�ֈړ�
                    If (r <> cFRS_NORMAL) Then                  ' �װ ?
                        Sub_Probe_OnOff = r                     ' Return�l = ����~��(��ү���ނ͕\����)
                        Exit Function
                    End If
                End If

                ' �y�v���[�u���I�t�ʒu�ֈړ�
            Else
                If ((gSysPrm.stDEV.giPrbTyp And 2) = 2) Then    ' ������۰�ނȂ��Ȃ�NOP
                    r = Form1.System1.EX_PROBOFF2(gSysPrm)             ' Z2 OFF�ʒu�ֈړ�
                    If (r <> cFRS_NORMAL) Then                  ' �װ ?
                        Sub_Probe_OnOff = r                     ' Return�l = ����~��(��ү���ނ͕\����)
                        Exit Function
                    End If
                End If
                r = Form1.System1.EX_PROBOFF(gSysPrm)                  ' Z OFF�ʒu�ֈړ�
                If (r <> cFRS_NORMAL) Then                      ' �װ ?
                    Sub_Probe_OnOff = r                         ' Return�l = ����~��(��ү���ނ͕\����)
                    Exit Function
                End If
            End If
            Exit Function

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.Sub_Probe_OnOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���H��������͂���(FL��)"
    '''=========================================================================
    '''<summary>���H��������͂���(FL��)</summary>
    ''' <param name="CondNum">(I/O)���H�����ԍ�</param>
    ''' <param name="dQrate"> (I/O)Q���[�g(KHz)</param>
    ''' <param name="Owner">  (INP)�I�[�i�[</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>�L�����u���[�V�����A�J�b�g�ʒu�␳(�O���J����)�̏\���J�b�g�p</remarks>
    '''=========================================================================
    Public Function Sub_FlCond(ByRef CondNum As Integer, ByRef dQrate As Double, ByVal Owner As IWin32Window) As Integer

        Dim r As Integer
        'V6.0.0.0�Q        Dim ObjForm As Object = Nothing
        Dim strMSG As String

        Try
            ' ���H��������͂���(FL��)
            r = cFRS_NORMAL                                             ' Return�l = ����
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then          ' FL�łȂ���
                CondNum = 0                                             ' ���H�����ԍ�(Dmy)

            Else                                                        ' FL���͉��H��������͂���
                ' ���H�������͉�ʕ\��
                'V6.0.0.0�Q                ObjForm = New FrmFlCond()                               ' �I�u�W�F�N�g����
                Dim ObjForm As FrmFlCond = New FrmFlCond()              ' �I�u�W�F�N�g����  'V6.0.0.0�Q
                Call ObjForm.ShowDialog(Owner, CondNum)                 ' ���H�������͉�ʕ\��
                r = ObjForm.GetResult(CondNum, dQrate)                  ' ���H�����擾

                ' �I�u�W�F�N�g�J��
                If (ObjForm Is Nothing = False) Then
                    Call ObjForm.Close()                                ' �I�u�W�F�N�g�J��
                    Call ObjForm.Dispose()                              ' ���\�[�X�J��
                End If
            End If

            Return (r)                                                  ' Return�l�ݒ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.Sub_FlCond() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region
    '----- V4.0.0.0-58�� -----
#Region "���H�����ԍ���ݒ肵�Ȃ���(SL436S��)"
    '''=========================================================================
    ''' <summary>���H�����ԍ���ݒ肵�Ȃ���(SL436S��)</summary>
    ''' <remarks></remarks>
    ''' <returns>cFRS_NORMAL  = ����
    '''          ��L�ȊO �@�@= �G���[</returns> 
    '''=========================================================================
    Public Function SetCndNum() As Integer

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   ��������
            '------------------------------------------------------------------
            ' FL�łȂ��܂���SL436S�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)
            Call Init_wkCND()                                           ' ���H�����\����(���[�N�p)������������

            '------------------------------------------------------------------
            '   �J�b�g�f�[�^�̉��H�����ԍ���ݒ肵�Ȃ���
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' �P�u���b�N����R�����`�F�b�N���� 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' ��R���J�b�g�����`�F�b�N����

                    ' �J�b�g�^�C�v�擾
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

                    ' ���H����1�����H�����\����(���[�N�p)�ɓo�^���ĉ��H�����ԍ����擾����
                    CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1), _
                                       typResistorInfoArray(Rn).ArrCut(Cn).dblQRate, _
                                       typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1), _
                                       typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1))
                    If (CndNum >= 0) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1) = CndNum
                    End If

                    ' ���H����2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
                    ' HOOK�J�b�g, U�J�b�g���ɐݒ肷��
                    If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                       (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                       (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                        ' ���H����2
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2) = CndNum
                    End If

                    ' ���H����3��HOOK�J�b�g, U�J�b�g���ɐݒ肷��(���Ή�)
                    If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                        ' ���H����3
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L3), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L3), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L3))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3) = CndNum
                    End If

                    ' ���H����4�͌���͖��g�p(�\��)

                    ' ���H����5�`8�̓��^�[��/���g���[�X�p 
                    ' ���H����5(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)��
                    If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                       (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                        ' ���H����5
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum
                    End If

                    ' ���H����5,6(L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)��
                    If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                        ' ���H����5
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum

                        ' ���H����6
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2_RET), _
                                 typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4, _
                                 typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2_RET), _
                                 typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2_RET))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET) = CndNum
                    End If

                    ' ���H����7,8�͌���͖��g�p(�\��)

                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return�l�ݒ� 

STP_ERR:
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    strMsg = "���H�����̓o�^�������ő匏���𒴂��܂����B�d���l�A���g���ASTEG�{�����m�F���ĉ������B"
            'Else
            '    strMsg = "The Registration Number Of Conditions Exceeded The Maximum. Please Check A Current Value, Frequency, And A Number Of STEG."
            'End If
            strMsg = Globals_Renamed_001
            MsgBox(strMsg)
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "basTrimming.SetCndNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try
    End Function
#End Region

#Region "���H�����\����(���[�N�p)������������(SL436S�p)"
    '''=========================================================================
    '''<summary>���H�����\����(���[�N�p)������������(SL436S�p)</summary>
    '''=========================================================================
    Public Sub Init_wkCND()

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' FL�łȂ��܂���SL436S�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return
            If (giMachineKd <> MACHINE_KD_RS) Then Return

            ' ���H�����ԍ����o�^�ς݂��`�F�b�N����
            gwkCndCount = 0                                             ' ���H�����o�^�� 
            For Idx = 0 To (MAX_BANK_NUM - 1)
                gwkCND.Curr(Idx) = 0
                gwkCND.Freq(Idx) = 0
                gwkCND.Steg(Idx) = 0
                gwkPower(Idx) = 0
            Next Idx

            Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.Init_wkCND() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���H���������H�����\����(���[�N�p)�ɓo�^����(SL436S�p)"
    '''=========================================================================
    '''<summary>���H���������H�����\����(���[�N�p)�ɓo�^����(SL436S�p)</summary>
    ''' <param name="Curr"> (INP)�d���l(mA)</param>
    ''' <param name="Freq"> (INP)���g��(KHz)</param>
    ''' <param name="Steg"> (INP)STEG�{��</param>
    ''' <param name="Power">(INP)�ڕW�p���[</param>
    ''' <returns>0-29=���H�����ԍ�, ���L�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Add_stCND(ByVal Curr As Integer, ByVal Freq As Double, ByVal Steg As Integer, ByVal Power As Double) As Integer

        Dim Idx As Integer
        Dim Count As Integer
        Dim strMSG As String

        Try
            ' FL�łȂ��܂���SL436S�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            ' ���H�����ԍ����o�^�ς݂��`�F�b�N����
            Count = gwkCndCount                                         ' ���H�����o�^�� 
            For Idx = 0 To (Count - 1)
                'V4.9.0.1�A��
                If typPlateInfo.intPowerAdjustMode = 0 Then     ' �p���[����Ȃ��̏ꍇ
                    ' �d���l, ���g��, STEG�{�������������H�����͓o�^�ς݂� ?
                    If ((gwkCND.Curr(Idx) = Curr) And _
                        (gwkCND.Freq(Idx) = Freq) And _
                        (gwkCND.Steg(Idx) = Steg)) Then
                        Return (Idx)                                        ' �o�^�ς݉��H�����ԍ���Ԃ�
                    End If
                Else
                    ' �p���[���肠��̏ꍇ
                    ' ���g��, STEG�{��,�ڕW�p���[�����������H�����͓o�^�ς݂� ?
                    If ((gwkCND.Freq(Idx) = Freq) And _
                        (gwkCND.Steg(Idx) = Steg) And _
                        (gwkPower(Idx) = Power)) Then
                        Return (Idx)                                        ' �o�^�ς݉��H�����ԍ���Ԃ�
                    End If
                End If
                'V4.9.0.1�A��
            Next Idx

            ' ���H�����o�^�\���`�F�b�N����(���H�����ԍ�30,31�͗\��ς�)
            If ((gwkCndCount + 1) > (MAX_BANK_NUM - 2)) Then
                Return (-1)
            End If

            ' ���H������o�^����
            Idx = gwkCndCount
            gwkCND.Curr(Idx) = Curr                                     ' �d���l(mA)
            gwkCND.Freq(Idx) = Freq                                     ' ���g��(KHz)
            gwkCND.Steg(Idx) = Steg                                     ' STEG�{��
            gwkPower(Idx) = Power                                       ' �ڕW�p���[

            gwkCndCount = Idx + 1                                       ' ���H�����o�^���X�V
            Return (Idx)                                                ' �o�^�������H�����ԍ���Ԃ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.Add_stCND() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "Q���[�g,�d���l,STEG�{��������H�����ԍ����J�b�g�f�[�^�ɐݒ肷��(SL436S��)"
    '''=========================================================================
    ''' <summary>Q���[�g,�d���l,STEG�{��������H�����ԍ����J�b�g�f�[�^�ɐݒ肷��</summary>
    ''' <param name="Rn">(INP)��R�ԍ�</param>
    ''' <param name="Cn">(INP)�J�b�g�ԍ�</param>
    ''' <remarks></remarks>
    ''' <returns>cFRS_NORMAL  = ����
    '''          ��L�ȊO �@�@= �G���[</returns> 
    '''=========================================================================
    Private Function Put_CutCndNum(ByVal Rn As Integer, ByVal Cn As Integer) As Integer

        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   ��������
            '------------------------------------------------------------------
            ' FL�łȂ��܂���SL436S�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            '------------------------------------------------------------------
            '   �g���~���O�f�[�^�̉��H�����ԍ���ݒ肵�Ȃ���
            '------------------------------------------------------------------
            ' �J�b�g�^�C�v�擾
            strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

            ' ���H����1�͑S�J�b�g�������ɐݒ肷��
            CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1), _
                                         typResistorInfoArray(Rn).ArrCut(Cn).dblQRate, _
                                         typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1), _
                                         typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1))
            If (CndNum >= 0) Then
                typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1) = CndNum
            End If

            ' ���H����2��L�J�b�g, �΂�L�J�b�g, L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)
            ' HOOK�J�b�g, U�J�b�g���ɐݒ肷��
            If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
               (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
               (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
               (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                ' ���H����2
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2) = CndNum
                End If
            End If

            ' ���H����3��HOOK�J�b�g, U�J�b�g���ɐݒ肷��(���Ή�)
            If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                ' ���H����3
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L3), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L3), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L3))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3) = CndNum
                End If
            End If

            ' ���H����4�͌���͖��g�p(�\��)

            ' ���H����5�`8�̓��^�[��/���g���[�X�p 
            ' ���H����5(ST�J�b�g(����/��ڰ�), �΂�ST�J�b�g(����/��ڰ�)��
            If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
               (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                ' ���H����5
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum
                End If
            End If

            ' ���H����5,6(L�J�b�g(����/��ڰ�), �΂�L�J�b�g(����/��ڰ�)��
            If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
               (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                ' ���H����5
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum
                End If

                ' ���H����6
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2_RET))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET) = CndNum
                End If
            End If

            ' ���H����7,8�͌���͖��g�p(�\��)

            Return (cFRS_NORMAL)                                        ' Return�l�ݒ� 

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMsg = "globals.SetAutoPowerCurrData() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return�l = �g���b�v�G���[����
        End Try
    End Function
#End Region

#Region "Q���[�g,�d���l,STEG�{��������H�����ԍ����擾����(SL436S�p)"
    '''=========================================================================
    '''<summary>Q���[�g,�d���l,STEG�{��������H�����ԍ����擾����(SL436S�p)</summary>
    ''' <param name="Curr"> (INP)�d���l(mA)</param>
    ''' <param name="Freq"> (INP)���g��(KHz)</param>
    ''' <param name="Steg"> (INP)STEG�{��</param>
    ''' <param name="Power">(INP)�ڕW�p���[</param>
    ''' <returns>0-29=���H�����ԍ�(�����H�����ԍ�30��31�͗\��ς�), ���L�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Get_CndNumFromWkCND(ByVal Curr As Integer, ByVal Freq As Double, ByVal Steg As Integer, ByVal Power As Double) As Integer

        Dim Idx As Integer
        Dim Count As Integer
        Dim strMSG As String

        Try
            ' FL�łȂ��܂���SL436S�łȂ����NOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            ' ���H�����ԍ����o�^�ς݂��`�F�b�N����
            Count = gwkCndCount                                         ' ���H�����o�^�� 
            For Idx = 0 To (Count - 1)
                ' �d���l, ���g��, STEG�{�������������H�����͓o�^�ς݂� ?
                If ((gwkCND.Curr(Idx) = Curr) And _
                    (gwkCND.Freq(Idx) = Freq) And _
                    (gwkCND.Steg(Idx) = Steg) And _
                    (gwkPower(Idx) = Power)) Then
                    Return (Idx)                                        ' �o�^�ς݉��H�����ԍ���Ԃ�
                End If
            Next Idx

            Return (Idx)                                                ' �o�^�������H�����ԍ���Ԃ�

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.Get_CndNumFromWkCND() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region
    '----- V4.0.0.0-58�� -----
#Region "�\���J�b�g���s��"
    '''=========================================================================
    '''<summary>�\���J�b�g���s��</summary>
    ''' <param name="BPx">         (INP)�J�b�g�ʒuX</param>
    ''' <param name="BPy">         (INP)�J�b�g�ʒuY</param>
    ''' <param name="CondNum">     (INP)���H�����ԍ�(FL�p)</param>
    ''' <param name="dQrate">      (INP)Q���[�g(KHz)</param>
    ''' <param name="dblCutLength">(INP)�J�b�g��</param>
    ''' <param name="dblCutSpeed"> (INP)�J�b�g���x</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>���\����Ă̒�����BP���ړ����Ă�������
    '''            �L�����u���[�V�����A�J�b�g�ʒu�␳(�O���J����)�̏\���J�b�g�p</remarks>
    '''=========================================================================
    Public Function CrossCutExec(ByVal BPx As Double, ByVal BPy As Double, ByVal CondNum As Integer, _
                                 ByVal dQrate As Double, ByVal dblCutLength As Double, ByVal dblCutSpeed As Double) As Integer

        Dim r As Integer
        Dim intXANG As Integer
        Dim intYANG As Integer
        Dim strMSG As String
        Dim stCutCmnPrm As CUT_COMMON_PRM                               ' �J�b�g�p�����[�^

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            Call InitCutParam(stCutCmnPrm)                              ' �J�b�g�p�����[�^������

            ' �J�b�g�p�x��ݒ肷��
            Select Case (gSysPrm.stDEV.giBpDirXy)
                Case 0      ' x��, y��
                    intXANG = 180
                    intYANG = 270
                Case 1      ' x��, y��
                    intXANG = 0
                    intYANG = 270
                Case 2      ' x��, y��
                    intXANG = 180
                    intYANG = 90
                Case 3      ' x��, y��
                    intXANG = 0
                    intYANG = 90
            End Select

            ' �J�b�g�p�����[�^(�J�b�g���\����)��ݒ肷��
            stCutCmnPrm.CutInfo.srtMoveMode = 2                         ' ���샂�[�h�i0:�g���~���O�A1:�e�B�[�`���O�A2:�����J�b�g�j
            stCutCmnPrm.CutInfo.srtCutMode = 4                          ' �J�b�g���[�h�́u�΂߁v
            stCutCmnPrm.CutInfo.dblTarget = 1000.0#                     ' �ڕW�l = 1�Ƃ���
            stCutCmnPrm.CutInfo.srtSlope = 4                            ' 4:��R����{�X���[�v
            stCutCmnPrm.CutInfo.srtMeasType = 0                         ' ����^�C�v(0:����(3��)�A1:�����x(2000��)
            stCutCmnPrm.CutInfo.dblAngle = intXANG                      ' �J�b�g�p�x(X��)

            ' �J�b�g�p�����[�^(���H�ݒ�\����)��ݒ肷��
            stCutCmnPrm.CutCond.CutLen.dblL1 = dblCutLength             ' �J�b�g��(Line1�p)
            stCutCmnPrm.CutCond.SpdOwd.dblL1 = dblCutSpeed              ' �J�b�g�X�s�[�h�i���H�j(Line1�p)
            stCutCmnPrm.CutCond.QRateOwd.dblL1 = dQrate                 ' �J�b�gQ���[�g�i���H�j(Line1�p)
            stCutCmnPrm.CutCond.CondOwd.srtL1 = CondNum                 ' �J�b�g�����ԍ��i���H�j(Line1�p)

            ' Q���[�g(FL���ȊO)�܂��͉��H�����ԍ�(FL��)��ݒ肷��
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then          ' FL�łȂ� ?
                Call QRATE(dQrate)                                      ' Q���[�g�ݒ�(KHz)
            Else                                                        ' ���H�����ԍ���ݒ肷��(FL��)
                Call QRATE(dQrate)                                      ' Q���[�g�ݒ�(KHz)
                r = FLSET(FLMD_CNDSET, CondNum)                         ' ���H�����ԍ��ݒ�
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_FL
            End If

            '-------------------------------------------------------------------
            '   �\���J�b�g��X�����J�b�g����
            '-------------------------------------------------------------------
            ' BP��X���n�_�ֈړ�����(��Βl�ړ�)
            r = Form1.System1.EX_MOVE(gSysPrm, BPx - (dblCutLength / 2), BPy, 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            Call SendCrossLineMsgToDispGazou()    'V5.0.0.6�K
            ' �\���J�b�g��X�����J�b�g����
            r = Sub_CrossCut(stCutCmnPrm)                               ' X���J�b�g
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            ' BP�𒆐S�֖߂�
            r = Form1.System1.EX_MOVE(gSysPrm, BPx, BPy, 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            Call System.Threading.Thread.Sleep(500)                     ' Wait(msec)

            '-------------------------------------------------------------------
            '   �\���J�b�g��Y�����J�b�g����
            '-------------------------------------------------------------------
            ' BP��Y���n�_�ֈړ�����(��Βl�ړ�)
            r = Form1.System1.EX_MOVE(gSysPrm, BPx, BPy - (dblCutLength / 2), 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            ' �\���J�b�g��Y�����J�b�g����
            stCutCmnPrm.CutInfo.dblAngle = intYANG                      ' �J�b�g�p�x(Y��)
            r = Sub_CrossCut(stCutCmnPrm)                               ' Y���J�b�g
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            ' BP�𒆐S�֖߂�
            r = Form1.System1.EX_MOVE(gSysPrm, BPx, BPy, 1)
            If (r <> cFRS_NORMAL) Then                                  ' �G���[ ?
                Return (r)
            End If
            Call System.Threading.Thread.Sleep(500)                     ' Wait(msec)

            Return (cFRS_NORMAL)

            ' ���H�����ԍ��̐ݒ�G���[��(FL��)
STP_ERR_FL:
            strMSG = MSG_151                                            ' "���H�����̐ݒ�Ɏ��s���܂����"
            Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, vbOKOnly, gAppName)
            Return (cFRS_ERR_RST)                                       ' Return�l = Cancel

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.CrossCutExec() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�\���J�b�g��X���܂���Y�����J�b�g����"
    '''=========================================================================
    '''<summary>�\���J�b�g��X���܂���Y�����J�b�g����</summary>
    ''' <param name="stCutCmnPrm">(INP)�J�b�g�p�����[�^</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>���\���J�b�g�ʒu��BP���ړ����Ă�������
    '''            �L�����u���[�V�����A�J�b�g�ʒu�␳(�O���J����)�̏\���J�b�g�p</remarks>
    '''=========================================================================
    Private Function Sub_CrossCut(ByRef stCutCmnPrm As CUT_COMMON_PRM) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �\���J�b�g��X���܂���Y�����J�b�g����
            r = TRIM_ST(stCutCmnPrm)                                    ' ST�J�b�g
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r < cFRS_NORMAL) Then                                   ' �G���[ ?
                Return (r)
            End If
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.Sub_CrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�J�b�g�p�����[�^������������"
    '''=========================================================================
    '''<summary>�J�b�g�p�����[�^������������</summary>
    ''' <param name="pstCutCmnPrm">(I/O)�J�b�g�p�����[�^</param>
    ''' <remarks>�L�����u���[�V�����A�J�b�g�ʒu�␳(�O���J����)�̏\���J�b�g�p</remarks>
    '''=========================================================================
    Private Sub InitCutParam(ByRef pstCutCmnPrm As CUT_COMMON_PRM)

        Dim strMSG As String

        Try
            ' �J�b�g�p�����[�^������������(�J�b�g���\����)
            pstCutCmnPrm.CutInfo.srtMoveMode = 1                        ' ���샂�[�h�i0:�g���~���O�A1:�e�B�[�`���O�A2:�����J�b�g�j
            pstCutCmnPrm.CutInfo.srtCutMode = 0                         ' �J�b�g���[�h(0:�m�[�}���A1:���^�[���A2:���g���[�X�A3:�΂߁j
            pstCutCmnPrm.CutInfo.dblTarget = 0.0#                       ' �ڕW�l
            pstCutCmnPrm.CutInfo.srtSlope = 4                           ' 4:��R����{�X���[�v
            pstCutCmnPrm.CutInfo.srtMeasType = 0                        ' ����^�C�v(0:����(3��)�A1:�����x(2000��)
            pstCutCmnPrm.CutInfo.dblAngle = 0.0#                        ' �J�b�g�p�x
            pstCutCmnPrm.CutInfo.dblLTP = 0.0#                          ' L�^�[���|�C���g
            pstCutCmnPrm.CutInfo.srtLTDIR = 0                           ' L�^�[����̕���
            pstCutCmnPrm.CutInfo.dblRADI = 0.0#                         ' R����]���a�iU�J�b�g�Ŏg�p�j
            '                                                           ' For Hook Or UCut
            pstCutCmnPrm.CutInfo.dblRADI2 = 0.0#                        ' R2����]���a�iU�J�b�g�Ŏg�p�j
            pstCutCmnPrm.CutInfo.srtHkOrUType = 0                       ' HookCut(3)��U�J�b�g�i3�ȊO�j�̎w��B
            '                                                           ' For Index
            pstCutCmnPrm.CutInfo.srtIdxScnCnt = 0                       ' �C���f�b�N�X/�X�L�����J�b�g��(1�`32767)
            pstCutCmnPrm.CutInfo.srtIdxMeasMode = 0                     ' �C���f�b�N�X���胂�[�h�i0:��R�A1:�d���A2:�O���j
            '                                                           ' For EdgeSense
            pstCutCmnPrm.CutInfo.dblEsPoint = 0.0#                      ' �G�b�W�Z���X�|�C���g
            pstCutCmnPrm.CutInfo.dblRdrJdgVal = 0.0#                    ' ���_�[��������ω���
            pstCutCmnPrm.CutInfo.dblMinJdgVal = 0.0#                    ' ���_�[�J�b�g��Œዖ�e�ω���
            pstCutCmnPrm.CutInfo.srtEsAftCutCnt = 0                     ' ���_�[�ؔ�����̃J�b�g�񐔁i����񐔁j
            pstCutCmnPrm.CutInfo.srtMinOvrNgCnt = 0                     ' ���_�[���o����A�Œ�ω��ʂ̘A��Over���e��
            pstCutCmnPrm.CutInfo.srtMinOvrNgMode = 0                    ' �A��Over����NG�����i0:NG���薢���{, 1:NG������{�B���_�[���؂�, 2:NG���薢���{�B���_�[�؏グ�j
            '                                                           ' For Scan
            pstCutCmnPrm.CutInfo.dblStepPitch = 0.0#                    ' �X�e�b�v�ړ��s�b�`
            pstCutCmnPrm.CutInfo.srtStepDir = 0                         ' �X�e�b�v����

            ' �J�b�g�p�����[�^������������(���H�ݒ�\����)
            pstCutCmnPrm.CutCond.CutLen.dblL1 = 0.0#                    ' �J�b�g��(Line1�p)
            pstCutCmnPrm.CutCond.CutLen.dblL2 = 0.0#                    ' �J�b�g��(Line2�p)
            pstCutCmnPrm.CutCond.CutLen.dblL3 = 0.0#                    ' �J�b�g��(Line3�p)
            pstCutCmnPrm.CutCond.CutLen.dblL4 = 0.0#                    ' �J�b�g��(Line4�p)

            pstCutCmnPrm.CutCond.SpdOwd.dblL1 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line1�p)
            pstCutCmnPrm.CutCond.SpdOwd.dblL2 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line2�p)
            pstCutCmnPrm.CutCond.SpdOwd.dblL3 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line3�p)
            pstCutCmnPrm.CutCond.SpdOwd.dblL4 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line4�p)

            pstCutCmnPrm.CutCond.SpdRet.dblL1 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line1�p)
            pstCutCmnPrm.CutCond.SpdRet.dblL2 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line2�p)
            pstCutCmnPrm.CutCond.SpdRet.dblL3 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line3�p)
            pstCutCmnPrm.CutCond.SpdRet.dblL4 = 0.0#                    ' �J�b�g�X�s�[�h�i���H�j(Line4�p)

            pstCutCmnPrm.CutCond.QRateOwd.dblL1 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line1�p)
            pstCutCmnPrm.CutCond.QRateOwd.dblL2 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line2�p)
            pstCutCmnPrm.CutCond.QRateOwd.dblL3 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line3�p)
            pstCutCmnPrm.CutCond.QRateOwd.dblL4 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line4�p)

            pstCutCmnPrm.CutCond.QRateRet.dblL1 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line1�p)
            pstCutCmnPrm.CutCond.QRateRet.dblL2 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line2�p)
            pstCutCmnPrm.CutCond.QRateRet.dblL3 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line3�p)
            pstCutCmnPrm.CutCond.QRateRet.dblL4 = 0.0#                  ' �J�b�gQ���[�g�i���H�j(Line4�p)

            pstCutCmnPrm.CutCond.CondOwd.srtL1 = 0                      ' �J�b�g�����ԍ��i���H�j(Line1�p)
            pstCutCmnPrm.CutCond.CondOwd.srtL2 = 0                      ' �J�b�g�����ԍ��i���H�j(Line2�p)
            pstCutCmnPrm.CutCond.CondOwd.srtL3 = 0                      ' �J�b�g�����ԍ��i���H�j(Line3�p)
            pstCutCmnPrm.CutCond.CondOwd.srtL4 = 0                      ' �J�b�g�����ԍ��i���H�j(Line4�p)

            pstCutCmnPrm.CutCond.CondRet.srtL1 = 0                      ' �J�b�g�����ԍ��i���H�j(Line1�p)
            pstCutCmnPrm.CutCond.CondRet.srtL2 = 0                      ' �J�b�g�����ԍ��i���H�j(Line2�p)
            pstCutCmnPrm.CutCond.CondRet.srtL3 = 0                      ' �J�b�g�����ԍ��i���H�j(Line3�p)
            pstCutCmnPrm.CutCond.CondRet.srtL4 = 0                      ' �J�b�g�����ԍ��i���H�j(Line4�p)

            Exit Sub

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.InitCutParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Exit Sub
    End Sub
#End Region

#Region "�p�^�[���F�������s���A����ʂ�Ԃ�"
    '''=========================================================================
    ''' <summary>�p�^�[���F�������s���A����ʂ�Ԃ�</summary>
    ''' <param name="GrpNo">    (INP)�O���[�v�ԍ�</param>
    ''' <param name="TmpNo">    (INP)�p�^�[���ԍ�</param>
    ''' <param name="fCorrectX">(OUT)�����X</param> 
    ''' <param name="fCorrectY">(OUT)�����Y</param>
    ''' <param name="coef">     (OUT)���֌W��</param> 
    ''' <returns>cFRS_NORMAL  = ����
    '''          cFRS_ERR_PTN = �p�^�[���p�^�[���F���G���[
    '''          ��L�ȊO     = ���̑��G���[</returns>
    ''' <remarks>�E�p�^�[���F���ʒu�փe�[�u���͈ړ��ςł��邱��
    '''          �E�L�����u���[�V�����A�J�b�g�ʒu�␳(�O���J����)�p
    ''' </remarks>
    '''=========================================================================
    Public Function Sub_PatternMatching(ByRef GrpNo As Short, ByRef TmpNo As Short, ByRef fCorrectX As Double, ByRef fCorrectY As Double, ByRef coef As Double) As Integer

        Dim ret As Short = cFRS_NORMAL
        Dim crx As Double = 0.0                                         ' �����X
        Dim cry As Double = 0.0                                         ' �����Y
        Dim fcoeff As Double = 0.0                                      ' ���֒l
        Dim Thresh As Double = 0.0                                      ' 臒l
        Dim r As Integer = cFRS_NORMAL                                  ' �֐��l
        Dim strMSG As String = ""

        Try
#If VIDEO_CAPTURE = 1 Then
            fCorrectX = 0.0
            fCorrectY = 0.0
            coef = 0.8
            Return (cFRS_NORMAL)   
#End If
            ' �p�^�[���}�b�`���O���̃e���v���[�g�O���[�v�ԍ���ݒ肷��(������ƒx���Ȃ�)
            'V5.0.0.6�B            If (giTempGrpNo <> GrpNo) Then                              ' �e���v���[�g�O���[�v�ԍ����ς���� ?
            giTempGrpNo = GrpNo                                     ' ���݂̃e���v���[�g�O���[�v�ԍ���ޔ�
            Form1.VideoLibrary1.SelectTemplateGroup(GrpNo)          ' �e���v���[�g�O���[�v�ԍ��ݒ�
            'V5.0.0.6�B            End If

            ' 臒l�擾
            Thresh = gDllSysprmSysParam_definst.GetPtnMatchThresh(GrpNo, TmpNo)
            coef = 0.0                                                  ' ��v�x

            ' �p�^�[���}�b�`���O(�O���J����)���s��(Video.ocx���g�p)
            ret = Form1.VideoLibrary1.PatternMatching_EX(TmpNo, 1, True, crx, cry, fcoeff)
            If (ret = cFRS_NORMAL) Then
                r = cFRS_NORMAL                                         ' Return�l = ����
                fCorrectX = crx                                         ' �����X
                fCorrectY = cry                                         ' �����Y
                '' �}�b�`�����p�^�[���̑���ʒu���炸��ʂ����߂�
                'fCorrectX = crx / 1000.0#
                'fCorrectY = -cry / 1000.0#
                coef = fcoeff                                           ' ���֌W��
                strMSG = Globals_Renamed_002 ' �p�^�[���F������
                If (fcoeff < Thresh) Then
                    r = cFRS_ERR_PT2                                    ' �p�^�[���F���G���[(臒l�G���[)
                    strMSG = Globals_Renamed_003 ' �p�^�[���F���G���[(臒l�G���[)
                End If
                'strMSG = strMSG + " (" & "���֌W��" & "=" & fcoeff.ToString("0.000") & " " & "�����X" & "=" & crx.ToString("0.0000") & ", " & "�����Y" & "=" & cry.ToString("0.0000") + ")"
                strMSG = strMSG + " (" & Globals_Renamed_004 & "=" & fcoeff.ToString("0.000") & " " & Globals_Renamed_005 & "=" & crx.ToString("0.0000") & ", " & Globals_Renamed_006 & "=" & cry.ToString("0.0000") + ")"
            Else
                r = cFRS_ERR_PTN                                        ' �p�^�[���F���G���[(�p�^�[����������Ȃ�����)
                strMSG = Globals_Renamed_007 ' �p�^�[���F���G���[(�p�^�[����������Ȃ�����)
            End If

            ' �㏈��
            Console.WriteLine(strMSG)
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.Sub_PatternMatching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�w��u���b�N�̒����փe�[�u�����ړ�����"
    '''=========================================================================
    '''<summary>�w��u���b�N�̒����փe�[�u�����ړ�����</summary>
    '''<param name="intCamera">(INP)��׎��(0:������� 1:�O�����)</param>
    '''<param name="iXPlate">(INP)XPlateNo</param> 
    '''<param name="iYPlate">(INP)YPlateNo</param>  
    '''<param name="iXBlock">(INP)XBlockNo</param> 
    '''<param name="iYBlock">(INP)YBlockNo</param>   
    '''<remarks>�\����Ĉʒu��è��ݸ��߲�Ă�����ڰ��ް�
    '''         ��PP47�̒l�����ꂽ�Ƃ��낪���S�ƂȂ�
    '''         �������ڰĂ��w�肵�Ă��Ӗ��Ȃ�</remarks>
    '''=========================================================================
    Public Function XYTableMoveBlock(ByRef intCamera As Short, ByRef iXPlate As Short, ByRef iYPlate As Short, ByRef iXBlock As Short, ByRef iYBlock As Short) As Short

        Dim dblX As Double
        Dim dblY As Double
        Dim dblRotX As Double
        Dim dblRotY As Double
        Dim dblPSX As Double
        Dim dblPSY As Double
        Dim dblBsoX As Double
        Dim dblBsoY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim intCDir As Short
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim dblTOffsX As Double
        Dim dblTOffsY As Double
        Dim dblStepInterval As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim r As Short
        Dim strMSG As String

        Try
            dblRotX = 0
            dblRotY = 0

            ' ����߼޼��X,Y�擾
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                 ' ����߼޼��X,Y�擾
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            ' ð��وʒu�̾�Ă̎擾
            dblTOffsX = typPlateInfo.dblTableOffsetXDir : dblTOffsY = typPlateInfo.dblTableOffsetYDir

            Call CalcBlockSize(dblBSX, dblBSY)                  ' ��ۯ����ގZ�o

            ' ��ۯ����޵̾�ĎZ�o�@��ۯ�����/2 ��ۯ��̏ی���XY�Ƃ���1 ð��ق̏ی���1
            dblBsoX = (dblBSX / 2.0#) * 1 * 1                   ' Table.BDirX * Table.dir
            dblBsoY = (dblBSY / 2) * 1                          ' Table.BDirY;

            ' �ƕ␳��ѵ̾��X,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
            ' 0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
            ' ���ݸވʒu���W (+or-) ��]���a + ð��ٵ̾�� (+or-) ��ۯ����޵̾�� + ð��ٕ␳��
            Select Case gSysPrm.stDEV.giBpDirXy

                Case 0 ' x��, y��
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 1 ' x��, y��
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 2 ' x��, y��
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

                Case 3 ' x��, y��
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

            End Select

            If (1 = intCamera) Then                             ' �O����׈ʒu���Z ?
                dblX = dblX + gSysPrm.stDEV.gfExCmX
                dblY = dblY + gSysPrm.stDEV.gfExCmY
            End If

            '�ï�ߊԊu�̎Z�o
            intCDir = typPlateInfo.intResistDir                 ' �`�b�v���ѕ����擾(CHIP-NET�̂�)

            If intCDir = 0 Then                                 ' X����
                dblStepInterval = CalcStepInterval(iYBlock)     ' �ï�߲�����َZ�o(Y��)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' ð���Y�������]�Ȃ�
                    dblY = dblY + dblStepInterval
                Else                                            ' ð���Y�������]
                    dblY = dblY - dblStepInterval
                End If
            Else                                                ' Y����
                dblStepInterval = CalcStepInterval(iXBlock)     ' �ï�߲�����َZ�o(X��)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' ð���X�������]�Ȃ�
                    dblX = dblX + dblStepInterval
                Else                                            ' ð���X�������]
                    dblX = dblX - dblStepInterval
                End If
            End If

            ' ��ڰ�/��ۯ��ʒu�̑��΍��W�v�Z
            dblPSX = 0.0 : dblPSY = 0.0                         ' ��ڰĻ��ގ擾(0�Œ�)
            Select Case gSysPrm.stDEV.giBpDirXy

                Case 0 ' x��, y��
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

                Case 1 ' x��, y��
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

                Case 2 ' x��, y��
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

                Case 3 ' x��, y��
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

            End Select

            ' �w����ڰ�/��ۯ��ʒu��XYð��ِ�Βl�ړ�
            r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)
            Return (r)                                      ' Return

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.XYTableMoveBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return�l = ��O�G���[ 
        End Try

    End Function
#End Region
    'V1.13.0.0�E ADD START
#Region "�e�[�u���I�t�Z�b�g�����������e�[�u���ʒu��Ԃ�"
    '''=========================================================================
    ''' <summary>
    ''' �e�[�u���I�t�Z�b�g�����������e�[�u���ʒu��Ԃ�
    ''' </summary>
    ''' <param name="PosX">(OUT)���WX</param>
    ''' <param name="PosY">(OUT)���WY</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    ''' <remarks>'V1.13.0.0�E</remarks>
    '''=========================================================================
    Public Function XYtableMoveOffsetPosition(ByRef PosX As Double, ByRef PosY As Double) As Integer

        Dim dblTrimPosX As Double                                   ' ����߼޼��X
        Dim dblTrimPosY As Double                                   ' ����߼޼��Y
        Dim Del_x As Double                                         ' �ƕ␳��X
        Dim Del_y As Double                                         ' �ƕ␳��Y
        Dim dblX As Double                                          ' �ړ����WX
        Dim dblY As Double                                          ' �ړ����WY
        Dim mdTbOffx As Double                                      ' ð��وʒu�̾��X
        Dim mdTbOffy As Double                                      ' ð��وʒu�̾��Y

        Dim strMSG As String

        Try
            ' ����߼޼��X,Y�擾
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                     ' ����߼޼��X,Y�擾
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            mdTbOffx = typPlateInfo.dblTableOffsetXDir              ' ð��وʒu�̾��X,Y�̎擾
            mdTbOffy = typPlateInfo.dblTableOffsetYDir

            ' �ƕ␳��ѵ̾��X,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy ���W�n�̐ݒ�(���ѐݒ�)
            ' 0:XY NOM(�E��)  1:X REV(����)  2:Y REV(�E��)  3:XY REV(����)
            ' ���ݸވʒu���W (+or-) ��]���a + ð��ٵ̾�� (+or-) ��ۯ����޵̾�� + ð��ٕ␳��
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x��, y��
                    dblX = dblTrimPosX + mdTbOffx + Del_x
                    dblY = dblTrimPosY + mdTbOffy + Del_y
                Case 1 ' x��, y��
                    dblX = dblTrimPosX - mdTbOffx - Del_x
                    dblY = dblTrimPosY + mdTbOffy + Del_y
                Case 2 ' x��, y��
                    dblX = dblTrimPosX + mdTbOffx + Del_x
                    dblY = dblTrimPosY + mdTbOffy - Del_y
                Case 3 ' x��, y��
                    dblX = dblTrimPosX - mdTbOffx + Del_x
                    dblY = dblTrimPosY + mdTbOffy - Del_y
            End Select

            PosX = dblX
            PosY = dblY
            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.XYtableMoveOffsetPosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[ 
        End Try
    End Function
#End Region
    'V1.13.0.0�E ADD END

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Ɉړ�"
#If False Then 'V5.0.0.8�@
#Region "GPIB�R�}���h��ݒ肷��"
    '''=========================================================================
    '''<summary>GPIB�R�}���h��ݒ肷��</summary>
    '''<param name="pltInfo">(OUT)�v���[�g�f�[�^</param>
    '''=========================================================================
    Public Sub SetGpibCommand(ByRef pltInfo As PlateInfo)

        Dim strDAT As String
        Dim strMSG As String

        Try
            ' ADEX AX-1152�p�ݒ�R�}���h��ݒ肷��
            pltInfo.intGpibDefAdder = giGpibDefAdder                ' GPIB�A�h���X 
            pltInfo.intGpibDefDelimiter = 0                         ' �����ݒ�(�����)(�Œ�)
            pltInfo.intGpibDefTimiout = 100                         ' �����ݒ�(��ѱ��)(�Œ�)
            If (pltInfo.intGpibMeasSpeed = 0) Then                  ' ���葬�x(0:�ᑬ, 1:����)
                strDAT = "W0"
            Else
                strDAT = "W1"
            End If

            '// ���胂�[�h�Ő؂�ւ�
            If (pltInfo.intGpibMeasMode = 0) Then                   ' ���胂�[�h(0:���, 1:�΍�)
                strDAT = strDAT + "FR"                              ' ���胂�[�h=���
                strDAT = strDAT + "LL00000" + "LH15000"             ' ����/������~�b�g�̐ݒ�
            Else

                strDAT = strDAT + "FD"                              ' ���胂�[�h=�΍�
                strDAT = strDAT + "DL-5000" + "DH+5000"             ' ����/������~�b�g�̐ݒ�
            End If

            pltInfo.strGpibInitCmnd1 = strDAT                       ' �����������1
            pltInfo.strGpibInitCmnd2 = ""                           ' �����������2
            pltInfo.strGpibTriggerCmnd = "E"                        ' �ض޺����

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "globals.SetGpibCommand() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
#End If
#End Region
    '----- ###211�� -----
#Region "START/RESET�L�[�����҂��T�u���[�`��"
    '''=========================================================================
    ''' <summary>START/RESET�L�[�����҂��T�u���[�`��</summary>
    ''' <param name="Md">(INP)cFRS_ERR_START                = START�L�[�����҂�
    '''                       cFRS_ERR_RST                  = RESET�L�[�����҂�
    '''                       cFRS_ERR_START + cFRS_ERR_RST = START/RESET�L�[�����҂�
    ''' </param>
    ''' <param name="bZ">(INP)True=Z�L�[�����`�F�b�N����, False=���Ȃ� ###220</param>
    ''' <returns>cFRS_ERR_START = START�L�[����
    '''          cFRS_ERR_RST   = RESET�L�[����
    '''          cFRS_ERR_Z     = Z�L�[����
    '''          ��L�ȊO=�G���[
    ''' </returns>
    '''=========================================================================
    Public Function WaitStartRestKey(ByVal Md As Integer, ByVal bZ As Boolean) As Integer

        Dim sts As Long = 0
        Dim r As Long = 0
        Dim ExitFlag As Integer
        Dim strMSG As String

        Try
            ' �p�����[�^�`�F�b�N
            If (Md = 0) Then
                Return (-1 * ERR_CMD_PRM)                               ' �p�����[�^�G���[
            End If

#If cOFFLINEcDEBUG Then                                                 ' OffLine���ޯ��ON ?(��FormReset���őO�ʕ\���Ȃ̂ŉ��L�̂悤�ɂ��Ȃ���MsgBox���őO�ʕ\������Ȃ�)
            Dim Dr As System.Windows.Forms.DialogResult
            Dr = MessageBox.Show("START SW CHECK", "Debug", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If (Dr = System.Windows.Forms.DialogResult.OK) Then
                ExitFlag = cFRS_ERR_START                               ' Return�l = START�L�[����
            Else
                ExitFlag = cFRS_ERR_RST                                 ' Return�l = RESET�L�[����
            End If
            Return (ExitFlag)
#End If
            ' START/RESET�L�[�����҂�
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            ExitFlag = -1
            Call Form1.System1.SetSysParam(gSysPrm)                     ' �V�X�e���p�����[�^�̐ݒ�(OcxSystem�p)

            ' START/RESET�L�[�����҂�
            Do
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW�����`�F�b�N
                If (sts = cFRS_ERR_RST) And ((Md = cFRS_ERR_RST) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    ExitFlag = cFRS_ERR_RST                             ' ExitFlag = Cancel(RESET�L�[)
                ElseIf (sts = cFRS_ERR_START) And ((Md = cFRS_ERR_START) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    ExitFlag = cFRS_ERR_START                           ' ExitFlag = OK(START�L�[)
                End If
                '----- ###220�� -----
                If (bZ = True) Then
                    r = Z_SWCHECK(sts)                                  ' Z SW�����`�F�b�N
                    If (sts <> 0) Then
                        ExitFlag = cFRS_ERR_Z                           ' ExitFlag = Z�L�[����
                    End If
                End If
                '----- ###220�� -----
                System.Windows.Forms.Application.DoEvents()             ' ���b�Z�[�W�|���v
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)

                ' �V�X�e���G���[�`�F�b�N
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' ����~��(���b�Z�[�W�͕\����) ?
                    ExitFlag = r
                    Exit Do
                End If
            Loop While (ExitFlag = -1)

            Call ZCONRST()                                              ' �R���\�[���L�[���b�`����
            Return (ExitFlag)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "Globals.WaitRestKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region
    '----- ###211�� -----
#Region "���[�U�J�X�^�}�C�Y��ʕ\��"
    '''=========================================================================
    ''' <summary>���[�U�J�X�^�}�C�Y��ʕ\�� V1.14.0.0�@</summary>
    ''' <param name="CTM_NO">���[�U�ԍ�</param>
    '''=========================================================================
    Public Function Set_UserSpecialCtrl(ByVal CTM_NO As Integer) As Integer

        Dim ret As Integer
        Dim strMsg As String

        Try
            ret = True

            With Form1
                Select Case (CTM_NO)
                    Case customSUSUMU
                        .CmdCnd.Visible = True

                        '�@V1.14.0.0�@ ��
                        If gSysPrm.stDEV.sEditLock_flg = True Then
                            ' UNLOCK->LOCK
                            gSysPrm.stDEV.sEditLock_flg = False
                            .CmdCnd.Text = "EDIT LOCK"
                            .CmdCnd.BackColor = System.Drawing.SystemColors.Control
                            giPassWord_Lock = 1
                        Else
                            ' LOCK->UNLOCK
                            gSysPrm.stDEV.sEditLock_flg = True
                            .CmdCnd.Text = "EDIT UNLOCK"
                            .CmdCnd.BackColor = System.Drawing.Color.Lime
                            giPassWord_Lock = 0
                        End If

                        '----- V1.18.0.0�A�� -----
                        ' ���[���a�d�l
                    Case customROHM
                        ' QR���ޏ��(������)��\��
                        Call QR_Info_Disp(0)                            ' QR���ޏ��̕\��������
                        .GrpQrCode.Visible = True                       ' QR���ޏ��\���O���[�v�{�b�N�X�\��

                        ' ���[�}�K�W���Ɏ��[����������\���O���[�v�{�b�N�X�\�� 'V1.18.0.0�K
                        '.GrpStrageBox.Visible = True
                        ' V6.0.3.0�D
                        QR_Read_Flg = 0                                 ' QR���ޓǍ��ݔ���(0)NG (1)OK

                        '�|�[�g�I�[�v��(QR�f�[�^��M�p)
                        ret = QR_Rs232c_Open()
                        If (ret <> SerialErrorCode.rRS_OK) Then
                            ' "�V���A���|�[�g�n�o�d�m�G���["
                            ' strMsg = MSG_136 + "(" + "COM4" + ")" 'V4.0.0.0-79
                            strMsg = MSG_136 + "(" + "QR Code Reader COM5" + ")" 'V4.0.0.0-79
                            Call MsgBox(strMsg, vbOKOnly)
                            'Return (ret)
                        End If

                        ' �����������
                        ret = Print_Init()
                        If (ret <> cFRS_NORMAL) Then
                            ' "������������Ɏ��s���܂����B(r=xxxx)"
                            strMsg = MSG_152 + "(r = " + ret.ToString("0") + ")"
                            Call MsgBox(strMsg, vbOKOnly)
                            Return (ret)
                        End If
                        '----- V1.18.0.0�A�� -----

                        '----- V6.0.3.0�D�� -----
                        ' QRLimit�{�^��������
                        Form1.btnQRLmit.Visible = True
                        Form1.btnQRLmit.BackColor = Color.Lime
                        '----- V6.0.3.0�D�� -----

                        '----- V1.23.0.0�@�� -----
                        ' ���z�Гa�d�l
                    Case customTAIYO
#If False Then                          'V5.0.0.9�N Form1.SetBarcodeMode() �ł����Ȃ�

                        ' �o�[�R�[�h���(������)��\��
                        Call BC_Info_Disp(0)                            ' �o�[�R�[�h���̕\��������
                        .GrpQrCode.Visible = True                       ' �o�[�R�[�h���(QR���ޏ�����g�p)�\���O���[�v�{�b�N�X�\��

                        ' �|�[�g�I�[�v��(�o�[�R�[�h�f�[�^��M�p)
                        ret = BC_Rs232c_Open()
                        If (ret <> SerialErrorCode.rRS_OK) Then
                            ' "�V���A���|�[�g�n�o�d�m�G���["
                            strMsg = MSG_136 + "(" + gsComPort + ")"
                            Call MsgBox(strMsg, vbOKOnly)
                            'Return (ret)
                        End If
                        '----- V1.23.0.0�@�� -----
#End If
                        '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
                    Case customWALSIN
#If False Then                          'V5.0.0.9�N Form1.SetBarcodeMode() �ł����Ȃ�
                        ' �o�[�R�[�h���(������)��\��
                        Call BC_Info_Disp(0)                            ' �o�[�R�[�h���̕\��������
                        .GrpQrCode.Visible = True                       ' �o�[�R�[�h���(QR���ޏ�����g�p)�\���O���[�v�{�b�N�X�\��

                        ' �|�[�g�I�[�v��(�o�[�R�[�h�f�[�^��M�p)
                        ret = BC_Rs232c_Open()
                        If (ret <> SerialErrorCode.rRS_OK) Then
                            ' "�V���A���|�[�g�n�o�d�m�G���["
                            strMsg = MSG_136 + "(" + gsComPort + ")"
                            Call MsgBox(strMsg, vbOKOnly)
                            'Return (ret)
                        End If

                        ' SizeCode.ini���\���̂�ݒ肷��
                        ret = IniSizeCodeData(StSizeCode)
                        '----- V4.11.0.0�A�� -----
#End If
                    Case Else
                        ret = True
                End Select
            End With

            Return (ret)
        Catch ex As Exception
            strMsg = "Globals.Set_UserSpecialCtrl() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (False)
        End Try
    End Function
#End Region
    '----- V1.18.0.0�@�� -----
#Region "�f�[�^�ҏW�p�p�X���[�h��ʎ擾"
    '''=========================================================================
    ''' <summary>�f�[�^�ҏW�p�p�X���[�h��ʎ擾</summary>
    ''' <param name="CTM_NO">���[�U�ԍ�</param>
    ''' <returns>�p�X���[�h�@�\(0=�Ȃ�, 1=����(�i�a�d�l), 2����(���[���a�d�l))</returns>
    '''=========================================================================
    Public Function Get_DataEdit_PasswordKind(ByVal CTM_NO As Integer) As Integer

        Dim ret As Integer = 0
        Dim strMsg As String

        Try
            Select Case (CTM_NO)
                Case customSUSUMU                                       ' �i�a�d�l ?
                    ret = giPassWord_Lock                               ' ret = 0(�p�X���[�h�@�\�Ȃ�)/ 1 (�p�X���[�h�@�\����(�i�a�d�l))

                Case customROHM                                         ' ���[���a�d�l ?
                    ' �uAdministrator Mode�v���́u���O�C���p�X���[�h�Ȃ��v?
                    'If (Form1.lblLoginResult.Visible = True) Or (Form1.flgLoginPWD = 0) Then ' V6.0.3.0_42
                    If (Form1.lblLoginResult.Visible = True) Or (flgLoginPWD = 0) Then        ' V6.0.3.0_42
                        ret = 0                                         ' ret = 0(�p�X���[�h�@�\�Ȃ�) 
                    Else
                        ret = 2                                         ' ret = 2(�p�X���[�h�@�\����(���[���a�d�l)) 
                    End If

                Case Else                                               ' ���̑� 
                    ret = 0                                             ' ret = 0(�p�X���[�h�@�\�Ȃ�) 
            End Select

            Return (ret)

        Catch ex As Exception
            strMsg = "Globals.Get_DataEdit_PasswordKind() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (0)
        End Try
    End Function
#End Region
    '----- V1.18.0.0�@�� -----
    '----- V1.14.0.0�D�� -----
#Region "�k�d�c�o�b�N���C�g����"
    '''=========================================================================
    ''' <summary>�k�d�c�o�b�N���C�g����</summary>
    ''' <param name="MD">   (INP)0:�����ݒ胂�[�h,1:���s���[�h, 2:�I�����[�h</param>
    ''' <param name="OnOff">(INP)On/Off�w�� (1:�n�m,0:�n�e�e)
    '''                     �����s���[�h�Ń��[�_�������ŉ摜�F�����̂ݗL��</param>
    ''' <remarks>stSPF.giLedOnOff = 0(LED����Ȃ�)
    ''' �@�@�@�@�@�@�@�@�@�@�@�@�@= 1(LED���䂠��ŏ펞ON)
    '''                           = 2(LED���䂠��ŏ펞OFF)
    '''                           = 3(LED���䂠��ŉ摜�F�����̂�ON)</remarks>
    '''=========================================================================
    Public Sub Set_Led(ByVal MD As Integer, ByVal OnOff As Integer)

        Dim strMsg As String

        Try
            ' �k�d�c�o�b�N���C�g����Ȃ��Ȃ�NOP
            If (gSysPrm.stSPF.giLedOnOff = 0) Then Exit Sub

            ' �I�����[�h��
            If (MD = 2) Then                                            ' �������[�h = �I�����[�h ?
                Call EXTOUT1(0, glLedBit)                               ' �o�b�N���C�g�Ɩ��n�e�e
                Exit Sub
            End If

            ' �����ݒ胂�[�h��
            If (MD = 0) Then                                            ' �������[�h = �����ݒ胂�[�h ?
                If (gSysPrm.stSPF.giLedOnOff = 2) Then                  ' �펞�n�m ?
                    Call EXTOUT1(glLedBit, 0)                           ' �o�b�N���C�g�Ɩ��n�m(��0x216a��xBIT BIT��Ԃ͕ێ�)

                Else                                                    ' �펞�n�e�e/�摜�F���� ?
                    Call EXTOUT1(0, glLedBit)                           ' �o�b�N���C�g�Ɩ��n�e�e
                End If

                ' ���s���[�h��
            Else
                ' �I�[�g���[�_�������ȊO��NOP
                'If (giHostMode <> cHOSTcMODEcAUTO) Then Exit Sub                                   'V1.20.0.0�I
                If (giHostMode = cHOSTcMODEcAUTO) Or (gSysPrm.stCTM.giSPECIAL = customSUSUMU) Then  'V1.20.0.0�I
                    If (gSysPrm.stSPF.giLedOnOff = 3) Then                  ' �o�b�N���C�g�Ɩ� = �摜�F�����̂� ?
                        If (OnOff = 1) Then                                 ' �o�b�N���C�g�Ɩ��n�m ?
                            Call EXTOUT1(glLedBit, 0)                       ' �o�b�N���C�g�Ɩ��n�m
                            Call System.Threading.Thread.Sleep(200)         ' Wait(ms) 
                        Else
                            Call EXTOUT1(0, glLedBit)                       ' �o�b�N���C�g�Ɩ��n�e�e
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            strMsg = "Globals.Set_Led() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V1.14.0.0�D�� -----
    '----- V1.18.0.0�B�� -----
#Region "�t�@�C���p�X������t�@�C�������������o���ĕԂ�"
    '''=========================================================================
    ''' <summary>�t�@�C���p�X������t�@�C�������������o���ĕԂ�</summary>
    ''' <param name="FPath">(INP)�t�@�C���p�X��</param>
    ''' <param name="FNam"> (OUT)�t�@�C����</param>
    '''=========================================================================
    Public Sub Sub_GetFileName(ByRef FPath As String, ByRef FNam As String)

        Dim Idx As Integer
        Dim rDATA() As String
        Dim strMsg As String

        Try
            rDATA = FPath.Split("\")
            Idx = rDATA.Length
            FNam = rDATA(Idx - 1)

        Catch ex As Exception
            strMsg = "Globals.Sub_GetFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "�t�@�C���p�X������h���C�u���ƃt�H���_�������o���ĕԂ�"
    '''=========================================================================
    ''' <summary>�t�@�C���p�X������h���C�u���ƃt�H���_�������o���ĕԂ�</summary>
    ''' <param name="FPath">(INP)�t�@�C���p�X��</param>
    ''' <param name="sDrv"> (OUT)�h���C�u��</param>
    ''' <param name="sFold">(OUT)�t�H���_��</param>
    '''=========================================================================
    Public Sub Sub_GetFileFolder(ByRef FPath As String, ByRef sDrv As String, ByRef sFold As String)

        Dim Idx As Integer
        Dim rDATA() As String
        Dim strMsg As String

        Try
            rDATA = FPath.Split("\")
            sFold = ""
            sDrv = rDATA(0)
            For Idx = 0 To (rDATA.Length - 2)
                If (Idx = (rDATA.Length - 2)) Then
                    sFold = sFold + rDATA(Idx)
                Else
                    sFold = sFold + rDATA(Idx) + "\"
                End If
            Next Idx

        Catch ex As Exception
            strMsg = "Globals.Sub_GetFileFolder() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0�B�� -----
    '----- V1.22.0.0�C�� -----
#Region "�g���q�𔲂������t�@�C���������o���ĕԂ�"
    '''=========================================================================
    ''' <summary>�g���q�𔲂������t�@�C���������o���ĕԂ�</summary>
    ''' <param name="strFilePath">(INP)�t�@�C���p�X��</param>
    '''=========================================================================
    Public Function GetFileNameNonExtension(ByRef strFilePath As String) As String

        Dim intPos As Integer
        Dim strFileName As String
        Dim strMsg As String

        Try
            ' �ŏI��\�L���̈ʒu���擾
            intPos = InStrRev(strFilePath, "\")
            If (intPos > 0) Then
                ' \�L���ȍ~�̕������擾(�t�@�C�����݂̂��擾)
                strFileName = Right$(strFilePath, Len(strFilePath) - intPos)
            Else
                strFileName = strFilePath
            End If

            ' �ŏI��.�L�����擾
            intPos = InStrRev(strFileName, ".")
            If (intPos > 0) Then
                ' .�L���O�̕������擾(�g���q�𔲂����`�Ŏ擾)
                strFileName = Left$(strFileName, intPos - 1)
            End If

            Return (strFileName)

        Catch ex As Exception
            strMsg = "Globals.GetFileNameNonExtension() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return ("")
        End Try
    End Function
#End Region

#Region "�t�@�C��������g���q�����o���ĕԂ�"
    '''=========================================================================
    ''' <summary>�t�@�C��������g���q�����o���ĕԂ� V4.0.0.0�H</summary>
    ''' <param name="strFilePath">(INP)�t�@�C���p�X��</param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function GetFileNameExtension(ByRef strFilePath As String) As String

        Dim Pos As Integer
        Dim Len As Integer
        Dim strEXT As String
        Dim strMsg As String

        Try
            ' �t�@�C��������g���q�����o���ĕԂ�
            Len = strFilePath.Length
            Pos = strFilePath.LastIndexOf(".")                             'V1.16.0.0�N
            strEXT = strFilePath.Substring(Pos, Len - Pos)

            Return (strEXT)

        Catch ex As Exception
            strMsg = "Globals.GetFileNameExtension() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return ("")
        End Try
    End Function
#End Region

#Region "�T�}���[���M���O�t�@�C���p�X���쐬"
    '''=========================================================================
    ''' <summary>�T�}���[���M���O�t�@�C���p�X���쐬</summary>
    ''' <returns>�T�}���[���M���O�t�@�C���p�X��</returns>
    '''=========================================================================
    Public Function MakeSummaryFileName() As String

        Dim strFileName As String
        Dim strFilePath As String
        Dim strMsg As String

        Try
            ' �T�}���[���M���O�t�@�C�����̍쐬 = "���H�f�[�^��_SUMMARY.TXT"
            strFileName = GetFileNameNonExtension(gStrTrimFileName) ' ���ݸ��ް�̧�ٖ��̊g���q�𔲂������t�@�C���������o���ĕԂ�
            strFilePath = gSysPrm.stLOG.gsLoggingDir + strFileName + "_SUMMARY.TXT"

            Return (strFilePath)

        Catch ex As Exception
            strMsg = "Globals.MakeSummaryFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return ("")
        End Try
    End Function
#End Region
    '----- V1.22.0.0�C�� -----
    '----- V3.0.0.0�C(V1.22.0.0�B) -----
#Region "�g���~���O�f�[�^�����[�h����Ă��邱�Ƃ��m�F"
    '''=========================================================================
    ''' <summary>�g���~���O�f�[�^�����[�h����Ă��邱�Ƃ��m�F</summary>
    ''' <returns></returns>
    '''=========================================================================
    Public Function ChkTrimDataLoaded() As Integer

        ChkTrimDataLoaded = cFRS_NORMAL

        If (gLoadDTFlag = False) Then                                   ' ���ݸ��ް�̧�ٖ�۰�� ?
            ' "�g���~���O�p�����[�^�f�[�^�����[�h���邩�V�K�쐬���Ă�������"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
            ChkTrimDataLoaded = cFRS_ERR_RST
        End If

    End Function
#End Region
    '----- V1.22.0.0�B�� -----
    '----- V4.0.0.0-40�� -----
#Region "�X�e�[�WY�̌��_�ʒu(���E��)���l������Y���W��ϊ����ĕԂ�"
    '''=========================================================================
    ''' <summary>�X�e�[�WY�̌��_�ʒu(���E��)���l������Y���W��ϊ����ĕԂ�</summary>
    ''' <param name="StgY">(I/O)</param>
    '''=========================================================================
    Public Sub Sub_GetStageYPosistion(ByRef StgY As Double)

        Dim strMsg As String

        Try
            ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
            If (giMachineKd = MACHINE_KD_RS) Then
                ''V4.3.0.0�A
                'If (giStageYOrg = STGY_ORG_UP) Then
                StgY = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
                'Else
                '    StgY = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2)
                'End If
                ''V4.3.0.0�A
            End If

            Return

        Catch ex As Exception
            strMsg = "Globals.Sub_GetStagePosistion() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V4.0.0.0-40�� -----
#Region "�u���b�N�ԍ�����u���b�N�ʒu�w�E�x���擾����"
    ''' <summary>�u���b�N�ԍ�����u���b�N�ʒu�w�E�x���擾����</summary>
    ''' <param name="blockNo">�u���b�N�ԍ�</param>
    ''' <param name="blockPosX">�u���b�N�ʒu�w(1ORG)</param>
    ''' <param name="blockPosY">�u���b�N�ʒu�x(1ORG)</param>
    ''' <remarks>'V4.12.2.0�@</remarks>
    Friend Sub GetBlockPosition(ByVal blockNo As Integer, ByRef blockPosX As Integer, ByRef blockPosY As Integer)
        If (1 <= blockNo) Then
            With typPlateInfo
                Dim x As Integer = .intBlockCntXDir
                Dim y As Integer = .intBlockCntYDir

                blockNo -= 1
                Select Case .intDirStepRepeat       ' �X�e�b�v&(���s�[�g����)
                    Case STEP_RPT_Y, STEP_RPT_CHIPXSTPY, STEP_RPT_NON
                        blockPosX = Math.Truncate(blockNo / y) + 1

                        If (0 = (blockPosX Mod 2)) Then
                            blockPosY = y - (blockNo Mod y)
                        Else
                            blockPosY = y - (y - (blockNo Mod y)) + 1
                        End If

                    Case STEP_RPT_X, STEP_RPT_CHIPYSTPX
                        blockPosY = Math.Truncate(blockNo / x) + 1

                        If (0 = (blockPosY Mod 2)) Then
                            blockPosX = x - (blockNo Mod x)
                        Else
                            blockPosX = x - (x - (blockNo Mod x)) + 1
                        End If

                    Case Else ' �ʂ�Ȃ�
                        blockPosX = 1
                        blockPosY = 1
                End Select
            End With
        Else
            blockPosX = 1
            blockPosY = 1
        End If

    End Sub
#End Region

#Region "���Ԗڂɏ�������u���b�N�ł��邩���擾����"
    ''' <summary>���Ԗڂɏ�������u���b�N�ł��邩���擾����</summary>
    ''' <param name="blockPosX">�u���b�N�ʒu�w(1ORG)</param>
    ''' <param name="blockPosY">�u���b�N�ʒu�x(1ORG)</param>
    ''' <returns>�������鏇��</returns>
    ''' <remarks>'V4.12.2.0�A</remarks>
    Friend Function GetBlockNumber(ByVal blockPosX As Integer, ByVal blockPosY As Integer) As Integer
        Dim ret As Integer

        ' �X�e�b�v&(���s�[�g����)
        With typPlateInfo
            Dim x As Integer = .intBlockCntXDir
            Dim y As Integer = .intBlockCntYDir

            Select Case .intDirStepRepeat
                Case STEP_RPT_Y, STEP_RPT_CHIPXSTPY, STEP_RPT_NON
                    If (0 = (blockPosX Mod 2)) Then
                        ret = (y * (blockPosX - 1)) + (y - blockPosY + 1)
                    Else
                        ret = (y * blockPosX) - (y - blockPosY)
                    End If

                Case STEP_RPT_X, STEP_RPT_CHIPYSTPX
                    If (0 = (blockPosY Mod 2)) Then
                        ret = (x * (blockPosY - 1)) + (x - blockPosX + 1)
                    Else
                        ret = (x * blockPosY) - (x - blockPosX)
                    End If

                Case Else ' �ʂ�Ȃ�
                    ret = 1
            End Select
        End With

        Return ret

    End Function
#End Region

#Region "���̏����Ώۃu���b�N�ԍ����擾����"
    ''' <summary>���̏����Ώۃu���b�N�ԍ����擾����</summary>
    ''' <param name="currentBlockNo">���݂̏����u���b�N�ԍ�</param>
    ''' <returns>���̏����Ώۃu���b�N�ԍ��A�����Ώۃu���b�N�Ȃ��̏ꍇ�̓u���b�N����+1</returns>
    ''' <remarks>'V4.12.2.0�A</remarks>
    Friend Function GetNextProcBlock(ByVal currentBlockNo As Integer) As Integer
        If (ProcBlock Is Nothing) Then
            Return currentBlockNo
        End If

        Dim x As Integer = typPlateInfo.intBlockCntXDir
        Dim y As Integer = typPlateInfo.intBlockCntYDir
        Dim allBlock As Integer = x * y
        Dim ret As Integer = allBlock + 1

        ' �����Ώۂ̎��u���b�N��T��
        For i As Integer = currentBlockNo To allBlock Step 1
            x = 0
            y = 0
            Globals_Renamed.GetBlockPosition(i, x, y)

            If (TrimControlLibrary.SelectedState.IsSelected = ProcBlock(x, y)) Then
                ret = i
                Exit For
            End If
        Next i

        Return ret

    End Function
#End Region

#Region "�O�̏����Ώۃu���b�N���擾����"
    ''' <summary>�O�̏����Ώۃu���b�N���擾����</summary>
    ''' <param name="currentBlockNo">���݂̏����u���b�N�ԍ�</param>
    ''' <returns>�O�̏����Ώۃu���b�N�ԍ��A�����Ώۃu���b�N�Ȃ��̏ꍇ��0</returns>
    ''' <remarks>'V4.12.2.0�B</remarks>
    Friend Function GetPrevProcBlock(ByVal currentBlockNo As Integer) As Integer
        If (ProcBlock Is Nothing) Then
            Return currentBlockNo
        End If

        Dim ret As Integer = 0

        ' �����Ώۂ̑O�u���b�N��T��
        For i As Integer = currentBlockNo To 1 Step (-1)
            Dim x, y As Integer
            Globals_Renamed.GetBlockPosition(i, x, y)

            If (TrimControlLibrary.SelectedState.IsSelected = ProcBlock(x, y)) Then
                ret = i
                Exit For
            End If
        Next i

        Return ret

    End Function
#End Region

#Region "�����Ώۃu���b�N�����擾����"
    ''' <summary>�����Ώۃu���b�N�����擾����</summary>
    ''' <returns>�����Ώۃu���b�N��</returns>
    ''' <remarks>'V4.12.2.0�A</remarks>
    Friend Function GetProcBlockCount() As Integer
        If (ProcBlock Is Nothing) Then
            Return 0
        End If

        Dim ret As Integer = 0

        For x As Integer = 0 To ProcBlock.GetLength(0) - 1 Step 1
            For y As Integer = 0 To ProcBlock.GetLength(1) - 1 Step 1
                If (TrimControlLibrary.SelectedState.IsSelected = ProcBlock(x, y)) Then
                    ret += 1
                End If
            Next y
        Next x

        Return ret

    End Function
#End Region
    '----- V6.0.3.0�L�� -----
#Region "Double �g�����\�b�h"
    '''=========================================================================
    '''<summary>Double �g�����\�b�h</summary>
    ''' <param name="value">Double.ToStringF(n)</param>
    ''' <param name="n">1�`15</param>
    ''' <returns>value��ϊ�����������</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    <Extension()>
    Public Function ToStringF(ByVal value As Double, ByVal n As Integer) As String
        If (n < 1) OrElse (15 < n) Then
            Throw New ArgumentOutOfRangeException()
        End If
        Return Math.Truncate(value).ToString("0") & "." & (value Mod 1).ToString("F" & n).Substring(2, n)
    End Function

#End Region
    '----- V6.0.3.0�L�� -----
    '----- V6.1.1.0�@�� -----
#Region "�u�U�[��"
    '''=========================================================================
    ''' <summary>�u�U�[��</summary>
    ''' <param name="iRtn">(INP)�g���~���O����</param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function Sub_Buzzer_On(ByVal iRtn As Integer) As Integer

        Dim r As Integer
        Dim strMsg As String = ""
        Dim strMsg1 As String = ""
        Dim strMsg2 As String = ""
        Dim strMsg3 As String = ""
        'V6.1.1.0�K��
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        'V6.1.1.0�K��

        Try
            ' ���b�Z�[�W�ݒ�
            If (giBuzerOn = 0) Then Return (cFRS_NORMAL) '                                  ' �g���~���O�I�����̃u�U�[�炳�Ȃ��Ȃ�NOP
            If (giHostMode = cHOSTcMODEcAUTO) Then Return (cFRS_NORMAL) '                   ' �����^�]���Ȃ�NOP
            strMsg1 = ""
            strMsg3 = MSG_frmLimit_07                                                       ' "START�L�[���������AOK�{�^���������ĉ������B"

            ' �V�O�i���^���[�_�Ł{�u�U�[
            If (iRtn = cFRS_ERR_PTN) Then
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)           ' �ԓ_�Ł{�u�U�[�P�n�m
                strMsg2 = MSG_127                                                           ' "�p�^�[���F���G���["
            Else
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)   ' �Γ_�Ł{�u�U�[�P�n�m
                'V6.1.1.0�K��
                Call Form1.GetMoveMode(digL, digH, digSW)
                If (digL < TRIM_MODE_FT) Then
                    strMsg2 = MSG_SPRASH77                                                      ' "�g���~���O�I��"
                Else
                    strMsg2 = MSG_SPRASH78                                                      ' "����I��"
                End If
                'V6.1.1.0�K��
            End If

            ' "", "�g���~���O�I�� or �p�^�[���F���G���[","START�L�[���������AOK�{�^���������ĉ������B"(���[�U�v���ƍ��킹���Form_MsgDispEx()���g�p)
            Call Form1.System1.EX_SLIDECOVERCHK(SLIDECOVER_CHECK_OFF)                       ' �X���C�h�J�o�[�`�F�b�N�Ȃ�
            r = Form1.System1.Form_MsgDispEx(cFRS_ERR_START, True, strMsg1, strMsg2, strMsg3, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
            Call ZCONRST()                                                                  ' �R���\�[���L�[���b�`����
            Call Form1.System1.EX_SLIDECOVERCHK(SLIDECOVER_CHECK_ON)                        ' �X���C�h�J�o�[�`�F�b�N����

            ' �V�O�i���^���[�����{�u�U�[��~
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)

            Return (r)

        Catch ex As Exception
            strMsg = "Globals.Sub_Buzzer_On() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V6.1.1.0�@�� -----

    '===========================================================================
    '   �ėp�^�C�}�[
    '===========================================================================
    Private bTmTimeOut As Boolean                                       ' �^�C���A�E�g�t���O

#Region "�ėp�^�C�}�[����"
    '''=========================================================================
    ''' <summary>�ėp�^�C�}�[����</summary>
    ''' <param name="TimerTM">(I/O)�^�C�}�[</param>
    ''' <param name="TimeVal">(INP)�^�C���A�E�g�l(msec)</param>
    ''' <remarks>�^�C�}�[���������ꍇ��TimerTM_Dispose��Call���ă^�C�}�[��j�����鎖</remarks>
    '''=========================================================================
    Public Sub TimerTM_Create(ByRef TimerTM As System.Threading.Timer, ByVal TimeVal As Integer)

        Dim strMSG As String

        Try
            ' �^�C���A�E�g�`�F�b�N�p�^�C�}�[�I�u�W�F�N�g�̍쐬(TimerTM_Tick��TimeVal msec�Ԋu�Ŏ��s����)
            bTmTimeOut = False                                          ' �^�C���A�E�g�t���OOFF
            '----- V1.18.0.0�A�� -----
            If (TimeVal = 0) Then
                TimerTM = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerTM_Tick), Nothing, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            Else
                TimerTM = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerTM_Tick), Nothing, TimeVal, TimeVal)
            End If
            '----- V1.18.0.0�A�� -----

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Create() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ėp�^�C�}�[�J�n"
    '''=========================================================================
    ''' <summary>�ėp�^�C�}�[�J�n</summary>
    ''' <param name="TimerTM">(INP)�^�C�}�[</param>
    '''=========================================================================
    Public Sub TimerTM_Start(ByRef TimerTM As System.Threading.Timer, ByVal TimeVal As Integer)

        Dim strMSG As String

        Try
            If (TimerTM Is Nothing) Then Return
            bTmTimeOut = False                                          ' �^�C���A�E�g�t���OOFF V1.18.0.1�G
            TimerTM.Change(TimeVal, TimeVal)
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Start() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ėp�^�C�}�[��~(�R�[���o�b�N���\�b�h(TimerTM_Tick)�̌ďo�����~����)"
    '''=========================================================================
    ''' <summary>�ėp�^�C�}�[��~(�R�[���o�b�N���\�b�h(TimerTM_Tick)�̌ďo�����~����)</summary>
    ''' <param name="TimerTM">(INP)�^�C�}�[</param>
    '''=========================================================================
    Public Sub TimerTM_Stop(ByRef TimerTM As System.Threading.Timer)

        Dim strMSG As String

        Try
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            If (TimerTM Is Nothing) Then Return
            TimerTM.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Stop() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�ėp�^�C�}�[��j������"
    '''=========================================================================
    ''' <summary>�ėp�^�C�}�[��j������</summary>
    ''' <param name="TimerTM">(I/O)�^�C�}�[</param>
    '''=========================================================================
    Public Sub TimerTM_Dispose(ByRef TimerTM As System.Threading.Timer)

        Dim strMSG As String

        Try
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            If (TimerTM Is Nothing) Then Return
            TimerTM.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            TimerTM.Dispose()                                           ' �^�C�}�[��j������
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Dispose() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�^�C���A�E�g�t���O��Ԃ�"
    '''=========================================================================
    ''' <summary>�^�C���A�E�g�t���O��Ԃ�</summary>
    ''' <returns>Trur=�^�C���A�E�g, False=�^�C���A�E�g�łȂ�</returns>
    '''=========================================================================
    Public Function TimerTM_Sts() As Boolean

        Dim strMSG As String

        Try
            ' �^�C���A�E�g�t���O��Ԃ�
            Return (bTmTimeOut)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Sts() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (bTmTimeOut)
        End Try
    End Function
#End Region

#Region "�^�C�}�[�C�x���g(�w��^�C�}�Ԋu���o�߂������ɔ���)"
    '''=========================================================================
    ''' <summary>�^�C�}�[�C�x���g(�w��^�C�}�Ԋu���o�߂������ɔ���)</summary>
    ''' <param name="Sts">(INP)</param>
    '''=========================================================================
    Private Sub TimerTM_Tick(ByVal Sts As Object)

        Dim strMSG As String

        Try
            bTmTimeOut = True                                           ' �^�C���A�E�g�t���OON
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���̓L�[�R�[�h�̃N���A"
    '''=========================================================================
    ''' <summary>
    ''' ���̓L�[�R�[�h�̃N���A
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ClearInpKey()

        Try
            InpKey = 0

            ' �g���b�v�G���[������
        Catch ex As Exception
            MsgBox("globals.ClearInpKey() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "���z�}�\���T�u"
    '''=========================================================================
    '''<summary>���z�}�\���T�u</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub picGraphAccumulationDrawSubLine()
        'Dim i As Short

        '      'UPGRADE_ISSUE: PictureBox ���\�b�h picGraphAccumulation.Line �̓A�b�v�O���[�h����܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' ���N���b�N���Ă��������B
        'picGraphAccumulation.Line (56, 16) - (56, 112), RGB(0, 255, 0)
        '      'UPGRADE_ISSUE: PictureBox ���\�b�h picGraphAccumulation.Line �̓A�b�v�O���[�h����܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' ���N���b�N���Ă��������B
        'picGraphAccumulation.Line (56, 112) - (288, 112), RGB(0, 255, 0)
        '      For i = 0 To 10
        '          'UPGRADE_ISSUE: PictureBox ���\�b�h picGraphAccumulation.Line �̓A�b�v�O���[�h����܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' ���N���b�N���Ă��������B
        '	picGraphAccumulation.Line (56, 24 + (i * 8)) - (288, 24 + (i * 8)), RGB(0, 0, 128)
        '      Next
        '      'UPGRADE_ISSUE: PictureBox ���\�b�h picGraphAccumulation.Line �̓A�b�v�O���[�h����܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' ���N���b�N���Ă��������B
        'picGraphAccumulation.Line (172, 112) - (172, 116), RGB(0, 255, 0)

    End Sub
#End Region

#Region "XY�e�[�u��/Z����JOG����"
    '''=========================================================================
    '''<summary>Z��/XY�e�[�u����JOG����</summary>
    '''<param name="stJOG">       (INP)JOG����p�p�����[�^</param>
    '''<param name="TBarLowPitch">(I/O)�X���C�_�[1(Low�߯�)</param>
    '''<param name="TBarHiPitch"> (I/O)�X���C�_�[2(HIGH�߯�)</param>
    '''<param name="TBarPause">   (I/O)�X���C�_�[3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)�ڐ�1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)�ڐ�2(Low�߯� Label)</param>
    '''<param name="LblTchMoval2">(I/O)�ڐ�3(HIGH�߯� Label)</param>
    '''<param name="dblTchMoval"> (I/O)�߯��ޔ���(0=�߯�, 1=HIGH�߯�, 2=Pause Time)</param>
    '''<returns>cFRS_ERR_ADV = OK(START��) 
    '''         cFRS_ERR_RST = Cancel(RESET��)
    '''         cFRS_ERR_HLT = HALT��
    '''         -1�ȉ�       = �G���[</returns>
    ''' <remarks>JogEzInit�֐���Call�ςł��邱��</remarks>
    '''=========================================================================
    Public Function JogEzMoveWithZ(ByRef stJOG As JOG_PARAM, ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double, _
                         ByVal DispProbePos As Action(Of Double)) As Integer    'V6.0.0.0�J �����ύX
        'ByRef stJogTextZ As Object) As Integer

        Dim strMSG As String
        Dim r As Short
        Dim z As Double

        Try
            '---------------------------------------------------------------------------
            '   ��������
            '---------------------------------------------------------------------------
            X = 0.0 : Y = 0.0                                   ' �ړ��߯�X,Y
            mvx = stJOG.PosX : mvy = stJOG.PosY                 ' BP or ð��وʒuX,Y
            mvxBk = stJOG.PosX : mvyBk = stJOG.PosY
            ' �L�����u���[�V�������s/�J�b�g�ʒu�␳�y�O���J�����z�� �����΍��W��\�����邽�߃N���A���Ȃ�
            ' �g���~���O���̈ꎞ��~��ʂ��N���A���Ȃ�
            If (giAppMode <> APP_MODE_CARIB_REC) And (giAppMode <> APP_MODE_CUTREVIDE) And _
               (giAppMode <> APP_MODE_FINEADJ) And (giAppMode <> APP_MODE_CARIB) Then             ' V1.14.0.0�F ###088
                '(giAppMode <> APP_MODE_TRIM) Then              '###088
                stJOG.cgX = 0.0# : stJOG.cgY = 0.0#             ' �ړ���X,Y
            End If

            stJOG.Flg = -1
            InpKey = 0
            stJOG.Md = MODE_STG
            stJOG.Md = iGlobalJogMode

            Call Init_Proc(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' ���݂̈ʒu��\������(÷���ޯ���̔w�i�F��������(���F)�ɐݒ肷��)
            Call DispPosition(stJOG, 1)
            'Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            'Call Me.Focus()                                     ' �t�H�[�J�X��ݒ肷��(�e���L�[���͂̂���)
            ''                                                   ' KeyPreview�v���p�e�B��True�ɂ���ƑS�ẴL�[�C�x���g���܂��t�H�[�����󂯎��悤�ɂȂ�B
            '---------------------------------------------------------------------------
            '   �R���\�[���{�^�����̓R���\�[���L�[����̃L�[���͏������s��
            '---------------------------------------------------------------------------
            Do
                ' �V�X�e���G���[�`�F�b�N
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then GoTo STP_END

                stJOG.Md = iGlobalJogMode

                ' ���b�Z�[�W�|���v
                '  ��VB.NET�̓}���`�X���b�h�Ή��Ȃ̂ŁA�{���̓C�x���g�̊J���ȂǂłȂ��A
                '    �X���b�h�𐶐����ăR�[�f�B���O������̂��������B
                '    �X���b�h�łȂ��Ă��A�Œ�Ń^�C�}�[�𗘗p����B
                System.Windows.Forms.Application.DoEvents()
                System.Threading.Thread.Sleep(10)               ' CPU�g�p���������邽�߃X���[�v

                '----- ###232�� -----

                '----- ###232�� -----

                ' �R���\�[���{�^�����̓R���\�[���L�[����̃L�[����
                Call ReadConsoleSw(stJOG, cin)                  ' �L�[����

                '-----------------------------------------------------------------------
                '   ���̓L�[�`�F�b�N
                '-----------------------------------------------------------------------
                If (cin And CONSOLE_SW_RESET) Then              ' RESET SW ?
                    ' RESET SW������
                    If (stJOG.Opt And CONSOLE_SW_RESET) Then    ' RESET�L�[�L�� ?
                        r = cFRS_ERR_RST                        ' Return�l = Cancel(RESET��)
                        Exit Do
                    End If

                    ' HALT SW������
                ElseIf (cin And CONSOLE_SW_HALT) Then           ' HALT SW ?
                    If (stJOG.Opt And CONSOLE_SW_HALT) Then     ' �I�v�V����(0:HALT�L�[����, 1:HALT�L�[�L��)
                        r = cFRS_ERR_HALT                       ' Return�l = HALT��
                        Exit Do
                    End If

                    ' START SW������
                ElseIf (cin And CONSOLE_SW_START) Then          ' START SW ?
                    If (stJOG.Opt And CONSOLE_SW_START) Then    ' START�L�[�L�� ?
                        'stJOG.PosX = mvx                       ' �ʒuX,Y�X�V
                        'stJOG.PosY = mvy
                        r = cFRS_ERR_START                      ' Return�l = OK(START��) 
                        Exit Do
                    End If

                    ' Z SW��ON����OFF(����OFF����ON)�ɐؑւ������
                ElseIf (stJOG.bZ <> bZ) Then
                    If (stJOG.Opt And CONSOLE_SW_ZSW) Then      ' Z�L�[�L�� ?
                        r = cFRS_ERR_Z                          ' Return�l = Z��ON/OFF
                        stJOG.bZ = bZ                           ' ON/OFF
                        Exit Do
                    End If
                    ' ���SW������
                ElseIf cin And &H1E00US Then                    ' ���SW
                    If cin And &H100US Then                     ' HI SW ? 
                        mPIT = dblTchMoval(IDX_HPT)             ' mPIT = �ړ������߯�
                    Else
                        mPIT = dblTchMoval(IDX_PIT)             ' mPIT = �ړ��ʏ��߯�
                    End If

                    ' XY�e�[�u����Βl�ړ�(�\�t�g���~�b�g�`�F�b�N�L��)
                    r = cFRS_NORMAL
                    'If (stJOG.Md = MODE_STG) Then                ' ���[�h = XY�e�[�u���ړ� ?
                    If (iGlobalJogMode = MODE_STG) Then                ' ���[�h = XY�e�[�u���ړ� ?
                        ' XY�e�[�u����Βl�ړ�
                        r = Sub_XYtableMove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then              ' �װ ?
                            If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                GoTo STP_END                    ' ����ЯĴװ�ȊO�ʹװ����
                            End If
                        End If
                    ElseIf (iGlobalJogMode = MODE_Z) Then             ' Z��Βl�ړ�
                        '                        If (cin And &H200) Or (cin And &H400)) Then    ' ����/���� ���́uZ�{�^����OFF�v���͖���
                        'V6.0.0.0�G                        If (cin And &H200) Or (cin And &H400) Or (giJogButtonEnable = 0) Then    ' ����/���� ���́uZ�{�^����OFF�v���͖���
                        If ((cin And &H200) <> &H0) OrElse ((cin And &H400) <> &H0) OrElse
                            ((cin And CtrlJog.MouseClickLocation.Move) <> &H0) OrElse (giJogButtonEnable = 0) Then
                            ' ����/���� ���́uZ�{�^����OFF�v���͖���      'V6.0.0.0�G �}�E�X�N���b�N��������
                        Else
                            If cin And &H800 Then                               ' ���� ?
                                z = -1 * mPIT
                            ElseIf cin And &H1000 Then                          ' ���� ?
                                z = mPIT
                            End If

                            ' �\�t�g���~�b�g�`�F�b�N
                            If ((CleaningPosZ + z) >= SysPrm.stDEV.gfSminMaxZ2) Then
                                'If (SysPrm.stTMN.giMsgTyp = 0) Then
                                '    strMSG = "Z���\�t�g���~�b�g "
                                'Else
                                '    strMSG = "Z AXIS Software LIMIT "
                                'End If
                                strMSG = Globals_Renamed_008
                                Call Form1.System1.TrmMsgBox(SysPrm, strMSG, vbOKOnly, "Z Low Limit")
                                Call ZCONRST()                                    ' �ݿ�ٷ�ׯ�����
                                JogEzMoveWithZ = ERR_SOFTLIMIT_Z                    ' Return�l =Z���\�t�g���~�b�g
                                Exit Function
                            End If

                            ' 0�ȏ�@
                            If ((CleaningPosZ + z) <= 0) Then
                                'If (SysPrm.stTMN.giMsgTyp = 0) Then
                                '    strMSG = "Z�� �\�t�g���~�b�g "
                                'Else
                                '    strMSG = "Z AXIS Software LIMIT "
                                'End If
                                strMSG = Globals_Renamed_008
                                Call Form1.System1.TrmMsgBox(SysPrm, strMSG, vbOKOnly, "Z High Limit")
                                Call ZCONRST()                                    ' �ݿ�ٷ�ׯ�����
                                JogEzMoveWithZ = ERR_SOFTLIMIT_Z                    ' Return�l =Z���\�t�g���~�b�g
                                Exit Function
                            End If

                            CleaningPosZ = CleaningPosZ + z
                            r = EX_ZMOVE(CleaningPosZ, MOVE_ABSOLUTE)

                            If (r = cFRS_NORMAL) Then                           ' ���� ?
                            End If
                            If (r <> cFRS_NORMAL) Then                          ' �G���[ ?
                                Exit Function
                            End If
                            'V6.0.0.0�J                            Call frmProbeCleaning.DispProbePos(CleaningPosZ)                 ' �ʒu�\��(Z)
                            DispProbePos.Invoke(CleaningPosZ)                   ' �ʒu�\��(Z)     'V6.0.0.0�J
                            'V6.0.0.0�J                            stJogTextZ.Text = Format(CleaningPosZ, "0.000")
                        End If

                        '  ���[�h = BP�ړ��̏ꍇ
                    ElseIf (iGlobalJogMode = MODE_BP) Then
                        ' BP��Βl�ړ�
                        r = Sub_BPmove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then              ' BP�ړ��G���[ ?
                            If (Form1.System1.IsSoftLimitBP(r) = False) Then
                                GoTo STP_END                    ' ����ЯĴװ�ȊO�ʹװ����
                            End If
                        End If
                    End If

                    ' �\�t�g���~�b�g�G���[�̏ꍇ�� HI SW�ȊO��OFF����
                    If (r <> cFRS_NORMAL) Then                  ' �װ ?
                        If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then
                            InpKey = cBIT_HI                    ' HI SW ON
                        Else
                            InpKey = 0                          ' HI SW�ȊO��OFF
                        End If
                        stJOG.ResetButtonStyle()                'V6.0.0.0-22
                    End If

                    ' ���݂̈ʒu��\������
                    Call DispPosition(stJOG, 1)
                    Call Form1.System1.WAIT(SysPrm.stSYP.gPitPause)    ' Wait(sec)

                    InpKey = CType(CtrlJog.MouseClickLocation.Clear(InpKey), UShort)    'V6.0.0.0�G
                    stJOG.KeyDown = Keys.None                                           'V6.0.0.0-23
                End If

            Loop While (stJOG.Flg = -1)

            '---------------------------------------------------------------------------
            '   �I������
            '---------------------------------------------------------------------------
            ' ���W�\���p÷���ޯ���̔w�i�F�𔒐F�ɐݒ肷��
            Call DispPosition(stJOG, 0)

            ' �e��ʂ���OK/Cancel���݉��� ?
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            ' OK(START��)�Ȃ�ʒuX,Y�X�V
            If (r = cFRS_ERR_START) Then                            ' OK(START��) ?
                stJOG.PosX = mvx                                    ' �ʒuX,Y�X�V
                stJOG.PosY = mvy
            End If

STP_END:
            Call ZCONRST()                                          ' �ݿ�ٷ�ׯ����� 
            Return (r)                                              ' Return�l�ݒ� 

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "Globals.JogEzMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[ 

        Finally
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
        End Try
    End Function

#End Region

    ''' <summary>
    ''' ���b�g��~�̏�����ǂݍ���
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadLotStopData() As Integer

        'V4.9.0.0�@
        If giNgStop = 0 Then
            Return 0
        End If

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "TIMMING_PLATE", JudgeNgRate.CheckTimmingPlate)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "TIMMING_BLOCK", JudgeNgRate.CheckTimmingBlock)

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_YIELD", JudgeNgRate.CheckYeld)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_OVERRANGE", JudgeNgRate.CheckOverRange)

        JudgeNgRate.ValYield = Val(GetPrivateProfileString_S("JUDGE", "TEXT_YIELD", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValOverRange = Val(GetPrivateProfileString_S("JUDGE", "TEXT_OVERRANGE", LOT_COND_FILENAME, "0.0"))

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "JUDGE_UNIT", JudgeNgRate.SelectUnit)

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_ITHI", JudgeNgRate.CheckITHI)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_ITLO", JudgeNgRate.CheckITLO)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_FTHI", JudgeNgRate.CheckFTHI)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_FTLO", JudgeNgRate.CheckFTLO)

        JudgeNgRate.ValITHI = Val(GetPrivateProfileString_S("JUDGE", "TEXT_ITHI", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValITLO = Val(GetPrivateProfileString_S("JUDGE", "TEXT_ITLO", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValFTHI = Val(GetPrivateProfileString_S("JUDGE", "TEXT_FTHI", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValFTLO = Val(GetPrivateProfileString_S("JUDGE", "TEXT_FTLO", LOT_COND_FILENAME, "0.0"))

    End Function

    ''' <summary>
    ''' XY�X�e�[�W�̑��x��؂�ւ��� 
    ''' </summary>mode:0:�ʏ푬�x�A1:�X�e�b�v�����s�[�g�����x
    ''' <returns></returns>
    Public Function SetXYStageSpeed(mode As Integer) As Integer
        Dim Axis As Integer
        Dim strSECT As String
        Dim strKEY As String
        Dim SpeedMode As Integer
        Dim r2 As Integer

        SpeedMode = mode

        ' X,Y��PCL�p�����[�^���V�X�p�����ݒ肷�遨���x�؂�ւ���W���Ƃ��� 
        'X���p�����[�^�̎擾
        strSECT = "PCL_PRM_0"
        Axis = 0
        strKEY = "FL_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FL = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "400"))
        strKEY = "FH_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FH = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "3000"))
        strKEY = "DRVRAT_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).DrvRat = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "65"))
        strKEY = "MAGNIF_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).Magnif = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "5"))

        'Y���p�����[�^�̎擾
        strSECT = "PCL_PRM_1"
        Axis = 1
        strKEY = "FL_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FL = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "800"))
        strKEY = "FH_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FH = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "15000"))
        strKEY = "DRVRAT_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).DrvRat = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "4"))
        strKEY = "MAGNIF_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).Magnif = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "29"))

        ' X,Y�����x��ʏ푬�x�ɐ؂�ւ���
        r2 = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, SpeedMode).FL, stPclAxisPrm(AXIS_X, SpeedMode).FH, stPclAxisPrm(AXIS_X, SpeedMode).DrvRat, stPclAxisPrm(AXIS_X, SpeedMode).Magnif)
        r2 = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, SpeedMode).FL, stPclAxisPrm(AXIS_Y, SpeedMode).FH, stPclAxisPrm(AXIS_Y, SpeedMode).DrvRat, stPclAxisPrm(AXIS_Y, SpeedMode).Magnif)
        r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' �G���[�Ȃ�A�v�������I��(���b�Z�[�W�\���ς�)

        Return r2

    End Function


End Module