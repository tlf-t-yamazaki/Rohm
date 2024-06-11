'===============================================================================
'   Description  : TKY,CHIP,NET�̃g���~���O�f�[�^�t�@�C���Ǐo������
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Collections.Generic      'V5.0.0.9�A
Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports LaserFront.Trimmer.TrimData.FileIO          'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module File
#Region "�y���[�J���萔/�ϐ��̒�`�z"
    '---------------------------------------------------------------------------
    '   �t�@�C���o�[�W������
    '---------------------------------------------------------------------------
    '----- �t�@�C���o�[�W������ -----
    'Private SL432HW_FileVer As Short
    'V5.0.0.8�@    Private SL432HW_FileVer As Double                       ' ###066
    Private gStrTkyFileVer As String                        ' TKY�p
#If False Then 'V5.0.0.8�@ LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`(���o�[�W�����p�̒�`�̂ݎc��)
    Private NewFileVer As Integer                           ' �����\�t�g�Ńo�[�W������
    Public Const FILE_VER_10 As Double = 10.0               ' �����\�t�g��
    Public Const FILE_VER_10_01 As Double = 10.01           ' �����\�t�g�� ###229
    Public Const FILE_VER_10_02 As Double = 10.02           ' �����\�t�g�� ###229
    Public Const FILE_VER_10_03 As Double = 10.03           ' �����\�t�g�� ###229
    Public Const FILE_VER_10_04 As Double = 10.04           ' �����\�t�g�� V1.14.0.0�@
    Public Const FILE_VER_10_05 As Double = 10.05           ' �����\�t�g�� V1.14.0.0�@
    Public Const FILE_VER_10_06 As Double = 10.06           ' �����\�t�g�� V1.16.0.0�@
    Public Const FILE_VER_10_07 As Double = 10.07           ' �����\�t�g�� V1.18.0.0�C
    Public Const FILE_VER_10_072 As Double = 10.072         ' �����\�t�g�� V2.0.0.0_24
    Public Const FILE_VER_10_073 As Double = 10.073         ' �����\�t�g�� V2.0.0.0_24
    '                                                       '
    Public Const FILE_VER_10_08 As Double = 10.08           ' �����\�t�g��(�m���^�P�a���� ���T�|�[�g)
    Public Const FILE_VER_10_09 As Double = 10.09           ' �����\�t�g�� V1.23.0.0�A
    Public Const FILE_VER_10_10 As Double = 10.1            ' �����\�t�g�� V4.0.0.0�C
    Public Const FILE_VER_10_11 As Double = 10.11           ' �����\�t�g�� V4.11.0.0�@
    Public Const FILE_VER_CUR As Double = 10.11             ' �����\�t�g��(���ݔŖ�) V4.11.0.0�@
#End If
    '----- TKY�p -----
    Private Const CONST_FILETYPE4 As String = "TKYDATA_Ver4"
    Private Const CONST_FILETYPE4_SP1 As String = "TKYDATA_Ver4_SP1"
    Private Const CONST_FILETYPE5 As String = "TKYDATA_Ver5"
    Private Const CONST_FILETYPE6 As String = "TKYDATA_Ver6"
#If False Then 'V5.0.0.8�@ LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`(���o�[�W�����p�̒�`�̂ݎc��)
    Private Const CONST_FILETYPE10 As String = "TKYDATA_Ver10.00"       ' �����\�t�g��
    Private Const CONST_FILETYPE10_01 As String = "TKYDATA_Ver10.01"    ' �����\�t�g��
    Private Const CONST_FILETYPE10_02 As String = "TKYDATA_Ver10.02"    ' �����\�t�g��(NEC SHOTT�p)
    Private Const CONST_FILETYPE10_03 As String = "TKYDATA_Ver10.03"    ' �����\�t�g�� V1.13.0.0�A
    Private Const CONST_FILETYPE10_04 As String = "TKYDATA_Ver10.04"    ' �����\�t�g�� V1.14.0.0�@
    Private Const CONST_FILETYPE10_05 As String = "TKYDATA_Ver10.05"    ' �����\�t�g�� V1.16.0.0�@
    Private Const CONST_FILETYPE10_06 As String = "TKYDATA_Ver10.06"    ' �����\�t�g�� V1.18.0.0�C
    Private Const CONST_FILETYPE10_07 As String = "TKYDATA_Ver10.07"    ' �����\�t�g�� V1.16.0.0�@
    Private Const CONST_FILETYPE10_072 As String = "TKYDATA_Ver10.072"  ' �����\�t�g�� V2.0.0.0_24
    Private Const CONST_FILETYPE10_073 As String = "TKYDATA_Ver10.073"  ' �����\�t�g�� V2.0.0.0_24
    '                            
    Private Const CONST_FILETYPE10_08 As String = "TKYDATA_Ver10.08"    ' �����\�t�g��(�m���^�P�a���� ���T�|�[�g)
    Private Const CONST_FILETYPE10_09 As String = "TKYDATA_Ver10.09"    ' �����\�t�g�� V1.23.0.0�A
    Private Const CONST_FILETYPE10_10 As String = "TKYDATA_Ver10.10"    ' �����\�t�g��  V4.0.0.0�C
    Private Const CONST_FILETYPE10_11 As String = "TKYDATA_Ver10.11"    ' �����\�t�g��   V4.11.0.0�@
    'Private Const CONST_FILETYPE_CUR As String = "TKYDATA_Ver10.00"    ' �����\�t�g��(���ݔŖ�) V4.11.0.0�@
    Private Const CONST_FILETYPE_CUR As String = "TKYDATA_Ver10.11"     ' �����\�t�g��(���ݔŖ�) V4.11.0.0�@
    '----- CHIP/NET�p -----
    Private FILETYPE01 As String                            ' 1���o��Ver
    Private FILETYPE02 As String                            ' 2���o��Ver
    Private FILETYPE03 As String                            ' 3���o��Ver
    Private FILETYPE04 As String                            ' 4���o��Ver
    Private FILETYPE05 As String                            ' 5���o��Ver
    Private FILETYPE06 As String                            ' 6���o��Ver
    Private FILETYPE07_02 As String                         ' 7���o��VerVer7.0.0.2 V1.14.0.0�E
    Private FILETYPE10 As String                            ' �����\�t�g��
    Private FILETYPE10_01 As String                         ' �����\�t�g��
    Private FILETYPE10_02 As String                         ' �����\�t�g��
    Private FILETYPE10_03 As String                         ' �����\�t�g�� V1.13.0.0�A
    Private FILETYPE10_04 As String                         ' �����\�t�g�� V1.14.0.0�@
    Private FILETYPE10_05 As String                         ' �����\�t�g�� V1.16.0.0�@
    Private FILETYPE10_06 As String                         ' �����\�t�g�� V1.18.0.0�C
    Private FILETYPE10_07 As String                         ' �����\�t�g�� V1.18.0.0�C
    Private FILETYPE10_072 As String                        ' �����\�t�g�� V2.0.0.0_24
    Private FILETYPE10_073 As String                        ' �����\�t�g�� V2.0.0.0_24
    Private FILETYPE10_08 As String                         ' �����\�t�g��(�m���^�P�a���� ���T�|�[�g)
    Private FILETYPE10_09 As String                         ' �����\�t�g�� V1.23.0.0�A
    Private FILETYPE10_10 As String                         ' �����\�t�g�� V4.0.0.0�C
    Private FILETYPE10_11 As String                         ' �����\�t�g�� V4.11.0.0�@
    Private FILETYPE_CUR As String                          ' �����\�t�g��(���ݔŖ�) V4.0.0.0�C ###066
#End If
    'V5.0.0.8�@ �ǉ�           ��
    Private Const FILETYPE_CHIP01 As String = "TKYCHIP_SL432HW_Ver1.00"   ' 1���o��Ver
    Private Const FILETYPE_CHIP02 As String = "TKYCHIP_SL432HW_Ver1.10"   ' 2���o��Ver
    Private Const FILETYPE_CHIP03 As String = "TKYCHIP_SL432HW_Ver1.20"   ' 3���o��Ver
    Private Const FILETYPE_CHIP04 As String = "TKYCHIP_SL432HW_Ver1.30"   ' 4���o��Ver
    Private Const FILETYPE_CHIP05 As String = "TKYCHIP_SL432HW_Ver1.40"   ' 5���o��Ver
    Private Const FILETYPE_CHIP06 As String = "TKYCHIP_SL432HW_Ver1.50"   ' 6���o��Ver
    Private Const FILETYPE_CHIP07_02 As String = "TKYCHIP_SL432HW_Ver7.0.0.2" 'V5.0.0.0-22

    Private Const FILETYPE_NET01 As String = "TKYNET_SL432HW_Ver1.00"     ' 1���o��Ver
    Private Const FILETYPE_NET02 As String = "TKYNET_SL432HW_Ver1.01"     ' 2���o��Ver
    Private Const FILETYPE_NET03 As String = "TKYNET_SL432HW_Ver1.02"     ' 3���o��Ver
    Private Const FILETYPE_NET07_02 As String = "TKYNET_SL432HW_Ver7.0.0.2"    'V5.0.0.0-22
    'V5.0.0.8�@ �ǉ�           ��

    '----- V1.23.0.0�G�� -----
    '----- CHIP/NET�p(SL436K) -----
    Private FILETYPE_K As String = ""                           ' SL436K�̃t�@�C���o�[�W����
    Private FILETYPE01_K As String = "TKYCHIP_SL436K_Ver0.00"   ' 1���o��Ver
    Private FILETYPE02_K As String = "TKYCHIP_SL436K_Ver1.00"   ' 2���o��Ver
    Private FILETYPE03_K As String = "TKYCHIP_SL436K_Ver1.10"   ' 3���o��Ver
    Private FILETYPE04_K As String = "TKYCHIP_SL436K_Ver1.20"   ' 4���o��Ver
    '----- V1.23.0.0�G�� -----
#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
    '---------------------------------------------------------------------------
    '   �Z�N�V������
    '---------------------------------------------------------------------------
    '----- TKY�p -----
    Private Const CONST_VERSION As String = "[FILE VERSION]"
    Private Const CONST_PLATE As String = "[PLATE]"
    Private Const CONST_PLATE1 As String = "[PLATE1]"
    Private Const CONST_PLATE2 As String = "[PLATE2]"
    Private Const CONST_PLATE3 As String = "[PLATE3]"
    Private Const CONST_CIRCUIT As String = "[CIRCUIT]"
    Private Const CONST_RESISTOR_DATA As String = "[RESIST]"
    Private Const CONST_CUT_DATA As String = "[CUT]"
    Private Const CONST_IKEI_DATA As String = "[IKEI]"
    Private Const CONST_CRC As String = "C="
    Private Const CONST_SINSYUKU_SELECT As String = "[SINSYUKU]"      'V1.13.0.0�D

    '----- CHIP�p/NET�p -----
    Private Const FILE_CONST_VERSION As String = "[FILE VERSION]"
    Private Const FILE_CONST_PLATE_01 As String = "[PLATE01]"
    Private Const FILE_CONST_PLATE_02 As String = "[PLATE02]"
    Private Const FILE_CONST_PLATE_03 As String = "[PLATE03]"
    Private Const FILE_CONST_PLATE_04 As String = "[PLATE04]"
    Private Const FILE_CONST_PLATE_05 As String = "[PLATE05]"
    Private Const FILE_CONST_PLATE_06 As String = "[PLATE06]"       ' V1.13.0.0�A
    Private Const FILE_CONST_STEPDATA As String = "[STEP]"
    Private Const FILE_CONST_RESISTOR As String = "[RESISTOR]"
    Private Const FILE_CONST_CUT_DATA As String = "[CUT]"
    Private Const FILE_CONST_PLATE_OPTION As String = "[OPTION]"     'V5.0.0.6�@
    '----- CHIP�p -----
    Private Const FILE_CONST_GRP_DATA As String = "[GROUP]"
    Private Const FILE_CONST_TY2_DATA As String = "[TY2]"
    '----- NET�p -----
    Private Const FILE_CONST_CIR_DATA As String = "[CIRCUIT]"       ' �����\�t�g�łł�TKY�p
    Private Const FILE_CONST_CIRN_DATA As String = "[CIRCUIT AXIS]" ' �����\�t�g�łł�NET�p
    Private Const FILE_CONST_CIRIDATA As String = "[CIRCUIT INTERVAL]"
    '----- ###229�� -----
    '----- TKY/CHIP/NET�p -----
    Private Const FILE_CONST_GPIB_DATA As String = "[GPIB]"       ' GPIB�f�[�^
    '----- ###229�� -----
    Private Const FILE_SINSYUKU_SELECT As String = "[SINSYUKU]"      'V1.13.0.0�D

    '----- �f�[�^��ʒ�`(������) -----
    Private Const SECT_VERSION As Integer = 0                   ' �t�@�C���o�[�W����
    Private Const SECT_PLATE01 As Integer = 1                   ' �v���[�g�f�[�^�P
    Private Const SECT_PLATE02 As Integer = 2                   ' �v���[�g�f�[�^�Q
    Private Const SECT_PLATE03 As Integer = 3                   ' �v���[�g�f�[�^�R
    Private Const SECT_PLATE04 As Integer = 4                   ' �v���[�g�f�[�^�S
    Private Const SECT_PLATE05 As Integer = 5                   ' �v���[�g�f�[�^�T
    Private Const SECT_PLATE06 As Integer = 16                  ' �v���[�g�f�[�^�U V1.13.0.0�A
    Private Const SECT_PLATE_OPTION As Integer = 18             ' �I�v�V���� 'V5.0.0.6�@
    Private Const SECT_CIRCUIT As Integer = 6                   ' �T�[�L�b�g�f�[�^(TKY�p) 
    Private Const SECT_STEP As Integer = 7                      ' STEP�f�[�^(CHIP�p)
    Private Const SECT_GRP_DATA As Integer = 8                  ' �O���[�v�f�[�^(CHIP�p)
    Private Const SECT_TY2_DATA As Integer = 9                  ' TY2�f�[�^(CHIP�p)
    Private Const SECT_CIR_AXIS As Integer = 10                 ' �T�[�L�b�g���W�f�[�^(NET�p)
    Private Const SECT_CIR_ITVL As Integer = 11                 ' �T�[�L�b�g�ԃC���^�[�o���f�[�^(NET�p)
    Private Const SECT_IKEI_DATA As Integer = 12                ' �ٌ`�ʕt��(TKY�p) 
    Private Const SECT_REGISTOR As Integer = 13                 ' ��R�f�[�^
    Private Const SECT_CUT_DATA As Integer = 14                 ' �J�b�g�f�[�^
    Private Const SECT_GPIB_DATA As Integer = 15                ' GPIB�f�[�^ ###229
    Private Const SECT_SINSYUKU_DATA As Integer = 17            ' �L�k�␳�p�f�[�^      'V1.13.0.0�D
#End If
#End Region 'V5.0.0.8�@
    '---------------------------------------------------------------------------
    '   ���̑��̕ϐ���`
    '---------------------------------------------------------------------------
    'Private Const MAX_PDATA As Integer = 100                    ' ۰�ނ�����ڰ��ް�ں��ލő吔'V1.13.0.0�A
    'Private Const MAX_PDATA As Integer = 128                    ' ۰�ނ�����ڰ��ް�ں��ލő吔'V1.13.0.0�A   'V5.0.0.9�A
    Private Const MAX_PGPIBDATA As Integer = 20                 ' ۰�ނ���GPIB�ް�ں��ލő吔 ###229
    'Private pData(100) As String                                ' ۰�ނ�����ڰ��ް����i�['V1.13.0.0�A
    'Private pData(128) As String                                ' ۰�ނ�����ڰ��ް����i�[ 'V1.13.0.0�A       'V5.0.0.9�A
    Private pGpibData(20) As String                             ' ۰�ނ���GPIB�ް����i�[ ###229

#End Region

#Region "�y�t�@�C�����[�h/���C�g�������\�b�h�z"
    '======================================================================
    '  TKY�p���\�b�h
    '======================================================================
#Region "�yTKY�p���\�b�h�z"
#Region "LOAD�ESAVE�̋��ʉ��ɂ�薢�g�p"
#If False Then 'V5.0.0.8�@
#Region "�Â��t�H�[�}�b�g�t�@�C���̃R���o�[�g�����yTKY�p�z"
    '''=========================================================================
    '''<summary>�Â��t�H�[�}�b�g�t�@�C���̃R���o�[�g�����yTKY�p�z</summary>
    '''<param name="sp">(INP) �t�@�C����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function DatConv_TKY(ByVal sp As String) As Integer

        Dim strD As String
        'Dim mPath As String

        DatConv_TKY = 0

        If sp = "" Then
            MsgBox(MSG_15, vbOKOnly, gAppName)
            DatConv_TKY = 1
            Exit Function
        End If

        strD = Right(sp, 3)
        Select Case strD
            Case "WDT", "wdt"
                sp = sp

                '----- V4.0.0.0�F�� -----
                'Case "DAT", "dat"
                '    ' SL436H
                '    mPath = Mid(sp, 1, Len(sp) - 3)
                '    mPath = mPath & "WDT"
                '    DatConv_TKY = Form1.DatConvert.DatConvert(sp, mPath)

                '    If DatConv_TKY Then                     ' �G���[ ? 
                '        Debug.Print("DatConvert error section =" + Form1.DatConvert.ErrorSection)
                '        Debug.Print("DatConvert error item No =" + Form1.DatConvert.ErrorItemNo)
                '        Debug.Print("DatConvert error msg     =" + Form1.DatConvert.ErrorMessage)
                '        Debug.Print("DatConvert error line    =" + Form1.DatConvert.ErrorLine)
                '        MsgBox(Form1.DatConvert.ErrorMessage & " Line=" & CStr(Form1.DatConvert.ErrorLine))
                '        Exit Function
                '    End If
                '    sp = mPath
                '----- V4.0.0.0�F�� -----
        End Select
    End Function
#End Region
#End If
#End Region 'V5.0.0.8�@

#Region "�t�@�C�����[�h�����yTKY�p�z"
    '''=========================================================================
    '''<summary>�t�@�C�����[�h�����yTKY�p�z</summary>
    '''<param name="pPath">(INP) �t�@�C����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function File_Read_Tky(ByVal pPath As String) As Integer

        'Dim intFileNo As Integer                        ' �t�@�C���ԍ�
        Dim strBuff As String                           ' �ǂݍ��݃f�[�^�o�b�t�@
        Dim intType As Integer                          ' �f�[�^���
        Dim Err_num As Integer                          ' �G���[���(1:�ΏۊO�t�@�C���A)
        Dim rCnt As Integer
        'Dim iFlg As Integer

        'On Error GoTo ErrTrap

        ' ��������
        File_Read_Tky = 0
        Err_num = 0
        'iFlg = 0
        Call Init_AllTrmmingData()                      ' �O���[�o���f�[�^������(Plate/Circuit/Resistor/Cut)
        intType = -1
        rCnt = 0

        Try
            ' �e�L�X�g�t�@�C�����I�[�v��
            'intFileNo = FreeFile()                          ' �g�p�\�ȃt�@�C���i���o�[���擾
            If (False = IO.File.Exists(pPath)) Then Throw New FileNotFoundException() 'V4.4.0.0-1
            'FileOpen(intFileNo, pPath, OpenMode.Input)
            'iFlg = 1
            Using sr As New StreamReader(pPath, Encoding.GetEncoding("Shift_JIS"))  ' ��̧�قȂ̂� Shift_JIS    V4.4.0.0-1
                ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ��܂��B
                'Do While Not EOF(intFileNo)
                Do While (False = sr.EndOfStream)               'V4.4.0.0-1

                    'strBuff = LineInput(intFileNo)              ' 1�s�ǂݍ���
                    strBuff = sr.ReadLine()                     ' 1�s�ǂݍ���        'V4.4.0.0-1
                    ' �f�[�^��ʔ���
                    Select Case strBuff
                        Case CONST_VERSION                      ' �t�@�C���o�[�W����
                            intType = 0
                        Case CONST_PLATE                        ' �f�[�^��
                            intType = 1
                        Case CONST_PLATE1                       ' �v���[�g�f�[�^�P
                            intType = 2
                        Case CONST_PLATE2                       ' �v���[�g�f�[�^�Q
                            intType = 3
                        Case CONST_PLATE3                       ' �v���[�g�f�[�^�R
                            intType = 4
                        Case CONST_CIRCUIT                      ' �T�[�L�b�g�f�[�^
                            intType = 5 : rCnt = 1
                        Case CONST_RESISTOR_DATA                ' ��R�f�[�^
                            intType = 6 : rCnt = 1
                        Case CONST_CUT_DATA                     ' �J�b�g�f�[�^
                            intType = 7 : rCnt = 0
                        Case CONST_IKEI_DATA                    ' �ٌ`�ʕt��
                            Err_num = 1
                            GoTo ErrTrap
                        Case Else
                            If Left(strBuff, 2) = CONST_CRC Then
                                'GoTo EndFile
                                Exit Function
                            End If

                            Select Case intType                 ' �f�[�^���
                                Case -1
                                    ' �Ώۃt�@�C���ł͂Ȃ�
                                    Err_num = 1
                                    GoTo ErrTrap
                                Case 0
                                    ' ���@�[�W�����`�F�b�N
                                    Select Case strBuff
                                        Case CONST_FILETYPE4
                                            gStrTkyFileVer = CONST_FILETYPE4

                                        Case CONST_FILETYPE4_SP1
                                            ' �ٌ`�ʕt���͓����Ή��ׁ̈A�Ή����Ȃ�
                                            Err_num = 1
                                            GoTo ErrTrap
                                        Case CONST_FILETYPE5
                                            '----- V1.13.0.0�A�� -----
                                            ' �v���[�u�ڐG�ʒu�m�F�I�v�V�����Ȃ��ł��ǂݍ���
                                            gStrTkyFileVer = CONST_FILETYPE5
                                            'If gSysPrm.stSPF.gblnUseProbePosChk = True Then
                                            '    gStrTkyFileVer = CONST_FILETYPE5
                                            'Else
                                            '    Err_num = 1
                                            '    GoTo ErrTrap
                                            'End If
                                            '----- V1.13.0.0�A�� -----
                                        Case Else
                                            Err_num = 1
                                            GoTo ErrTrap
                                    End Select
                                Case 1 To 4
                                    ' �v���[�g�f�[�^���O���[�o���Z�b�g
                                    If Set_typPlateInfoTky(strBuff, intType) < 0 Then GoTo ErrTrap
                                Case 5
                                    ' �T�[�L�b�g�f�[�^���O���[�o���Z�b�g
                                    If Set_typCircuitInfoArray(strBuff, rCnt) < 0 Then GoTo ErrTrap
                                    rCnt = rCnt + 1
                                Case 6
                                    ' ��R�f�[�^���O���[�o���Z�b�g
                                    If Set_typResistorInfoArray(strBuff, rCnt) < 0 Then GoTo ErrTrap
                                    'typPlateInfo.intResistCntInGroup = rCnt     ' ���݂̒�R�f�[�^�������Z�b�g
                                    typPlateInfo.intResistCntInBlock = rCnt     ' ���݂̒�R�f�[�^�������Z�b�g
                                    gRegistorCnt = rCnt
                                    rCnt = rCnt + 1
                                    System.Windows.Forms.Application.DoEvents()
                                Case 7
                                    ' �J�b�g�f�[�^���O���[�o���Z�b�g
                                    If Set_typCutInfoArray(strBuff, rCnt) < 0 Then GoTo ErrTrap
                                    System.Windows.Forms.Application.DoEvents()
                                Case 8
                                    ' �ٌ`�ʕt���f�[�^���O���[�o���Z�b�g
                                    If Set_typIKEIInfo(strBuff) < 0 Then GoTo ErrTrap

                            End Select
                    End Select
                Loop

            End Using

            ' �I������
EndFile:
            'If (iFlg = 1) Then
            '    FileClose(intFileNo)
            'End If
            'On Error GoTo 0
            Exit Function

            'ErrExit:
            '            File_Read_Tky = 1
            '            Select Case Err_num
            '                Case 1  ' �ΏۊO�t�@�C��
            '                    ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
            '                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, vbExclamation Or vbOKOnly, gAppName)

            '                Case Else
            '                    ' ���b�Z�[�W�ݒ�
            '                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '                    '    MsgBox("##�װ���<DATA LOAD>##" & vbCrLf & "  �װ�ԍ�=" & Str(Err_num))
            '                    'Else
            '                    '    MsgBox("##ERROR INFO<DATA LOAD>##" & vbCrLf & "  Err_nun=" & Str(Err_num))
            '                    'End If
            '                    MsgBox("##" & File_001 & "<DATA LOAD>##" & vbCrLf & "  " & File_002 & "=" & Str(Err_num))
            '            End Select

            'On Error GoTo 0
            'GoTo EndFile

ErrTrap:
            File_Read_Tky = 1
            'Select Case Err_num
            '    Case 1  '�ΏۊO�t�@�C��
            ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, vbExclamation Or vbOKOnly, gAppName)

            '    Case Else
            '        If Err.Number = 53 Then
            '        ElseIf Err.Number <> 0 Then
            '            ' ���b�Z�[�W�ݒ�
            '            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '            '    MsgBox("##�װ���<DATA LOAD>##" & vbCrLf & _
            '            '                               "  �װ���e" & vbTab & " : " & Err.Description & vbCrLf & _
            '            '                               "  �װ�ԍ�" & vbTab & " : " & Err.Number & vbCrLf)
            '            'Else
            '            '    MsgBox("##ERROR INFO<DATA LOAD>##" & vbCrLf & _
            '            '                               "  ERROR DISCRIPTION" & vbTab & " : " & Err.Description & vbCrLf & _
            '            '                               "  ERROR NUMBER     " & vbTab & " : " & Err.Number & vbCrLf)
            '            'End If

            '        End If
            'End Select
            GoTo EndFile

        Catch ex As FileNotFoundException
            File_Read_Tky = 1
            ' "�w�肳�ꂽ�t�@�C���͑��݂��܂���"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_15, vbOKOnly, gAppName)

        Catch ex As Exception
            File_Read_Tky = 1
            MessageBox.Show("##" & File_001 & "<DATA LOAD>##" & vbCrLf & _
                            "  " & File_003 & vbTab & " : " & Err.Description & vbCrLf & _
                            "  " & File_004 & vbTab & " : " & Err.Number & vbCrLf)
        End Try

    End Function
#End Region

#Region "�t�@�C���Z�[�u�����yTKY�p�z"
    '''=========================================================================
    '''<summary>�t�@�C���Z�[�u�����yTKY�p�z</summary>
    '''<param name="pPath">(INP) �t�@�C����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function File_SaveTky(ByVal pPath As String) As Integer

        'Dim intFileNo As Integer                        ' �t�@�C���ԍ�
        Dim i As Integer
        Dim mDATA As String                             ' �ҏW�p������
        Dim j As Integer
        Dim CutNum As Integer                           ' �J�b�g��

        File_SaveTky = 0

        Try
            ' �t�@�C���o�[�W�����Z�b�g
            If gSysPrm.stSPF.gblnUseProbePosChk = True Then
                gStrTkyFileVer = CONST_FILETYPE5
            Else
                gStrTkyFileVer = CONST_FILETYPE4            ' �t�@�C�����@�[�W����
            End If

            If (False = IO.File.Exists(pPath)) Then Throw New FileNotFoundException()
            ' �e�L�X�g�t�@�C�����I�[�v��
            Using sw As New StreamWriter(pPath, False, Encoding.UTF8)           'V4.4.0.0-1
                '[�t�@�C���^�C�g��]-------------
                sw.WriteLine(CONST_VERSION)
                Select Case gStrTkyFileVer
                    Case CONST_FILETYPE4
                        sw.WriteLine(CONST_FILETYPE4)
                    Case CONST_FILETYPE4_SP1
                        sw.WriteLine(CONST_FILETYPE4_SP1)
                    Case CONST_FILETYPE5
                        sw.WriteLine(CONST_FILETYPE5)
                    Case CONST_FILETYPE6
                        sw.WriteLine(CONST_FILETYPE6)
                    Case Else
                        sw.WriteLine("NOT FILE VERSION")
                End Select

                '[�v���[�g�f�[�^]-------------
                sw.WriteLine(CONST_PLATE)
                sw.WriteLine(typPlateInfo.strDataName)  ' �f�[�^��
                '[�v���[�g�P]
                sw.WriteLine(CONST_PLATE1)
                sw.WriteLine(Get_PInfoArray1)           ' �g�������[�h�`'��ެ�Ĉʒu�̾�Ăx
                '[�v���[�g�Q]
                sw.WriteLine(CONST_PLATE2)
                sw.WriteLine(Get_PInfoArray2)           ' �T�[�L�b�g���`'�A�b�e�l�[�^(%)
                '[�v���[�g�R]
                sw.WriteLine(CONST_PLATE3)
                sw.WriteLine(Get_PInfoArray3)           ' �␳���[�h�`''��ٰ�߂m��

                '[�T�[�L�b�g]-------------
                sw.WriteLine(CONST_CIRCUIT)
                mDATA = ""
                If typPlateInfo.intNGMark <> 0 Then

                    For i = 1 To typPlateInfo.intCurcuitCnt
                        With typCircuitInfoArray(i)
                            mDATA = Right(Space(3) & .intIP1, 3) & ","
                            mDATA = mDATA & Right(Space(8) & .dblIP2X.ToString("0.0000"), 8) & ","
                            mDATA = mDATA & Right(Space(8) & .dblIP2Y.ToString("0.0000"), 8)
                        End With
                        sw.WriteLine(mDATA)             ' IP�ԍ�,�}�[�L���OX,�}�[�L���OY
                    Next
                End If

                '[��R]--------------------
                sw.WriteLine(CONST_RESISTOR_DATA)
                '''' 2009/07/20 minato
                '''' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���ׁ̈A1�I���W���ɍ��킹��B
                For i = 1 To gRegistorCnt
                    mDATA = Get_RInfoArray(i)
                    sw.WriteLine(mDATA)                 ' ��R�f�[�^1�s
                Next

                '[�J�b�g]------------------
                sw.WriteLine(CONST_CUT_DATA)
                '''' 2009/07/20 minato
                '''' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���ׁ̈A1�I���W���ɍ��킹��B
                For i = 1 To gRegistorCnt
                    CutNum = typResistorInfoArray(i).intCutCount
                    For j = 1 To CutNum
                        mDATA = Get_CInfoArray(j, i)
                        sw.WriteLine(mDATA)             ' �J�b�g�f�[�^1�s
                    Next
                Next

                '[CRC]------------------
                sw.WriteLine("C=")

            End Using

        Catch ex As Exception
            File_SaveTky = 1
            MessageBox.Show("##" & File_001 & "<DATA SAVE>##" & vbCrLf & _
                               "  " & File_003 & vbTab & " : " & Err.Description & vbCrLf & _
                               "  " & File_004 & vbTab & " : " & Err.Number & vbCrLf)
        End Try

#If False Then
        On Error GoTo ErrTrap

        File_SaveTky = 0

        ' �t�@�C���o�[�W�����Z�b�g
        If gSysPrm.stSPF.gblnUseProbePosChk = True Then
            gStrTkyFileVer = CONST_FILETYPE5
        Else
            gStrTkyFileVer = CONST_FILETYPE4            ' �t�@�C�����@�[�W����
        End If

        ' �g�p�\�ȃt�@�C���i���o�[���擾
        intFileNo = FreeFile()
        ' �e�L�X�g�t�@�C�����I�[�v��
        FileOpen(intFileNo, pPath, OpenMode.Output)

        '[�t�@�C���^�C�g��]-------------
        PrintLine(intFileNo, CONST_VERSION)
        Select Case gStrTkyFileVer
            Case CONST_FILETYPE4
                PrintLine(intFileNo, CONST_FILETYPE4)
            Case CONST_FILETYPE4_SP1
                PrintLine(intFileNo, CONST_FILETYPE4_SP1)
            Case CONST_FILETYPE5
                PrintLine(intFileNo, CONST_FILETYPE5)
            Case CONST_FILETYPE6
                PrintLine(intFileNo, CONST_FILETYPE6)
            Case Else
                PrintLine(intFileNo, "NOT FILE VERSION")
        End Select

        '[�v���[�g�f�[�^]-------------
        PrintLine(intFileNo, CONST_PLATE)
        PrintLine(intFileNo, typPlateInfo.strDataName)  ' �f�[�^��
        '[�v���[�g�P]
        PrintLine(intFileNo, CONST_PLATE1)
        PrintLine(intFileNo, Get_PInfoArray1)           ' �g�������[�h�`'��ެ�Ĉʒu�̾�Ăx
        '[�v���[�g�Q]
        PrintLine(intFileNo, CONST_PLATE2)
        PrintLine(intFileNo, Get_PInfoArray2)           ' �T�[�L�b�g���`'�A�b�e�l�[�^(%)
        '[�v���[�g�R]
        PrintLine(intFileNo, CONST_PLATE3)
        PrintLine(intFileNo, Get_PInfoArray3)           ' �␳���[�h�`''��ٰ�߂m��

        '[�T�[�L�b�g]-------------
        PrintLine(intFileNo, CONST_CIRCUIT)
        mDATA = ""
        If typPlateInfo.intNGMark <> 0 Then

            For i = 1 To typPlateInfo.intCurcuitCnt
                With typCircuitInfoArray(i)
                    mDATA = Right(Space(3) & .intIP1, 3) & ","
                    mDATA = mDATA & Right(Space(8) & .dblIP2X.ToString("0.0000"), 8) & ","
                    mDATA = mDATA & Right(Space(8) & .dblIP2Y.ToString("0.0000"), 8)
                End With
                PrintLine(intFileNo, mDATA)             ' IP�ԍ�,�}�[�L���OX,�}�[�L���OY
            Next
        End If

        '[��R]--------------------
        PrintLine(intFileNo, CONST_RESISTOR_DATA)
        '''' 2009/07/20 minato
        '''' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���ׁ̈A1�I���W���ɍ��킹��B
        For i = 1 To gRegistorCnt
            mDATA = Get_RInfoArray(i)
            PrintLine(intFileNo, mDATA)                 ' ��R�f�[�^1�s
        Next

        '[�J�b�g]------------------
        PrintLine(intFileNo, CONST_CUT_DATA)
        '''' 2009/07/20 minato
        '''' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���ׁ̈A1�I���W���ɍ��킹��B
        For i = 1 To gRegistorCnt
            CutNum = typResistorInfoArray(i).intCutCount
            For j = 1 To CutNum
                mDATA = Get_CInfoArray(j, i)
                PrintLine(intFileNo, mDATA)             ' �J�b�g�f�[�^1�s
            Next
        Next

        '[CRC]------------------
        PrintLine(intFileNo, "C=")

        FileClose(intFileNo)
        Exit Function

ErrTrap:
        File_SaveTky = 1
        If Err.Number <> 0 Then
            ' ���b�Z�[�W�ݒ�
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    MsgBox("##�װ���<DATA SAVE>##" & vbCrLf & _
            '                       "  �װ�ڍ�" & vbTab & " : " & Err.Description & vbCrLf & _
            '                       "  �װ�ԍ�" & vbTab & " : " & Err.Number & vbCrLf)
            'Else
            '    MsgBox("##ERROR INFO<DATA SAVE>##" & vbCrLf & _
            '                       "  ERROR DISCRIPTION" & vbTab & " : " & Err.Description & vbCrLf & _
            '                       "  ERROR NUMBER     " & vbTab & " : " & Err.Number & vbCrLf)
            'End If
            MsgBox("##" & File_001 & "<DATA SAVE>##" & vbCrLf & _
                               "  " & File_003 & vbTab & " : " & Err.Description & vbCrLf & _
                               "  " & File_004 & vbTab & " : " & Err.Number & vbCrLf)
        End If
#End If
    End Function

#End Region

#Region "���[�h�����v���[�g�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z"
    '''=========================================================================
    '''<summary>���[�h�����v���[�g�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z</summary>
    '''<param name="pBuff">(INP) ���[�h�f�[�^</param>
    '''<param name="pType">(INP) �f�[�^���</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typPlateInfoTky(ByVal pBuff As String, ByVal pType As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim flg As Integer

        On Error GoTo ErrTrap
        Set_typPlateInfoTky = 0
        flg = 0

        With typPlateInfo
            Select Case pType
                Case 1  ' �f�[�^��
                    .strDataName = pBuff

                Case 2  ' �v���[�g�f�[�^�P
                    i = 0
                    ' ���[�h�f�[�^��z��Z�b�g
                    mDATA = pBuff.Split(",")

                    .intMeasType = CInt(mDATA(i)) : i = i + 1                        '�g�������[�h
                    .intDirStepRepeat = CInt(mDATA(i)) : i = i + 1                   '�ï�߁���߰�
                    .intPlateCntXDir = CInt(mDATA(i)) : i = i + 1                    '�v���[�g��X
                    .intPlateCntYDir = CInt(mDATA(i)) : i = i + 1                    '�v���[�g��Y
                    .intBlockCntXDir = CInt(mDATA(i)) : i = i + 1                    '��ۯ���X
                    .intBlockCntYDir = CInt(mDATA(i)) : i = i + 1                    '��ۯ���Y
                    .dblPlateItvXDir = CDbl(mDATA(i)) : i = i + 1                    '��ڰĊԊu�w
                    .dblPlateItvYDir = CDbl(mDATA(i)) : i = i + 1                    '��ڰĊԊu�x
                    .dblBlockSizeXDir = CDbl(mDATA(i)) : i = i + 1                   '��ۯ����ނw
                    .dblBlockSizeYDir = CDbl(mDATA(i)) : i = i + 1                   '��ۯ����ނx
                    .dblTableOffsetXDir = CDbl(mDATA(i)) : i = i + 1                 'ð��وʒu�̾��X
                    .dblTableOffsetYDir = CDbl(mDATA(i)) : i = i + 1                 'ð��وʒu�̾��Y
                    .dblBpOffSetXDir = CDbl(mDATA(i)) : i = i + 1                    '�ްшʒu�̾��X
                    .dblBpOffSetYDir = CDbl(mDATA(i)) : i = i + 1                    '�ްшʒu�̾��Y
                    .dblAdjOffSetXDir = CDbl(mDATA(i)) : i = i + 1                   '��ެ�Ĉʒu�̾��X
                    .dblAdjOffSetYDir = CDbl(mDATA(i)) : i = i + 1                   '��ެ�Ĉʒu�̾��Y

                Case 3  '�v���[�g�f�[�^�Q
                    i = 0
                    mDATA = pBuff.Split(",")
                    .intCurcuitCnt = CInt(mDATA(i)) : i = i + 1                      '�T�[�L�b�g��
                    .intNGMark = CInt(mDATA(i)) : i = i + 1                          '�}�[�L���O
                    .intDelayTrim = CInt(mDATA(i)) : i = i + 1                       '�ިڲ���
                    .intNgJudgeUnit = CInt(mDATA(i)) : i = i + 1                     '�m�f����P��
                    .intNgJudgeLevel = CInt(mDATA(i)) : i = i + 1                    '�m�f����
                    .dblZOffSet = CDbl(mDATA(i)) : i = i + 1                         '��۰�ނy�̾��
                    .dblZStepUpDist = CDbl(mDATA(i)) : i = i + 1                     '��۰�޽ï�ߏ㏸����
                    .dblZWaitOffset = CDbl(mDATA(i)) : i = i + 1                     '��۰�ޑҋ@�y�̾��
                    .intFinalJudge = CInt(0) : i = i + 1                             '�t�@�C�i������
                    .intAttLevel = CInt(0) : i = i + 1                               '�A�b�e�l�[�^(%)
                    '----- V1.22.0.0�A�� -----
                    ' Z ON/OFF�ʒu���Đݒ肷��
                    .dblZOffSet = 5.0                                               'ZON�ʒu
                    .dblZStepUpDist = 3.0                                           '��۰�޽ï�ߏ㏸����
                    .dblZWaitOffset = 1.0                                           'ZOFF�ʒu
                    '----- V1.22.0.0�A�� -----

                Case 4  '�v���[�g�f�[�^�R
                    i = 0
                    mDATA = pBuff.Split(",")
                    .intReviseMode = CInt(mDATA(i)) : i = i + 1                      '�␳���[�h
                    .intManualReviseType = CInt(mDATA(i)) : i = i + 1                '�␳���@
                    .dblReviseCordnt1XDir = CDbl(mDATA(i)) : i = i + 1               '�␳�ʒu���W1X
                    .dblReviseCordnt1YDir = CDbl(mDATA(i)) : i = i + 1               '�␳�ʒu���W1Y
                    .dblReviseCordnt2XDir = CDbl(mDATA(i)) : i = i + 1               '�␳�ʒu���W2X
                    .dblReviseCordnt2YDir = CDbl(mDATA(i)) : i = i + 1               '�␳�ʒu���W2Y
                    .dblReviseOffsetXDir = CDbl(mDATA(i)) : i = i + 1                '�␳�߼޼�ݵ̾��X
                    .dblReviseOffsetYDir = CDbl(mDATA(i)) : i = i + 1                '�␳�߼޼�ݵ̾��Y
                    .intRecogDispMode = CInt(mDATA(i)) : i = i + 1                   '�F���f�[�^�\�����[�h
                    If gSysPrm.stDEV.giEXCAM = 1 Then
                        .dblPixelValXDir = gSysPrm.stGRV.gfEXCAM_PixelX : i = i + 1  '�߸�ْlX
                        .dblPixelValYDir = gSysPrm.stGRV.gfEXCAM_PixelY : i = i + 1  '�߸�ْlY
                    Else
                        .dblPixelValXDir = gSysPrm.stGRV.gfPixelX : i = i + 1        '�߸�ْlX
                        .dblPixelValYDir = gSysPrm.stGRV.gfPixelY : i = i + 1        '�߸�ْlY
                    End If
                    .intRevisePtnNo1 = CInt(mDATA(i)) : i = i + 1                    '�␳�ʒu����݂m�n1
                    .intRevisePtnNo2 = CInt(mDATA(i)) : i = i + 1                    '�␳�ʒu����݂m�n2
                    ' �t�@�C���o�[�W�����ɂ��f�[�^�̐ݒ菈����ύX
                    ' Ver5�ȑO�̃t�@�C���̏ꍇ
                    If gStrTkyFileVer <= CONST_FILETYPE5 Then
                        .intRevisePtnNo1GroupNo = CInt(mDATA(i))                    '�␳�ʒu����݂m�n1��ٰ�߂m��
                        .intRevisePtnNo2GroupNo = CInt(mDATA(i))                    '�␳�ʒu����݂m�n2��ٰ�߂m��
                        .intCutPosiReviseGroupNo = CInt(mDATA(i)) : i = i + 1        '��ٰ�߂m��
                    Else
                        .intRevisePtnNo1GroupNo = CInt(mDATA(i)) : i = i + 1         '�␳�ʒu����݂m�n1��ٰ�߂m��
                        .intRevisePtnNo2GroupNo = CInt(mDATA(i)) : i = i + 1         '�␳�ʒu����݂m�n2��ٰ�߂m��
                        .intCutPosiReviseGroupNo = CInt(mDATA(i)) : i = i + 1        '��ٰ�߂m��
                    End If
                    flg = 1
                    .dblRotateTheta = CDbl(mDATA(i)) : i = i + 1                    '�Ɖ�]�p�x

            End Select
        End With
        Exit Function

ErrTrap:
        If flg = 1 Then Exit Function ' �Ɖ�]�̑O�܂Őݒ�Ȃ琳������
        Set_typPlateInfoTky = -1

    End Function
#End Region

#Region "���[�h������R�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z"
    '''=========================================================================
    '''<summary>���[�h������R�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z</summary>
    '''<param name="pBuff">(INP) ���[�h�f�[�^</param>
    '''<param name="pCnt"> (INP) �z��</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typResistorInfoArray(ByVal pBuff As String, ByVal pCnt As Integer) As Integer

        Dim mDATA As Object
        Dim i As Integer

        On Error GoTo ErrTrap
        Set_typResistorInfoArray = 0

        '''' 2009/07/20 minato
        '''' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���ׁ̈A1�I���W���ɍ��킹��B
        '    With typResistorInfoArray(pCnt - 1)
        With typResistorInfoArray(pCnt)
            i = 0
            ' ���[�h�f�[�^��z��Z�b�g
            mDATA = pBuff.Split(",")

            .intResNo = CInt(mDATA(i)) : i = i + 1          ' ��R�ԍ�
            .intResMeasMode = typPlateInfo.intMeasType      ' �g�������[�h(0:��R ,1:�d��) ���ύX�v���[�g�f�[�^���ݒ�
            .intResMeasType = CInt(mDATA(i)) : i = i + 1    ' ����^�C�v(0:���� ,1:�����x)���ǉ�
            .intCircuitGrp = CInt(mDATA(i)) : i = i + 1     ' �����T�[�L�b�g

            If (.intResNo >= 1000 And .intCircuitGrp = 0) Then
                .intCircuitGrp = 1
            End If

            '----- V6.1.4.0_23�� -----
            ' ��R�ԍ���1000�ȍ~�̏ꍇ�ɂ́A�ڕW�l�̓f�t�H���g��1��ݒ肷��
            '.intProbHiNo = CInt(mDATA(i)) : i = i + 1       ' �v���[�u�ԍ��i�n�C���j
            '.intProbLoNo = CInt(mDATA(i)) : i = i + 1       ' �v���[�u�ԍ��i���[���j
            If .intResNo < 1000 Then
                .intProbHiNo = CInt(mDATA(i)) : i = i + 1   ' �v���[�u�ԍ��i�n�C���j
                .intProbLoNo = CInt(mDATA(i)) : i = i + 1   ' �v���[�u�ԍ��i���[���j
            Else
                .intProbHiNo = 1 : i = i + 1                ' �v���[�u�ԍ��i�n�C���j
                .intProbLoNo = 2 : i = i + 1                ' �v���[�u�ԍ��i���[���j
            End If
            '----- V6.1.4.0_23�� -----
            .intProbAGNo1 = CInt(mDATA(i)) : i = i + 1      ' �v���[�u�ԍ��i�P�j
            .intProbAGNo2 = CInt(mDATA(i)) : i = i + 1      ' �v���[�u�ԍ��i�Q�j
            .intProbAGNo3 = CInt(mDATA(i)) : i = i + 1      ' �v���[�u�ԍ��i�R�j
            .intProbAGNo4 = CInt(mDATA(i)) : i = i + 1      ' �v���[�u�ԍ��i�S�j
            .intProbAGNo5 = CInt(mDATA(i)) : i = i + 1      ' �v���[�u�ԍ��i�T�j
            .strExternalBits = CLng(mDATA(i)) : i = i + 1   ' EXTERNAL BITS
            .intPauseTime = CInt(mDATA(i)) : i = i + 1      ' �|�[�Y�^�C��
            .intTargetValType = CInt(mDATA(i)) : i = i + 1  ' �g�������[�h
            .intBaseResNo = CInt(mDATA(i)) : i = i + 1      ' �x�|�X��R�ԍ�
            '----- V6.1.4.0_23�� -----
            ' ��R�ԍ���1000�ȍ~�̏ꍇ�ɂ́A�ڕW�l�̓f�t�H���g��1��ݒ肷��
            '.dblTrimTargetVal = CDbl(mDATA(i)) : i = i + 1  ' �g���~���O�ڕW�l
            If .intResNo < 1000 Then
                .dblTrimTargetVal = CDbl(mDATA(i)) : i = i + 1 ' �g���~���O�ڕW�l
            Else
                .dblTrimTargetVal = 1.0# : i = i + 1        ' �g���~���O�ڕW�l
            End If
            '----- V6.1.4.0_23�� -----

            ' �t�@�C���o�[�W�����ɂ���ď����킯
            Select Case gStrTkyFileVer
                Case CONST_FILETYPE5
                    .dblProbCfmPoint_Hi_X = CDbl(mDATA(i)) : i = i + 1  ' �v���[�u�m�F�ʒu HI X���W
                    .dblProbCfmPoint_Hi_Y = CDbl(mDATA(i)) : i = i + 1  ' �v���[�u�m�F�ʒu HI Y���W
                    .dblProbCfmPoint_Lo_X = CDbl(mDATA(i)) : i = i + 1  ' �v���[�u�m�F�ʒu LO X���W
                    .dblProbCfmPoint_Lo_Y = CDbl(mDATA(i)) : i = i + 1  ' �v���[�u�m�F�ʒu LO Y���W
                Case Else
                    ' �f�t�H���g�l���
                    .dblProbCfmPoint_Hi_X = 0                           ' �v���[�u�m�F�ʒu HI X���W
                    .dblProbCfmPoint_Hi_Y = 0                           ' �v���[�u�m�F�ʒu HI Y���W
                    .dblProbCfmPoint_Lo_X = 0                           ' �v���[�u�m�F�ʒu LO X���W
                    .dblProbCfmPoint_Lo_Y = 0                           ' �v���[�u�m�F�ʒu LO Y���W
            End Select

            .intSlope = CInt(mDATA(i)) : i = i + 1                  ' �d���ω� �۰��
            .dblInitTest_HighLimit = CDbl(mDATA(i)) : i = i + 1     ' �C�j�V�����e�X�gHIGH���~�b�g
            .dblInitTest_LowLimit = CDbl(mDATA(i)) : i = i + 1      ' �C�j�V�����e�X�gLOW���~�b�g
            .dblFinalTest_HighLimit = CDbl(mDATA(i)) : i = i + 1    ' �t�@�C�i���e�X�gHIGH���~�b�g
            .dblFinalTest_LowLimit = CDbl(mDATA(i)) : i = i + 1     ' �t�@�C�i���e�X�gLOW���~�b�g
            .intCutReviseMode = CInt(mDATA(i)) : i = i + 1          ' ��� �␳
            .intCutReviseDispMode = CInt(mDATA(i)) : i = i + 1      ' �\��Ӱ��
            .intCutRevisePtnNo = CInt(mDATA(i)) : i = i + 1         ' ����� No.
            .dblCutRevisePosX = CDbl(mDATA(i)) : i = i + 1          ' ��ĕ␳�ʒuX
            .dblCutRevisePosY = CDbl(mDATA(i)) : i = i + 1          ' ��ĕ␳�ʒuY
            .intIsNG = CInt(mDATA(i)) : i = i + 1                   ' NG�L��
            .intCutCount = CInt(mDATA(i)) : i = i + 1               ' �J�b�g��
            .strRatioTrimTargetVal = CStr(mDATA(i)) : i = i + 1     ' �g���~���O�ڕW�l�v�Z�l

        End With

        Exit Function

ErrTrap:
        Set_typResistorInfoArray = -1
    End Function
#End Region

#Region "���[�h�����J�b�g�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z"
    '''=========================================================================
    '''<summary>���[�h�����J�b�g�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z</summary>
    '''<param name="pBuff">(INP) ���[�h�f�[�^</param>
    '''<param name="pCnt"> (I/O) ��R�f�[�^�z��</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typCutInfoArray(ByVal pBuff As String, ByRef pCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim j As Integer
        Dim mCTNum As Integer                           ' �J�b�g��
        On Error GoTo ErrTrap
        Set_typCutInfoArray = 0

        i = 0
        ' ���[�h�f�[�^��z��Z�b�g
        mDATA = pBuff.Split(",")
        mCTNum = CInt(mDATA(1))                         ' �J�b�g�ԍ��擾
        If mCTNum = 1 Then                              ' �J�b�g�ԍ����P�̏ꍇ�͒�R�f�[�^�z��ԍ����J�E���g�A�b�v
            pCnt = pCnt + 1
        End If

        ' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���̈�1�I���W���ɍ��킹��B
        'With typCutInfoArray(pCnt)
        With typResistorInfoArray(pCnt)
            '.intCRNO = CInt(mDATA(i)) : i = i + 1                       ' ��R�ԍ�
            i = i + 1                                                   ' ��R�ԍ�

            .ArrCut(mCTNum).intCutNo = CInt(mDATA(i)) : i = i + 1       ' �J�b�g�ԍ�
            .ArrCut(mCTNum).intDelayTime = CInt(mDATA(i)) : i = i + 1   ' �f�B���C�^�C��
            .ArrCut(mCTNum).dblStartPointX = CDbl(mDATA(i)) : i = i + 1 ' �X�^�[�g�|�C���gX
            .ArrCut(mCTNum).dblStartPointY = CDbl(mDATA(i)) : i = i + 1 ' �X�^�[�g�|�C���gY
            .ArrCut(mCTNum).dblCutSpeed = CDbl(mDATA(i)) : i = i + 1    ' �J�b�g�X�s�[�h
            .ArrCut(mCTNum).dblQRate = CDbl(mDATA(i)) : i = i + 1       ' �p�X�C�b�`���[�g
            .ArrCut(mCTNum).dblCutOff = CDbl(mDATA(i)) : i = i + 1      ' �J�b�g�I�t�l
            .ArrCut(mCTNum).dblJudgeLevel = CDbl(mDATA(i)) : i = i + 1  ' �ؑփ|�C���g (���ް�����(���ω���))
            .ArrCut(mCTNum).dblJudgeLevel = 0.0                             ' �ؑփ|�C���g�͏��������� 'V1.22.0.0�A
            .ArrCut(mCTNum).strCutType = Trim(CStr(mDATA(i))) : i = i + 1   ' �J�b�g�`�� 9
            .ArrCut(mCTNum).intCutDir = CInt(mDATA(i)) : i = i + 1          ' �J�b�g���� 10
            .ArrCut(mCTNum).dblMaxCutLength = CDbl(mDATA(i)) : i = i + 1    ' �ő�J�b�e�B���O�� 11
            .ArrCut(mCTNum).dblR1 = CDbl(mDATA(i)) : i = i + 1              ' �q�P 12
            .ArrCut(mCTNum).dblLTurnPoint = CDbl(mDATA(i)) : i = i + 1      ' �k�^�[���|�C���g 13
            .ArrCut(mCTNum).dblMaxCutLengthL = CDbl(mDATA(i)) : i = i + 1   ' �k�^�[����̍ő�J�b�e�B���O�� 14
            .ArrCut(mCTNum).dblR2 = CDbl(mDATA(i)) : i = i + 1              ' �q�Q 15
            .ArrCut(mCTNum).dblMaxCutLengthHook = CDbl(mDATA(i)) : i = i + 1 ' �t�b�N�^�[����̃J�b�e�B���O�� 16
            .ArrCut(mCTNum).intIndexCnt = CInt(mDATA(i)) : i = i + 1        ' �C���f�b�N�X�� 17
            .ArrCut(mCTNum).intMeasMode = CInt(mDATA(i)) : i = i + 1        ' ���胂�[�h 18
            .ArrCut(mCTNum).dblCutSpeed2 = CDbl(mDATA(i)) : i = i + 1       ' �J�b�g�X�s�[�h�Q 19
            '----- V1.13.0.0�A�� -----
            If (.ArrCut(mCTNum).dblCutSpeed2 = 0) Then                      ' �J�b�g�X�s�[�h2�ɃJ�b�g�X�s�[�h��ݒ肷��
                .ArrCut(mCTNum).dblCutSpeed2 = .ArrCut(mCTNum).dblCutSpeed
            End If
            '----- V1.13.0.0�A�� -----
            .ArrCut(mCTNum).dblQRate2 = CDbl(mDATA(i)) : i = i + 1          ' �p�X�C�b�`���[�g�Q 20
            '----- V1.13.0.0�A�� -----
            If (.ArrCut(mCTNum).dblQRate2 = 0) Then                         ' �p�X�C�b�`���[�g�Q�ɂp�X�C�b�`���[�g��ݒ肷��
                .ArrCut(mCTNum).dblQRate2 = .ArrCut(mCTNum).dblQRate
            End If
            '----- V1.13.0.0�A�� -----
            .ArrCut(mCTNum).intCutAngle = CInt(mDATA(i)) : i = i + 1        ' �΂߃J�b�g�̐؂�o���p�x 21
            .ArrCut(mCTNum).dblPitch = CDbl(mDATA(i)) : i = i + 1           ' �s�b�` 22
            .ArrCut(mCTNum).intStepDir = CInt(mDATA(i)) : i = i + 1         ' �X�e�b�v���� 23
            .ArrCut(mCTNum).intCutCnt = CInt(mDATA(i)) : i = i + 1          ' �{�� 24
            .ArrCut(mCTNum).dblZoom = CDbl(mDATA(i)) : i = i + 1            ' �{�� 25
            .ArrCut(mCTNum).strChar = CStr(mDATA(i)) : i = i + 1            ' ������ 26
            .ArrCut(mCTNum).strChar = HexAsc2Str(.ArrCut(mCTNum).strChar)

            '----- V1.13.0.0�A�� -----
            If (.ArrCut(mCTNum).strCutType = CNS_CUTP_NOP) Then             ' �J�b�g�`�� = Z(NOP)�Ȃ� 
                .ArrCut(mCTNum).strCutType = CNS_CUTP_NST                   ' �΂�ST�J�b�g�ɕύX
                .ArrCut(mCTNum).dblMaxCutLength = 0.0                       ' �J�b�g��  = 0 
            End If
            '----- V1.13.0.0�A�� -----

            ' �J�b�g��������΂߃J�b�g�̐؂�o���p�x��ݒ肷��
            Call GetCutAngle(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).intCutDir, .ArrCut(mCTNum).intCutAngle)
            ' �J�b�g��������΂߃J�b�g�̐؂�o���p�x��L�^�[��������ݒ肷��(L�J�b�g/HOOK�J�b�g�p)
            Call GetCutLTurnDir(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).intCutDir, .ArrCut(mCTNum).intCutAngle, .ArrCut(mCTNum).intLTurnDir)
            ' �X�e�b�v������ϊ�����(�X�L�����J�b�g�p)
            Call GetStepDir(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).intStepDir, .ArrCut(mCTNum).intStepDir)
            '----- V1.13.0.0�A�� -----
            ' ST�J�b�g��L�J�b�g�͎΂�ST�J�b�g, �΂�L�J�b�g�ɕϊ�����
            Call CnvCutType(.ArrCut(mCTNum).strCutType, .ArrCut(mCTNum).strCutType)
            '----- V1.13.0.0�A�� -----

            ' �ڕW�p���[�Ƌ��e�͈�(�f�t�H���g�l��ݒ肷��) ###066
            'For j = 0 To (cCNDNUM - 1)                                          ' ���H�����ԍ�1�`n(0�ؼ��)
            For j = 0 To (MaxCndNum - 1)                                        ' ���H�����ԍ�1�`n(0�ؼ��) 'V5.0.0.8�@
                'V6.0.0.1�E                .ArrCut(mCTNum).dblPowerAdjustTarget(j) = POWERADJUST_TARGET   ' �ڕW�p���[(W)
                'V6.0.0.1�E                .ArrCut(mCTNum).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL  ' ���e�͈�(�}W)
                .ArrCut(mCTNum).dblPowerAdjustTarget(j) = DEFAULT_ADJUST_TAERGET   ' �ڕW�p���[(W)   'V6.0.0.1�E
                .ArrCut(mCTNum).dblPowerAdjustToleLevel(j) = DEFAULT_ADJUST_LEVEL  ' ���e�͈�(�}W)    'V6.0.0.1�E
            Next j

        End With

        Exit Function

ErrTrap:
        Set_typCutInfoArray = -1

    End Function

#End Region

#Region "���[�h�����T�[�L�b�g�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z"
    '''=========================================================================
    '''<summary>���[�h�����T�[�L�b�g�f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z</summary>
    '''<param name="pBuff">(INP) ���[�h�f�[�^</param>
    '''<param name="pCnt"> (INP) �z��</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typCircuitInfoArray(ByVal pBuff As String, ByVal pCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer

        On Error GoTo ErrTrap
        Set_typCircuitInfoArray = 0

        '''''2009/07/20 minato
        '''' TKY�ł́A0�I���W�������ACHIP�n��1�I���W���ׁ̈A1�I���W���ɍ��킹��B
        ''''    (���j�T�[�L�b�g�����Ȃ���1�I���W��
        With typCircuitInfoArray(pCnt)
            i = 0
            ' ���[�h�f�[�^��z��Z�b�g
            mDATA = pBuff.Split(",")

            .intIP1 = CInt(mDATA(i)) : i = i + 1        ' IP�ԍ�
            .dblIP2X = CDbl(mDATA(i)) : i = i + 1       ' �}�[�L���OX
            .dblIP2Y = CDbl(mDATA(i)) : i = i + 1       ' �}�[�L���OY
        End With

        Exit Function

ErrTrap:
        Set_typCircuitInfoArray = -1
    End Function
#End Region

#Region "���[�h�����ٌ`�ʕt���f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z"
    '''=========================================================================
    '''<summary>���[�h�����ٌ`�ʕt���f�[�^���O���[�o���ϐ��֊i�[����yTKY�p�z</summary>
    '''<param name="pBuff">(INP) ���[�h�f�[�^</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typIKEIInfo(ByVal pBuff As String) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim j As Integer
        Dim n As Integer

        Set_typIKEIInfo = 0

        i = 0
        ' ���[�h�f�[�^��z��Z�b�g
        mDATA = pBuff.Split(",")

        With typIKEIInfo
            .intI1 = CInt(mDATA(i)) : i = i + 1             ' �ٌ`�ʕt���̗L��
            n = CInt(mDATA(i)) : i = i + 1                  ' �T�[�L�b�g��
            If n > MaxCntCircuit Then n = MaxCntCircuit
            For j = 0 To n - 1
                .intI2(j) = CInt(mDATA(i)) : i = i + 1
            Next j
        End With

        Exit Function

ErrTrap:
        Set_typIKEIInfo = -1

    End Function

#End Region

#Region "�O���[�o���f�[�^����v���[�g�f�[�^1���擾����yTKY�p�z"
    '''=========================================================================
    '''<summary>�O���[�o���f�[�^����v���[�g�f�[�^1���擾����yTKY�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_PInfoArray1() As String

        Get_PInfoArray1 = ""

        With typPlateInfo
            '[�v���[�g�P]
            Get_PInfoArray1 = CStr(.intMeasType) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intDirStepRepeat) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intPlateCntXDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intPlateCntYDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intBlockCntXDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & CStr(.intBlockCntYDir) & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblPlateItvXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblPlateItvYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBlockSizeXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBlockSizeYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblTableOffsetXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblTableOffsetYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBpOffSetXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblBpOffSetYDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblAdjOffSetXDir.ToString("0.0000") & ","
            Get_PInfoArray1 = Get_PInfoArray1 & .dblAdjOffSetYDir.ToString("0.0000")
        End With


    End Function
#End Region

#Region "�O���[�o���f�[�^����v���[�g�f�[�^2���擾����yTKY�p�z"
    '''=========================================================================
    '''<summary>�O���[�o���f�[�^����v���[�g�f�[�^2���擾����yTKY�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_PInfoArray2() As String

        Get_PInfoArray2 = ""

        With typPlateInfo
            '[�v���[�g�P]
            Get_PInfoArray2 = CStr(.intCurcuitCnt) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intNGMark) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intDelayTrim) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intNgJudgeUnit) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(.intNgJudgeLevel) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & .dblZOffSet.ToString("0.0000") & ","
            Get_PInfoArray2 = Get_PInfoArray2 & .dblZStepUpDist.ToString("0.0000") & ","
            Get_PInfoArray2 = Get_PInfoArray2 & .dblZWaitOffset.ToString("0.0000") & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(0) & ","
            Get_PInfoArray2 = Get_PInfoArray2 & CStr(0)
        End With


    End Function
#End Region

#Region "�O���[�o���f�[�^����v���[�g�f�[�^3���擾����yTKY�p�z"
    '''=========================================================================
    '''<summary>�O���[�o���f�[�^����v���[�g�f�[�^3���擾����yTKY�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_PInfoArray3() As String

        Get_PInfoArray3 = ""

        With typPlateInfo
            '[�v���[�g�P]
            Get_PInfoArray3 = CStr(.intReviseMode) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intManualReviseType) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt1XDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt1YDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt2XDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseCordnt2YDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseOffsetXDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblReviseOffsetYDir.ToString("0.0000") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRecogDispMode) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblPixelValXDir.ToString("0.00") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & .dblPixelValYDir.ToString("0.00") & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo1) & ","
            Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo2) & ","
            '�t�@�C���^�C�v�ɂ��
            If gStrTkyFileVer <= CONST_FILETYPE5 Then
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intCutPosiReviseGroupNo) & ","
            Else
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo1GroupNo) & ","
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intRevisePtnNo2GroupNo) & ","
                Get_PInfoArray3 = Get_PInfoArray3 & CStr(.intCutPosiReviseGroupNo) & ","
            End If
            Get_PInfoArray3 = Get_PInfoArray3 & .dblRotateTheta.ToString("0.00000") '�Ǝ��p�x

        End With

    End Function
#End Region

#Region "�O���[�o���f�[�^�����R�f�[�^���擾����yTKY�p�z"
    '''=========================================================================
    '''<summary>�O���[�o���f�[�^�����R�f�[�^���擾����yTKY�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_RInfoArray(ByVal pCnt As Integer) As String

        Get_RInfoArray = ""

        With typResistorInfoArray(pCnt)

            Get_RInfoArray = Right(Space(4) & .intResNo, 4) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intResMeasMode, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(3) & .intCircuitGrp, 3) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbHiNo, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbLoNo, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo1, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo2, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo3, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo4, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intProbAGNo5, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(17) & .strExternalBits, 17) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(6) & .intPauseTime, 6) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intTargetValType, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(4) & .intBaseResNo, 4) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(18) & .dblTrimTargetVal.ToString("0.000000"), 18) & ","
            ' �t�@�C���o�[�W�����ɂ���ďo��
            If gStrTkyFileVer = CONST_FILETYPE5 Then
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Hi_X.ToString("0.0000"), 9) & ","
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Hi_Y.ToString("0.0000"), 9) & ","
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Lo_X.ToString("0.0000"), 9) & ","
                Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblProbCfmPoint_Lo_Y.ToString("0.0000"), 9) & ","
            End If

            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intSlope, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intCutReviseMode, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intCutReviseDispMode, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(3) & .intCutRevisePtnNo, 3) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblCutRevisePosX.ToString("0.00"), 9) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(9) & .dblCutRevisePosY.ToString("0.00"), 9) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intIsNG, 2) & ","
            Get_RInfoArray = Get_RInfoArray & Right(Space(2) & .intCutCount, 2) & ","
            Get_RInfoArray = Get_RInfoArray & .strRatioTrimTargetVal

        End With

    End Function
#End Region

#Region "�O���[�o���f�[�^����J�b�g�f�[�^���擾����yTKY�p�z"
    '''=========================================================================
    '''<summary>�O���[�o���f�[�^����J�b�g�f�[�^���擾����yTKY�p�z</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function Get_CInfoArray(ByVal pCnt As Integer, ByVal pRCnt As Integer) As String

        Get_CInfoArray = ""

        Get_CInfoArray = Right(Space(4) & typResistorInfoArray(pRCnt).intResNo, 4) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutNo, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intDelayTime, 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(9) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblStartPointX.ToString("0.0000"), 9) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(9) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblStartPointY.ToString("0.0000"), 9) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblCutSpeed.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(5) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblQRate.ToString("0.0"), 5) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblCutOff.ToString("0.000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblJudgeLevel.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).strCutType, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutDir, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblMaxCutLength.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblR1.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblLTurnPoint.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblMaxCutLengthL.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblR2.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblMaxCutLengthHook.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intIndexCnt, 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intMeasMode, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblCutSpeed2.ToString("0.0"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(5) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblQRate2.ToString("0.0"), 5) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(4) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutAngle, 4) & "," & vbTab
        Get_CInfoArray = Get_CInfoArray & Right(Space(8) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblPitch.ToString("0.0000"), 8) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(2) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intStepDir, 2) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(5) & typResistorInfoArray(pRCnt).ArrCut(pCnt).intCutCnt, 5) & ","
        Get_CInfoArray = Get_CInfoArray & Right(Space(6) & typResistorInfoArray(pRCnt).ArrCut(pCnt).dblZoom.ToString("0.00"), 6) & ","
        Get_CInfoArray = Get_CInfoArray & Str2HexAsc(typResistorInfoArray(pRCnt).ArrCut(pCnt).strChar)

    End Function
#End Region
#End Region

    '======================================================================
    '  CHIP,NET�p���\�b�h
    '======================================================================
#Region "�yCHIP,NET�p���\�b�h�z"
#Region "��ڰ��ް���۰�ޏ����yCHIP,NET�p�z"
    '''=========================================================================
    '''<summary>��ڰ��ް���۰�ޏ����yCHIP,NET�p�z</summary>
    '''<remarks>TKY��CHIP�ANET�Ńt�@�C�����������܂�ɈقȂ邽�߁A
    '''         ��U�ACHIP��NET�̓}�[�W���s���ATKY�͕ʊ֐��̓Ǐo���Ƃ���B</remarks>
    '''=========================================================================
    Public Function FileLoadExe(ByRef fileName As String) As Short

        'Dim fn As Short                                 ' file no.
        Dim rBuff As String                             ' read buff
        Dim rType As Short                              ' data type
        Dim i As Short
        Dim rCnt As Short
        Dim rDATA() As String
        Dim blnTy2Data As Boolean                       ' CHIP�̂�

        'On Error GoTo ErrTrap

        ' �e��f�[�^�̏�����
        FileLoadExe = 0
        'V5.0.0.8�@        Call Init_FileVer_Sub()                         ' �����p̧���ް�ޮݐݒ�
        blnTy2Data = False                              ' CHIP�̂�
        'Call ClearBuff()                                       'V5.0.0.9�A
        Dim pData As List(Of String) = New List(Of String)()    'V5.0.0.9�A
        Call Init_AllTrmmingData()                      ' ���ݸ����Ұ��̏�����

        ' �e�L�X�g�t�@�C�� �I�[�v��
        rType = -1
        rCnt = 0
        'fn = FreeFile()
        Try
            If (False = IO.File.Exists(fileName)) Then Throw New FileNotFoundException() 'V4.4.0.0-1
            'FileOpen(fn, fileName, OpenMode.Input)
            Using sr As New StreamReader(fileName, Encoding.GetEncoding("Shift_JIS"))   ' ��̧�قȂ̂� Shift_JIS    V4.4.0.0-1
                Dim retVal As Short
                'Do While Not EOF(1)
                Do While (False = sr.EndOfStream)   'V4.4.0.0-1
                    ' 1 line read
                    'rBuff = LineInput(fn)
                    rBuff = sr.ReadLine()   'V4.4.0.0-1

                    ' �f�[�^�`�F�b�N(�Z�N�V�������̎���rType=�f�[�^�^�C�v���ݒ肳���)
                    retVal = FileLoadExe_Sub(rBuff, rType, rCnt, blnTy2Data)
                    If (retVal = -1) Then                       ' �f�[�^(�Z�N�V�������ȊO) ?
                        Select Case rType
                            Case -1
                                ' ϳ��߲������̫�Ăɖ߂�
                                Call SetMousePointer(Form1, False)
                                ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
                                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
                                FileLoadExe = 1
                                GoTo ERR_LINE
                            Case 0
                                ' file version check
                                If (gTkyKnd = KND_CHIP) Then
                                    retVal = FileVerCheck_CHIP(rBuff)
                                ElseIf (gTkyKnd = KND_NET) Then
                                    retVal = FileVerCheck_NET(rBuff)
                                End If
                                If retVal = 1 Then
                                    FileLoadExe = 1
                                    GoTo ERR_LINE
                                End If
                            Case 1 To 5 ' PLAT1-5
                                ' load data �� stock buff
                                'pData(rCnt) = rBuff : rCnt = rCnt + 1
                                pData.Add(rBuff) : rCnt = rCnt + 1      'V5.0.0.9�A
                            Case 6
                                ' load data �� step buff(global)
                                MaxStep = rCnt
                                With typStepInfoArray(rCnt)
                                    i = 0
                                    rDATA = rBuff.Split(",")
                                    '----- V1.14.0.0�E�� -----
                                    '.intSP1 = CShort(rDATA(i)) : i = i + 1  ' �ï�ߔԍ�
                                    '.intSP2 = CShort(rDATA(i)) : i = i + 1  ' ��ۯ���
                                    '.dblSP3 = CDbl(rDATA(i)) : i = i + 1    ' �ï�ߊԲ������
                                    '----- V1.14.0.0�E�� -----
                                End With
                                rCnt = rCnt + 1
                            Case 7
                                'load data �� resistor buff(global)
                                'V4.7.3.1�A                        Call GetResistorData(rBuff) V6.1.3.0�@
                                Call GetResistorData(rBuff, rCnt)         'V4.7.3.1�A V6.1.3.0�@
                                rCnt = rCnt + 1
                                System.Windows.Forms.Application.DoEvents()
                            Case 8
                                ' load data �� cut buff(global)
                                'V4.7.3.1�A                        Call GetCutData(rBuff)           V6.1.3.0�@       
                                Call GetCutData(rBuff, rCnt)            'V4.7.3.1�A V6.1.3.0�@
                                System.Windows.Forms.Application.DoEvents()
                            Case 9
                                ' load data �� group buff(global)
                                If (gTkyKnd = KND_CHIP) Then
                                    ' �O���[�v���̎擾
                                    If FileIO.FileVersion >= 3 Then
                                        MaxGrp = rCnt
                                        With typGrpInfoArray(rCnt)
                                            i = 0
                                            rDATA = rBuff.Split(",")
                                            '----- V1.14.0.0�E�� -----
                                            '.intGP1 = CShort(rDATA(i)) : i = i + 1  ' ��ٰ�ߔԍ�
                                            '.intGP2 = CShort(rDATA(i)) : i = i + 1  ' ��R��
                                            '.dblGP3 = CDbl(rDATA(i)) : i = i + 1    ' ��ٰ�ߊԲ������
                                            '----- V1.14.0.0�E�� -----
                                        End With
                                        rCnt = rCnt + 1
                                    End If

                                ElseIf (gTkyKnd = KND_NET) Then
                                    ' �T�[�L�b�g���̎擾
                                    With typCirAxisInfoArray(rCnt)
                                        '----- V1.14.0.0�E�� -----
                                        ' �T�[�L�b�g�e�B�[�`���O�R�}���h���L�����ɐݒ肷��
                                        If (Form1.stFNC(F_CIRCUIT).iDEF = 1) Then
                                            i = 0
                                            rDATA = rBuff.Split(",")
                                            .intCaP1 = CInt(rDATA(i)) : i = i + 1        ' �ï�ߔԍ�
                                            .dblCaP2 = CDbl(rDATA(i)) : i = i + 1        ' ���WX
                                            .dblCaP3 = CDbl(rDATA(i)) : i = i + 1        ' ���WY
                                        End If
                                        '----- V1.14.0.0�E�� -----
                                    End With
                                    rCnt = rCnt + 1
                                End If

                            Case 10
                                'load data �� Ty2 Data (global)
                                If (gTkyKnd = KND_CHIP) Then
                                    'If (SL432HW_FileVer >= 3) Then                                 ' V1.23.0.0�G
                                    If (FileIO.FileVersion >= 3) Or (FILETYPE_K = FILETYPE04_K) Then   ' V1.23.0.0�G
                                        MaxTy2 = rCnt                                   ' ���ۂ�Ty2��ۯ�����
                                        '----- V1.14.0.0�E�� -----
                                        ' TY2�R�}���h���L�����ɐݒ肷��
                                        If (Form1.stFNC(F_TY2).iDEF = 1) Then
                                            With typTy2InfoArray(rCnt)
                                                i = 0
                                                rDATA = rBuff.Split(",")
                                                .intTy21 = CShort(rDATA(i)) : i = i + 1     ' ��ۯ�No.
                                                .dblTy22 = CDbl(rDATA(i)) : i = i + 1       ' �ï�߲������
                                            End With
                                        End If
                                        '----- V1.14.0.0�E�� -----
                                        rCnt = rCnt + 1
                                    End If

                                ElseIf (gTkyKnd = KND_NET) Then
                                    With typCirInInfoArray(rCnt)
                                        i = 0
                                        rDATA = rBuff.Split(",")
                                        '----- V1.14.0.0�E�� -----
                                        '.intCiP1 = CInt(rDATA(i)) : i = i + 1            ' �ï�ߔԍ�
                                        '.intCiP2 = CInt(rDATA(i)) : i = i + 1            ' ����Đ�
                                        '.dblCiP3 = CDbl(rDATA(i)) : i = i + 1            ' ����ĊԲ������
                                        '----- V1.14.0.0�E�� -----
                                    End With
                                    rCnt = rCnt + 1
                                End If

                        End Select
                    End If
                Loop
ERR_LINE:
            End Using

            ' error?
            If FileLoadExe = 0 Then
                ' load data �� plate buff(global)
                'Call SetFileLoadPlateData()
                Call SetFileLoadPlateData(pData)        'V5.0.0.9�A
                ' TY2���ް������݂��Ȃ��ꍇ�A������ް�����쐬����B
                If Not blnTy2Data Then
                    Call GetTy2StepPos(1, False)
                End If
            Else
                ' mouse pointer default
                Call SetMousePointer(Form1, True)
            End If
            ' NG����߲�Ď擾
            'Call SetNG_MarkingPos()

            Exit Function

        Catch ex As FileNotFoundException
            FileLoadExe = 1
            ' �w�肳�ꂽ�t�@�C���͑��݂��܂���
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_15, MsgBoxStyle.OkOnly, gAppName)

        Catch ex As Exception
            FileLoadExe = 1
            MessageBox.Show("##ERROR INFO<DATA LOAD>##" & vbCrLf &
                   "  ERROR DISCRIPTION" & vbTab & " : " & Err.Description & vbCrLf &
                   "  ERROR NUMBER     " & vbTab & " : " & Err.Number & vbCrLf)
        End Try

    End Function
#End Region

#Region "�f�[�^�`�F�b�N�����yCHIP,NET�p�z"
    '''=========================================================================
    '''<summary>�f�[�^�`�F�b�N�����yCHIP,NET�p�z</summary>
    '''<param name="rBuff">     (INP) �f�[�^</param>
    '''<param name="rType">     (OUT) �f�[�^�^�C�v</param>
    '''<param name="rCnt">      (OUT) rCnt</param>
    '''<param name="blnTy2Data">(OUT) TY2�f�[�^(CHIP�̂�)</param>
    '''<returns>0=�f�[�^�^�C�v�ݒ�, -1=���̑��f�[�^</returns>
    '''=========================================================================
    Private Function FileLoadExe_Sub(ByRef rBuff As String, ByRef rType As Short, ByRef rCnt As Short, ByRef blnTy2Data As Boolean) As Short

        ' �f�[�^�`�F�b�N
        FileLoadExe_Sub = 0                             ' Return�l = ���� 
        Select Case rBuff
            Case FILE_CONST_VERSION
                rType = 0                               ' file ver data
            Case FILE_CONST_PLATE_01
                rType = 1                               ' plate01 data
            Case FILE_CONST_PLATE_02
                rType = 2                               ' plate02 data
            Case FILE_CONST_PLATE_03
                rType = 3                               ' plate03 data
            Case FILE_CONST_PLATE_04
                rType = 4                               ' plate04 data
            Case FILE_CONST_PLATE_05
                rType = 5                               ' plate05 data
            Case FILE_CONST_STEPDATA
                rType = 6 : rCnt = 1                    ' step data
            Case FILE_CONST_RESISTOR
                rType = 7 : rCnt = 1                    ' resistor data
            Case FILE_CONST_CUT_DATA
                rType = 8 : rCnt = 0                    ' cut data
            Case FILE_CONST_GRP_DATA                    ' CHIP�̂� 
                If (gTkyKnd = KND_CHIP) Then
                    rType = 9 : rCnt = 1                ' group data
                Else
                    FileLoadExe_Sub = -1                ' Return�l = ���̑��f�[�^
                End If
            Case FILE_CONST_TY2_DATA                    ' CHIP�̂� 
                If (gTkyKnd = KND_CHIP) Then
                    rType = 10 : rCnt = 1               ' TY2 data
                    blnTy2Data = True
                Else
                    FileLoadExe_Sub = -1                ' Return�l = ���̑��f�[�^
                End If

            Case FILE_CONST_CIR_DATA                    ' NET�̂� 
                If (gTkyKnd = KND_NET) Then
                    rType = 9 : rCnt = 1                 ' circuit data
                Else
                    FileLoadExe_Sub = -1                ' Return�l = ���̑��f�[�^
                End If
            Case FILE_CONST_CIRIDATA                    ' NET�̂� 
                If (gTkyKnd = KND_NET) Then
                    rType = 10 : rCnt = 1                ' circuit interval data
                Else
                    FileLoadExe_Sub = -1                ' Return�l = ���̑��f�[�^
                End If
            Case Else
                FileLoadExe_Sub = -1                    ' Return�l = ���̑��f�[�^
        End Select

    End Function

#End Region

#Region "��ڰ��ް��̾��ޏ����yCHIP,NET�p�z    'V4.4.0.0-1 ���ı��"
    ''''=========================================================================
    ''''<summary>��ڰ��ް��̾��ޏ����yCHIP,NET�p�z</summary>
    ''''<param name="fileName">(INP) �t�@�C���p�X��</param>
    ''''<returns>0=����, 0�ȊO=�G���[</returns>
    ''''=========================================================================
    'Public Function FileSaveExe(ByRef fileName As String) As Short

    '    Dim fn As Short
    '    Dim i As Short
    '    Dim ii As Short
    '    Dim ChipNum As Short
    '    Dim CutNum As Short
    '    Dim RegNo As Short
    '    Dim uDATA As String
    '    Dim dData As String
    '    '                                                   ' NET�p  
    '    Dim CirNum As Integer                               ' ����Đ�
    '    Dim GrpNum As Integer

    '    Dim strMSG As String

    '    Try
    '        FileSaveExe = 0
    '        uDATA = ""
    '        dData = ""
    '        If (gTkyKnd = KND_NET) Then
    '            CirNum = typPlateInfo.intCircuitCntInBlock  ' 1��ۯ��໰��Đ�
    '        End If

    '        ' file open(save)
    '        fn = FreeFile()
    '        FileOpen(fn, fileName, OpenMode.Output)

    '        '-----------------------------------------------------------------------
    '        '   FILE VERSION
    '        '-----------------------------------------------------------------------
    '        SL432HW_FileVer = NewFileVer
    '        If (gTkyKnd = KND_CHIP) Then
    '            If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then
    '                SL432HW_FileVer = 5
    '            End If
    '        End If

    '        PrintLine(fn, FILE_CONST_VERSION)
    '        Select Case SL432HW_FileVer
    '            Case 1 : PrintLine(fn, FILETYPE01)
    '            Case 2 : PrintLine(fn, FILETYPE02)
    '            Case 3 : PrintLine(fn, FILETYPE03)
    '            Case 4 : PrintLine(fn, FILETYPE04)
    '            Case 5 : PrintLine(fn, FILETYPE05)
    '            Case 6 : PrintLine(fn, FILETYPE06)
    '            Case 10 : PrintLine(fn, FILETYPE10)
    '            Case Else : PrintLine(fn, "NOT FILE VERSION")
    '        End Select

    '        '-----------------------------------------------------------------------
    '        '   PLATE DATA
    '        '-----------------------------------------------------------------------
    '        With typPlateInfo
    '            '-----------------------------------------------------------------------
    '            '   PLATE 01
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_01)
    '            PrintLine(fn, .strDataName)                             ' �ް�No.
    '            If (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, CStr(.intMeasType))                   ' ���Ӱ��
    '            End If
    '            PrintLine(fn, CStr(.intDirStepRepeat))                  ' �ï��&��߰�
    '            PrintLine(fn, .intBlockCntXDir & "," & .intBlockCntYDir) ' ��ۯ���XY
    '            PrintLine(fn, .dblTableOffsetXDir.ToString("0.0000") & "," & .dblTableOffsetYDir.ToString("0.0000"))  ' ð��وʒu�̾��XY
    '            PrintLine(fn, .dblBpOffSetXDir.ToString("0.0000") & "," & .dblBpOffSetYDir.ToString("0.0000"))        ' �ްшʒu�̾��XY
    '            PrintLine(fn, .dblAdjOffSetXDir.ToString("0.0000") & "," & .dblAdjOffSetYDir.ToString("0.0000"))      ' ��ެ�Ĉʒu�̾��XY
    '            PrintLine(fn, CStr(.intNGMark))                         ' NGϰ�ݸ�
    '            PrintLine(fn, CStr(.intDelayTrim))                      ' �ިڲ���
    '            PrintLine(fn, CStr(.intNgJudgeUnit))                    ' NG����P��
    '            PrintLine(fn, CStr(.intNgJudgeLevel))                   ' NG����
    '            PrintLine(fn, .dblZOffSet.ToString("0.0000"))           ' ��۰��Z�̾��
    '            PrintLine(fn, .dblZStepUpDist.ToString("0.0000"))       ' ��۰�޽ï�ߏ㏸����
    '            PrintLine(fn, .dblZWaitOffset.ToString("0.0000"))       ' ��۰�ޑҋ@Z�̾��
    '            '-----------------------------------------------------------------------
    '            '   PLATE 03
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_03)
    '            If (gTkyKnd = KND_CHIP) Then
    '                PrintLine(fn, CStr(.intResistDir))                                                                              ' ��R���ѕ���
    '                PrintLine(fn, CStr(.intResistCntInGroup))                                                                       ' 1��ٰ�ߓ���R��
    '                PrintLine(fn, .intGroupCntInBlockXBp & "," & .intGroupCntInBlockYStage)                                         ' ��ۯ����ٰ�ߐ�XY
    '                PrintLine(fn, .dblBpGrpItv.ToString("0.0000") & "," & .dblStgGrpItvY.ToString("0.0000"))                        ' ��ٰ�ߊԊuBP(X),Stage(Y)
    '                'PrintLine(fn, .dblGroupItvXDir.ToString("0.0000") & "," & .dblGroupItvYDir.ToString("0.0000"))                 ' ��ٰ�ߊԊuXY
    '                PrintLine(fn, .dblChipSizeXDir.ToString("0.0000") & "," & .dblChipSizeYDir.ToString("0.0000"))                  ' ���߻���XY
    '                PrintLine(fn, .dblStepOffsetXDir.ToString("0.0000") & "," & .dblStepOffsetYDir.ToString("0.0000"))              ' �ï�ߵ̾�ė�XY
    '                PrintLine(fn, .dblBlockSizeReviseXDir.ToString("0.0000") & "," & .dblBlockSizeReviseYDir.ToString("0.0000"))    ' ��ۯ����ޕ␳XY
    '                PrintLine(fn, .dblBlockItvXDir.ToString("0.0000") & "," & .dblBlockItvYDir.ToString("0.0000"))                  ' ��ۯ��ԊuXY
    '                PrintLine(fn, CStr(.intContHiNgBlockCnt))                                                                       ' �A��NG-HIGH��R��ۯ���
    '            ElseIf (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, .intPlateCntXDir & "," & .intPlateCntYDir)                                                        ' ��ڰĐ�XY
    '                PrintLine(fn, .dblPlateItvXDir.ToString("0.0000") & "," & .dblPlateItvYDir.ToString("0.0000"))                  ' ��ڰĊԊuXY
    '                PrintLine(fn, CStr(.intCircuitCntInBlock))                                                                      ' 1��ۯ��໰��Đ�
    '                PrintLine(fn, .dblCircuitSizeXDir.ToString("0.0000") & "," & .dblCircuitSizeYDir.ToString("0.0000"))            ' ����Ļ���XY
    '                PrintLine(fn, CStr(.intResistCntInGroup))                                                                       ' 1����ē���R��
    '                PrintLine(fn, .intGroupCntInBlockXBp & "," & .intGroupCntInBlockYStage)                                         ' ��ۯ����ٰ�ߐ�XY
    '                PrintLine(fn, .dblBlockSizeReviseXDir.ToString("0.0000") & "," & .dblBlockSizeReviseYDir.ToString("0.0000"))    ' ��ۯ����ޕ␳XY
    '            End If
    '            '-----------------------------------------------------------------------
    '            '   PLATE 02
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_02)
    '            PrintLine(fn, CStr(.intReviseMode))                                                                                 ' �␳Ӱ��
    '            PrintLine(fn, CStr(.intManualReviseType))                                                                           ' �␳���@
    '            PrintLine(fn, .dblReviseCordnt1XDir.ToString("0.0000") & "," & .dblReviseCordnt1YDir.ToString("0.0000"))            ' �␳�ʒu���W1XY
    '            PrintLine(fn, .dblReviseCordnt2XDir.ToString("0.0000") & "," & .dblReviseCordnt2YDir.ToString("0.0000"))            ' �␳�ʒu���W2XY
    '            PrintLine(fn, .dblReviseOffsetXDir.ToString("0.0000") & "," & .dblReviseOffsetYDir.ToString("0.0000"))              ' �␳�߼޼�ݵ̾��XY
    '            PrintLine(fn, CStr(.intRecogDispMode))                                                                              ' �F���ް��\��Ӱ��
    '            If gSysPrm.stDEV.giEXCAM = 1 Then
    '                PrintLine(fn, gSysPrm.stGRV.gfEXCAM_PixelX.ToString("0.0000") & "," & gSysPrm.stGRV.gfEXCAM_PixelY.ToString("0.0000")) ' �߸�ْlXY
    '            Else
    '                PrintLine(fn, gSysPrm.stGRV.gfPixelX.ToString("0.0000") & "," & gSysPrm.stGRV.gfPixelY.ToString("0.0000"))             ' �߸�ْlXY
    '            End If
    '            PrintLine(fn, .intRevisePtnNo1 & "," & .intRevisePtnNo2)                    ' �␳�ʒu�����No1,2
    '            ' �␳�ʒu�p�^�[���ԍ��̃O���[�v�ԍ��ݒ�
    '            If SL432HW_FileVer > 6 Then
    '                PrintLine(fn, .intRevisePtnNo1GroupNo & "," & .intRevisePtnNo2GroupNo)  ' �␳�ʒu�O���[�vNo1,2
    '            End If

    '            If (gTkyKnd = KND_CHIP) Then
    '                PrintLine(fn, gSysPrm.stDEV.gfRot_X1.ToString("0.000") & "," & gSysPrm.stDEV.gfRot_Y1.ToString("0.000"))    ' XY�����̉�]���S 
    '                PrintLine(fn, .dblThetaAxis.ToString("0.00000"))                        '�Ǝ�
    '            ElseIf (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, gSysPrm.stDEV.gfRot_X1.ToString("0.000") & "," & gSysPrm.stDEV.gfRot_X1.ToString("0.000"))    ' XY�����̉�]���S 
    '                PrintLine(fn, .dblThetaAxis.ToString("0.00000"))                        '�Ǝ�
    '            End If

    '            If (gTkyKnd = KND_CHIP) Then
    '                If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then                         ' KOA(EW�a)�Ȃ�Ver1.40�`���Ƃ���
    '                Else
    '                    PrintLine(fn, .dblTThetaOffset.ToString("0.00000"))                 ' �s�ƃI�t�Z�b�g
    '                    PrintLine(fn, .dblTThetaBase1XDir.ToString("0.0000"))               ' �s�Ɗ�ʒu�PX
    '                    PrintLine(fn, .dblTThetaBase1YDir.ToString("0.0000"))               ' �s�Ɗ�ʒu�PY
    '                    PrintLine(fn, .dblTThetaBase2XDir.ToString("0.0000"))               ' �s�Ɗ�ʒu�QX
    '                    PrintLine(fn, .dblTThetaBase2YDir.ToString("0.0000"))               ' �s�Ɗ�ʒu�QY
    '                End If
    '            End If
    '            '-----------------------------------------------------------------------
    '            '   PLATE 04
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_04)
    '            PrintLine(fn, .dblCaribBaseCordnt1XDir.ToString("0.0000") & "," & .dblCaribBaseCordnt1YDir.ToString("0.0000")) ' �����ڰ��݊���W1XY
    '            PrintLine(fn, .dblCaribBaseCordnt2XDir.ToString("0.0000") & "," & .dblCaribBaseCordnt2YDir.ToString("0.0000")) ' �����ڰ��݊���W2XY
    '            PrintLine(fn, .dblCaribTableOffsetXDir.ToString("0.0000") & "," & .dblCaribTableOffsetYDir.ToString("0.0000")) ' �����ڰ��ݵ̾��XY
    '            PrintLine(fn, .intCaribPtnNo1 & "," & .intCaribPtnNo2)                      ' �����ڰ�������ݓo�^No1,2
    '            ' �L�����u���[�V�����p�^�[���ԍ��̃O���[�v�ԍ��ݒ�
    '            If SL432HW_FileVer > 6 Then
    '                PrintLine(fn, .intCaribPtnNo1GroupNo & "," & .intCaribPtnNo2GroupNo)    ' �����ڰ��������No1,2
    '            End If
    '            PrintLine(fn, .dblCaribCutLength.ToString("0.0000"))                        ' �����ڰ��ݶ�Ē�
    '            PrintLine(fn, .dblCaribCutSpeed.ToString("0.0000"))                         ' �����ڰ��ݶ�đ��x
    '            PrintLine(fn, .dblCaribCutQRate.ToString("0.0"))                            ' �����ڰ���ڰ��Qڰ�
    '            PrintLine(fn, .dblCutPosiReviseOffsetXDir.ToString("0.0000") & "," & .dblCutPosiReviseOffsetYDir.ToString("0.0000")) ' ��Ĉʒu�␳ð��ٵ̾��XY
    '            PrintLine(fn, CStr(.intCutPosiRevisePtnNo))                                 ' ��Ĉʒu�␳����ݓo�^No
    '            PrintLine(fn, .dblCutPosiReviseCutLength.ToString("0.0000"))                ' ��Ĉʒu�␳��Ē�
    '            PrintLine(fn, .dblCutPosiReviseCutSpeed.ToString("0.0000"))                 ' ��Ĉʒu�␳��đ��x
    '            PrintLine(fn, .dblCutPosiReviseCutQRate.ToString("0.0"))                    ' ��Ĉʒu�␳ڰ��Qڰ�
    '            PrintLine(fn, CStr(.intCutPosiReviseGroupNo))                               ' ��ٰ��No
    '            '-----------------------------------------------------------------------
    '            '   PLATE 05
    '            '-----------------------------------------------------------------------
    '            PrintLine(fn, FILE_CONST_PLATE_05)
    '            If (gTkyKnd = KND_CHIP) Then
    '                PrintLine(fn, CStr(.intMaxTrimNgCount))                     ' ���ݸ�NG����(���)
    '                PrintLine(fn, CStr(.intMaxBreakDischargeCount))             ' ���ꌇ���r�o����(���)
    '                PrintLine(fn, CStr(.intTrimNgCount))                        ' �A�����ݸ�NG����
    '                PrintLine(fn, CStr(.intRetryProbeCount))                    ' ����۰��ݸމ�
    '                PrintLine(fn, .dblRetryProbeDistance.ToString("0.0000"))    ' ����۰��ݸވړ���
    '                PrintLine(fn, CStr(.intLedCtrl))                            ' LED����
    '                ' ����ڰ����ܰ��������
    '                If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then             ' KOA(EW�a)�Ȃ�Ver1.40�`���Ƃ���
    '                Else
    '                    PrintLine(fn, CStr(.intPowerAdjustMode))                ' �p���[�������[�h
    '                    PrintLine(fn, .dblPowerAdjustTarget.ToString("0.00"))   ' �����ڕW�p���[
    '                    PrintLine(fn, .dblPowerAdjustQRate.ToString("0.0"))     ' �p���[����Q���[�g
    '                    PrintLine(fn, .dblPowerAdjustToleLevel.ToString("0.00")) ' �p���[�������e�͈�
    '                End If

    '                PrintLine(fn, CStr(.intGpibCtrl))                           ' GP-IB����
    '                PrintLine(fn, CStr(.intGpibDefDelimiter))                   ' �����ݒ�(�����)
    '                PrintLine(fn, CStr(.intGpibDefTimiout))                     ' �����ݒ�(��ѱ��)
    '                PrintLine(fn, CStr(.intGpibDefAdder))                       ' �����ݒ�(�@����ڽ)
    '                PrintLine(fn, CStr(.strGpibInitCmnd1))                      ' �����������
    '                PrintLine(fn, CStr(.strGpibInitCmnd2))                      ' �����������
    '                PrintLine(fn, CStr(.strGpibTriggerCmnd))                    ' �ض޺����
    '                PrintLine(fn, CStr(.intGpibMeasSpeed))                      ' ���葬�x
    '                PrintLine(fn, CStr(.intGpibMeasMode))                       ' ���胂�[�h

    '            ElseIf (gTkyKnd = KND_NET) Then
    '                PrintLine(fn, CStr(.intRetryProbeCount))                    ' ����۰��ݸމ�
    '                PrintLine(fn, .dblRetryProbeDistance.ToString("0.0000"))    ' ����۰��ݸވړ���
    '            End If
    '        End With

    '        '-----------------------------------------------------------------------
    '        '   STEP DATA(TKY)
    '        '-----------------------------------------------------------------------
    '        If (gTkyKnd = KND_CHIP) Then
    '            ' STEP
    '            PrintLine(fn, FILE_CONST_STEPDATA)
    '            For i = 1 To MaxStep
    '                With typStepInfoArray(i)
    '                    ' �ï�ߔԍ�+��ۯ���+�ï�ߊԲ������
    '                    PrintLine(fn, Right(Space(3) & .intSP1, 3) & "," & Right(Space(2) & .intSP2, 3) & "," & Right(Space(7) & .dblSP3.ToString("0.0000"), 7))
    '                End With
    '            Next i

    '            ' GROUP DATA
    '            PrintLine(fn, FILE_CONST_GRP_DATA)
    '            For i = 1 To MaxGrp
    '                With typGrpInfoArray(i)
    '                    '�ï�ߔԍ�+��ۯ���+�ï�ߊԲ������
    '                    PrintLine(fn, Right(Space(3) & .intGP1, 3) & "," & Right(Space(2) & .intGP2, 2) & "," & Right(Space(7) & .dblGP3.ToString("0.0000"), 7))
    '                End With
    '            Next i

    '            ' TY2 DATA
    '            PrintLine(fn, FILE_CONST_TY2_DATA)
    '            For i = 1 To MaxTy2
    '                With typTy2InfoArray(i)
    '                    '�ï�ߔԍ�+��ۯ���+�ï�ߊԲ������
    '                    PrintLine(fn, Right(Space(3) & .intTy21, 3) & "," & Right(Space(7) & .dblTy22.ToString("0.0000"), 9))
    '                End With
    '            Next i

    '        ElseIf (gTkyKnd = KND_NET) Then
    '            '-----------------------------------------------------------------------
    '            '   STEP DATA(NET)
    '            '-----------------------------------------------------------------------
    '            ' CIRCUIT
    '            PrintLine(fn, FILE_CONST_CIR_DATA)
    '            For i = 1 To CirNum
    '                With typCirAxisInfoArray(i)
    '                    ' �ï�ߔԍ�+���WX+���WY
    '                    PrintLine(fn, Right(Space(3) & .intCaP1, 3) & "," & _
    '                                  Right(Space(7) & .dblCaP2.ToString("0.0000"), 7) & "," & _
    '                                  Right(Space(7) & .dblCaP3.ToString("0.0000"), 7))
    '                End With
    '            Next i
    '            ' CIRCUIT INTERVAL
    '            If typPlateInfo.intResistDir = 0 Then
    '                GrpNum = typPlateInfo.intGroupCntInBlockXBp     ' ��ٰ�ߐ�X
    '            Else
    '                GrpNum = typPlateInfo.intGroupCntInBlockYStage
    '            End If
    '            PrintLine(fn, FILE_CONST_CIRIDATA)
    '            For i = 1 To GrpNum
    '                With typCirInInfoArray(i)
    '                    ' �ï�ߔԍ�+����Đ�+����ĊԲ������
    '                    PrintLine(fn, Right(Space(3) & .intCiP1, 3) & "," & Right(Space(3) & .intCiP2, 3) & "," & Right(Space(7) & .dblCiP3.ToString("0.0000"), 7))
    '                End With
    '            Next i
    '            ' STEP
    '            PrintLine(fn, FILE_CONST_STEPDATA)
    '            For i = 1 To MaxStep
    '                With typStepInfoArray(i)
    '                    ' �ï�ߔԍ�+��ۯ���+�ï�ߊԲ������
    '                    PrintLine(fn, Right(Space(3) & .intSP1, 3) & "," & Right(Space(2) & .intSP2, 2) & "," & Right(Space(7) & .dblSP3.ToString("0.0000"), 7))
    '                End With
    '            Next i
    '        End If

    '        '-----------------------------------------------------------------------
    '        '   RESISTOR
    '        '-----------------------------------------------------------------------
    '        PrintLine(fn, FILE_CONST_RESISTOR)
    '        Call GetChipNum(ChipNum)                    ' ��R���擾
    '        ii = Len(CStr(gSysPrm.stDEV.giDCScaner))
    '        For i = 1 To ChipNum

    '            With typResistorInfoArray(i)
    '                If (gTkyKnd = KND_CHIP) Then
    '                    '-----------------------------------------------------------
    '                    '   CHIP��
    '                    '-----------------------------------------------------------
    '                    ' ��R�ԍ�,���葪��, ��۰�ޔԍ�HI, ��۰�ޔԍ�LO, ��۰�ޔԍ�AG1�`AG5, EXTERNAL BIT, �߰��,���ݸޖڕW�l, ��R, �؂�グ�{��
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(4) & .intResMeasMode.ToString("0"), 4) & "," & Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & "," & Right(Space(7) & .dblDeltaR.ToString("0.00"), 7) & "," & Right(Space(7) & .dblCutOffRatio.ToString("0.00"), 7) & ","
    '                    ' �Ƽ��ý� HI/LO�Я�, ̧���ý� HI/LO�Я�, ��Đ�
    '                    dData = Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(2) & .intCutCount, 2)

    '                ElseIf (gTkyKnd = KND_NET) Then
    '                    '-----------------------------------------------------------
    '                    '   NET��
    '                    '-----------------------------------------------------------
    '                    ' ��R�ԍ�, ���������, ���葪��, ��۰�ޔԍ�HI, ��۰�ޔԍ�LO, ��۰�ޔԍ�AG1�`AG5, EXTERNAL BIT, �߰��, ���Ӱ��, �ް���R, ���ݸޖڕW�l
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(3) & .intCircuitGrp, 3) & "," & Right(Space(1) & .intResMeasMode, 1) & "," & _
    '                            Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & _
    '                            Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & _
    '                            Right(Space(1) & .intTargetValType, 1) & "," & Right(Space(3) & .intBaseResNo, 3) & "," & _
    '                            Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & ","
    '                    ' �d���ω��۰��, �Ƽ��ý� HI/LO�Я�, ̧���ý� HI/LO�Я�, ��Đ�
    '                    dData = Right(Space(1) & .intSlope, 1) & "," & _
    '                            Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(2) & .intCutCount, 2)
    '                End If
    '                PrintLine(fn, uDATA & dData)
    '            End With
    '        Next i

    '        If typPlateInfo.intNGMark = 1 Then
    '            With typResistorInfoArray(1000)
    '                If (gTkyKnd = KND_CHIP) Then
    '                    '-----------------------------------------------------------
    '                    '   CHIP��
    '                    '-----------------------------------------------------------
    '                    ' ��R�ԍ�, ���葪��, ��۰�ޔԍ�HI, ��۰�ޔԍ�LO,��۰�ޔԍ�AG1�`AG5,EXTERNAL BIT,�߰��,���ݸޖڕW�l,��R,�؂�グ�{��
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(4) & .intResMeasMode.ToString("0"), 4) & "," & Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & "," & Right(Space(7) & .dblDeltaR.ToString("0.00"), 7) & "," & Right(Space(7) & .dblCutOffRatio.ToString("0.00"), 7) & ","
    '                    ' �Ƽ��ý� HI/LO�Я�,̧���ý� HI/LO�Я�,��Đ�
    '                    dData = Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & Right(Space(2) & .intCutCount, 2)

    '                ElseIf (gTkyKnd = KND_NET) Then
    '                    '-----------------------------------------------------------
    '                    '   NET��
    '                    '-----------------------------------------------------------
    '                    ' ��R�ԍ�,���������,���葪��,��۰�ޔԍ�HI,��۰�ޔԍ�LO,��۰�ޔԍ�AG1�`AG5,EXTERNAL BIT,�߰��,���Ӱ��,�ް���R,���ݸޖڕW�l
    '                    uDATA = Right(Space(3) & .intResNo, 4) & "," & Right(Space(3) & .intCircuitGrp, 3) & "," & Right(Space(1) & .intResMeasMode, 1) & "," & _
    '                            Right(Space(ii) & .intProbHiNo, ii) & "," & Right(Space(ii) & .intProbLoNo, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo1, ii) & "," & Right(Space(ii) & .intProbAGNo2, ii) & "," & _
    '                            Right(Space(ii) & .intProbAGNo3, ii) & "," & Right(Space(ii) & .intProbAGNo4, ii) & "," & Right(Space(ii) & .intProbAGNo5, ii) & "," & _
    '                            Right(Space(16) & .strExternalBits, 16) & "," & Right(Space(5) & .intPauseTime, 5) & "," & _
    '                            Right(Space(1) & .intTargetValType, 1) & "," & Right(Space(3) & .intBaseResNo, 3) & "," & _
    '                            Right(Space(16) & .dblTrimTargetVal.ToString("0.00000"), 16) & ","
    '                    ' �d���ω��۰��,�Ƽ��ý� HI/LO�Я�,̧���ý� HI/LO�Я�,��Đ�
    '                    dData = Right(Space(1) & .intSlope, 1) & "," & _
    '                            Right(Space(7) & .dblInitTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblInitTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(7) & .dblFinalTest_HighLimit.ToString("0.00"), 7) & "," & Right(Space(7) & .dblFinalTest_LowLimit.ToString("0.00"), 7) & "," & _
    '                            Right(Space(2) & .intCutCount, 2)
    '                End If
    '                PrintLine(fn, uDATA & dData)
    '            End With
    '        End If

    '        '-----------------------------------------------------------------------
    '        '   CUT DATA
    '        '-----------------------------------------------------------------------
    '        PrintLine(fn, FILE_CONST_CUT_DATA)
    '        For i = 1 To ChipNum                            ' ��R�����ݒ肷��
    '            RegNo = typResistorInfoArray(i).intResNo    ' ��R�ԍ��擾
    '            Call GetRegCutNum(RegNo, CutNum)            ' ��Đ��擾

    '            With typResistorInfoArray(i)
    '                For ii = 1 To CutNum
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP��
    '                        '-----------------------------------------------------------
    '                        If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then ' KOA(EW�a)�Ȃ�Ver1.40�`���Ƃ���
    '                            ' ��R�ԍ�+��Ĕԍ�,�ިڲ���,è��ݸ��߲��(XY),�����߲��(XY),��Ľ�߰��,Qڰ�,��ĵ�,���藦
    '                            uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                        Else
    '                            ' ��R�ԍ�+��Ĕԍ�,�ިڲ���,è��ݸ��߲��(XY),�����߲��(XY),��Ľ�߰��,Qڰ�,��ĵ�,��ĵ̵̾��,���藦
    '                            uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblCutOffOffset.ToString("0.000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                        End If

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET��
    '                        '-----------------------------------------------------------
    '                        ' ��R�ԍ�,��Ĕԍ�,�ިڲ���,è��ݸ��߲��(XY),�����߲��(XY),��Ľ�߰��,Qڰ�,��ĵ�,���藦
    '                        uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                    End If

    '                    ' ��Č`��,��ĕ���,��Ē�1,R1,L����߲��,L��݌�̍ő嶯èݸޒ�,R2,̯���݌�̶�èݸޒ�,���ޯ����
    '                    ' ����Ӱ��,��Ľ�߰��2,Q����ڰ�2,�΂ߊp�x,�߯�,�ï�ߕ���,�{��,���޾ݽ�߲��,���޾ݽ�̔���ω���
    '                    ' ���޾ݽ��̶�Ē�,�{��,������
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP��
    '                        '-----------------------------------------------------------
    '                        '���޾ݽ��̕ω���,���޾ݽ��̊m�F��
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & ","
    '                        dData = dData & Right(Space(4) & .ArrCut(ii).intMeasMode.ToString("0"), 4) & "," ' ����Ӱ��(4���o��)
    '                        dData = dData & Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & Right(Space(18) & .ArrCut(ii).strChar, 18) & "," & Right(Space(5) & .ArrCut(ii).dblESChangeRatio.ToString("0.0"), 6) & "," & Right(Space(2) & .ArrCut(ii).intESConfirmCnt.ToString("0"), 2) & "," & Right(Space(4) & .ArrCut(ii).intRadderInterval.ToString("0"), 4)

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET��
    '                        '-----------------------------------------------------------
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intMeasMode, 1) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & _
    '                                Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & _
    '                                Right(Space(18) & .ArrCut(ii).strChar, 18)
    '                    End If

    '                    PrintLine(fn, uDATA & dData)
    '                Next ii
    '            End With
    '        Next i

    '        ' NG�}�[�N�p(1000��)
    '        If typPlateInfo.intNGMark = 1 Then
    '            RegNo = typResistorInfoArray(1000).intResNo ' ��R�ԍ��擾
    '            Call GetRegCutNum(RegNo, CutNum)            ' ��Đ��擾
    '            With typResistorInfoArray(1000)
    '                For ii = 1 To CutNum
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP��
    '                        '-----------------------------------------------------------
    '                        ' ��R�ԍ�,��Ĕԍ�,�ިڲ���,è��ݸ��߲��(XY),�����߲��(XY),��Ľ�߰��,Qڰ�+��ĵ�,��ĵ̵̾��,���藦
    '                        uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblCutOffOffset.ToString("0.000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET��
    '                        '-----------------------------------------------------------
    '                        '��R�ԍ�,��Ĕԍ�,�ިڲ���,è��ݸ��߲��(XY),�����߲��(XY),��Ľ�߰��,Qڰ�,��ĵ�,���藦
    '                        uDATA = Right(Space(4) & RegNo, 4) & "," & Right(Space(2) & .ArrCut(ii).intCutNo, 2) & "," & Right(Space(5) & .ArrCut(ii).intDelayTime, 5) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblTeachPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblTeachPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblStartPointX.ToString("0.0000"), 8) & "," & Right(Space(8) & .ArrCut(ii).dblStartPointY.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed.ToString("0.0"), 5) & "," & Right(Space(4) & .ArrCut(ii).dblQRate.ToString("0.0"), 4) & "," & Right(Space(7) & .ArrCut(ii).dblCutOff.ToString("0.000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblJudgeLevel.ToString("0.0"), 5) & ","
    '                    End If

    '                    ' ��Č`��,��ĕ���,��Ē�1,R1,L����߲��,L��݌�̍ő嶯èݸޒ�,R2,̯���݌�̶�èݸޒ�,���ޯ����,����Ӱ��
    '                    ' ��Ľ�߰��2,Q����ڰ�2,�΂ߊp�x,�߯�,�ï�ߕ���,�{��,���޾ݽ�߲��,���޾ݽ�̔���ω���,���޾ݽ��̶�Ē�,�{��,������
    '                    If (gTkyKnd = KND_CHIP) Then
    '                        '-----------------------------------------------------------
    '                        '   CHIP��
    '                        '-----------------------------------------------------------
    '                        '���޾ݽ��̕ω���,���޾ݽ��̊m�F��
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & ","
    '                        dData = dData & Right(Space(4) & .ArrCut(ii).intMeasMode.ToString("0"), 4) & "," ' ###177 ����Ӱ��(4���o��)
    '                        dData = dData & Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & _
    '                                Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & _
    '                                Right(Space(18) & .ArrCut(ii).strChar, 18) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESChangeRatio.ToString("0.00"), 6) & "," & _
    '                                Right(Space(2) & .ArrCut(ii).intESConfirmCnt.ToString("0"), 2) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intRadderInterval.ToString("0"), 4)

    '                    ElseIf (gTkyKnd = KND_NET) Then
    '                        '-----------------------------------------------------------
    '                        '   NET��
    '                        '-----------------------------------------------------------
    '                        dData = Right(Space(1) & .ArrCut(ii).strCutType, 1) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intCutDir, 1) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLength.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR1.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblLTurnPoint.ToString("0.0"), 5) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthL.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblR2.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblMaxCutLengthHook.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).intIndexCnt, 5) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intMeasMode, 1) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblCutSpeed2.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblQRate2.ToString("0.0"), 4) & "," & _
    '                                Right(Space(3) & .ArrCut(ii).intCutAngle.ToString("0"), 3) & "," & _
    '                                Right(Space(7) & .ArrCut(ii).dblPitch.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(1) & .ArrCut(ii).intStepDir, 1) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).intCutCnt, 4) & "," & _
    '                                Right(Space(8) & .ArrCut(ii).dblESPoint.ToString("0.0000"), 8) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblESJudgeLevel.ToString("0.0"), 5) & "," & _
    '                                Right(Space(4) & .ArrCut(ii).dblMaxCutLengthES.ToString("0.0000"), 7) & "," & _
    '                                Right(Space(5) & .ArrCut(ii).dblZoom.ToString("0.00"), 5) & "," & _
    '                                Right(Space(18) & .ArrCut(ii).strChar, 18)
    '                    End If
    '                    PrintLine(fn, uDATA & dData)
    '                Next ii
    '            End With
    '        End If

    '        FileClose(fn)
    '        Exit Function

    '        ' �g���b�v�G���[������
    '    Catch ex As Exception
    '        strMSG = "File.FileSaveExe() TRAP ERROR = " + ex.Message
    '        MsgBox(strMSG)
    '        Return (1)                                      ' Return�l = ��O�G���[
    '    End Try

    'End Function

#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ�薢�g�p"
#If False Then 'V5.0.0.8�@
#Region "�����p̧���ް�ޮݐݒ�yCHIP,NET�p�z"
    '''=========================================================================
    '''<summary>�����p̧���ް�ޮݐݒ�yCHIP,NET�p�z</summary>
    '''=========================================================================
    Public Sub Init_FileVer_Sub()

        ' �����p̧���ް�ޮݐݒ�
        If (gTkyKnd = KND_CHIP) Then
            NewFileVer = 6                                      ' �t�@�C���o�[�W������
            FILETYPE01 = "TKYCHIP_SL432HW_Ver1.00"
            FILETYPE02 = "TKYCHIP_SL432HW_Ver1.10"
            FILETYPE03 = "TKYCHIP_SL432HW_Ver1.20"
            FILETYPE04 = "TKYCHIP_SL432HW_Ver1.30"
            FILETYPE05 = "TKYCHIP_SL432HW_Ver1.40"
            FILETYPE06 = "TKYCHIP_SL432HW_Ver1.50"
            FILETYPE07_02 = "TKYCHIP_SL432HW_Ver7.0.0.2"        'V1.14.0.0�E
            FILETYPE10 = "TKYCHIP_SL432HW_Ver10.00"             ' ������
            FILETYPE10_01 = "TKYCHIP_SL432HW_Ver10.01"          ' ������
            FILETYPE10_02 = "TKYCHIP_SL432HW_Ver10.02"          ' ������
            FILETYPE10_03 = "TKYCHIP_SL432HW_Ver10.03"          ' ������ V1.13.0.0�A
            FILETYPE10_04 = "TKYCHIP_SL432HW_Ver10.04"          ' ������ V1.14.0.0�@
            FILETYPE10_05 = "TKYCHIP_SL432HW_Ver10.05"          ' ������ V1.16.0.0�@
            FILETYPE10_06 = "TKYCHIP_SL432HW_Ver10.06"          ' ������ V1.18.0.0�C
            FILETYPE10_07 = "TKYCHIP_SL432HW_Ver10.07"          ' ������ V1.18.0.0�C
            FILETYPE10_072 = "TKYCHIP_SL432HW_Ver10.072"        ' ������ V2.0.0.0_24
            FILETYPE10_073 = "TKYCHIP_SL432HW_Ver10.073"        ' ������ V2.0.0.0_24
            FILETYPE10_08 = "TKYCHIP_SL432HW_Ver10.08"          ' ������(�m���^�P�a���� ���T�|�[�g)
            FILETYPE10_09 = "TKYCHIP_SL432HW_Ver10.09"          ' ������ V1.23.0.0�A
            FILETYPE10_10 = "TKYCHIP_SL432HW_Ver10.10"          ' ������ V4.0.0.0�C
            FILETYPE10_11 = "TKYCHIP_SL432HW_Ver10.11"          ' ������ V4.11.0.0�@
            FILETYPE_CUR = "TKYCHIP_SL432HW_Ver10.11"           ' ������(���ݔŖ�) V4.11.0.0�@

        ElseIf (gTkyKnd = KND_NET) Then
            NewFileVer = 3                                      ' �t�@�C���o�[�W������
            FILETYPE01 = "TKYNET_SL432HW_Ver1.00"
            FILETYPE02 = "TKYNET_SL432HW_Ver1.01"
            FILETYPE03 = "TKYNET_SL432HW_Ver1.02"
            FILETYPE04 = "---"
            FILETYPE05 = "---"
            FILETYPE06 = "---"
            FILETYPE07_02 = "TKYNET_SL432HW_Ver7.0.0.2"         'V1.14.0.0�E
            FILETYPE10 = "TKYNET_SL432HW_Ver10.00"              ' ������
            FILETYPE10_01 = "TKYNET_SL432HW_Ver10.01"           ' ������
            FILETYPE10_02 = "TKYNET_SL432HW_Ver10.02"           ' ������
            FILETYPE10_03 = "TKYNET_SL432HW_Ver10.03"           ' ������ V1.13.0.0�A
            FILETYPE10_04 = "TKYNET_SL432HW_Ver10.04"           ' ������ V1.14.0.0�@
            FILETYPE10_05 = "TKYNET_SL432HW_Ver10.05"           ' ������ V1.16.0.0�@
            FILETYPE10_06 = "TKYNET_SL432HW_Ver10.06"           ' ������ V1.18.0.0�C
            FILETYPE10_07 = "TKYNET_SL432HW_Ver10.07"           ' ������ V1.18.0.0�C
            FILETYPE10_072 = "TKYNET_SL432HW_Ver10.072"         ' ������ V2.0.0.0_24
            FILETYPE10_073 = "TKYNET_SL432HW_Ver10.073"         ' ������ V2.0.0.0_24
            FILETYPE10_08 = "TKYNET_SL432HW_Ver10.08"           ' ������(�m���^�P�a���� ���T�|�[�g)
            FILETYPE10_09 = "TKYNET_SL432HW_Ver10.09"           ' ������ V1.23.0.0�A
            FILETYPE10_10 = "TKYNET_SL432HW_Ver10.10"           ' ������ V4.0.0.0�C
            FILETYPE10_11 = "TKYNET_SL432HW_Ver10.11"           ' ������ V4.11.0.0�@
            FILETYPE_CUR = "TKYNET_SL432HW_Ver10.11"            ' ������(���ݔŖ�) V4.11.0.0�@
        End If

    End Sub

#End Region
#End If
#End Region 'V5.0.0.8�@

#Region "۰�ނ�����ڰ��ް����۰��ٕϐ��֊i�[����yCHIP/NET���ʁz"
    '''=========================================================================
    '''<summary>۰�ނ�����ڰ��ް����۰��ٕϐ��֊i�[����yCHIP/NET���ʁz</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetFileLoadPlateData(ByVal pData As List(Of String))    'V5.0.0.9�A
        'Private Sub SetFileLoadPlateData()
        On Error Resume Next
        Dim i As Short

        With typPlateInfo
            '<PLATE DATA 1>
            .strDataName = pData(i) : i = i + 1
            If (gTkyKnd = KND_NET) Then
                .intMeasType = pData(i) : i = i + 1     ' ���Ӱ��
            End If
            .intDirStepRepeat = CShort(pData(i)) : i = i + 1
            .intBlockCntXDir = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
            .intBlockCntYDir = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            '----- V6.1.4.0�K�� -----
            .intBlkCntInStgGrpX = .intBlockCntXDir
            .intBlkCntInStgGrpY = .intBlockCntYDir

            '----- V6.1.4.0�K�� -----
            ' BP�̾�Ă̓�������
            If gSysPrm.stCTM.giBPOffsetInput = 0 Then
                ' ���͂���̏ꍇ
                .dblTableOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblTableOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblBpOffSetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblBpOffSetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            Else
                ' ���͂Ȃ��̏ꍇ�Að��ٵ̾�Ēl�̋t�����
                .dblTableOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblTableOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblBpOffSetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblBpOffSetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                ' BP�̾�Ăɋt�����
                .dblBpOffSetXDir = -(.dblTableOffsetXDir)
                .dblBpOffSetYDir = -(.dblTableOffsetYDir)
            End If

            .dblAdjOffSetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblAdjOffSetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intNGMark = CShort(pData(i)) : i = i + 1
            .intNGMark = 0  'V4.7.3.1�A�m�f�}�[�N�̓f�[�^�\�����قȂ�̂Ŗ����ɂ���B V6.1.3.0�@
            .intDelayTrim = CShort(pData(i)) : i = i + 1
            .intNgJudgeUnit = CShort(pData(i)) : i = i + 1
            .intNgJudgeLevel = CShort(pData(i)) : i = i + 1
            .dblZOffSet = CDbl(pData(i)) : i = i + 1
            .dblZStepUpDist = CDbl(pData(i)) : i = i + 1
            .dblZWaitOffset = CDbl(pData(i)) : i = i + 1
            '----- V1.23.0.0�G�� -----
            ' Z ON/OFF�ʒu���Đݒ肷��
            .dblZOffSet = 5.0                                               'ZON�ʒu
            .dblZStepUpDist = 3.0                                           '��۰�޽ï�ߏ㏸����
            .dblZWaitOffset = 1.0                                           'ZOFF�ʒu
            '----- V1.23.0.0�G�� -----

            '<PLATE DATA 3>
            If (gTkyKnd = KND_CHIP) Then
                .intResistDir = CShort(pData(i)) : i = i + 1
            ElseIf (gTkyKnd = KND_NET) Then
                .intPlateCntXDir = Left(pData(i), InStr(pData(i), ",") - 1)                 ' ��ڰĐ�X 
                .intPlateCntYDir = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1      ' ��ڰĐ�Y 
                .dblCircuitSizeXDir = Left(pData(i), InStr(pData(i), ",") - 1)              ' ��ڰĊԊuX 
                .dblCircuitSizeYDir = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1   ' ��ڰĊԊuY 
                .intCircuitCntInBlock = pData(i) : i = i + 1                                ' 1��ۯ��໰��Đ� 
                .dblCircuitSizeXDir = Left(pData(i), InStr(pData(i), ",") - 1)              ' ����Ļ���X 
                .dblCircuitSizeYDir = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1   ' ����Ļ���Y 
                .dblChipSizeXDir = .dblCircuitSizeXDir                                      ' V1.14.0.0�E
                .dblChipSizeYDir = .dblCircuitSizeYDir                                      ' V1.14.0.0�E
            End If
            .intResistCntInGroup = CShort(pData(i)) : i = i + 1         ' 1�O���[�v����R��
            '----- V1.14.0.0�E�� -----
            If (gTkyKnd = KND_CHIP) Then
                .intResistCntInBlock = .intResistCntInGroup             ' 1�u���b�N����R��
                gRegistorCnt = .intResistCntInGroup
            End If
            '----- V1.14.0.0�E�� -----

            If (gTkyKnd = KND_CHIP) Then
                .intGroupCntInBlockXBp = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
                .intGroupCntInBlockYStage = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblBpGrpItv = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblStgGrpItvY = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                '.dblGroupItvXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                '.dblGroupItvYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblChipSizeXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblChipSizeYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .dblStepOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblStepOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            ElseIf (gTkyKnd = KND_NET) Then
                If FileIO.FileVersion >= 2 Then
                    .intGroupCntInBlockXBp = Left(pData(i), InStr(pData(i), ",") - 1)
                    .intGroupCntInBlockYStage = Mid(pData(i), InStr(pData(i), ",") + 1) : i = i + 1
                    .intGroupCntInBlockXBp = .intCircuitCntInBlock      ' V1.14.0.0�E BP�O���[�v��(�T�[�L�b�g��)
                    .intBlkCntInStgGrpY = .intBlockCntYDir              ' V1.14.0.0�E Y�����X�e�[�W�O���[�v���u���b�N��
                    .intResistCntInBlock = .intGroupCntInBlockXBp       ' V1.14.0.0�E 1�u���b�N����R��
                    .intResistCntInBlock = .intGroupCntInBlockXBp * .intResistCntInGroup        'V4.7.3.1�A 1�u���b�N����R�� V6.1.3.0�@
                    gRegistorCnt = .intResistCntInBlock                 ' V1.14.0.0�E
                    .dblChipSizeXDir = .dblCircuitSizeXDir / .intResistCntInGroup              'V4.7.3.1�A��R���ѕ����͂w�Œ�ŏ��� V6.1.3.0�@
                End If
            End If
            '----- V6.1.4.0�K�� -----
            '.dblBlockSizeReviseXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            '.dblBlockSizeReviseYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblBlockSizeXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))              '�u���b�N�T�C�Y�w
            .dblBlockSizeYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1   '�u���b�N�T�C�Y�x
            .dblBlockSizeReviseXDir = .dblBlockSizeXDir
            .dblBlockSizeReviseYDir = .dblBlockSizeYDir
            '----- V6.1.4.0�K�� -----
            If (gTkyKnd = KND_CHIP) Then
                .dblBlockItvXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
                .dblBlockItvYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
                .intContHiNgBlockCnt = CShort(pData(i)) : i = i + 1
            End If

            '<PLATE DATA 2>
            .intReviseMode = CShort(pData(i)) : i = i + 1
            .intManualReviseType = CShort(pData(i)) : i = i + 1
            .dblReviseCordnt1XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblReviseCordnt1YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblReviseCordnt2XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblReviseCordnt2YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblReviseOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblReviseOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intRecogDispMode = CShort(pData(i)) : i = i + 1
            .dblPixelValXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblPixelValYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intRevisePtnNo1 = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
            .intRevisePtnNo2 = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            If FileIO.FileVersion >= 10 Then
                .intRevisePtnNo1GroupNo = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
                .intRevisePtnNo2GroupNo = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            End If

            .dblRotateXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblRotateYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1

            If (gTkyKnd = KND_CHIP) Then
                If (FileIO.FileVersion >= 4) Or (FILETYPE_K = FILETYPE04_K) Then ' V1.23.0.0�G
                    .dblThetaAxis = CDbl(pData(i)) : i = i + 1
                End If
                If FileIO.FileVersion >= 6 Then
                    .dblTThetaOffset = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase1XDir = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase1YDir = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase2XDir = CDbl(pData(i)) : i = i + 1
                    .dblTThetaBase2YDir = CDbl(pData(i)) : i = i + 1
                End If
            ElseIf (gTkyKnd = KND_NET) Then
                If FileIO.FileVersion >= 3 Then
                    .dblThetaAxis = pData(i) : i = i + 1
                End If
            End If

            '<PLATE DATA 4>
            .dblCaribBaseCordnt1XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCaribBaseCordnt1YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblCaribBaseCordnt2XDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCaribBaseCordnt2YDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .dblCaribTableOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCaribTableOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intCaribPtnNo1 = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
            .intCaribPtnNo2 = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            If FileIO.FileVersion >= 10 Then
                .intCaribPtnNo1GroupNo = CShort(Left(pData(i), InStr(pData(i), ",") - 1))
                .intCaribPtnNo2GroupNo = CShort(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            End If
            .dblCaribCutLength = CDbl(pData(i)) : i = i + 1
            .dblCaribCutSpeed = CDbl(pData(i)) : i = i + 1
            .dblCaribCutQRate = CDbl(pData(i)) : i = i + 1
            .dblCutPosiReviseOffsetXDir = CDbl(Left(pData(i), InStr(pData(i), ",") - 1))
            .dblCutPosiReviseOffsetYDir = CDbl(Mid(pData(i), InStr(pData(i), ",") + 1)) : i = i + 1
            .intCutPosiRevisePtnNo = CShort(pData(i)) : i = i + 1
            .dblCutPosiReviseCutLength = CDbl(pData(i)) : i = i + 1
            .dblCutPosiReviseCutSpeed = CDbl(pData(i)) : i = i + 1
            .dblCutPosiReviseCutQRate = CDbl(pData(i)) : i = i + 1
            If FileIO.FileVersion < 10 Then
                .intRevisePtnNo1GroupNo = CShort(pData(i))
                .intRevisePtnNo2GroupNo = CShort(pData(i))
                '----- V6.1.4.0�K�� -----
                .intCaribPtnNo1GroupNo = CShort(pData(i))                       ' �L�����u���[�V�����O���[�v�ԍ�
                .intCaribPtnNo2GroupNo = CShort(pData(i))
                '----- V6.1.4.0�K�� -----
            End If
            .intCutPosiReviseGroupNo = CShort(pData(i)) : i = i + 1

            '<PLATE DATA 5>
            If (gTkyKnd = KND_CHIP) Then
                .intMaxTrimNgCount = CShort(pData(i)) : i = i + 1
                .intMaxBreakDischargeCount = CShort(pData(i)) : i = i + 1
                .intTrimNgCount = CShort(pData(i)) : i = i + 1
            End If
            .intRetryProbeCount = CShort(pData(i)) : i = i + 1
            .dblRetryProbeDistance = CDbl(pData(i)) : i = i + 1

            If (gTkyKnd = KND_CHIP) Then
                '----- V1.23.0.0�G�� -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then
                    ' ����ڰ����ܰ��������
                    .intPowerAdjustMode = CShort(pData(i)) : i = i + 1          ' ��ܰ����Ӱ��
                    .dblPowerAdjustTarget = CDbl(pData(i)) : i = i + 1          ' �����ڕW��ܰ
                    .dblPowerAdjustQRate = CDbl(pData(i)) : i = i + 1           ' ��ܰ����Qڰ�
                    .dblPowerAdjustToleLevel = CDbl(pData(i)) : i = i + 1       ' ��ܰ�������e�͈�

                    .intInitialOkTestDo = CShort(pData(i)) : i = i + 1          ' �Ƽ��OKý�
                    .intWorkSetByLoader = CShort(pData(i)) : i = i + 1          ' ��i��
                    .intOpenCheck = CShort(pData(i)) : i = i + 1                ' 4�[�q���������

                    Return
                End If
                '----- V1.23.0.0�G�� -----

                If FileIO.FileVersion >= 2 Then
                    .intLedCtrl = CShort(pData(i)) : i = i + 1  ' LED����
                End If

                If FileIO.FileVersion >= 5 Then
                    If (FileIO.FileVersion >= 6) Then
                        ' ����ڰ����ܰ��������
                        .intPowerAdjustMode = CShort(pData(i)) : i = i + 1      ' ��ܰ����Ӱ��
                        .dblPowerAdjustTarget = CDbl(pData(i)) : i = i + 1      ' �����ڕW��ܰ
                        .dblPowerAdjustQRate = CDbl(pData(i)) : i = i + 1       ' ��ܰ����Qڰ�
                        .dblPowerAdjustToleLevel = CDbl(pData(i)) : i = i + 1   ' ��ܰ�������e�͈�
                    End If

                    ' GP-IB����
                    If gSysPrm.stCTM.giGP_IB_flg = 0 Then
                        .intGpibCtrl = 0 : i = i + 1                        ' GP-IB����
                    Else
                        .intGpibCtrl = CShort(pData(i)) : i = i + 1         ' GP-IB����
                    End If

                    If gSysPrm.stCTM.giGP_IB_flg = 2 Then
                        .intGpibDefDelimiter = 0 : i = i + 1                ' �����ݒ�(�����) (�Œ�)
                        .intGpibDefTimiout = 100 : i = i + 1                ' �����ݒ�(��ѱ��) (�Œ�)
                        .intGpibDefAdder = CShort(pData(i)) : i = i + 1     ' �����ݒ�(�@����ڽ)
                        .strGpibInitCmnd1 = "" : i = i + 1                  ' �����������(�����ł͉������Ȃ�)
                        .strGpibInitCmnd2 = "" : i = i + 1                  ' �����������(�����ł͉������Ȃ�)
                        .strGpibTriggerCmnd = "E" : i = i + 1               ' �ض޺���� (�Œ�)
                        .intGpibMeasSpeed = CShort(pData(i)) : i = i + 1    ' ���葬�x
                        .intGpibMeasMode = CShort(pData(i)) : i = i + 1     ' ���胂�[�h
                        Call MakeGPIBinit()                                 ' ����������ނ��쐬
                    Else
                        .intGpibDefDelimiter = CShort(pData(i)) : i = i + 1 ' �����ݒ�(�����)
                        .intGpibDefTimiout = CShort(pData(i)) : i = i + 1   ' �����ݒ�(��ѱ��)
                        .intGpibDefAdder = CShort(pData(i)) : i = i + 1     ' �����ݒ�(�@����ڽ)
                        .strGpibInitCmnd1 = pData(i) : i = i + 1            ' �����������
                        .strGpibInitCmnd2 = pData(i) : i = i + 1            ' �����������
                        .strGpibTriggerCmnd = pData(i) : i = i + 1          ' �ض޺����
                        .intGpibMeasSpeed = 1 : i = i + 1                   ' ���葬�x(1:����)  (�Œ�)
                        .intGpibMeasMode = 0 : i = i + 1                    ' ���胂�[�h(0:���)  (�Œ�)
                    End If
                End If
            End If
        End With
    End Sub

#End Region

#Region "۰���ް��i�[�ޯ̧�̸ر�yCHIP/NET���ʁz"
    ' '''=========================================================================      'V5.0.0.9�A
    ' '''<summary>۰���ް��i�[�ޯ̧�̸ر�yCHIP/NET���ʁz</summary>
    ' '''<remarks></remarks>
    ' '''=========================================================================
    'Private Sub ClearBuff()

    '    Dim i As Short

    '    For i = 0 To 100
    '        pData(i) = ""
    '    Next i

    'End Sub

#End Region

#Region "��R�f�[�^�̎擾�yCHIP/NET���ʁz"
    '''=========================================================================
    '''<summary>��R�f�[�^�̎擾�yCHIP/NET���ʁz</summary>
    '''<param name="strDATA">(INP) �t�@�C���p�X��</param>
    '''<param name="rCnt">   (I/O)�f�[�^�C���f�b�N�X</param>	'V4.7.3.1�AADD V6.1.3.0�@
    '''<returns>0=����, 1=�G���[ </returns>
    '''=========================================================================
    '    Private Function GetResistorData(ByRef strDATA As String) As Short
    Private Function GetResistorData(ByRef strDATA As String, ByRef rCnt As Integer) As Short   'V4.7.3.1�A ByRef rCnt As Integer�ǉ� V6.1.3.0�@

        Dim i As Short
        Dim vWork() As String
        Dim ResNo As Short                              ' ��R�ԍ�

        GetResistorData = 0
        i = 0
        vWork = strDATA.Split(",")
        ResNo = CShort(vWork(0))                        ' ��R�ԍ��擾

        'V4.7.3.1�A        With typResistorInfoArray(ResNo) V6.1.3.0�@
        With typResistorInfoArray(rCnt)                 'V4.7.3.1�A �z��ԍ���ResNo����rCnt�֕ύX V6.1.3.0�@ 
            .intResNo = CShort(vWork(i)) : i = i + 1            ' ��R�ԍ�
            If (gTkyKnd = KND_NET) Then
                .intCircuitGrp = CInt(vWork(i)) : i = i + 1     ' ���������
            End If
            '----- V6.1.4.0�K�� -----
            '.intResMeasMode = CShort(vWork(i)) : i = i + 1      ' �����x����A����Ӱ��
            .intResMeasType = CShort(vWork(i)) : i = i + 1      ' �����x����A����Ӱ��
            '----- V6.1.4.0�K�� -----
            .intProbHiNo = CShort(vWork(i)) : i = i + 1         ' ��۰�ޔԍ�(HI)
            .intProbLoNo = CShort(vWork(i)) : i = i + 1         ' ��۰�ޔԍ�(LO)
            .intProbAGNo1 = CShort(vWork(i)) : i = i + 1        ' ��۰�ޔԍ�(AG1)
            .intProbAGNo2 = CShort(vWork(i)) : i = i + 1        ' ��۰�ޔԍ�(AG2)
            .intProbAGNo3 = CShort(vWork(i)) : i = i + 1        ' ��۰�ޔԍ�(AG3)
            .intProbAGNo4 = CShort(vWork(i)) : i = i + 1        ' ��۰�ޔԍ�(AG4)
            .intProbAGNo5 = CShort(vWork(i)) : i = i + 1        ' ��۰�ޔԍ�(AG5)
            '----- V1.23.0.0�G�� -----
            ' CHIP(SL436K)
            If (FILETYPE_K = FILETYPE04_K) Then
                .strExternalBits = 0                            ' EXTERNAL BIT
                .intPauseTime = 0                               ' �߰��
                .dblTrimTargetVal = CDbl(vWork(i)) : i = i + 1  ' ���ݸޖڕW�l
                .dblInitOKTest_HighLimit = CDbl(vWork(i)) : i = i + 1   '�Ƽ��OKý�(HI�Я�)
                .dblInitOKTest_LowLimit = CDbl(vWork(i)) : i = i + 1    '�Ƽ��OKý�(LO�Я�)
                GoTo STP_100
            Else
                .strExternalBits = CStr(vWork(i)) : i = i + 1       ' EXTERNAL BIT
                .intPauseTime = CShort(vWork(i)) : i = i + 1        ' �߰��
                If (gTkyKnd = KND_NET) Then
                    .intTargetValType = CInt(vWork(i)) : i = i + 1  ' ���Ӱ��
                    .intBaseResNo = CInt(vWork(i)) : i = i + 1      ' �ް���R
                End If
            End If
            .dblTrimTargetVal = CDbl(vWork(i)) : i = i + 1          ' ���ݸޖڕW�l
            '----- V1.23.0.0�G�� -----

            If (gTkyKnd = KND_CHIP) Then
                If FileIO.FileVersion >= 5 Then
                    .dblDeltaR = CDbl(vWork(i)) : i = i + 1     ' ��R
                    .dblCutOffRatio = CDbl(vWork(i)) : i = i + 1 ' �؂�グ�{��
                End If
                .intSlope = 4                                   ' V6.1.4.0�K
            ElseIf (gTkyKnd = KND_NET) Then
                .intSlope = CInt(vWork(i)) : i = i + 1          ' �d���ω��۰��
            End If
STP_100:    ' V1.23.0.0�G
            .dblInitTest_HighLimit = CDbl(vWork(i)) : i = i + 1 ' �Ƽ��ý�(HI�Я�)
            .dblInitTest_LowLimit = CDbl(vWork(i)) : i = i + 1  ' �Ƽ��ý�(LO�Я�)
            .dblFinalTest_HighLimit = CDbl(vWork(i)) : i = i + 1 ' ̧���ý�(HI�Я�)
            .dblFinalTest_LowLimit = CDbl(vWork(i)) : i = i + 1 ' ̧���ý�(LO�Я�)
            .intCutCount = CShort(vWork(i)) : i = i + 1         ' ��Đ�
        End With

    End Function
#End Region

#Region "�J�b�g�f�[�^�̎擾�yCHIP/NET���ʁz"
    '''=========================================================================
    '''<summary>�J�b�g�f�[�^�̎擾�yCHIP/NET���ʁz</summary>
    '''<param name="strDATA">(INP) �t�@�C���p�X��</param>
    '''<param name="rCnt">   (I/O)�f�[�^�C���f�b�N�X</param>	'V4.7.3.1�A V6.1.3.0�@
    '''<returns>0=����, 1=�G���[ </returns>
    '''=========================================================================
    '    Private Function GetCutData(ByRef strDATA As String) As Short
    Private Function GetCutData(ByRef strDATA As String, ByRef rCnt As Integer) As Short    'V4.7.3.1�A ByRef rCnt As Integer�ǉ� V6.1.3.0�@

        Dim i As Short
        Dim j As Integer
        Dim vWork() As String
        Dim ResNo As Short                              ' ��R�ԍ�
        Dim CutNum As Short                             ' ��Đ�
        Dim shWK As Short
        Dim strMSG As String                            ' V1.23.0.0�G

        Try ' V1.23.0.0�G

            GetCutData = 0
            i = 0
            vWork = strDATA.Split(",")

            ' CHIP�� 
            If (gTkyKnd = KND_CHIP) Then
                If UBound(vWork) < 35 Then
                    For i = UBound(vWork) + 1 To 35
                        ' ����Ȃ����ڂ�ǉ�
                        strDATA = strDATA & ","
                    Next
                    Erase vWork
                    vWork = strDATA.Split(",")
                    i = 0
                End If
            End If

            'V4.7.3.1�A            ResNo = CShort(vWork(0))                        ' ��R�ԍ��擾 V6.1.3.0�@
            CutNum = CShort(vWork(1))                       ' ��Ĕԍ��擾
            'V4.7.3.1�A�� V6.1.3.0�@
            If (CutNum = 1) Then                                        ' �J�b�g�ԍ�=�P�̏ꍇ�͒�R�f�[�^�C���f�b�N�X���J�E���g�A�b�v 
                rCnt = rCnt + 1
            End If
            ResNo = rCnt                                                               ' Rn = ��R�f�[�^�C���f�b�N�X
            'V4.7.3.1�A�� V6.1.3.0�@

            With typResistorInfoArray(ResNo)
                shWK = CShort(vWork(i)) : i = i + 1                         ' ��R�ԍ�
                .ArrCut(CutNum).intCutNo = CShort(vWork(i)) : i = i + 1     ' ��Ĕԍ�
                .ArrCut(CutNum).intDelayTime = CShort(vWork(i)) : i = i + 1 ' �ިڲ���
                .ArrCut(CutNum).dblTeachPointX = CDbl(vWork(i)) : i = i + 1 ' è��ݸ��߲��X
                .ArrCut(CutNum).dblTeachPointY = CDbl(vWork(i)) : i = i + 1 ' è��ݸ��߲��Y
                .ArrCut(CutNum).dblStartPointX = CDbl(vWork(i)) : i = i + 1 ' �����߲��X
                .ArrCut(CutNum).dblStartPointY = CDbl(vWork(i)) : i = i + 1 ' �����߲��Y
                .ArrCut(CutNum).dblCutSpeed = CDbl(vWork(i)) : i = i + 1    ' ��Ľ�߰��
                .ArrCut(CutNum).dblQRate = CDbl(vWork(i)) : i = i + 1       ' Q����ڰ�
                .ArrCut(CutNum).dblCutOff = CDbl(vWork(i)) : i = i + 1      ' ��ĵ̒l
                '----- V1.23.0.0�G�� -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then
                    i = i + 1                                               ' ��ٽ������
                    i = i + 1                                               ' ��ٽ������
                    i = i + 1                                               ' LSw�p���X������(�O���V���b�^)

                    .ArrCut(CutNum).dblJudgeLevel = 0.0                     ' �ؑփ|�C���g (���ް�����(���ω���))
                    .ArrCut(CutNum).dblCutOffOffset = 0.0                   ' ��ĵ� �̾��
                Else
                    .ArrCut(CutNum).dblJudgeLevel = CDbl(vWork(i)) : i = i + 1              ' �ؑփ|�C���g (���ް�����(���ω���))
                    If (gTkyKnd = KND_CHIP) Then
                        If (FileIO.FileVersion >= 6) Then
                            .ArrCut(CutNum).dblCutOffOffset = CDbl(vWork(i)) : i = i + 1    ' ��ĵ� �̾��
                        End If
                    End If
                End If

                '.ArrCut(CutNum).dblJudgeLevel = CDbl(vWork(i)) : i = i + 1  ' �ؑփ|�C���g (���ް�����(���ω���))
                'If (gTkyKnd = KND_CHIP) Then
                '    If (SL432HW_FileVer >= 6) Then
                '        .ArrCut(CutNum).dblCutOffOffset = CDbl(vWork(i)) : i = i + 1    ' ��ĵ� �̾��
                '    End If
                'End If
                '----- V1.23.0.0�G�� -----

                .ArrCut(CutNum).strCutType = CStr(vWork(i)) : i = i + 1         ' ��Č`��
                .ArrCut(CutNum).intCutDir = CShort(vWork(i)) : i = i + 1        ' ��ĕ���
                .ArrCut(CutNum).dblMaxCutLength = CDbl(vWork(i)) : i = i + 1    ' �ő嶯èݸޒ�
                .ArrCut(CutNum).dblR1 = CDbl(vWork(i)) : i = i + 1              ' R1
                .ArrCut(CutNum).dblLTurnPoint = CDbl(vWork(i)) : i = i + 1      ' L����߲��
                .ArrCut(CutNum).dblMaxCutLengthL = CDbl(vWork(i)) : i = i + 1   ' L��݌�̍ő嶯èݸޒ�
                .ArrCut(CutNum).dblR2 = CDbl(vWork(i)) : i = i + 1              ' R2
                .ArrCut(CutNum).dblMaxCutLengthHook = CDbl(vWork(i)) : i = i + 1 '̯���݌�̶�èݸޒ�
                .ArrCut(CutNum).intIndexCnt = CShort(vWork(i)) : i = i + 1      ' ���ޯ����
                .ArrCut(CutNum).intMeasMode = CShort(vWork(i)) : i = i + 1      ' ����Ӱ��
                .ArrCut(CutNum).dblCutSpeed2 = CDbl(vWork(i)) : i = i + 1       ' ��Ľ�߰��2
                '----- V1.13.0.0�A�� -----
                If (.ArrCut(CutNum).dblCutSpeed2 = 0) Then                      ' �J�b�g�X�s�[�h2�ɃJ�b�g�X�s�[�h��ݒ肷��
                    .ArrCut(CutNum).dblCutSpeed2 = .ArrCut(CutNum).dblCutSpeed
                End If
                '----- V1.13.0.0�A�� -----
                .ArrCut(CutNum).dblQRate2 = CDbl(vWork(i)) : i = i + 1          ' Q����ڰ�2
                '----- V1.13.0.0�A�� -----
                If (.ArrCut(CutNum).dblQRate2 = 0) Then                         ' �p�X�C�b�`���[�g�Q�ɂp�X�C�b�`���[�g��ݒ肷��
                    .ArrCut(CutNum).dblQRate2 = .ArrCut(CutNum).dblQRate
                End If
                '----- V1.13.0.0�A�� -----

                '----- V1.23.0.0�G�� -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then
                    i = i + 1                                                   'Q����ڰ�3
                    .ArrCut(CutNum).dblJudgeLevel = CDbl(vWork(i)) : i = i + 1  '�ؑւ��߲�� (���ް�����(���ω���))
                    .ArrCut(CutNum).intCutAngle = 0                             ' �΂߶�Ă̐؂�o���p�x
                Else
                    .ArrCut(CutNum).intCutAngle = CShort(vWork(i)) : i = i + 1  ' �΂߶�Ă̐؂�o���p�x
                End If
                '.ArrCut(CutNum).intCutAngle = CShort(vWork(i)) : i = i + 1      ' �΂߶�Ă̐؂�o���p�x
                '----- V1.23.0.0�G�� -----

                .ArrCut(CutNum).dblPitch = CDbl(vWork(i)) : i = i + 1           ' �߯�
                .ArrCut(CutNum).intStepDir = CShort(vWork(i)) : i = i + 1       ' �ï�ߕ���
                .ArrCut(CutNum).intCutCnt = CShort(vWork(i)) : i = i + 1        ' �{��
                .ArrCut(CutNum).dblESPoint = CDbl(vWork(i)) : i = i + 1         ' ���޾ݽ�߲��
                .ArrCut(CutNum).dblESJudgeLevel = CDbl(vWork(i)) : i = i + 1    ' ���޾ݽ�̔���ω���
                .ArrCut(CutNum).dblMaxCutLengthES = CDbl(vWork(i)) : i = i + 1  ' ���޾ݽ��̶�Ē�
                .ArrCut(CutNum).dblZoom = CDbl(vWork(i)) : i = i + 1            ' �{��
                .ArrCut(CutNum).strChar = Trim(CStr(vWork(i))) : i = i + 1      ' ������

                '----- V1.23.0.0�G�� -----
                ' CHIP(SL436K)
                If (FILETYPE_K = FILETYPE04_K) Then

                Else
                    If (gTkyKnd = KND_CHIP) Then
                        .ArrCut(CutNum).dblESChangeRatio = IIf(Len(vWork(i)) = 0, 0, vWork(i)) : i = i + 1  '���޾ݽ��̔���ω���
                        .ArrCut(CutNum).intESConfirmCnt = IIf(Len(vWork(i)) = 0, 0, vWork(i)) : i = i + 1   '���޾ݽ��̊m�F��
                        .ArrCut(CutNum).intRadderInterval = IIf(Len(vWork(i)) = 0, 0, vWork(i)) : i = i + 1 ' ��ް�ԋ���
                    End If
                End If
                '----- V1.23.0.0�G�� -----

                ' �J�b�g��������΂߃J�b�g�̐؂�o���p�x��ݒ肷��
                Call GetCutAngle(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).intCutDir, .ArrCut(CutNum).intCutAngle)
                ' �J�b�g��������΂߃J�b�g�̐؂�o���p�x��L�^�[��������ݒ肷��(L�J�b�g/HOOK�J�b�g�p)
                Call GetCutLTurnDir(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).intCutDir, .ArrCut(CutNum).intCutAngle, .ArrCut(CutNum).intLTurnDir)
                ' �X�e�b�v������ϊ�����(�X�L�����J�b�g�p)
                Call GetStepDir(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).intStepDir, .ArrCut(CutNum).intStepDir)
                '----- V1.18.0.0�D�� -----
                ' ST�J�b�g��L�J�b�g�͎΂�ST�J�b�g, �΂�L�J�b�g�ɕϊ�����
                Call CnvCutType(.ArrCut(CutNum).strCutType, .ArrCut(CutNum).strCutType)
                '----- V1.18.0.0�D�� -----

                ' �ڕW�p���[�Ƌ��e�͈�(�f�t�H���g�l��ݒ肷��) ###066
                'For j = 0 To (cCNDNUM - 1)                                          ' ���H�����ԍ�1�`n(0�ؼ��)
                For j = 0 To (MaxCndNum - 1)                                        ' ���H�����ԍ�1�`n(0�ؼ��) 'V5.0.0.8�@
                    'V6.0.0.1�E                    .ArrCut(CutNum).dblPowerAdjustTarget(j) = POWERADJUST_TARGET    ' �ڕW�p���[(W)
                    'V6.0.0.1�E                    .ArrCut(CutNum).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL  ' ���e�͈�(�}W)
                    .ArrCut(CutNum).dblPowerAdjustTarget(j) = DEFAULT_ADJUST_TAERGET    ' �ڕW�p���[(W)  'V6.0.0.1�E
                    .ArrCut(CutNum).dblPowerAdjustToleLevel(j) = DEFAULT_ADJUST_LEVEL   ' ���e�͈�(�}W)   'V6.0.0.1�E
                Next j

            End With

            Return (cFRS_NORMAL)                                        ' V1.23.0.0�G

            ' �g���b�v�G���[������ V1.23.0.0�G
        Catch ex As Exception
            strMSG = "File.GetCutData() TRAP ERROR = " + ex.Message
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ�薢�g�p"
#If False Then 'V5.0.0.8�@
#Region "�f�[�^�R���o�[�^�����yCHIP/NET���ʁz"
    '''=========================================================================
    '''<summary>�f�[�^�R���o�[�^�����yCHIP/NET���ʁz</summary>
    '''<param name="sp">(INP) �t�@�C���p�X��</param>
    '''<returns>0=����, 1=�G���[ </returns>
    '''=========================================================================
    Public Function DatConv_CHIPNET(ByRef sp As String) As Short

        Dim strD As String
        'Dim mPath As String

        DatConv_CHIPNET = 0

        If sp = "" Then
            ' "�w�肳�ꂽ�t�@�C���͑��݂��܂���"
            MsgBox(MSG_15, MsgBoxStyle.OkOnly, gAppName)
            DatConv_CHIPNET = 1
            Exit Function
        End If

        strD = Right(sp, 3)                                 ' �g���q���擾����
        Select Case strD
            'Case "WTN", "wtn", "WTC", "wtc"                ' V1.23.0.0�G
            Case "WTN", "wtn", "WTC", "wtc", "WDC", "wdc"   ' V1.23.0.0�G
                sp = sp

                '----- V4.0.0.0�F�� -----
                'Case ".DC", ".dc", "DAT", "dat"
                '    ' SL436H
                '    mPath = Mid(sp, 1, Len(sp) - 3)
                '    If UCase(strD) = ".DC" Then
                '        mPath = mPath & ".WTC"
                '    ElseIf UCase(strD) = "DAT" Then
                '        mPath = mPath & "WTN"
                '    End If

                '    DatConv_CHIPNET = Form1.DCConvert.DCConvert(sp, mPath)
                '    If DatConv_CHIPNET Then
                '        Debug.Print("DCConvert error section =" & Form1.DCConvert.ErrorSection)
                '        Debug.Print("DCConvert error item No =" & Form1.DCConvert.ErrorItemNo)
                '        Debug.Print("DCConvert error msg     =" & Form1.DCConvert.ErrorMessage)
                '        Debug.Print("DCConvert error line    =" & Form1.DCConvert.ErrorLine)
                '        MsgBox(Form1.DCConvert.ErrorMessage & " Line=" & CStr(Form1.DCConvert.ErrorLine))
                '        Exit Function
                '    End If
                '    sp = mPath
                '----- V4.0.0.0�F�� -----
        End Select

    End Function
#End Region
#End If
#End Region 'V5.0.0.8�@

#End Region

    '======================================================================
    '   TKY,CHIP,NET�����Ń��\�b�h
    '======================================================================
#Region "�yTKY,CHIP,NET�����ŗp���\�b�h�z"
#Region "�t�@�C�����[�h�����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>�t�@�C�����[�h�����yTKY,CHIP,NET�����Łz</summary>
    '''<param name="pPath">(INP) �t�@�C����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function File_Read(ByVal pPath As String) As Integer

        'Dim intFileNo As Integer                                ' �t�@�C���ԍ�
        'V5.0.0.8�@        Dim strDAT As String                                    ' �ǂݍ��݃f�[�^�o�b�t�@
        'V5.0.0.8�@        Dim intType As Integer                                  ' �f�[�^���
        'V5.0.0.8�@        Dim Err_num As Integer                                  ' �G���[���(1:�ΏۊO�t�@�C���A)
        'V5.0.0.8�@        Dim rCnt As Integer
        'Dim iFlg As Integer
        Dim r As Integer
        Dim strSECT As String = ""
        Dim strMSG As String

        Try
            ' ��������
            r = cFRS_NORMAL                                         ' Return�l = ����
            'V5.0.0.8�@            File_Read = 0
            'V5.0.0.8�@            Err_num = 0
            'iFlg = 0
            'V5.0.0.8�@            Call Init_FileVer_Sub()                                 ' �����p̧���ް�ޮݐݒ�
            Call Init_AllTrmmingData()                              ' �O���[�o���f�[�^������(Plate/Circuit/Resistor/Cut)

            'V5.0.0.8�@                  ��
            r = DirectCast(FileIO.File_Read(pPath, strSECT), Integer)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            SetTemporaryData(True)     ' �l��ޔ�����
#If False Then
            intType = -1
            rCnt = 0
            'ReDim pData(MAX_PDATA)
            Dim pData As List(Of String) = New List(Of String)()    'V5.0.0.9�A
            ReDim pGpibData(MAX_PGPIBDATA)                          ' ###229

            If (False = IO.File.Exists(pPath)) Then Throw New FileNotFoundException() 'V4.4.0.0-1

            ' �e�L�X�g�t�@�C�����I�[�v������
            Using fs As New FileStream(pPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Dim enc As Encoding
                If (True = fs.IsUTF8()) Then
                    enc = Encoding.UTF8
                Else
                    enc = Encoding.GetEncoding("Shift_JIS")
                End If
                Using sr As New StreamReader(fs, enc)
                    ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ��܂��B
                    Do While (False = sr.EndOfStream)
                        strDAT = sr.ReadLine()                          ' 1�s�ǂݍ���

                        ' �f�[�^��ʔ���
                        Select Case strDAT
                            Case FILE_CONST_VERSION                     ' �t�@�C���o�[�W����
                                strSECT = "[FILE VERSION]"
                                intType = SECT_VERSION
                            Case FILE_CONST_PLATE_01                    ' �v���[�g�f�[�^�P
                                strSECT = "[PLATE01]"
                                intType = SECT_PLATE01
                            Case FILE_CONST_PLATE_02                    ' �v���[�g�f�[�^�Q
                                strSECT = "[PLATE02]"
                                intType = SECT_PLATE02
                            Case FILE_CONST_PLATE_03                    ' �v���[�g�f�[�^�R
                                strSECT = "[PLATE03]"
                                intType = SECT_PLATE03
                            Case FILE_CONST_PLATE_04                    ' �v���[�g�f�[�^�S
                                strSECT = "[PLATE04]"
                                intType = SECT_PLATE04
                            Case FILE_CONST_PLATE_05                    ' �v���[�g�f�[�^�T
                                strSECT = "[PLATE05]"
                                intType = SECT_PLATE05
                                '------ V1.13.0.0�A�� ------
                            Case FILE_CONST_PLATE_06                    ' �v���[�g�f�[�^�U
                                strSECT = "[PLATE06]"
                                intType = SECT_PLATE06
                                '------ V1.13.0.0�A�� ------
                            Case FILE_CONST_CIR_DATA                    ' �T�[�L�b�g�f�[�^(TKY�p) 
                                strSECT = "[CIRCUIT]"
                                intType = SECT_CIRCUIT
                                rCnt = 1                                ' �T�[�L�b�g�f�[�^�C���f�b�N�X������(1�ؼ��) 
                            Case FILE_CONST_STEPDATA                    ' STEP�f�[�^
                                strSECT = "[STEP]"
                                intType = SECT_STEP
                                rCnt = 1                                ' STEP�f�[�^�C���f�b�N�X������(1�ؼ��) 
                            Case FILE_CONST_GRP_DATA                    ' �O���[�v�f�[�^
                                strSECT = "[GROUP]"
                                intType = SECT_GRP_DATA
                                rCnt = 1                                ' �O���[�v�f�[�^�C���f�b�N�X������(1�ؼ��) 
                            Case FILE_CONST_TY2_DATA                    ' TY2�f�[�^
                                strSECT = "[TY2]"
                                intType = SECT_TY2_DATA
                                rCnt = 1                                ' TY2�f�[�^�C���f�b�N�X������(1�ؼ��) 
                            Case FILE_CONST_CIRN_DATA                   ' CIRCUIT�f�[�^(NET�p)
                                strSECT = "[CIRCUIT AXIS]"
                                intType = SECT_CIR_AXIS
                                rCnt = 1                                ' CIRCUIT�f�[�^�C���f�b�N�X������(1�ؼ��) 
                            Case FILE_CONST_CIRIDATA                    ' �T�[�L�b�g�ԃC���^�[�o���f�[�^(NET�p)
                                strSECT = "[CIRCUIT INTERVAL]"
                                intType = SECT_CIR_ITVL
                                rCnt = 1                                ' �T�[�L�b�g�ԃC���^�[�o���f�[�^�C���f�b�N�X������(1�ؼ��) 
                                'Case CONST_IKEI_DATA                    ' �ٌ`�ʕt��(TKY�p) ���T�|�[�g 
                                '    intType = SECT_IKEI_DATA

                            Case FILE_CONST_RESISTOR                    ' ��R�f�[�^
                                strSECT = "[RESISTOR]]"
                                intType = SECT_REGISTOR
                                rCnt = 1                                ' ��R�f�[�^�C���f�b�N�X������(1�ؼ��)
                            Case FILE_CONST_CUT_DATA                    ' �J�b�g�f�[�^
                                strSECT = "[CUT]]"
                                intType = SECT_CUT_DATA
                                rCnt = 0                                ' ��R�f�[�^�C���f�b�N�X������(1�ؼ��)

                                '----- ###229�� -----
                            Case FILE_CONST_GPIB_DATA                   ' GPIB�f�[�^
                                strSECT = "[GPIB]"
                                intType = SECT_GPIB_DATA
                                rCnt = 0                                ' GPIB�f�[�^�C���f�b�N�X������(1�ؼ��)
                                '----- ###229�� -----
                                'V1.13.0.0�D
                            Case FILE_SINSYUKU_SELECT
                                strSECT = "[SINSYUKU]"
                                intType = SECT_SINSYUKU_DATA
                                rCnt = 0                                ' �L�k�␳�p�f�[�^�C���f�b�N�X������(1�ؼ��)
                                ClearBlockSelect()                                  'V1.13.0.0�D
                                'V1.13.0.0�D

                            ' �f�[�^(�Z�N�V�������ȊO)   
                            Case Else
                                ' ���[�h�����f�[�^���f�[�^�\���̂֊i�[����
                                'r = Set_Trim_Data(intType, strDAT, rCnt)
                                r = Set_Trim_Data(intType, strDAT, rCnt, pData)     'V5.0.0.9�A
                                If (r <> cFRS_NORMAL) Then              ' �G���[�Ȃ�I��
                                    Exit Do
                                End If
                        End Select
                    Loop

                End Using
            End Using
#End If
            'V5.0.0.8�@                  ��
            Dim isTdcs As Boolean = pPath.EndsWith(".tdcs") OrElse _
                ((MACHINE_KD_RS = giMachineKd) AndAlso (pPath.EndsWith(".tdcw")))   ' V4.0.0.0-28
#If False Then  'V5.0.0.8�@
            ' ���[�h�����v���[�g�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            r = Set_typPlateInfo(pData, strSECT, isTdcs)                ' V4.0.0.0-28
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            ' TY2���ް������݂��Ȃ��ꍇ�A������ް�����쐬����
            If ((r = cFRS_NORMAL) And (MaxTy2 = 0)) Then
                'r = GetTy2StepPos(1)
            End If

            '----- ###229�� -----
            ' GPIB�f�[�^��GPIB�f�[�^�\���̂֊i�[����
            r = Set_typGpibInfo(pGpibData, typGpibInfo)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            '----- ###229�� -----

            ' NG����߲�Ď擾
            If (r = cFRS_NORMAL) Then
                'SetNG_MarkingPos() ' 2011.10.14 �폜
            End If
#End If
            '----- V4.0.0.0-35�� -----
            ' SL436S���Ŋg���q��".tdc"�̏ꍇ�ɕϊ����s��
            If ((MACHINE_KD_RS = giMachineKd) And (isTdcs = False)) Then
                ConvertPlateData()
            End If
            'If (False = isTdcs) Then ConvertPlateData() ' V4.0.0.0-28
            '----- V4.0.0.0-35�� -----

            ' 'V5.0.0.8�B��
            SetToOrgData()
            ' 'V5.0.0.8�B��

            ' �����J������{����ݒ�(�t�@�C���ǂݍ��ݎ��͓����J�������\������Ă��邽�߁ADllVideo.dll���ŕ\���{�����ύX�����)  'V6.0.0.0�P 
            Form1.Instance.VideoLibrary1.StdMagnification = typPlateInfo.dblStdMagnification

            ' �I������
STP_EXIT:
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X���Đݒ肷��
            If (r = cFRS_NORMAL) Then
                Call Form1.Set_StartBlkComb()
            End If
            '----- V4.11.0.0�D�� -----

            Return (r)

STP_ERR:
            If (strSECT = "") Then
                If frmAutoObj.gbFgAutoOperation432 = False Then                 'V6.1.4.0�I
                    ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
                    'V6.1.4.0�I��
                Else
                    Call FormMain.Z_PRINT(MSG_16 & vbCrLf)
                End If
                'V6.1.4.0�I��
            Else
                ' "�t�@�C�����̓G���[ [�Z�N�V������]"
                strMSG = MSG_130 + " " + strSECT
                If frmAutoObj.gbFgAutoOperation432 = False Then                 'V6.1.4.0�I
                    Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, vbExclamation Or vbOKOnly, gAppName)
                    'V6.1.4.0�I��
                Else
                    Call FormMain.Z_PRINT(strMSG & vbCrLf)
                End If
                'V6.1.4.0�I��
            End If
            GoTo STP_EXIT

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.File_Read() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return�l = ��O�G���[
            GoTo STP_EXIT
        End Try

#If False Then
        Try
            ' ��������
            r = cFRS_NORMAL                                         ' Return�l = ����
            File_Read = 0
            Err_num = 0
            iFlg = 0
            Call Init_FileVer_Sub()                                 ' �����p̧���ް�ޮݐݒ�
            Call Init_AllTrmmingData()                              ' �O���[�o���f�[�^������(Plate/Circuit/Resistor/Cut)
            intType = -1
            rCnt = 0
            ReDim pData(MAX_PDATA)
            ReDim pGpibData(MAX_PGPIBDATA)                          ' ###229

            ' �e�L�X�g�t�@�C�����I�[�v������
            intFileNo = FreeFile()                                  ' �g�p�\�ȃt�@�C���i���o�[���擾
            FileOpen(intFileNo, pPath, OpenMode.Input)
            iFlg = 1

            ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ��܂��B
            Do While Not EOF(intFileNo)

                strDAT = LineInput(intFileNo)                       ' 1�s�ǂݍ���
                ' �f�[�^��ʔ���
                Select Case strDAT
                    Case FILE_CONST_VERSION                         ' �t�@�C���o�[�W����
                        strSECT = "[FILE VERSION]"
                        intType = SECT_VERSION
                    Case FILE_CONST_PLATE_01                        ' �v���[�g�f�[�^�P
                        strSECT = "[PLATE01]"
                        intType = SECT_PLATE01
                    Case FILE_CONST_PLATE_02                        ' �v���[�g�f�[�^�Q
                        strSECT = "[PLATE02]"
                        intType = SECT_PLATE02
                    Case FILE_CONST_PLATE_03                        ' �v���[�g�f�[�^�R
                        strSECT = "[PLATE03]"
                        intType = SECT_PLATE03
                    Case FILE_CONST_PLATE_04                        ' �v���[�g�f�[�^�S
                        strSECT = "[PLATE04]"
                        intType = SECT_PLATE04
                    Case FILE_CONST_PLATE_05                        ' �v���[�g�f�[�^�T
                        strSECT = "[PLATE05]"
                        intType = SECT_PLATE05
                        '------ V1.13.0.0�A�� ------
                    Case FILE_CONST_PLATE_06                        ' �v���[�g�f�[�^�U
                        strSECT = "[PLATE06]"
                        intType = SECT_PLATE06
                        '------ V1.13.0.0�A�� ------
                        'V5.0.0.6�@��
                    Case FILE_CONST_PLATE_OPTION                    ' �I�v�V����
                        strSECT = "[OPTION]"
                        intType = SECT_PLATE_OPTION
                        'V5.0.0.6�@��
                    Case FILE_CONST_CIR_DATA                        ' �T�[�L�b�g�f�[�^(TKY�p) 
                        strSECT = "[CIRCUIT]"
                        intType = SECT_CIRCUIT
                        rCnt = 1                                    ' �T�[�L�b�g�f�[�^�C���f�b�N�X������(1�ؼ��) 
                    Case FILE_CONST_STEPDATA                        ' STEP�f�[�^
                        strSECT = "[STEP]"
                        intType = SECT_STEP
                        rCnt = 1                                    ' STEP�f�[�^�C���f�b�N�X������(1�ؼ��) 
                    Case FILE_CONST_GRP_DATA                        ' �O���[�v�f�[�^
                        strSECT = "[GROUP]"
                        intType = SECT_GRP_DATA
                        rCnt = 1                                    ' �O���[�v�f�[�^�C���f�b�N�X������(1�ؼ��) 
                    Case FILE_CONST_TY2_DATA                        ' TY2�f�[�^
                        strSECT = "[TY2]"
                        intType = SECT_TY2_DATA
                        rCnt = 1                                    ' TY2�f�[�^�C���f�b�N�X������(1�ؼ��) 
                    Case FILE_CONST_CIRN_DATA                       ' CIRCUIT�f�[�^(NET�p)
                        strSECT = "[CIRCUIT AXIS]"
                        intType = SECT_CIR_AXIS
                        rCnt = 1                                    ' CIRCUIT�f�[�^�C���f�b�N�X������(1�ؼ��) 
                    Case FILE_CONST_CIRIDATA                        ' �T�[�L�b�g�ԃC���^�[�o���f�[�^(NET�p)
                        strSECT = "[CIRCUIT INTERVAL]"
                        intType = SECT_CIR_ITVL
                        rCnt = 1                                    ' �T�[�L�b�g�ԃC���^�[�o���f�[�^�C���f�b�N�X������(1�ؼ��) 
                        'Case CONST_IKEI_DATA                        ' �ٌ`�ʕt��(TKY�p) ���T�|�[�g 
                        '    intType = SECT_IKEI_DATA

                    Case FILE_CONST_RESISTOR                        ' ��R�f�[�^
                        strSECT = "[RESISTOR]]"
                        intType = SECT_REGISTOR
                        rCnt = 1                                    ' ��R�f�[�^�C���f�b�N�X������(1�ؼ��)
                    Case FILE_CONST_CUT_DATA                        ' �J�b�g�f�[�^
                        strSECT = "[CUT]]"
                        intType = SECT_CUT_DATA
                        rCnt = 0                                    ' ��R�f�[�^�C���f�b�N�X������(1�ؼ��)

                        '----- ###229�� -----
                    Case FILE_CONST_GPIB_DATA                       ' GPIB�f�[�^
                        strSECT = "[GPIB]"
                        intType = SECT_GPIB_DATA
                        rCnt = 0                                    ' GPIB�f�[�^�C���f�b�N�X������(1�ؼ��)
                        '----- ###229�� -----
                        'V1.13.0.0�D
                    Case FILE_SINSYUKU_SELECT
                        strSECT = "[SINSYUKU]"
                        intType = SECT_SINSYUKU_DATA
                        rCnt = 0                                    ' �L�k�␳�p�f�[�^�C���f�b�N�X������(1�ؼ��)
                        ClearBlockSelect()                                      'V1.13.0.0�D
                        'V1.13.0.0�D

                        ' �f�[�^(�Z�N�V�������ȊO)   
                    Case Else
                        ' ���[�h�����f�[�^���f�[�^�\���̂֊i�[����
                        r = Set_Trim_Data(intType, strDAT, rCnt)
                        If (r <> cFRS_NORMAL) Then                  ' �G���[�Ȃ�I��
                            Exit Do
                        End If
                End Select
            Loop

            Dim isTdcs As Boolean = pPath.EndsWith(".tdcs") OrElse _
                ((MACHINE_KD_RS = giMachineKd) AndAlso (pPath.EndsWith(".tdcw")))   ' V4.0.0.0-28

            ' ���[�h�����v���[�g�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            r = Set_typPlateInfo(pData, strSECT, isTdcs)                ' V4.0.0.0-28
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            ' TY2���ް������݂��Ȃ��ꍇ�A������ް�����쐬����
            If ((r = cFRS_NORMAL) And (MaxTy2 = 0)) Then
                'r = GetTy2StepPos(1)
            End If

            '----- ###229�� -----
            ' GPIB�f�[�^��GPIB�f�[�^�\���̂֊i�[����
            r = Set_typGpibInfo(pGpibData, typGpibInfo)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR
            '----- ###229�� -----

            ' NG����߲�Ď擾
            If (r = cFRS_NORMAL) Then
                'SetNG_MarkingPos() ' 2011.10.14 �폜
            End If

            '----- V4.0.0.0-35�� -----
            ' SL436S���Ŋg���q��".tdc"�̏ꍇ�ɕϊ����s��
            If ((MACHINE_KD_RS = giMachineKd) And (isTdcs = False)) Then
                ConvertPlateData()
            End If
            'If (False = isTdcs) Then ConvertPlateData() ' V4.0.0.0-28
            '----- V4.0.0.0-35�� -----

            ' �I������
STP_EXIT:
            If (iFlg = 1) Then
                FileClose(intFileNo)
            End If
            '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
            ' �g���~���O�J�n�u���b�N�ԍ��R���{�{�b�N�X���Đݒ肷��
            If (r = cFRS_NORMAL) Then
                Call Form1.Set_StartBlkComb()
            End If
            '----- V4.11.0.0�D�� -----
            Return (r)
            Exit Function

STP_ERR:
            If (strSECT = "") Then
                ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
            Else
                ' "�t�@�C�����̓G���[ [�Z�N�V������]"
                strMSG = MSG_130 + " " + strSECT
                Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, vbExclamation Or vbOKOnly, gAppName)
            End If
            GoTo STP_EXIT

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.File_Read() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return�l = ��O�G���[
            GoTo STP_EXIT
        End Try
#End If
    End Function
#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
#Region "���[�h�����f�[�^���f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''----------------------------------------------------------------------
    '''<summary>���[�h�����f�[�^���f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz</summary>
    '''<param name="intType">(INP)�f�[�^�^�C�v</param>
    '''<param name="strDAT"> (INP)�f�[�^</param>
    '''<param name="rCnt">   (I/O)�f�[�^�C���f�b�N�X</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''----------------------------------------------------------------------       
    Private Function Set_Trim_Data(ByVal intType As Integer, ByVal strDAT As String, ByRef rCnt As Integer,
                                   ByVal pData As List(Of String)) As Integer   'V5.0.0.9�A
        'Private Function Set_Trim_Data(ByVal intType As Integer, ByVal strDAT As String, ByRef rCnt As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            r = cFRS_NORMAL                                             ' Return�l = ����

            Select Case (intType)
                Case SECT_VERSION
                    ' �t�@�C���o�[�W�����f�[�^(�t�@�C���o�[�W������SL432HW_FileVer�ɐݒ�
                    If (gTkyKnd = KND_TKY) Then
                        r = FileVerCheck_TKY(strDAT)
                    ElseIf (gTkyKnd = KND_CHIP) Then
                        r = FileVerCheck_CHIP(strDAT)
                    Else
                        r = FileVerCheck_NET(strDAT)
                    End If

                Case SECT_PLATE01, SECT_PLATE02, SECT_PLATE03, SECT_PLATE04, SECT_PLATE05, SECT_PLATE06, SECT_PLATE_OPTION ' V1.13.0.0�A 'V5.0.0.6�@SECT_PLATE_OPTION ADD
                    ' �v���[�g�f�[�^1����6���v���[�g�f�[�^�i�[��֐ݒ肷��
                    'pData(rCnt) = strDAT                            ' �v���[�g�f�[�^�i�[(0�ؼ��)
                    pData.Add(strDAT)                               ' �v���[�g�f�[�^�i�[(0�ؼ��)     'V5.0.0.9�A
                    rCnt = rCnt + 1

                Case SECT_CIRCUIT
                    ' �T�[�L�b�g�f�[�^(TKY�p) 
                    ' �f�[�^���T�[�L�b�g�f�[�^�\���̂֊i�[����
                    'r = Set_typCircuitInfoArray(rCnt, strDAT)      ' V1.13.0.0�A
                    r = Set_typCircuitInfoArray(strDAT, rCnt)       ' V1.13.0.0�A

                Case SECT_STEP
                    ' �X�e�b�v�f�[�^(CHIP�p) ���X�e�b�v�f�[�^�\���̂֊i�[����
                    r = Set_typStepInfo(strDAT, rCnt)

                Case SECT_GRP_DATA
                    ' �O���[�v�f�[�^(CHIP�p)���O���[�v�f�[�^�\���̂֊i�[����
                    r = Set_typGrpInfoArrayChip(strDAT, rCnt)

                Case SECT_TY2_DATA
                    ' TY2�f�[�^��TY2�f�[�^�\���̂֊i�[����
                    r = Set_typTy2InfoArray(strDAT, rCnt)

                Case SECT_CIR_AXIS
                    ' �T�[�L�b�g���W�f�[�^(NET�p)���T�[�L�b�g���W�f�[�^�\���̂֊i�[����
                    r = Set_typCirAxisInfoArray(strDAT, rCnt)

                Case SECT_CIR_ITVL
                    ' �T�[�L�b�g�ԃC���^�[�o���f�[�^(NET�p)���T�[�L�b�g�ԃC���^�[�o���f�[�^�\���̂֊i�[����
                    r = Set_typCirInInfoArray(strDAT, rCnt)

                    ' �ٌ`�ʕt���f�[�^(TKY�p) ���T�|�[�g
                    ' Case SECT_IKEI_DATA
                    ' �f�[�^���ٌ`�ʕt���f�[�^�\���̂֊i�[����
                    ' r = Set_typIKEIInfo(strDAT)

                Case SECT_REGISTOR
                    ' ��R�f�[�^���R�f�[�^�\���̂֊i�[����
                    r = Get_RESIST_Data(strDAT, rCnt)

                Case SECT_CUT_DATA
                    ' �J�b�g�f�[�^���J�b�g�f�[�^�\���̂֊i�[����
                    r = Get_CUT_Data(strDAT, rCnt)

                    '----- ###229�� -----
                Case SECT_GPIB_DATA
                    pGpibData(rCnt) = strDAT                        ' GPIB�f�[�^��pGpibData�Ɋi�[(0�ؼ��)
                    rCnt = rCnt + 1
                    '----- ###229�� -----
                    'V1.13.0.0�D
                Case SECT_SINSYUKU_DATA
                    r = SetSinsyukuData(strDAT)
                    'V1.13.0.0�D
                Case Else
                    r = cFRS_FIOERR_INP                             ' Return�l = �G���[
            End Select
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_Trim_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h�����v���[�g�f�[�^���v���[�g�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    ''' <summary>���[�h�����v���[�g�f�[�^���v���[�g�f�[�^�\���̂֊i�[����</summary>
    ''' <param name="mDATA">  (INP)�f�[�^</param>
    ''' <param name="strSECT">(OUT)�G���[�ƂȂ����Z�N�V������</param>
    ''' <param name="isTdcs"> (INP)true:tdcş�قł���,false:tdcş�قł͂Ȃ� V4.0.0.0-28</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typPlateInfo(ByVal mDATA As List(Of String), ByRef strSECT As String, ByVal isTdcs As Boolean) As Integer  'V5.0.0.9�A
        'Private Function Set_typPlateInfo(ByVal mDATA() As String, ByRef strSECT As String, ByVal isTdcs As Boolean) As Integer

        Dim i As Integer
        Dim strWK() As String
        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE01]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[PLATE01]"
                i = 0
                .strDataName = mDATA(i) : i = i + 1                             ' �g���~���O�f�[�^��
                .intDirStepRepeat = Short.Parse(mDATA(i)) : i = i + 1           ' �ï�߁���߰�
                .intChipStepCnt = Short.Parse(mDATA(i)) : i = i + 1             ' �`�b�v�X�e�b�v��
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �v���[�g��X,Y��','�ŕ������Ď�o��
                .intPlateCntXDir = Short.Parse(strWK(0))                        ' �v���[�g���w
                .intPlateCntYDir = Short.Parse(strWK(1))                        ' �v���[�g�x
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ��ۯ���X,Y��','�ŕ������Ď�o��
                .intBlockCntXDir = Short.Parse(strWK(0))                        ' ��ۯ����w
                .intBlockCntYDir = Short.Parse(strWK(1))                        ' ��ۯ����x
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �v���[�g�ԊuX,Y��','�ŕ������Ď�o��
                .dblPlateItvXDir = Double.Parse(strWK(0))                       ' �v���[�g�Ԋu�w
                .dblPlateItvYDir = Double.Parse(strWK(1))                       ' �v���[�g�Ԋu�x
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �u���b�N�T�C�YX,Y��','�ŕ������Ď�o��
                .dblBlockSizeXDir = Double.Parse(strWK(0))                      ' �u���b�N�T�C�Y�w
                .dblBlockSizeYDir = Double.Parse(strWK(1))                      ' �u���b�N�T�C�Y�x
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ð��وʒu�̾��X,Y��','�ŕ������Ď�o��
                .dblTableOffsetXDir = Double.Parse(strWK(0))                    ' ð��وʒu�̾��X
                .dblTableOffsetYDir = Double.Parse(strWK(1))                    ' ð��وʒu�̾��Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �ްшʒu�̾��X,Y��','�ŕ������Ď�o��
                .dblBpOffSetXDir = Double.Parse(strWK(0))                       ' �ްшʒu�̾��X
                .dblBpOffSetYDir = Double.Parse(strWK(1))                       ' �ްшʒu�̾��X
                ' BP�̾�Ă̓��͂Ȃ��̏ꍇ�Að��ٵ̾�Ēl�̋t�����
                If (gSysPrm.stCTM.giBPOffsetInput <> 0) Then
                    .dblBpOffSetXDir = -1.0 * .dblTableOffsetXDir
                    .dblBpOffSetYDir = -1.0 * .dblTableOffsetYDir
                End If
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ��ެ�Ĉʒu�̾��X,Y��','�ŕ������Ď�o��(���g�p)
                .dblAdjOffSetXDir = Double.Parse(strWK(0))                      ' ��ެ�Ĉʒu�̾��X
                .dblAdjOffSetYDir = Double.Parse(strWK(1))                      ' ��ެ�Ĉʒu�̾��Y
                .intCurcuitCnt = Short.Parse(mDATA(i)) : i = i + 1              ' �T�[�L�b�g��
                .intNGMark = Short.Parse(mDATA(i)) : i = i + 1                  ' NGϰ�ݸ�
                .intDelayTrim = Short.Parse(mDATA(i)) : i = i + 1               ' �ިڲ���
                '----- V1.23.0.0�E�� -----
                ' �f�B���C�g�����Q�w�莞�ŁA�V�X�p���̃f�B���C�g�����Q�����Ȃ�u�f�B���C�g�������s���Ȃ��v�Ƃ���
                If (.intDelayTrim = 2) And (gSysPrm.stSPF.giDelayTrim2 = 0) Then
                    .intDelayTrim = 0
                End If
                '----- V1.23.0.0�E�� -----
                .intNgJudgeUnit = Short.Parse(mDATA(i)) : i = i + 1             ' �m�f����P��
                .intNgJudgeLevel = Short.Parse(mDATA(i)) : i = i + 1            ' �m�f����
                '----- V4.0.0.0-28 �� -----
                'If (True = isTdcs) Then                                        ' V4.0.0.0-35
                If ((isTdcs = True) Or (giMachineKd <> MACHINE_KD_RS)) Then     ' V4.0.0.0-35
                    .dblZOffSet = Double.Parse(mDATA(i)) : i = i + 1            ' ZON�ʒu
                    .dblZStepUpDist = Double.Parse(mDATA(i)) : i = i + 1        ' �ï�ߏ㏸����
                    .dblZWaitOffset = Double.Parse(mDATA(i)) : i = i + 1        ' ZOFF�ʒu
                Else
                    .dblZOffSet = Z_ON_POS_SIMPLE : i = i + 1                   ' ZON�ʒu
                    .dblZStepUpDist = Z_STEP_POS_SIMPLE : i = i + 1             ' �ï�ߏ㏸����
                    .dblZWaitOffset = Z_OFF_POS_SIMPLE : i = i + 1              ' ZOFF�ʒu
                End If
                '----- V4.0.0.0-28 �� -----
                '----- V1.13.0.0�A�� -----
                If (SL432HW_FileVer >= 10.04) Then
                    .dblLwPrbStpDwDist = Double.Parse(mDATA(i)) : i = i + 1     ' ������۰�޽ï�߉��~����
                    .dblLwPrbStpUpDist = Double.Parse(mDATA(i)) : i = i + 1     ' ������۰�޽ï�ߏ㏸����
                Else
                    .dblLwPrbStpDwDist = 0.0                                    ' ������۰�޽ï�߉��~����(�����l)
                    .dblLwPrbStpUpDist = 0.0                                    ' ������۰�޽ï�ߏ㏸����(�����l)
                End If
                '----- V1.13.0.0�A�� -----
                '----- V4.0.0.0�C�� -----
                ' �v���[�u���g���C��(0=���g���C�Ȃ�)(�I�v�V����)
                If (SL432HW_FileVer >= 10.1) Then
                    .intPrbRetryCount = Short.Parse(mDATA(i)) : i = i + 1       ' �v���[�u���g���C��(0=���g���C�Ȃ�)
                Else
                    .intPrbRetryCount = 0                                       ' �v���[�u���g���C��(0=���g���C�Ȃ�)
                End If
                '----- V4.0.0.0�C�� -----
                '----- V1.23.0.0�F�� -----
                ' �v���[�u�`�F�b�N����(�I�v�V����)
                If (SL432HW_FileVer >= 10.09) Then
                    .intPrbChkPlt = Short.Parse(mDATA(i)) : i = i + 1           ' ����
                    .intPrbChkBlk = Short.Parse(mDATA(i)) : i = i + 1           ' �u���b�N
                    .dblPrbTestLimit = Double.Parse(mDATA(i)) : i = i + 1       ' �덷�}%
                End If
                If ((SL432HW_FileVer < 10.09) Or (giProbeCheck = 0)) Then
                    .intPrbChkPlt = 0                                           ' ����
                    .intPrbChkBlk = 1                                           ' �u���b�N
                    .dblPrbTestLimit = 0.0                                      ' �덷�}%
                End If
                '----- V1.23.0.0�F�� -----

                ' [PLATE03]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[PLATE03]"
                .intResistDir = Short.Parse(mDATA(i)) : i = i + 1               ' ��R���ѕ���
                .intCircuitCntInBlock = Short.Parse(mDATA(i)) : i = i + 1       ' 1��ۯ��໰��Đ� 
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ����Ļ���X,Y��','�ŕ������Ď�o��
                .dblCircuitSizeXDir = Double.Parse(strWK(0))                    ' ����Ļ���X  
                .dblCircuitSizeYDir = Double.Parse(strWK(1))                    ' ����Ļ���Y 
                .intResistCntInBlock = Short.Parse(mDATA(i)) : i = i + 1        ' 1��ۯ�����R��
                .intResistCntInGroup = Short.Parse(mDATA(i)) : i = i + 1        ' 1��ٰ�ߓ���R��
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ��ۯ����ٰ�ߐ�X,Y��','�ŕ������Ď�o��
                .intGroupCntInBlockXBp = Short.Parse(strWK(0))                  ' ��ٰ�ߐ�X(�a�o�O���[�v��)
                .intGroupCntInBlockYStage = Short.Parse(strWK(1))               ' ��ٰ�ߐ�Y(�X�e�[�W�O���[�v��)
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �X�e�[�W�O���[�v���u���b�N��X,Y��','�ŕ������Ď�o��
                .intBlkCntInStgGrpX = Short.Parse(strWK(0))                     ' �X�e�[�W�O���[�v���u���b�N��X
                .intBlkCntInStgGrpY = Short.Parse(strWK(1))                     ' �X�e�[�W�O���[�v���u���b�N��Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ��ٰ�ߊԊuBP(X),Stage(Y) ��','�ŕ������Ď�o��
                .dblBpGrpItv = Double.Parse(strWK(0))                           ' ��ٰ�ߊԊuX
                .dblStgGrpItvY = Double.Parse(strWK(1))                         ' ��ٰ�ߊԊuY(Dummy)
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ���߻���X,Y��','�ŕ������Ď�o��
                .dblChipSizeXDir = Double.Parse(strWK(0))                       ' ���߻���X
                .dblChipSizeYDir = Double.Parse(strWK(1))                       ' ���߻���Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �ï�ߵ̾�ė�X,Y��','�ŕ������Ď�o��
                .dblStepOffsetXDir = Double.Parse(strWK(0))                     ' �ï�ߵ̾�ė�X
                .dblStepOffsetYDir = Double.Parse(strWK(1))                     ' �ï�ߵ̾�ė�Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ��ۯ����ޕ␳X,Y��','�ŕ������Ď�o��
                .dblBlockSizeReviseXDir = Double.Parse(strWK(0))                ' ��ۯ����ޕ␳X
                .dblBlockSizeReviseYDir = Double.Parse(strWK(1))                ' ��ۯ����ޕ␳Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ��ۯ��ԊuX,Y��','�ŕ������Ď�o��
                .dblBlockItvXDir = Double.Parse(strWK(0))                       ' ��ۯ��ԊuX
                .dblBlockItvYDir = Double.Parse(strWK(1))                       ' ��ۯ��ԊuY
                .intContHiNgBlockCnt = Short.Parse(mDATA(i)) : i = i + 1        ' �A��NG-HIGH��R��ۯ���
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �v���[�g�T�C�YX,Y��','�ŕ������Ď�o��
                .dblPlateSizeX = Double.Parse(strWK(0))                         ' �v���[�g�T�C�YX 
                .dblPlateSizeY = Double.Parse(strWK(1))                         ' �v���[�g�T�C�YY
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �X�e�[�W�O���[�v�ԊuX,Y��','�ŕ������Ď�o��
                .dblStgGrpItvX = Double.Parse(strWK(0))                         ' �X�e�[�W�O���[�v�ԊuX
                .dblStgGrpItvY = Double.Parse(strWK(1))                         ' �X�e�[�W�O���[�v�ԊuY

                ' [PLATE02]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[PLATE02]"
                .intReviseMode = Short.Parse(mDATA(i)) : i = i + 1              ' �␳���[�h
                .intManualReviseType = Short.Parse(mDATA(i)) : i = i + 1        ' �␳���@
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �␳�ʒu���W1X,Y��','�ŕ������Ď�o��
                .dblReviseCordnt1XDir = Double.Parse(strWK(0))                  ' �␳�ʒu���W1X
                .dblReviseCordnt1YDir = Double.Parse(strWK(1))                  ' �␳�ʒu���W1Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �␳�ʒu���W2X,Y��','�ŕ������Ď�o��
                .dblReviseCordnt2XDir = Double.Parse(strWK(0))                  ' �␳�ʒu���W2X
                .dblReviseCordnt2YDir = Double.Parse(strWK(1))                  ' �␳�ʒu���W2Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �␳�߼޼�ݵ̾��X,Y��','�ŕ������Ď�o��
                .dblReviseOffsetXDir = Double.Parse(strWK(0))                   ' �␳�߼޼�ݵ̾��X
                .dblReviseOffsetYDir = Double.Parse(strWK(1))                   ' �␳�߼޼�ݵ̾��Y
                .intRecogDispMode = Short.Parse(mDATA(i)) : i = i + 1           ' �F���f�[�^�\�����[�h
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �߸�ْlX,Y��','�ŕ������Ď�o��
                .dblPixelValXDir = Double.Parse(strWK(0))                       ' �߸�ْlX
                .dblPixelValYDir = Double.Parse(strWK(1))                       ' �߸�ْlY
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �␳�ʒu�����No1,2��','�ŕ������Ď�o��
                .intRevisePtnNo1 = Short.Parse(strWK(0))                        ' �␳�ʒu�����No1
                .intRevisePtnNo2 = Short.Parse(strWK(1))                        ' �␳�ʒu�����No2
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �␳�ʒu����ݸ�ٰ��No1,2��','�ŕ������Ď�o��
                .intRevisePtnNo1GroupNo = Short.Parse(strWK(0))                 ' �␳�ʒu����ݸ�ٰ��No1
                .intRevisePtnNo2GroupNo = Short.Parse(strWK(1))                 ' �␳�ʒu����ݸ�ٰ��No2
                .dblRotateTheta = Double.Parse(mDATA(i)) : i = i + 1            ' �Ɖ�]�p�x '###037
                .dblTThetaOffset = Double.Parse(mDATA(i)) : i = i + 1           ' �s�ƃI�t�Z�b�g
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �s�Ɗ�ʒu1X,Y��','�ŕ������Ď�o��
                .dblTThetaBase1XDir = Double.Parse(strWK(0))                    ' �s�Ɗ�ʒu1X
                .dblTThetaBase1YDir = Double.Parse(strWK(1))                    ' �s�Ɗ�ʒu1Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' �s�Ɗ�ʒu2X,Y��','�ŕ������Ď�o��
                .dblTThetaBase2XDir = Double.Parse(strWK(0))                    ' �s�Ɗ�ʒu2X
                .dblTThetaBase2YDir = Double.Parse(strWK(1))                    ' �s�Ɗ�ʒu2Y
                'V5.0.0.9�A                                          �� ���t�A���C�����g�p
                If (FILE_VER_10_12 <= SL432HW_FileVer) Then
                    .intReviseExecRgh = Short.Parse(mDATA(i)) : i = i + 1       ' �␳�L��(0:�␳�Ȃ�, 1:�␳����)
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .dblReviseCordnt1XDirRgh = Double.Parse(strWK(0))           ' �␳�ʒu���W1X
                    .dblReviseCordnt1YDirRgh = Double.Parse(strWK(1))           ' �␳�ʒu���W1Y
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .dblReviseCordnt2XDirRgh = Double.Parse(strWK(0))           ' �␳�ʒu���W2X
                    .dblReviseCordnt2YDirRgh = Double.Parse(strWK(1))           ' �␳�ʒu���W2Y
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .dblReviseOffsetXDirRgh = Double.Parse(strWK(0))            ' �␳�߼޼�ݵ̾��X
                    .dblReviseOffsetYDirRgh = Double.Parse(strWK(1))            ' �␳�߼޼�ݵ̾��Y
                    .intRecogDispModeRgh = Short.Parse(mDATA(i)) : i = i + 1    ' �F���ް��\��Ӱ��(0:�\���Ȃ�, 1:�\������)
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .intRevisePtnNo1Rgh = Short.Parse(strWK(0))                 ' �␳�ʒu�����No1
                    .intRevisePtnNo2Rgh = Short.Parse(strWK(1))                 ' �␳�ʒu�����No2
                    strWK = mDATA(i).Split(",") : i = i + 1
                    .intRevisePtnNo1GroupNoRgh = Short.Parse(strWK(0))          ' �␳�ʒu�����No1�O���[�vNo
                    .intRevisePtnNo2GroupNoRgh = Short.Parse(strWK(1))          ' �␳�ʒu�����No2�O���[�vNo
                End If
                'V5.0.0.9�A                                          ��
                ' [PLATE04]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[PLATE04]"
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ���WX,Y��','�ŕ������Ď�o��
                .dblCaribBaseCordnt1XDir = Double.Parse(strWK(0))               ' �����ڰ��݊���W1X
                .dblCaribBaseCordnt1YDir = Double.Parse(strWK(1))               ' �����ڰ��݊���W1Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ���WX,Y��','�ŕ������Ď�o��
                .dblCaribBaseCordnt2XDir = Double.Parse(strWK(0))               ' �����ڰ��݊���W2X
                .dblCaribBaseCordnt2YDir = Double.Parse(strWK(1))               ' �����ڰ��݊���W2Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ���WX,Y��','�ŕ������Ď�o��
                .dblCaribTableOffsetXDir = Double.Parse(strWK(0))               ' �����ڰ���ð��ٵ̾��X
                .dblCaribTableOffsetYDir = Double.Parse(strWK(1))               ' �����ڰ���ð��ٵ̾��Y
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ����ݓo�^No1,2��','�ŕ������Ď�o��
                .intCaribPtnNo1 = Short.Parse(strWK(0))                         ' ����ݓo�^No1
                .intCaribPtnNo2 = Short.Parse(strWK(1))                         ' ����ݓo�^No2
                strWK = mDATA(i).Split(",") : i = i + 1                         ' ����ݸ�ٰ��No1,2��','�ŕ������Ď�o��
                .intCaribPtnNo1GroupNo = Short.Parse(strWK(0))                  ' ����ݸ�ٰ��No1
                .intCaribPtnNo2GroupNo = Short.Parse(strWK(1))                  ' ����ݸ�ٰ��No2
                .dblCaribCutLength = Double.Parse(mDATA(i)) : i = i + 1         ' �����ڰ��ݶ�Ē�
                .dblCaribCutSpeed = Double.Parse(mDATA(i)) : i = i + 1          ' �����ڰ��ݶ�đ��x
                .dblCaribCutQRate = Double.Parse(mDATA(i)) : i = i + 1          ' �����ڰ��ݶ��Qڰ�
                .intCaribCutCondNo = Short.Parse(mDATA(i)) : i = i + 1          ' �����ڰ��݉��H�����ԍ�(FL�p)

                strWK = mDATA(i).Split(",") : i = i + 1                         ' ���WX,Y��','�ŕ������Ď�o��
                .dblCutPosiReviseOffsetXDir = Double.Parse(strWK(0))            ' ��ē_�␳ð��ٵ̾��X
                .dblCutPosiReviseOffsetYDir = Double.Parse(strWK(1))            ' ��ē_�␳ð��ٵ̾��Y
                .intCutPosiRevisePtnNo = Short.Parse(mDATA(i)) : i = i + 1      ' ��ē_�␳����ݓo�^No
                .dblCutPosiReviseCutLength = Double.Parse(mDATA(i)) : i = i + 1 ' ��ē_�␳��Ē�
                .dblCutPosiReviseCutSpeed = Double.Parse(mDATA(i)) : i = i + 1  ' ��ē_�␳��đ��x
                .dblCutPosiReviseCutQRate = Double.Parse(mDATA(i)) : i = i + 1  ' ��ē_�␳ڰ��Qڰ�
                .intCutPosiReviseGroupNo = Short.Parse(mDATA(i)) : i = i + 1    ' ��ē_�␳����ݸ�ٰ��No
                .intCutPosiReviseCondNo = Short.Parse(mDATA(i)) : i = i + 1     ' �J�b�g�ʒu�␳���H�����ԍ�(FL�p)

                ' [PLATE05]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[PLATE05]"
                .intMaxTrimNgCount = Short.Parse(mDATA(i)) : i = i + 1          ' ���ݸ�NG����(���)
                .intMaxBreakDischargeCount = Short.Parse(mDATA(i)) : i = i + 1  ' ���ꌇ���r�o����(���)
                .intTrimNgCount = Short.Parse(mDATA(i)) : i = i + 1             ' �A�����ݸ�NG����
                If (SL432HW_FileVer >= 10.02) Then
                    .intContHiNgResCnt = Short.Parse(mDATA(i)) : i = i + 1      ' �A�����ݸ�NG��R��    ###230
                Else
                    .intContHiNgResCnt = 0
                End If
                .intRetryProbeCount = Short.Parse(mDATA(i)) : i = i + 1         ' ����۰��ݸމ�
                .dblRetryProbeDistance = Double.Parse(mDATA(i)) : i = i + 1     ' ����۰��ݸވړ���
                .intWorkSetByLoader = Short.Parse(mDATA(i)) : i = i + 1         ' ��i��
                .intOpenCheck = Short.Parse(mDATA(i)) : i = i + 1               ' 4�[�q���������
                .intLedCtrl = Short.Parse(mDATA(i)) : i = i + 1                 ' LED����
                '                                                               ' ����ڰ����ܰ��������
                '----- V4.0.0.0-28 �� 'V4.4.0.0�C-----
                If (giMachineKd = MACHINE_KD_RS) Then
                    If (OSCILLATOR_FL = gSysPrm.stRAT.giOsc_Res) Then
                        .intPowerAdjustMode = Short.Parse(mDATA(i)) : i = i + 1     ' ��ܰ����Ӱ��
                    Else
                        ' FL�ł͂Ȃ��ꍇ�A�������������Ȃ��ɐݒ肷��
                        .intPowerAdjustMode = 0 : i = i + 1                         ' ��ܰ����Ӱ��
                    End If
                Else
                    .intPowerAdjustMode = Short.Parse(mDATA(i)) : i = i + 1     ' ��ܰ����Ӱ��
                End If
                '----- V4.0.0.0-28 �� 'V4.4.0.0�C-----
                .dblPowerAdjustTarget = Double.Parse(mDATA(i)) : i = i + 1      ' �����ڕW��ܰ
                .dblPowerAdjustQRate = Double.Parse(mDATA(i)) : i = i + 1       ' ��ܰ����Qڰ�
                .dblPowerAdjustToleLevel = Double.Parse(mDATA(i)) : i = i + 1   ' ��ܰ�������e�͈�
                .intPowerAdjustCondNo = Short.Parse(mDATA(i)) : i = i + 1       ' ��ܰ�������H�����ԍ�(FL�p)�@
                '                                                               ' GP-IB����
                .intGpibCtrl = Short.Parse(mDATA(i)) : i = i + 1                ' GP-IB����
                .intGpibDefDelimiter = Short.Parse(mDATA(i)) : i = i + 1        ' �����ݒ�(�����)(�Œ�)
                .intGpibDefTimiout = Short.Parse(mDATA(i)) : i = i + 1          ' �����ݒ�(��ѱ��) (�Œ�)
                .intGpibDefAdder = Short.Parse(mDATA(i)) : i = i + 1            ' �����ݒ�(�@����ڽ)
                .strGpibInitCmnd1 = mDATA(i) : i = i + 1                        ' �����������1
                .strGpibInitCmnd2 = mDATA(i) : i = i + 1                        ' �����������2
                .strGpibTriggerCmnd = mDATA(i) : i = i + 1                      ' �ض޺���� (�Œ�)
                .intGpibMeasSpeed = Short.Parse(mDATA(i)) : i = i + 1           ' ���葬�x(0:�ᑬ, 1:����)
                .intGpibMeasMode = Short.Parse(mDATA(i)) : i = i + 1            ' ���胂�[�h(0:���, 1:�΍�)
                '----- V1.13.0.0�A�� -----
                If (SL432HW_FileVer >= 10.04) Then
                    .intNgJudgeStop = Short.Parse(mDATA(i)) : i = i + 1         ' NG���莞��~
                Else
                    .intNgJudgeStop = 0                                         ' NG���莞��~(�����l)
                End If

                '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
                If (SL432HW_FileVer >= 10.11) Then                              ' �t�@�C���o�[�W������10.11�ȏ�̎��ɐݒ肷��
                    .intPwrChkPltNum = Short.Parse(mDATA(i)) : i = i + 1        ' �I�[�g�p���[�`�F�b�N�����
                    .intPwrChkTime = Short.Parse(mDATA(i)) : i = i + 1          ' �I�[�g�p���[�`�F�b�N����(��) 
                Else
                    .intPwrChkPltNum = 0                                        ' �I�[�g�p���[�`�F�b�N�����
                    .intPwrChkTime = 0                                          ' �I�[�g�p���[�`�F�b�N����(��) 
                End If
                '----- V4.11.0.0�@�� -----

                ' [PLATE06]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[PLATE06]"
                If (SL432HW_FileVer >= 10.04) Then
                    .intContExpMode = Short.Parse(mDATA(i)) : i = i + 1         ' �L�k�␳ (0:�Ȃ�, 1:����)
                    .intContExpGrpNo = Short.Parse(mDATA(i)) : i = i + 1        ' �L�k�␳��ٰ�ߔԍ�
                    .intContExpPtnNo = Short.Parse(mDATA(i)) : i = i + 1        ' �L�k�␳����ݔԍ�
                    .dblContExpPosX = Double.Parse(mDATA(i)) : i = i + 1        ' �L�k�␳�ʒuX (mm)
                    .dblContExpPosY = Double.Parse(mDATA(i)) : i = i + 1        ' �L�k�␳�ʒuXY (mm)            
                    .intStepMeasCnt = Short.Parse(mDATA(i)) : i = i + 1         ' �ï�ߑ����
                    .dblStepMeasPitch = Double.Parse(mDATA(i)) : i = i + 1      ' �ï�ߑ����߯�
                    .intStepMeasReptCnt = Short.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ���J��Ԃ��ï�߉�
                    .dblStepMeasReptPitch = Double.Parse(mDATA(i)) : i = i + 1  ' �ï�ߑ���J��Ԃ��ï���߯�
                    .intStepMeasLwGrpNo = Short.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ��艺����۰�޸�ٰ�ߔԍ�
                    .intStepMeasLwPtnNo = Short.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ��艺����۰������ݔԍ�
                    .dblStepMeasBpPosX = Double.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ���BP�ʒuX
                    .dblStepMeasBpPosY = Double.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ���BP�ʒuY
                    .intStepMeasUpGrpNo = Short.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ�������۰�޸�ٰ�ߔԍ�
                    .intStepMeasUpPtnNo = Short.Parse(mDATA(i)) : i = i + 1     ' �ï�ߑ�������۰������ݔԍ�
                    .dblStepMeasTblOstX = Double.Parse(mDATA(i)) : i = i + 1    ' �ï�ߑ�������۰��ð��ٵ̾��X
                    .dblStepMeasTblOstY = Double.Parse(mDATA(i)) : i = i + 1    ' �ï�ߑ�������۰��ð��ٵ̾��Y
                    .intIDReaderUse = Short.Parse(mDATA(i)) : i = i + 1         ' IDذ�� (0:���g�p, 1:�g�p)
                    .dblIDReadPos1X = Double.Parse(mDATA(i)) : i = i + 1        ' IDذ�ޓǂݎ���߼޼�� 1X
                    .dblIDReadPos1Y = Double.Parse(mDATA(i)) : i = i + 1        ' IDذ�ޓǂݎ���߼޼�� 1Y
                    .dblIDReadPos2X = Double.Parse(mDATA(i)) : i = i + 1        ' IDذ�ޓǂݎ���߼޼�� 2X
                    .dblIDReadPos2Y = Double.Parse(mDATA(i)) : i = i + 1        ' IDذ�ޓǂݎ���߼޼�� 2Y
                    .dblReprobeVar = Double.Parse(mDATA(i)) : i = i + 1         ' ����۰��ݸނ΂����
                    .dblReprobePitch = Double.Parse(mDATA(i)) : i = i + 1       ' ����۰��ݸ��߯�
                Else
                    .intContExpMode = 0                                         ' �L�k�␳ (0:�Ȃ�, 1:����)
                    .intContExpGrpNo = 5                                        ' �L�k�␳��ٰ�ߔԍ�
                    .intContExpPtnNo = 1                                        ' �L�k�␳����ݔԍ�
                    .dblContExpPosX = 0.0                                       ' �L�k�␳�ʒuX (mm)
                    .dblContExpPosY = 0.0                                       ' �L�k�␳�ʒuXY (mm)            
                    .intStepMeasCnt = 0                                         ' �ï�ߑ����
                    .dblStepMeasPitch = 0.0                                     ' �ï�ߑ����߯�
                    .intStepMeasReptCnt = 0                                     ' �ï�ߑ���J��Ԃ��ï�߉�
                    .dblStepMeasReptPitch = 0.0                                 ' �ï�ߑ���J��Ԃ��ï���߯�
                    .intStepMeasLwGrpNo = 6                                     ' �ï�ߑ��艺����۰�޸�ٰ�ߔԍ�
                    .intStepMeasLwPtnNo = 1                                     ' �ï�ߑ��艺����۰������ݔԍ�
                    .dblStepMeasBpPosX = 0.0                                    ' �ï�ߑ���BP�ʒuX
                    .dblStepMeasBpPosY = 0.0                                    ' �ï�ߑ���BP�ʒuY
                    .intStepMeasUpGrpNo = 6                                     ' �ï�ߑ�������۰�޸�ٰ�ߔԍ�
                    .intStepMeasUpPtnNo = 2                                     ' �ï�ߑ�������۰������ݔԍ�
                    .dblStepMeasTblOstX = 0.0                                   ' �ï�ߑ�������۰��ð��ٵ̾��X
                    .dblStepMeasTblOstY = 0.0                                   ' �ï�ߑ�������۰��ð��ٵ̾��Y
                    .intIDReaderUse = 0                                         ' IDذ�� (0:���g�p, 1:�g�p)
                    .dblIDReadPos1X = 0.0                                       ' IDذ�ޓǂݎ���߼޼�� 1X
                    .dblIDReadPos1Y = 0.0                                       ' IDذ�ޓǂݎ���߼޼�� 1Y
                    .dblIDReadPos2X = 0.0                                       ' IDذ�ޓǂݎ���߼޼�� 2X
                    .dblIDReadPos2Y = 0.0                                       ' IDذ�ޓǂݎ���߼޼�� 2Y
                    .dblReprobeVar = 0.0                                        ' ����۰��ݸނ΂����
                    .dblReprobePitch = 0.0                                      ' ����۰��ݸ��߯�
                End If
                '----- V1.13.0.0�A�� -----
                '----- V2.0.0.0_24�� -----
                '----- �v���[�u�N���[�j���O���� -----
                If (SL432HW_FileVer >= FILE_VER_10_10) Then
                    .dblPrbCleanPosX = Double.Parse(mDATA(i)) : i = i + 1       ' �N���[�j���O�ʒuX
                    .dblPrbCleanPosY = Double.Parse(mDATA(i)) : i = i + 1       ' �N���[�j���O�ʒuY
                    .dblPrbCleanPosZ = Double.Parse(mDATA(i)) : i = i + 1       ' �N���[�j���O�ʒuZ
                    .intPrbCleanUpDwCount = Short.Parse(mDATA(i)) : i = i + 1   ' �v���[�u�㉺��
                    .intPrbCleanAutoSubCount = Short.Parse(mDATA(i)) : i = i + 1 '  �����^�]���N���[�j���O���s�����
                Else
                    .dblPrbCleanPosX = 0.0                                      ' �N���[�j���O�ʒuX
                    .dblPrbCleanPosY = 0.0                                      ' �N���[�j���O�ʒuY
                    .dblPrbCleanPosZ = 0.0                                      ' �N���[�j���O�ʒuZ
                    .intPrbCleanUpDwCount = 0                                   ' �v���[�u�㉺��
                    .intPrbCleanAutoSubCount = 0                                '  �����^�]���N���[�j���O���s�����
                End If
                '----- V2.0.0.0_24�� -----

                'V5.0.0.6�@��
                ' [OPTIN]�f�[�^���v���[�g�f�[�^�\���̂֊i�[����
                strSECT = "[OPTION]"
                If gbControllerInterlock Then
                    If Integer.TryParse(mDATA(i), .intControllerInterlock) Then
                    Else
                        .intControllerInterlock = 0
                    End If
                End If
                i = i + 1
                'V5.0.0.6�@��

                'V4.5.1.0�N              ��
                If (FILE_VER_10_11 <= SL432HW_FileVer) Then
                    .dblTXChipsizeRelationX = If(String.IsNullOrEmpty(mDATA(i)), 0.0, Double.Parse(mDATA(i))) : i = i + 1 ' �␳�ʒu�P�ƂQ�̑��Βl�w
                    .dblTXChipsizeRelationY = If(String.IsNullOrEmpty(mDATA(i)), 0.0, Double.Parse(mDATA(i))) : i = i + 1 ' �␳�ʒu�P�ƂQ�̑��Βl�x
                Else
                    .dblTXChipsizeRelationX = 0.0                               ' �␳�ʒu�P�ƂQ�̑��Βl�w
                    .dblTXChipsizeRelationY = 0.0                               ' �␳�ʒu�P�ƂQ�̑��Βl�x
                End If
                'V4.5.1.0�N              ��

                'V4.10.0.0�C            ��
                'If (FILE_VER_10_11 <= SL432HW_FileVer) Then
                '    .dblPrbCleanStagePitchX = Double.Parse(mDATA(i)) : i += 1   ' �X�e�[�W����s�b�`X
                '    .dblPrbCleanStagePitchY = Double.Parse(mDATA(i)) : i += 1   ' �X�e�[�W����s�b�`Y
                '    .intPrbCleanStageCountX = Short.Parse(mDATA(i)) : i += 1    ' �X�e�[�W�����X
                '    .intPrbCleanStageCountY = Short.Parse(mDATA(i)) : i += 1    ' �X�e�[�W�����Y
                '    .dblPrbDistance = Double.Parse(mDATA(i)) : i += 1           ' �v���[�u�ԋ����imm�j'V4.10.0.0�H
                '    .dblPrbCleaningOffset = Double.Parse(mDATA(i)) : i += 1     ' �N���[�j���O�I�t�Z�b�g(mm)'V4.10.0.0�H
                'Else
                '    .dblPrbCleanStagePitchX = 0.0                               ' �X�e�[�W����s�b�`X
                '    .dblPrbCleanStagePitchY = 0.0                               ' �X�e�[�W����s�b�`Y
                '    .intPrbCleanStageCountX = 0                                 ' �X�e�[�W�����X
                '    .intPrbCleanStageCountY = 0                                 ' �X�e�[�W�����Y
                '    .dblPrbDistance = 0.0                                       ' �v���[�u�ԋ����imm�j'V4.10.0.0�H
                '    .dblPrbCleaningOffset = 0.0                                 ' �N���[�j���O�I�t�Z�b�g(mm)'V4.10.0.0�H
                'End If
                'V4.10.0.0�C            ��

            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typPlateInfo() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)                                                     ' Call���Ń��b�Z�[�W�\������̂ō폜
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h�����X�e�b�v�f�[�^���X�e�b�v�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h�����X�e�b�v�f�[�^���X�e�b�v�f�[�^�\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)�X�e�b�v�f�[�^�\���̂̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typStepInfo(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' �X�e�b�v�f�[�^�\���̂փf�[�^���i�[����
            With typStepInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' �������','�ŕ������Ď�o�� 
                .intSP1 = Short.Parse(mDATA(i)) : i = i + 1         ' �ï�ߔԍ�
                .intSP2 = Short.Parse(mDATA(i)) : i = i + 1         ' ��ۯ���
                .dblSP3 = Double.Parse(mDATA(i)) : i = i + 1        ' �ï�ߊԲ������
            End With
            MaxStep = rCnt                                          ' �X�e�b�v�f�[�^����
            rCnt = rCnt + 1

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typStepInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h�����O���[�v�f�[�^���O���[�v�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h�����O���[�v�f�[�^���O���[�v�f�[�^�\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)�O���[�v�f�[�^�\���̂̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typGrpInfoArrayChip(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' �O���[�v�f�[�^�\���̂փf�[�^���i�[����
            With typGrpInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' �������','�ŕ������Ď�o�� 
                .intGP1 = Short.Parse(mDATA(i)) : i = i + 1         ' ��ٰ�ߔԍ�
                .intGP2 = Short.Parse(mDATA(i)) : i = i + 1         ' ��R��
                .dblGP3 = Double.Parse(mDATA(i)) : i = i + 1        ' ��ٰ�ߊԲ������
            End With
            MaxGrp = rCnt                                           ' ��ٰ�ߌ���
            rCnt = rCnt + 1

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typGrpInfoArrayChip() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h����TY2�f�[�^��TY2�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h����TY2�f�[�^��TY2�f�[�^�\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)TY2�f�[�^�\���̂̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typTy2InfoArray(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' TY2�f�[�^�\���̂փf�[�^���i�[����
            With typTy2InfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' �������','�ŕ������Ď�o�� 
                .intTy21 = Short.Parse(mDATA(i)) : i = i + 1        ' ��ۯ��ԍ�
                .dblTy22 = Double.Parse(mDATA(i)) : i = i + 1       ' �ï�߲������
            End With
            If (typTy2InfoArray(rCnt).intTy21 <> 0) Then
                MaxTy2 = rCnt                                       ' Ty2��ۯ�����
            End If
            rCnt = rCnt + 1

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typTy2InfoArray() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h�����T�[�L�b�g���W�f�[�^(NET�p)���T�[�L�b�g���W�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h�����T�[�L�b�g���W�f�[�^(NET�p)���T�[�L�b�g���W�f�[�^�\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)�T�[�L�b�g���W�f�[�^�̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typCirAxisInfoArray(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' �T�[�L�b�g���W�f�[�^�\���̂փf�[�^���i�[����
            With typCirAxisInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' �������','�ŕ������Ď�o�� 
                .intCaP1 = Short.Parse(mDATA(i)) : i = i + 1        ' �ï�ߔԍ�
                .dblCaP2 = Double.Parse(mDATA(i)) : i = i + 1       ' ���WX
                .dblCaP3 = Double.Parse(mDATA(i)) : i = i + 1       ' ���WY
            End With
            rCnt = rCnt + 1                                         ' �C���f�b�N�X�X�V

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typCirAxisInfoArray() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h�����T�[�L�b�g�ԃC���^�[�o���f�[�^(NET�p)���T�[�L�b�g�ԃC���^�[�o���f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h�����T�[�L�b�g�ԃC���^�[�o���f�[�^(NET�p)���T�[�L�b�g�ԃC���^�[�o���f�[�^�\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)�T�[�L�b�g���W�f�[�^�̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typCirInInfoArray(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' �T�[�L�b�g�ԃC���^�[�o���f�[�^�\���̂փf�[�^���i�[����
            With typCirInInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                            ' �������','�ŕ������Ď�o�� 
                .intCiP1 = Short.Parse(mDATA(i)) : i = i + 1        ' �ï�ߔԍ�
                .intCiP2 = Short.Parse(mDATA(i)) : i = i + 1        ' ����Đ�
                .dblCiP3 = Double.Parse(mDATA(i)) : i = i + 1       ' ����ĊԲ������
            End With
            rCnt = rCnt + 1                                         ' �C���f�b�N�X�X�V

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typCirInInfoArray() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h������R�f�[�^���R�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h������R�f�[�^���R�f�[�^�\���̍\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)��R�f�[�^�\���̂̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Get_RESIST_Data(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim strMSG As String

        Try
            ' ���[�h������R�f�[�^���R�f�[�^�\���̂֊i�[����
            With typResistorInfoArray(rCnt)
                i = 0
                mDATA = pBuff.Split(",")                                           ' �������','�ŕ������Ď�o�� 
                .intResNo = Short.Parse(mDATA(i)) : i = i + 1                      ' ��R�ԍ�
                .intResMeasMode = Short.Parse(mDATA(i)) : i = i + 1                ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��) 
                .intResMeasType = UShort.Parse(mDATA(i)) : i = i + 1               ' ����^�C�v(0:���� ,1:�����x)�@���ǉ�
                .intCircuitGrp = Short.Parse(mDATA(i)) : i = i + 1                 ' ��������Ĕԍ�
                .intProbHiNo = Short.Parse(mDATA(i)) : i = i + 1                   ' �v���[�u�ԍ��i�n�C���j
                .intProbLoNo = Short.Parse(mDATA(i)) : i = i + 1                   ' �v���[�u�ԍ��i���[���j
                .intProbAGNo1 = Short.Parse(mDATA(i)) : i = i + 1                  ' �v���[�u�ԍ��i�P�j
                .intProbAGNo2 = Short.Parse(mDATA(i)) : i = i + 1                  ' �v���[�u�ԍ��i�Q�j
                .intProbAGNo3 = Short.Parse(mDATA(i)) : i = i + 1                  ' �v���[�u�ԍ��i�R�j
                .intProbAGNo4 = Short.Parse(mDATA(i)) : i = i + 1                  ' �v���[�u�ԍ��i�S�j
                .intProbAGNo5 = Short.Parse(mDATA(i)) : i = i + 1                  ' �v���[�u�ԍ��i�T�j
                .strExternalBits = mDATA(i) : i = i + 1                            ' EXTERNAL BITS
                .intPauseTime = Short.Parse(mDATA(i)) : i = i + 1                  ' �|�[�Y�^�C��
                .intTargetValType = Short.Parse(mDATA(i)) : i = i + 1              ' �ڕW�l�w��i0:��Βl,1:���V�I,2:�v�Z���j
                .intBaseResNo = Short.Parse(mDATA(i)) : i = i + 1                  ' �x�|�X��R�ԍ�
                .dblTrimTargetVal = Double.Parse(mDATA(i)) : i = i + 1             ' �g���~���O�ڕW�l
                .dblProbCfmPoint_Hi_X = Double.Parse(mDATA(i)) : i = i + 1         ' �v���[�u�m�F�ʒu HI X���W
                .dblProbCfmPoint_Hi_Y = Double.Parse(mDATA(i)) : i = i + 1         ' �v���[�u�m�F�ʒu HI Y���W
                .dblProbCfmPoint_Lo_X = Double.Parse(mDATA(i)) : i = i + 1         ' �v���[�u�m�F�ʒu LO X���W
                .dblProbCfmPoint_Lo_Y = Double.Parse(mDATA(i)) : i = i + 1         ' �v���[�u�m�F�ʒu LO Y���W
                .intSlope = Short.Parse(mDATA(i)) : i = i + 1                      ' �d���ω� �۰��
                .dblInitTest_HighLimit = Double.Parse(mDATA(i)) : i = i + 1        ' �C�j�V�����e�X�gHIGH���~�b�g
                .dblInitTest_LowLimit = Double.Parse(mDATA(i)) : i = i + 1         ' �C�j�V�����e�X�gLOW���~�b�g
                .dblFinalTest_HighLimit = Double.Parse(mDATA(i)) : i = i + 1       ' �t�@�C�i���e�X�gHIGH���~�b�g
                .dblFinalTest_LowLimit = Double.Parse(mDATA(i)) : i = i + 1        ' �t�@�C�i���e�X�gLOW���~�b�g
                .intInitialOkTestDo = Short.Parse(mDATA(i)) : i = i + 1            ' �Ƽ��OKý�(0:���Ȃ�,1:����)���ǉ�(�v���[�g�f�[�^����ړ�)
                .intCutReviseMode = Short.Parse(mDATA(i)) : i = i + 1              ' ��� �␳
                .intCutReviseDispMode = Short.Parse(mDATA(i)) : i = i + 1          ' �\��Ӱ��
                .intCutRevisePtnNo = Short.Parse(mDATA(i)) : i = i + 1             ' ����� No.
                .intCutReviseGrpNo = Short.Parse(mDATA(i)) : i = i + 1             ' ����ݸ�ٰ�ߔԍ�  
                .dblCutRevisePosX = Double.Parse(mDATA(i)) : i = i + 1             ' ��ĕ␳�ʒuX
                .dblCutRevisePosY = Double.Parse(mDATA(i)) : i = i + 1             ' ��ĕ␳�ʒuY
                .intIsNG = Short.Parse(mDATA(i)) : i = i + 1                       ' �摜�F��NG����(0:����, 1:�Ȃ�, �蓮)
                .intCutCount = Short.Parse(mDATA(i)) : i = i + 1                   ' �J�b�g��
                .strRatioTrimTargetVal = mDATA(i) : i = i + 1                      ' ���V�I�v�Z�� V1.13.0.0�A

                '----- V1.13.0.0�A�� -----
                If (SL432HW_FileVer >= 10.04) Then
                    .intCvMeasNum = Short.Parse(mDATA(i)) : i = i + 1               ' CV �ő呪���
                    .intCvMeasTime = Short.Parse(mDATA(i)) : i = i + 1              ' CV �ő呪�莞��(ms) 
                    .dblCvValue = Double.Parse(mDATA(i)) : i = i + 1                ' CV CV�l         
                    .intOverloadNum = Short.Parse(mDATA(i)) : i = i + 1             ' ���ް۰�� �� 
                    .dblOverloadMin = Double.Parse(mDATA(i)) : i = i + 1            ' ���ް۰�� �����l 
                    .dblOverloadMax = Double.Parse(mDATA(i)) : i = i + 1            ' ���ް۰�� ����l
                Else
                    .intCvMeasNum = 0                                               ' CV �ő呪���
                    .intCvMeasTime = 0                                              ' CV �ő呪�莞��(ms) 
                    .dblCvValue = 0.0                                               ' CV CV�l         
                    .intOverloadNum = 0                                             ' ���ް۰�� �� 
                    .dblOverloadMin = 0.0                                           ' ���ް۰�� �����l 
                    .dblOverloadMax = 0.0                                           ' ���ް۰�� ����l
                End If
                '----- V1.13.0.0�A�� -----

                '----- V2.0.0.0_23�� -----
                ' �t�@�C���o�[�W������10.072/10.073(���[��)����10.10�ȏ�̎��ɐݒ肷�� V4.0.0.0-33
                If ((SL432HW_FileVer = 10.072) Or (SL432HW_FileVer = 10.073) Or (SL432HW_FileVer >= 10.1)) Then
                    .wPauseTimeFT = Short.Parse(mDATA(i)) : i = i + 1               '  FT�O�̃|�[�Y�^�C��(0-32767msec) 
                Else
                    .wPauseTimeFT = 0                                               '  FT�O�̃|�[�Y�^�C��(0msec) 
                End If

                ' �t�@�C���o�[�W������10.10�ȏ�̎��ɐݒ肷��
                If (10.1 <= SL432HW_FileVer) Then
                    .intInsideEndChkCount = Short.Parse(mDATA(i)) : i = i + 1       ' ���؂蔻���
                    .dblInsideEndChgRate = Double.Parse(mDATA(i)) : i = i + 1       ' ���؂蔻��ω���(0.00-100.00%)
                Else
                    .intInsideEndChkCount = 0                                       ' ���؂蔻���
                    .dblInsideEndChgRate = 0.0                                      ' ���؂蔻��ω���(0.00-100.00%)
                End If
                '----- V2.0.0.0_23�� -----

                '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
                If (10.11 <= SL432HW_FileVer) Then                                  ' �t�@�C���o�[�W������10.11�ȏ�̎��ɐݒ肷��
                    'V5.0.0.2�@��
                    '                    .dblTrimTargetOfs = Double.Parse(mDATA(i)) : i = i + 1          ' ���ݸޖڕW�l�̾��
                    .dblTrimTargetOfs_Save = Double.Parse(mDATA(i)) : i = i + 1          ' ���ݸޖڕW�l�̾��
                    .dblTrimTargetOfs = .dblTrimTargetVal * (.dblTrimTargetOfs_Save / 100)
                    'V5.0.0.2�@��
                Else
                    .dblTrimTargetOfs = 0.0                                         ' ���ݸޖڕW�l�̾��
                    .dblTrimTargetOfs_Save = 0.0                                     'V5.0.0.2�@
                End If
                ' �ڕW�l�I�t�Z�b�g�L���Ȃ����ݸޖڕW�l�����ݸޖڕW�l+�̾��(�w�肪����ꍇ)�Ƃ���
                .dblTrimTargetVal_Save = .dblTrimTargetVal                          ' ���ݸޖڕW�l��ޔ�
                If (giTargetOfs = 1) And (.dblTrimTargetOfs <> 0.0) Then            ' �ڕW�l�I�t�Z�b�g�L�� ? 
                    .dblTrimTargetVal = .dblTrimTargetVal + .dblTrimTargetOfs       ' ���ݸޖڕW�l = �ڕW�l + �̾��
                End If
                '----- V4.11.0.0�@�� -----
                typPlateInfo.intResistCntInBlock = rCnt                            ' 1�u���b�N����R�����Z�b�g
                gRegistorCnt = rCnt

            End With
            rCnt = rCnt + 1                                                         ' �C���f�b�N�X�X�V

            Return (cFRS_NORMAL)                                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Get_RESIST_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "���[�h�����J�b�g�f�[�^���J�b�g�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>���[�h�����J�b�g�f�[�^���J�b�g�f�[�^�\���̂֊i�[����</summary>
    '''<param name="pBuff">(INP)�f�[�^</param>
    '''<param name="rCnt"> (I/O)��R�f�[�^�\���̂̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Get_CUT_Data(ByVal pBuff As String, ByRef rCnt As Integer) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim j As Integer
        Dim Rn As Integer
        Dim Cn As Integer
        Dim strMSG As String

        Try
            ' ��R�f�[�^�C���f�b�N�X�����߂� 
            mDATA = pBuff.Split(",")                                ' �������','�ŕ������Ď�o�� 
            Cn = Integer.Parse(mDATA(1))                            ' �J�b�g�ԍ�
            If (Cn = 1) Then                                        ' �J�b�g�ԍ�=�P�̏ꍇ�͒�R�f�[�^�C���f�b�N�X���J�E���g�A�b�v 
                rCnt = rCnt + 1
            End If
            ' ���[�h�����J�b�g�f�[�^���J�b�g�f�[�^�\���̂֊i�[����
            Rn = rCnt                                                               ' Rn = ��R�f�[�^�C���f�b�N�X
            i = 1                                                                   ' i  = ���[�h�����J�b�g�f�[�^�z��̃C���f�b�N�X
            With typResistorInfoArray(rCnt)
                .ArrCut(Cn).intCutNo = Short.Parse(mDATA(i)) : i = i + 1            ' �J�b�g�ԍ�
                .ArrCut(Cn).intDelayTime = Short.Parse(mDATA(i)) : i = i + 1        ' �f�B���C�^�C��
                .ArrCut(Cn).dblTeachPointX = Double.Parse(mDATA(i)) : i = i + 1     ' è��ݸ��߲��X
                .ArrCut(Cn).dblTeachPointY = Double.Parse(mDATA(i)) : i = i + 1     ' è��ݸ��߲��Y
                .ArrCut(Cn).dblStartPointX = Double.Parse(mDATA(i)) : i = i + 1     ' �X�^�[�g�|�C���gX
                .ArrCut(Cn).dblStartPointY = Double.Parse(mDATA(i)) : i = i + 1     ' �X�^�[�g�|�C���gY
                .ArrCut(Cn).dblCutSpeed = Double.Parse(mDATA(i)) : i = i + 1        ' �J�b�g�X�s�[�h
                '----- V4.0.0.0-28 �� -----
                If (FILE_VER_10_10 <= SL432HW_FileVer) Then
                    .ArrCut(Cn).dblQRate = Double.Parse(mDATA(i)) : i = i + 1       ' �p�X�C�b�`���[�g
                Else
                    .ArrCut(Cn).dblQRate = 0.1 : i = i + 1                          ' �p�X�C�b�`���[�g
                End If
                '----- V4.0.0.0-28 �� -----
                .ArrCut(Cn).dblCutOff = Double.Parse(mDATA(i)) : i = i + 1          ' �J�b�g�I�t�l
                .ArrCut(Cn).dblJudgeLevel = Double.Parse(mDATA(i)) : i = i + 1      ' �ؑփ|�C���g (���ް�����(���ω���))
                .ArrCut(Cn).dblCutOffOffset = Double.Parse(mDATA(i)) : i = i + 1    ' ��ĵ� �̾��
                .ArrCut(Cn).strCutType = mDATA(i).Trim() : i = i + 1                ' �J�b�g�`��(�O��̋󔒂��폜)
                .ArrCut(Cn).intCutDir = Short.Parse(mDATA(i)) : i = i + 1           ' �J�b�g���� 
                .ArrCut(Cn).intLTurnDir = Short.Parse(mDATA(i)) : i = i + 1         ' L��ݕ���(1:CW, 2:CCW) ���ύX
                '----- V4.0.0.0-57 �� -----
                ' L��ݕ�����0�̏ꍇ��1�ɕϊ�����(1:CW, 2:CCW)
                ' �J�b�g�`��L�J�b�g, HOOK�J�b�g, U�J�b�g�̏ꍇ
                If (.ArrCut(Cn).strCutType = CNS_CUTP_L) Or (.ArrCut(Cn).strCutType = CNS_CUTP_NL) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_Lr) Or (.ArrCut(Cn).strCutType = CNS_CUTP_Lt) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_NLr) Or (.ArrCut(Cn).strCutType = CNS_CUTP_NLt) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_HK) Or (.ArrCut(Cn).strCutType = CNS_CUTP_U) Or _
                       (.ArrCut(Cn).strCutType = CNS_CUTP_Ut) Then

                    If (.ArrCut(Cn).intLTurnDir = 0) Then
                        .ArrCut(Cn).intLTurnDir = 1
                    End If
                End If
                '----- V4.0.0.0-57 �� -----
                .ArrCut(Cn).dblMaxCutLength = Double.Parse(mDATA(i)) : i = i + 1    ' �ő�J�b�e�B���O�� 
                .ArrCut(Cn).dblR1 = Double.Parse(mDATA(i)) : i = i + 1              ' �q�P 
                .ArrCut(Cn).dblLTurnPoint = Double.Parse(mDATA(i)) : i = i + 1      ' �k�^�[���|�C���g 
                .ArrCut(Cn).dblMaxCutLengthL = Double.Parse(mDATA(i)) : i = i + 1   ' �k�^�[����̍ő�J�b�e�B���O�� 
                .ArrCut(Cn).dblR2 = Double.Parse(mDATA(i)) : i = i + 1              ' �q�Q 
                .ArrCut(Cn).dblMaxCutLengthHook = Double.Parse(mDATA(i)) : i = i + 1 ' �t�b�N�^�[����̃J�b�e�B���O�� 
                .ArrCut(Cn).intIndexCnt = Short.Parse(mDATA(i)) : i = i + 1         ' �C���f�b�N�X�� 

                ' 2011.08.30  
                '.ArrCut(Cn).intMeasMode = Short.Parse(mDATA(i)) : i = i + 1        ' ���胂�[�h 
                .ArrCut(Cn).intMeasType = Short.Parse(mDATA(i)) : i = i + 1         ' ����^�C�v(0:���� ,1:�����x, 2:�O��))��IX�p
                .ArrCut(Cn).intMeasMode = typResistorInfoArray(rCnt).intResMeasMode ' ���胂�[�h(0:��R ,1:�d��)����R�f�[�^����ݒ�
                If (.ArrCut(Cn).intMeasType >= MEASMODE_EXT) Then                   ' �O���Ȃ瑪�胂�[�h���Đݒ肷��
                    .ArrCut(Cn).intMeasMode = MEASMODE_EXT                          ' ���胂�[�h(0:��R ,1:�d��, 2:�O��)��IX�p
                End If

                .ArrCut(Cn).dblCutSpeed2 = Double.Parse(mDATA(i)) : i = i + 1       ' �J�b�g�X�s�[�h�Q 
                '----- V4.0.0.0-28 �� -----
                If (FILE_VER_10_10 <= SL432HW_FileVer) Then
                    .ArrCut(Cn).dblQRate2 = Double.Parse(mDATA(i)) : i = i + 1      ' �p�X�C�b�`���[�g�Q 
                Else
                    .ArrCut(Cn).dblQRate2 = 0.1 : i = i + 1                         ' �p�X�C�b�`���[�g�Q 
                End If
                '----- V4.0.0.0-28 �� -----
                .ArrCut(Cn).intCutAngle = Short.Parse(mDATA(i)) : i = i + 1         ' �΂߃J�b�g�̐؂�o���p�x 
                .ArrCut(Cn).dblPitch = Double.Parse(mDATA(i)) : i = i + 1           ' �s�b�` 
                .ArrCut(Cn).intStepDir = Short.Parse(mDATA(i)) : i = i + 1          ' �X�e�b�v���� 
                .ArrCut(Cn).intCutCnt = Short.Parse(mDATA(i)) : i = i + 1           ' �{�� 
                .ArrCut(Cn).dblESPoint = Double.Parse(mDATA(i)) : i = i + 1         ' ���޾ݽ�߲��
                .ArrCut(Cn).dblESJudgeLevel = Double.Parse(mDATA(i)) : i = i + 1    ' ���޾ݽ�̔���ω���
                .ArrCut(Cn).dblMaxCutLengthES = Double.Parse(mDATA(i)) : i = i + 1  ' ���޾ݽ��̶�Ē�

                .ArrCut(Cn).dblESChangeRatio = Double.Parse(mDATA(i)) : i = i + 1   ' ���޾ݽ��ω���
                .ArrCut(Cn).intESConfirmCnt = Short.Parse(mDATA(i)) : i = i + 1     ' ���޾ݽ��̊m�F��
                .ArrCut(Cn).intRadderInterval = Short.Parse(mDATA(i)) : i = i + 1   ' ��ް�ԋ���
                '----- V1.14.0.0�@�� -----
                If (SL432HW_FileVer >= FILE_VER_10_05) Then
                    .ArrCut(Cn).intCTcount = Short.Parse(mDATA(i)) : i = i + 1      ' ���޾ݽ��A��NG�m�F�񐔁��ǉ�(ES�p)
                Else
                    .ArrCut(Cn).intCTcount = 0                                      ' ���޾ݽ��A��NG�m�F�񐔁��ǉ�(ES�p) 
                End If
                '----- V1.14.0.0�@�� -----

                .ArrCut(Cn).dblZoom = Double.Parse(mDATA(i)) : i = i + 1            ' �{�� 

                .ArrCut(Cn).intMoveMode = Short.Parse(mDATA(i)) : i = i + 1         ' ���샂�[�h 
                .ArrCut(Cn).intDoPosition = Short.Parse(mDATA(i)) : i = i + 1       ' �|�W�V���j���O(0:�L, 1:��)
                '----- V2.0.0.0_24(V1.18.0.3�A)�� -----
                If (SL432HW_FileVer >= FILE_VER_10_073) Then
                    .ArrCut(Cn).intCutAftPause = Short.Parse(mDATA(i)) : i = i + 1  ' �J�b�g��|�[�Y�^�C��
                Else
                    .ArrCut(Cn).intCutAftPause = 0                                  ' �J�b�g��|�[�Y�^�C��
                End If
                '----- V2.0.0.0_24(V1.18.0.3�A)�� -----
                '----- V1.16.0.0�@�� -----
                If (SL432HW_FileVer >= FILE_VER_10_06) Then
                    .ArrCut(Cn).dblReturnPos = Double.Parse(mDATA(i)) : i = i + 1   ' ���^�[���J�b�g�̃��^�[���ʒu
                Else
                    .ArrCut(Cn).dblReturnPos = 0.0                                  ' ���^�[���J�b�g�̃��^�[���ʒu
                End If
                '----- V1.16.0.0�@�� -----
                '----- V1.18.0.0�C�� -----
                If (SL432HW_FileVer >= FILE_VER_10_07) Then
                    .ArrCut(Cn).dblLimitLen = Double.Parse(mDATA(i)) : i = i + 1    ' IX�J�b�g�̃��~�b�g��
                Else
                    .ArrCut(Cn).dblLimitLen = 0.0                                   ' IX�J�b�g�̃��~�b�g��
                End If
                '----- V1.18.0.0�C�� -----

                ' FL�p�f�[�^
                .ArrCut(Cn).dblCutSpeed3 = Double.Parse(mDATA(i)) : i = i + 1       ' �J�b�g�X�s�[�h3
                .ArrCut(Cn).dblCutSpeed4 = Double.Parse(mDATA(i)) : i = i + 1       ' �J�b�g�X�s�[�h4               
                .ArrCut(Cn).dblCutSpeed5 = Double.Parse(mDATA(i)) : i = i + 1       ' �J�b�g�X�s�[�h5               
                .ArrCut(Cn).dblCutSpeed6 = Double.Parse(mDATA(i)) : i = i + 1       ' �J�b�g�X�s�[�h6
                For j = 0 To (cCNDNUM - 1)                                          ' ���H�����ԍ�1�`8(0�ؼ��)
                    .ArrCut(Cn).CndNum(j) = Short.Parse(mDATA(i)) : i = i + 1
                Next j

                '----- V2.0.0.0_23�� -----
                ' SL436S�p(�V���v���g���}�p)
                ' �t�@�C���o�[�W������10.10�ȏ�̎��ɐݒ肷��
                If (SL432HW_FileVer >= FILE_VER_10_10) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3 = Double.Parse(mDATA(i)) : i = i + 1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4 = Double.Parse(mDATA(i)) : i = i + 1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5 = Double.Parse(mDATA(i)) : i = i + 1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6 = Double.Parse(mDATA(i)) : i = i + 1
                Else
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3 = 0.1             ' (�����l)
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4 = 0.1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5 = 0.1
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6 = 0.1
                End If
                '----- V2.0.0.0_23�� -----

                ' �ڕW�p���[�Ƌ��e�͈� ###066
                For j = 0 To (cCNDNUM - 1)                                          ' ���H�����ԍ�1�`n(0�ؼ��)
                    '----- V2.0.0.0_23�� -----
                    ' SL436S�p(�V���v���g���}�p)
                    ' �t�@�C���o�[�W������10.10�ȏ�̎��ɐݒ肷��
                    If (SL432HW_FileVer >= FILE_VER_10_10) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(j) = Short.Parse(mDATA(i)) : i = i + 1
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(j) = Short.Parse(mDATA(i)) : i = i + 1
                    Else
                        ' �t�@�C���o�[�W������10.00�̎��̓f�t�H���g�l��ݒ肷��
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(j) = POWERADJUST_CURRENT  ' �d���l1�`8
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(j) = POWERADJUST_STEG        ' STEG1�`8
                    End If
                    '----- V2.0.0.0�K�� -----

                    ' �t�@�C���o�[�W������10.01�ȏ�̎��ɐݒ肷��
                    If (SL432HW_FileVer >= FILE_VER_10_01) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = Double.Parse(mDATA(i)) : i = i + 1
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(j) = Double.Parse(mDATA(i)) : i = i + 1
                    Else
                        ' �t�@�C���o�[�W������10.00�ȉ��̎��̓f�t�H���g�l��ݒ肷��
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = POWERADJUST_TARGET   ' �ڕW�p���[(W)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL  ' ���e�͈�(�}W)
                    End If
                    '----- V1.18.0.4�@�� -----
                    If (typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = 0.0) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(j) = POWERADJUST_TARGET
                        typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(j) = POWERADJUST_LEVEL
                    End If
                    '----- V1.18.0.4�@�� -----
                Next j

                ' ������f�[�^
                .ArrCut(Cn).strChar = mDATA(i).Trim() : i = i + 1                   ' ������ 
                .ArrCut(Cn).strDataName = mDATA(i).Trim() : i = i + 1               ' U�J�b�g�f�[�^�����ǉ� 
            End With

            Return (cFRS_NORMAL)                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Get_CUT_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- ###229�� -----
#Region "���[�h����GPIB�f�[�^��GPIB�f�[�^�\���̂֊i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    ''' <summary>���[�h����GPIB�f�[�^��GPIB�f�[�^�\���̂֊i�[����</summary>
    ''' <param name="mDATA">       (INP)�f�[�^</param>
    ''' <param name="typGpibInfo"> (OUT)GPIB�f�[�^�\����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Set_typGpibInfo(ByVal mDATA() As String, ByRef typGpibInfo As GpibInfo) As Integer

        Dim i As Integer
        Dim strMSG As String

        Try
            ' �t�@�C���o�[�W���� >= 10.03�̎��ɐݒ肷��
            If (SL432HW_FileVer < FILE_VER_10_03) Then Return (cFRS_NORMAL)

            ' GPIB�f�[�^�\���̂փf�[�^���i�[����
            With typGpibInfo
                i = 0
                .wGPIBmode = Short.Parse(mDATA(i)) : i = i + 1          ' GP-IB����(0:���Ȃ� 1:����)
                .wDelim = Short.Parse(mDATA(i)) : i = i + 1             ' �����(0:CR+LF 1:CR 2:LF 3:NONE)
                .wTimeout = Short.Parse(mDATA(i)) : i = i + 1           ' ��ѱ��(1�`32767)(ms�P��)
                .wAddress = Short.Parse(mDATA(i)) : i = i + 1           ' �@����ڽ(0�`30)
                .wEOI = Short.Parse(mDATA(i)) : i = i + 1               ' EOI(0:�g�p���Ȃ�, 1:�g�p����)
                .wPause1 = Short.Parse(mDATA(i)) : i = i + 1            ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
                .wPause2 = Short.Parse(mDATA(i)) : i = i + 1            ' �ݒ�����2���M��|�[�Y����(1�`32767msec)
                .wPause3 = Short.Parse(mDATA(i)) : i = i + 1            ' �ݒ�����3���M��|�[�Y����(1�`32767msec)
                .wPauseT = Short.Parse(mDATA(i)) : i = i + 1            ' �ض޺���ޑ��M��|�[�Y����(1�`32767msec)
                .wRev = Short.Parse(mDATA(i)) : i = i + 1               ' �\��
                .strI = mDATA(i) : i = i + 1                            ' �����������(MAX40byte)
                .strI2 = mDATA(i) : i = i + 1                           ' �����������2(MAX40byte)
                .strI3 = mDATA(i) : i = i + 1                           ' �����������3(MAX40byte)
                .strT = mDATA(i) : i = i + 1                            ' �ض޺����(50byte)
                .strName = mDATA(i) : i = i + 1                         ' �@�햼(10byte)
                .wReserve = mDATA(i) : i = i + 1                        ' �\��(8byte)  
            End With

            Return (cFRS_NORMAL)                                        ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Set_typGpibInfo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- ###229�� -----
#Region "���[�h�����L�k�␳�p�f�[�^���i�[����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    ''' <summary>���[�h�����L�k�␳�p�f�[�^���i�[����</summary>
    ''' <param name="pBuff">       (INP)�ǂݍ��݃f�[�^</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function SetSinsyukuData(ByVal pBuff As String) As Integer

        Dim mDATA() As String
        Dim i As Integer
        Dim mBlock() As String

        If pBuff = "" Then
            Return 0
        End If

        i = 0
        ' ���[�h�f�[�^��z��Z�b�g 
        mDATA = pBuff.Split(",")
        For i = 0 To mDATA.Length - 1
            mBlock = mDATA(i).Split("-")
            SelectBlock((mBlock(0) - 1), (mBlock(1) - 1)) = 1
        Next i
    End Function

#End Region
#End If
#End Region 'V5.0.0.8�@

#Region "�t�@�C���Z�[�u�����yTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>�t�@�C���Z�[�u�����yTKY,CHIP,NET�����Łz</summary>
    '''<param name="strPath">(INP) �t�@�C����</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function File_Save(ByVal strPath As String) As Integer

        'Dim writer As System.IO.StreamWriter
        Dim r As Integer
        Dim strMSG As String

        Try
#If False Then      'V5.0.0.8�@
            ' ��������
            '                                                       ' false = �㏑��(true = �ǉ�)
            'writer = New System.IO.StreamWriter(strPath, False, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using writer As New StreamWriter(strPath, False, Encoding.UTF8)     'V4.4.0.0-1

                ' [FILE VERSION]�f�[�^����������
                writer.WriteLine(CONST_VERSION)                         ' "[FILE VERSION]" ��"\r\n"�t���@�P���ɕ�������������ނɂ́AWrite() ���g�p����B
                If (gTkyKnd = KND_TKY) Then
                    'writer.WriteLine(CONST_FILETYPE10)                 ' "TKYDATA_Ver10.00"
                    writer.WriteLine(CONST_FILETYPE_CUR)                ' �����\�t�g��(���ݔŖ�) ###066
                ElseIf (gTkyKnd = KND_CHIP) Then
                    'writer.WriteLine(FILETYPE10)                       ' "TKYCHIP_SL432HW_Ver10.00"
                    writer.WriteLine(FILETYPE_CUR)                      '�����\�t�g��(���ݔŖ�) ###066
                Else
                    'writer.WriteLine(FILETYPE10)                       ' "TKYNET_SL432HW_Ver10.00"
                    writer.WriteLine(FILETYPE_CUR)                      ' �����\�t�g��(���ݔŖ�) ###066
                End If

                ' [PLATE01]�f�[�^����������
                r = Put_PLT01_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE03]�f�[�^����������
                r = Put_PLT03_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE02]�f�[�^����������
                r = Put_PLT02_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE04]�f�[�^����������
                r = Put_PLT04_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' [PLATE05]�f�[�^����������
                r = Put_PLT05_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                '----- V1.13.0.0�A�� -----
                ' [PLATE06]�f�[�^����������
                r = Put_PLT06_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If
                '----- V1.13.0.0�A�� -----

            'V5.0.0.6�@��
            ' [OPTION]�f�[�^����������
            r = Put_OPTION_Data(writer)
            If (r <> cFRS_NORMAL) Then
                writer.Close()
                Return (cFRS_FIOERR_OUT)
            End If
            'V5.0.0.6�@��

                ' �T�[�L�b�g�f�[�^����������(TKY�p)
                r = Put_CIRCUIT_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' �X�e�b�v�f�[�^����������(CHIP/NET�p)
                r = Put_STEP_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' �O���[�v�f�[�^����������(CHIP�p)
                r = Put_GROUP_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' TY2�f�[�^����������(CHIP�p)
                r = Put_TY2_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' �T�[�L�b�g���W�f�[�^����������(NET�p)
                r = Put_CIRCUITAXIS_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' �T�[�L�b�g�ԃC���^�[�o���f�[�^����������(NET�p)
                r = Put_CIRCUITINTERVAL_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' �ٌ`�ʕt���f�[�^(TKY�p) ���T�|�[�g

                ' ��R�f�[�^����������
                r = Put_RESIST_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                ' �J�b�g�f�[�^����������
                r = Put_CUT_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If

                '----- ###229�� -----
                ' GPIB�f�[�^����������(TKY/CHIP/NET�p)
                r = Put_GPIB_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If
                '----- ###229�� -----

                'V1.13.0.0�D
                '�L�k�␳�p�f�[�^�̏������� 
                r = Put_SINSYUKU_Data(writer)
                If (r <> cFRS_NORMAL) Then
                    'writer.Close()
                    Return (cFRS_FIOERR_OUT)
                End If
                'V1.13.0.0�D

                ' �I������
                'writer.Close()
            End Using

            Return (cFRS_NORMAL)                                    ' Return�l = ����
#End If
            'V5.0.0.8�@              ��
            SetTemporaryData(False)    ' �l�𕜋A����
            r = DirectCast(FileIO.File_Save(strPath), Integer)
            SetTemporaryData(True)     ' �l��ޔ�����
            'V5.0.0.8�@              ��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.File_Save() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            'V5.0.0.8�@            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
            r = cERR_TRAP               ' Return�l = ��O�G���[
        End Try

        Return r    'V5.0.0.8�@

    End Function
#End Region

#Region "�f�[�^�ޔ��E���A"
    '''=========================================================================
    ''' <summary>
    ''' �f�[�^�ޔ��E���A
    ''' </summary>
    ''' <param name="toSave">True:�ޔ�,False:���A</param>
    ''' <remarks>'V5.0.0.8�@</remarks>
    '''=========================================================================
    Private Sub SetTemporaryData(ByVal toSave As Boolean)

        ' �t�@�C���ǂݍ��ݍς݂ŖڕW�l�I�t�Z�b�g���L���̏ꍇ
        If (True = gLoadDTFlag) AndAlso (0 <> giTargetOfs) Then
            If (toSave) Then
                ' �ޔ�
                For i As Integer = 1 To (typResistorInfoArray.Length - 1) Step 1
                    With typResistorInfoArray(i)
                        .dblTrimTargetOfs_Save = .dblTrimTargetOfs                      ' ���ݸޖڕW�l�̾�Ă�ޔ�
                        .dblTrimTargetOfs = .dblTrimTargetVal * (.dblTrimTargetOfs_Save / 100)

                        .dblTrimTargetVal_Save = .dblTrimTargetVal                      ' ���ݸޖڕW�l��ޔ�
                        ' �ڕW�l�I�t�Z�b�g�L���Ȃ����ݸޖڕW�l�����ݸޖڕW�l+�̾��(�w�肪����ꍇ)�Ƃ���
                        If (0.0 <> .dblTrimTargetOfs) Then                              ' �ڕW�l�I�t�Z�b�g�L�� ? 
                            .dblTrimTargetVal = .dblTrimTargetVal + .dblTrimTargetOfs   ' ���ݸޖڕW�l = �ڕW�l + �̾��
                        End If
                    End With
                Next i
            Else
                ' ���A
                For i As Integer = 1 To (typResistorInfoArray.Length - 1) Step 1
                    With typResistorInfoArray(i)
                        .dblTrimTargetOfs = .dblTrimTargetOfs_Save ' ���ݸޖڕW�l�̾�đޔ��悩��߂�
                        .dblTrimTargetVal = .dblTrimTargetVal_Save ' ���ݸޖڕW�l�ޔ��悩��߂�
                    End With
                Next i
            End If
            '----- V6.0.3.0_43�� -----
        Else
            ' �ڕW�l�I�t�Z�b�g�������̏ꍇ
            For i As Integer = 1 To (typResistorInfoArray.Length - 1) Step 1
                With typResistorInfoArray(i)
                    .dblTrimTargetOfs = 0.0
                    .dblTrimTargetVal_Save = .dblTrimTargetVal
                End With
            Next i
            '----- V6.0.3.0_43�� -----
        End If

    End Sub
#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�Œ�`"
#If False Then 'V5.0.0.8�@
#Region "[PLATE01]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[PLATE01]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_PLT01_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE01]�f�[�^����������
                writer.WriteLine(FILE_CONST_PLATE_01)                           ' "[PLATE01]"
                writer.WriteLine(.strDataName)                                  ' �g���~���O�f�[�^��
                writer.WriteLine(.intDirStepRepeat.ToString("0"))               ' �ï�߁���߰�
                writer.WriteLine(.intChipStepCnt.ToString("0"))                 ' �`�b�v�X�e�b�v��
                strDat = .intPlateCntXDir.ToString("0") + "," + .intPlateCntYDir.ToString("0")
                writer.WriteLine(strDat)                                        ' �v���[�g��X,Y 
                strDat = .intBlockCntXDir.ToString("0") + "," + .intBlockCntYDir.ToString("0")
                writer.WriteLine(strDat)                                        ' ��ۯ���X,Y 
                strDat = .dblPlateItvXDir.ToString("0.00000") + "," + .dblPlateItvYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �v���[�g�ԊuX,Y 
                strDat = .dblBlockSizeXDir.ToString("0.00000") + "," + .dblBlockSizeYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �u���b�N�T�C�YX,Y 
                strDat = .dblTableOffsetXDir.ToString("0.0000") + "," + .dblTableOffsetYDir.ToString("0.0000")
                writer.WriteLine(strDat)                                        ' ð��وʒu�̾��X,Y 
                strDat = .dblBpOffSetXDir.ToString("0.00000") + "," + .dblBpOffSetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �ްшʒu�̾��X,Y 
                strDat = .dblAdjOffSetXDir.ToString("0.00000") + "," + .dblAdjOffSetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ��ެ�Ĉʒu�̾��X,Y(���g�p)
                writer.WriteLine(.intCurcuitCnt.ToString("0"))                  ' �T�[�L�b�g��
                writer.WriteLine(.intNGMark.ToString("0"))                      ' NGϰ�ݸ�
                writer.WriteLine(.intDelayTrim.ToString("0"))                   ' �ިڲ���
                writer.WriteLine(.intNgJudgeUnit.ToString("0"))                 ' NG����P��
                writer.WriteLine(.intNgJudgeLevel.ToString("0"))                ' NG����
                writer.WriteLine(.dblZOffSet.ToString("0.0000"))                ' ZON�ʒu
                writer.WriteLine(.dblZStepUpDist.ToString("0.0000"))            ' �ï�ߏ㏸����
                writer.WriteLine(.dblZWaitOffset.ToString("0.0000"))            ' ZOFF�ʒu
                '----- V1.13.0.0�A�� -----
                writer.WriteLine(.dblLwPrbStpDwDist.ToString("0.0000"))         ' ������۰�޽ï�߉��~����
                writer.WriteLine(.dblLwPrbStpUpDist.ToString("0.0000"))         ' ������۰�޽ï�ߏ㏸����
                '----- V1.13.0.0�A�� -----
                '----- V4.0.0.0�C�� -----
                ' �v���[�u���g���C��(0=���g���C�Ȃ�)(�I�v�V����)
                writer.WriteLine(.intPrbRetryCount.ToString("0"))               ' �v���[�u���g���C��(0=���g���C�Ȃ�)(�I�v�V����)
                '----- V4.0.0.0�C�� -----
                '----- V1.23.0.0�F�� -----
                ' �v���[�u�`�F�b�N����(�I�v�V����)
                writer.WriteLine(.intPrbChkPlt.ToString("0"))                   ' ����
                writer.WriteLine(.intPrbChkBlk.ToString("0"))                   ' �u���b�N
                writer.WriteLine(.dblPrbTestLimit.ToString("0.000"))            ' �덷�}%
                '----- V1.23.0.0�F�� -----

            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_PLT01_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[PLATE03]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[PLATE03]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_PLT03_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE03]�f�[�^����������
                writer.WriteLine(FILE_CONST_PLATE_03)                           ' "[PLATE03]"
                writer.WriteLine(.intResistDir.ToString("0"))                   ' ��R���ѕ���
                writer.WriteLine(.intCircuitCntInBlock.ToString("0"))           ' 1��ۯ��໰��Đ� 
                strDat = .dblCircuitSizeXDir.ToString("0.00000") + "," + .dblCircuitSizeYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ����Ļ���X,Y 
                writer.WriteLine(.intResistCntInBlock.ToString("0"))            ' 1�u���b�N����R��
                writer.WriteLine(.intResistCntInGroup.ToString("0"))            ' 1��ٰ�ߓ���R��
                strDat = .intGroupCntInBlockXBp.ToString("0") + "," + .intGroupCntInBlockYStage.ToString("0")
                writer.WriteLine(strDat)                                        ' ��ۯ����ٰ�ߐ�X,Y 
                strDat = .intBlkCntInStgGrpX.ToString("0") + "," + .intBlkCntInStgGrpY.ToString("0")
                writer.WriteLine(strDat)                                        ' �X�e�[�W�O���[�v���u���b�N��X,Y 
                strDat = .dblBpGrpItv.ToString("0.00000") + "," + .dblStgGrpItvY.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ��ٰ�ߊԊuBP(X),Stage(Y) 
                strDat = .dblChipSizeXDir.ToString("0.00000") + "," + .dblChipSizeYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ���߻���X,Y
                strDat = .dblStepOffsetXDir.ToString("0.00000") + "," + .dblStepOffsetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �ï�ߵ̾�ė�X,Y
                strDat = .dblBlockSizeReviseXDir.ToString("0.00000") + "," + .dblBlockSizeReviseYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ��ۯ����ޕ␳��X,Y
                strDat = .dblBlockItvXDir.ToString("0.00000") + "," + .dblBlockItvYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' ��ۯ��ԊuX,Y
                writer.WriteLine(.intContHiNgBlockCnt.ToString("0"))            ' �A��NG-HIGH��R��ۯ���
                strDat = .dblPlateSizeX.ToString("0.00000") + "," + .dblPlateSizeY.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �v���[�g�T�C�YX,Y
                strDat = .dblStgGrpItvX.ToString("0.00000") + "," + .dblStgGrpItvY.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �X�e�[�W�O���[�v�ԊuX,Y
            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_PLT03_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[PLATE02]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[PLATE02]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_PLT02_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE02]�f�[�^����������
                writer.WriteLine(FILE_CONST_PLATE_02)                           ' "[PLATE02]"
                writer.WriteLine(.intReviseMode.ToString("0"))                  ' �␳���[�h
                writer.WriteLine(.intManualReviseType.ToString("0"))            ' �␳���@
                strDat = .dblReviseCordnt1XDir.ToString("0.00000") + "," + .dblReviseCordnt1YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �␳�ʒu���W1X,Y 
                strDat = .dblReviseCordnt2XDir.ToString("0.00000") + "," + .dblReviseCordnt2YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �␳�ʒu���W2X,Y 
                strDat = .dblReviseOffsetXDir.ToString("0.00000") + "," + .dblReviseOffsetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �␳�߼޼�ݵ̾��X,Y 
                writer.WriteLine(.intRecogDispMode.ToString("0"))               ' �F���ް��\��Ӱ��
                strDat = .dblPixelValXDir.ToString("0.0000") + "," + .dblPixelValYDir.ToString("0.0000")
                writer.WriteLine(strDat)                                        ' �߸�ْlX,Y 
                strDat = .intRevisePtnNo1.ToString("0") + "," + .intRevisePtnNo2.ToString("0")
                writer.WriteLine(strDat)                                        ' �␳�ʒu�����No1,2
                strDat = .intRevisePtnNo1GroupNo.ToString("0") + "," + .intRevisePtnNo2GroupNo.ToString("0")
                writer.WriteLine(strDat)                                        ' �␳�ʒu�O���[�vNo1,2
                writer.WriteLine(.dblRotateTheta.ToString("0.00000"))           ' �Ǝ��p�x
                writer.WriteLine(.dblTThetaOffset.ToString("0.00000"))          ' �s�ƃI�t�Z�b�g
                strDat = .dblTThetaBase1XDir.ToString("0.00000") + "," + .dblTThetaBase1YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �s�Ɗ�ʒu1X,Y
                strDat = .dblTThetaBase2XDir.ToString("0.00000") + "," + .dblTThetaBase2YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �s�Ɗ�ʒu2X,Y
                'V5.0.0.9�A                                          �� ���t�A���C�����g�p
                writer.WriteLine(.intReviseExecRgh.ToString("0"))               ' �␳�L��(0:�␳�Ȃ�, 1:�␳����)
                strDat = .dblReviseCordnt1XDirRgh.ToString("0.00000") & "," & .dblReviseCordnt1YDirRgh.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �␳�ʒu���W1X,�␳�ʒu���W1Y
                strDat = .dblReviseCordnt2XDirRgh.ToString("0.00000") & "," & .dblReviseCordnt2YDirRgh.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �␳�ʒu���W2X,�␳�ʒu���W2Y
                strDat = .dblReviseOffsetXDirRgh.ToString("0.00000") & "," & .dblReviseOffsetYDirRgh.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �␳�߼޼�ݵ̾��X,�␳�߼޼�ݵ̾��Y
                writer.WriteLine(.intRecogDispModeRgh.ToString("0"))            ' �F���ް��\��Ӱ��(0:�\���Ȃ�, 1:�\������)
                strDat = .intRevisePtnNo1Rgh & "," & .intRevisePtnNo2Rgh
                writer.WriteLine(strDat)                                        ' �␳�ʒu�����No1,�␳�ʒu�����No2
                strDat = .intRevisePtnNo1GroupNoRgh & "," & .intRevisePtnNo2GroupNoRgh
                writer.WriteLine(strDat)                                        ' �␳�ʒu�����No1�O���[�vNo,�␳�ʒu�����No2�O���[�vNo
                'V5.0.0.9�A                                          ��
            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_PLT02_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[PLATE04]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[PLATE04]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_PLT04_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strDat As String
        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE04]�f�[�^����������
                writer.WriteLine(FILE_CONST_PLATE_04)                           ' "[PLATE04]"
                strDat = .dblCaribBaseCordnt1XDir.ToString("0.00000") + "," + .dblCaribBaseCordnt1YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �����ڰ��݊���W1X,Y 
                strDat = .dblCaribBaseCordnt2XDir.ToString("0.00000") + "," + .dblCaribBaseCordnt2YDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �����ڰ��݊���W2X,Y 
                strDat = .dblCaribTableOffsetXDir.ToString("0.00000") + "," + .dblCaribTableOffsetYDir.ToString("0.00000")
                writer.WriteLine(strDat)                                        ' �����ڰ��ݵ̾��X,Y 
                strDat = .intCaribPtnNo1.ToString("0") + "," + .intCaribPtnNo2.ToString("0")
                writer.WriteLine(strDat)                                        ' �����ڰ�������ݓo�^No1,2 
                strDat = .intCaribPtnNo1GroupNo.ToString("0") + "," + .intCaribPtnNo2GroupNo.ToString("0")
                writer.WriteLine(strDat)                                        ' �����ڰ�������݃O���[�vNo1,2 
                writer.WriteLine(.dblCaribCutLength.ToString("0.00000"))        ' �����ڰ��ݶ�Ē�
                writer.WriteLine(.dblCaribCutSpeed.ToString("0.0"))             ' �����ڰ��ݶ�đ��x
                writer.WriteLine(.dblCaribCutQRate.ToString("0.0"))             ' �����ڰ���ڰ��Qڰ�
                writer.WriteLine(.intCaribCutCondNo.ToString("0"))              ' �����ڰ��݉��H�����ԍ�(FL�p)

                strDat = .dblCutPosiReviseOffsetXDir.ToString("0.000") + "," + .dblCutPosiReviseOffsetYDir.ToString("0.000")
                writer.WriteLine(strDat)                                        ' ��Ĉʒu�␳ð��ٵ̾��X,Y 
                writer.WriteLine(.intCutPosiRevisePtnNo.ToString("0"))          ' ��Ĉʒu�␳����ݓo�^No
                writer.WriteLine(.dblCutPosiReviseCutLength.ToString("0.00000")) ' ��Ĉʒu�␳��Ē�
                writer.WriteLine(.dblCutPosiReviseCutSpeed.ToString("0.0"))     ' ��Ĉʒu�␳��đ��x
                writer.WriteLine(.dblCutPosiReviseCutQRate.ToString("0.0"))     ' ��Ĉʒu�␳ڰ��Qڰ�
                writer.WriteLine(.intCutPosiReviseGroupNo.ToString("0"))        ' ��Ĉʒu�␳��ٰ��No
                writer.WriteLine(.intCutPosiReviseCondNo.ToString("0"))         ' ��Ĉʒu�␳���H�����ԍ�(FL�p)
            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_PLT04_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[PLATE05]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[PLATE05]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_PLT05_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE05]�f�[�^����������
                writer.WriteLine(FILE_CONST_PLATE_05)                           ' "[PLATE05]"
                writer.WriteLine(.intMaxTrimNgCount.ToString("0"))              ' ���ݸ�NG����(���)
                writer.WriteLine(.intMaxBreakDischargeCount.ToString("0"))      ' ���ꌇ���r�o����(���)
                writer.WriteLine(.intTrimNgCount.ToString("0"))                 ' �A�����ݸ�NG����
                writer.WriteLine(.intContHiNgResCnt.ToString("0"))              ' �A�����ݸ�NG��R��    ###230
                writer.WriteLine(.intRetryProbeCount.ToString("0"))             ' ����۰��ݸމ�
                writer.WriteLine(.dblRetryProbeDistance.ToString("0.00000"))    ' ����۰��ݸވړ���

                writer.WriteLine(.intWorkSetByLoader.ToString("0"))             ' ��i��
                writer.WriteLine(.intOpenCheck.ToString("0"))                   ' 4�[�q���������
                writer.WriteLine(.intLedCtrl.ToString("0"))                     ' LED����

                writer.WriteLine(.intPowerAdjustMode.ToString("0"))             ' �p���[�������[�h
                writer.WriteLine(.dblPowerAdjustTarget.ToString("0.00"))        ' �����ڕW�p���[
                writer.WriteLine(.dblPowerAdjustQRate.ToString("0.0"))          ' �p���[����Q���[�g
                writer.WriteLine(.dblPowerAdjustToleLevel.ToString("0.00"))     ' �p���[�������e�͈�
                writer.WriteLine(.intPowerAdjustCondNo.ToString("0"))           ' �p���[�������H�����ԍ�(FL�p) 

                writer.WriteLine(.intGpibCtrl.ToString("0"))                    ' GP-IB����
                writer.WriteLine(.intGpibDefDelimiter.ToString("0"))            ' �����ݒ�(�����)
                writer.WriteLine(.intGpibDefTimiout.ToString("0"))              ' �����ݒ�(��ѱ��)
                writer.WriteLine(.intGpibDefAdder.ToString("0"))                ' �����ݒ�(�@����ڽ)'###002
                writer.WriteLine(.strGpibInitCmnd1)                             ' �����������1
                writer.WriteLine(.strGpibInitCmnd2)                             ' �����������2
                writer.WriteLine(.strGpibTriggerCmnd)                           ' �ض޺����
                writer.WriteLine(.intGpibMeasSpeed.ToString("0"))               ' ���葬�x(0:�ᑬ, 1:����)
                writer.WriteLine(.intGpibMeasMode.ToString("0"))                ' ���胂�[�h(0:���, 1:�΍�)
                writer.WriteLine(.intNgJudgeStop.ToString("0"))                 ' NG���莞��~ V1.13.0.0�A
                '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
                writer.WriteLine(.intPwrChkPltNum.ToString("0"))                ' �I�[�g�p���[�`�F�b�N�����
                writer.WriteLine(.intPwrChkTime.ToString("0"))                  ' �I�[�g�p���[�`�F�b�N����(��) 
                '----- V4.11.0.0�@�� -----
            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_PLT05_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- V1.13.0.0�A�� -----
#Region "[PLATE06]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[PLATE06]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_PLT06_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            With typPlateInfo
                ' [PLATE06]�f�[�^����������
                writer.WriteLine(FILE_CONST_PLATE_06)                           ' "[PLATE06]"
                writer.WriteLine(.intContExpMode.ToString("0"))                 ' �L�k�␳ (0:�Ȃ�, 1:����)
                writer.WriteLine(.intContExpGrpNo.ToString("0"))                ' �L�k�␳��ٰ�ߔԍ�
                writer.WriteLine(.intContExpPtnNo.ToString("0"))                ' �L�k�␳����ݔԍ�
                writer.WriteLine(.dblContExpPosX.ToString("0.0000"))            ' �L�k�␳�ʒuX (mm)
                writer.WriteLine(.dblContExpPosY.ToString("0.0000"))            ' �L�k�␳�ʒuXY (mm)            
                writer.WriteLine(.intStepMeasCnt.ToString("0"))                 ' �ï�ߑ����
                writer.WriteLine(.dblStepMeasPitch.ToString("0.0000"))          ' �ï�ߑ����߯�
                writer.WriteLine(.intStepMeasReptCnt.ToString("0"))             ' �ï�ߑ���J��Ԃ��ï�߉�
                writer.WriteLine(.dblStepMeasReptPitch.ToString("0.0000"))      ' �ï�ߑ���J��Ԃ��ï���߯�
                writer.WriteLine(.intStepMeasLwGrpNo.ToString("0"))             ' �ï�ߑ��艺����۰�޸�ٰ�ߔԍ�
                writer.WriteLine(.intStepMeasLwPtnNo.ToString("0"))             ' �ï�ߑ��艺����۰������ݔԍ�
                writer.WriteLine(.dblStepMeasBpPosX.ToString("0.0000"))         ' �ï�ߑ���BP�ʒuX
                writer.WriteLine(.dblStepMeasBpPosY.ToString("0.0000"))         ' �ï�ߑ���BP�ʒuY
                writer.WriteLine(.intStepMeasUpGrpNo.ToString("0"))             ' �ï�ߑ�������۰�޸�ٰ�ߔԍ�
                writer.WriteLine(.intStepMeasUpPtnNo.ToString("0"))             ' �ï�ߑ�������۰������ݔԍ�
                writer.WriteLine(.dblStepMeasTblOstX.ToString("0.0000"))        ' �ï�ߑ�������۰��ð��ٵ̾��X
                writer.WriteLine(.dblStepMeasTblOstY.ToString("0.0000"))        ' �ï�ߑ�������۰��ð��ٵ̾��Y
                writer.WriteLine(.intIDReaderUse.ToString("0"))                 ' IDذ�� (0:���g�p, 1:�g�p)
                writer.WriteLine(.dblIDReadPos1X.ToString("0.0000"))            ' IDذ�ޓǂݎ���߼޼�� 1X
                writer.WriteLine(.dblIDReadPos1Y.ToString("0.0000"))            ' IDذ�ޓǂݎ���߼޼�� 1Y
                writer.WriteLine(.dblIDReadPos2X.ToString("0.0000"))            ' IDذ�ޓǂݎ���߼޼�� 2X
                writer.WriteLine(.dblIDReadPos2Y.ToString("0.0000"))            ' IDذ�ޓǂݎ���߼޼�� 2Y
                writer.WriteLine(.dblReprobeVar.ToString("0.0000"))             ' ����۰��ݸނ΂����
                writer.WriteLine(.dblReprobePitch.ToString("0.0000"))           ' ����۰��ݸ��߯�

                '----- V2.0.0.0_23�� -----
                '----- �v���[�u�N���[�j���O���� -----
                writer.WriteLine(.dblPrbCleanPosX.ToString("0.0000"))           ' �N���[�j���O�ʒuX
                writer.WriteLine(.dblPrbCleanPosY.ToString("0.0000"))           ' �N���[�j���O�ʒuY
                writer.WriteLine(.dblPrbCleanPosZ.ToString("0.0000"))           ' �N���[�j���O�ʒuZ
                writer.WriteLine(.intPrbCleanUpDwCount.ToString("0"))           ' �v���[�u�㉺��
                writer.WriteLine(.intPrbCleanAutoSubCount.ToString("0"))        ' �����^�]���N���[�j���O���s�����
                '----- V2.0.0.0�K�� -----

                writer.WriteLine(.dblTXChipsizeRelationX.ToString("0.0000"))    ' �␳�ʒu�P�ƂQ�̑��Βl�w 'V4.5.1.0�N
                writer.WriteLine(.dblTXChipsizeRelationY.ToString("0.0000"))    ' �␳�ʒu�P�ƂQ�̑��Βl�x 'V4.5.1.0�N

                ''V4.10.0.0�C            ��
                'writer.WriteLine(.dblPrbCleanStagePitchX.ToString("0.0000"))    ' �X�e�[�W����s�b�`X
                'writer.WriteLine(.dblPrbCleanStagePitchY.ToString("0.0000"))    ' �X�e�[�W����s�b�`Y
                'writer.WriteLine(.intPrbCleanStageCountX.ToString())            ' �X�e�[�W�����X
                'writer.WriteLine(.intPrbCleanStageCountY.ToString())            ' �X�e�[�W�����Y
                ''V4.10.0.0�C            ��
                ''V4.10.0.0�H��
                'writer.WriteLine(.dblPrbDistance.ToString("0.0000"))            ' �X�e�[�W����s�b�`X
                'writer.WriteLine(.dblPrbCleaningOffset.ToString("0.0000"))      ' �X�e�[�W����s�b�`Y
                ''V4.10.0.0�H��

            End With
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_PLT06_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- V1.13.0.0�A�� -----

#Region "[OPTION]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    'V5.0.0.6�@��
    ''' <summary>
    ''' [OPTION]�f�[�^����������
    ''' </summary>
    ''' <param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    ''' <returns>����:cFRS_NORMAL, �G���[:cERR_TRAP</returns>
    ''' <remarks></remarks>
    Private Function Put_OPTION_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' �f�[�^���v���[�g�f�[�^�\���̂֊i�[����
            If gbControllerInterlock Then
                With typPlateInfo
                    ' [OPTION]�f�[�^����������
                    writer.WriteLine(FILE_CONST_PLATE_OPTION)                        ' "[OPTION]"
                    writer.WriteLine(.intControllerInterlock.ToString("0"))         ' �O���@��ɂ��C���^�[���b�N�̗L��
                End With
            End If
            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_OPTION_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
    'V5.0.0.6�@��
#End Region

#Region "[CIRCUIT]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[CIRCUIT]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_CIRCUIT_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strMSG As String

        Try
            ' [CIRCUIT]�f�[�^����������(TKY�p)
            writer.WriteLine(CONST_CIRCUIT)                                     ' "[CIRCUIT]"
            If (gTkyKnd <> KND_TKY) Then                                        ' TKY�ȊO�Ȃ�NOP 
                Return (cFRS_NORMAL)
            End If

            If (typPlateInfo.intNGMark <> 0) Then
                For i = 1 To typPlateInfo.intCurcuitCnt
                    strDat = typCircuitInfoArray(i).intIP1.ToString("0") + ","                  ' IP�ԍ�
                    strDat = strDat + typCircuitInfoArray(i).dblIP2X.ToString("0.00000") + ","  ' �}�[�L���OX
                    strDat = strDat + typCircuitInfoArray(i).dblIP2Y.ToString("0.00000")        ' �}�[�L���OY
                    writer.WriteLine(strDat)
                Next i
            End If

            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_CIRCUIT_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[STEP]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[STEP]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_STEP_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [STEP]�f�[�^����������(CHIP�p)
            writer.WriteLine(FILE_CONST_STEPDATA)                               ' "[STEP]"
            If (gTkyKnd = KND_TKY) Then                                         ' TKY�Ȃ�NOP 
                Return (cFRS_NORMAL)
            End If

            For i = 1 To MaxStep
                strWAK = typStepInfoArray(i).intSP1.ToString("0")               ' �ï�ߔԍ�
                strDat = strWAK.PadLeft(3) + ","                                ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typStepInfoArray(i).intSP2.ToString("0")               ' ��ۯ���
                strDat = strDat + strWAK.PadLeft(3) + ","                       ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typStepInfoArray(i).dblSP3.ToString("0.00000")         ' �ï�ߊԲ������
                strDat = strDat + strWAK.PadLeft(8)                             ' 8����(�����ɋ󔒃p�f�B���O)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_STEP_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[GROUP]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[GROUP]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_GROUP_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [GROUP]�f�[�^����������(CHIP�p)
            writer.WriteLine(FILE_CONST_GRP_DATA)                               ' "[GROUP]"
            If (gTkyKnd <> KND_CHIP) Then                                       ' CHIP�ȊO�Ȃ�NOP 
                Return (cFRS_NORMAL)
            End If

            For i = 1 To MaxGrp
                strWAK = typGrpInfoArray(i).intGP1.ToString("0")                ' ��ٰ�ߔԍ�
                strDat = strWAK.PadLeft(3) + ","                                ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typGrpInfoArray(i).intGP2.ToString("0")                ' ��R��
                strDat = strDat + strWAK.PadLeft(3) + ","                       ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typGrpInfoArray(i).dblGP3.ToString("0.00000")          ' ��ٰ�ߊԲ������
                strDat = strDat + strWAK.PadLeft(8)                             ' 8����(�����ɋ󔒃p�f�B���O)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_GROUP_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[TY2]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[TY2]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_TY2_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [TY2]�f�[�^����������(CHIP�p)
            writer.WriteLine(FILE_CONST_TY2_DATA)                               ' "[TY2]"
            If (gTkyKnd <> KND_CHIP) Then                                       ' CHIP�ȊO�Ȃ�NOP 
                Return (cFRS_NORMAL)
            End If

            For i = 1 To MaxTy2
                strWAK = typTy2InfoArray(i).intTy21.ToString("0")               ' ��ۯ��ԍ�
                strDat = strWAK.PadLeft(3) + ","                                ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typTy2InfoArray(i).dblTy22.ToString("0.00000")         ' �ï�ߋ���
                strDat = strDat + strWAK.PadLeft(8)                             ' 8����(�����ɋ󔒃p�f�B���O)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_TY2_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[CIRCUIT AXIS]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[CIRCUIT AXIS]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_CIRCUITAXIS_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim CirNum As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [CIRCUIT AXIS]�f�[�^����������(NET�p)
            writer.WriteLine(FILE_CONST_CIRN_DATA)                              ' "[CIRCUIT AXIS]"
            If (gTkyKnd <> KND_NET) Then                                        ' NET�ȊO�Ȃ�NOP 
                Return (cFRS_NORMAL)
            End If

            CirNum = typPlateInfo.intCircuitCntInBlock
            For i = 1 To CirNum
                strWAK = typCirAxisInfoArray(i).intCaP1.ToString("0")           ' �ï�ߔԍ�
                strDat = strWAK.PadLeft(3) + ","                                ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typCirAxisInfoArray(i).dblCaP2.ToString("0.00000")     ' ���WX
                strDat = strDat + strWAK.PadLeft(8) + ","                       ' 8����(�����ɋ󔒃p�f�B���O)
                strWAK = typCirAxisInfoArray(i).dblCaP3.ToString("0.00000")     ' ���WY
                strDat = strDat + strWAK.PadLeft(8)                             ' 8����(�����ɋ󔒃p�f�B���O)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_CIRCUITAXIS_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "[CIRCUIT INTERVAL]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[CIRCUIT INTERVAL]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_CIRCUITINTERVAL_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim i As Integer
        Dim GrpNum As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' [CIRCUIT INTERVAL]�f�[�^����������(NET�p)
            writer.WriteLine(FILE_CONST_CIRIDATA)                               ' "[CIRCUIT INTERVAL]"
            If (gTkyKnd <> KND_NET) Then                                        ' NET�ȊO�Ȃ�NOP 
                Return (cFRS_NORMAL)
            End If

            If (typPlateInfo.intCircuitCntInBlock = 0) Then
                GrpNum = typPlateInfo.intGroupCntInBlockXBp                     ' ��ٰ�ߐ�X
            Else
                GrpNum = typPlateInfo.intGroupCntInBlockYStage                  ' ��ٰ�ߐ�Y
            End If

            For i = 1 To GrpNum
                strWAK = typCirInInfoArray(i).intCiP1.ToString("0")             ' �ï�ߔԍ�
                strDat = strWAK.PadLeft(3) + ","                                ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typCirInInfoArray(i).intCiP2.ToString("0")             ' ����Đ�
                strDat = strDat + strWAK.PadLeft(3) + ","                       ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typCirInInfoArray(i).dblCiP3.ToString("0.00000")       ' ����ĊԲ������
                strDat = strDat + strWAK.PadLeft(8)                             ' 8����(�����ɋ󔒃p�f�B���O)
                writer.WriteLine(strDat)
            Next i

            Return (cFRS_NORMAL)                                                ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_CIRCUITINTERVAL_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                  ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "��R�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>��R�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_RESIST_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim Rn As Integer
        Dim Count As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' ��R�f�[�^����������
            'Call GetChipNum(Count)                                                         ' ��R���擾
            Count = typPlateInfo.intResistCntInBlock                                        ' ��R���擾
            writer.WriteLine(FILE_CONST_RESISTOR)                                           ' "[RESISTOR]"

            For Rn = 1 To Count
                ' ��R�f�[�^����������
                strWAK = typResistorInfoArray(Rn).intResNo.ToString("0")                    ' ��R�ԍ�
                strDat = strWAK.PadLeft(4) + ","                                            ' 4����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intResMeasMode.ToString("0")              ' ���胂�[�h(0:��R ,1:�d�� ,2:�O��) 
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intResMeasType.ToString("0")              ' ����^�C�v(0:���� ,1:�����x)�@���ǉ�
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCircuitGrp.ToString("0")               ' ��������Ĕԍ�
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbHiNo.ToString("0")                 ' ��۰�ޔԍ�(HI)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbLoNo.ToString("0")                 ' ��۰�ޔԍ�(LO)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbAGNo1.ToString("0")                ' ��۰�ޔԍ�(AG1)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbAGNo2.ToString("0")                ' ��۰�ޔԍ�(AG2)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbAGNo3.ToString("0")                ' ��۰�ޔԍ�(AG3)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbAGNo4.ToString("0")                ' ��۰�ޔԍ�(AG4)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intProbAGNo5.ToString("0")                ' ��۰�ޔԍ�(AG5)
                strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).strExternalBits                           ' EXTERNAL BITS(16�ޯ�)
                strDat = strDat + strWAK.PadLeft(17) + ","                                  ' 17����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intPauseTime.ToString("0")                ' �߰�����(msec)
                strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intTargetValType.ToString("0")            ' �ڕW�l�w��i0:��Βl,1:���V�I,2:�v�Z���j
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intBaseResNo.ToString("0")                ' �ް���R�ԍ�
                strDat = strDat + strWAK.PadLeft(4) + ","                                   ' 4����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblTrimTargetVal.ToString("0.000000")     ' ���ݸޖڕW�l
                '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
                ' �ڕW�l = �ڕW�l�I�t�Z�b�g�L���Ȃ����ݸޖڕW�l�ޔ��悩��ݒ肷��
                If (giTargetOfs = 1) Then                                                   ' �ڕW�l�I�t�Z�b�g�L�� ? 
                    strWAK = typResistorInfoArray(Rn).dblTrimTargetVal_Save.ToString("0.000000")
                End If
                '----- V4.11.0.0�@�� -----
                strDat = strDat + strWAK.PadLeft(16) + ","                                  ' 16����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Hi_X.ToString("0.00000")  ' �v���[�u�m�F�ʒu HI X���W 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Hi_Y.ToString("0.00000")  ' �v���[�u�m�F�ʒu HI Y���W 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Lo_X.ToString("0.00000")  ' �v���[�u�m�F�ʒu LO X���W 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblProbCfmPoint_Lo_Y.ToString("0.00000")  ' �v���[�u�m�F�ʒu LO Y���W 
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intSlope.ToString("0")                    ' �d���ω��۰�� 
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblInitTest_HighLimit.ToString("0.00")    ' �Ƽ��ý�(HIGH�Я�)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblInitTest_LowLimit.ToString("0.00")     ' �Ƽ��ý�(Low�Я�)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblFinalTest_HighLimit.ToString("0.00")   ' ̧���ý�(HIGH�Я�)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblFinalTest_LowLimit.ToString("0.00")    ' ̧���ý�(Low�Я�)
                strDat = strDat + strWAK.PadLeft(7) + ","                                   ' 7����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intInitialOkTestDo.ToString("0")          ' �Ƽ��OKý�(0:���Ȃ�,1:����)���ǉ�(�v���[�g�f�[�^����ړ�)
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCutReviseMode.ToString("0")            ' ��ĕ␳(0:����, 1:����)     
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCutReviseDispMode.ToString("0")        ' �\��Ӱ��(0:����, 1:CRT)     
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCutRevisePtnNo.ToString("0")           ' ����ݔԍ�
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCutReviseGrpNo.ToString("0")           ' ����ݸ�ٰ�ߔԍ�    
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblCutRevisePosX.ToString("0.00000")      ' �␳�ʒuX
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblCutRevisePosY.ToString("0.00000")      ' �␳�ʒuY
                strDat = strDat + strWAK.PadLeft(9) + ","                                   ' 9����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intIsNG.ToString("0")                     ' �摜�F��NG����(0:����, 1:�Ȃ�, �蓮)
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCutCount.ToString("0")                 ' ��Đ�
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strDat = strDat + typResistorInfoArray(Rn).strRatioTrimTargetVal            ' ���V�I�v�Z��
                '----- V1.13.0.0�A�� -----
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).intCvMeasNum.ToString("0")                ' CV �ő呪���
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intCvMeasTime.ToString("0")               ' CV �ő呪�莞��(ms)
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblCvValue.ToString("0.000000")           ' CV CV�l  
                strDat = strDat + strWAK.PadLeft(10) + ","                                  ' 10����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).intOverloadNum.ToString("0")              ' ���ް۰�� �� 
                strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblOverloadMin.ToString("0.00")           ' ���ް۰�� �����l  
                strDat = strDat + strWAK.PadLeft(8) + ","                                   ' 8����(�����ɋ󔒃p�f�B���O)
                strWAK = typResistorInfoArray(Rn).dblOverloadMax.ToString("0.00")           ' ���ް۰�� ����l
                strDat = strDat + strWAK.PadLeft(8)                                         ' 8����(�����ɋ󔒃p�f�B���O)
                '----- V1.13.0.0�A�� -----
                '----- V2.0.0.0_23�� -----
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).wPauseTimeFT.ToString("0")                ' FT�O�̃|�[�Y�^�C��
                strDat = strDat + strWAK.PadLeft(6)                                         ' 6����(�����ɋ󔒃p�f�B���O)
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).intInsideEndChkCount.ToString("0")        ' ���؂蔻���
                strDat = strDat + strWAK.PadLeft(6)                                         ' 6����(�����ɋ󔒃p�f�B���O)
                strDat = strDat + ","
                strWAK = typResistorInfoArray(Rn).dblInsideEndChgRate.ToString("0.00")      ' ���؂蔻��ω���(0.00-100.00%)
                strDat = strDat + strWAK.PadLeft(8)                                         ' 6����(�����ɋ󔒃p�f�B���O)
                '----- V2.0.0.0_23�� -----
                '----- V4.11.0.0�@�� (WALSIN�aSL436S�Ή�) -----
                strDat = strDat + ","
                ''V5.0.0.2�@ ���ݸ                strWAK = typResistorInfoArray(Rn).dblTrimTargetOfs.ToString("0.000000")     ' �ڕW�l�I�t�Z�b�g
                strWAK = typResistorInfoArray(Rn).dblTrimTargetOfs_Save.ToString("0.000000")     ' �ڕW�l�I�t�Z�b�g
                strDat = strDat + strWAK.PadLeft(16)                                        ' 16����(�����ɋ󔒃p�f�B���O)
                '----- V4.11.0.0�@�� -----

                writer.WriteLine(strDat)
            Next Rn

            Return (cFRS_NORMAL)                                                            ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_RESIST_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                              ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#Region "�J�b�g�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>�J�b�g�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_CUT_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim RegCount As Integer
        Dim Rn As Integer
        Dim Cn As Integer
        Dim i As Integer
        Dim strDat As String
        Dim strWAK As String
        Dim strMSG As String

        Try
            ' �J�b�g�f�[�^����������
            'Call GetChipNum(RegCount)                                                              ' ��R���擾
            RegCount = typPlateInfo.intResistCntInBlock                                             ' ��R���擾
            writer.WriteLine(FILE_CONST_CUT_DATA)                                                   ' "[CUT]"

            For Rn = 1 To RegCount ' ��R�����J��Ԃ� 
                ' �J�b�g�f�[�^����������
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount ' �J�b�g�����J��Ԃ� 
                    strWAK = typResistorInfoArray(Rn).intResNo.ToString("0")                        ' ��R�ԍ�
                    strDat = strWAK.PadLeft(4) + ","                                                ' 4����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutNo.ToString("0")             ' ��Ĕԍ�
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intDelayTime.ToString("0")         ' �ިڲ���
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblTeachPointX.ToString("0.00000") ' è��ݸ��߲��X
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblTeachPointY.ToString("0.00000") ' è��ݸ��߲��Y
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX.ToString("0.00000") ' �����߲��X
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY.ToString("0.00000") ' �����߲��Y
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed.ToString("0.0")        ' ��Ľ�߰��
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate.ToString("0.0")           ' Q����ڰ�
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutOff.ToString("0.000")        ' ��ĵ̒l
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblJudgeLevel.ToString("0.0")      ' �ް�����i���ω����j�
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutOffOffset.ToString("0.000")  ' ��ĵ� �̾��
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).strCutType                         ' ��Č`��
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutDir.ToString("0")            ' ��ĕ���  �����g�p �΂߶�Ă̐؂�o���p�x���g�p
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intLTurnDir.ToString("0")          ' L��ݕ���(1:CW, 2:CCW) ���ύX
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLength.ToString("0.00000") ' �ő嶯èݸޒ�
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblR1.ToString("0.00000")          ' R1
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblLTurnPoint.ToString("0.0")      ' L����߲��
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLengthL.ToString("0.00000") ' L��݌�̍ő嶯èݸޒ�
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblR2.ToString("0.00000")          ' R2
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLengthHook.ToString("0.00000") ' ̯���݌�̶�èݸޒ�
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intIndexCnt.ToString("0")          ' ���ޯ����
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intMeasMode.ToString("0")         ' ����Ӱ�� 2011.08.30
                    'strDat = strDat + strWAK.PadLeft(2) + ","                                      ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intMeasType.ToString("0")          ' ����^�C�v(0:���� ,1:�����x, 2:�O��))��IX�p�
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)

                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed2.ToString("0.0")       ' ��Ľ�߰��2
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2.ToString("0.0")          ' Q����ڰ�2
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutAngle.ToString("0")          ' �΂߶�Ă̐؂�o���p�x
                    strDat = strDat + strWAK.PadLeft(4) + ","                                       ' 4����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblPitch.ToString("0.00000")       ' �߯�
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intStepDir.ToString("0")           ' �ï�ߕ���
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutCnt.ToString("0")            ' �{��
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)

                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESPoint.ToString("0.0000")      ' ���޾ݽ�߲��
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    ' ������ V3.1.0.0�B 2014/12/03
                    ''strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESJudgeLevel.ToString("0.0")   ' ���޾ݽ�̔���ω���           'V3.0.0.0�H
                    ''strDat = strDat + strWAK.PadLeft(5) + ","                                      ' 5����(�����ɋ󔒃p�f�B���O)   'V3.0.0.0�H
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESJudgeLevel.ToString("0.000")  ' ���޾ݽ�̔���ω���           'V3.0.0.0�H
                    'strDat = strDat + strWAK.PadLeft(7) + ","                                       ' 7����(�����ɋ󔒃p�f�B���O)   'V3.0.0.0�H
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESJudgeLevel.ToString("0.0")   ' ���޾ݽ�̔���ω���           'V3.0.0.0�H
                    strDat = strDat + strWAK.PadLeft(5) + ","                                      ' 5����(�����ɋ󔒃p�f�B���O)   'V3.0.0.0�H
                    ' ������ V3.1.0.0�B 2014/12/03
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblMaxCutLengthES.ToString("0.000")  ' ���޾ݽ��̶�Ē� 'V1.14.0.0�@
                    strDat = strDat + strWAK.PadLeft(7) + ","                                       ' 7����(�����ɋ󔒃p�f�B���O)
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.0")  ' ���޾ݽ��ω���
                    ' ������ V3.1.0.0�B 2014/12/03
                    ''strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.00") ' ���޾ݽ��ω����@             'V3.0.0.0�H V1.14.0.1�A 
                    ''strDat = strDat + strWAK.PadLeft(6) + ","                                      ' 6����(�����ɋ󔒃p�f�B���O)   'V3.0.0.0�H
                    'strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.000") ' ���޾ݽ��ω����@             'V3.0.0.0�H
                    'strDat = strDat + strWAK.PadLeft(7) + ","                                       ' 7����(�����ɋ󔒃p�f�B���O)   'V3.0.0.0�H
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblESChangeRatio.ToString("0.000") ' ���޾ݽ��ω����@             'V3.0.0.0�H
                    strDat = strDat + strWAK.PadLeft(6) + ","                                      ' 6����(�����ɋ󔒃p�f�B���O)   'V3.0.0.0�H
                    ' ������ V3.1.0.0�B 2014/12/03
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intESConfirmCnt.ToString("0")      ' ���޾ݽ��̊m�F��
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intRadderInterval.ToString("0")    ' ��ް�ԋ���
                    strDat = strDat + strWAK.PadLeft(4) + ","                                       ' 4����(�����ɋ󔒃p�f�B���O)
                    '----- V1.14.0.0�@�� -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCTcount.ToString("0")           ' ���޾ݽ��A��NG�m�F��
                    strDat = strDat + strWAK.PadLeft(4) + ","                                       ' 4����(�����ɋ󔒃p�f�B���O)
                    '----- V1.14.0.0�@�� -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblZoom.ToString("0.00")           ' �{��
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)

                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intMoveMode.ToString("0")          ' ���샂�[�h
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intDoPosition.ToString("0")        ' �|�W�V���j���O
                    strDat = strDat + strWAK.PadLeft(2) + ","                                       ' 2����(�����ɋ󔒃p�f�B���O)
                    '----- V2.0.0.0_24(V1.18.0.3�A)�� -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).intCutAftPause.ToString("0")       ' �J�b�g��|�[�Y�^�C��
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    '----- V2.0.0.0_24(V1.18.0.3�A)�� -----
                    '----- V1.16.0.0�@�� -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblReturnPos.ToString("0.00000")   ' ���^�[���J�b�g�̃��^�[���ʒu
                    strDat = strDat + strWAK.PadLeft(9) + ","                                       ' 9����(�����ɋ󔒃p�f�B���O)
                    '----- V1.16.0.0�@�� -----
                    '----- V1.18.0.0�C�� -----
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblLimitLen.ToString("0.00000")    ' IX�J�b�g�̃��~�b�g��
                    strDat = strDat + strWAK.PadLeft(8) + ","                                       ' 8����(�����ɋ󔒃p�f�B���O)
                    '----- V1.18.0.0�C�� -----

                    ' FL�p�f�[�^
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed3.ToString("0.0")       ' ��Ľ�߰��3
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed4.ToString("0.0")       ' ��Ľ�߰��4
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed5.ToString("0.0")       ' ��Ľ�߰��5
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblCutSpeed6.ToString("0.0")       ' ��Ľ�߰��6
                    strDat = strDat + strWAK.PadLeft(6) + ","                                       ' 6����(�����ɋ󔒃p�f�B���O)

                    For i = 0 To (cCNDNUM - 1)                                                      ' ���H�����ԍ�1�`n(0�ؼ��)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(i).ToString("0")
                        strDat = strDat + strWAK.PadLeft(2) + ","                                   ' 2����(�����ɋ󔒃p�f�B���O)
                    Next i

                    '----- V2.0.0.0_23�� -----
                    ' SL436S�p(�V���v���g���}�p)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3.ToString("0.0")          ' Q����ڰ�3
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4.ToString("0.0")          ' Q����ڰ�4
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5.ToString("0.0")          ' Q����ڰ�5
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)
                    strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6.ToString("0.0")          ' Q����ڰ�6
                    strDat = strDat + strWAK.PadLeft(5) + ","                                       ' 5����(�����ɋ󔒃p�f�B���O)
                    '----- V2.0.0.0_23�� -----

                    ' �ڕW�p���[�Ƌ��e�͈�(Ver10.01�Œǉ�) ###066
                    For i = 0 To (cCNDNUM - 1)                                                      ' ���H�����ԍ�1�`n(0�ؼ��)
                        '----- V2.0.0.0_23�� -----
                        ' SL436S�p(�V���v���g���}�p)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(i).ToString("0")     ' �d���l1�`8
                        strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6����(�����ɋ󔒃p�f�B���O)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(i).ToString("0")        ' STEG1�`8
                        strDat = strDat + strWAK.PadLeft(3) + ","                                   ' 3����(�����ɋ󔒃p�f�B���O)
                        '----- V2.0.0.0_23�� -----
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(i).ToString("0.00") ' ###071
                        strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6����(�����ɋ󔒃p�f�B���O)
                        strWAK = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(i).ToString("0.00")
                        strDat = strDat + strWAK.PadLeft(6) + ","                                   ' 6����(�����ɋ󔒃p�f�B���O)
                    Next i

                    ' ������f�[�^
                    strDat = strDat + typResistorInfoArray(Rn).ArrCut(Cn).strChar + ","             ' ������
                    strDat = strDat + typResistorInfoArray(Rn).ArrCut(Cn).strDataName + ","         ' U�J�b�g�f�[�^�����ǉ� 

                    writer.WriteLine(strDat)
                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_CUT_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- ###229�� -----
#Region "[GPIB]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[GPIB]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_GPIB_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String

        Try
            ' [GPIB]�f�[�^����������
            writer.WriteLine(FILE_CONST_GPIB_DATA)                      ' "[GPIB]"
            With typGpibInfo
                writer.WriteLine(.wGPIBmode.ToString("0"))              ' GP-IB����(0:���Ȃ� 1:����)
                writer.WriteLine(.wDelim.ToString("0"))                 ' �����(0:CR+LF 1:CR 2:LF 3:NONE)
                writer.WriteLine(.wTimeout.ToString("0"))               ' ��ѱ��(1�`32767)(ms�P��)
                writer.WriteLine(.wAddress.ToString("0"))               ' �@����ڽ(0�`30)
                writer.WriteLine(.wEOI.ToString("0"))                   ' EOI(0:�g�p���Ȃ�, 1:�g�p����)
                writer.WriteLine(.wPause1.ToString("0"))                ' �ݒ�����1���M��|�[�Y����(1�`32767msec)
                writer.WriteLine(.wPause2.ToString("0"))                ' �ݒ�����2���M��|�[�Y����(1�`32767msec)
                writer.WriteLine(.wPause3.ToString("0"))                ' �ݒ�����3���M��|�[�Y����(1�`32767msec)
                writer.WriteLine(.wPauseT.ToString("0"))                ' �ض޺���ޑ��M��|�[�Y����(1�`32767msec)
                writer.WriteLine(.wRev.ToString("0"))                   ' �\��

                writer.WriteLine(.strI)                                 ' �����������(MAX40byte)
                writer.WriteLine(.strI2)                                ' �����������2(MAX40byte)
                writer.WriteLine(.strI3)                                ' �����������3(MAX40byte)
                writer.WriteLine(.strT)                                 ' �ض޺����(50byte)
                writer.WriteLine(.strName)                              ' �@�햼(10byte)
                writer.WriteLine(.wReserve)                             ' �\��(8byte) 
            End With

            Return (cFRS_NORMAL)                                        ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_GPIB_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- ###229�� -----

#Region "[SINSYUKU]�f�[�^���������ށyTKY,CHIP,NET�����Łz"
    '''=========================================================================
    '''<summary>[SINSYUKU]�f�[�^����������</summary>
    '''<param name="writer">(INP)StreamWriter�I�u�W�F�N�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function Put_SINSYUKU_Data(ByVal writer As System.IO.StreamWriter) As Integer

        Dim strMSG As String
        Dim x As Integer
        Dim y As Integer
        Dim writeCount As Integer
        Dim writeStr As String

        Try
            writeCount = 0
            writeStr = ""
            ' [SINSYUKU]�f�[�^����������
            'V5.0.0.8�@            writer.WriteLine(FILE_SINSYUKU_SELECT)                      ' "[SINSYUKU]"
            writer.WriteLine(FILE_CONST_SHINSYUKUDATA)                      ' "[SINSYUKU]"
            For x = 0 To typPlateInfo.intBlockCntXDir - 1
                For y = 0 To typPlateInfo.intBlockCntYDir - 1
                    If SelectBlock(x, y) <> 0 Then
                        If writeStr <> "" And Right(writeStr, 1) <> "," Then
                            writeStr = writeStr + ","
                        End If
                        writeStr = writeStr + CStr(x + 1) + "-" + CStr(y + 1)
                        writeCount = writeCount + 1
                        If writeCount > 10 Then
                            writer.WriteLine(writeStr)
                            writeCount = 0
                            writeStr = ""
                        End If
                    End If
                Next y
            Next x
            If writeCount <= 10 Then     ' �܂���������ł��Ȃ��f�[�^������ꍇ�����ŏ����o��
                writer.WriteLine(writeStr)
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Put_SINSYUKU_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
    '----- ###229�� -----
#End If
#End Region 'V5.0.0.8�@

#End Region

    '======================================================================
    '  ���ʊ֐�
    '======================================================================
#Region "�y���ʊ֐��z"
#Region "�t�@�C���o�[�W�����擾�y���ʁz"
    '''=========================================================================
    '''<summary>�t�@�C���o�[�W�����擾�y���ʁz</summary>
    '''<param name="strPath">(INP)�t�@�C����</param>
    '''<param name="dblVer"> (OUT)�t�@�C���o�[�W������</param> 
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function File_Read_Ver(ByVal strPath As String, ByRef dblVer As Double) As Integer

        'Dim intFileNo As Integer                                    ' �t�@�C���ԍ�
        'Dim iFlg As Integer
        Dim r As Integer
        Dim intType As Integer
        Dim Pos As Integer
        Dim strDAT As String
        Dim strVer As String
        Dim strMSG As String

        Try
            ' ��������
            r = cFRS_FIOERR_INP                                     ' Return�l = �G���[
            intType = -1                                            ' �f�[�^���
            FILETYPE_K = ""                                         ' SL436K�̃t�@�C���o�[�W���� V1.23.0.0�G

            If (False = IO.File.Exists(strPath)) Then Throw New FileNotFoundException()

            Using sr As New StreamReader(strPath, Encoding.UTF8)    ' ̧���ް�ޮ݂̕���ASCII�Ȃ̂�UTF8��OK     V4.4.0.0-1
                ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ��܂��B
                Do While (False = sr.EndOfStream)
                    strDAT = sr.ReadLine()
                    Select Case strDAT
                        Case CONST_VERSION                              ' �t�@�C���o�[�W����
                            intType = 0

                        Case Else
                            If (intType = 0) Then                       '[FILE VERSION]�f�[�^ ?
                                ' "TKYDATA_Ver4_SP1"�Ȃ�"TKYDATA_Ver4"�Ƃ���
                                If (strDAT = CONST_FILETYPE4_SP1) Then
                                    strDAT = CONST_FILETYPE4
                                End If
                                '----- V1.23.0.0�G�� -----
                                '----- CHIP/NET�p(SL436K) -----
                                If (strDAT = FILETYPE04_K) Then
                                    FILETYPE_K = FILETYPE04_K
                                End If
                                '----- V1.23.0.0�G�� -----
                                ' �t�@�C���o�[�W��������double�^�ɕύX����
                                Pos = strDAT.IndexOf("Ver")             ' "Ver"��������B�Ȃ���΃G���[�߂�
                                If (Pos = -1) Then Exit Do
                                strVer = strDAT.Substring(Pos + 3, strDAT.Length - (Pos + 3))
                                '----- V1.14.0.0�E�� -----
                                If (strVer = "7.0.0.2") Then            ' V7.0.0.2�Ȃ� 
                                    dblVer = 7.02                       ' V7.02�Ƃ��� 
                                Else
                                    dblVer = Double.Parse(strVer)       ' �t�@�C���o�[�W��������double�^�ɕύX����
                                End If
                                'dblVer = Double.Parse(strVer)          ' �t�@�C���o�[�W��������double�^�ɕύX����
                                '----- V1.14.0.0�E�� -----
                                r = cFRS_NORMAL                         ' Return�l = ����
                                Exit Try
                            Else
                                Exit Do
                            End If
                    End Select
                Loop
            End Using

            ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.File_Read_Ver() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return�l = ��O�G���[
            'GoTo STP_EXT
        End Try

        Return (r)

#If False Then
        Try
            ' ��������
            r = cFRS_FIOERR_INP                                     ' Return�l = �G���[
            intType = -1                                            ' �f�[�^���
            FILETYPE_K = ""                                         ' SL436K�̃t�@�C���o�[�W���� V1.23.0.0�G
            iFlg = 0
            ' �e�L�X�g�t�@�C�����I�[�v��
            intFileNo = FreeFile()                                  ' �g�p�\�ȃt�@�C���i���o�[���擾
            FileOpen(intFileNo, strPath, OpenMode.Input)
            iFlg = 1

            ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ��܂��B
            Do While Not EOF(intFileNo)
                strDAT = LineInput(intFileNo)                       ' 1�s�ǂݍ���
                Select Case strDAT
                    Case CONST_VERSION                              ' �t�@�C���o�[�W����
                        intType = 0

                    Case Else
                        If (intType = 0) Then                       '[FILE VERSION]�f�[�^ ?
                            ' "TKYDATA_Ver4_SP1"�Ȃ�"TKYDATA_Ver4"�Ƃ���
                            If (strDAT = CONST_FILETYPE4_SP1) Then
                                strDAT = CONST_FILETYPE4
                            End If
                            '----- V1.23.0.0�G�� -----
                            '----- CHIP/NET�p(SL436K) -----
                            If (strDAT = FILETYPE04_K) Then
                                FILETYPE_K = FILETYPE04_K
                            End If
                            '----- V1.23.0.0�G�� -----
                            ' �t�@�C���o�[�W��������double�^�ɕύX����
                            Pos = strDAT.IndexOf("Ver")             ' "Ver"��������B�Ȃ���΃G���[�߂�
                            If (Pos = -1) Then Exit Do
                            strVer = strDAT.Substring(Pos + 3, strDAT.Length - (Pos + 3))
                            '----- V1.14.0.0�E�� -----
                            If (strVer = "7.0.0.2") Then            ' V7.0.0.2�Ȃ� 
                                dblVer = 7.02                       ' V7.02�Ƃ��� 
                            Else
                                dblVer = Double.Parse(strVer)       ' �t�@�C���o�[�W��������double�^�ɕύX����
                            End If
                            'dblVer = Double.Parse(strVer)          ' �t�@�C���o�[�W��������double�^�ɕύX����
                            '----- V1.14.0.0�E�� -----
                            r = cFRS_NORMAL                         ' Return�l = ����
                            GoTo STP_EXT
                        End If
                End Select
            Loop

            ' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)

            ' �I������
STP_EXT:
            If (iFlg = 1) Then
                FileClose(intFileNo)
            End If
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.File_Read_Ver() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                           ' Return�l = ��O�G���[
            GoTo STP_EXT
        End Try
#End If
    End Function
#End Region

#Region "LOAD�ESAVE�̋��ʉ��ɂ�薢�g�p"
#If False Then 'V5.0.0.8�@
#Region "�t�@�C���o�[�W�����̃`�F�b�N�yTKY�p�z"
    '''=========================================================================
    '''<summary>�t�@�C���o�[�W�����̃`�F�b�N�yTKY�p�z</summary>
    '''<param name="strFileVer">(INP) �o�[�W����</param>
    '''<returns>0=����, 1=�G���[ </returns>
    '''=========================================================================
    Private Function FileVerCheck_TKY(ByRef strFileVer As String) As Short

        Dim r As Short
        Dim strMSG As String

        Try
            r = cFRS_NORMAL                                         ' Return�l = ����
            Select Case strFileVer

                Case CONST_FILETYPE4
                Case CONST_FILETYPE4_SP1
                    SL432HW_FileVer = 4

                Case CONST_FILETYPE5
                    SL432HW_FileVer = 5

                Case CONST_FILETYPE10
                    SL432HW_FileVer = 10

                Case CONST_FILETYPE10_01
                    SL432HW_FileVer = 10.01                         ' ###229

                Case CONST_FILETYPE10_02
                    SL432HW_FileVer = 10.02                         ' ###229

                Case CONST_FILETYPE10_03
                    SL432HW_FileVer = 10.03                         ' ###229

                Case CONST_FILETYPE10_04                            'V1.14.0.0�E
                    SL432HW_FileVer = 10.04

                Case CONST_FILETYPE10_05                            'V1.16.0.0�@
                    SL432HW_FileVer = 10.05

                Case CONST_FILETYPE10_06                            'V1.18.0.0�C
                    SL432HW_FileVer = 10.06

                    '----- V2.0.0.0_23�� -----
                Case CONST_FILETYPE10_07
                    SL432HW_FileVer = 10.07

                Case CONST_FILETYPE10_072
                    SL432HW_FileVer = 10.072

                Case CONST_FILETYPE10_073
                    SL432HW_FileVer = 10.073
                    '----- V2.0.0.0_23�� -----

                Case CONST_FILETYPE10_09                            ' V4.0.0.0�C
                    SL432HW_FileVer = 10.09

                Case CONST_FILETYPE10_10                            'V4.10.0.0�C
                    SL432HW_FileVer = FILE_VER_10_10

                Case CONST_FILETYPE_CUR                             ' ###066
                    SL432HW_FileVer = FILE_VER_CUR

                Case Else
                    r = cFRS_FIOERR_INP                             ' Return�l = NG
            End Select
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.FileVerCheck_TKY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region
#End If
#End Region 'V5.0.0.8�@

#Region "�t�@�C���o�[�W�����̃`�F�b�N�yCHIP�p�z"
    '''=========================================================================
    '''<summary>�t�@�C���o�[�W�����̃`�F�b�N�yCHIP�p�z</summary>
    '''<param name="strFileVer">(INP) �o�[�W����</param>
    '''<returns>0=����, 1=�G���[ </returns>
    '''=========================================================================
    Private Function FileVerCheck_CHIP(ByRef strFileVer As String) As Short

        FileVerCheck_CHIP = cFRS_NORMAL                                 ' Return�l = ����
        Select Case strFileVer
            Case FILETYPE_CHIP01
                FileIO.FileVersion = 1
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & Space(4) & "�E�Ǝ�" & vbCrLf & Space(4) & "�ELED����" & vbCrLf & Space(4) & "�EGP-IB����" & vbCrLf & Space(2) & "�y�O���[�v�f�[�^�z" & vbCrLf & Space(4) & "�E��R��" & vbCrLf & Space(4) & "�E�O���[�v�ԃC���^�[�o��", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA" & vbCrLf & Space(4) & "* LED Control" & vbCrLf & Space(4) & "* GP-IB" & vbCrLf & Space(2) & "[GROUP DATA]" & vbCrLf & Space(4) & "* Resistor Number" & vbCrLf & Space(4) & "* Group Interval", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008 & vbCrLf & Space(4) & File_009 & vbCrLf & Space(4) & File_010 & vbCrLf & Space(2) & File_011 & vbCrLf & Space(4) & File_012 & vbCrLf & Space(4) & File_013, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP02
                FileIO.FileVersion = 2
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & Space(4) & "�E�Ǝ�" & vbCrLf & Space(4) & "�EGP-IB����" & vbCrLf & Space(2) & "�y�O���[�v�f�[�^�z" & vbCrLf & Space(4) & "�E��R��" & vbCrLf & Space(4) & "�E�O���[�v�ԃC���^�[�o��", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA" & vbCrLf & Space(4) & "* GP-IB" & vbCrLf & Space(2) & "[GROUP DATA]" & vbCrLf & Space(4) & "* Resistor Number" & vbCrLf & Space(4) & "* Group Interval", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008 & vbCrLf & Space(4) & File_010 & vbCrLf & Space(2) & File_011 & vbCrLf & Space(4) & File_012 & vbCrLf & Space(4) & File_013, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP03
                FileIO.FileVersion = 3
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & Space(4) & "�E�Ǝ�" & vbCrLf & Space(4) & "�EGP-IB����", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA" & vbCrLf & Space(4) & "* GP-IB", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008 & vbCrLf & Space(4) & File_010, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP04
                FileIO.FileVersion = 4
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & Space(4) & "�EGP-IB����", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* GP-IB", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_010, MsgBoxStyle.OkOnly)

            Case FILETYPE_CHIP05
                FileIO.FileVersion = 5 ' V1.40
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ? V6.1.4.0�K
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                ' KOA(EW)�a�Ȃ�ү���ޕ\���Ȃ�
                If (gSysPrm.stCTM.giSPECIAL = customKOAEW) Then
                Else
                    'If gSysPrm.stTMN.giMsgTyp = 0 Then
                    '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & Space(4) & "�E�s�ƃI�t�Z�b�g" & vbCrLf & Space(4) & "�E�s�Ɗ�ʒu�PXY" & vbCrLf & Space(4) & "�E�s�Ɗ�ʒu�QXY" & vbCrLf & Space(4) & "�E�p���[�������[�h" & vbCrLf & Space(4) & "�E�����ڕW�p���[" & vbCrLf & Space(4) & "�E�p���[���� �p���[�g" & vbCrLf & Space(4) & "�E�p���[�������e�͈�" & vbCrLf & Space(2) & "�y�J�b�g�f�[�^�z" & vbCrLf & Space(4) & "�E�J�b�g�I�t �I�t�Z�b�g", MsgBoxStyle.OkOnly)
                    'Else
                    '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* T_THETA OFFSET" & vbCrLf & Space(4) & "* T_THETA ADJUST POINT1XY" & vbCrLf & Space(4) & "* T_THETA ADJUST POINT2XY" & vbCrLf & Space(4) & "* POWER ADJUSTMENT MODE" & vbCrLf & Space(4) & "* ADJUSTMENT POWER" & vbCrLf & Space(4) & "* POWER ADJUSTMENT QRATE" & vbCrLf & Space(4) & "* ADJUSTMENT TOLERANCE" & vbCrLf & Space(2) & "[CUT DATA]" & vbCrLf & Space(4) & "* CUTOFF OFFSET", MsgBoxStyle.OkOnly)
                    'End If
                    MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_014 & vbCrLf & Space(4) & File_015 & vbCrLf & Space(4) & File_016 & vbCrLf & Space(4) & File_017 & vbCrLf & Space(4) & File_018 & vbCrLf & Space(4) & File_019 & vbCrLf & Space(4) & File_020 & vbCrLf & Space(2) & File_021 & vbCrLf & Space(4) & File_022, MsgBoxStyle.OkOnly)
                End If

                ' ̧�ٍ\���ύXVol.6
            Case FILETYPE_CHIP06
                FileIO.FileVersion = 6
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^ 2�z" & vbCrLf & Space(4) & "�E�␳�ʒu�p�^�[���̃O���[�v�ԍ�", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & Space(2) & "[PLATE DATA 2]" & vbCrLf & Space(4) & "* GroupNo. for revise pattern.", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & Space(2) & File_023 & vbCrLf & Space(4) & File_024, MsgBoxStyle.OkOnly)

                '----- V1.14.0.0�E�� -----
                ' ̧���ް�ޮ�V7.0.0.2
            Case FILETYPE_CHIP07_02
                FileIO.FileVersion = 7.02
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���ڊm�F��ASAVE���s���Ă��������B", MsgBoxStyle.OkOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation.", MsgBoxStyle.OkOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_025, MsgBoxStyle.OkOnly)
                '----- V1.14.0.0�E�� -----

#If False Then  'V5.0.0.8�@ FILE_VER_10 �ȍ~�� Form1.Sub_FileLoad() �� File_Read() �ŏ��������
            Case FILETYPE_CHIP10
                SL432HW_FileVer = FILE_VER_10

            Case FILETYPE_CHIP10_01
                SL432HW_FileVer = FILE_VER_10_01

            Case FILETYPE_CHIP10_02
                SL432HW_FileVer = FILE_VER_10_02

            Case FILETYPE_CHIP10_03
                SL432HW_FileVer = FILE_VER_10_03

            Case FILETYPE_CHIP10_04                              'V1.15.0.0�@
                SL432HW_FileVer = FILE_VER_10_04                     'V1.15.0.0�@

            Case FILETYPE_CHIP10_05                              'V1.16.0.0�@
                SL432HW_FileVer = FILE_VER_10_05                     'V1.16.0.0�@

            Case FILETYPE_CHIP10_06                              'V1.18.0.0�C
                SL432HW_FileVer = FILE_VER_10_06                     'V1.18.0.0�C
                '----- V2.0.0.0_23�� -----
            Case FILETYPE_CHIP10_07
                SL432HW_FileVer = FILE_VER_10_07
            Case FILETYPE_CHIP10_072
                SL432HW_FileVer = FILE_VER_10_072
            Case FILETYPE_CHIP10_073
                SL432HW_FileVer = FILE_VER_10_073
                '----- V2.0.0.0_23�� -----
            Case FILETYPE_CHIP10_08
                SL432HW_FileVer = FILE_VER_10_08

            Case FILETYPE_CHIP10_09                             'V4.0.0.0�C
                SL432HW_FileVer = FILE_VER_10_09

            Case FILETYPE_CHIP10_10                              'V4.10.0.0�C
                SL432HW_FileVer = FILE_VER_10_10

            Case FILETYPE_CHIP10_11
                SL432HW_FileVer = FILE_VER_10_11

            Case FILETYPE_CHIP_CUR                               '###066
                SL432HW_FileVer = FILE_VER_CUR
#End If
                '----- V1.23.0.0�G�� -----
                '----- CHIP/NET�p(SL436K) -----
            Case FILETYPE04_K                               ' TKYCHIP_SL436K_Ver1.20 
                FileIO.FileVersion = 1.2
                '----- V1.23.0.0�G�� -----

            Case Else
                'mouse pointer default
                Call SetMousePointer(Form1, False)
                '' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
                'Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, gAppName)
                FileVerCheck_CHIP = 1
        End Select

    End Function
#End Region

#Region "�t�@�C���o�[�W�����̃`�F�b�N�yNET�p�z"
    '''=========================================================================
    '''<summary>�t�@�C���o�[�W�����̃`�F�b�N�yNET�p�z</summary>
    '''<param name="strFileVer">(INP) �o�[�W����</param>
    '''<returns>0=����, 1=�G���[ </returns>
    '''=========================================================================
    Private Function FileVerCheck_NET(ByVal strFileVer As String) As Integer

        FileVerCheck_NET = cFRS_NORMAL                                  ' Return�l = ����
        Select Case strFileVer
            Case FILETYPE_NET01
                FileIO.FileVersion = 1
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & _
                '                           Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & _
                '                           Space(4) & "�E�O���[�v���w�x", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & _
                '                           vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* GROUP NUMBER XY", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & _
                                       Space(2) & File_007 & vbCrLf & _
                                       Space(4) & File_026, vbOKOnly)

            Case FILETYPE_NET02
                FileIO.FileVersion = 2
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & _
                '                           vbCrLf & vbCrLf & Space(2) & "�y�v���[�g�f�[�^�z" & vbCrLf & Space(4) & "�E�Ǝ�", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & _
                '                           vbCrLf & vbCrLf & Space(2) & "[PLATE DATA]" & vbCrLf & Space(4) & "* THETA", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & _
                                       vbCrLf & vbCrLf & Space(2) & File_007 & vbCrLf & Space(4) & File_008, vbOKOnly)

            Case FILETYPE_NET03
                FileIO.FileVersion = 3
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���L�Ɏ������ڂ��m�F��ASAVE���s���Ă��������B" & vbCrLf & vbCrLf & _
                '                            Space(2) & "�y�v���[�g�f�[�^ 2�z" & vbCrLf & _
                '                            Space(4) & "�E�␳�ʒu�p�^�[���̃O���[�v�ԍ�", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation in an item to show as follows." & vbCrLf & vbCrLf & _
                '                            Space(2) & "[PLATE DATA 2]" & vbCrLf & _
                '                            Space(4) & "* GroupNo. for revise pattern.", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_006 & vbCrLf & vbCrLf & _
                                        Space(2) & File_023 & vbCrLf & _
                                        Space(4) & File_024, vbOKOnly)

                '----- V1.14.0.0�E�� -----
            Case FILETYPE_NET07_02
                FileIO.FileVersion = 7.02
                '----- V6.1.4.0_44��(KOA EW�aSL432RD�Ή�) -----
                ' ���b�Z�[�W�\���Ȃ� ?
                If (giFileMsgNoDsp = 1) Then Return (cFRS_NORMAL)
                '----- V6.1.4.0_44�� -----
                ' MSG select
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�t�@�C���\�����ύX����Ă��܂��B" & vbCrLf & "���ڂ��m�F��ASAVE���s���Ă��������B", vbOKOnly)
                'Else
                '    MsgBox("File constitution is changed." & vbCrLf & "Please do SAVE after confirmation.", vbOKOnly)
                'End If
                MsgBox(File_005 & vbCrLf & File_025, vbOKOnly)
                '----- V1.14.0.0�E�� -----

#If False Then  'V5.0.0.8�@ FILE_VER_10 �ȍ~�� Form1.Sub_FileLoad() �� File_Read() �ŏ��������
            Case FILETYPE_NET10
                SL432HW_FileVer = FILE_VER_10

            Case FILETYPE_NET10_01
                SL432HW_FileVer = FILE_VER_10_01

            Case FILETYPE_NET10_02
                SL432HW_FileVer = FILE_VER_10_02

            Case FILETYPE_NET10_03
                SL432HW_FileVer = FILE_VER_10_03

            Case FILETYPE_NET10_04                              'V1.15.0.0�@
                SL432HW_FileVer = FILE_VER_10_04                     'V1.15.0.0�@

            Case FILETYPE_NET10_05                              'V1.16.0.0�@
                SL432HW_FileVer = FILE_VER_10_05                     'V1.16.0.0�@

            Case FILETYPE_NET10_06                              'V1.18.0.0�C
                SL432HW_FileVer = FILE_VER_10_06                     'V1.18.0.0�C
                '----- V2.0.0.0_23�� -----
            Case FILETYPE_NET10_07
                SL432HW_FileVer = FILE_VER_10_07
            Case FILETYPE_NET10_072
                SL432HW_FileVer = FILE_VER_10_072
            Case FILETYPE_NET10_073
                SL432HW_FileVer = FILE_VER_10_073
                '----- V2.0.0.0_23�� -----
            Case FILETYPE_NET10_08
                SL432HW_FileVer = FILE_VER_10_08

            Case FILETYPE_NET10_09                              'V4.0.0.0�C
                SL432HW_FileVer = FILE_VER_10_09                     'V4.0.0.0�C

            Case FILETYPE_NET10_10                              'V4.10.0.0�C
                SL432HW_FileVer = FILE_VER_10_10

            Case FILETYPE_NET10_11
                SL432HW_FileVer = FILE_VER_10_11

            Case FILETYPE_NET_CUR                               '###066
                SL432HW_FileVer = FILE_VER_CUR
#End If
            Case Else
                ' mouse pointer default
                Call SetMousePointer(Form1, False)
                '' "�w�肳�ꂽ�t�@�C���̓g���~���O�p�����[�^�̃f�[�^�ł͂���܂���"
                'Call Form1.System1.TrmMsgBox(gSysPrm, MSG_16, vbExclamation Or vbOKOnly, gAppName)
                FileVerCheck_NET = 1
        End Select
    End Function

#End Region

#Region "ϳ��߲���̍����v�\��(�ݒ�^����)"
    '''=========================================================================
    '''<summary>ϳ��߲���̍����v�\��(�ݒ�^����)</summary>
    '''<param name="frm"> (INP) �Ώ�̫��</param>
    '''<param name="mode">(INP) True(���s), False(����)</param>
    '''=========================================================================
    Public Sub SetMousePointer(ByRef frm As System.Windows.Forms.Form, ByRef mode As Boolean)

        ' mode check
        If mode = True Then
            ' �����v
            frm.Enabled = False
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        ElseIf mode = False Then
            ' default
            frm.Enabled = True
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End If
    End Sub
#End Region

#Region "�J�b�g��������΂߃J�b�g�̐؂�o���p�x��ݒ肷��y���ʁz"
    '''=========================================================================
    '''<summary>�J�b�g��������΂߃J�b�g�̐؂�o���p�x��ݒ肷��y���ʁz</summary>
    '''<param name="CutType">(INP)�J�b�g�`�� </param>
    '''<param name="Dir">    (INP)�J�b�g���� </param>
    '''<param name="Angle">  (OUT)�΂߃J�b�g�̐؂�o���p�x</param>
    '''=========================================================================
    Private Sub GetCutAngle(ByRef CutType As String, ByVal Dir As Short, ByRef Angle As Short)

        Dim strMSG As String

        Try
            Select Case (CutType)
                ' �΂߃J�b�g�̏ꍇ�͐ݒ肵�Ȃ�
                Case CNS_CUTP_NST                                                           ' �΂�ST�J�b�g 
                Case CNS_CUTP_NL                                                            ' �΂�L�J�b�g
                Case CNS_CUTP_NSTr                                                          ' �΂�ST�J�b�g(���^�[��)  
                Case CNS_CUTP_NLr                                                           ' �΂�L�J�b�g(���^�[��)    
                Case CNS_CUTP_NSTt                                                          ' �΂�ST�J�b�g(���g���[�X)     
                Case CNS_CUTP_NLt                                                           ' �΂�L�J�b�g(���g���[�X)    

                Case CNS_CUTP_C                                                             ' C�J�b�g(���T�|�[�g)  
                    strMSG = "File.GetCutAngle() Not Support Cut Type(C Cut)"
                    MessageBox.Show(strMSG, "", MessageBoxButtons.OK)
                '----- V6.1.4.0_49�� -----
                'Case CNS_CUTP_ES                                                            ' ��ES�J�b�g(���T�|�[�g)  
                '    strMSG = "File.GetCutAngle() Not Support Cut Type(ES Cut)"
                '    MessageBox.Show(strMSG, "", MessageBoxButtons.OK)
                '----- V6.1.4.0_49�� -----
                Case CNS_CUTP_NOP                                                           ' NOP(���T�|�[�g)  
                    strMSG = "File.GetCutAngle() Not Support Cut Type(Z Cut)"
                    MessageBox.Show(strMSG, "", MessageBoxButtons.OK)

                'Case CNS_CUTP_ST, CNS_CUTP_IX, CNS_CUTP_SC, CNS_CUTP_STr, CNS_CUTP_STt, CNS_CUTP_M, CNS_CUTP_ES2, CNS_CUTP_ST2, CNS_CUTP_IX2               ' V6.1.4.0_49
                Case CNS_CUTP_ST, CNS_CUTP_IX, CNS_CUTP_SC, CNS_CUTP_STr, CNS_CUTP_STt, CNS_CUTP_M, CNS_CUTP_ES2, CNS_CUTP_ST2, CNS_CUTP_IX2, CNS_CUTP_ES   ' V6.1.4.0_49
                    ' ST�J�b�g, �X�L�����J�b�g��
                    ' �J�b�g��������΂߃J�b�g�̐؂�o���p�x��ݒ肷�� 
                    Select Case (Dir)
                        Case 1      ' +X �� 0��
                            Angle = 0
                        Case 2      ' -Y �� 270��
                            Angle = 270
                        Case 3      ' -X �� 180��
                            Angle = 180
                        Case 4      ' +Y �� 90��
                            Angle = 90
                    End Select

                Case Else

            End Select

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.GetCutAngle() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�J�b�g��������΂߃J�b�g�̐؂�o���p�x��L�^�[��������ݒ肷��y���ʁz"
    '''=========================================================================
    '''<summary>�J�b�g��������΂߃J�b�g�̐؂�o���p�x��L�^�[��������ݒ肷��y���ʁz</summary>
    '''<param name="CutType"> (INP)�J�b�g�`�� </param>
    '''<param name="Dir">     (INP)�J�b�g���� </param>
    '''<param name="Angle">   (OUT)�΂߃J�b�g�̐؂�o���p�x</param>
    '''<param name="LTurnDir">(OUT)L��ݕ���(1:CW, 2:CCW)</param>
    '''=========================================================================
    Private Sub GetCutLTurnDir(ByRef CutType As String, ByVal Dir As Short, ByRef Angle As Short, ByRef LTurnDir As Short)

        Dim strMSG As String

        Try
            Select Case (CutType)
                Case CNS_CUTP_L, CNS_CUTP_HK, CNS_CUTP_Lr, CNS_CUTP_Lt, CNS_CUTP_U, CNS_CUTP_Ut 'V1.22.0.0�A
                    ' L�J�b�g/HOOK�J�b�g/u�J�b�g
                    ' �J�b�g��������΂߃J�b�g�̐؂�o���p�x��L��ݕ���(1:CW, 2:CCW)��ݒ肷�� 
                    Select Case (Dir)
                        Case 1                                          ' +X+Y  
                            Angle = 0                                   ' �؂�o���p�x = 0��
                            LTurnDir = 2                                ' L��ݕ���     = �����v���� 
                        Case 2                                          ' -Y+X 
                            Angle = 270                                 ' �؂�o���p�x = 0��
                            LTurnDir = 2                                ' L��ݕ���     = �����v���� 
                        Case 3                                          ' -X-Y
                            Angle = 180                                 ' �؂�o���p�x = 0��
                            LTurnDir = 2                                ' L��ݕ���     = �����v���� 
                        Case 4                                          ' +Y-X
                            Angle = 90                                  ' �؂�o���p�x = 0��
                            LTurnDir = 2                                ' L��ݕ���     = �����v���� 
                        Case 5                                          ' +X-Y
                            Angle = 0                                   ' �؂�o���p�x = 0��
                            LTurnDir = 1                                ' L��ݕ���     = ���v���� 
                        Case 6                                          ' -Y-X
                            Angle = 270                                 ' �؂�o���p�x = 0��
                            LTurnDir = 1                                ' L��ݕ���     = ���v���� 
                        Case 7                                          ' -X+Y
                            Angle = 180                                 ' �؂�o���p�x = 0��
                            LTurnDir = 1                                ' L��ݕ���     = ���v���� 
                        Case 8                                          ' +Y+X
                            Angle = 90                                  ' �؂�o���p�x = 0��
                            LTurnDir = 1                                ' L��ݕ���     = ���v���� 
                    End Select

                Case CNS_CUTP_NL, CNS_CUTP_NLr, CNS_CUTP_NLt
                    ' �΂�L�J�b�g
                    LTurnDir = Dir                                      ' L��ݕ���     = �J�b�g����(1:CW, 2:CCW)
                Case Else

            End Select

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.GetCutLTurnDir() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�X�e�b�v������ϊ�����y���ʁz"
    '''=========================================================================
    '''<summary>�X�e�b�v������ϊ�����y���ʁz</summary>
    '''<param name="CutType">(INP)�J�b�g�`�� </param>
    '''<param name="Dir">    (INP)�X�e�b�v���� </param>
    '''<param name="Stp">    (OUT)�X�e�b�v����</param>
    '''=========================================================================
    Private Sub GetStepDir(ByRef CutType As String, ByVal Dir As Short, ByRef Stp As Short)

        Dim strMSG As String

        Try
            Select Case (CutType)
                Case CNS_CUTP_SC
                    ' �X�L�����J�b�g
                    Select Case (Dir)
                        Case 1      ' +X �� 0��
                            Stp = 0
                        Case 2      ' -Y �� 270��
                            Stp = 3
                        Case 3      ' -X �� 180��
                            Stp = 2
                        Case 4      ' +Y �� 90��
                            Stp = 1
                    End Select
                Case Else
            End Select

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.GetStepDir() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0�A�� -----
#Region "�J�b�g�^�C�v��ϊ�����y���ʁz"
    '''=========================================================================
    '''<summary>�J�b�g�^�C�v��ϊ�����y���ʁz</summary>
    '''<param name="CutType">(INP)�J�b�g�`�� </param>
    '''<param name="CnvType">(OUT)�ϊ���J�b�g�`�� </param>
    '''=========================================================================
    Private Sub CnvCutType(ByRef CutType As String, ByRef CnvType As String)

        Dim strMSG As String

        Try
            ' ST�J�b�g��L�J�b�g�͎΂�ST�J�b�g, �΂�L�J�b�g�ɕϊ�����
            Select Case (CutType)
                Case CNS_CUTP_ST                                        ' ST�J�b�g �� �΂�ST�J�b�g
                    CnvType = CNS_CUTP_NST
                Case CNS_CUTP_STr                                       ' ST�J�b�g(���^�[��) �� �΂�ST�J�b�g(���^�[��)
                    CnvType = CNS_CUTP_NSTr
                Case CNS_CUTP_STt                                       ' ST�J�b�g(���g���[�X) �� �΂�ST�J�b�g(���g���[�X)
                    CnvType = CNS_CUTP_NSTt
                Case CNS_CUTP_L                                         ' L�J�b�g �� �΂�L�J�b�g
                    CnvType = CNS_CUTP_NL
                Case CNS_CUTP_Lr                                        ' L�J�b�g(���^�[��) �� �΂�L�J�b�g(���^�[��)
                    CnvType = CNS_CUTP_NLr
                Case CNS_CUTP_Lt                                        ' L�J�b�g(���g���[�X) �� �΂�L�J�b�g(���g���[�X)
                    CnvType = CNS_CUTP_NLt
                    '----- V1.18.0.0�C�� -----
                Case CNS_CUTP_ST2                                       ' �߼޼��ݸޖ���ST�J�b�g �� �΂�ST�J�b�g
                    CnvType = CNS_CUTP_NST
                Case CNS_CUTP_IX2                                       ' �߼޼��ݸޖ���IX�J�b�g �� IX�J�b�g
                    CnvType = CNS_CUTP_IX
                    '----- V1.18.0.0�C�� -----
                Case Else
                    CnvType = CutType
            End Select
            Console.WriteLine("File.CnvCutType() CutType(INP)=" + CutType + ", CutType(OUT)=" + CnvType)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.CnvCutType() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0�A�� -----
#Region "������->HEX ASCII�ϊ�"
    '''=========================================================================
    '''<summary>������->HEX ASCII�ϊ�</summary>
    '''<param name="sin">(INP) ������</param>
    '''<returns>HEX ASCII������</returns>
    '''=========================================================================
    Private Function Str2HexAsc(ByVal sin As String) As String

        Dim i As Integer

        Str2HexAsc = ""
        For i = 1 To Len(sin)
            Str2HexAsc = Str2HexAsc & Right("0" & Hex(Asc(Mid(sin, i, 1))), 2)
        Next

    End Function
#End Region

#Region "HEX ASCII->������ϊ�"
    '''=========================================================================
    '''<summary>HEX ASCII->������ϊ�</summary>
    '''<param name="sin">(INP) HEX ASCII������</param>
    '''<returns>������</returns>
    '''=========================================================================
    Private Function HexAsc2Str(ByVal sin As String) As String

        Dim i As Integer

        HexAsc2Str = ""
        For i = 1 To Len(sin) Step 2
            HexAsc2Str = HexAsc2Str & Chr(Val("&H" & Mid(sin, i, 2)))
        Next

    End Function
#End Region
#End Region

#Region "�v���[�g�f�[�^�̓��e��ϊ�����"
    '''=========================================================================
    '''<summary>�v���[�g�f�[�^�̓��e���E�㌴�_����E�����_�ɕϊ�����</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function ConvertPlateData() As Integer

        'Dim mDATA() As String
        Dim i As Integer
        Dim flg As Integer
        Dim LocaltypPlateInfo As PlateInfo                              ' ��ڰ��ް�
        Dim strMSG As String

        Try

            ConvertPlateData = 0
            flg = 0
            LocaltypPlateInfo = typPlateInfo

            With typPlateInfo
                '----- V4.0.0.0-35�� -----
                ' �`�b�v���ѕ��� = Y�����̏ꍇ
                If (typPlateInfo.intResistDir = 1) Then
                    ' �e�[�u���ʒu�I�t�Z�b�gX
                    LocaltypPlateInfo.dblTableOffsetXDir = 0.0
                    ' �e�[�u���ʒu�I�t�Z�b�gY
                    LocaltypPlateInfo.dblTableOffsetYDir = 0.0
                    ' BP�I�t�Z�b�gX
                    LocaltypPlateInfo.dblBpOffSetXDir = 0.0
                    ' BP�I�t�Z�b�gY
                    LocaltypPlateInfo.dblBpOffSetYDir = 0.0
                    ' �X�e�b�vX
                    LocaltypPlateInfo.dblStepOffsetXDir = 0.0
                    ' �X�e�b�vY
                    LocaltypPlateInfo.dblStepOffsetYDir = 0.0

                    ' Z ON/OFF�ʒu���Đݒ肷��
                    LocaltypPlateInfo.dblZOffSet = Z_ON_POS_SIMPLE                  ' ZON�ʒu
                    LocaltypPlateInfo.dblZStepUpDist = Z_STEP_POS_SIMPLE            ' ��۰�޽ï�ߏ㏸����
                    LocaltypPlateInfo.dblZWaitOffset = Z_OFF_POS_SIMPLE             ' ZOFF�ʒu

                    typPlateInfo = LocaltypPlateInfo

                    Return (cFRS_NORMAL)
                End If
                '----- V4.0.0.0-35�� -----

                ' �u���b�N��X
                LocaltypPlateInfo.intBlockCntXDir = .intBlockCntYDir                '��ۯ���X
                ' �u���b�N��Y
                LocaltypPlateInfo.intBlockCntYDir = .intBlockCntXDir                '��ۯ���Y
                ' �u���b�N��X
                LocaltypPlateInfo.intBlkCntInStgGrpX = .intBlkCntInStgGrpY          '�X�e�[�W�O���[�v����ۯ���X
                ' �u���b�N��Y
                LocaltypPlateInfo.intBlkCntInStgGrpY = .intBlkCntInStgGrpX          '�X�e�[�W�O���[�v����ۯ���Y

                '�X�e�b�v����
                Select Case (.intDirStepRepeat)
                    Case STEP_RPT_NON           ' �X�e�b�v�����s�[�g�����i�Ȃ��j

                    Case STEP_RPT_X             ' �X�e�b�v�����s�[�g�����iX�����j
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_Y
                    Case STEP_RPT_Y             ' �X�e�b�v�����s�[�g�����iY�����j
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_X
                    Case STEP_RPT_CHIPXSTPY     ' �X�e�b�v�����s�[�g�����iX�����`�b�v���X�e�b�v�{Y�����j
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_CHIPYSTPX
                    Case STEP_RPT_CHIPYSTPX     ' �X�e�b�v�����s�[�g�����iY�����`�b�v���X�e�b�v�{X�����j
                        LocaltypPlateInfo.intDirStepRepeat = STEP_RPT_CHIPXSTPY
                    Case Else
                End Select
                ' �v���[�g�T�C�YX
                LocaltypPlateInfo.dblPlateSizeX = .dblPlateSizeY
                ' �v���[�g�T�C�YY
                LocaltypPlateInfo.dblPlateSizeY = .dblPlateSizeX
                ' �v���[�g��X
                LocaltypPlateInfo.intPlateCntXDir = .intPlateCntYDir
                ' �v���[�g��Y
                LocaltypPlateInfo.intPlateCntYDir = .intPlateCntXDir
                ' �v���[�g�ԊuX
                LocaltypPlateInfo.dblPlateItvXDir = .dblPlateItvYDir
                ' �v���[�g�ԊuY
                LocaltypPlateInfo.dblPlateItvYDir = .dblPlateItvXDir
                ' �u���b�N�T�C�YX
                LocaltypPlateInfo.dblBlockSizeXDir = .dblBlockSizeYDir              '��ۯ���X
                ' �u���b�N�T�C�YY
                LocaltypPlateInfo.dblBlockSizeYDir = .dblBlockSizeXDir              '��ۯ���Y
                ' �`�b�v�T�C�YX
                LocaltypPlateInfo.dblChipSizeXDir = .dblChipSizeYDir                '�`�b�v��X
                ' �`�b�v�T�C�YY
                LocaltypPlateInfo.dblChipSizeYDir = .dblChipSizeXDir                '�`�b�v��Y

                ' �e�[�u���ʒu�I�t�Z�b�gX
                LocaltypPlateInfo.dblTableOffsetXDir = 0
                ' �e�[�u���ʒu�I�t�Z�b�gY
                LocaltypPlateInfo.dblTableOffsetYDir = .dblBlockSizeYDir * (-1)
                ' BP�I�t�Z�b�gX
                LocaltypPlateInfo.dblBpOffSetXDir = 0
                ' BP�I�t�Z�b�gY
                LocaltypPlateInfo.dblBpOffSetYDir = 0
                ' �X�e�b�vX
                LocaltypPlateInfo.dblStepOffsetXDir = 0
                ' �X�e�b�vY
                LocaltypPlateInfo.dblStepOffsetYDir = 0

                ' Z ON/OFF�ʒu���Đݒ肷��
                '----- V4.0.0.0-35�� -----
                LocaltypPlateInfo.dblZOffSet = Z_ON_POS_SIMPLE                      ' ZON�ʒu
                LocaltypPlateInfo.dblZStepUpDist = Z_STEP_POS_SIMPLE                ' ��۰�޽ï�ߏ㏸����
                LocaltypPlateInfo.dblZWaitOffset = Z_OFF_POS_SIMPLE                 ' ZOFF�ʒu
                '----- V4.0.0.0-35�� -----

                '�␳�ʒu���W1X
                LocaltypPlateInfo.dblReviseCordnt1XDir = .dblReviseCordnt1YDir
                '�␳�ʒu���W1Y
                LocaltypPlateInfo.dblReviseCordnt1YDir = .dblReviseCordnt1XDir
                '�␳�ʒu���W2X
                LocaltypPlateInfo.dblReviseCordnt2XDir = .dblReviseCordnt2YDir
                '�␳�ʒu���W2Y
                LocaltypPlateInfo.dblReviseCordnt2YDir = .dblReviseCordnt2XDir
                '�␳�߼޼�ݵ̾��X
                LocaltypPlateInfo.dblReviseOffsetXDir = .dblReviseOffsetYDir
                '�␳�߼޼�ݵ̾��Y
                LocaltypPlateInfo.dblReviseOffsetYDir = .dblReviseOffsetXDir
                For i = 1 To LocaltypPlateInfo.intResistCntInGroup
                    ConvertResistData(i)
                Next

            End With

            typPlateInfo = LocaltypPlateInfo

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            If flg = 1 Then Return (cFRS_NORMAL) ' �Ɖ�]�̑O�܂Őݒ�Ȃ琳������
            strMSG = "File.ConvertPlateData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "��R�f�[�^��ϊ�����"
    '''=========================================================================
    '''<summary>��R�f�[�^��ϊ�����</summary>
    '''<param name="rCnt"> (I/O)��R�f�[�^�\���̂̃C���f�b�N�X(1�ؼ��)</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Private Function ConvertResistData(ByRef rCnt As Integer) As Integer

        Dim i As Integer
        Dim strMSG As String
        Dim ResistorInfoLocal As ResistorInfo
        Dim tmpintCutDir As Integer
        Dim tempData As Double

        Try
            ResistorInfoLocal = typResistorInfoArray(rCnt)

            '----- V4.0.0.0-35�� -----
            ' �`�b�v���ѕ��� = Y�����̏ꍇ
            If (typPlateInfo.intResistDir = 1) Then
                Return (cFRS_NORMAL)
            End If
            '----- V4.0.0.0-35�� -----

            ' ���[�h������R�f�[�^���R�f�[�^�\���̂֊i�[����
            With typResistorInfoArray(rCnt)
                For i = 0 To .intCutCount
                    '----- V6.0.3.0�I�� -----
                    'X,Y�����ւ���,�p�x���X�O�x��]
                    '' �J�b�g�J�n�ʒuY
                    'ResistorInfoLocal.ArrCut(i).dblStartPointX = .ArrCut(i).dblStartPointY
                    '' �J�b�g�J�n�ʒuY
                    'ResistorInfoLocal.ArrCut(i).dblStartPointY = .ArrCut(i).dblStartPointX
                    tempData = .ArrCut(i).dblStartPointX
                    '' �J�b�g�J�n�ʒuY
                    ResistorInfoLocal.ArrCut(i).dblStartPointX = .ArrCut(i).dblStartPointY
                    '' �J�b�g�J�n�ʒuY
                    ResistorInfoLocal.ArrCut(i).dblStartPointY = tempData

                    ' �J�b�g�p�x
                    If ResistorInfoLocal.ArrCut(i).strCutType = CNS_CUTP_NST Or ResistorInfoLocal.ArrCut(i).strCutType = CNS_CUTP_NL Then
                        tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intCutAngle - 90
                        If (tmpintCutDir < 0) Then
                            tmpintCutDir = 360 + tmpintCutDir
                        End If
                        ResistorInfoLocal.ArrCut(i).intCutAngle = tmpintCutDir
                    End If
                    'tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intCutDir - 90
                    'If (tmpintCutDir < 0) Then
                    '    tmpintCutDir = 360 + tmpintCutDir
                    'End If
                    'ResistorInfoLocal.ArrCut(i).intCutDir = tmpintCutDir

                    If ResistorInfoLocal.ArrCut(i).strCutType = CNS_CUTP_SC Then
                        tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intCutAngle - 90
                        If (tmpintCutDir < 0) Then
                            tmpintCutDir = 360 + tmpintCutDir
                        End If
                        ResistorInfoLocal.ArrCut(i).intCutAngle = tmpintCutDir

                        tmpintCutDir = typResistorInfoArray(rCnt).ArrCut(i).intStepDir - 1
                        If (tmpintCutDir < 0) Then
                            tmpintCutDir = 3
                        End If
                        ResistorInfoLocal.ArrCut(i).intStepDir = tmpintCutDir

                    End If
                    'X,Y�����ւ���,�p�x���X�O�x��]
                    '----- V6.0.3.0�I�� -----
                Next

            End With

            typResistorInfoArray(rCnt) = ResistorInfoLocal

            Return (cFRS_NORMAL)                                                    ' Return�l = ����

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "File.Get_RESIST_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                                      ' Return�l = ��O�G���[
        End Try
    End Function
#End Region

#End Region

#Region "̧�ق̕������ނ�Shift_JIS����Unicode(UTF16LE BOM�L)�ɕϊ�����"
    '''=========================================================================
    ''' <summary>̧�ق̕������ނ�Shift_JIS����Unicode(UTF16LE BOM�L)�ɕϊ�����</summary>
    ''' <param name="fileFullPath">̧�����߽</param>
    ''' <remarks>'V4.4.0.0-1</remarks>
    '''=========================================================================
    Public Sub ConvertFileEncoding(ByVal fileFullPath As String)
        Try
            If (True = String.IsNullOrEmpty(fileFullPath)) Then
                Throw New ArgumentException()
            End If
            If (False = IO.File.Exists(fileFullPath)) Then
                Throw New FileNotFoundException()
            End If

            ' tky.ini���B��̧�قɂȂ��Ă������߉����i�������ݎ��ɗ�O����������j
            IO.File.SetAttributes(fileFullPath, FileAttributes.Normal)

            Dim src As String
            Using fs As New FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                If (True = fs.IsUnicode()) Then Exit Sub ' Unicode(UTF16LE BOM�L)�ɕϊ��ς݂Ȃ�NOP

                Using sr As New StreamReader(fs, Encoding.GetEncoding("Shift_JIS"))
                    src = sr.ReadToEnd()    ' Shift_JIS �Ƃ��ēǂݍ���
                End Using
            End Using

            Using sw As New StreamWriter(fileFullPath, False, Encoding.Unicode)
                sw.Write(src)               ' Unicode(UTF16LE BOM�L)�Ƃ��ď���������
            End Using

        Catch ex As Exception
            Dim strMSG As String = "File.ConvertFileEncoding() TRAP ERROR = " + ex.Message
            MessageBox.Show(strMSG & vbCrLf & fileFullPath)
        End Try

    End Sub
#End Region

#Region "FileStream�׽�g��ҿ���"
    '''=========================================================================
    '''<summary>fileStream�̐擪�޲Ă�UTF8��BOM���ǂ�����Ԃ�</summary>
    '''<param name="fileStream">FileStream�׽</param>
    '''<returns>True=UTF8, False=UTF8�ł͂Ȃ�</returns>
    ''' <remarks>V4.4.0.0-1</remarks>
    '''=========================================================================
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsUTF8(ByVal fileStream As System.IO.FileStream) As Boolean

        If (fileStream IsNot Nothing) AndAlso (3 <= fileStream.Length) Then
            Dim BOM As Byte() = {&HEF, &HBB, &HBF}
            Dim src As Byte() = New Byte(BOM.Length) {}

            fileStream.Read(src, 0, src.Length)
            fileStream.Position = 0L

            Return (BOM(0) = src(0)) AndAlso (BOM(1) = src(1)) AndAlso (BOM(2) = src(2))
        End If

        Return False

    End Function

    '''=========================================================================
    '''<summary>fileStream�̐擪�޲Ă�Unicode(UTF16LE)��BOM���ǂ�����Ԃ�</summary>
    '''<param name="fileStream">FileStream�׽</param>
    '''<returns>True=Unicode, False=Unicode�ł͂Ȃ�</returns>
    ''' <remarks>V4.4.0.0-1</remarks>
    '''=========================================================================
    <System.Runtime.CompilerServices.Extension()>
    Public Function IsUnicode(ByVal fileStream As System.IO.FileStream) As Boolean

        If (fileStream IsNot Nothing) AndAlso (2 <= fileStream.Length) Then
            Dim BOM As Byte() = {&HFF, &HFE}
            Dim src As Byte() = New Byte(BOM.Length) {}

            fileStream.Read(src, 0, src.Length)
            fileStream.Position = 0L

            Return (BOM(0) = src(0)) AndAlso (BOM(1) = src(1))
        End If

        Return False

    End Function
#End Region

#Region "�g���~���O�f�[�^�ǂݍ��݌�A�����炠��ϐ��Ɋi�[���鏈��"
    ''' <summary>
    ''' �ǂݍ��񂾃g���~���O�f�[�^���猳�̕ϐ��Ɋi�[���鏈��
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>'V5.0.0.8�B��</remarks>
    Private Function SetToOrgData() As Integer

        gRegistorCnt = typPlateInfo.intResistCntInBlock

    End Function


#End Region

#Region "�g�����f�[�^�t�@�C���̊g���q���擾����"
    Private _extension As String = Nothing
    ''' <summary>�g�����f�[�^�t�@�C���̊g���q���擾����</summary>
    ''' <value>ReadOnly</value>
    ''' <returns>�g�����f�[�^�t�@�C���̊g���q</returns>
    ''' <remarks>'V5.0.0.9�N</remarks>
    Public ReadOnly Property Extension As String
        Get
            ' �g���q�ݒ�
            If (_extension Is Nothing) Then
                Select Case (gTkyKnd)
                    Case KND_TKY                        ' TKY�̏ꍇ
                        _extension = ".tdt"
                    Case KND_CHIP                       ' CHIP�̏ꍇ
                        _extension = ".tdc"
                    Case KND_NET                        ' NET�̏ꍇ
                        _extension = ".tdn"
                    Case Else
                        _extension = String.Empty
                End Select

                If (String.Empty <> _extension) AndAlso
                    (giMachineKd = MACHINE_KD_RS) Then  ' SL436S ?

                    _extension &= "s"
                End If
            End If

            Return _extension
        End Get
    End Property
#End Region

End Module