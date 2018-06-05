


'===============================================================================
'   Description  : �g���~���O�f�[�^�`����`�Ƃh�n����
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module DataAccess
#Region "�萔��`"
    '---------------------------------------------------------------------------
    '   ��R�f�[�^�̖ڕW�l�w��(0:��Βl,1:���V�I,2:�v�Z��, 3�`9:�������V�I)
    '---------------------------------------------------------------------------
    Public Const TARGET_TYPE_ABSOLUTE As Short = 0
    Public Const TARGET_TYPE_RATIO As Short = 1
    Public Const TARGET_TYPE_CALC As Short = 2
    Public Const TARGET_TYPE_CUSRTO_3 As Short = 3
    Public Const TARGET_TYPE_CUSRTO_4 As Short = 4
    Public Const TARGET_TYPE_CUSRTO_5 As Short = 5
    Public Const TARGET_TYPE_CUSRTO_6 As Short = 6
    Public Const TARGET_TYPE_CUSRTO_7 As Short = 7
    Public Const TARGET_TYPE_CUSRTO_8 As Short = 8
    Public Const TARGET_TYPE_CUSRTO_9 As Short = 9

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
    '---------------------------------------------------------------------------
    '   �J�b�g�f�[�^�̃J�b�g�`���`
    '---------------------------------------------------------------------------
    Public Const CNS_CUTP_ST As String = "1"                ' ST�J�b�g
    Public Const CNS_CUTP_L As String = "2"                 ' L�J�b�g
    Public Const CNS_CUTP_HK As String = "3"                ' HOOK�J�b�g
    Public Const CNS_CUTP_IX As String = "4"                ' IX�J�b�g
    Public Const CNS_CUTP_SC As String = "5"                ' �X�L�����J�b�g
    Public Const CNS_CUTP_STr As String = "6"               ' ST�J�b�g(���^�[��)
    Public Const CNS_CUTP_Lr As String = "7"                ' L�J�b�g(���^�[��)
    Public Const CNS_CUTP_STt As String = "8"               ' ST�J�b�g(���g���[�X)
    Public Const CNS_CUTP_Lt As String = "9"                ' L�J�b�g(���g���[�X)
    Public Const CNS_CUTP_NST As String = "A"               ' �΂�ST�J�b�g
    Public Const CNS_CUTP_NL As String = "B"                ' �΂�L�J�b�g
    Public Const CNS_CUTP_NSTr As String = "C"              ' �΂�ST�J�b�g(���^�[��)
    Public Const CNS_CUTP_NLr As String = "D"               ' �΂�L�J�b�g(���^�[��)
    Public Const CNS_CUTP_NSTt As String = "E"              ' �΂�ST�J�b�g(���g���[�X)
    Public Const CNS_CUTP_NLt As String = "F"               ' �΂�L�J�b�g(���g���[�X)
    Public Const CNS_CUTP_C As String = "G"                 ' C�J�b�g
    Public Const CNS_CUTP_U As String = "H"                 ' U�J�b�g
    Public Const CNS_CUTP_Ut As String = "I"                ' U�J�b�g(���g���[�X) V1.22.0.0�@
    Public Const CNS_CUTP_ES As String = "K"                ' ES�J�b�g
    Public Const CNS_CUTP_M As String = "M"                 ' �����}�[�L���O 
    Public Const CNS_CUTP_ES2 As String = "S"               ' ES2�J�b�g
    Public Const CNS_CUTP_ST2 As String = "T"               ' �߼޼��ݸޖ���ST�J�b�g
    Public Const CNS_CUTP_IX2 As String = "X"               ' �߼޼��ݸޖ������ޯ��
    Public Const CNS_CUTP_NOP As String = "Z"               ' NO CUT

    '---------------------------------------------------------------------------
    '   �J�b�g�f�[�^�̑��胂�[�h(IX�p)��`  2011.08.30
    '---------------------------------------------------------------------------
    Public Const MEASMODE_REG As Short = 0                  ' ��R����
    Public Const MEASMODE_VOL As Short = 1                  ' �d������
    Public Const MEASMODE_EXT As Short = 2                  ' �O������

    '---------------------------------------------------------------------------
    '   ��ڰ��ް���÷���ޯ���z��ԍ�
    '---------------------------------------------------------------------------
    Public Const glNo_DataNo As Short = 0                   '�ް�no.
    Public Const glNo_MeasType As Short = 200               '�g�������[�h
    Public Const glNo_DirStepRepeat As Short = 1            '�ï�߁���߰�
    Public Const glNo_PlateCntXDir As Short = 96            'X�����̃v���[�g��
    Public Const glNo_PlateCntYDir As Short = 97            'Y�����̃v���[�g��
    Public Const glNo_BlockCntXDir As Short = 2             '��ۯ���X
    Public Const glNo_BlockCntYDir As Short = 3             '��ۯ���Y
    'Public Const glPlateInvXDir    As Integer = 1          'X�����̃v���[�g�Ԋu
    'Public Const glPlateInvYDir    As Integer = 1          'Y�����̃v���[�g�Ԋu
    Public Const glNo_TableOffSetXDir As Short = 4          'ð��وʒu�̾��X
    Public Const glNo_TableOffSetYDir As Short = 5          'ð��وʒu�̾��Y
    Public Const glNo_BpOffsetXDir As Short = 6             '�ްшʒu�̾��X
    Public Const glNo_BpOffsetYDir As Short = 7             '�ްшʒu�̾��Y
    Public Const glNo_AdjOffsetXDir As Short = 8            '��ެ�Ĉʒu�̾��X
    Public Const glNo_AdjOffsetYDir As Short = 9            '��ެ�Ĉʒu�̾��Y
    Public Const glNo_NGMark As Short = 10                  'NGϰ�ݸ�
    Public Const glNo_DelayTrim As Short = 11               '�ިڲ���
    Public Const glNo_NgJudgeUnit As Short = 12             'NG����P��
    Public Const glNo_NgJudgeLevel As Short = 13            'NG����
    Public Const glNo_ZOffSet As Short = 14                 '��۰��Z�̾��
    Public Const glNo_ZStepUpDist As Short = 15             '��۰�޽ï�ߏ㏸����
    Public Const glNo_ZWaitOffset As Short = 16             '��۰�ޑҋ@Z�̾��
    '
    Public Const glNo_ResistDir As Short = 17               '��R���ѕ���
    Public Const glNo_CurcuitCnt As Short = 18              '1��ٰ�ߓ���R��
    Public Const glNo_ResistCntInGroup As Short = 102       '1��ٰ�ߓ��T�[�L�b�g��
    Public Const glNo_GroupCntInBlockXBp As Short = 19      '��ۯ����ٰ�ߐ�X
    Public Const glNo_GroupCntInBlockYStage As Short = 20   '��ۯ����ٰ�ߐ�Y
    Public Const glNo_GroupItvXDir As Short = 21            '��ٰ�ߊԊuX
    Public Const glNo_GroupItvYDir As Short = 22            '��ٰ�ߊԊuY
    Public Const glNo_CircuitSizeXDir As Short = 103        '�T�[�L�b�g����X
    Public Const glNo_CircuitSizeYDir As Short = 104        '�T�[�L�b�g����Y
    Public Const glNo_ChipSizeXDir As Short = 23            '���߻���X
    Public Const glNo_ChipSizeYDir As Short = 24            '���߻���Y
    Public Const glNo_StepOffsetXDir As Short = 25          '�ï�ߵ̾�ė�X
    Public Const glNo_StepOffsetYDir As Short = 26          '�ï�ߵ̾�ė�Y
    Public Const glNo_BlockSizeReviseXDir As Short = 27     '��ۯ����ޕ␳X
    Public Const glNo_BlockSizeReviseYDir As Short = 28     '��ۯ����ޕ␳Y
    Public Const glNo_BlockItvXDir As Short = 29            '��ۯ��ԊuX
    Public Const glNo_BlockItvYDir As Short = 30            '��ۯ��ԊuY
    Public Const glNo_ContHiNgBlockCnt As Short = 31        '�A��NG-HIGH��R��ۯ���
    Public Const glNo_DistStepRepeat As Short = 32          '�ï��&��߰Ĉړ��ʁ@���g�p
    '
    Public Const glNo_ReviseMode As Short = 33              '�␳Ӱ��
    Public Const glNo_ManualReviseType As Short = 34        '�␳���@
    Public Const glNo_ReviseCordnt1XDir As Short = 35       '�␳�ʒu���W1X
    Public Const glNo_ReviseCordnt1YDir As Short = 36       '�␳�ʒu���W1Y
    Public Const glNo_ReviseCordnt2XDir As Short = 37       '�␳�ʒu���W2X
    Public Const glNo_ReviseCordnt2YDir As Short = 38       '�␳�ʒu���W2Y
    Public Const glNo_ReviseOffsetXDir As Short = 39        '�␳�߼޼�ݵ̾��X
    Public Const glNo_ReviseOffsetYDir As Short = 40        '�␳�߼޼�ݵ̾��Y
    Public Const glNo_RecogDispMode As Short = 41           '�F���ް��\��Ӱ��
    Public Const glNo_PixelValXDir As Short = 42            '�߸�ْlX
    Public Const glNo_PixelValYDir As Short = 43            '�߸�ْlY
    Public Const glNo_RevisePtnNo1 As Short = 44            '�␳�ʒu�����No1
    Public Const glNo_RevisePtnNo2 As Short = 45            '�␳�ʒu�����No2
    Public Const glNo_RevisePtnGrpNo1 As Short = 98         '�␳�ʒu�����No1�O���[�vNo
    Public Const glNo_RevisePtnGrpNo2 As Short = 99         '�␳�ʒu�����No2�O���[�vNo
    Public Const glNo_RotateXDir As Short = 46              'X�����̉�]���S
    Public Const glNo_RotateYDir As Short = 47              'Y�����̉�]���S
    Public Const glNo_RotateTheta As Short = 48             '�Ǝ� 

    Public Const glNo_TThetaOffset As Short = 91            '�s�ƃI�t�Z�b�g�ideg�j
    Public Const glNo_TThetaBase1XDir As Short = 92         '�s�Ɗ�ʒu�PX�imm�j
    Public Const glNo_TThetaBase1YDir As Short = 93         '�s�Ɗ�ʒu�PY�imm�j
    Public Const glNo_TThetaBase2XDir As Short = 94         '�s�Ɗ�ʒu�QX�imm�j
    Public Const glNo_TThetaBase2YDir As Short = 95         '�s�Ɗ�ʒu�QY�imm�j

    Public Const glNo_CaribBaseCordnt1XDir As Short = 49    '�����ڰ��݊���W1X
    Public Const glNo_CaribBaseCordnt1YDir As Short = 50    '�����ڰ��݊���W1Y
    Public Const glNo_CaribBaseCordnt2XDir As Short = 51    '�����ڰ��݊���W2X
    Public Const glNo_CaribBaseCordnt2YDir As Short = 52    '�����ڰ��݊���W2Y
    Public Const glNo_CaribTableOffsetXDir As Short = 53    '�����ڰ���ð��ٵ̾��X
    Public Const glNo_CaribTableOffsetYDir As Short = 54    '�����ڰ���ð��ٵ̾��Y
    Public Const glNo_CaribPtnNo1 As Short = 55             '�����ڰ�������ݓo�^No1
    Public Const glNo_CaribPtnNo2 As Short = 56             '�����ڰ�������ݓo�^No2
    Public Const glNo_CaribPtnNo1GroupNo As Short = 100     '�����ڰ�������ݓo�^No1�O���[�vNo
    Public Const glNo_CaribPtnNo2GroupNo As Short = 101     '�����ڰ�������ݓo�^No2�O���[�vNo
    Public Const glNo_CaribCutLength As Short = 57          '�����ڰ��ݶ�Ē�
    Public Const glNo_CaribCutSpeed As Short = 58           '�����ڰ��ݶ�đ��x
    Public Const glNo_CaribCutQRate As Short = 59           '�����ڰ���ڰ��Qڰ�
    Public Const glNo_CutPosiReviseOffsetXDir As Short = 60 '��ē_�␳ð��ٵ̾��X
    Public Const glNo_CutPosiReviseOffsetYDir As Short = 61 '��ē_�␳ð��ٵ̾��Y
    Public Const glNo_CutPosiRevisePtnNo As Short = 62      '��ē_�␳����ݓo�^No
    Public Const glNo_CutPosiReviseCutLength As Short = 63  '��ē_�␳��Ē�
    Public Const glNo_CutPosiReviseCutSpeed As Short = 64   '��ē_�␳��đ��x
    Public Const glNo_CutPosiReviseCutQRate As Short = 65   '��ē_�␳ڰ��Qڰ�
    Public Const glNo_CutPosiReviseGroupNo As Short = 66    '��ٰ��No

    '�ȉ���436K�p�̃p�����[�^�ׁ̈A��U�R�����g�A�E�g
    'Public Const glNo_MaxTrimNgCount            As Integer = 77 '���ݸ�NG����(���)
    'Public Const glNo_MaxBreakDischargeCount    As Integer = 78 '���ꌇ���r�o����(���)
    'Public Const glNo_TrimNgCount               As Integer = 79 '�A�����ݸ�NG����
    'Public Const glNo_InitialOkTestDo           As Integer = 84 '�Ƽ��OKý�
    'Public Const glNo_WorkSetByLoader           As Integer = 85 '��i��

    '�I�v�V�����@�\
    Public Const glNo_RetryProbeCount As Short = 67         '����۰��ݸމ�
    Public Const glNo_RetryProbeDistance As Short = 68      '����۰��ݸވړ���
    Public Const glNo_LedCtrl As Short = 69                 'LED����

    Public Const glNo_GpibMode As Short = 70                'GP-IB����(ROHM)
    Public Const glNo_GpibDelim As Short = 71               '�����ݒ�(�����)       
    Public Const glNo_GpibTimeOut As Short = 72             '�����ݒ�(��ѱ��)      
    Public Const glNo_GpibAddress As Short = 73             '�����ݒ�(�@����ڽ)   
    Public Const glNo_GpibInitCmnd As Short = 74            '�����������            
    Public Const glNo_GpibInit2Cmnd As Short = 75           '�����������            
    Public Const glNo_GpibTrigCmnd As Short = 76            '�ض޺����            

    Public Const glNo_Gpib2Mode As Short = 87               'GP-IB����
    Public Const glNo_Gpib2Address As Short = 88            '�����ݒ�(�@����ڽ)
    Public Const glNo_Gpib2MeasSpeed As Short = 89          '���葬�x
    Public Const glNo_Gpib2MeasMode As Short = 90           '���胂�[�h

    Public Const glNo_PowerAdjustMode As Short = 80         '��ܰ����Ӱ��
    Public Const glNo_PowerAdjustTarget As Short = 81       '�����ڕW��ܰ
    Public Const glNo_PowerAdjustQRate As Short = 82        '��ܰ����Qڰ�
    Public Const glNo_PowerAdjustToleLevel As Short = 83    '��ܰ�������e�͈�
    Public Const glNo_OpenCheck As Short = 86               '4�[�q���������
#End Region

#Region "�g���~���O�f�[�^�`����`"
    '--------------------------------------------------------------------------
    '   �v���[�g�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure PlateInfo
        '----- �v���[�g�f�[�^�P -----
        Dim strDataName As String                           ' �g���~���O�f�[�^��  �@�@�@�@���ύX(U�J�b�g�f�[�^�̓J�b�g�f�[�^�Ɉړ�)     
        Dim intMeasType As Short                            ' �g�������[�h(0:��R ,1:�d��)�����g�p ��R�f�[�^�̑��胂�[�h(0:��R ,1:�d�� ,2:�O��)�ֈړ� 
        Dim intDirStepRepeat As Short                       ' �ï�߁���߰�
        Dim intPlateCntXDir As Short                        ' �v���[�g��X 
        Dim intPlateCntYDir As Short                        ' �v���[�g��Y 
        Dim intBlockCntXDir As Short                        ' ��ۯ����w
        Dim intBlockCntYDir As Short                        ' ��ۯ����x
        Dim dblPlateItvXDir As Double                       ' �v���[�g�Ԋu�w
        Dim dblPlateItvYDir As Double                       ' �v���[�g�Ԋu�x
        Dim dblBlockSizeXDir As Double                      ' �u���b�N�T�C�Y�w   
        Dim dblBlockSizeYDir As Double                      ' �u���b�N�T�C�Y�x   
        Dim dblTableOffsetXDir As Double                    ' ð��وʒu�̾��X
        Dim dblTableOffsetYDir As Double                    ' ð��وʒu�̾��Y
        Dim dblBpOffSetXDir As Double                       ' �ްшʒu�̾��X
        Dim dblBpOffSetYDir As Double                       ' �ްшʒu�̾��Y
        Dim dblAdjOffSetXDir As Double                      ' ��ެ�Ĉʒu�̾��X(���g�p)
        Dim dblAdjOffSetYDir As Double                      ' ��ެ�Ĉʒu�̾��Y(���g�p)
        Dim intCurcuitCnt As Short                          ' �T�[�L�b�g��  
        Dim intNGMark As Short                              ' NGϰ�ݸ�
        Dim intDelayTrim As Short                           ' �ިڲ���
        Dim intNgJudgeUnit As Short                         ' NG����P��
        Dim intNgJudgeLevel As Short                        ' NG����
        Dim dblZOffSet As Double                            ' ��۰��Z�̾��(ZON�ʒu)
        Dim dblZStepUpDist As Double                        ' ��۰�޽ï�ߏ㏸����(Z���ï��&��߰Ĉʒu)
        Dim dblZWaitOffset As Double                        ' ��۰�ޑҋ@�ʒu(ZOFF�ʒu)
        '----- V1.13.0.0�A�� -----
        Dim dblLwPrbStpDwDist As Double                     ' �����v���[�u�X�e�b�v���~����
        Dim dblLwPrbStpUpDist As Double                     ' �����v���[�u�X�e�b�v�㏸����
        '----- V1.13.0.0�A�� -----
        Dim intPrbRetryCount As Short                       ' �v���[�u���g���C��(0=���g���C�Ȃ�)(TKY���݂̂Ŏg�pINtime���ւ͑��M���Ȃ�)
        Dim intFinalJudge As Short                          ' �t�@�C�i������(option)   
        Dim intAttLevel As Short                            ' �A�b�e�l�[�^(%)(option)    
        '----- V1.23.0.0�F�� -----
        ' �v���[�g�P�^�u�̃v���[�u�`�F�b�N����(�I�v�V����)
        Dim intPrbChkPlt As Short                           ' ����
        Dim intPrbChkBlk As Short                           ' �u���b�N
        Dim dblPrbTestLimit As Double                       ' �덷�}%
        '----- V1.23.0.0�F�� -----

        '----- �v���[�g�f�[�^�R -----
        Dim intResistDir As Short                           ' ��R���ѕ���
        Dim intCircuitCntInBlock As Short                   ' 1��ۯ��໰��Đ�
        Dim dblCircuitSizeXDir As Double                    ' ����Ļ���X         
        Dim dblCircuitSizeYDir As Double                    ' ����Ļ���Y   
        Dim intResistCntInBlock As Short                    ' 1�u���b�N����R��
        Dim intResistCntInGroup As Short                    ' 1�O���[�v����R��
        Dim intGroupCntInBlockXBp As Short                  ' �u���b�N���a�o�O���[�v��(�T�[�L�b�g��)
        Dim intGroupCntInBlockYStage As Short               ' �u���b�N���X�e�[�W�O���[�v��
        Dim dblGroupItvXDir As Double                       ' ��ٰ�ߊԊuX���i2010/11/09)dblBpGrpItv��
        Dim dblGroupItvYDir As Double                       ' ��ٰ�ߊԊuY���i2010/11/09)dblStgGrpItv��
        Dim dblChipSizeXDir As Double                       ' ���߻���X
        Dim dblChipSizeYDir As Double                       ' ���߻���Y
        '                                                   ' �璹�X�e�b�v���ɑΉ��B�O���[�v�����Ń`�b�v�T�C�Y���X�e�b�v�ړ�����ꍇ�Ɏg�p����
        Dim intChipStepCnt As Short                         ' �O���[�v���̃`�b�v�X�e�b�v���B ���ǉ�
        Dim dblChipStepItv As Double                        ' �O���[�v���`�b�v�X�e�b�v���̃X�e�b�v�Ԋu�i��{�̓`�b�v�T�C�Y�j ���ǉ�
        Dim dblStepOffsetXDir As Double                     ' �ï�ߵ̾�ė�X
        Dim dblStepOffsetYDir As Double                     ' �ï�ߵ̾�ė�Y
        Dim dblBlockSizeReviseXDir As Double                ' ��ۯ����ޕ␳X��(2010/11/11)CHIP���g�p�BNET�ł̓u���b�N�T�C�Y�Ƃ��Ďg�p���Ă��邪�g�p���@���Č����̏�폜����B
        Dim dblBlockSizeReviseYDir As Double                ' ��ۯ����ޕ␳Y��(2010/11/11)CHIP���g�p�BNET�ł̓u���b�N�T�C�Y�Ƃ��Ďg�p���@���Č����̏�폜����B
        Dim dblBlockItvXDir As Double                       ' ��ۯ��ԊuX
        Dim dblBlockItvYDir As Double                       ' ��ۯ��ԊuY
        Dim intContHiNgBlockCnt As Short                    ' �A��NG-HIGH��R��ۯ���
        'Dim intDistStepRepeat   As Double                  ' �ï��&��߰Ĉړ��� �� ���g�p

        '(2010/11/09)
        '   �ꎞ�I�Ƀf�[�^�ǉ�-�u���b�N�̍l������V�K�ɍ��킹�邽�߁A
        '   BP�O���[�v�Ԋu�ƃX�e�[�W�O���[�v�Ԋu�̕ϐ���ǉ�
        Dim dblBpGrpItv As Double                           ' BP�O���[�v�Ԋu�i�ȑO��CHIP�̃O���[�v�Ԋu�j
        Dim dblPlateSizeX As Double                         ' �v���[�g�T�C�YX
        Dim dblPlateSizeY As Double                         ' �v���[�g�T�C�YY
        Dim intBpGrpCntInBlk As Short                       ' �u���b�N��BP�O���[�v��
        Dim dblStgGrpItvX As Double                         ' X�����̃X�e�[�W�O���[�v�Ԋu�i�ȑO�̂b�g�h�o�̃X�e�b�v�ԃC���^�[�o���j
        Dim dblStgGrpItvY As Double                         ' Y�����̃X�e�[�W�O���[�v�Ԋu�i�ȑO�̂b�g�h�o�̃X�e�b�v�ԃC���^�[�o���j
        Dim intBlkCntInStgGrpX As Short                     ' X�����̃X�e�[�W�O���[�v���u���b�N��
        Dim intBlkCntInStgGrpY As Short                     ' Y�����̃X�e�[�W�O���[�v���u���b�N��
        Dim intStgGrpCntInPlt As Short                      ' �v���[�g���X�e�[�W�O���[�v�����s�v����

        '----- �v���[�g�f�[�^�Q -----
        Dim intReviseMode As Short                          ' �␳���[�h(0:����,1:�蓮, 2:����+����)
        Dim intManualReviseType As Short                    ' �␳���@(0:�␳�Ȃ�, 1:1��̂�, 2:����)
        Dim dblReviseCordnt1XDir As Double                  ' �␳�ʒu���W1X
        Dim dblReviseCordnt1YDir As Double                  ' �␳�ʒu���W1Y
        Dim dblReviseCordnt2XDir As Double                  ' �␳�ʒu���W2X
        Dim dblReviseCordnt2YDir As Double                  ' �␳�ʒu���W2Y
        Dim dblReviseOffsetXDir As Double                   ' �␳�߼޼�ݵ̾��X
        Dim dblReviseOffsetYDir As Double                   ' �␳�߼޼�ݵ̾��Y
        Dim intRecogDispMode As Short                       ' �F���ް��\��Ӱ��(0:�\���Ȃ�, 1:�\������)
        Dim dblPixelValXDir As Double                       ' �߸�ْlX
        Dim dblPixelValYDir As Double                       ' �߸�ْlY
        Dim intRevisePtnNo1 As Short                        ' �␳�ʒu�����No1
        Dim intRevisePtnNo2 As Short                        ' �␳�ʒu�����No2
        Dim intRevisePtnNo1GroupNo As Short                 ' �␳�ʒu�����No1�O���[�vNo
        Dim intRevisePtnNo2GroupNo As Short                 ' �␳�ʒu�����No2�O���[�vNo
        Dim dblRotateXDir As Double                         ' X�����̉�]���S
        Dim dblRotateYDir As Double                         ' Y�����̉�]���S
        Dim dblRotateTheta As Double                        ' �Ɖ�]�p�x
        'V5.0.0.9�@                              �� �N�����v���X�E���t�A���C�����g�p
        Dim intReviseExecRgh As Short                       ' �␳�L��(0:�␳�Ȃ�, 1:�␳����)
        Dim dblReviseCordnt1XDirRgh As Double               ' �␳�ʒu���W1X
        Dim dblReviseCordnt1YDirRgh As Double               ' �␳�ʒu���W1Y
        Dim dblReviseCordnt2XDirRgh As Double               ' �␳�ʒu���W2X
        Dim dblReviseCordnt2YDirRgh As Double               ' �␳�ʒu���W2Y
        Dim dblReviseOffsetXDirRgh As Double                ' �␳�߼޼�ݵ̾��X
        Dim dblReviseOffsetYDirRgh As Double                ' �␳�߼޼�ݵ̾��Y
        Dim intRecogDispModeRgh As Short                    ' �F���ް��\��Ӱ��(0:�\���Ȃ�, 1:�\������)
        Dim intRevisePtnNo1Rgh As Short                     ' �␳�ʒu�����No1
        Dim intRevisePtnNo2Rgh As Short                     ' �␳�ʒu�����No2
        Dim intRevisePtnNo1GroupNoRgh As Short              ' �␳�ʒu�����No1�O���[�vNo
        Dim intRevisePtnNo2GroupNoRgh As Short              ' �␳�ʒu�����No2�O���[�vNo
        'V5.0.0.9�@                              ��

        '----- �v���[�g�f�[�^�S -----
        Dim dblCaribBaseCordnt1XDir As Double               ' �����ڰ��݊���W1X
        Dim dblCaribBaseCordnt1YDir As Double               ' �����ڰ��݊���W1Y
        Dim dblCaribBaseCordnt2XDir As Double               ' �����ڰ��݊���W2X
        Dim dblCaribBaseCordnt2YDir As Double               ' �����ڰ��݊���W2Y
        Dim dblCaribTableOffsetXDir As Double               ' �����ڰ���ð��ٵ̾��X
        Dim dblCaribTableOffsetYDir As Double               ' �����ڰ���ð��ٵ̾��Y
        Dim intCaribPtnNo1 As Short                         ' �����ڰ�������ݓo�^No1
        Dim intCaribPtnNo2 As Short                         ' �����ڰ�������ݓo�^No2
        Dim intCaribPtnNo1GroupNo As Short                  ' �����ڰ�������ݓo�^No1�O���[�vNo
        Dim intCaribPtnNo2GroupNo As Short                  ' �����ڰ�������ݓo�^No2�O���[�vNo
        Dim dblCaribCutLength As Double                     ' �����ڰ��ݶ�Ē�
        Dim dblCaribCutSpeed As Double                      ' �����ڰ��ݶ�đ��x
        Dim dblCaribCutQRate As Double                      ' �����ڰ���ڰ��Qڰ�
        Dim intCaribCutCondNo As Short                      ' �����ڰ��݉��H�����ԍ�(FL�p)

        Dim dblCutPosiReviseOffsetXDir As Double            ' ��ē_�␳ð��ٵ̾��X
        Dim dblCutPosiReviseOffsetYDir As Double            ' ��ē_�␳ð��ٵ̾��Y
        Dim intCutPosiRevisePtnNo As Short                  ' ��ē_�␳����ݓo�^No
        Dim dblCutPosiReviseCutLength As Double             ' ��ē_�␳��Ē�
        Dim dblCutPosiReviseCutSpeed As Double              ' ��ē_�␳��đ��x
        Dim dblCutPosiReviseCutQRate As Double              ' ��ē_�␳ڰ��Qڰ�
        Dim intCutPosiReviseGroupNo As Short                ' ��ٰ��No
        Dim intCutPosiReviseCondNo As Short                 ' �J�b�g�ʒu�␳���H�����ԍ�(FL�p)

        '----- �v���[�g�f�[�^�T -----
        Dim intMaxTrimNgCount As Short                      ' ���ݸ�NG����(���)
        Dim intMaxBreakDischargeCount As Short              ' ���ꌇ���r�o����(���)
        Dim intTrimNgCount As Short                         ' �A�����ݸ�NG����
        Dim intContHiNgResCnt As Short                      ' �A�����ݸ�NG��R�� ###230
        Dim intNgJudgeStop As Short                         ' NG���莞��~  V1.13.0.0�A

        Dim intRetryProbeCount As Short                     ' ����۰��ݸމ�
        Dim dblRetryProbeDistance As Double                 ' ����۰��ݸވړ���

        Dim intPowerAdjustMode As Short                     ' ��ܰ����Ӱ��
        Dim intPwrChkPltNum As Short                        ' �`�F�b�N����� V4.11.0.0�@
        Dim intPwrChkTime As Short                          ' �`�F�b�N����(��) V4.11.0.0�@
        Dim dblPowerAdjustTarget As Double                  ' �����ڕW��ܰ
        Dim dblPowerAdjustQRate As Double                   ' ��ܰ����Qڰ�
        Dim dblPowerAdjustToleLevel As Double               ' ��ܰ�������e�͈�
        Dim intPowerAdjustCondNo As Short                   ' ��ܰ�������H�����ԍ�(FL�p)

        Dim intInitialOkTestDo As Short                     ' �Ƽ��OKý�
        Dim intWorkSetByLoader As Short                     ' ��i��
        Dim intOpenCheck As Short                           ' 4�[�q���������
        Dim intLedCtrl As Short                             ' LED����
        Dim dblThetaAxis As Double                          ' �Ǝ�

        Dim intGpibCtrl As Short                            ' GP-IB����
        Dim intGpibDefDelimiter As Short                    ' �����ݒ�(�����)
        Dim intGpibDefTimiout As Short                      ' �����ݒ�(��ѱ��)
        Dim intGpibDefAdder As Short                        ' �����ݒ�(�@����ڽ)
        Dim strGpibInitCmnd1 As String                      ' �����������1
        Dim strGpibInitCmnd2 As String                      ' �����������2
        Dim strGpibTriggerCmnd As String                    ' �ض޺����
        Dim intGpibMeasSpeed As Short                       ' ���葬�x
        Dim intGpibMeasMode As Short                        ' ���胂�[�h

        Dim dblTThetaOffset As Double                       ' �s�ƃI�t�Z�b�g
        Dim dblTThetaBase1XDir As Double                    ' �s�Ɗ�ʒu�PX�imm�j
        Dim dblTThetaBase1YDir As Double                    ' �s�Ɗ�ʒu�PY�imm�j
        Dim dblTThetaBase2XDir As Double                    ' �s�Ɗ�ʒu�QX�imm�j
        Dim dblTThetaBase2YDir As Double                    ' �s�Ɗ�ʒu�QY�imm�j

        '----- �v���[�g�f�[�^�U (��ڰ����3(V1.13.0.0�A)) -----
        Dim intContExpMode As Short                         ' �L�k�␳ (0:�Ȃ�, 1:����)
        Dim intContExpGrpNo As Short                        ' �L�k�␳��ٰ�ߔԍ�
        Dim intContExpPtnNo As Short                        ' �L�k�␳����ݔԍ�
        Dim dblContExpPosX As Double                        ' �L�k�␳�ʒuX (mm)
        Dim dblContExpPosY As Double                        ' �L�k�␳�ʒuXY (mm)            
        Dim intStepMeasCnt As Short                         ' �ï�ߑ����
        Dim dblStepMeasPitch As Double                      ' �ï�ߑ����߯�
        Dim intStepMeasReptCnt As Short                     ' �ï�ߑ���J��Ԃ��ï�߉�
        Dim dblStepMeasReptPitch As Double                  ' �ï�ߑ���J��Ԃ��ï���߯�
        Dim intStepMeasLwGrpNo As Short                     ' �ï�ߑ��艺����۰�޸�ٰ�ߔԍ�
        Dim intStepMeasLwPtnNo As Short                     ' �ï�ߑ��艺����۰������ݔԍ�
        Dim dblStepMeasBpPosX As Double                     ' �ï�ߑ���BP�ʒuX
        Dim dblStepMeasBpPosY As Double                     ' �ï�ߑ���BP�ʒuY
        Dim intStepMeasUpGrpNo As Short                     ' �ï�ߑ�������۰�޸�ٰ�ߔԍ�
        Dim intStepMeasUpPtnNo As Short                     ' �ï�ߑ�������۰������ݔԍ�
        Dim dblStepMeasTblOstX As Double                    ' �ï�ߑ�������۰��ð��ٵ̾��X
        Dim dblStepMeasTblOstY As Double                    ' �ï�ߑ�������۰��ð��ٵ̾��Y
        Dim intIDReaderUse As Double                        ' IDذ�� (0:���g�p, 1:�g�p)
        Dim dblIDReadPos1X As Double                        ' IDذ�ޓǂݎ���߼޼�� 1X
        Dim dblIDReadPos1Y As Double                        ' IDذ�ޓǂݎ���߼޼�� 1Y
        Dim dblIDReadPos2X As Double                        ' IDذ�ޓǂݎ���߼޼�� 2X
        Dim dblIDReadPos2Y As Double                        ' IDذ�ޓǂݎ���߼޼�� 2Y
        Dim dblReprobeVar As Double                         ' ����۰��ݸނ΂����
        Dim dblReprobePitch As Double                       ' ����۰��ݸ��߯�
        '----- V2.0.0.0_23�� -----
        '----- �v���[�u�N���[�j���O���� -----(TKY���݂̂Ŏg�pINtime���ւ͑��M���Ȃ�)
        Dim dblPrbCleanPosX As Double                       ' �N���[�j���O�ʒuX
        Dim dblPrbCleanPosY As Double                       ' �N���[�j���O�ʒuY
        Dim dblPrbCleanPosZ As Double                       ' �N���[�j���O�ʒuZ
        Dim intPrbCleanUpDwCount As Short                   ' �v���[�u�㉺��
        Dim intPrbCleanAutoSubCount As Short                '  �����^�]���N���[�j���O���s�����
        '----- V2.0.0.0_23�� -----

        'V4.10.0.0�C           ��
        Public dblPrbCleanStagePitchX As Double             ' �X�e�[�W����s�b�`X
        Public dblPrbCleanStagePitchY As Double             ' �X�e�[�W����s�b�`Y
        Public intPrbCleanStageCountX As Short              ' �X�e�[�W�����X
        Public intPrbCleanStageCountY As Short              ' �X�e�[�W�����Y
        'V4.10.0.0�C           ��
        'V4.10.0.0�H��
        Public dblPrbDistance As Double                     ' �v���[�u�ԋ����imm�j
        Public dblPrbCleaningOffset As Double               ' �N���[�j���O�I�t�Z�b�g(mm)
        'V4.10.0.0�H��
        Dim intControllerInterlock As Integer               ' �O���@��ɂ��C���^�[���b�N�̗L�� 'V5.0.0.6�@

        Dim dblTXChipsizeRelationX As Double                ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
        Dim dblTXChipsizeRelationY As Double                ' �␳�ʒu�P�ƂQ�̑��Βl�x 'V4.5.1.0�N

    End Structure

    '----- ###229�� -----
    '--------------------------------------------------------------------------
    '   GPIB�f�[�^(�ėp)�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure GpibInfo
        Dim wGPIBmode As UShort                                         ' GP-IB����(0:���Ȃ� 1:����)
        Dim wDelim As UShort                                            ' �����(0:CR+LF 1:CR 2:LF 3:NONE)
        Dim wTimeout As UShort                                          ' ��ѱ��(1�`32767)(ms�P��)
        Dim wAddress As UShort                                          ' �@����ڽ(0�`30)
        Dim wEOI As UShort                                              ' EOI(0:�g�p���Ȃ�, 1:�g�p����)
        Dim wPause1 As UShort                                           ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
        Dim wPause2 As UShort                                           ' �ݒ�����2���M��|�[�Y����(1�`32767msec)
        Dim wPause3 As UShort                                           ' �ݒ�����3���M��|�[�Y����(1�`32767msec)
        Dim wPauseT As UShort                                           ' �ض޺���ޑ��M��|�[�Y����(1�`32767msec)
        Dim wRev As UShort                                              ' �\��
        Dim strI As String                                              ' �����������(MAX40byte)
        Dim strI2 As String                                             ' �����������2(MAX40byte)
        Dim strI3 As String                                             ' �����������3(MAX40byte)
        Dim strT As String                                              ' �ض޺����(50byte)
        Dim strName As String                                           ' �@�햼(10byte)
        Dim wReserve As String                                          ' �\��(8byte)  
    End Structure
    '----- ###229�� -----

    '--------------------------------------------------------------------------
    '   �X�e�b�v�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure StepInfo
        Dim intSP1 As Short                                 ' �ï�ߔԍ�
        Dim intSP2 As Short                                 ' ��ۯ���
        Dim dblSP3 As Double                                ' �ï�ߊԲ������
    End Structure

    '--------------------------------------------------------------------------
    '   �O���[�v�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure GrpInfo
        Dim intGP1 As Short                                 ' ��ٰ�ߔԍ�
        Dim intGP2 As Short                                 ' ��R��
        Dim dblGP3 As Double                                ' ��ٰ�ߊԲ������
        Dim dblStgPosX As Double                            ' �X�e�[�WX�|�W�V����
        Dim dblStgPosY As Double                            ' �X�e�[�WY�|�W�V����
    End Structure

    '--------------------------------------------------------------------------
    '   �T�[�L�b�g�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure CircuitInfo
        Dim intIP1 As Short                                 ' IP�ԍ�
        Dim dblIP2X As Double                               ' �}�[�L���OX
        Dim dblIP2Y As Double                               ' �}�[�L���OY
    End Structure

    '--------------------------------------------------------------------------
    '   �T�[�L�b�g���W�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure CirAxisInfo
        Dim intCaP1 As Short                                ' �ԍ�
        Dim dblCaP2 As Double                               ' ���WX
        Dim dblCaP3 As Double                               ' ���WY
    End Structure

    '-------------------------------------------------------------------------
    '   �T�[�L�b�g�ԃC���^�[�o���f�[�^�\���̌`����`
    '-------------------------------------------------------------------------
    Public Structure CirInInfo
        Dim intCiP1 As Short                                ' �ï�ߔԍ�
        Dim intCiP2 As Short                                ' ����Đ�
        Dim dblCiP3 As Double                               ' ����ĊԲ������
    End Structure

    '--------------------------------------------------------------------------
    '   �J�b�g�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure CutList
        Dim intCutNo As Short                               ' ��Ĕԍ�(1�`n)
        Dim intDelayTime As Short                           ' �ިڲ���
        Dim strCutType As String                            ' ��Č`��
        Dim dblTeachPointX As Double                        ' è��ݸ��߲��X
        Dim dblTeachPointY As Double                        ' è��ݸ��߲��Y
        Dim dblStartPointX As Double                        ' �����߲��X
        Dim dblStartPointY As Double                        ' �����߲��Y
        Dim dblCutSpeed As Double                           ' ��Ľ�߰��
        Dim dblQRate As Double                              ' Q����ڰ�
        Dim dblCutOff As Double                             ' ��ĵ̒l
        Dim dblJudgeLevel As Double                         ' �ؑփ|�C���g (���ް�����(���ω���))
        Dim dblCutOffOffset As Double                       ' ��ĵ̵̾�� 

        Dim intPulseWidthCtrl As Short                      ' ��ٽ������
        Dim dblPulseWidthTime As Double                     ' ��ٽ������
        Dim dblLSwPulseWidthTime As Double                  ' LSw��ٽ������(�O������)

        Dim intCutDir As Short                              ' ��ĕ���(1:0��, 2:90��, 3:180��, 4:270) ���ύX
        Dim intLTurnDir As Short                            ' L��ݕ���(1:CW, 2:CCW) ���ύX
        Dim dblMaxCutLength As Double                       ' �ő嶯èݸޒ�
        Dim dblLTurnPoint As Double                         ' L����߲��
        Dim dblMaxCutLengthL As Double                      ' L��݌�̍ő嶯èݸޒ�
        Dim dblMaxCutLengthHook As Double                   ' ̯���݌�̶�èݸޒ�
        Dim dblR1 As Double                                 ' R1
        Dim dblR2 As Double                                 ' R2
        Dim intCutAngle As Short                            ' �΂߶�Ă̐؂�o���p�x
        Dim dblCutSpeed2 As Double                          ' ��Ľ�߰��2
        Dim dblQRate2 As Double                             ' Q����ڰ�2
        Dim dblESPoint As Double                            ' ���޾ݽ�߲��
        Dim dblESJudgeLevel As Double                       ' ���޾ݽ�̔���ω���
        Dim dblMaxCutLengthES As Double                     ' ���޾ݽ��̶�Ē�
        Dim intIndexCnt As Short                            ' ���ޯ����
        Dim intMeasMode As Short                            ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��)
        Dim dblPitch As Double                              ' �߯�
        Dim intStepDir As Short                             ' �ï�ߕ���(0:0��, 1:90��, 2:180��, 3:270) ���ύX
        Dim intCutCnt As Short                              ' �{��
        Dim dblUCutDummy1 As Double                         ' U��ėp(������а)
        Dim dblUCutDummy2 As Double                         ' U��ėp(������а)
        Dim dblZoom As Double                               ' �{��
        Dim strChar As String                               ' ������
        Dim dblESChangeRatio As Double                      ' ���޾ݽ��ω���
        Dim intESConfirmCnt As Short                        ' ���޾ݽ��m�F��
        Dim intRadderInterval As Short                      ' ��ް�ԋ���
        Dim intCTcount As Short                             ' ���޾ݽ��A��NG�m�F�񐔁��ǉ�(ES�p)
        Dim intJudgeNg As Short                             ' NG���肷��/���Ȃ�(0:TRUE/1:FALSE)���ǉ�(ES�p)
        Dim intMeasType As Short                            ' ����^�C�v(0:���� ,1:�����x)�@���ǉ�(IX�p)
        Dim intMoveMode As Short                            ' ���샂�[�h(0:�ʏ탂�[�h, 2:�����J�b�g���[�h)�@���ǉ�
        Dim intDoPosition As Short                          ' �|�W�V���j���O(0:�L, 1:��)�@���ǉ�
        Dim intCutAftPause As Short                         ' �J�b�g��|�[�Y�^�C���i�����jV2.0.0.0_24(V1.18.0.3�@)(���[���a�Ή�)
        Dim dblReturnPos As Double                          ' ���^�[���J�b�g�̃��^�[���ʒu 'V1.16.0.0�@
        Dim dblLimitLen As Double                           ' IX�J�b�g�̃��~�b�g�� 'V1.18.0.0�C
        Dim strDataName As String                           ' U�J�b�g�f�[�^�����ǉ�      

        ' FL�p�ɉ��H������ǉ�
        Dim dblCutSpeed3 As Double                          ' ��Ľ�߰��3(L��Ă�����/��ڰ�����L��ݑO�̽�߰��)
        Dim dblCutSpeed4 As Double                          ' ��Ľ�߰��4(L��Ă�����/��ڰ�����L��݌�̽�߰��)
        Dim dblCutSpeed5 As Double                          ' ��Ľ�߰��5(u��Ă�����/��ڰ�����L��݌�̽�߰��)
        Dim dblCutSpeed6 As Double                          ' ��Ľ�߰��6(u��Ă�����/��ڰ�����L��݌�̽�߰��)
        <VBFixedArray(cCNDNUM)> Dim CndNum() As Short       ' ���H�����ԍ�1�`n(0�`31) 
        <VBFixedArray(cCNDNUM)> Dim dblPowerAdjustTarget() As Double    ' �����ڕW�p���[1�`n(0�`31)     '###066
        <VBFixedArray(cCNDNUM)> Dim dblPowerAdjustToleLevel() As Double ' �p���[�������e�͈�1�`n(0�`31) '###066

        '----- V2.0.0.0_23�� -----
        ' SL436S�p(�V���v���g���}�p)
        Dim dblQRate3 As Double                             ' Q����ڰ�3(L��Ă�����/��ڰ�����L��ݑO��Q����ڰ�)
        Dim dblQRate4 As Double                             ' Q����ڰ�4(L��Ă�����/��ڰ�����L��݌��Q����ڰ�)
        Dim dblQRate5 As Double                             ' Q����ڰ�5(U��Ă�����/��ڰ�����L��݌��Q����ڰ�) ���g�p
        Dim dblQRate6 As Double                             ' Q����ڰ�6(U��Ă�����/��ڰ�����L��݌��Q����ڰ�) ���g�p
        <VBFixedArray(cCNDNUM)> Dim FLCurrent() As Short    ' �d���l1�`8
        <VBFixedArray(cCNDNUM)> Dim FLSteg() As Short       ' STEG1�`8
        '----- V2.0.0.0�K�� -----

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim CndNum(cCNDNUM)                           ' ���H�����ԍ�1�`n(0�`31) 
            ReDim dblPowerAdjustTarget(cCNDNUM)             ' �����ڕW�p���[1�`n(0�`31)     '###066
            ReDim dblPowerAdjustToleLevel(cCNDNUM)          ' �p���[�������e�͈�1�`n(0�`31) '###066

            ReDim FLCurrent(cCNDNUM)                        ' �d���l1�`8
            ReDim FLSteg(cCNDNUM)                           ' STEG1�`8

        End Sub

    End Structure

    Public Const MaxCutInfo As Short = 30                   ' �ő嶯ď��

    '' ���̍\���͍̂폜����
    'Public Structure CutInfo
    '    Dim intCRNO As Short                                ' ��R�ԍ�(1�`9999)
    '    <VBFixedArray(MaxCutInfo)> Dim ArrCut() As CutList  ' ��Ė���ď��

    '    ' �\���̂̏�����
    '    Public Sub Initialize()
    '        ReDim ArrCut(MaxCutInfo)
    '    End Sub
    'End Structure

    '--------------------------------------------------------------------------
    '   ��R�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure ResistorInfo
        Dim intResNo As Short                               ' ��R�ԍ�(1�`9999)
        Dim intResMeasMode As Short                         ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��) ���ύX
        Dim intResMeasType As UShort                        ' ����^�C�v(0:���� ,1:�����x)�@���ǉ�
        Dim intCircuitGrp As Short                          ' ��������Ĕԍ�
        Dim intProbHiNo As Short                            ' ��۰�ޔԍ�(HI)
        Dim intProbLoNo As Short                            ' ��۰�ޔԍ�(LO)
        Dim intProbAGNo1 As Short                           ' ��۰�ޔԍ�(1)
        Dim intProbAGNo2 As Short                           ' ��۰�ޔԍ�(2)
        Dim intProbAGNo3 As Short                           ' ��۰�ޔԍ�(3)
        Dim intProbAGNo4 As Short                           ' ��۰�ޔԍ�(4)
        Dim intProbAGNo5 As Short                           ' ��۰�ޔԍ�(5)
        Dim strExternalBits As String                       ' EXTERNAL BITS(16�ޯ�)
        '    lngRP5      As Long
        ''''2009/06/01 ��TKY�ł�Long�^�������K�v
        Dim intPauseTime As Short                           ' �߰�����(msec)
        Dim intTargetValType As Short                       ' �ڕW�l�w��i0:��Βl,1:���V�I,2:�v�Z���j
        Dim intBaseResNo As Short                           ' �ް���R�ԍ�
        Dim dblTrimTargetVal As Double                      ' ���ݸޖڕW�l
        Dim dblTrimTargetVal_Save As Double                 ' V4.11.0.0�@ ���ݸޖڕW�l�ޔ�p���[�N 
        Dim dblTrimTargetOfs As Double                      ' V4.11.0.0�@ ���ݸޖڕW�l�̾��(��Βl)
        Dim dblTrimTargetOfs_Save As Double                 ' 'V5.0.0.2�@ ���ݸޖڕW�l�̾��(��)
        Dim strRatioTrimTargetVal As String                 ' �g���~���O�ڕW�l(���V�I�v�Z��) 
        Dim dblDeltaR As Double                             ' ��R
        ''''2009/06/01 ��  TKY�ł�Integer�^�œd���ω� �X���[�v
        Dim intSlope As Short                               ' �d���ω��۰�� 
        Dim dblCutOffRatio As Double                        ' �؂�グ�{��
        Dim dblProbCfmPoint_Hi_X As Double                  ' �v���[�u�m�F�ʒu HI X���W 
        Dim dblProbCfmPoint_Hi_Y As Double                  ' �v���[�u�m�F�ʒu HI Y���W 
        Dim dblProbCfmPoint_Lo_X As Double                  ' �v���[�u�m�F�ʒu LO X���W 
        Dim dblProbCfmPoint_Lo_Y As Double                  ' �v���[�u�m�F�ʒu LO Y���W  
        Dim dblInitTest_HighLimit As Double                 ' �Ƽ��ý�(HIGH�Я�)
        Dim dblInitTest_LowLimit As Double                  ' �Ƽ��ý�(LOW�Я�)
        Dim dblFinalTest_HighLimit As Double                ' ̧���ý�(HIGH�Я�)
        Dim dblFinalTest_LowLimit As Double                 ' ̧���ý�(LOW�Я�)
        Dim dblInitOKTest_HighLimit As Double               ' �Ƽ��OKý�(HIGH�Я�)
        Dim dblInitOKTest_LowLimit As Double                ' �Ƽ��OKý�(LOW�Я�)
        Dim intInitialOkTestDo As Short                     ' �����n�j����(0:���Ȃ�,1:����)���ǉ�(�v���[�g�f�[�^����ړ�)
        Dim intCutCount As Short                            ' ��Đ�

        '----- TKY�p -----
        Dim intCutReviseMode As Short                       ' ��ĕ␳(0:����, 1:����)       
        Dim intCutReviseDispMode As Short                   ' �\��Ӱ��(0:����, 1:CRT)        
        Dim intCutReviseGrpNo As Short                      ' ����ݸ�ٰ�ߔԍ�      
        Dim intCutRevisePtnNo As Short                      ' ����ݔԍ�      
        Dim dblCutRevisePosX As Double                      ' ��ĕ␳�ʒuX   
        Dim dblCutRevisePosY As Double                      ' ��ĕ␳�ʒuY   
        Dim intIsNG As Short                                ' �摜�F��NG����(0:����, 1:�Ȃ�)

        '----- V1.13.0.0�A�� -----
        Dim intCvMeasNum As Short                           ' CV �ő呪���
        Dim intCvMeasTime As Short                          ' CV �ő呪�莞��(ms) 
        Dim dblCvValue As Double                            ' CV CV�l         
        Dim intOverloadNum As Short                         ' ���ް۰�� �� 
        Dim dblOverloadMin As Double                        ' ���ް۰�� �����l 
        Dim dblOverloadMax As Double                        ' ���ް۰�� ����l
        '----- V1.13.0.0�A�� -----
        '----- V2.0.0.0_23�� -----
        Dim wPauseTimeFT As Short                           ' FT�O�̃|�[�Y�^�C��(0-32767msec) (���[���a�Ή�) V2.0.0.0_24
        Dim intInsideEndChkCount As Short                   ' ���؂蔻���(0-255)
        Dim dblInsideEndChgRate As Double                   ' ���؂蔻��ω���(0.00-100.00%)
        '----- V2.0.0.0_23�� -----

        <VBFixedArray(MaxCutInfo)> Dim ArrCut() As CutList  ' ��ď��

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim ArrCut(MaxCutInfo)
        End Sub

    End Structure

    '--------------------------------------------------------------------------
    '   Ty2�f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure Ty2Info
        Dim intTy21 As Short                                ' ��ۯ��ԍ�
        Dim dblTy22 As Double                               ' �ï�ߋ���
    End Structure

    '--------------------------------------------------------------------------
    '   �ٌ`�ʕt���f�[�^�\���̌`����`
    '--------------------------------------------------------------------------
    Public Structure IKEIInfo
        Dim intI1 As Short                                  ' �ٌ`�ʕt���̗L���i0:����,1:X����,2:Y�����j
        <VBFixedArray(MaxCntCircuit)> Dim intI2() As Short  ' �T�[�L�b�g�̗L���i0:����,1:�L��j

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim intI2(MaxCntCircuit)
        End Sub
    End Structure
#End If
#End Region 'V5.0.0.8�@
#End Region

#Region "�g���~���O�f�[�^�̍ŏ��l/�ő�l�`�F�b�N�p�\���̌`����`"
    '-------------------------------------------------------------------------------
    '   STEP(0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure SPInputArea
        <VBFixedArray(1)> Dim SP1() As Short                ' �ï�ߔԍ�
        <VBFixedArray(1)> Dim SP2() As Short                ' ��ۯ���
        <VBFixedArray(1)> Dim SP3() As Double               ' �ï�ߊԲ������

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim SP1(1)
            ReDim SP2(1)
            ReDim SP3(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   GROUP(0)Min (1)Max 
    '-------------------------------------------------------------------------------
    Public Structure GPInputArea
        <VBFixedArray(1)> Dim GP1() As Short                ' ��ٰ�ߔԍ�
        <VBFixedArray(1)> Dim GP2() As Short                ' ��R��
        <VBFixedArray(1)> Dim GP3() As Double               ' ��ٰ�ߊԲ������

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim GP1(1)
            ReDim GP2(1)
            ReDim GP3(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   Ty2 (0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure Ty2InputArea
        <VBFixedArray(1)> Dim Ty1() As Short                ' ��ۯ��ԍ�
        <VBFixedArray(1)> Dim Ty2() As Double               ' X
        <VBFixedArray(1)> Dim Ty3() As Double               ' Y

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim Ty1(1)
            ReDim Ty2(1)
            ReDim Ty3(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   RESISTOR(0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure RPInputArea
        <VBFixedArray(1)> Dim RP1() As Short                ' ��R�ԍ�
        <VBFixedArray(1)> Dim RP2() As Short                ' �����x����A����Ӱ��
        <VBFixedArray(1)> Dim RP2K() As Double              ' �����x����A����Ӱ�� KOA���̗��a
        <VBFixedArray(1)> Dim RP4HL() As Short              ' ��۰�ޔԍ�(HI/Lo)
        <VBFixedArray(1)> Dim RP4AG() As Short              ' ��۰�ޔԍ�(AG)
        <VBFixedArray(1)> Dim RP5() As Double               ' EXTERNAL BITS
        <VBFixedArray(1)> Dim RP6() As Double               ' �߰�����
        <VBFixedArray(1)> Dim RP9() As Double               ' ���ݸޖڕW�l
        <VBFixedArray(1)> Dim RP10() As Double              ' ���q
        <VBFixedArray(1)> Dim RP15() As Double              ' �؂�グ�{��
        <VBFixedArray(1)> Dim RP11() As Double              ' �Ƽ��ý�H/L�Я�
        <VBFixedArray(1)> Dim RP12() As Double              ' ̧���ý�H/L�Я�
        <VBFixedArray(1)> Dim RP13() As Short               ' ��Đ�
        <VBFixedArray(1)> Dim RP14() As Double              ' �Ƽ��OKý�H/L�Я�

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim RP1(1)
            ReDim RP2(1)
            ReDim RP2K(1)
            ReDim RP4HL(1)
            ReDim RP4AG(1)
            ReDim RP5(1)
            ReDim RP6(1)
            ReDim RP9(1)
            ReDim RP10(1)
            ReDim RP15(1)
            ReDim RP11(1)
            ReDim RP12(1)
            ReDim RP13(1)
            ReDim RP14(1)
        End Sub
    End Structure

    '-------------------------------------------------------------------------------
    '   CUT(0)Min (1)Max
    '-------------------------------------------------------------------------------
    Public Structure CPInputArea
        <VBFixedArray(1)> Dim wCutNo() As Short                ' ��Ĕԍ�
        <VBFixedArray(1)> Dim wDelayTime() As Short                ' �ިڲ���
        <VBFixedArray(1)> Dim wCutType() As String               ' ��Č`��
        <VBFixedArray(1)> Dim CP99() As Double              ' è��ݸ��߲��XY
        <VBFixedArray(1)> Dim CP4() As Double               ' �����߲��XY
        <VBFixedArray(1)> Dim CP5() As Double               ' ��Ľ�߰��
        <VBFixedArray(1)> Dim CP6() As Double               ' Q����ڰ�
        <VBFixedArray(1)> Dim fCutOff() As Double               ' ��ĵ̒l
        <VBFixedArray(1)> Dim CP7_2() As Double             ' ��ĵ̵̾��
        <VBFixedArray(1)> Dim CP7_1() As Double             ' �ް�����(���ω���)�
        <VBFixedArray(1)> Dim CP50() As Short               ' ��ٽ������
        <VBFixedArray(1)> Dim CP51() As Double              ' ��ٽ������
        <VBFixedArray(1)> Dim CP52() As Double              ' LSw��ٽ������(�O������)
        <VBFixedArray(1)> Dim CP8() As Short                ' ��ĕ���
        <VBFixedArray(1)> Dim CP9() As Double               ' �ő嶯èݸޒ�
        <VBFixedArray(1)> Dim CP11() As Double              ' L����߲��
        <VBFixedArray(1)> Dim CP14() As Double              ' L��݌�̍ő嶯èݸޒ�
        <VBFixedArray(1)> Dim CP15() As Double              ' ̯���݌�̶�èݸޒ�
        <VBFixedArray(1)> Dim CP56() As Double              ' R1
        <VBFixedArray(1)> Dim CP57() As Double              ' R2
        <VBFixedArray(1)> Dim CP33() As Short               ' �΂߶�Ă̐؂�o���p�x
        <VBFixedArray(1)> Dim CP36() As Double              ' ��Ľ�߰��2
        <VBFixedArray(1)> Dim CP37() As Double              ' Q����ڰ�2
        <VBFixedArray(1)> Dim CP53() As Double              ' Q����ڰ�3
        <VBFixedArray(1)> Dim CP54() As Double              ' �ؑւ��߲�� 
        <VBFixedArray(1)> Dim CP38() As Double              ' ���޾ݽ�߲��
        <VBFixedArray(1)> Dim CP39() As Double              ' ���޾ݽ�̔���ω���
        <VBFixedArray(1)> Dim CP40() As Double              ' ���޾ݽ��̶�Ē�
        <VBFixedArray(1)> Dim CP18() As Short               ' ���ޯ����
        <VBFixedArray(1)> Dim CP19() As Short               ' ����Ӱ��
        <VBFixedArray(1)> Dim CP19K() As Double             ' ����Ӱ��(KOA���̗�) 
        <VBFixedArray(1)> Dim CP60() As Double              ' �߯�
        <VBFixedArray(1)> Dim CP61() As Short               ' �ï�ߕ���
        <VBFixedArray(1)> Dim CP62() As Short               ' �{��
        <VBFixedArray(1)> Dim CP70() As Double              ' U��ėp(������а)
        <VBFixedArray(1)> Dim fAveDataRate() As Double              ' U��ėp(������а)
        <VBFixedArray(1)> Dim CP72() As Double              ' �{�� 
        <VBFixedArray(1)> Dim CP73() As Short               ' ������   
        <VBFixedArray(1)> Dim CP74() As Double              ' ���޾ݽ��̕ω���
        <VBFixedArray(1)> Dim CP75() As Short               ' ���޾ݽ��̊m�F��
        <VBFixedArray(1)> Dim CP76() As Short               ' ��ް�ԋ���

        ' �\���̂̏�����
        Public Sub Initialize()
            ReDim wCutNo(1)
            ReDim wDelayTime(1)
            ReDim wCutType(1)
            ReDim CP99(1)
            ReDim CP4(1)
            ReDim CP5(1)
            ReDim CP6(1)
            ReDim fCutOff(1)
            ReDim CP7_2(1)
            ReDim CP7_1(1)
            ReDim CP50(1)
            ReDim CP51(1)
            ReDim CP52(1)
            ReDim CP8(1)
            ReDim CP9(1)
            ReDim CP11(1)
            ReDim CP14(1)
            ReDim CP15(1)
            ReDim CP56(1)
            ReDim CP57(1)
            ReDim CP33(1)
            ReDim CP36(1)
            ReDim CP37(1)
            ReDim CP53(1)
            ReDim CP54(1)
            ReDim CP38(1)
            ReDim CP39(1)
            ReDim CP40(1)
            ReDim CP18(1)
            ReDim CP19(1)
            ReDim CP19K(1)
            ReDim CP60(1)
            ReDim CP61(1)
            ReDim CP62(1)
            ReDim CP70(1)
            ReDim fAveDataRate(1)
            ReDim CP72(1)
            ReDim CP73(1)
            ReDim CP74(1)
            ReDim CP75(1)
            ReDim CP76(1)
        End Sub
    End Structure
#End Region

#Region "�O���[�o���ϐ��̒�`"
    '-------------------------------------------------------------------------------
    '   �g���~���O�f�[�^�\���̕ϐ���`
    '-------------------------------------------------------------------------------
    '----- ��ڰ��ް� -----
    'V5.0.0.8�@    Public typPlateInfo As PlateInfo                        ' ��ڰ��ް�

    '----- GPIB�f�[�^(�ėp) -----                           ' ###229
    'V5.0.0.8�@    Public typGpibInfo As GpibInfo                          ' GPIB�f�[�^(�ėp)

    '----- �ï���ް�(CHIP�p) -----
    'V5.0.0.8�@    Public Const MaxCntStep As Short = 256                  ' �ï�ߍő匏��
    Public Const MAXCNT_PLT_BLK As Short = 1024
    'V5.0.0.8�@    Public MaxStep As Short                                 ' ���ۂ̽ï�ߌ���
    'V5.0.0.8�@    Public typStepInfoArray(MaxCntStep) As StepInfo         ' �ï���ް�

    '----- ��ٰ���ް� -----
    'V5.0.0.8�@    Public MaxGrp As Short                                  ' ���ۂ̸�ٰ�ߌ���
    'V5.0.0.8�@    Public typGrpInfoArray(MaxCntStep) As GrpInfo           ' ��ٰ���ް�
    Public gBlkStagePosX(MAXCNT_PLT_BLK) As Double          ' �u���b�N�̃v���[�g���̃X�e�[�WX�ʒu���W�f�[�^
    Public gBlkStagePosY(MAXCNT_PLT_BLK) As Double          ' �u���b�N�̃v���[�g���̃X�e�[�WY�ʒu���W�f�[�^
    Public gPltStagePosX(MAXCNT_PLT_BLK) As Double          ' �v���[�g�̃X�e�[�WX�ʒu���W�f�[�^
    Public gPltStagePosY(MAXCNT_PLT_BLK) As Double          ' �v���[�g�̃X�e�[�WY�ʒu���W�f�[�^

    '----- �T�[�L�b�g�f�[�^(TKY�p) -----
    'V5.0.0.8�@    Public Const MaxCntCircuit As Short = 256                   ' ����čő匏��
    'V5.0.0.8�@    Public typCircuitInfoArray(MaxCntCircuit) As CircuitInfo    ' �T�[�L�b�g�f�[�^

    '----- �T�[�L�b�g�f�[�^(NET�p) -----
    'V5.0.0.8�@    Public typCirAxisInfoArray(MaxCntCircuit) As CirAxisInfo    ' ����č��W
    'V5.0.0.8�@    Public typCirInInfoArray(MaxCntCircuit) As CirInInfo        ' ����ĊԲ������

    '----- ��R�ް� -----
    'V5.0.0.8�@    Public Const MaxCntResist As Short = 512                   ' ��R�ő匏��
    'V5.0.0.8�@    Public typResistorInfoArray(MaxCntResist) As ResistorInfo   ' ��R�ް�
    'V5.0.0.8�@    Public markResistorInfoArray(MaxCntResist) As ResistorInfo  ' ��R�ް�(NGϰ�ݸޗp)

    ''----- ����ް� -----
    'Public typCutInfoArray(MaxCntResist) As CutInfo         ' ����ް�
    'Public markCutInfoArray(MaxCntResist) As CutInfo        ' ����ް�(NGϰ�ݸޗp)

    '----- Ty2�ް� -----
    'V5.0.0.8�@    Public Const MaxCntTy2 As Short = 256                   ' Ty2��ۯ��ő匏��
    'V5.0.0.8�@    Public MaxTy2 As Short                                  ' ���ۂ�Ty2��ۯ�����
    'V5.0.0.8�@    Public typTy2InfoArray(MaxCntTy2) As Ty2Info            ' Ty2�f�[�^

    '----- �g���~���O���ʃf�[�^ -----
    '    Public Const MAX_RESULT_NUM As Short = 21               ' �g���~���O���ʂ̍ő�ԍ� 'V2.0.0.0�L
    Public Const MAX_RESULT_NUM As Short = 22               ' �g���~���O���ʂ̍ő�ԍ� 'V2.0.0.0�L
    Public gwTrimResult(MaxCntResist) As UShort             ' OK/NG����(0:�����{, 1:OK, 2:ITNG, 3:FTNG, 4:SKIP, ... 13:PATTERN NG)
    Public gfInitialTest(MaxCntResist) As Double            ' IT ��R�l
    Public gfFinalTest(MaxCntResist) As Double              ' FT ��R�l
    Public gfTargetVal(MaxCntResist) As Double              ' ���V�I�ڕW�l
    Public iNgHiCount(MaxCntResist) As Integer              ' �A��NG-HI���� ###129

    '----- �ٌ`�ʕt���f�[�^ -----
    'V5.0.0.8�@    Public typIKEIInfo As IKEIInfo                          ' �ٌ`�ʕt���f�[�^

    ''-------------------------------------------------------------------------------
    ''   �g���~���O�f�[�^�̍ŏ��l/�ő�l�`�F�b�N�p�\���̕ϐ���`
    ''-------------------------------------------------------------------------------
    Public typSPInputArea As SPInputArea                    ' STEP (0)Min (1)Max
    Public typGPInputArea As GPInputArea                    ' GROUP(0)Min (1)Max 
    Public typTy2InputArea As Ty2InputArea                  ' Ty2  (0)Min (1)Max
    Public typRPInputArea As RPInputArea                    ' RES. (0)Min (1)Max
    Public typCPInputArea As CPInputArea                    ' CUT  (0)Min (1)Max

#End Region

#Region "���ݸ����Ұ��̏�����"
    '''=========================================================================
    '''<summary>���ݸ����Ұ��̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_AllTrmmingData()

        ' ���ݸ����Ұ��̏�����
        DataManager.Init_AllTrmmingData()               'V5.0.0.8�@
        'V5.0.0.8�@        Call Init_typCircuitInfo()                              ' �T�[�L�b�g
        'V5.0.0.8�@        Call Init_typPlateInfo()                                ' �v���[�g�f�[�^
        'V5.0.0.8�@        Call Init_typGpibInfo()                                 ' BPIB�f�[�^ ' ###229
        'V5.0.0.8�@        Call Init_typStepInfoArray()                            ' �X�e�b�v�f�[�^
        Call Init_typGrpInfoArray()                             ' �O���[�v�f�[�^
        'V5.0.0.8�@        Call Init_typResistorInfoArray(typResistorInfoArray)    ' ��R�f�[�^/�J�b�g�f�[�^
        'V5.0.0.8�@        Call Init_typResistorInfoArray(markResistorInfoArray)   ' ��R�f�[�^/�J�b�g�f�[�^(NGϰ�ݸޗp)
        'V5.0.0.8�@        Call Init_typTy2InfoArray()                             ' Ty2�f�[�^
        'V5.0.0.8�@        Call Init_typIKEIInfo()                                 ' �ٌ`�ʕt���f�[�^

    End Sub
#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
#Region "��ڰ��ް��\���̂̏�����"
    '''=========================================================================
    '''<summary>��ڰ��ް��\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_typPlateInfo()

        With typPlateInfo
            '<PLATE DATA 1>
            .strDataName = "(DATA NAME)"                    ' �ް���
            .intMeasType = 0                                ' ������
            .intDirStepRepeat = 0                           ' �ï�߁���߰�
            .intPlateCntXDir = 1                            ' �v���[�g��X 
            .intPlateCntYDir = 1                            ' �v���[�g��Y
            .intBlockCntXDir = 1                            ' ��ۯ����w
            .intBlockCntYDir = 1                            ' ��ۯ����x
            .dblPlateItvXDir = 0.0#                         ' �v���[�g�Ԋu�w
            .dblPlateItvYDir = 0.0#                         ' �v���[�g�Ԋu�x 
            .dblBlockSizeXDir = 0.0#                        ' �u���b�N�T�C�Y�w   
            .dblBlockSizeYDir = 0.0#                        ' �u���b�N�T�C�Y�x   
            .dblTableOffsetXDir = 0.0#                      ' ð��وʒu�̾��X
            .dblTableOffsetYDir = 0.0#                      ' ð��وʒu�̾��Y
            .dblBpOffSetXDir = 0.0#                         ' �ްшʒu�̾��X
            .dblBpOffSetYDir = 0.0#                         ' �ްшʒu�̾��Y
            .dblAdjOffSetXDir = 0.0#                        ' ��ެ�Ĉʒu�̾��X
            .dblAdjOffSetYDir = 0.0#                        ' ��ެ�Ĉʒu�̾��Y
            .intCurcuitCnt = 0                              ' �T�[�L�b�g��  
            .intNGMark = 0                                  ' NGϰ�ݸ�
            .intDelayTrim = 0                               ' �ިڲ���
            .intNgJudgeUnit = 0                             ' NG����P��
            .intNgJudgeLevel = 0                            ' NG����
            'V4.0.0.0-38                ��������
            If (gKeiTyp = MACHINE_KD_RS) Then
                .dblZOffSet = 1.0#                          ' ��۰��Z�̾��
                .dblZStepUpDist = 1.0#                      ' ��۰�޽ï�ߏ㏸����
                .dblZWaitOffset = 0.0#                      ' ��۰�ޑҋ@Z�̾��
            Else
                .dblZOffSet = 5.0#                          ' ��۰��Z�̾��
                .dblZStepUpDist = 3.0#                      ' ��۰�޽ï�ߏ㏸����
                .dblZWaitOffset = 1.0#                      ' ��۰�ޑҋ@Z�̾��
            End If
            'V4.0.0.0-38                ��������
            .dblLwPrbStpDwDist = 0.0                        ' ������۰�޽ï�߉��~���� V1.13.0.0�A
            .dblLwPrbStpUpDist = 5.0                        ' ������۰�޽ï�ߏ㏸���� V1.13.0.0�A
            .intPrbRetryCount = 0                           ' �v���[�u���g���C��(0=���g���C�Ȃ�) V4.0.0.0�C
            .intFinalJudge = 0                              ' �t�@�C�i������(option)  
            .intAttLevel = 0                                ' �A�b�e�l�[�^(%)(option)    
            '----- V1.23.0.0�F�� -----
            ' �v���[�g�P�^�u�̃v���[�u�`�F�b�N����(�I�v�V����)
            .intPrbChkPlt = 0                               '  ����
            .intPrbChkBlk = 1                               ' �u���b�N
            .dblPrbTestLimit = 0.0                          '  �덷�}%
            '----- V1.23.0.0�F�� -----

            '<PLATE DATA 3>
            .intResistDir = 0                               ' ��R���ѕ���
            .intCircuitCntInBlock = 1                       ' 1��ۯ��໰��Đ�   
            .dblCircuitSizeXDir = 0.0001                    ' ����Ļ���X    
            .dblCircuitSizeYDir = 0.0001                    ' ����Ļ���Y  
            .intResistCntInBlock = 1                        ' 1�u���b�N����R��
            .intResistCntInGroup = 1                        ' 1��ٰ�ߓ���R��
            .intGroupCntInBlockXBp = 1                      ' ��ۯ����ٰ�ߐ�BP(X�����j(NET���̓T�[�L�b�g��)
            .intGroupCntInBlockYStage = 1                   ' ��ۯ����ٰ�ߐ�Stage(Y�����j
            .dblGroupItvXDir = 0.0#                         ' ��ٰ�ߊԊuX
            .dblGroupItvYDir = 0.0#                         ' ��ٰ�ߊԊuY
            .dblChipSizeXDir = 0.0#                         ' ���߻���X
            .dblChipSizeYDir = 0.0#                         ' ���߻���Y
            .dblStepOffsetXDir = 0.0#                       ' �ï�ߵ̾�ė�X
            .dblStepOffsetYDir = 0.0#                       ' �ï�ߵ̾�ė�Y
            .dblBlockSizeReviseXDir = 0.0#                  ' ��ۯ����ޕ␳X
            .dblBlockSizeReviseYDir = 0.0#                  ' ��ۯ����ޕ␳Y
            .dblBlockItvXDir = 0.0#                         ' ��ۯ��ԊuX
            .dblBlockItvYDir = 0.0#                         ' ��ۯ��ԊuY
            .intContHiNgBlockCnt = 0                        ' �A��NG-HIGH��R��ۯ���

            '(2010/11/09)
            '   �ꎞ�I�Ƀf�[�^�ǉ�-�u���b�N�̍l������V�K�ɍ��킹�邽�߁A
            '   BP�O���[�v�Ԋu�ƃX�e�[�W�O���[�v�Ԋu�̕ϐ���ǉ�
            .dblBpGrpItv = 0.0#                             ' BP�O���[�v�Ԋu�i�ȑO��CHIP�̃O���[�v�Ԋu�j
            .dblStgGrpItvX = 0.0#                           ' X�����X�e�[�W�O���[�v�Ԋu�i�ȑO�̂b�g�h�o�̃X�e�b�v�ԃC���^�[�o���j
            .dblStgGrpItvY = 0.0#                           ' Y�����X�e�[�W�O���[�v�Ԋu�i�ȑO�̂b�g�h�o�̃X�e�b�v�ԃC���^�[�o���j
            .dblPlateSizeX = 0.0#                           ' �v���[�g�T�C�YX
            .dblPlateSizeY = 0.0#                           ' �v���[�g�T�C�YY
            .intBpGrpCntInBlk = 0                           ' �u���b�N��BP�O���[�v��
            .intBlkCntInStgGrpX = 1                         ' X�����X�e�[�W�O���[�v���u���b�N��
            .intBlkCntInStgGrpY = 1                         ' Y�����X�e�[�W�O���[�v���u���b�N��
            .intStgGrpCntInPlt = 0                          ' �v���[�g���X�e�[�W�O���[�v�����s�v����
            .intChipStepCnt = 0                             ' �O���[�v���̃`�b�v�X�e�b�v���B
            .dblChipStepItv = 0                             ' �O���[�v���`�b�v�X�e�b�v���̃X�e�b�v�Ԋu�i��{�̓`�b�v�T�C�Y�j

            '<PLATE DATA 2>
            .intReviseMode = 0                              ' �␳Ӱ��
            .intManualReviseType = 0                        ' �␳���@
            .dblReviseCordnt1XDir = 0.0#                    ' �␳�ʒu���W1X
            .dblReviseCordnt1YDir = 0.0#                    ' �␳�ʒu���W1Y
            .dblReviseCordnt2XDir = 0.0#                    ' �␳�ʒu���W2X
            .dblReviseCordnt2YDir = 0.0#                    ' �␳�ʒu���W2Y
            .dblReviseOffsetXDir = 0.0#                     ' �␳�߼޼�ݵ̾��X
            .dblReviseOffsetYDir = 0.0#                     ' �␳�߼޼�ݵ̾��Y
            .intRecogDispMode = 0                           ' �F���ް��\��Ӱ��
            If gSysPrm.stDEV.giEXCAM = 1 Then
                .dblPixelValXDir = gSysPrm.stGRV.gfEXCAM_PixelX ' �߸�ْlX
                .dblPixelValYDir = gSysPrm.stGRV.gfEXCAM_PixelY ' �߸�ْlY
            Else
                .dblPixelValXDir = gSysPrm.stGRV.gfPixelX   ' �߸�ْlX
                .dblPixelValYDir = gSysPrm.stGRV.gfPixelY   ' �߸�ْlY
            End If
            .intRevisePtnNo1 = 1                            ' �␳�ʒu�����No1
            .intRevisePtnNo2 = 2                            ' �␳�ʒu�����No2
            .intRevisePtnNo1GroupNo = 2                     ' �␳�ʒu�����No1�O���[�vNo
            .intRevisePtnNo2GroupNo = 2                     ' �␳�ʒu�����No2�O���[�vNo
            .dblRotateXDir = 0.0#                           ' X�����̉�]���S
            .dblRotateYDir = 0.0#                           ' Y�����̉�]���S
            .dblThetaAxis = 0.0#                            ' �Ɖ�]�p�x
            'V5.0.0.9�@                              �� �N�����v���X�E���t�A���C�����g�p
            .intReviseExecRgh = 0S                          ' �␳�L��(0:�␳�Ȃ�, 1:�␳����)
            .dblReviseCordnt1XDirRgh = 0.0                  ' �␳�ʒu���W1X
            .dblReviseCordnt1YDirRgh = 0.0                  ' �␳�ʒu���W1Y
            .dblReviseCordnt2XDirRgh = 0.0                  ' �␳�ʒu���W2X
            .dblReviseCordnt2YDirRgh = 0.0                  ' �␳�ʒu���W2Y
            .dblReviseOffsetXDirRgh = 0.0                   ' �␳�߼޼�ݵ̾��X
            .dblReviseOffsetYDirRgh = 0.0                   ' �␳�߼޼�ݵ̾��Y
            .intRecogDispModeRgh = 0S                       ' �F���ް��\��Ӱ��(0:�\���Ȃ�, 1:�\������)
            .intRevisePtnNo1Rgh = 3S                        ' �␳�ʒu�����No1
            .intRevisePtnNo2Rgh = 4S                        ' �␳�ʒu�����No2
            .intRevisePtnNo1GroupNoRgh = 2S                 ' �␳�ʒu�����No1�O���[�vNo
            .intRevisePtnNo2GroupNoRgh = 2S                 ' �␳�ʒu�����No2�O���[�vNo
            'V5.0.0.9�@                              ��

            '<PLATE DATA 4>
            .dblCaribBaseCordnt1XDir = 0.0#                 ' �����ڰ��݊���W1X
            .dblCaribBaseCordnt1YDir = 0.0#                 ' �����ڰ��݊���W1Y
            .dblCaribBaseCordnt2XDir = 0.0#                 ' �����ڰ��݊���W2X
            .dblCaribBaseCordnt2YDir = 0.0#                 ' �����ڰ��݊���W2Y
            .dblCaribTableOffsetXDir = 0.0#                 ' �����ڰ���ð��ٵ̾��X
            .dblCaribTableOffsetYDir = 0.0#                 ' �����ڰ���ð��ٵ̾��Y
            .intCaribPtnNo1 = 1                             ' �����ڰ�������ݓo�^No1
            .intCaribPtnNo2 = 2                             ' �����ڰ�������ݓo�^No2
            .intCaribPtnNo1GroupNo = 3                      ' �����ڰ�������ݓo�^No1�O���[�vNo
            .intCaribPtnNo2GroupNo = 3                      ' �����ڰ�������ݓo�^No2�O���[�vNo
            .dblCaribCutLength = 0.0#                       ' �����ڰ��ݶ�Ē�
            .dblCaribCutSpeed = 0.1                         ' �����ڰ��ݶ�đ��x
            .dblCaribCutQRate = 0.1                         ' �����ڰ���ڰ��Qڰ�
            .dblCutPosiReviseOffsetXDir = 0.0#              ' ��ē_�␳ð��ٵ̾��X
            .dblCutPosiReviseOffsetYDir = 0.0#              ' ��ē_�␳ð��ٵ̾��Y
            .intCutPosiRevisePtnNo = 1                      ' ��ē_�␳����ݓo�^No
            .dblCutPosiReviseCutLength = 0.0#               ' ��ē_�␳��Ē�
            .dblCutPosiReviseCutSpeed = 0.1                 ' ��ē_�␳��đ��x
            .dblCutPosiReviseCutQRate = 0.1                 ' ��ē_�␳ڰ��Qڰ�
            .intCutPosiReviseGroupNo = 4                    ' ��ٰ��No

            '<PLATE DATA 5>
            .intMaxTrimNgCount = 0                          ' ���ݸ�NG����(���)
            .intMaxBreakDischargeCount = 0                  ' ���ꌇ���r�o����(���)
            .intTrimNgCount = 0                             ' �A�����ݸ�NG����
            .intContHiNgResCnt = 0                          ' �A�����ݸ�NG��R��    ###230
            .intNgJudgeStop = 0                             ' NG���莞��~  V1.13.0.0�A
            .intRetryProbeCount = 0                         ' ����۰��ݸމ�
            .dblRetryProbeDistance = 0.0#                   ' ����۰��ݸވړ���
            .intPowerAdjustMode = 0                         ' ��ܰ����Ӱ��
            .intPwrChkPltNum = 0                            ' �`�F�b�N����� V4.11.0.0�@
            .intPwrChkTime = 0                              ' �`�F�b�N����(��) V4.11.0.0�@
            .dblPowerAdjustTarget = 0.1                     ' �����ڕW��ܰ
            .dblPowerAdjustQRate = 0.1                      ' ��ܰ����Qڰ�
            .dblPowerAdjustToleLevel = 0.01                 ' ��ܰ�������e�͈�
            .intPowerAdjustCondNo = 0                       ' ��ܰ�������H�����ԍ�(FL�p)�@
            .intInitialOkTestDo = 0                         ' �Ƽ��OKý�
            .intWorkSetByLoader = 1                         ' ��i��
            .intOpenCheck = 0                               ' 4�[�q���������
            .intLedCtrl = 0                                 ' LED����
            .dblThetaAxis = 0.0#                            ' �Ǝ�
            .intGpibCtrl = 0                                ' GP-IB����
            .intGpibDefDelimiter = 0                        ' �����ݒ�(�����)
            .intGpibDefTimiout = 0                          ' �����ݒ�(��ѱ��)
            .intGpibDefAdder = 0                            ' �����ݒ�(�@����ڽ)
            .strGpibInitCmnd1 = ""                          ' �����������1
            .strGpibInitCmnd2 = ""                          ' �����������2
            .strGpibTriggerCmnd = ""                        ' �ض޺����
            .intGpibMeasSpeed = 0                           ' ���葬�x
            .intGpibMeasMode = 0                            ' ���胂�[�h

            .dblTThetaOffset = 0.0#                         ' �s�ƃI�t�Z�b�g
            .dblTThetaBase1XDir = 0.0#                      ' �s�Ɗ�ʒu�PX�imm�j
            .dblTThetaBase1YDir = 0.0#                      ' �s�Ɗ�ʒu�PY�imm�j
            .dblTThetaBase2XDir = 0.0#                      ' �s�Ɗ�ʒu�QX�imm�j
            .dblTThetaBase2YDir = 0.0#                      ' �s�Ɗ�ʒu�QY�imm�j

            '<PLATE DATA 6> (��ڰ����3(V1.13.0.0�A))
            .intContExpMode = 0                             ' �L�k�␳ (0:�Ȃ�, 1:����)
            .intContExpGrpNo = 5                            ' �L�k�␳��ٰ�ߔԍ�
            .intContExpPtnNo = 1                            ' �L�k�␳����ݔԍ�
            .dblContExpPosX = 0.0                           ' �L�k�␳�ʒuX (mm)
            .dblContExpPosY = 0.0                           ' �L�k�␳�ʒuXY (mm)            
            .intStepMeasCnt = 0                             ' �ï�ߑ����
            .dblStepMeasPitch = 0.0                         ' �ï�ߑ����߯�
            .intStepMeasReptCnt = 0                         ' �ï�ߑ���J��Ԃ��ï�߉�
            .dblStepMeasReptPitch = 0.0                     ' �ï�ߑ���J��Ԃ��ï���߯�
            .intStepMeasLwGrpNo = 6                         ' �ï�ߑ��艺����۰�޸�ٰ�ߔԍ�
            .intStepMeasLwPtnNo = 1                         ' �ï�ߑ��艺����۰������ݔԍ�
            .dblStepMeasBpPosX = 0.0                        ' �ï�ߑ���BP�ʒuX
            .dblStepMeasBpPosY = 0.0                        ' �ï�ߑ���BP�ʒuY
            .intStepMeasUpGrpNo = 6                         ' �ï�ߑ�������۰�޸�ٰ�ߔԍ�
            .intStepMeasUpPtnNo = 2                         ' �ï�ߑ�������۰������ݔԍ�
            .dblStepMeasTblOstX = 0.0                       ' �ï�ߑ�������۰��ð��ٵ̾��X
            .dblStepMeasTblOstY = 0.0                       ' �ï�ߑ�������۰��ð��ٵ̾��Y
            .intIDReaderUse = 0                             ' IDذ�� (0:���g�p, 1:�g�p)
            .dblIDReadPos1X = 60.0                          ' IDذ�ޓǂݎ���߼޼�� 1X
            .dblIDReadPos1Y = 110.0                         ' IDذ�ޓǂݎ���߼޼�� 1Y
            .dblIDReadPos2X = 60.0                          ' IDذ�ޓǂݎ���߼޼�� 2X
            .dblIDReadPos2Y = 110.0                         ' IDذ�ޓǂݎ���߼޼�� 2Y
            .dblReprobeVar = 0.0                            ' ����۰��ݸނ΂����
            .dblReprobePitch = 0.0                          ' ����۰��ݸ��߯�

            'V4.10.0.0�C            ��
            .dblPrbCleanStagePitchX = 0.0                   ' �X�e�[�W����s�b�`X
            .dblPrbCleanStagePitchY = 0.0                   ' �X�e�[�W����s�b�`Y
            .intPrbCleanStageCountX = 0                     ' �X�e�[�W�����X
            .intPrbCleanStageCountY = 0                     ' �X�e�[�W�����Y
            'V4.10.0.0�C            ��
            'V4.10.0.0�H��
            .dblPrbDistance = 0.0                           ' �v���[�u�ԋ����imm�j
            .dblPrbCleaningOffset = 0.0                     ' �N���[�j���O�I�t�Z�b�g(mm)
            'V4.10.0.0�H��

            '<OPTION DATA> (�I�v�V�����^�u'V5.0.0.6�@)
            .intControllerInterlock = 0                     ' �O���@��ɂ��C���^�[���b�N�̗L���i�^�cKOA�a�̉��x�R���g���[���̃C���^�[���b�N�Ɏg�p�j'V5.0.0.6�@

            .dblTXChipsizeRelationX = 0.0                   ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
            .dblTXChipsizeRelationY = 0.0                   ' �␳�ʒu�P�ƂQ�̑��Βl�x 'V4.5.1.0�N

        End With
    End Sub
#End Region
    '----- ###229�� -----
#Region "GPIB�ް��\���̂̏�����"
    '''=========================================================================
    '''<summary>GPIB�ް��\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_typGpibInfo()

        With typGpibInfo
            .wGPIBmode = 0                                              ' GP-IB����(0:���Ȃ� 1:����)
            .wDelim = 0                                                 ' �����(0:CR+LF 1:CR 2:LF 3:NONE)
            .wTimeout = 300                                             ' ��ѱ��(1�`32767)(ms�P��)
            .wAddress = 0                                               ' �@����ڽ(0�`30)
            .wEOI = 0                                                   ' EOI(0:�g�p���Ȃ�, 1:�g�p����)
            .wPause1 = 0                                                ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
            .wPause2 = 0                                                ' �ݒ�����2���M��|�[�Y����(1�`32767msec)
            .wPause3 = 0                                                ' �ݒ�����3���M��|�[�Y����(1�`32767msec)
            .wPauseT = 0                                                ' �ض޺���ޑ��M��|�[�Y����(1�`32767msec)
            .wRev = 0                                                   ' �\��
            .strI = ""                                                  ' �����������(MAX40byte)
            .strI2 = ""                                                 ' �����������2(MAX40byte)
            .strI3 = ""                                                 ' �����������3(MAX40byte)
            .strT = "READ?"                                             ' �ض޺����(50byte)
            .strName = ""                                               ' �@�햼(10byte)
            .wReserve = ""                                              ' �\��(8byte)  
        End With
    End Sub
#End Region
    '----- ###229�� -----
#Region "�ï�ߍ\���̂̏�����"
    '''=========================================================================
    '''<summary>�ï�ߍ\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typStepInfoArray()

        Dim i As Short

        ' �ï�ߍ\���̐�������������
        For i = 1 To MaxCntStep
            With typStepInfoArray(i)
                .intSP1 = 1                                 ' �ï�ߔԍ�
                .intSP2 = 1                                 ' ��ۯ���
                .dblSP3 = 0.0#                              ' �ï�ߊԲ������
            End With
        Next i

    End Sub
#End Region
#End If
#End Region 'V5.0.0.8�@

#Region "��ٰ�ߍ\���̂̏�����"
    '''=========================================================================
    '''<summary>��ٰ�ߍ\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typGrpInfoArray()

        Dim i As Short
#If False Then 'V5.0.0.8�@ LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�ŏ���
        ' ��ٰ�ߍ\���̐�������������
        For i = 1 To MaxCntStep
            With typGrpInfoArray(i)
                .intGP1 = 1                     ' ��ٰ�ߔԍ�
                .intGP2 = 1                     ' ��R��
                .dblGP3 = 0.0#                  ' ��ٰ�ߊԲ������
                .dblStgPosX = 0.0#              ' �X�e�[�W�|�W�V����X
                .dblStgPosY = 0.0#              ' �X�e�[�W�|�W�V����Y
            End With
        Next i
#End If
        For i = 0 To MAXCNT_PLT_BLK
            gPltStagePosX(i) = 0.0#             ' �v���[�g�̃X�e�[�WX�ʒu���W�f�[�^
            gPltStagePosY(i) = 0.0#             ' �v���[�g�̃X�e�[�WY�ʒu���W�f�[�^
            gBlkStagePosX(i) = 0.0#             ' �O���[�v�̃X�e�[�WX�ʒu���W�f�[�^
            gBlkStagePosY(i) = 0.0#             ' �O���[�v�̃X�e�[�WY�ʒu���W�f�[�^
        Next
    End Sub
#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
#Region "�T�[�L�b�g�\���̂̏�����"
    '''=========================================================================
    '''<summary>�T�[�L�b�g�\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_typCircuitInfo()

        Dim i As Short

        ' �z��̐擪��������
        With typCircuitInfoArray(1)
            .intIP1 = 0                                 ' IP�ԍ�
            .dblIP2X = 0.0#                             ' �}�[�L���OX
            .dblIP2Y = 0.0#                             ' �}�[�L���OY
        End With

        ' �z��̎c���������
        For i = 2 To UBound(typCircuitInfoArray)
            typCircuitInfoArray(i) = typCircuitInfoArray(1)
        Next i

    End Sub
#End Region

#Region "��R�ް��\���̂̏�����"
    '''=========================================================================
    '''<summary>��R�ް��\���̂̏�����</summary>
    '''<param name="stREG">(INP) ��R�ԍ�</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typResistorInfoArray(ByVal stREG() As ResistorInfo)

        Dim rn As Short

        For rn = 1 To MaxCntResist
            stREG(rn).intResNo = rn                              ' ��R�ԍ�
            stREG(rn).intResMeasMode = 0                         ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��) ���ύX
            stREG(rn).intResMeasType = 1                         ' ����^�C�v(0:���� ,1:�����x)�@���ǉ�
            stREG(rn).intCircuitGrp = 1                          ' ���������
            stREG(rn).intProbHiNo = 1                            ' ��۰�ޔԍ�(HI��)
            stREG(rn).intProbLoNo = 2                            ' ��۰�ޔԍ�(LO��)
            stREG(rn).intProbAGNo1 = 0                           ' ��۰�ޔԍ�(1)
            stREG(rn).intProbAGNo2 = 0                           ' ��۰�ޔԍ�(2)
            stREG(rn).intProbAGNo3 = 0                           ' ��۰�ޔԍ�(3)
            stREG(rn).intProbAGNo4 = 0                           ' ��۰�ޔԍ�(4)
            stREG(rn).intProbAGNo5 = 0                           ' ��۰�ޔԍ�(5)
            stREG(rn).strExternalBits = "00000000"               ' EXTERNAL BITS
            stREG(rn).intPauseTime = 0                           ' �߰�����
            stREG(rn).intTargetValType = 0                       ' �ڕW�l�w��
            stREG(rn).intBaseResNo = 0                           ' �ް���R�ԍ�
            stREG(rn).dblTrimTargetVal = 1.0#                    ' ���ݸޖڕW�l
            stREG(rn).dblTrimTargetVal_Save = stREG(rn).dblTrimTargetVal    ' V4.11.0.0�@ ���ݸޖڕW�l�ޔ�p���[�N
            stREG(rn).dblTrimTargetOfs = 0.0                     ' V4.11.0.0�@ ���ݸޖڕW�l
            stREG(rn).strRatioTrimTargetVal = ""                 ' �g���~���O�ڕW�l(���V�I�v�Z��) 
            stREG(rn).dblDeltaR = 0.0#                           ' ���q
            stREG(rn).intSlope = 0                               ' �d���ω��۰��
            stREG(rn).dblCutOffRatio = 0.0#                      ' �؂�グ�{��
            stREG(rn).dblProbCfmPoint_Hi_X = 0.0#                ' �v���[�u�m�F�ʒu HI X���W
            stREG(rn).dblProbCfmPoint_Hi_Y = 0.0#                ' �v���[�u�m�F�ʒu HI Y���W
            stREG(rn).dblProbCfmPoint_Lo_X = 0.0#                ' �v���[�u�m�F�ʒu LO X���W
            stREG(rn).dblProbCfmPoint_Lo_Y = 0.0#                ' �v���[�u�m�F�ʒu LO Y���W
            stREG(rn).dblInitTest_HighLimit = 0.0#               ' �Ƽ��ý�HIGH�Я�
            stREG(rn).dblInitTest_LowLimit = 0.0#                ' �Ƽ��ý�LOW�Я�
            stREG(rn).dblFinalTest_HighLimit = 0.0#              ' ̧���ý�HIGH�Я�
            stREG(rn).dblFinalTest_LowLimit = 0.0#               ' ̧���ý�LOW�Я�
            stREG(rn).dblInitOKTest_HighLimit = 0.0#             ' �Ƽ��OKý�HIGH�Я� �����g�p
            stREG(rn).dblInitOKTest_LowLimit = 0.0#              ' �Ƽ��OKý�LOW�Я�  �����g�p
            stREG(rn).intInitialOkTestDo = 0                     ' �����n�j����(0:���Ȃ�,1:����)���ǉ�(�v���[�g�f�[�^����ړ�)
            stREG(rn).intCutCount = 1                            ' ��Đ�
            stREG(rn).intCutReviseMode = 0                       ' ��� �␳
            stREG(rn).intCutReviseDispMode = 0                   ' �\��Ӱ��
            stREG(rn).intCutReviseGrpNo = 1                      ' ����ݸ�ٰ�ߔԍ�  
            stREG(rn).intCutRevisePtnNo = 1                      ' ����� No.
            stREG(rn).dblCutRevisePosX = 0.0#                    ' ��ĕ␳�ʒuX
            stREG(rn).dblCutRevisePosY = 0.0#                    ' ��ĕ␳�ʒuY
            stREG(rn).intIsNG = 0                                ' NG�L��
            '----- V1.13.0.0�A�� -----
            stREG(rn).intCvMeasNum = 0                          ' CV �ő呪���
            stREG(rn).intCvMeasTime = 0                         ' CV �ő呪�莞��(ms) 
            stREG(rn).dblCvValue = 0.0                          ' CV CV�l         
            stREG(rn).intOverloadNum = 0                        ' ���ް۰�� �� 
            stREG(rn).dblOverloadMin = 0.0                      ' ���ް۰�� �����l 
            stREG(rn).dblOverloadMax = 0.0                      ' ���ް۰�� ����l
            '----- V1.13.0.0�A�� -----

            stREG(rn).Initialize()                               ' ��ď��
            Call Init_typCutInfoArray(stREG, rn)                 ' ����ް��\���̂̏�����
        Next rn

    End Sub
#End Region

#Region "����ް��\���̂̏�����"
    '''=========================================================================
    '''<summary>����ް��\���̂̏�����</summary>
    '''<param name="stREG">(INP)��R�ް��\���̔z��</param>
    '''<param name="rn">   (INP)��R�ԍ�(1ORG)</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typCutInfoArray(ByVal stREG() As ResistorInfo, ByVal rn As Integer)

        Dim cn As Short

        ' ����ް��\���̂�������
        For cn = 1 To MaxCutInfo
            stREG(rn).ArrCut(cn).intCutNo = cn                  ' ��Ĕԍ�
            stREG(rn).ArrCut(cn).intDelayTime = 0               ' �ިڲ���
            stREG(rn).ArrCut(cn).strCutType = "A"               ' ��Č`��
            stREG(rn).ArrCut(cn).dblTeachPointX = 0.0#          ' è��ݸ��߲��X
            stREG(rn).ArrCut(cn).dblTeachPointY = 0.0#          ' è��ݸ��߲��Y
            stREG(rn).ArrCut(cn).dblStartPointX = 0.0#          ' �����߲��X
            stREG(rn).ArrCut(cn).dblStartPointY = 0.0#          ' �����߲��Y
            stREG(rn).ArrCut(cn).dblCutSpeed = 0.1              ' ��Ľ�߰��
            stREG(rn).ArrCut(cn).dblQRate = 0.1                 ' Q����ڰ�
            stREG(rn).ArrCut(cn).dblCutOff = 0.0#               ' ��ĵ̒l
            stREG(rn).ArrCut(cn).dblJudgeLevel = 0.0#           ' �ؑփ|�C���g (���ް�����(���ω���))
            stREG(rn).ArrCut(cn).dblCutOffOffset = 0.0#         ' ��ĵ̵̾��
            stREG(rn).ArrCut(cn).intPulseWidthCtrl = 0          ' ��ٽ������
            stREG(rn).ArrCut(cn).dblPulseWidthTime = 0          ' ��ٽ������
            stREG(rn).ArrCut(cn).dblLSwPulseWidthTime = 1.0#    ' LSw��ٽ������
            stREG(rn).ArrCut(cn).intCutDir = 1                  ' ��ĕ���
            stREG(rn).ArrCut(cn).intLTurnDir = 1                ' L��ݕ���(1:CW, 2:CCW)       V4.0.0.0-67
            stREG(rn).ArrCut(cn).dblMaxCutLength = 0.1#         ' �ő嶯èݸޒ�
            stREG(rn).ArrCut(cn).dblLTurnPoint = 0.0#           ' L����߲��
            stREG(rn).ArrCut(cn).dblMaxCutLengthL = 0.1#        ' L��݌�̍ő嶯èݸޒ�
            stREG(rn).ArrCut(cn).dblMaxCutLengthHook = 0.1#     ' ̯���݌�̶�èݸޒ�
            stREG(rn).ArrCut(cn).dblR1 = 0.0#                   ' R1
            stREG(rn).ArrCut(cn).dblR2 = 0.0#                   ' R2
            stREG(rn).ArrCut(cn).intCutAngle = 0                ' �΂߶�Ă̐؂�o���p�x
            stREG(rn).ArrCut(cn).dblCutSpeed2 = 0.1             ' ��Ľ�߰��2
            stREG(rn).ArrCut(cn).dblQRate2 = 0.1                ' Q����ڰ�2
            '''''2009/08/17
            ''''    ��436K�̃p�����[�^�B432��INTRTM���ł͍\���̒�`�Ȃ��ׁA��U�폜
            ''''            stREG(rn).ArrCut(cn).dblCP53 = 0#     'Q����ڰ�3
            ''''            stREG(rn).ArrCut(cn).dblCP54 = 0#     '�ؑւ��߲��
            stREG(rn).ArrCut(cn).dblESPoint = 0.0#              ' ���޾ݽ�߲��
            stREG(rn).ArrCut(cn).dblESJudgeLevel = 0.0#         ' ���޾ݽ�̔���ω���
            stREG(rn).ArrCut(cn).dblMaxCutLengthES = 0.1        ' ���޾ݽ��̶�Ē�
            stREG(rn).ArrCut(cn).intIndexCnt = 1                ' ���ޯ����
            stREG(rn).ArrCut(cn).intMeasMode = 0                ' ����Ӱ��
            stREG(rn).ArrCut(cn).dblPitch = 0.1#                ' �߯�
            stREG(rn).ArrCut(cn).intStepDir = 1                 ' �ï�ߕ���
            stREG(rn).ArrCut(cn).intCutCnt = 1                  ' �{��
            stREG(rn).ArrCut(cn).dblUCutDummy1 = 0.0#           ' U��ėp��а
            stREG(rn).ArrCut(cn).dblUCutDummy2 = 0.0#           ' U��ėp��а
            stREG(rn).ArrCut(cn).dblESChangeRatio = 0.0#        ' ���޾ݽ��̕ω���
            stREG(rn).ArrCut(cn).intESConfirmCnt = 0            ' ���޾ݽ��̊m�F��
            stREG(rn).ArrCut(cn).intRadderInterval = 0          ' ��ް�ԋ���
            '----- V1.14.0.0�@�� -----
            stREG(rn).ArrCut(cn).intCTcount = 0                 ' ���޾ݽ��A��NG�m�F�񐔁��ǉ�(ES�p)
            stREG(rn).ArrCut(cn).intJudgeNg = 0                 ' NG���肷��/���Ȃ�(0:TRUE/1:FALSE)���ǉ�(ES�p)
            '----- V1.14.0.0�@�� -----
            stREG(rn).ArrCut(cn).strDataName = ""               ' U�J�b�g�f�[�^�����ǉ�   
            stREG(rn).ArrCut(cn).intMoveMode = 0                ' ���샂�[�h(0:�ʏ탂�[�h, 2:�����J�b�g���[�h)�@���ǉ� 
            stREG(rn).ArrCut(cn).intDoPosition = 0              ' �|�W�V���j���O(0:�L, 1:��)�@���ǉ�
            stREG(rn).ArrCut(cn).dblReturnPos = 0.0             ' ���^�[���J�b�g�̃��^�[���ʒu 'V1.16.0.0�@
            stREG(rn).ArrCut(cn).dblLimitLen = 0.0              ' IX�J�b�g�̃��~�b�g�� 'V1.18.0.0�C

            '�폜����\��
            stREG(rn).ArrCut(cn).dblZoom = 1.0#                 ' �{��
            stREG(rn).ArrCut(cn).strChar = "TEST"               ' ������

            ' FL�p�ɉ��H������ǉ�
            stREG(rn).ArrCut(cn).dblCutSpeed3 = 0.1             ' ��Ľ�߰��3(L��Ă�����/��ڰ�����L��ݑO�̽�߰��)
            stREG(rn).ArrCut(cn).dblCutSpeed4 = 0.1             ' ��Ľ�߰��4(L��Ă�����/��ڰ�����L��݌�̽�߰��)
            stREG(rn).ArrCut(cn).dblCutSpeed5 = 0.1             ' ��Ľ�߰��5(u��Ă�����/��ڰ�����L��݌�̽�߰��)
            stREG(rn).ArrCut(cn).dblCutSpeed6 = 0.1             ' ��Ľ�߰��6(u��Ă�����/��ڰ�����L��݌�̽�߰��)
            stREG(rn).ArrCut(cn).Initialize()                   ' ���H�����ԍ�1�`n(0�`31)

            'V4.0.0.0-67                ��������
            stREG(rn).ArrCut(cn).dblQRate2 = 0.1                ' Q����ڰ�3(L��Ă�����/��ڰ�����L��ݑO��Q����ڰ�)
            stREG(rn).ArrCut(cn).dblQRate3 = 0.1                ' Q����ڰ�4(L��Ă�����/��ڰ�����L��݌��Q����ڰ�)
            stREG(rn).ArrCut(cn).dblQRate4 = 0.1                ' Q����ڰ�5(U��Ă�����/��ڰ�����L��݌��Q����ڰ�) ���g�p
            stREG(rn).ArrCut(cn).dblQRate5 = 0.1                ' Q����ڰ�6(U��Ă�����/��ڰ�����L��݌��Q����ڰ�) ���g�p
            stREG(rn).ArrCut(cn).dblQRate6 = 0.1
            'V4.0.0.0-67                ��������

            'V4.0.0.0-38                ��������
            ' �����ڕW�p���[�ƒ������e�͈͂ɏ����l��ݒ肷��
            For i As Integer = 0 To (cCNDNUM - 1) Step 1
                stREG(rn).ArrCut(cn).dblPowerAdjustTarget(i) = gwModule.POWERADJUST_TARGET
                stREG(rn).ArrCut(cn).dblPowerAdjustToleLevel(i) = gwModule.POWERADJUST_LEVEL
                stREG(rn).ArrCut(cn).FLCurrent(i) = gwModule.POWERADJUST_CURRENT
                stREG(rn).ArrCut(cn).FLSteg(i) = gwModule.POWERADJUST_STEG
            Next i
            'V4.0.0.0-38                ��������
        Next cn

    End Sub

#End Region

#Region "TY2�\���̂̏�����"
    '''=========================================================================
    '''<summary>TY2�\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typTy2InfoArray()

        Dim i As Short

        ' TY2�\���̂������� ###119
        For i = 1 To MaxCntTy2
            With typTy2InfoArray(i)
                .intTy21 = i                                            ' ��ۯ��ԍ�
                .dblTy22 = 0.0#                                         ' �ï�ߋ���
            End With
        Next i

    End Sub
#End Region
#End If
#End Region 'V5.0.0.8�@

#Region "�g���~���O���ʎ擾�G���A�̏�����"
    '''=========================================================================
    '''<summary>�g���~���O���ʎ擾�G���A�̏���������</summary>
    '''=========================================================================
    Public Sub Init_TrimResultData()
        Dim regCnt As Integer

        Try
            For regCnt = 0 To MaxCntResist                        ' MaxCntResist-1
                gwTrimResult(regCnt) = TRIM_RESULT_NOTDO
            Next


        Catch ex As Exception
            Dim strMsg As String
            strMsg = "DataAccess.Init_TrimResultData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
#Region "�ٌ`�ʕt���f�[�^�\���̂̏�����"
    '''=========================================================================
    '''<summary>�ٌ`�ʕt���f�[�^�\���̂̏�����</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Init_typIKEIInfo()

        typIKEIInfo.intI1 = 0                               ' �ٌ`�ʕt���̗L���i0:����,1:X����,2:Y����
        typIKEIInfo.Initialize()                            ' �T�[�L�b�g�̗L���i0:����,1:�L��j

    End Sub
#End Region
#End If
#End Region  'V5.0.0.8�@

#Region "��ٰ�ߓ����ߐ��擾"
    '''=========================================================================
    '''<summary>��ٰ�ߓ����ߐ��擾</summary>
    '''<param name="intChipNum">(OUT) ���ߐ�</param>
    '''<remarks>CHIP��NET�ŏ������قȂ�̂ŗv����</remarks>
    '''=========================================================================
    Public Sub GetChipNum(ByRef intChipNum As Short)

        If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_CHIP) Then
            intChipNum = typPlateInfo.intResistCntInGroup

        ElseIf (gTkyKnd = KND_NET) Then
            Dim CirNum As Integer
            CirNum = typPlateInfo.intCircuitCntInBlock  ' 1��ۯ��໰��Đ�
            intChipNum = typPlateInfo.intResistCntInGroup * CirNum
        End If

    End Sub
#End Region

#Region "�߸�ْlX,Y�擾"
    '''=========================================================================
    '''<summary>�߸�ْlX,Y�擾</summary>
    '''<param name="dblPixel2umX">(OUT) �߸�ْlX</param>
    '''<param name="dblPixel2umY">(OUT) �߸�ْlY</param>
    '''<remarks>1�߸�ق������mm��Ԃ�</remarks>
    '''=========================================================================
    Public Sub GetPixel2um(ByRef dblPixel2umX As Double, ByRef dblPixel2umY As Double)

        If gSysPrm.stDEV.giEXCAM = 1 Then               ' �O���J���� ? 
            dblPixel2umX = gSysPrm.stGRV.gfEXCAM_PixelX ' 1�߸�ق������mm X
            dblPixel2umY = gSysPrm.stGRV.gfEXCAM_PixelY ' 1�߸�ق������mm Y
        Else
            dblPixel2umX = gSysPrm.stGRV.gfPixelX       ' 1�߸�ق������mm X
            dblPixel2umY = gSysPrm.stGRV.gfPixelY       ' 1�߸�ق������mm Y
        End If

    End Sub
#End Region

#Region "�v���[�g�����擾����"
    '''=========================================================================
    '''<summary>�v���[�g�����擾����</summary>
    '''=========================================================================
    Public Function GetPlateCnt() As Integer
        Try
            With typPlateInfo
                ' Short * Short ���� 256 * 256 �ŃI�[�o�[�t���[����
                'V5.0.0.9�O                GetPlateCnt = .intPlateCntXDir * .intPlateCntYDir
                Dim x As Integer = .intPlateCntXDir
                Dim y As Integer = .intPlateCntYDir
                GetPlateCnt = (x * y)

                Exit Function
            End With
        Catch ex As Exception
            Dim strMSG As String
            strMSG = "DataAccess.GetPlateCnt() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "�v���[�g���̃u���b�N�����擾����"

    '''=========================================================================
    '''<summary>�v���[�g���̃u���b�N�����擾����</summary>
    '''=========================================================================
    Public Function GetBlockCnt() As Integer
        Try
            With typPlateInfo
                ' Short * Short ���� 256 * 256 �ŃI�[�o�[�t���[����
                'V5.0.0.9�O                GetBlockCnt = .intBlockCntXDir * .intBlockCntYDir
                Dim x As Integer = .intBlockCntXDir
                Dim y As Integer = .intBlockCntYDir
                GetBlockCnt = (x * y)

                Exit Function
            End With

        Catch ex As Exception
            Dim strMSG As String
            strMSG = "DataAccess.GetBlockCnt() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "�u���b�N�T�C�Y���Z�o����yCHIP/NET�p�z"
    '''=========================================================================
    '''<summary>�u���b�N�T�C�Y���Z�o����yCHIP/NET�p�z</summary>
    '''<param name="dblBSX">(OUT) �u���b�N�T�C�YX</param>
    '''<param name="dblBSY">(OUT) �u���b�N�T�C�YY</param>
    '''=========================================================================
    Public Sub CalcBlockSize(ByRef dblBSX As Double, ByRef dblBSY As Double)

        Dim i As Integer
        Dim intChipNum As Integer
        Dim intGNx As Integer
        Dim intGNY As Integer
        Dim dData As Double = 0.0

        Try
            ' CHIP/NET�� 
            ' �O���[�v��X,Y
            intGNx = typPlateInfo.intGroupCntInBlockXBp                 ' �a�o�O���[�v��(�T�[�L�b�g��)
            intGNY = typPlateInfo.intGroupCntInBlockXBp

            ' �O���[�v����R��             
            intChipNum = typPlateInfo.intResistCntInGroup

            ' �u���b�N�T�C�YX,Y�����߂�
            If (typPlateInfo.intResistDir = 0) Then                     ' ��R(����)���ѕ���(0:X, 1:Y)
                ' ��R(����)���ѕ��� = X�����̏ꍇ
                If (intGNx = 1) Then
                    ' 1�O���[�v(1�T�[�L�b�g)�̏ꍇ
                    dData = typPlateInfo.dblChipSizeXDir * intChipNum   ' Data = �`�b�v�T�C�YX * �`�b�v��

                Else
                    ' �����O���[�v(�����T�[�L�b�g)�̏ꍇ
                    For i = 1 To intGNx
                        If (i = intGNx) Then                            ' �ŏI�O���[�v ?
                            ' Data = Data + (�`�b�v�T�C�YX * �O���[�v��(�T�[�L�b�g��)��R��)
                            dData = dData + (typPlateInfo.dblChipSizeXDir * typPlateInfo.intResistCntInGroup)
                        Else
                            ' Data = Data + (�`�b�v�T�C�YX * �O���[�v��(�T�[�L�b�g��)��R�� + �a�o�O���[�v(�T�[�L�b�g)�Ԋu)
                            dData = dData + (typPlateInfo.dblChipSizeXDir * typPlateInfo.intResistCntInGroup + typPlateInfo.dblBpGrpItv)
                        End If
                    Next i
                End If

                ' �u���b�N�T�C�YX,Y��Ԃ�
                dblBSX = dData                                          ' �u���b�N�T�C�YX = �v�Z�l
                dblBSY = typPlateInfo.dblChipSizeYDir                   ' �u���b�N�T�C�YY = �`�b�v�T�C�YY

            Else
                ' ��R(����)���ѕ��� = Y�����̏ꍇ
                If (intGNY = 1) Then
                    ' 1�O���[�v(1�T�[�L�b�g)�̏ꍇ
                    dData = typPlateInfo.dblChipSizeYDir * intChipNum   ' Data = �`�b�v�T�C�YY * �`�b�v��

                Else
                    ' �����O���[�v(�����T�[�L�b�g)�̏ꍇ
                    For i = 1 To intGNY
                        If (i = intGNY) Then                            ' �ŏI�O���[�v ?
                            ' Data = Data + (�`�b�v�T�C�YY * �O���[�v��(�T�[�L�b�g��)��R��)
                            dData = dData + (typPlateInfo.dblChipSizeYDir * typPlateInfo.intResistCntInGroup)
                        Else
                            ' Data = Data + (�`�b�v�T�C�YY * �O���[�v��(�T�[�L�b�g��)��R�� + �a�o�O���[�v(�T�[�L�b�g)�Ԋu)
                            dData = dData + (typPlateInfo.dblChipSizeYDir * typPlateInfo.intResistCntInGroup + typPlateInfo.dblBpGrpItv)
                        End If
                    Next i

                End If

                ' �u���b�N�T�C�YX,Y��Ԃ�
                dblBSX = typPlateInfo.dblChipSizeXDir                   ' �u���b�N�T�C�YX = �`�b�v�T�C�YX
                dblBSY = dData                                          ' �u���b�N�T�C�YY = �v�Z�l

            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            Dim strMSG As String
            strMSG = "DataAccess.CalcBlockSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "���O�\���̌����ɁA�w��u���b�NXY����X�e�[�W�O���[�v�ԍ��A�u���b�N�ԍ����擾����"
    '''=========================================================================
    '''<summary>�w��u���b�NXY�̃|�W�V������񂩂�A</summary>
    ''' <param name="curBlkNoX">(INP)���݃u���b�N��X�ʒu�i�s�ʒu�j</param>
    ''' <param name="curBlkNoY">(INP)���݃u���b�N��Y�ʒu�i��ʒu�j</param>
    ''' <param name="stgGrpNoX">(OUT)�X�e�[�W�O���[�v�ԍ�X</param>
    ''' <param name="stgGrpNoY">(OUT)�X�e�[�W�O���[�v�ԍ�Y</param>
    ''' <param name="blockNoX"> (OUT)�X�e�[�W�O���[�v�ԍ������������u���b�N�ԍ�X</param>
    ''' <param name="blockNoY"> (OUT)�X�e�[�W�O���[�v�ԍ������������u���b�N�ԍ�Y</param>
    '''<returns></returns>
    '''<remarks></remarks>
    '''  �v���[�g�̕��с@�@�v���[�g�����̕���
    '''  ____ ____ ____      ____ ____
    ''' | �H | �G | �@ |�@�@| �G | �@ |
    ''' |____|____|____|    |____|____|
    ''' | �I | �F | �A |    | �F | �A |    
    ''' |____|____|____|    |____|____|
    ''' | �J | �E | �B |    | �E | �B |
    ''' |____|____|____|    |____|____|
    ''' | �K | �D | �C |    | �D | �C |
    ''' |____|____|____|    |____|____|
    ''' 
    '''=========================================================================
    Public Function GetDisplayPosInfo(ByVal curBlkNoX As Integer, ByVal curBlkNoY As Integer, _
                ByRef stgGrpNoX As Integer, ByRef stgGrpNoY As Integer, _
                ByRef blockNoX As Integer, ByRef blockNoY As Integer) As Boolean

        Dim strMSG As String

        GetDisplayPosInfo = True
        Try

            '----- ###165�� -----
            With typPlateInfo

                If (.intResistDir = 0) Then                             ' ��R(����)���ѕ��� = X�����̏ꍇ
                    ' �X�e�[�W�O���[�v�ԍ�X = 1
                    stgGrpNoX = 1
                    ' �X�e�[�W�O���[�v�ԍ�Y = �u���b�N�ԍ�Y / �X�e�[�W�O���[�v���u���b�N��
                    If ((curBlkNoY Mod .intBlkCntInStgGrpY) <> 0) Then  ' �]��L�� ? 
                        stgGrpNoY = curBlkNoY \ .intBlkCntInStgGrpY + 1
                    Else
                        stgGrpNoY = curBlkNoY \ .intBlkCntInStgGrpY
                    End If

                    ' �u���b�N�ԍ�X,Y
                    blockNoX = curBlkNoX
                    blockNoY = curBlkNoY

                Else                                                    ' ��R(����)���ѕ��� = Y�����̏ꍇ
                    ' �X�e�[�W�O���[�v�ԍ�Y = 1
                    stgGrpNoY = 1
                    ' �X�e�[�W�O���[�v�ԍ�X = �u���b�N�ԍ�X / �X�e�[�W�O���[�v���u���b�N��
                    If ((curBlkNoY Mod .intBlkCntInStgGrpY) <> 0) Then  ' �]��L�� ? 
                        stgGrpNoX = curBlkNoX \ .intBlkCntInStgGrpX + 1
                    Else
                        stgGrpNoX = curBlkNoX \ .intBlkCntInStgGrpX
                    End If

                    ' �u���b�N�ԍ�X,Y
                    blockNoX = curBlkNoX
                    blockNoY = curBlkNoY
                End If

            End With

            'With typPlateInfo
            '    '�X�e�[�W�O���[�v�Ԋu�����������u���b�N�ʒu�̎Z�o
            '    '   ���`�b�v�����X�e�b�v������ꍇ�̃X�e�b�v�̃J�E���g���@��ʓr�������K�v
            '    ' X����
            '    'If (.intBlkCntInStgGrpX <> 0) Then
            '    If (.dblStgGrpItvX <> 0) Then
            '        '   �X�e�[�W�O���[�v�Ԋu���ݒ肳��Ă���ꍇ�B
            '        '   ���u���݃u���b�N/�X�e�[�W�O���[�v���v���X�e�[�W�O���[�v�̔ԍ�
            '        '   �@�u���݃u���b�N/�X�e�[�W�O���[�v���̗]��v���X�e�[�W�O���[�v�����̃u���b�N�ԍ�
            '        'X�����`�b�v�X�e�b�v������ꍇ
            '        '   ���`�b�v�X�e�b�v�̃J�E���g�����Z����B
            '        '   �@�X�e�[�W�O���[�v�́A�u���݂̃u���b�N/�i�X�e�[�W�O���[�v���u���b�N�����`�b�v�X�e�b�v���j�v
            '        '   �@�u���b�N���́A�u���݂̃u���b�N/�`�b�v�X�e�b�v���v
            '        '   �@�`�b�v�X�e�b�v���́A�u(���݂̃u���b�N/�`�b�v�X�e�b�v��)�̗]��v
            '        stgGrpNoX = (curBlkNoX + 1) \ .intBlkCntInStgGrpX
            '        blockNoX = curBlkNoX Mod .intBlkCntInStgGrpX
            '        If (blockNoX = 0) Then
            '            blockNoX = .intBlkCntInStgGrpX
            '        End If
            '    Else
            '        '   �X�e�[�W�O���[�v�Ԋu���ݒ肳��Ă��Ȃ��ꍇ
            '        '   �����݃u���b�N=�X�e�[�W�O���[�v�ԍ�
            '        '   �@���݃u���b�N=�u���b�N�ԍ�
            '        stgGrpNoX = curBlkNoX
            '        blockNoX = curBlkNoX
            '    End If

            '    'Y����
            '    'If (.intBlkCntInStgGrpY <> 0) Then
            '    If (.dblStgGrpItvY <> 0) Then
            '        '   �X�e�[�W�O���[�v�Ԋu���ݒ肳��Ă���ꍇ�B
            '        '   ���u���݃u���b�N/�X�e�[�W�O���[�v���v���X�e�[�W�O���[�v�̔ԍ�
            '        '   �@�u���݃u���b�N/�X�e�[�W�O���[�v���̗]��v���X�e�[�W�O���[�v�����̃u���b�N�ԍ�
            '        stgGrpNoY = (curBlkNoY + 1) \ .intBlkCntInStgGrpY
            '        blockNoY = curBlkNoY Mod .intBlkCntInStgGrpY
            '        If (blockNoY = 0) Then
            '            blockNoY = .intBlkCntInStgGrpY
            '        End If
            '    Else
            '        '   �X�e�[�W�O���[�v�Ԋu���ݒ肳��Ă��Ȃ��ꍇ
            '        '   �����݃u���b�N=�X�e�[�W�O���[�v�ԍ�
            '        '   �@���݃u���b�N=�u���b�N�ԍ�
            '        stgGrpNoY = curBlkNoY
            '        blockNoY = curBlkNoY
            '    End If
            'End With
            '----- ###165�� -----

        Catch ex As Exception
            strMSG = "DataAccess.GetDisplayPosInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetDisplayPosInfo = False
        End Try
    End Function
#End Region

    '#Region "�w��u���b�N�̃X�e�[�W�ʒu���擾-(���g�p)"
    '    '''=========================================================================
    '    '''<summary>�w��u���b�N�̍��W�ʒu���擾����</summary>
    '    '''<param name="plateNo"> (INP) �v���[�g�ԍ�</param>
    '    '''<param name="blockNo">(INP)�v���[�g���̃u���b�N�ԍ�</param>
    '    '''<param name="stgx">(OUT)�X�e�[�WX���W</param>
    '    '''<param name="stgy">(OUT)�X�e�[�WY���W</param>
    '    '''<returns>�ŏI�v���[�g�A�ŏI�u���b�N�̏ꍇTRUE��Ԃ��B</returns>
    '    '''<remarks></remarks>
    '    '''  �v���[�g�̕��с@�@�v���[�g�����̕���
    '    '''  ____ ____ ____      ____ ____
    '    ''' | �H | �G | �@ |�@�@| �G | �@ |
    '    ''' |____|____|____|    |____|____|
    '    ''' | �I | �F | �A |    | �F | �A |    
    '    ''' |____|____|____|    |____|____|
    '    ''' | �J | �E | �B |    | �E | �B |
    '    ''' |____|____|____|    |____|____|
    '    ''' | �K | �D | �C |    | �D | �C |
    '    ''' |____|____|____|    |____|____|
    '    ''' 
    '    '''=========================================================================
    '    Public Function GetTargetStagePos(ByVal plateNo As Integer, ByVal blockNo As Integer, ByRef stgx As Double, ByRef stgy As Double) As Boolean
    '        Dim intPlateXCnt As Integer
    '        Dim intPlateYCnt As Integer
    '        Dim intLastPlateNo As Integer
    '        Dim dblWorkBaseStgPosX As Double
    '        Dim dblWorkBaseStgPosY As Double
    '        Dim strMSG As String

    '        GetTargetStagePos = False

    '        Try

    '            '�v���[�g�ɂ��x�[�X�̈ʒu���W���擾����B
    '            With typPlateInfo
    '                '�v���[�g�Ԋu�̌v�Z
    '                intPlateXCnt = plateNo / .intPlateCntXDir
    '                If (intPlateXCnt Mod 2) = 0 Then
    '                    intPlateYCnt = plateNo Mod .intPlateCntYDir
    '                Else
    '                    intPlateYCnt = (.intPlateCntYDir + 1) - (plateNo Mod .intPlateCntYDir)
    '                End If

    '                '�v���[�g�̃x�[�X�|�W�V����
    '                dblWorkBaseStgPosX = (.dblPlateSizeX * intPlateXCnt)
    '                dblWorkBaseStgPosY = (.dblPlateSizeY * intPlateYCnt)

    '                '�u���b�N���W���擾����
    '                stgx = dblWorkBaseStgPosX + typGrpInfoArray(blockNo).dblStgPosX
    '                stgy = dblWorkBaseStgPosY + typGrpInfoArray(blockNo).dblStgPosY

    '                '�ŏI�v���[�g�̔���
    '                intLastPlateNo = .intPlateCntXDir * .intPlateCntYDir
    '                If (plateNo = intLastPlateNo) Then
    '                    GetTargetStagePos = True
    '                End If
    '            End With

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            strMSG = "DataAccess.GetTargetStagePos() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '            GetTargetStagePos = 0
    '        End Try

    '    End Function
    '#End Region

#Region "�J�n�ʒu�Z�o"
    '''=========================================================================
    '''<summary>�u���b�N/�v���[�g�̊J�n�ʒu�Z�o</summary>
    '''<param name="totalCount"> �iIN)�u���b�Nor�v���[�g�̃g�[�^����</param>
    '''<param name="size">       �iIN)�u���b�Nor�v���[�g�̃T�C�Y</param>
    ''' <param name="interval_1">�iIN)�u���b�Nor�v���[�g�Ԋu</param> 
    ''' <param name="interval_2">�iIN)�X�e�[�W�O���[�v�Ԋu�i�v���[�g�Z�o�ł͖��g�p�B�u0�v�w��j</param>
    ''' <param name="cntInItv2"> �iIN)�X�e�[�W�O���[�v���̃u���b�N���i�v���[�g�Z�o�ł͖��g�p�B�u0�v�w��j</param>
    ''' <param name="interval_3">�iIN)�`�b�v�X�e�b�v�Ԋu�i�v���[�g�Z�o�ł͖��g�p�B�u0�v�w��j</param>
    ''' <param name="cntInItv3"> �iIN)�`�b�v�X�e�b�v���̃O���[�v�����̃X�e�b�v���i�v���[�g�Z�o�ł͖��g�p�B�u0�v�w��j</param>
    ''' <param name="startPos">  �iOUT)���ʕۑ���z��B�Z�o���ʂ�ۑ�����Double�^�̔z��</param>
    '''<remarks>�v���[�g�����̃u���b�N�J�n�ʒu���Z�o����B</remarks>
    '''=========================================================================
    Public Sub CalcStartPos(ByVal totalCount As Integer, _
                            ByVal size As Double, ByVal interval_1 As Double, _
                            ByVal interval_2 As Double, ByVal cntInItv2 As Integer, _
                            ByVal interval_3 As Double, ByVal cntInItv3 As Integer, _
                            ByRef startPos() As Double)
        Dim count As Short
        Dim strMSG As String
        Dim intervalCount As Integer
        Dim chipStpCnt As Integer
        Dim workStagePos As Double

        Try
            ' �g�[�^���̃u���b�N�������[�v���񂷁B
            For count = 0 To totalCount - 1
                ' �C���^�[�o���̐����擾
                If (cntInItv2 <> 0) Then
                    intervalCount = count \ cntInItv2
                Else
                    intervalCount = 0
                End If

                '�`�b�v�X�e�b�v���s���̌��Z
                If (cntInItv3 <> 0) Then
                    For chipStpCnt = 0 To cntInItv3
                        '�X�e�[�W�̊J�n�ʒu�Z�o
                        '  'V4.3.0.0�D
                        If (giStageYOrg = STGY_ORG_UP) Then
                            workStagePos = (size * count) + (interval_1 * count) + (interval_2 * intervalCount) - (interval_3 * chipStpCnt)
                        Else
                            workStagePos = (size * count) + (interval_1 * count) + (interval_2 * intervalCount) + (interval_3 * chipStpCnt)
                        End If
                        '  'V4.3.0.0�D

                        startPos((count * (cntInItv3 + 1)) + chipStpCnt) = workStagePos
                    Next
                Else
                    '�X�e�[�W�̊J�n�ʒu�Z�o
                    workStagePos = (size * count) + (interval_1 * count) + (interval_2 * intervalCount)
                    startPos(count) = workStagePos
                End If

            Next

        Catch ex As Exception
            strMSG = "DataAccess.CalcBlockStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "�v���[�g��X�����AY�����̊J�n�ʒu�Z�o"
    '''=========================================================================
    '''<summary>�v���[�g��X�����AY�����̊J�n�ʒu�Z�o</summary>
    '''<returns>�Ȃ�</returns>
    '''<remarks>�v���[�g����X�����̊J�n�ʒu�AY�����̊J�n�ʒu���Z�o����B</remarks>
    '''=========================================================================
    Public Function CalcPlateXYStartPos() As Integer
        Dim strMSG As String

        CalcPlateXYStartPos = 0

        Try
            ' X�����̃u���b�N�J�n�ʒu���Z�o
            With typPlateInfo
                ' X����
                Call CalcStartPos(.intPlateCntXDir, .dblPlateSizeX, .dblPlateItvXDir, _
                                0, 0, 0, 0, gPltStagePosX)

                ' Y����
                Call CalcStartPos(.intPlateCntYDir, .dblPlateSizeY, .dblPlateItvYDir, _
                                0, 0, 0, 0, gPltStagePosY)
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.CalcPlateXYStartPos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcPlateXYStartPos = 0
        End Try

    End Function
#End Region

#Region "�v���[�g���u���b�N��X�����AY�����̊J�n�ʒu�Z�o"
    '''=========================================================================
    '''<summary>�u���b�N��X�����AY�����̊J�n�ʒu�Z�o</summary>
    '''<returns>�Ȃ�</returns>
    '''<remarks>�v���[�g������X�����̃u���b�N�J�n�ʒu�AY�����̃u���b�N�J�n�ʒu���Z�o����B</remarks>
    '''=========================================================================
    Public Function CalcBlockXYStartPos() As Integer

        Dim strMSG As String
        Dim chipStpItvX As Double
        Dim chipStpCntX As Integer
        Dim chipStpItvY As Double
        Dim chipStpCntY As Integer
        Dim cnt As Integer
        Dim dblIntval As Double                                         ' ###119

        CalcBlockXYStartPos = 0

        Try
            '�X�e�[�W���W�f�[�^�ۑ��̈�̏�����
            For cnt = 0 To MAXCNT_PLT_BLK
                gBlkStagePosX(cnt) = 0
                gBlkStagePosY(cnt) = 0
            Next

            ' X�����̃u���b�N�J�n�ʒu���Z�o
            With typPlateInfo
                '�`�b�v�X�e�b�v���̐ݒ�j
                If (.intDirStepRepeat = STEP_RPT_CHIPXSTPY) Then        ' �ï�߁���߰� = �`�b�v��+X����
                    'chipStpItvX = .dblChipStepItv                       ' �`�b�v�X�e�b�v��X = �O���[�v���̃`�b�v�X�e�b�v��
                    'chipStpCntX = .intChipStepCnt                       ' �X�e�b�v�ԊuX     = �O���[�v���`�b�v�X�e�b�v���̃X�e�b�v�Ԋu
                    chipStpItvX = .dblChipSizeXDir / .intBlockCntXDir   ' �`�b�v�X�e�b�v��X = �O���[�v���̃`�b�v�X�e�b�v��              '###115�@�璹�Ή�
                    chipStpCntX = .intBlockCntXDir - 1                  ' �X�e�b�v�ԊuX     = �O���[�v���`�b�v�X�e�b�v���̃X�e�b�v�Ԋu  '###115�@�璹�Ή�
                    chipStpItvY = 0
                    chipStpCntY = 0
                ElseIf (.intDirStepRepeat = STEP_RPT_CHIPYSTPX) Then    ' �ï�߁���߰� = �`�b�v��+Y����
                    chipStpItvX = 0
                    chipStpCntX = 0
                    'chipStpItvY = .dblChipStepItv                       ' �`�b�v�X�e�b�v��Y = �O���[�v���̃`�b�v�X�e�b�v��
                    'chipStpCntY = .intChipStepCnt                       ' �X�e�b�v�ԊuY     = �O���[�v���`�b�v�X�e�b�v���̃X�e�b�v�Ԋu
                    chipStpItvY = .dblChipSizeYDir / .intBlockCntYDir   ' �`�b�v�X�e�b�v��Y = �O���[�v���̃`�b�v�X�e�b�v��              '###115�@�璹�Ή�
                    chipStpCntY = .intBlockCntYDir - 1                  ' �X�e�b�v�ԊuY     = �O���[�v���`�b�v�X�e�b�v���̃X�e�b�v�Ԋu  '###115�@�璹�Ή�

                Else
                    chipStpItvX = 0
                    chipStpCntX = 0
                    chipStpItvY = 0
                    chipStpCntY = 0
                End If

                ' X����(gBlkStagePosX�z��Ƀu���b�NX�J�n�ʒu��ݒ�)
                Call CalcStartPos(.intBlockCntXDir, .dblBlockSizeXDir, .dblBlockItvXDir, _
                                .dblStgGrpItvX, .intBlkCntInStgGrpX, _
                                chipStpItvX, chipStpCntX, gBlkStagePosX)

                ' Y����(gBlkStagePosY�z��Ƀu���b�NY�J�n�ʒu��ݒ�)
                Call CalcStartPos(.intBlockCntYDir, .dblBlockSizeYDir, .dblBlockItvYDir, _
                                .dblStgGrpItvY, .intBlkCntInStgGrpY, _
                                chipStpItvY, chipStpCntY, gBlkStagePosY)

                '-----  ###119�� -----
                ' TY2�̃X�e�b�v����(�u���b�N�C���^�[�o��)�����Z����
                dblIntval = 0.0
                For cnt = 1 To (MaxTy2 - 1)
                    dblIntval = dblIntval + typTy2InfoArray(cnt).dblTy22
                    If (.intResistDir = 0) Then                         ' �`�b�v���ѕ��� = X�����̏ꍇ
                        gBlkStagePosY(cnt) = gBlkStagePosY(cnt) + dblIntval
                    Else                                                ' �`�b�v���ѕ��� = Y�����̏ꍇ
                        gBlkStagePosX(cnt) = gBlkStagePosX(cnt) + dblIntval
                    End If
                Next
                '-----  ###119�� -----

            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.CalcStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcBlockXYStartPos = 0
        End Try

    End Function
#End Region

#Region "�v���[�g�����̊e�u���b�N�̃X�e�[�W�ʒu���Z�o-(���g�p�j"
    '''=========================================================================
    '''<summary>�e�u���b�N�X�e�[�W�ʒu�Z�o</summary>
    '''<returns>�Ȃ�</returns>
    '''<remarks>�u���b�N���̃X�e�[�W�X�^�[�g�ʒu���Z�o���f�[�^��ۑ�����B</remarks>
    '''�v���[�g�����̕ۑ��̈�͈ȉ��̏��Ԃŕۑ����s���B
    '''  
    '''�@�v���[�g�����̕���
    '''   ____ ____
    '''�@| �E | �@ |
    '''  |____|____|
    '''  | �D | �A |    
    '''  |____|____|
    '''  | �C | �B |
    '''  |____|____|
    ''' 
    '''=========================================================================
    Public Function CalcAllBlockStagePos() As Integer

        '#4.12.2.0�F        Dim count As Short
        Dim count As Integer            '#4.12.2.0�F
        Dim strMSG As String

        ' �u���b�N�ʒu�̎Z�o�W��
        Dim intXBlockCnt As Integer
        Dim intYBlockCnt As Integer
        ' �X�e�[�W�O���[�v�Ԋu�̎Z�o�W��
        Dim intYStgGrpCnt As Integer
        Dim intXStgGrpCnt As Integer

        ' �v���[�g�Ԋu�̎Z�o�W��
        Dim intTotalBlkCntInPlate As Integer
        'Dim intPlateXCnt As Integer
        'Dim intPlateYCnt As Integer

        ' �u���b�N�ʒu�̈ꎞ�ۑ��̈�
        Dim dblWorkBlockPosX As Double
        Dim dblWorkBlockPosY As Double

        CalcAllBlockStagePos = 0

        Try
            ' �v���[�g���̃u���b�N���̎Z�o
            '#4.12.2.0�F           intTotalBlkCntInPlate = typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir
            intTotalBlkCntInPlate = CInt(typPlateInfo.intBlockCntXDir) * CInt(typPlateInfo.intBlockCntYDir) '#4.12.2.0�F

            For count = 1 To intTotalBlkCntInPlate
                '�u���b�N�ʒu�̎Z�o
                With typPlateInfo
                    '�c�����A�������u���b�N��
                    intXBlockCnt = count \ .intBlockCntYDir
                    If (intXBlockCnt Mod 2) = 0 Then
                        intYBlockCnt = count Mod .intBlockCntYDir
                    Else
                        intYBlockCnt = (.intBlockCntYDir + 1) - (count Mod .intBlockCntYDir)
                    End If

                    '�X�e�[�W�O���[�v�Ԋu�̐�
                    intXStgGrpCnt = intXBlockCnt \ .intBlkCntInStgGrpX
                    intYStgGrpCnt = intYBlockCnt \ .intBlkCntInStgGrpY

                    '�v���[�g�����̃|�W�V�����v�Z- �u���b�N�T�C�Y X �u���b�N�� + �X�e�[�W�C���^�[�o��
                    dblWorkBlockPosX = (.dblBlockSizeXDir * intXBlockCnt) + (.dblBlockItvXDir * intXBlockCnt) + (.dblStgGrpItvX * intYStgGrpCnt)
                    dblWorkBlockPosY = (.dblBlockSizeYDir * intYBlockCnt) + (.dblBlockItvYDir * intYBlockCnt) + (.dblStgGrpItvY * intYStgGrpCnt)
                End With

                '�Ώۃu���b�N�̈ʒu���W����
                With typGrpInfoArray(count)
                    .dblStgPosX = dblWorkBlockPosX
                    .dblStgPosX = dblWorkBlockPosY
                End With
            Next

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.CalcStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcAllBlockStagePos = 0
        End Try

    End Function
#End Region

#Region "X������BlockNo��Y������BlockNo����X�e�[�W�ʒu��Ԃ�"
    '''=========================================================================
    ''' <summary>X������BlockNo��Y������BlockNo����X�e�[�W�ʒu��Ԃ��B</summary>
    ''' <param name="xBlockNo">(INP)X�����̃u���b�N�ԍ�</param>
    ''' <param name="yBlockNo">(INP)Y�����̃u���b�N�ԍ�</param>
    ''' <param name="stgx">    (OUT)�X�e�[�W�ʒuX</param>
    ''' <param name="stgy">    (OUT)�X�e�[�W�ʒuY</param>
    ''' <returns>0=����</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetTargetStagePosByXY(ByVal xBlockNo As Integer, ByVal yBlockNo As Integer, _
                                          ByRef stgx As Double, ByRef stgy As Double) As Integer

        Dim r As Integer

        Try
            r = cFRS_NORMAL

            ' �w��u���b�N�ԍ��̃`�F�b�N
            If ((typPlateInfo.intBlockCntXDir < (xBlockNo - 1)) Or (xBlockNo < 1)) _
                Or ((typPlateInfo.intBlockCntYDir < (yBlockNo - 1)) Or (yBlockNo < 1)) Then

            End If

            ' �w��ʒu�̃X�e�[�W�ʒu�擾
            stgx = gBlkStagePosX(xBlockNo - 1)
            stgy = gBlkStagePosY(yBlockNo - 1)

            ' X�J�n�ʒu�{�I�t�Z�b�g�{�␳�l
            stgx = stgx + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX
            ' Y�J�n�ʒu�{�I�t�Z�b�g�{�␳�l
            stgy = stgy + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.GetTargetStagePosByXY() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "�w��u���b�N�ԍ��̃X�e�[�W�ʒu���擾"
    '''=========================================================================
    ''' <summary>�w�肳�ꂽ�u���b�N�ԍ��̈ʒu�����擾����B</summary>
    ''' <param name="curPltNo">   (INP)���݂̃v���[�g�ԍ���ݒ�</param>
    ''' <param name="curBlkNo">   (INP)���݂̃u���b�N�ԍ���ݒ�</param>
    ''' <param name="stgx">       (OUT)�X�e�[�W�ʒuX</param>
    ''' <param name="stgy">       (OUT)�X�e�[�W�ʒuY</param>
    ''' <param name="dispPltPosX">(OUT)�v���[�g�ԍ�X</param>
    ''' <param name="dispPltPosY">(OUT)�v���[�g�ԍ�Y</param>
    ''' <param name="dispBlkPosX">(OUT)�u���b�N�ԍ�X</param>
    ''' <param name="dispBlkPosY">(OUT)�u���b�N�ԍ�Y</param>
    ''' <returns>�g���~���O�̍ŏI�u���b�N�̏ꍇBLOCK_END��1�A
    '''          �ŏI�v���[�g�ŏI�u���b�N�̏ꍇPLATE_BLOCK_END=2��Ԃ��B</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetTargetStagePos(ByVal curPltNo As Integer, ByVal curBlkNo As Integer, _
                                        ByRef stgx As Double, ByRef stgy As Double, _
                                        ByRef dispPltPosX As Integer, ByRef dispPltPosY As Integer, _
                                        ByRef dispBlkPosX As Integer, ByRef dispBlkPosY As Integer) As Integer
        Dim strMsg As String
        Dim workStgPosX As Double
        Dim workStgPosY As Double
        Dim totalPlateCnt As Integer
        Dim totalBlockCnt As Integer

        GetTargetStagePos = cFRS_NORMAL
        Try
            ' ��ʒu�A�X�e�b�v�����ɂ�莟�̃u���b�N�ʒu���擾����B
            With typPlateInfo
                ' �ŏI�|�W�V�����̔���
                '----- V1.23.0.0�H�� -----
                totalPlateCnt = .intPlateCntXDir
                totalPlateCnt = totalPlateCnt * .intPlateCntYDir
                totalBlockCnt = .intBlockCntXDir
                totalBlockCnt = totalBlockCnt * .intBlockCntYDir

                'totalPlateCnt = .intPlateCntXDir * .intPlateCntYDir
                'totalBlockCnt = .intBlockCntXDir * .intBlockCntYDir             '###115 �璹�Ή�
                '----- V1.23.0.0�H�� -----

                ' �p�����[�^�`�F�b�N
                If ((curPltNo < 0) Or (curBlkNo < 0)) Then
                    GetTargetStagePos = -1 * ERR_CMD_PRM
                    Exit Function
                End If

                If (curPltNo > totalPlateCnt) Then
                    GetTargetStagePos = PLATE_BLOCK_END
                    Exit Function
                ElseIf (curBlkNo > totalBlockCnt) Then
                    GetTargetStagePos = BLOCK_END
                    Exit Function
                End If

                ' �f�[�^�擾�Ώۂ�"0"�I���W���̂��߁A�����ň���Z����B
                curPltNo = curPltNo - 1
                curBlkNo = curBlkNo - 1

                ' �X�e�b�v&(���s�[�g����)
                If (.intDirStepRepeat = STEP_RPT_Y) _
                    Or (.intDirStepRepeat = STEP_RPT_CHIPXSTPY) Then
                    ' Y����
                    Call GetBlockPos_StpY(curPltNo, curBlkNo, gSysPrm.stDEV.giBpDirXy, _
                                    .intPlateCntXDir, .intPlateCntYDir, _
                                    .intBlockCntXDir, .intBlockCntYDir, workStgPosX, workStgPosY, _
                                    dispPltPosX, dispPltPosY, dispBlkPosX, dispBlkPosY)
                ElseIf (.intDirStepRepeat = STEP_RPT_X) _
                       Or (.intDirStepRepeat = STEP_RPT_CHIPYSTPX) Then
                    ' X����
                    Call GetBlockPos_StpX(curPltNo, curBlkNo, gSysPrm.stDEV.giBpDirXy, _
                                    .intPlateCntXDir, .intPlateCntYDir, _
                                    .intBlockCntXDir, .intBlockCntYDir, workStgPosX, workStgPosY, _
                                    dispPltPosX, dispPltPosY, dispBlkPosX, dispBlkPosY)
                Else
                    ' �X�e�b�v&���s�[�g�Ȃ�
                    '----- ###169�� -----
                    ' �X�e�b�v&���s�[�g�Ȃ��ł��\���p�u���b�N�����X�V���邽�߉��L��Call����
                    Call GetBlockPos_StpY(curPltNo, curBlkNo, gSysPrm.stDEV.giBpDirXy, _
                                    .intPlateCntXDir, .intPlateCntYDir, _
                                    .intBlockCntXDir, .intBlockCntYDir, workStgPosX, workStgPosY, _
                                    dispPltPosX, dispPltPosY, dispBlkPosX, dispBlkPosY)

                    workStgPosX = 0.0                                   ' �X�e�[�W�ʒuX,Y��0�ɍĐݒ�
                    workStgPosY = 0.0

                    'dispPltPosX = 1
                    'dispPltPosY = 1
                    'dispBlkPosX = 1
                    'dispBlkPosY = 1
                    '----- ###169�� -----
                End If

                stgx = workStgPosX
                stgy = workStgPosY
            End With

        Catch ex As Exception
            strMsg = "basTrimming.GetTargetStagePos() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "�X�e�[�W�I�t�Z�b�g�̕␳�l���擾����"
    '''=========================================================================
    ''' <summary>�X�e�[�W�I�t�Z�b�g�̕␳�l���擾����</summary>
    ''' <param name="pltRowNo">  (INP)���݂̃v���[�g�ԍ�</param>
    ''' <param name="blkRowNo">  (INP)���݂̃u���b�N�ԍ�</param>
    ''' <param name="AddSubPosX">(OUT)�X�e�b�v�I�t�Z�b�g��X</param>
    ''' <param name="AddSubPosY">(OUT)�X�e�b�v�I�t�Z�b�g��Y</param>
    ''' <param name="blkNoX">    (INP)�u���b�N�ԍ�X V1.20.0.0�H</param>
    ''' <param name="blkNoY">    (INP)�u���b�N�ԍ�Y V1.20.0.0�H</param>
    ''' <returns>cFRS_NORMAL = ����</returns>
    '''=========================================================================
    Public Function GetStepOffSetPos(ByVal pltRowNo As Integer, ByVal blkRowNo As Integer, _
                                    ByRef AddSubPosX As Double, ByRef AddSubPosY As Double, ByVal blkNoX As Integer, ByVal blkNoY As Integer) As Integer

        'Dim workPosY As Double 'V1.22.0.0�E
        'Dim a As Double
        'Dim curStagePosY As Double

        Try
            With typPlateInfo
                '----- V2.0.0.0�J(V1.22.0.0�E)�� -----
                AddSubPosX = 0.0
                AddSubPosY = 0.0

                ' �`�b�v����Y�������̃X�e�b�v�I�t�Z�b�gY�̎Z�o
                If (.intResistDir = 1) Then                             ' �`�b�v���т�Y���� ?
                    ' �X�e�b�v�I�t�Z�b�gY�̎w��Ȃ��Ȃ�NOP
                    If (.dblStepOffsetYDir = 0) Then Return (cFRS_NORMAL)

                    ''V4.12.2.1�@��'V6.0.5.0�@
                    If (.intBlockCntXDir <= 1) Then
                        Return (cFRS_NORMAL)
                    End If
                    ''V4.12.2.1�@��'V6.0.5.0�@

                    ' X�����ړ�����Y�����I�t�Z�b�g�l�����߂�
                    AddSubPosY = (.dblStepOffsetYDir / (.intBlockCntXDir - 1)) * (blkNoX - 1)

                    Return (cFRS_NORMAL)

                Else
                    ' �X�e�b�v�I�t�Z�b�gX�̎w��Ȃ��Ȃ�NOP
                    If (.dblStepOffsetXDir = 0) Then Return (cFRS_NORMAL)

                    ''V4.12.2.1�@��'V6.0.5.0�@
                    If (.intBlockCntYDir <= 1) Then
                        Return (cFRS_NORMAL)
                    End If
                    ''V4.12.2.1�@��'V6.0.5.0�@

                    ' Y�����ړ�����X�����I�t�Z�b�g�l�����߂�
                    AddSubPosX = (.dblStepOffsetXDir / (.intBlockCntYDir - 1)) * (blkNoY - 1)

                    Return (cFRS_NORMAL)

                End If
                '----- V2.0.0.0�J(V1.22.0.0�E)�� -----

                'V1.22.0.0�E
                ''Y�����̎Z�o��
                ''workPosY = (gBlkStagePosY(.intBlockCntYDir - 1) + .dblStepOffsetYDir) / gBlkStagePosY(.intBlockCntYDir - 1)
                'If ((gBlkStagePosY(.intBlockCntYDir - 1) + .dblStepOffsetYDir) <> 0) And _
                '   (gBlkStagePosY(.intBlockCntYDir - 1) <> 0) Then
                '    workPosY = (gBlkStagePosY(.intBlockCntYDir - 1) + .dblStepOffsetYDir) / gBlkStagePosY(.intBlockCntYDir - 1)
                'Else
                '    workPosY = 0.0
                'End If
                'curStagePosY = gPltStagePosY(pltRowNo) + gBlkStagePosY(blkRowNo)
                'workPosY = workPosY * curStagePosY
                'AddSubPosY = workPosY - curStagePosY

                ''X�����̎Z�o��
                'If (.dblStepOffsetXDir <> 0) Then
                '    'a = (.dblStepOffsetYDir + gBlkStagePosY(.intBlockCntYDir - 1)) / .dblStepOffsetXDir
                '    'AddSubPosX = workPosY / a
                '    If (.dblStepOffsetYDir + gBlkStagePosY(.intBlockCntYDir - 1) <> 0) And _
                '       (.dblStepOffsetXDir <> 0) Then
                '        a = (.dblStepOffsetYDir + gBlkStagePosY(.intBlockCntYDir - 1)) / .dblStepOffsetXDir
                '        AddSubPosX = workPosY / a
                '    Else
                '        AddSubPosX = 0
                '    End If
                'Else
                '    AddSubPosX = 0
                'End If
            End With

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "DataAccess.GetBlockPos_StpY() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "�X�e�b�vY�����̃v���[�g���̃u���b�N�ʒu���擾����"
    '''=========================================================================
    ''' <summary>�X�e�b�vY�����̎w��v���[�g�ԍ��A�u���b�N�ԍ��̃X�e�[�W�ʒu���W���擾����B</summary>
    ''' <param name="curPlateNo">  (INP)���݂̃v���[�g�ԍ�(0�I���W��)</param>
    ''' <param name="curBlkNo">    (INP)���݂̃u���b�N�ԍ�(0�I���W��)</param>
    ''' <param name="bpDir">       (INP)BP��R�[�i�[</param>
    ''' <param name="pltCntTarCol">(INP)�v���[�g��X</param>
    ''' <param name="pltCntTarRow">(INP)�v���[�g��Y</param>
    ''' <param name="blkCntTarCol">(INP)�u���b�N��X</param>
    ''' <param name="blkCntTarRow">(INP)�u���b�N��Y</param>
    ''' <param name="stgx">        (OUT)�X�e�[�W�ʒuX</param>
    ''' <param name="stgy">        (OUT)�X�e�[�W�ʒuY</param>
    ''' <param name="pltCurPosX">  (OUT)�v���[�g�ԍ�X</param>
    ''' <param name="pltCurPosY">  (OUT)�v���[�g�ԍ�Y</param>
    ''' <param name="blkCurPosX">  (OUT)�u���b�N�ԍ�X</param>
    ''' <param name="blkCurPosY">  (OUT)�u���b�N�ԍ�Y</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function GetBlockPos_StpY(ByVal curPlateNo As Integer, ByVal curBlkNo As Integer, _
            ByVal bpDir As Integer, ByVal pltCntTarCol As Integer, ByVal pltCntTarRow As Integer, _
            ByVal blkCntTarCol As Integer, ByVal blkCntTarRow As Integer, _
            ByRef stgx As Double, ByRef stgy As Double, _
            ByRef pltCurPosX As Integer, ByRef pltCurPosY As Integer, _
            ByRef blkCurPosX As Integer, ByRef blkCurPosY As Integer) As Boolean

        Dim strMsg As String
        Dim curBlkCol As Integer
        Dim curBlkRow As Integer
        Dim workBlkCol As Integer
        Dim curPltCol As Integer
        Dim curPltRow As Integer
        Dim workPltCol As Integer
        Dim workBlockPosY As Double
        Dim addSubPosX As Double
        Dim addSubPosY As Double
        ' V4.12.0.0�@�@��'V6.1.2.0�A
        Dim DispBlkX As Integer
        Dim DispBlkY As Integer
        ' V4.12.0.0�@�@��'V6.1.2.0�A

        Try
            With typPlateInfo
                ' �ΏۂƂȂ�u���b�N�̗�ƍs���擾
                ' ��ԍ��̎擾
                workPltCol = curPlateNo \ pltCntTarRow          ' �/��������̂��߂ɕۑ�
                workBlkCol = curBlkNo \ blkCntTarRow            ' �/��������̂��߂ɕۑ�(�u���b�N�ԍ�/�u���b�N��Y)

                '----- ###044(�C��  ��������) -----
                curPltCol = workPltCol
                curBlkCol = workBlkCol

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_RIGHTDOWN)
                '        ' �E�
                '        curPltCol = workPltCol
                '        curBlkCol = workBlkCol
                '    Case (BP_DIR_LEFTUP), (BP_DIR_LEFTDOWN)
                '        ' ���
                '        curPltCol = (pltCntTarCol - 1) - workPltCol
                '        curBlkCol = (blkCntTarCol - 1) - workBlkCol
                'End Select
                '----- ###044(�C��  �����܂�) -----

                ' �s�ԍ��̎擾
                curPltRow = curPlateNo Mod pltCntTarRow
                curBlkRow = curBlkNo Mod blkCntTarRow

                '----- ###044(�C��  ��������) -----
                ' �v���[�g�ʒu
                If ((workPltCol Mod 2) = 0) Then        ' �/��������
                    curPltRow = curPltRow
                Else
                    curPltRow = (pltCntTarRow - 1) - curPltRow
                End If

                ' �u���b�N�ʒu
                ' V4.12.0.0�@�@��'V6.1.2.0�A
                ' Y�X�e�b�v���I������Ă���ꍇ�ɁA��A�����ɂ���ăX�e�b�v�𔽓]����
                If JudgeStepYInverse() Then
                    ' Y�������t�]����B������X�e�b�v����
                    If ((workBlkCol Mod 2) = 0) Then        ' �/��������
                        DispBlkX = curBlkRow
                        curBlkRow = (blkCntTarRow - 1) - curBlkRow
                    Else
                        DispBlkX = (blkCntTarRow - 1) - curBlkRow
                        curBlkRow = curBlkRow
                    End If
                Else
                    ' �]���ʂ�̏ォ��X�e�b�v
                    If ((workBlkCol Mod 2) = 0) Then        ' �/��������
                        curBlkRow = curBlkRow
                    Else
                        curBlkRow = (blkCntTarRow - 1) - curBlkRow
                    End If
                End If
                ' V4.12.0.0�@��'V6.1.2.0�A

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_LEFTUP)
                '        ' ��
                '        ' �v���[�g�ʒu
                '        If ((workPltCol Mod 2) = 0) Then        ' �/��������
                '            curPltRow = curPltRow
                '        Else
                '            curPltRow = (pltCntTarRow - 1) - curPltRow
                '        End If

                '        ' �u���b�N�ʒu
                '        If ((workBlkCol Mod 2) = 0) Then        ' �/��������
                '            curBlkRow = curBlkRow
                '        Else
                '            curBlkRow = (blkCntTarRow - 1) - curBlkRow
                '        End If

                '    Case (BP_DIR_RIGHTDOWN), (BP_DIR_LEFTDOWN)
                '        ' ���
                '        ' �v���[�g�ʒu
                '        If ((workPltCol Mod 2) = 0) Then        ' �/��������
                '            curPltRow = (pltCntTarRow - 1) - curPltRow
                '        Else
                '            curPltRow = curPltRow
                '        End If
                '        ' �u���b�N�ʒu
                '        If ((workBlkCol Mod 2) = 0) Then        ' �/��������
                '            curBlkRow = (blkCntTarRow - 1) - curBlkRow
                '        Else
                '            curBlkRow = curBlkRow
                '        End If
                'End Select
                '----- ###044(�C��  �����܂�) -----

                'OPT:TY2
                If (Form1.CmdTy2.Enabled = True) Then
                    ' TY2�I�v�V����������ꍇ�AY�����̃u���b�N�ʒu���ɒ����l�������Z����B
                    workBlockPosY = gBlkStagePosY(curBlkRow) + typTy2InfoArray(curBlkRow).dblTy22
                Else
                    ' �ʏ�̏ꍇ�͂��̂܂�   
                    workBlockPosY = gBlkStagePosY(curBlkRow)
                End If

                ' �X�e�[�W���W�̎擾
                stgx = gPltStagePosX(curPltCol) + gBlkStagePosX(curBlkCol)
                '                stgy = gPltStagePosY(curPltRow) + gBlkStagePosY(curBlkRow)
                stgy = gPltStagePosY(curPltRow) + workBlockPosY

                ' �X�e�[�W�I�t�Z�b�g�̕␳�l���擾
                'GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY)                                 'V1.20.0.0�H
                GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY, curBlkCol + 1, curBlkRow + 1)    'V1.20.0.0�H
                stgx = stgx + addSubPosX
                stgy = stgy + addSubPosY

                ' �s�A��̈ʒu����ݒ�
                pltCurPosX = curPltCol + 1
                pltCurPosY = curPltRow + 1
                ' V4.12.0.0�@�@���@'V6.1.2.0�A
                ' Y�X�e�b�v���I������Ă���ꍇ�ɁA��A�����ɂ���ăX�e�b�v�𔽓]����
                If JudgeStepYInverse() Then
                    ' Y�������t�]����B������X�e�b�v����
                    blkCurPosY = DispBlkX + 1
                Else
                    blkCurPosY = curBlkRow + 1
                End If
                ' V4.12.0.0�@�@���@'V6.1.2.0�A
                blkCurPosX = curBlkCol + 1

            End With

        Catch ex As Exception
            strMsg = "DataAccess.GetBlockPos_StpY() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Function
#End Region

#Region "�X�e�b�vX�����̃v���[�g���̃u���b�N�ʒu���擾����"
    '''=========================================================================
    '''<summary>�X�e�b�vX�����̎w��v���[�g�ԍ��A�u���b�N�ԍ��̃X�e�[�W�ʒu���W���擾����B</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function GetBlockPos_StpX(ByVal curPlateNo As Integer, ByVal curBlkNo As Integer, _
            ByVal bpDir As Integer, ByVal pltCntTarCol As Integer, ByVal pltCntTarRow As Integer, _
            ByVal blkCntTarCol As Integer, ByVal blkCntTarRow As Integer, _
            ByRef stgx As Double, ByRef stgy As Double, _
            ByRef pltCurPosX As Integer, ByRef pltCurPosY As Integer, _
            ByRef blkCurPosX As Integer, ByRef blkCurPosY As Integer) As Boolean

        Dim strMsg As String
        Dim curBlkCol As Integer
        Dim curBlkRow As Integer
        Dim workBlkRow As Integer
        Dim curPltCol As Integer
        Dim workPltRow As Integer
        Dim curPltRow As Integer
        Dim workBlockPosY As Double
        Dim addSubPosX As Double
        Dim addSubPosY As Double

        Try
            With typPlateInfo
                ' �ΏۂƂȂ�u���b�N�̗�ƍs���擾
                ' �s�ԍ��̎擾
                workPltRow = curPlateNo \ pltCntTarCol          ' �/��������̂��߂ɕۑ�
                workBlkRow = curBlkNo \ blkCntTarCol            ' �/��������̂��߂ɕۑ�

                '----- ###044(�C��  ��������) -----
                curPltRow = workPltRow
                curBlkRow = workBlkRow

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_LEFTUP)
                '        ' ��
                '        curPltRow = workPltRow
                '        curBlkRow = workBlkRow
                '    Case (BP_DIR_RIGHTDOWN), (BP_DIR_LEFTDOWN)
                '        ' ���
                '        curPltRow = (pltCntTarRow - 1) - workPltRow
                '        curBlkRow = (blkCntTarRow - 1) - workBlkRow
                'End Select
                '----- ###044(�C��  �����܂�) -----

                ' ��ԍ��̎擾
                curPltCol = curPlateNo Mod pltCntTarCol
                curBlkCol = curBlkNo Mod blkCntTarCol

                '----- ###044(�C��  ��������) -----
                ' �v���[�g�ʒu
                If ((workPltRow Mod 2) = 0) Then        ' �/��������
                    curPltCol = curPltCol
                Else
                    curPltCol = (pltCntTarCol - 1) - curPltCol
                End If
                '�u���b�N�ʒu
                If ((workBlkRow Mod 2) = 0) Then        ' �/��������
                    curBlkCol = curBlkCol
                Else
                    curBlkCol = (blkCntTarCol - 1) - curBlkCol
                End If

                'Select Case (bpDir)
                '    Case (BP_DIR_RIGHTUP), (BP_DIR_RIGHTDOWN)
                '        ' �E�
                '        ' �v���[�g�ʒu
                '        If ((workPltRow Mod 2) = 0) Then        ' �/��������
                '            curPltCol = curPltCol
                '        Else
                '            curPltCol = (pltCntTarCol - 1) - curPltCol
                '        End If
                '        '�u���b�N�ʒu
                '        If ((workBlkRow Mod 2) = 0) Then        ' �/��������
                '            curBlkCol = curBlkCol
                '        Else
                '            curBlkCol = (blkCntTarCol - 1) - curBlkCol
                '        End If

                '    Case (BP_DIR_LEFTUP), (BP_DIR_LEFTDOWN)
                '        ' ���
                '        ' �v���[�g�ʒu
                '        If ((workPltRow Mod 2) = 0) Then        ' �/��������
                '            curPltCol = (pltCntTarCol - 1) - curPltCol
                '        Else
                '            curPltCol = curPltCol
                '        End If
                '        ' �u���b�N�ʒu
                '        If ((workBlkRow Mod 2) = 0) Then        ' �/��������
                '            curBlkCol = (blkCntTarCol - 1) - curBlkCol
                '        Else
                '            curBlkCol = curBlkCol
                '        End If
                'End Select
                '----- ###044(�C��  �����܂�) -----

                'OPT:TY2
                If (Form1.CmdTy2.Enabled = True) Then
                    ' TY2�I�v�V����������ꍇ�AY�����̃u���b�N�ʒu���ɒ����l�������Z����B
                    workBlockPosY = gBlkStagePosY(curBlkRow) + typTy2InfoArray(curBlkRow).dblTy22
                Else
                    ' �ʏ�̏ꍇ�͂��̂܂�   
                    workBlockPosY = gBlkStagePosY(curBlkRow)
                End If

                ' �X�e�[�W���W�̎擾
                stgx = gPltStagePosX(curPltCol) + gBlkStagePosX(curBlkCol)
                stgy = gPltStagePosY(curPltRow) + workBlockPosY
                'stgy = gPltStagePosY(curPltRow) + gBlkStagePosY(curBlkRow)

                ' �X�e�[�W�I�t�Z�b�g�̕␳�l���擾
                'GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY)                                 'V1.20.0.0�H
                GetStepOffSetPos(curPltRow, curBlkRow, addSubPosX, addSubPosY, curBlkCol + 1, curBlkRow + 1)    'V1.20.0.0�H
                stgx = stgx + addSubPosX
                stgy = stgy + addSubPosY

                ' �s�A��̈ʒu����ݒ�
                pltCurPosX = curPltCol + 1
                pltCurPosY = curPltRow + 1
                blkCurPosX = curBlkCol + 1
                blkCurPosY = curBlkRow + 1
            End With

        Catch ex As Exception
            strMsg = "DataAccess.GetBlockPos_StpX() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Function
#End Region

#Region "��ٰ�ߊԊu�Z�o(��ٰ�߲������)"
    '''=========================================================================
    '''<summary>��ٰ�ߊԊu�Z�o(��ٰ�߲������)</summary>
    '''<param name="intBNum">(INP) ��R�ԍ�</param>
    '''<returns>��ٰ�ߊԊu�݂̂̐ώZ�l</returns>
    '''<remarks>NET�Ŏg�p</remarks>
    '''=========================================================================
    Public Function CalcGrpInterval(ByRef intBNum As Short) As Double

        Dim intCDir As Short
        Dim intGNx As Short
        Dim intGNY As Short
        Dim intStpMax As Short
        Dim intBMax As Short
        Dim i As Short
        Dim dblBMov As Double
        Dim strMSG As String

        Try

            ' �f�[�^�ݒ�
            intGNx = typPlateInfo.intGroupCntInBlockXBp ' �O���[�v��X,Y
            intGNY = typPlateInfo.intGroupCntInBlockYStage
            intCDir = typPlateInfo.intResistDir         ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
            If intCDir = 0 Then                         ' X����
                intStpMax = intGNx
            Else
                intStpMax = intGNY
            End If

            dblBMov = 0
            intBMax = 0
            For i = 1 To intStpMax - 1
                intBMax = intBMax + typCirInInfoArray(i).intCiP2 ' ����Đ�
                If intBNum <= intBMax Then
                    Exit For
                End If
                dblBMov = dblBMov + typCirInInfoArray(i).dblCiP3 ' ��ٰ�ߊԊu
            Next i
            CalcGrpInterval = dblBMov

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.CalcGrpInterval() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcGrpInterval = 0.0
        End Try

    End Function
#End Region

#Region "�ï�ߊԊu�Z�o(�ï�߲������)�y�ï���ް��z"
    '''=========================================================================
    '''<summary>�ï�ߊԊu�Z�o(�ï�߲������)</summary>
    '''<param name="intBNum">(INP) ��ۯ�No.</param>
    '''<returns>�ï�ߊԊu�݂̂̐ώZ�l</returns>
    '''=========================================================================
    Public Function CalcStepInterval(ByRef intBNum As Short) As Double

        Dim intCDir As Short
        Dim intGNx As Short
        Dim intGNY As Short
        Dim intStpMax As Short
        Dim intBMax As Short
        Dim i As Short
        Dim dblBMov As Double
        Dim strMSG As String

        Try

            ' �O���[�v��(�ï�ߐ�)X,Y
            intGNx = typPlateInfo.intGroupCntInBlockXBp
            intGNY = typPlateInfo.intGroupCntInBlockYStage
            intCDir = typPlateInfo.intResistDir               ' �`�b�v���ѕ����擾(CHIP-NET�̂�)
            If intCDir = 0 Then ' X����
                intStpMax = intGNx
            Else
                intStpMax = intGNY
            End If

            dblBMov = 0
            intBMax = 0
            For i = 1 To intStpMax - 1
                intBMax = intBMax + typStepInfoArray(i).intSP2 ' ��ۯ���
                If intBNum <= intBMax Then
                    Exit For
                End If
                dblBMov = dblBMov + typStepInfoArray(i).dblSP3 ' ��ٰ�ߊԊu(�ï�ߊԲ������)
            Next i
            CalcStepInterval = dblBMov

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.CalcStepInterval() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CalcStepInterval = 0.0
        End Try

    End Function
#End Region

#Region "�w��ï�ߔԍ�����ۯ����ƽï�߲�����ق�Ԃ��y�ï���ް��z"
    '''=========================================================================
    '''<summary>�w��ï�ߔԍ�����ۯ����ƽï�߲�����ق�Ԃ�</summary>
    '''<param name="intStepNo">  (INP) �ï�ߔԍ�</param>
    '''<param name="intBlockNum">(OUT) ��ۯ���</param>
    '''<param name="dblStepInt"> (OUT) �ï�߲������</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetStepData(ByRef intStepNo As Short, ByRef intBlockNum As Short, ByRef dblStepInt As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim strMSG As String

        Try

            bRetc = False
            For i = 1 To MaxCntStep
                If (intStepNo = typStepInfoArray(i).intSP1) Then    ' �ï�ߔԍ���v
                    intBlockNum = typStepInfoArray(i).intSP2        ' ��ۯ���
                    dblStepInt = typStepInfoArray(i).dblSP3         ' �ï�߲������
                    bRetc = True
                    Exit For
                End If
            Next
            GetStepData = bRetc

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.GetStepData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetStepData = False
        End Try

    End Function
#End Region

#Region "�w��ï�ߔԍ��̻���Đ��Ƹ�ٰ�ߊԲ�����ق�Ԃ��y�ï���ް��z"
    '''=========================================================================
    '''<summary>�w��ï�ߔԍ�����ۯ����ƽï�߲����ނ�Ԃ�</summary>
    '''<param name="intStepNo">(INP) �ï�ߔԍ�</param>
    '''<param name="iCirCnt">  (OUT) ����Đ�</param>
    '''<param name="dGrpInt">  (OUT) ��ٰ�ߊԲ������</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetCirInInfoData(ByRef intStepNo As Short, ByRef iCirCnt As Short, ByRef dGrpInt As Double) As Boolean

        Dim bRetc As Boolean
        Dim iCnt As Short
        Dim strMSG As String

        Try

            bRetc = False
            For iCnt = 1 To MaxCntStep
                If (intStepNo = typCirInInfoArray(iCnt).intCiP1) Then ' �ï�ߔԍ���v
                    iCirCnt = typCirInInfoArray(iCnt).intCiP2 ' ����Đ�
                    dGrpInt = typCirInInfoArray(iCnt).dblCiP3 ' ��ٰ�ߊԲ������
                    bRetc = True
                    Exit For
                End If
            Next
            GetCirInInfoData = bRetc

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.GetCirInInfoData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "�w��ï�ߔԍ��̒�R���Ƹ�ٰ�߲�����ق�Ԃ��y��ٰ���ް��z"
    '''=========================================================================
    '''<summary>�w���ٰ�ߔԍ�����ۯ����Ƹ�ٰ�߲�����ق�Ԃ�</summary>
    '''<param name="intGrpNo"> (INP)��ٰ�ߔԍ�</param>
    '''<param name="intResNum">(OUT)��R��</param>
    '''<param name="dblGrpInt">(OUT)��ٰ�߲������</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetGrpData(ByRef intGrpNo As Short, ByRef intResNum As Short, ByRef dblGrpInt As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short

        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim cs As Double
        Dim intCDir As Short
        Dim strMSG As String

        Try

            ' ���߻���
            dblCSx = typPlateInfo.dblChipSizeXDir
            dblCSy = typPlateInfo.dblChipSizeYDir

            intCDir = typPlateInfo.intResistDir                 ' �`�b�v���ѕ����擾(CHIP-NET�̂�)

            If intCDir = 0 Then                                 ' �`�b�v���ѕ��� = X ? 
                cs = dblCSx                                     ' cs = ���߻���X 
            Else
                cs = dblCSy                                     ' cs = ���߻���Y 
            End If

            bRetc = False
            For i = 1 To MaxCntStep
                If (intGrpNo = typGrpInfoArray(i).intGP1) Then  ' ��ٰ�ߔԍ���v
                    intResNum = typGrpInfoArray(i).intGP2       ' ��ٰ�ߓ���R��
                    dblGrpInt = typGrpInfoArray(i).dblGP3 + cs  ' ��ٰ�߲������ = ��ٰ�߲������ + ���߻���

                    bRetc = True
                    Exit For
                End If
            Next
            GetGrpData = bRetc

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.GetGrpData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetGrpData = False
        End Try

    End Function
#End Region

#Region "�w���R�ԍ��̶�Đ���Ԃ��y��R�ް��z"
    '''=========================================================================
    '''<summary>�w���R�ԍ��̶�Đ���Ԃ�</summary>
    '''<param name="intRegNo"> (INP) ��R�ԍ�</param>
    '''<param name="intCutNum">(OUT) ��Đ�</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetRegCutNum(ByRef intRegNo As Short, ByRef intCutNum As Short) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim strMSG As String

        Try
            bRetc = False
            For i = 1 To MaxCntResist
                If (intRegNo = typResistorInfoArray(i).intResNo) Then   ' ��R�ԍ���v
                    intCutNum = typResistorInfoArray(i).intCutCount     ' ��Đ�
                    bRetc = True
                    Exit For
                End If
            Next
            GetRegCutNum = bRetc

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.GetRegCutNum() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetRegCutNum = False
        End Try

    End Function
#End Region

#Region "�w��z��ԍ��̒�R�ԍ���Ԃ��y��R�ް��z"
    '''=========================================================================
    '''<summary>�w��z��ԍ��̒�R�ԍ���Ԃ�</summary>
    '''<param name="intRegNo"> (INP) �z��ԍ�</param>
    '''<param name="intRegNum">(OUT) ��R�ԍ�</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetRegNum(ByRef intRegNo As Short, ByRef intRegNum As Short) As Boolean

        Dim bRetc As Boolean
        Dim strMSG As String

        Try
            bRetc = False
            If ((1 <= intRegNo) And (MaxCntResist >= intRegNo)) Then
                intRegNum = typResistorInfoArray(intRegNo).intResNo ' ��R�ԍ�
                bRetc = True
            End If
            GetRegNum = bRetc

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "DataAccess.GetRegNum() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GetRegNum = False
        End Try

    End Function
#End Region

#Region "�w���R�ԍ��A��Ĕԍ��̽����߲�Ă�Ԃ�"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ��̽����߲�Ă�Ԃ�</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<param name="dblX"    >(OUT) �����߲��X</param>
    '''<param name="dblY"    >(OUT) �����߲��Y</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetCutStartPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                       ' ��R�ԍ���v
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then     ' ��Ĕԍ���v
                        dblX = typResistorInfoArray(i).ArrCut(j).dblStartPointX         ' �����߲��
                        dblY = typResistorInfoArray(i).ArrCut(j).dblStartPointY
                        bRetc = True
                        GetCutStartPoint = bRetc
                        Exit Function
                    End If
                Next
            End If
        Next
        GetCutStartPoint = bRetc
    End Function
#End Region

#Region "�w���R�ԍ��A��Ĕԍ��̽����߲�Ă�ݒ肷��"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ��̽����߲�Ă�ݒ肷��</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<param name="dblX"    >(INP) �����߲��X</param>
    '''<param name="dblY"    >(INP) �����߲��Y</param>
    '''<returns>TRUE:����, FALSE:���s</returns>
    '''<remarks>�ް����㏑�����邽�߁A�����ް����Ȃ��ꍇ�͎��s����</remarks>
    '''=========================================================================
    Public Function SetCutStartPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                    ' ��R�ԍ���v
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then  ' ��Ĕԍ���v
                        typResistorInfoArray(i).ArrCut(j).dblStartPointX = dblX      ' �����߲��
                        typResistorInfoArray(i).ArrCut(j).dblStartPointY = dblY
                        bRetc = True
                        Exit For
                    End If
                Next
            End If
        Next
        SetCutStartPoint = bRetc
    End Function
#End Region

    'V5.0.0.6�I��
#Region "�e�B�[�`���O�|�C���g���O�ŏ���������"
    '''=========================================================================
    ''' <summary>
    ''' �e�B�[�`���O�|�C���g���O�ŏ���������
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ResetCutTeachPoint()
        Try
            Dim i As Short
            Dim j As Short

            For i = 1 To MaxCntResist
                For j = 1 To MaxCntCut
                    typResistorInfoArray(i).ArrCut(j).dblTeachPointX = 0.0
                    typResistorInfoArray(i).ArrCut(j).dblTeachPointY = 0.0
                Next
            Next
        Catch ex As Exception
            MsgBox("DataAccess.ResetCutTeachPoint() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "�w���R�ԍ��A�J�b�g�ԍ��̃e�B�[�`���O�|�C���g�����Z�����X�^�[�g�|�C���g��Ԃ�"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A�J�b�g�ԍ��̃X�^�[�g�|�C���g��Ԃ�</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) �J�b�g�ԍ�</param>
    '''<param name="dblX"    >(OUT) �X�^�[�g�|�C���gX</param>
    '''<param name="dblY"    >(OUT) �X�^�[�g�|�C���gY</param>
    '''<returns>TRUE:�f�[�^����, FALSE:�f�[�^�Ȃ�</returns>
    '''=========================================================================
    Public Function GetCutStartPointAddTeachPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Try
            dblX = typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblStartPointX + typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblTeachPointX  ' �X�^�[�g�|�C���g
            dblY = typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblStartPointY + typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblTeachPointY
            Return (True)
        Catch ex As Exception
            MsgBox("DataAccess.GetCutStartPointAddTeachPoint() TRAP ERROR = " + ex.Message)
            Return (False)
        End Try

    End Function
#End Region
    'V5.0.0.6�I��

#Region "�w���R�ԍ��A��Ĕԍ���è��ݸ��߲�Ă�Ԃ�"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ���è��ݸ��߲�Ă�Ԃ�</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<param name="dblX"    >(OUT) è��ݸ��߲��X</param>
    '''<param name="dblY"    >(OUT) è��ݸ��߲��Y</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetCutTeachPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                    ' ��R�ԍ���v
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then  ' ��Ĕԍ���v
                        dblX = typResistorInfoArray(i).ArrCut(j).dblTeachPointX      ' è��ݸ��߲��
                        dblY = typResistorInfoArray(i).ArrCut(j).dblTeachPointY
                        bRetc = True
                        Exit For
                    End If
                Next
                If (bRetc) Then
                    Exit For
                End If
            End If
        Next
        GetCutTeachPoint = bRetc
    End Function
#End Region

#Region "�w���R�ԍ��A��Ĕԍ���è��ݸ��߲�Ă�ݒ肷��"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ���è��ݸ��߲�Ă�ݒ肷��</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<param name="dblX"    >(INP) è��ݸ��߲��X</param>
    '''<param name="dblY"    >(INP) è��ݸ��߲��Y</param>
    '''<returns>TRUE:����, FALSE:���s</returns>
    '''<remarks>�ް����㏑�����邽�߁A�����ް����Ȃ��ꍇ�͎��s����</remarks>
    '''=========================================================================
    Public Function SetCutTeachPoint(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef dblX As Double, ByRef dblY As Double) As Boolean
        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                    ' ��R�ԍ���v
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then  ' ��Ĕԍ���v
                        typResistorInfoArray(i).ArrCut(j).dblTeachPointX = dblX      ' è��ݸ��߲��
                        typResistorInfoArray(i).ArrCut(j).dblTeachPointY = dblY
                        bRetc = True
                        Exit For
                    End If
                Next
            End If
        Next
        SetCutTeachPoint = bRetc
    End Function

#End Region

#Region "��R�ԍ��ƶ�Ĕԍ����綯��ް��̒�R��R�ް���Ԃ�"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ��̶���ް��z��ԍ���Ԃ�</summary>
    '''<param name="i"       >(INP)��R�ԍ�(1�`9999)</param>
    '''<param name="intCutNo">(INP)��Ĕԍ�(1�`n)</param>
    '''<param name="n"       >(OUT)����ް��̒�R�ް��̲��ޯ��</param>
    '''<param name="nn"      >(OUT)����ް��̲��ޯ��</param>
    '''<returns>TRUE:����, FALSE:���s</returns>
    '''=========================================================================
    Public Function GetResistorCutAddress(ByRef i As Short, ByRef intCutNo As Short, ByRef n As Short, ByRef nn As Short) As Boolean

        Dim bRetc As Boolean
        'Dim intRegNo As Short
        'Dim k As Short
        Dim j As Short

        bRetc = False
        'intRegNo = typResistorInfoArray(i).intResNo                                 ' ��R�ԍ�
        'For k = 1 To MaxCntResist                                                   ' �ő��R
        '    If (intRegNo = typResistorInfoArray(k).intResNo) Then                   ' ��R�ԍ���v
        For j = 1 To MaxCntCut
            If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then ' ��Ĕԍ���v
                n = i
                nn = j
                bRetc = True
                GetResistorCutAddress = bRetc
                Exit Function
            End If
        Next
        '    End If
        'Next
        GetResistorCutAddress = bRetc
    End Function
#End Region

#Region "�w���R�ԍ��A��Ĕԍ��̶�Ď�ʂƕ�����Ԃ�"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ��̶�Ď�ʂƕ�����Ԃ�</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<param name="iKind"   >(OUT) ��Ď��</param>
    '''<param name="iDir"    >(OUT) ����</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetCutKindDir(ByRef intRegNo As Short, ByRef intCutNo As Short, ByRef iKind As Short, ByRef iDir As Short) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim j As Short
        Dim s As String

        bRetc = False
        For i = 1 To MaxCntResist
            If (intRegNo = typResistorInfoArray(i).intResNo) Then                   ' ��R�ԍ���v
                For j = 1 To MaxCntCut
                    If (intCutNo = typResistorInfoArray(i).ArrCut(j).intCutNo) Then ' ��Ĕԍ���v
                        s = typResistorInfoArray(i).ArrCut(j).strCutType            ' ��Č`��
                        ' ��Č`���Ď�ʂɕϊ�
                        iKind = Form1.Utility1.GetCutTypeNum(s.Trim())                      ' �O��̋󔒂��폜���ēn��
                        iDir = typResistorInfoArray(i).ArrCut(j).intCutDir
                        bRetc = True
                        GetCutKindDir = bRetc
                        Exit Function
                    End If
                Next
            End If
        Next
        GetCutKindDir = bRetc
    End Function
#End Region

#Region "NG�f�[�^ �w���R�z��ԍ��̎w�趯Ĕԍ��̶���ް��z��ԍ���Ԃ�"
#If False Then  'V5.0.0.8�@
    '''=========================================================================
    '''<summary>NG�f�[�^ �w���R�z��ԍ��̎w�趯Ĕԍ��̶���ް��z��ԍ���Ԃ�</summary>
    '''<param name="i"       >(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<param name="n"       >(OUT) ����ް��z��ԍ�</param>
    '''<param name="nn"      >(OUT) ����ް��z��Ĕz��ԍ�</param>
    '''<returns>TRUE:����, FALSE:���s</returns>
    '''=========================================================================
    Public Function GetNGResistorCutAddress(ByRef i As Short, ByRef intCutNo As Short, ByRef n As Short, ByRef nn As Short) As Boolean

        Dim bRetc As Boolean
        Dim intRegNo As Short
        Dim k As Short
        Dim j As Short

        bRetc = False
        intRegNo = markResistorInfoArray(i).intResNo                                    ' ��R�ԍ�
        For k = 1 To MaxCntResist
            If (intRegNo = markResistorInfoArray(k).intResNo) Then                      ' ��R�ԍ���v
                For j = 1 To MaxCntCut
                    If (intCutNo = markResistorInfoArray(k).ArrCut(j).intCutNo) Then    ' ��Ĕԍ���v
                        n = k
                        nn = j
                        bRetc = True
                        GetNGResistorCutAddress = bRetc
                        Exit Function
                    End If
                Next
            End If
        Next
        GetNGResistorCutAddress = bRetc
    End Function
#End If
#End Region

#Region "�w���R�ԍ��A��Ĕԍ��̃J�b�g�`��擾"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ��̃J�b�g�`��擾</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<returns>�J�b�g�`��</returns>
    '''=========================================================================
    Public Function GetCutType(ByRef intRegNo As Short, ByRef intCutNo As Short) As String
        On Error GoTo ErrTrap
        GetCutType = typResistorInfoArray(intRegNo).ArrCut(intCutNo).strCutType
        Exit Function

ErrTrap:
        MsgBox("GetCutType() " & DataAccess_001 & vbCrLf & DataAccess_002 & CStr(intRegNo) & vbTab & DataAccess_003 & CStr(intCutNo) & vbCrLf & DataAccess_004 & Err.Number & vbCrLf & DataAccess_005 & Err.Description)
        ' "�f�[�^�Ǐo���G���[�I" & vbCrLf & "��R�ԍ�= " & CStr(intRegNo) & vbTab & "�J�b�g�ԍ�= " & CStr(intCutNo) & vbCrLf & "�G���[�R�[�h�F" & Err.Number & vbCrLf & "�G���[�����F" & Err.Description)
        GetCutType = CStr(-1)
    End Function
#End Region

#Region "�w���R�ԍ��A��Ĕԍ���Q���[�g�擾"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ���Q���[�g�擾</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<returns>Q���[�g</returns>
    '''=========================================================================
    Public Function GetQSwitchRate(ByRef intRegNo As Short, ByRef intCutNo As Short) As Double
        On Error GoTo ErrTrap
        GetQSwitchRate = typResistorInfoArray(intRegNo).ArrCut(intCutNo).dblQRate
        Exit Function

ErrTrap:
        MsgBox("GetQSwitchRate() " & DataAccess_001 & vbCrLf & DataAccess_002 & CStr(intRegNo) & vbTab & DataAccess_003 & CStr(intCutNo) & vbCrLf & DataAccess_004 & Err.Number & vbCrLf & DataAccess_005 & Err.Description)
        ' "�f�[�^�Ǐo���G���[�I" & vbCrLf & "��R�ԍ�= " & CStr(intRegNo) & vbTab & "�J�b�g�ԍ�= " & CStr(intCutNo) & vbCrLf & "�G���[�R�[�h�F" & Err.Number & vbCrLf & "�G���[�����F" & Err.Description)
        GetQSwitchRate = -1
    End Function
#End Region

#Region "�w���R�ԍ��A��Ĕԍ��̃��_�[�ԋ����擾"
    '''=========================================================================
    '''<summary>�w���R�ԍ��A��Ĕԍ��̃��_�[�ԋ����擾</summary>
    '''<param name="intRegNo">(INP) ��R�ԍ�</param>
    '''<param name="intCutNo">(INP) ��Ĕԍ�</param>
    '''<returns>���_�[�ԋ���</returns>
    '''=========================================================================
    Public Function GetLadderDistance(ByRef intRegNo As Short, ByRef intCutNo As Short) As Short

        On Error GoTo ErrTrap
        GetLadderDistance = typResistorInfoArray(intRegNo).ArrCut(intCutNo).intRadderInterval
        Exit Function

ErrTrap:
        MsgBox("GetLadderDistance() " & DataAccess_001 & vbCrLf & DataAccess_002 & CStr(intRegNo) & vbTab & DataAccess_003 & CStr(intCutNo) & vbCrLf & DataAccess_004 & Err.Number & vbCrLf & DataAccess_005 & Err.Description)
        ' "�f�[�^�Ǐo���G���[�I" & vbCrLf & "��R�ԍ�= " & CStr(intRegNo) & vbTab & "�J�b�g�ԍ�= " & CStr(intCutNo) & vbCrLf & "�G���[�R�[�h�F" & Err.Number & vbCrLf & "�G���[�����F" & Err.Description)
        GetLadderDistance = -1
    End Function

#End Region

#Region "�w���R�����ɒǉ������ٰ�ߊԲ�����ْl�擾"
    '''=========================================================================
    '''<summary>�w���R�����ɒǉ������ٰ�ߊԲ�����ْl�擾</summary>
    '''<param name="intBlockNo">(INP) ��ۯ��ԍ�</param>
    '''<param name="dblX">      (OUT) �ï�߲������</param>
    '''<returns>TRUE:�ް�����, FALSE:�ް��Ȃ�</returns>
    '''=========================================================================
    Public Function GetTy2Data(ByRef intBlockNo As Short, ByRef dblX As Double) As Boolean

        Dim bRetc As Boolean
        Dim i As Short
        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim cs As Double
        Dim intCDir As Short

        ' ���߻���
        dblCSx = typPlateInfo.dblChipSizeXDir
        dblCSy = typPlateInfo.dblChipSizeYDir

        intCDir = typPlateInfo.intResistDir               ' �`�b�v���ѕ����擾(CHIP-NET�̂�)

        If intCDir = 0 Then
            cs = dblCSx
        Else
            cs = dblCSy
        End If

        bRetc = False
        For i = 0 To 255
            'UPGRADE_WARNING: �I�u�W�F�N�g typTy2InfoArray(i).intTy21 �̊���v���p�e�B�������ł��܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' ���N���b�N���Ă��������B
            If (intBlockNo = typTy2InfoArray(i).intTy21) Then ' ��ٰ�ߔԍ���v

                'UPGRADE_WARNING: �I�u�W�F�N�g typTy2InfoArray().dblTy22 �̊���v���p�e�B�������ł��܂���ł����B �ڍׂɂ��ẮA'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' ���N���b�N���Ă��������B
                dblX = typTy2InfoArray(i).dblTy22

                bRetc = True
                Exit For

            End If
        Next

        GetTy2Data = bRetc
    End Function
#End Region

    '' '' ''#Region "�ް��߼޼�ł͈̔͂�����"
    '' '' ''    '''=========================================================================
    '' '' ''    '''<summary>�ް��߼޼�ł͈̔͂�����</summary>
    '' '' ''    '''<remarks></remarks>
    '' '' ''    '''=========================================================================
    '' '' ''    Public Function ChkBP_Area() As Short

    '' '' ''        Dim RNO As Short 'resistor counter
    '' '' ''        Dim Cno As Short 'cut counter
    '' '' ''        Dim bpofx As Double 'BP offset X
    '' '' ''        Dim bpofy As Double 'BP offset Y
    '' '' ''        Dim startpx As Double 'start point X
    '' '' ''        Dim startpy As Double 'start point Y
    '' '' ''        Dim lenxy As Double 'cut length 1
    '' '' ''        Dim dirxy As Short 'cut direc 1
    '' '' ''        Dim lenxy2 As Double 'cut length 2
    '' '' ''        Dim dirxy2 As Short 'cut direc 2
    '' '' ''        Dim ChipNum As Short 'chip number
    '' '' ''        Dim CutNum As Short 'cut number
    '' '' ''        Dim Cdir As Short 'cut direc

    '' '' ''        Dim judge_flg As Short

    '' '' ''        judge_flg = 0

    '' '' ''        ' BP�ʒu�̾��X,Y�ݒ�
    '' '' ''        bpofx = typPlateInfo.dblBpOffSetXDir
    '' '' ''        bpofy = typPlateInfo.dblBpOffSetYDir
    '' '' ''        Call GetChipNum(ChipNum)                                            ' ���ߐ��擾

    '' '' ''        For RNO = 1 To ChipNum
    '' '' ''            ' ��Đ��擾
    '' '' ''            Call GetRegCutNum(RNO, CutNum)
    '' '' ''            For Cno = 1 To CutNum
    '' '' ''                Call GetCutStartPoint(RNO, Cno, startpx, startpy)           ' �����߲��XY�擾
    '' '' ''                Cdir = typResistorInfoArray(RNO).ArrCut(Cno).intCutDir      ' ��ĕ����擾
    '' '' ''                ' �J�b�g�^�C�v���(Cdir)����J�b�g����(dirxy, dirxy2)�����߂ĕԂ�
    '' '' ''                Call Form1.Utility1.sGetCuttypeXY(dirxy, dirxy2, Cdir)              ' ��ĕ����擾2
    '' '' ''                lenxy = typResistorInfoArray(RNO).ArrCut(Cno).dblMaxCutLength    ' ��Ē�1�擾
    '' '' ''                lenxy2 = typResistorInfoArray(RNO).ArrCut(Cno).dblMaxCutLengthL  ' ��Ē�2�擾

    '' '' ''                ' BP���H�͈����� OcxSystem���g�p
    '' '' ''                judge_flg = Form1.System1.BpLimitCheck(gSysPrm, bpofx, bpofy, startpx, startpy, lenxy, dirxy, lenxy2, dirxy2)
    '' '' ''                ' �װ�Ȃ��������~
    '' '' ''                If judge_flg <> 0 Then Exit For
    '' '' ''            Next Cno
    '' '' ''            ' �װ�Ȃ��������~
    '' '' ''            If judge_flg <> 0 Then Exit For
    '' '' ''        Next RNO

    '' '' ''        'return status
    '' '' ''        ChkBP_Area = judge_flg
    '' '' ''    End Function
    '' '' ''#End Region

#Region "�ް��߼޼�ł̃I�t�Z�b�g�l�̎擾"
    '''=========================================================================
    '''<summary>�ް��߼޼�ł̃I�t�Z�b�g�l�̎擾</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetBpOffset(ByRef BpOffX As Double, ByRef BpOffY As Double)
        BpOffX = typPlateInfo.dblBpOffSetXDir
        BpOffY = typPlateInfo.dblBpOffSetYDir
    End Sub
#End Region

#Region "�u���b�N�T�C�Y�̎擾"
    '''=========================================================================
    '''<summary>�u���b�N�T�C�Y�̎擾</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetBlockSize(ByRef BsizeX As Double, ByRef BsizeY As Double)

        BsizeX = typPlateInfo.dblBlockSizeXDir
        BsizeY = typPlateInfo.dblBlockSizeYDir
    End Sub
#End Region

#Region "�ް��߼޼�ł̃I�t�Z�b�g�l�̐ݒ�"
    '''=========================================================================
    '''<summary>�ް��߼޼�ł̃I�t�Z�b�g�l�̐ݒ�</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetBpOffset(ByVal BpOffX As Double, ByVal BpOffY As Double)
        Dim strMSG As String        ' ###270

        With typPlateInfo
            If .dblBpOffSetXDir <> 0 And BpOffX = 0 And .dblBpOffSetYDir <> 0 And BpOffY = 0 Then       ' ###270
                strMSG = "SetBpOffset() : BP Offset Update 0 :BpOffX= " + CStr(BpOffX) + " :BpOffY= " + CStr(BpOffY) + " :dblBpOffSetXDir=" + CStr(.dblBpOffSetXDir) + " :dblBpOffSetYDir=" + CStr(.dblBpOffSetYDir)
                MsgBox(strMSG)
                Exit Sub
            End If
            .dblBpOffSetXDir = BpOffX
            .dblBpOffSetYDir = BpOffY

            'INTRIM���̃I�t�Z�b�g�l���X�V����
            Call BPOFF(.dblBpOffSetXDir, .dblBpOffSetYDir)

        End With
    End Sub
#End Region

    'V5.0.0.6�I���g�p�Ȃ̂ŃR�����g�A�E�g��
    '#Region "NGϰ�ݸޗp�̽����߲�Ă��Đݒ肷��yCHIP,NET�p�z(���g�p)"
    '    '''=========================================================================
    '    '''<summary>NGϰ�ݸޗp�̽����߲�Ă��Đݒ肷��yCHIP,NET�p�z</summary>
    '    '''<remarks>TrimDataEditor��SetNG_MarkingPos()�Ɠ�������</remarks>
    '    '''=========================================================================
    '    Public Sub SetNG_MarkingPos()

    '        Dim ChipNum As Short                                    ' ��R��(���ߐ�)
    '        Dim CirNum As Short                                     ' �T�[�L�b�g�ԍ�
    '        Dim rn As Short                                         ' ��R�ԍ�
    '        Dim cn As Short                                         ' �J�b�g�ԍ� 
    '        Dim strMSG As String

    '        Try
    '            ' NGϰ�ݸނ�è��ݸ��߲��/�����߲�Ă��Z�o���A�ޯ̧�Ɋi�[����
    '            If (gTkyKnd = KND_TKY) Then Exit Sub
    '            Call GetChipNum(ChipNum)                                ' ��R���擾
    '            CirNum = 1                                              ' �T�[�L�b�g�ԍ�

    '            For rn = 1 To ChipNum                                   ' ��R����ٰ��

    '                ' �}�[�L���O�p�o�b�t�@�Ƀ}�[�L���O�f�[�^(1000��)��ݒ肷��(CHIP/NET)
    '                Call CopyResDataToMarkData(rn)

    '                ' �}�[�L���O�p�f�[�^�̽����߲��X,Y��è��ݸ��߲��X,Y��ݒ肷��
    '                For cn = 1 To markResistorInfoArray(rn).intCutCount ' �J�b�g����ٰ��

    '                    ' ϰ�ݸ��ް��̽����߲��X,Y���R�f�[�^��ϰ�ݸޗp�ޯ̧��è��ݸ��߲��X,Y�ɐݒ肷��
    '                    typResistorInfoArray(1000).ArrCut(cn).dblTeachPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX
    '                    typResistorInfoArray(1000).ArrCut(cn).dblTeachPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY
    '                    markResistorInfoArray(1000).ArrCut(cn).dblTeachPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX
    '                    markResistorInfoArray(1000).ArrCut(cn).dblTeachPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY

    '                    ' NGϰ�ݸ��߲�Ă��Z�o��ϰ�ݸޗp�ޯ̧�̽����߲��X,Y�ɐݒ肷��
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        ' CHIP��
    '                        If (typPlateInfo.intResistDir = 0) Then     ' ��R(����)���ѕ���(0:X, 1:Y)���l�� 
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX + typPlateInfo.dblChipSizeXDir * (rn - 1)
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY
    '                        Else
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX
    '                            markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY + typPlateInfo.dblChipSizeYDir * (rn - 1)
    '                        End If

    '                        ' NET��
    '                    Else
    '                        ' �T�[�L�b�g�ԍ����ς���� ?
    '                        If (markResistorInfoArray(rn).intCircuitGrp <> CirNum) Then
    '                            CirNum = CirNum + 1                     ' �T�[�L�b�g�ԍ��X�V
    '                        End If
    '                        ' �T�[�L�b�g���W�f�[�^(NET�p)�̍��W�f�[�^�����Z
    '                        markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX + typCirAxisInfoArray(CirNum).dblCaP2
    '                        markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY + typCirAxisInfoArray(CirNum).dblCaP3
    '                    End If

    '                    ' �}�[�L���O�p�f�[�^��è��ݸ��߲�Ăɽ����߲�Ă�ݒ肷��
    '                    markResistorInfoArray(rn).ArrCut(cn).dblTeachPointX = markResistorInfoArray(rn).ArrCut(cn).dblStartPointX
    '                    markResistorInfoArray(rn).ArrCut(cn).dblTeachPointY = markResistorInfoArray(rn).ArrCut(cn).dblStartPointY

    '                Next cn
    '            Next rn

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            strMSG = "DataAccess.SetNG_MarkingPos() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try

    '    End Sub
    '#End Region
    'V5.0.0.6�I���g�p�Ȃ̂ŃR�����g�A�E�g��

#Region "�}�[�L���O�p�o�b�t�@�Ƀ}�[�L���O�f�[�^(1000��)��ݒ肷��((CHIP/NET)(���g�p)"
#If False Then  'V5.0.0.8�@
    '''=========================================================================
    '''<summary>�}�[�L���O�p�o�b�t�@�Ƀ}�[�L���O�f�[�^(1000��)��ݒ肷��((CHIP/NET)</summary>
    '''<param name="rn">(INP) ��R�ԍ�</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CopyResDataToMarkData(ByVal rn As Integer)

        Dim cn As Integer

        'For rn = 1 To MaxCntResist
        markResistorInfoArray(rn).intResNo = typResistorInfoArray(1000).intResNo                                  ' ��R�ԍ�
        markResistorInfoArray(rn).intResMeasMode = typPlateInfo.intMeasType                                       ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��) ���ύX
        markResistorInfoArray(rn).intResMeasType = typResistorInfoArray(1000).intResMeasType                      ' ����^�C�v(0:���� ,1:�����x)�@���ǉ�
        markResistorInfoArray(rn).intCircuitGrp = typResistorInfoArray(1000).intCircuitGrp                        ' ���������
        markResistorInfoArray(rn).intCircuitGrp = typResistorInfoArray(rn).intCircuitGrp                          ' ��������Ă��Đݒ�
        markResistorInfoArray(rn).intProbHiNo = typResistorInfoArray(1000).intProbHiNo                            ' ��۰�ޔԍ�(HI��)
        markResistorInfoArray(rn).intProbLoNo = typResistorInfoArray(1000).intProbLoNo                            ' ��۰�ޔԍ�(LO��)
        markResistorInfoArray(rn).intProbAGNo1 = typResistorInfoArray(1000).intProbAGNo1                          ' ��۰�ޔԍ�(1)
        markResistorInfoArray(rn).intProbAGNo2 = typResistorInfoArray(1000).intProbAGNo2                          ' ��۰�ޔԍ�(2)
        markResistorInfoArray(rn).intProbAGNo3 = typResistorInfoArray(1000).intProbAGNo3                          ' ��۰�ޔԍ�(3)
        markResistorInfoArray(rn).intProbAGNo4 = typResistorInfoArray(1000).intProbAGNo4                          ' ��۰�ޔԍ�(4)
        markResistorInfoArray(rn).intProbAGNo5 = typResistorInfoArray(1000).intProbAGNo5                          ' ��۰�ޔԍ�(5)
        markResistorInfoArray(rn).strExternalBits = typResistorInfoArray(1000).strExternalBits                    ' EXTERNAL BITS
        markResistorInfoArray(rn).intPauseTime = typResistorInfoArray(1000).intPauseTime                          ' �߰�����
        markResistorInfoArray(rn).intTargetValType = typResistorInfoArray(1000).intTargetValType                  ' �ڕW�l�w��
        markResistorInfoArray(rn).intBaseResNo = typResistorInfoArray(1000).intBaseResNo                          ' �ް���R�ԍ�
        markResistorInfoArray(rn).dblTrimTargetVal = typResistorInfoArray(1000).dblTrimTargetVal                  ' ���ݸޖڕW�l
        markResistorInfoArray(rn).strRatioTrimTargetVal = typResistorInfoArray(1000).strRatioTrimTargetVal        ' �g���~���O�ڕW�l(���V�I�v�Z��) 
        markResistorInfoArray(rn).dblDeltaR = typResistorInfoArray(1000).dblDeltaR                                ' ���q
        markResistorInfoArray(rn).intSlope = typResistorInfoArray(1000).intSlope                                  ' �d���ω��۰��
        markResistorInfoArray(rn).dblCutOffRatio = typResistorInfoArray(1000).dblCutOffRatio                      ' �؂�グ�{��
        markResistorInfoArray(rn).dblProbCfmPoint_Hi_X = typResistorInfoArray(1000).dblProbCfmPoint_Hi_X          ' �v���[�u�m�F�ʒu HI X���W
        markResistorInfoArray(rn).dblProbCfmPoint_Hi_Y = typResistorInfoArray(1000).dblProbCfmPoint_Hi_Y          ' �v���[�u�m�F�ʒu HI Y���W
        markResistorInfoArray(rn).dblProbCfmPoint_Lo_X = typResistorInfoArray(1000).dblProbCfmPoint_Lo_X          ' �v���[�u�m�F�ʒu LO X���W
        markResistorInfoArray(rn).dblProbCfmPoint_Lo_Y = typResistorInfoArray(1000).dblProbCfmPoint_Lo_Y          ' �v���[�u�m�F�ʒu LO Y���W
        markResistorInfoArray(rn).dblInitTest_HighLimit = typResistorInfoArray(1000).dblInitTest_HighLimit        ' �Ƽ��ý�HIGH�Я�
        markResistorInfoArray(rn).dblInitTest_LowLimit = typResistorInfoArray(1000).dblInitTest_LowLimit          ' �Ƽ��ý�LOW�Я�
        markResistorInfoArray(rn).dblFinalTest_HighLimit = typResistorInfoArray(1000).dblFinalTest_HighLimit      ' ̧���ý�HIGH�Я�
        markResistorInfoArray(rn).dblFinalTest_LowLimit = typResistorInfoArray(1000).dblFinalTest_LowLimit        ' ̧���ý�LOW�Я�
        markResistorInfoArray(rn).dblInitOKTest_HighLimit = typResistorInfoArray(1000).dblInitOKTest_HighLimit    ' �Ƽ��OKý�HIGH�Я�
        markResistorInfoArray(rn).dblInitOKTest_LowLimit = typResistorInfoArray(1000).dblInitOKTest_LowLimit      ' �Ƽ��OKý�LOW�Я�
        markResistorInfoArray(rn).intCutCount = typResistorInfoArray(1000).intCutCount                            ' ��Đ�
        markResistorInfoArray(rn).intCutReviseMode = typResistorInfoArray(1000).intCutReviseMode                  ' ��� �␳
        markResistorInfoArray(rn).intCutReviseDispMode = typResistorInfoArray(1000).intCutReviseDispMode          ' �\��Ӱ��
        markResistorInfoArray(rn).intCutRevisePtnNo = typResistorInfoArray(1000).intCutRevisePtnNo                ' ����� No.
        markResistorInfoArray(rn).dblCutRevisePosX = typResistorInfoArray(1000).dblCutRevisePosX                  ' ��ĕ␳�ʒuX
        markResistorInfoArray(rn).dblCutRevisePosY = typResistorInfoArray(1000).dblCutRevisePosY                  ' ��ĕ␳�ʒuY
        markResistorInfoArray(rn).intIsNG = typResistorInfoArray(1000).intIsNG                                    ' NG�L��

        ' ����ް��\���̂��R�s�[
        For cn = 1 To MaxCutInfo
            markResistorInfoArray(rn).ArrCut(cn).intCutNo = typResistorInfoArray(1000).ArrCut(cn).intCutNo                            ' ��Ĕԍ�
            markResistorInfoArray(rn).ArrCut(cn).intDelayTime = typResistorInfoArray(1000).ArrCut(cn).intDelayTime                    ' �ިڲ���
            markResistorInfoArray(rn).ArrCut(cn).strCutType = typResistorInfoArray(1000).ArrCut(cn).strCutType                        ' ��Č`��
            markResistorInfoArray(rn).ArrCut(cn).dblTeachPointX = typResistorInfoArray(1000).ArrCut(cn).dblTeachPointX                ' è��ݸ��߲��X
            markResistorInfoArray(rn).ArrCut(cn).dblTeachPointY = typResistorInfoArray(1000).ArrCut(cn).dblTeachPointY                ' è��ݸ��߲��Y
            markResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(1000).ArrCut(cn).dblStartPointX                ' �����߲��X
            markResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(1000).ArrCut(cn).dblStartPointY                ' �����߲��Y
            markResistorInfoArray(rn).ArrCut(cn).dblCutSpeed = typResistorInfoArray(1000).ArrCut(cn).dblCutSpeed                      ' ��Ľ�߰��
            markResistorInfoArray(rn).ArrCut(cn).dblQRate = typResistorInfoArray(1000).ArrCut(cn).dblQRate                            ' Q����ڰ�
            markResistorInfoArray(rn).ArrCut(cn).dblCutOff = typResistorInfoArray(1000).ArrCut(cn).dblCutOff                          ' ��ĵ̒l
            markResistorInfoArray(rn).ArrCut(cn).dblJudgeLevel = typResistorInfoArray(1000).ArrCut(cn).dblJudgeLevel                  ' �ؑփ|�C���g (���ް�����(���ω���))
            markResistorInfoArray(rn).ArrCut(cn).dblCutOffOffset = typResistorInfoArray(1000).ArrCut(cn).dblCutOffOffset              ' ��ĵ̵̾��
            markResistorInfoArray(rn).ArrCut(cn).intPulseWidthCtrl = typResistorInfoArray(1000).ArrCut(cn).intPulseWidthCtrl          ' ��ٽ������
            markResistorInfoArray(rn).ArrCut(cn).dblPulseWidthTime = typResistorInfoArray(1000).ArrCut(cn).dblPulseWidthTime          ' ��ٽ������
            markResistorInfoArray(rn).ArrCut(cn).dblLSwPulseWidthTime = typResistorInfoArray(1000).ArrCut(cn).dblLSwPulseWidthTime    ' LSw��ٽ������
            markResistorInfoArray(rn).ArrCut(cn).intCutDir = typResistorInfoArray(1000).ArrCut(cn).intCutDir                          ' ��ĕ���
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLength              ' �ő嶯èݸޒ�
            markResistorInfoArray(rn).ArrCut(cn).dblLTurnPoint = typResistorInfoArray(1000).ArrCut(cn).dblLTurnPoint                  ' L����߲��
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLengthL            ' L��݌�̍ő嶯èݸޒ�
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthHook = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLengthHook      ' ̯���݌�̶�èݸޒ�
            markResistorInfoArray(rn).ArrCut(cn).dblR1 = typResistorInfoArray(1000).ArrCut(cn).dblR1                                  ' R1
            markResistorInfoArray(rn).ArrCut(cn).dblR2 = typResistorInfoArray(1000).ArrCut(cn).dblR2                                  ' R2
            markResistorInfoArray(rn).ArrCut(cn).intCutAngle = typResistorInfoArray(1000).ArrCut(cn).intCutAngle                      ' �΂߶�Ă̐؂�o���p�x
            markResistorInfoArray(rn).ArrCut(cn).dblCutSpeed2 = typResistorInfoArray(1000).ArrCut(cn).dblCutSpeed2                    ' ��Ľ�߰��2
            markResistorInfoArray(rn).ArrCut(cn).dblQRate2 = typResistorInfoArray(1000).ArrCut(cn).dblQRate2                          ' Q����ڰ�2
            ''''    ��436K�̃p�����[�^�B432��INTRTM���ł͍\���̒�`�Ȃ��ׁA��U�폜
            ''''markResistorInfoArray(rn).ArrCut(cn).dblCP53 = typResistorInfoArray(1000).ArrCut(cn).dblCP53                          ' Q����ڰ�3
            ''''markResistorInfoArray(rn).ArrCut(cn).dblCP54 = typResistorInfoArray(1000).ArrCut(cn).dblCP54                          ' �ؑւ��߲��
            markResistorInfoArray(rn).ArrCut(cn).dblESPoint = typResistorInfoArray(1000).ArrCut(cn).dblESPoint                        ' ���޾ݽ�߲��
            markResistorInfoArray(rn).ArrCut(cn).dblESJudgeLevel = typResistorInfoArray(1000).ArrCut(cn).dblESJudgeLevel              ' ���޾ݽ�̔���ω���
            markResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthES = typResistorInfoArray(1000).ArrCut(cn).dblMaxCutLengthES          ' ���޾ݽ��̶�Ē�
            markResistorInfoArray(rn).ArrCut(cn).intIndexCnt = typResistorInfoArray(1000).ArrCut(cn).intIndexCnt                      ' ���ޯ����
            markResistorInfoArray(rn).ArrCut(cn).intMeasMode = typResistorInfoArray(1000).ArrCut(cn).intMeasMode                      ' ����Ӱ��
            markResistorInfoArray(rn).ArrCut(cn).dblPitch = typResistorInfoArray(1000).ArrCut(cn).dblPitch                            ' �߯�
            markResistorInfoArray(rn).ArrCut(cn).intStepDir = typResistorInfoArray(1000).ArrCut(cn).intStepDir                        ' �ï�ߕ���
            markResistorInfoArray(rn).ArrCut(cn).intCutCnt = typResistorInfoArray(1000).ArrCut(cn).intCutCnt                          ' �{��
            markResistorInfoArray(rn).ArrCut(cn).dblUCutDummy1 = typResistorInfoArray(1000).ArrCut(cn).dblUCutDummy1                  ' U��ėp��а
            markResistorInfoArray(rn).ArrCut(cn).dblUCutDummy2 = typResistorInfoArray(1000).ArrCut(cn).dblUCutDummy2                  ' U��ėp��а
            markResistorInfoArray(rn).ArrCut(cn).dblESChangeRatio = typResistorInfoArray(1000).ArrCut(cn).dblESChangeRatio            ' ���޾ݽ��̕ω���
            markResistorInfoArray(rn).ArrCut(cn).intESConfirmCnt = typResistorInfoArray(1000).ArrCut(cn).intESConfirmCnt              ' ���޾ݽ��̊m�F��
            markResistorInfoArray(rn).ArrCut(cn).intRadderInterval = typResistorInfoArray(1000).ArrCut(cn).intRadderInterval          ' ��ް�ԋ���
            markResistorInfoArray(rn).ArrCut(cn).dblZoom = typResistorInfoArray(1000).ArrCut(cn).dblZoom                              ' �{��
            markResistorInfoArray(rn).ArrCut(cn).strChar = typResistorInfoArray(1000).ArrCut(cn).strChar                              ' ������
        Next cn
        'Next rn

    End Sub
#End If
#End Region

#Region "�f�[�^�ҏW�I�����̏���������(NET)(���g�p)"
    '''=========================================================================
    '''<summary>�f�[�^�ҏW�I�����̏���������(NET)</summary>
    '''<remarks>NET�ɂ̂ݑ��݂���R�[�h</remarks>
    '''=========================================================================
    Public Sub DataEditExitInit()

        Dim iArray As Short
        Dim iResMax As Short

        ' �g�������[�h�������ʁ���R�̏ꍇ
        If typPlateInfo.intMeasType = 0 Then

            ' �ő��R�����擾
            iResMax = typPlateInfo.intResistCntInGroup * typPlateInfo.intCircuitCntInBlock
            ' �d���X���[�v�͓��͕s�Ȃ̂ŏ��������Ă���
            For iArray = 1 To iResMax
                typResistorInfoArray(iArray).intSlope = 0
            Next
        End If

    End Sub
#End Region

    '#Region "��ۯ����ނ��Z�o����"
    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    ''''(2010/11/09)
    '    ''''    �u���b�N�T�C�Y�͕ҏW���A��������BpPosADJ��StagePosADJ���s���ɐݒ�
    '    ''''    �u���b�N�T�C�YX = ��R���~�iX�����`�b�v�T�C�Y�j
    '    ''''    �u���b�N�T�C�YY = ��R���~�iY�����`�b�v�T�C�Y�j
    '    ''''
    '    ''''    ���L�̊֐��͍ŏI�I�ɂ͍폜����B
    '    ''''    TX�ł��ݒ肷�邽�ߍ폜���Ȃ�
    '    ''''    
    '    ''''    �u���b�N�T�C�Y
    '    ''''    (�`�b�v���т�X�����̏ꍇ�j
    '    ''''    �u���b�N�T�C�YX = (��R��(�`�b�v��) �~ X�����`�b�v�T�C�Y + BP�O���[�v�Ԋu) �~ BP�O���[�v��
    '    ''''    �u���b�N�T�C�YY = Y�����`�b�v�T�C�Y
    '    ''''    (�`�b�v���т�Y�����̏ꍇ�j
    '    ''''    �u���b�N�T�C�YX = X�����`�b�v�T�C�Y
    '    ''''    �u���b�N�T�C�YY = (��R��(�`�b�v��) �~ Y�����`�b�v�T�C�Y + BP�O���[�v�Ԋu) �~ BP�O���[�v��
    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    '''=========================================================================
    '    '''<summary>��ۯ����ނ��Z�o����</summary>
    '    '''<param name="dblBSX">(OUT) ��ۯ�����X</param>
    '    '''<param name="dblBSY">(OUT) ��ۯ�����Y</param>
    '    '''=========================================================================
    '    Public Sub CalcBlockSize(ByRef dblBSX As Double, ByRef dblBSY As Double)

    '        Dim dblCSx As Double
    '        Dim dblCSy As Double
    '        Dim dblGIx As Double
    '        Dim dblGIy As Double
    '        Dim intGNx As Short
    '        Dim intGNY As Short
    '        Dim intCDir As Short
    '        Dim intChipNum As Short
    '        Dim i As Short
    '        Dim ResNum As Short
    '        Dim GrpIntv As Double
    '        Dim dData As Double

    '        Try
    '            ' NET��
    '            If (gTkyKnd = KND_NET) Then
    '                ' ��ۯ����ޕ␳X,Y
    '                dblBSX = typPlateInfo.dblBlockSizeReviseXDir
    '                dblBSY = typPlateInfo.dblBlockSizeReviseYDir
    '                Exit Sub
    '            End If

    '            ' �O���[�v��X,Y
    '            intGNx = typPlateInfo.intGroupCntInBlockXBp
    '            intGNY = typPlateInfo.intGroupCntInBlockYStage
    '            Call GetChipNum(intChipNum)                         ' �O���[�v���`�b�v��

    '            ' ���߻���
    '            dblCSx = typPlateInfo.dblChipSizeXDir
    '            dblCSy = typPlateInfo.dblChipSizeYDir

    '            dblGIx = typPlateInfo.dblBpGrpItv                   ' BP�O���[�v�C���^�[�o��
    '            dblGIy = typPlateInfo.dblStgGrpItvY                 ' Stage�O���[�v�C���^�[�o��
    '            '            dblGIx = typPlateInfo.dblGroupItvXDir  ' �O���[�v�C���^�[�o��
    '            '            dblGIy = typPlateInfo.dblGroupItvYDir
    '            intCDir = typPlateInfo.intResistDir                 ' �`�b�v���ѕ����擾(CHIP-NET�̂�)

    '            dData = 0.0#
    '            If intCDir = 0 Then                                 ' �`�b�v���ѕ��� = X���� ?
    '                If intGNx = 1 Then
    '                    '1��ٰ��
    '                    dData = dblCSx * intChipNum                 ' �`�b�v�T�C�YX * �`�b�v��
    '                Else
    '                    '������ٰ��
    '                    For i = 1 To intGNx
    '                        '��ٰ���ް��Q��
    '                        Call GetGrpData(i, ResNum, GrpIntv)
    '                        If i = intGNx Then
    '                            dData = dData + (dblCSx * (ResNum - 1))
    '                        Else
    '                            dData = dData + ((dblCSx * (ResNum - 1)) + GrpIntv)
    '                        End If
    '                    Next i
    '                End If
    '                dblBSX = dData
    '                dblBSY = dblCSy

    '            Else
    '                ' �`�b�v���ѕ��� = Y������
    '                If intGNY = 1 Then
    '                    '1��ٰ��
    '                    dData = dblCSy * intChipNum
    '                Else
    '                    '������ٰ��
    '                    For i = 1 To intGNY
    '                        '��ٰ���ް��Q��
    '                        Call GetGrpData(i, ResNum, GrpIntv)
    '                        If i = intGNx Then
    '                            dData = dData + (dblCSy * (ResNum - 1))
    '                        Else
    '                            dData = dData + ((dblCSy * (ResNum - 1)) + GrpIntv)
    '                        End If
    '                    Next i
    '                End If
    '                dblBSX = dblCSx
    '                dblBSY = dData
    '            End If

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            Dim strMSG As String
    '            strMSG = "DataAccess.CalcBlockSize() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try

    '    End Sub

    '#End Region

    ' V4.12.0.0�@�@���@'V6.1.2.0�A
    ''' <summary>
    ''' Y�X�e�b�v�̋t�]����
    ''' </summary>
    ''' <returns>true:�t�]����AFalse�F�t�]���Ȃ�</returns>
    ''' <remarks></remarks>
    Public Function JudgeStepYInverse() As Boolean

        JudgeStepYInverse = False

        ' Y�X�e�b�v���I������Ă���ꍇ�ɁA��A�����ɂ���ăX�e�b�v�𔽓]����
        If (iInverseStepY = 1) And ((typPlateInfo.intDirStepRepeat = STEP_RPT_Y) Or (typPlateInfo.intDirStepRepeat = STEP_RPT_CHIPXSTPY)) Then
            JudgeStepYInverse = True
        End If

        Return JudgeStepYInverse

    End Function
    ' V4.12.0.0�@ ���@'V6.1.2.0�A

End Module

#Region "�g���~���O�f�[�^�\���̂��o�b�N�A�b�v�E��������"
''' <summary>�g���~���O�f�[�^�\���̂��o�b�N�A�b�v�E��������</summary>
''' <remarks>'#4.12.2.0�D</remarks>
Public Class Temporary
    Private _gBlkStagePosX() As Double          ' �u���b�N�̃v���[�g���̃X�e�[�WX�ʒu���W�f�[�^
    Private _gBlkStagePosY() As Double          ' �u���b�N�̃v���[�g���̃X�e�[�WY�ʒu���W�f�[�^
    Private _gPltStagePosX() As Double          ' �v���[�g�̃X�e�[�WX�ʒu���W�f�[�^
    Private _gPltStagePosY() As Double          ' �v���[�g�̃X�e�[�WY�ʒu���W�f�[�^

    Private Shared _instance As Temporary

    ''' <summary>�g���~���O�f�[�^�\���̂��o�b�N�A�b�v����</summary>
    Public Shared Sub Backup()
        _instance = New Temporary()
        DataManager.Backup()            '#5.0.0.8�@

        With _instance
            ._gPltStagePosX = DataAccess.gPltStagePosX.Clone()
            ._gPltStagePosY = DataAccess.gPltStagePosY.Clone()
            ._gBlkStagePosX = DataAccess.gBlkStagePosX.Clone()
            ._gBlkStagePosY = DataAccess.gBlkStagePosY.Clone()
        End With
    End Sub

    ''' <summary>�o�b�N�A�b�v�����g���~���O�f�[�^�\���̂𕜌�����</summary>
    Public Shared Sub Restore()
        If (_instance IsNot Nothing) Then
            DataManager.Restore()       '#5.0.0.8�@

            With _instance
                DataAccess.gPltStagePosX = ._gPltStagePosX.Clone()
                DataAccess.gPltStagePosY = ._gPltStagePosY.Clone()
                DataAccess.gBlkStagePosX = ._gBlkStagePosX.Clone()
                DataAccess.gBlkStagePosY = ._gBlkStagePosY.Clone()
            End With

            _instance = Nothing
        End If
    End Sub

End Class


#End Region
